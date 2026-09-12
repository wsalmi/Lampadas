(() => {
  const lampGrid = document.querySelector('#lamp-grid');
  const lampTemplate = document.querySelector('#lamp-template');
  const historyList = document.querySelector('#history-list');
  const historyCount = document.querySelector('#history-count');
  const onCount = document.querySelector('#on-count');
  const eventCount = document.querySelector('#event-count');
  const teamTitle = document.querySelector('#team-title');
  const teamSubtitle = document.querySelector('#team-subtitle');
  const connectionDot = document.querySelector('#connection-dot');
  const connectionLabel = document.querySelector('#connection-label');
  let interactionCount = 0;
  const lamps = new Map();

  function setConnection(state, label) {
    connectionDot.className = `status-dot ${state}`;
    connectionLabel.textContent = label;
  }

  function lampCard(lamp) {
    let card = document.querySelector(`[data-lamp-card="${lamp.idLampada}"]`);
    if (!card) {
      card = lampTemplate.content.firstElementChild.cloneNode(true);
      card.dataset.lampCard = lamp.idLampada;
      card.querySelector('[data-lamp-id]').textContent = lamp.idLampada;
      lampGrid.append(card);
    }
    card.classList.toggle('is-on', lamp.status);
    card.querySelector('[data-lamp-state]').textContent = lamp.status ? 'Acesa' : 'Apagada';
    return card;
  }

  function updateMetrics() {
    onCount.textContent = [...lamps.values()].filter((lamp) => lamp.status).length;
    eventCount.textContent = interactionCount;
    historyCount.textContent = interactionCount;
  }

  function renderSnapshot(snapshot) {
    for (const lamp of snapshot.lampadas ?? []) {
      lamps.set(lamp.idLampada, lamp);
      lampCard(lamp);
    }
    const active = (snapshot.equipes ?? []).find((team) => team.autorizada);
    teamTitle.textContent = active?.nome ?? 'Nenhuma equipe autorizada';
    teamSubtitle.textContent = active ? `Código ${active.codEquipe} · painel pronto para apresentação` : 'Aguardando autorização';
    updateMetrics();
  }

  function addHistory(event) {
    if (historyList.querySelector('.history-empty')) historyList.replaceChildren();
    interactionCount += 1;
    const item = document.createElement('div');
    item.className = 'history-item';
    const action = event.type === 'acender' ? 'Acendeu' : 'Apagou';
    item.innerHTML = `<span class="history-number">#${interactionCount}</span><span><strong>Lâmpada ${event.lampada}</strong><small>Intervalo: ${event.ultimaAlteracaoLampada ?? 0} ms</small></span><span class="history-action">${action}</span>`;
    historyList.prepend(item);
    while (historyList.children.length > 100) historyList.lastElementChild.remove();
    updateMetrics();
  }

  function applyEvent(event) {
    if (event.type === 'snapshot') {
      renderSnapshot(event);
      return;
    }
    if (event.type === 'acender' || event.type === 'apagar') {
      const lamp = lamps.get(event.lampada);
      if (lamp) {
        lamp.status = event.status;
        lampCard(lamp);
      }
      addHistory(event);
      return;
    }
    if (event.type === 'reiniciar') {
      renderSnapshot(event.snapshot);
      historyList.replaceChildren();
      interactionCount = 0;
      const empty = document.createElement('p');
      empty.className = 'history-empty';
      empty.textContent = 'Nenhuma interação registrada nesta rodada.';
      historyList.append(empty);
      updateMetrics();
      return;
    }
    if (event.type === 'equipeAtualizada' || event.type === 'equipesAtualizadas') renderSnapshot(event.snapshot);
  }

  function connect() {
    setConnection('', 'Conectando');
    const source = new EventSource('/api/events');
    source.addEventListener('snapshot', (message) => {
      setConnection('connected', 'Conectado');
      applyEvent({ type: 'snapshot', ...JSON.parse(message.data) });
    });
    source.addEventListener('update', (message) => {
      setConnection('connected', 'Conectado');
      applyEvent(JSON.parse(message.data));
    });
    source.onerror = () => setConnection('error', 'Reconectando');
  }

  historyList.innerHTML = '<p class="history-empty">Nenhuma interação registrada nesta rodada.</p>';
  fetch('/api/snapshot').then((response) => response.json()).then(renderSnapshot).catch(() => setConnection('error', 'API indisponível'));
  connect();
})();
