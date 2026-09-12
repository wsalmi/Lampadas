const DEFAULT_LAMP_COUNT = 10;
const DEFAULT_TEAM_CODE = 0;
const DEFAULT_TEAM_NAME = 'squad37';

function toByte(value, fieldName) {
  const number = Number(value);
  if (!Number.isInteger(number) || number < 0 || number > 255) {
    throw new Error(`${fieldName} deve ser um inteiro entre 0 e 255`);
  }
  return number;
}

function toLampId(value) {
  const id = toByte(value, 'idLampada');
  if (id === 0) {
    throw new Error('idLampada deve ser maior que zero');
  }
  return id;
}

function now() {
  return new Date();
}

function createState(options = {}) {
  const lampCount = Number(options.lampCount ?? process.env.LAMP_COUNT ?? DEFAULT_LAMP_COUNT);
  if (!Number.isInteger(lampCount) || lampCount < 1 || lampCount > 255) {
    throw new Error('LAMP_COUNT deve ser um inteiro entre 1 e 255');
  }

  const defaultTeamCode = toByte(
    options.defaultTeamCode ?? process.env.DEFAULT_TEAM_CODE ?? DEFAULT_TEAM_CODE,
    'DEFAULT_TEAM_CODE',
  );
  const lamps = new Map();
  for (let id = 1; id <= lampCount; id += 1) {
    lamps.set(id, { idLampada: id, status: false, ultimaAlteracao: null });
  }

  const state = {
    lamps,
    teams: new Map([[defaultTeamCode, {
      codEquipe: defaultTeamCode,
      nome: options.defaultTeamName ?? process.env.DEFAULT_TEAM_NAME ?? DEFAULT_TEAM_NAME,
      autorizada: true,
    }]]),
    activeTeamCode: defaultTeamCode,
    historyVisible: false,
    lastInteractionAt: null,
    eventSequence: 0,
    subscribers: new Set(),
  };

  return state;
}

function snapshot(state) {
  return {
    lampadas: listLamps(state),
    equipes: listTeams(state),
    equipeAutorizada: state.activeTeamCode,
    historicoVisivel: state.historyVisible,
  };
}

function listLamps(state) {
  return [...state.lamps.values()]
    .sort((left, right) => left.idLampada - right.idLampada)
    .map((lamp) => ({ ...lamp }));
}

function listTeams(state) {
  return [...state.teams.values()]
    .sort((left, right) => left.codEquipe - right.codEquipe)
    .map((team) => ({ ...team }));
}

function emit(state, event) {
  const payload = JSON.stringify({
    id: ++state.eventSequence,
    occurredAt: now().toISOString(),
    ...event,
  });
  for (const subscriber of state.subscribers) {
    subscriber(payload);
  }
}

function subscribe(state, subscriber) {
  state.subscribers.add(subscriber);
  return () => state.subscribers.delete(subscriber);
}

function clearLamps(state) {
  const clearedAt = now();
  for (const lamp of state.lamps.values()) {
    lamp.status = false;
    lamp.ultimaAlteracao = clearedAt.toISOString();
  }
  state.lastInteractionAt = null;
  emit(state, { type: 'reiniciar', snapshot: snapshot(state) });
  return listLamps(state);
}

function setLampStatus(state, lampId, teamCode, status) {
  const id = toLampId(lampId);
  const code = toByte(teamCode, 'idEquipe');
  if (typeof status !== 'boolean') {
    throw new Error('status deve ser booleano');
  }

  const lamp = state.lamps.get(id);
  if (!lamp) {
    const error = new Error('Essa lampada não existe');
    error.code = 'LAMP_NOT_FOUND';
    throw error;
  }
  if (!state.teams.has(code)) {
    const error = new Error('CodEquipe não cadastrado');
    error.code = 'TEAM_NOT_FOUND';
    throw error;
  }
  if (state.activeTeamCode !== code) {
    const error = new Error('A equipe não está autorizada para controlar as lampadas');
    error.code = 'TEAM_NOT_AUTHORIZED';
    throw error;
  }

  const changedAt = now();
  const previousChange = lamp.ultimaAlteracao ? Date.parse(lamp.ultimaAlteracao) : changedAt.getTime();
  const previousInteraction = state.lastInteractionAt ? Date.parse(state.lastInteractionAt) : changedAt.getTime();
  const elapsedLampMs = Math.max(0, Math.floor((changedAt.getTime() - previousChange) / 100) * 100);
  const elapsedInteractionMs = Math.max(0, Math.floor((changedAt.getTime() - previousInteraction) / 100) * 100);

  lamp.status = status;
  lamp.ultimaAlteracao = changedAt.toISOString();
  state.lastInteractionAt = changedAt.toISOString();
  emit(state, {
    type: status ? 'acender' : 'apagar',
    lampada: id,
    status,
    ultimaAlteracaoLampada: elapsedLampMs,
    ultimaInteracao: elapsedInteractionMs,
  });

  return {
    lampada: id,
    status: status ? 'Acesa' : 'Apagada',
    lampadaData: { ...lamp },
  };
}

