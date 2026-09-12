import { loadConfig } from './config';
import { LampadasApiClient } from './client';
import { runOperations, sleep } from './runner';
import { TASKS } from './tasks';

interface CliOptions {
  task: number | 'all';
  repeat: boolean;
  dryRun: boolean;
}

export function parseArgs(args: readonly string[]): CliOptions {
  let task: number | 'all' = 1;
  let repeat = false;
  let dryRun = false;
  for (let index = 0; index < args.length; index += 1) {
    const argument = args[index];
    if (!argument) continue;
    if (argument === '--repeat') {
      repeat = true;
    } else if (argument === '--dry-run') {
      dryRun = true;
    } else if (argument === '--task') {
      const value = args[index + 1];
      if (!value) throw new Error('--task exige um valor entre 1 e 8 ou all');
      index += 1;
      task = value === 'all' ? 'all' : Number(value);
    } else if (argument.startsWith('--task=')) {
      const value = argument.slice('--task='.length);
      task = value === 'all' ? 'all' : Number(value);
    } else if (argument === '--help' || argument === '-h') {
      throw new Error('Uso: npm run once -- --task 1|all [--repeat] [--dry-run]');
    } else {
      throw new Error(`Argumento desconhecido: ${argument}`);
    }
  }
  if (task !== 'all' && (!Number.isInteger(task) || !(task in TASKS))) {
    throw new Error('--task deve ser um número entre 1 e 8 ou all');
  }
  return { task, repeat, dryRun };
}

function selectedTasks(task: number | 'all'): number[] {
  return task === 'all' ? Object.keys(TASKS).map(Number) : [task];
}

export async function main(args: readonly string[] = process.argv.slice(2)): Promise<void> {
  const options = parseArgs(args);
  const config = loadConfig({ ...process.env, ...(options.dryRun ? { DRY_RUN: 'true' } : {}) });
  const client = new LampadasApiClient(config);
  const taskIds = selectedTasks(options.task);

  do {
    for (const taskId of taskIds) {
      console.log(`\nIniciando tarefa ${taskId}`);
      const taskPlan = TASKS[taskId];
      if (!taskPlan) throw new Error(`Tarefa ${taskId} não encontrada`);
      await runOperations(taskPlan(config.lampCount), client);
    }
    if (options.repeat) await sleep(1_000);
  } while (options.repeat);
}

if (require.main === module) {
  main().catch((error: unknown) => {
    console.error(error instanceof Error ? error.message : error);
    process.exitCode = 1;
  });
}
