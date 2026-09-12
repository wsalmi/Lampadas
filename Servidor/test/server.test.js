const test = require('node:test');
const assert = require('node:assert/strict');
const { createServer } = require('../src/index');

async function withServer(callback) {
  const server = createServer({ logger: false });
  await new Promise((resolve) => server.listen(0, '127.0.0.1', resolve));
  const address = server.address();
  try {
    await callback(`http://127.0.0.1:${address.port}`);
  } finally {
    await new Promise((resolve, reject) => server.close((error) => (error ? reject(error) : resolve())));
  }
}

async function json(url, options) {
  const response = await fetch(url, options);
  return { response, body: await response.json() };
}

test('expõe health, snapshot e estado de lâmpadas', async () => {
  await withServer(async (baseUrl) => {
    const health = await json(`${baseUrl}/health`);
    assert.equal(health.response.status, 200);
    assert.equal(health.body.status, 'ok');

    const snapshot = await json(`${baseUrl}/api/snapshot`);
    assert.equal(snapshot.response.status, 200);
    assert.equal(snapshot.body.lampadas.length, 10);
  });
});

test('mantém o contrato de atualização de lâmpada', async () => {
  await withServer(async (baseUrl) => {
    const update = await json(`${baseUrl}/api/lampada/2/status`, {
      method: 'PUT',
      headers: { 'content-type': 'application/json' },
      body: JSON.stringify({ idEquipe: 0, status: true }),
    });
    assert.equal(update.response.status, 200);
    assert.deepEqual(update.body.lampadaData.status, true);

    const status = await json(`${baseUrl}/api/lampada/status`);
    assert.equal(status.body.find((lamp) => lamp.idLampada === 2).status, true);
  });
});

test('valida equipe inexistente e lâmpada inexistente', async () => {
  await withServer(async (baseUrl) => {
    const team = await json(`${baseUrl}/api/lampada/1/status`, {
      method: 'PUT',
      headers: { 'content-type': 'application/json' },
      body: JSON.stringify({ idEquipe: 99, status: true }),
    });
    assert.equal(team.response.status, 404);
    assert.equal(team.body.error.code, 'TEAM_NOT_FOUND');

    const lamp = await json(`${baseUrl}/api/lampada/99/status`, {
      method: 'PUT',
      headers: { 'content-type': 'application/json' },
      body: JSON.stringify({ idEquipe: 0, status: true }),
    });
    assert.equal(lamp.response.status, 404);
    assert.equal(lamp.body.error.code, 'LAMP_NOT_FOUND');
  });
});
