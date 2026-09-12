import test from 'node:test';
import assert from 'node:assert/strict';
import { loadConfig } from '../src/config';
import { parseArgs } from '../src/index';
import { runOperations } from '../src/runner';
import { task4, task5, task6, task8 } from '../src/tasks';

test('valida configuração e usa valores locais previsíveis', () => {
  const config = loadConfig({ LAMPADAS_API_URL: 'http://example.test/api', COD_EQUIPE: '7' });
  assert.equal(config.apiUrl, 'http://example.test/api');
  assert.equal(config.teamCode, 7);
  assert.equal(config.lampCount, 10);
  assert.throws(() => loadConfig({ LAMPADAS_API_URL: 'ftp://example.test' }), /http/);
});

test('interpreta tarefa, repetição e dry-run', () => {
  assert.deepEqual(parseArgs(['--task', 'all', '--repeat', '--dry-run']), { task: 'all', repeat: true, dryRun: true });
  assert.deepEqual(parseArgs(['--task=3']), { task: 3, repeat: false, dryRun: false });
  assert.throws(() => parseArgs(['--task', '9']), /entre 1 e 8/);
});

test('gera uma ordem aleatória determinística e sem duplicatas', () => {
  const plan = task4(4, () => 0);
  assert.deepEqual(plan.filter((operation) => operation.status).map((operation) => operation.idLampada), [2, 3, 4, 1]);
  assert.equal(plan.length, 8);
});

test('gera Fibonacci e números primos dentro do limite', () => {
  assert.deepEqual(task5(10).map((operation) => operation.idLampada), [1, 1, 2, 3, 5, 8]);
  assert.deepEqual(task6(10).map((operation) => operation.idLampada), [2, 3, 5, 7]);
});

test('separa união e interseção sem mutar os vetores de entrada', () => {
  const plan = task8(10);
  const firstStage = plan.slice(0, 7).filter((operation) => operation.status).map((operation) => operation.idLampada);
  const lastStage = plan.slice(-3).map((operation) => operation.idLampada);
  assert.deepEqual(firstStage, [2, 3, 8, 5, 6, 1, 10]);
  assert.deepEqual(lastStage, [5, 8, 3]);
});

test('executa operações em sequência e aguarda entre elas', async () => {
  const calls: string[] = [];
  const client = { updateLamp: async (id: number, status: boolean) => { calls.push(`${id}:${status}`); } };
  await runOperations([{ idLampada: 1, status: true, delayMs: 10 }, { idLampada: 2, status: false }], client, async (delay) => { calls.push(`sleep:${delay}`); });
  assert.deepEqual(calls, ['1:true', 'sleep:10', '2:false']);
});
