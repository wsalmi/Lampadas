export interface LampOperation {
  idLampada: number;
  status: boolean;
  delayMs?: number;
}

export type TaskPlan = (lampCount: number, random?: () => number) => LampOperation[];

function set(idLampada: number, status: boolean, delayMs = 0): LampOperation {
  return { idLampada, status, delayMs };
}

function allLamps(lampCount: number, status: boolean, delayMs = 0): LampOperation[] {
  return Array.from({ length: lampCount }, (_, index) => set(index + 1, status, delayMs));
}

export const task1: TaskPlan = (lampCount) => allLamps(lampCount, true, 500).flatMap((operation) => [operation, set(operation.idLampada, false, 500)]);

export const task2: TaskPlan = (lampCount) => {
  const evenOperations = Array.from({ length: lampCount }, (_, index) => index + 1)
    .filter((id) => id % 2 === 0)
    .map((id) => set(id, true));
  if (evenOperations.length > 0) evenOperations.at(-1)!.delayMs = 3_000;
  return [...evenOperations, ...allLamps(lampCount, true, 1_000)];
};

export const task3: TaskPlan = (lampCount) => {
  const operations: LampOperation[] = [];
  for (let left = 1; left <= Math.ceil(lampCount / 2); left += 1) {
    const right = lampCount + 1 - left;
    operations.push(set(left, true), ...(right === left ? [] : [set(right, true)]), set(left, false, 500));
    if (right !== left) operations.push(set(right, false, 500));
  }
  return operations;
};

export const task4: TaskPlan = (lampCount, random = Math.random) => {
  const order = Array.from({ length: lampCount }, (_, index) => index + 1);
  for (let index = order.length - 1; index > 0; index -= 1) {
    const swapIndex = Math.floor(random() * (index + 1));
    const current = order[index]!;
    order[index] = order[swapIndex]!;
    order[swapIndex] = current;
  }
  return order.flatMap((id) => [set(id, true, 1_000), set(id, false)]);
};

export const task5: TaskPlan = (lampCount) => {
  const sequence: number[] = [];
  let previous = 0;
  let current = 1;
  while (current <= lampCount) {
    sequence.push(current);
    [previous, current] = [current, previous + current];
  }
  return sequence.map((id) => set(id, true, 500));
};

function isPrime(value: number): boolean {
  if (value < 2) return false;
  for (let divisor = 2; divisor * divisor <= value; divisor += 1) {
    if (value % divisor === 0) return false;
  }
  return true;
}

export const task6: TaskPlan = (lampCount) => Array.from({ length: lampCount }, (_, index) => index + 1)
  .filter(isPrime)
  .map((id) => set(id, true, 500));

export const task7: TaskPlan = (lampCount) => {
  const groups = [[10, 2, 7, 3, 9, 4, 6, 1], [5, 2, 9, 4, 7], [1, 8]];
  return groups.flatMap((group) => {
    const operations = [...group].filter((id) => id <= lampCount).sort((left, right) => right - left)
      .flatMap((id) => [set(id, true, 500), set(id, false, 500)]);
    operations.at(-1)!.delayMs = 3_000;
    return operations;
  });
};

export const task8: TaskPlan = (lampCount) => {
  const first = [1, 5, 10, 8, 3].filter((id) => id <= lampCount);
  const second = [2, 3, 8, 5, 6].filter((id) => id <= lampCount);
  const union = [...new Set([...second, ...first])];
  const intersection = first.filter((id) => second.includes(id));
  const unionOperations = union.map((id) => set(id, true));
  unionOperations.at(-1)!.delayMs = 3_000;
  return [
    ...unionOperations,
    ...allLamps(lampCount, false),
    ...intersection.map((id) => set(id, true)),
  ];
};

export const TASKS: Record<number, TaskPlan> = {
  1: task1,
  2: task2,
  3: task3,
  4: task4,
  5: task5,
  6: task6,
  7: task7,
  8: task8,
};
