const test = require('node:test');
const assert = require('node:assert/strict');
const {
  createState,
  setLampStatus,
  createTeam,
  updateTeam,
  clearLamps,
} = require('../src/store');

test('cria dez lâmpadas desligadas e uma equipe padrão autorizada', () => {
  const state = createState();
  assert.equal(state.lamps.size, 10);
  assert.equal(state.activeTeamCode, 0);
  assert.equal(state.teams.get(0).autorizada, true);
  assert.equal(state.lamps.get(1).status, false);
});

test('altera uma lâmpada apenas pela equipe autorizada', () => {
  const state = createState();
  createTeam(state, { codEquipe: 1, nome: 'Equipe 1' });
  const result = setLampStatus(state, 3, 0, true);
  assert.equal(result.lampada, 3);
  assert.equal(state.lamps.get(3).status, true);
  assert.throws(() => setLampStatus(state, 3, 1, false), /não está autorizada/);
});

test('trocar a equipe autorizada desliga todas as lâmpadas', () => {
  const state = createState();
  createTeam(state, { codEquipe: 7, nome: 'Equipe 7' });
  setLampStatus(state, 1, 0, true);
  updateTeam(state, 7, { autorizada: true });
  assert.equal(state.activeTeamCode, 7);
  assert.equal(state.lamps.get(1).status, false);
  assert.equal(state.teams.get(0).autorizada, false);
  assert.equal(state.teams.get(7).autorizada, true);
});

test('limpar lâmpadas preserva a lista de equipamentos', () => {
  const state = createState();
  setLampStatus(state, 1, 0, true);
  clearLamps(state);
  assert.equal(state.lamps.get(1).status, false);
  assert.equal(state.teams.size, 1);
});