function createTeam(state, input) {
  const codEquipe = toByte(input?.codEquipe, 'codEquipe');
  const nome = String(input?.nome ?? '').trim();
  if (codEquipe === 0) {
    throw new Error('codEquipe deve ser maior que zero para novas equipes');
  }
  if (!nome || nome.length > 80) {
    throw new Error('nome deve ter entre 1 e 80 caracteres');
  }
  if (state.teams.has(codEquipe)) {
    const error = new Error('CodEquipe já cadastrado');
    error.code = 'TEAM_EXISTS';
    throw error;
  }
  const team = { codEquipe, nome, autorizada: false };
  state.teams.set(codEquipe, team);
  emit(state, { type: 'equipesAtualizadas', snapshot: snapshot(state) });
  return { ...team };
}

function updateTeam(state, teamCode, input) {
  const codEquipe = toByte(teamCode, 'codEquipe');
  const team = state.teams.get(codEquipe);
  if (!team) {
    const error = new Error('CodEquipe não cadastrado');
    error.code = 'TEAM_NOT_FOUND';
    throw error;
  }
  const nome = input?.nome === undefined ? team.nome : String(input.nome).trim();
  if (!nome || nome.length > 80) {
    throw new Error('nome deve ter entre 1 e 80 caracteres');
  }
  if (input?.autorizada !== undefined && typeof input.autorizada !== 'boolean') {
    throw new Error('autorizada deve ser booleano');
  }

  const shouldAuthorize = input?.autorizada === true;
  const shouldRevoke = input?.autorizada === false;
  if (shouldAuthorize && state.activeTeamCode !== codEquipe) {
    clearLamps(state);
    for (const currentTeam of state.teams.values()) {
      currentTeam.autorizada = currentTeam.codEquipe === codEquipe;
    }
    state.activeTeamCode = codEquipe;
  } else if (shouldRevoke && state.activeTeamCode === codEquipe) {
    clearLamps(state);
    team.autorizada = false;
    state.activeTeamCode = null;
  }

  team.nome = nome;
  if (state.activeTeamCode === codEquipe) {
    team.autorizada = true;
  }
  emit(state, { type: 'equipeAtualizada', snapshot: snapshot(state) });
  return { ...team };
}

function removeTeam(state, teamCode) {
  const codEquipe = toByte(teamCode, 'codEquipe');
  if (!state.teams.has(codEquipe)) {
    const error = new Error('CodEquipe não cadastrado');
    error.code = 'TEAM_NOT_FOUND';
    throw error;
  }
  if (state.activeTeamCode === codEquipe) {
    clearLamps(state);
    state.activeTeamCode = null;
  }
  const [team] = [state.teams.get(codEquipe)];
  state.teams.delete(codEquipe);
  emit(state, { type: 'equipesAtualizadas', snapshot: snapshot(state) });
  return { ...team };
}

function setHistoryVisible(state, visible) {
  state.historyVisible = visible;
  emit(state, { type: 'verHistorico', exibir: visible });
  return visible;
}

module.exports = {
  createState,
  snapshot,
  listLamps,
  listTeams,
  subscribe,
  clearLamps,
  setLampStatus,
  createTeam,
  updateTeam,
  removeTeam,
  setHistoryVisible,
};
