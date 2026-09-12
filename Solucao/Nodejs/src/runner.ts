import type { LampClient } from './client';
import type { LampOperation } from './tasks';

export const sleep = (delayMs: number): Promise<void> => new Promise((resolve) => setTimeout(resolve, delayMs));

export async function runOperations(
  operations: readonly LampOperation[],
  client: LampClient,
  wait: (delayMs: number) => Promise<void> = sleep,
): Promise<void> {
  for (const operation of operations) {
    if (operation.idLampada < 1) continue;
    await client.updateLamp(operation.idLampada, operation.status);
    if (operation.delayMs && operation.delayMs > 0) await wait(operation.delayMs);
  }
}
