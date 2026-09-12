export interface ClientConfig {
  apiUrl: string;
  teamCode: number;
  lampCount: number;
  requestTimeoutMs: number;
  dryRun: boolean;
}

function integerFromEnv(value: string | undefined, fallback: number, name: string, min: number, max: number): number {
  if (value === undefined || value.trim() === '') return fallback;
  const parsed = Number(value);
  if (!Number.isInteger(parsed) || parsed < min || parsed > max) {
    throw new Error(`${name} deve ser um inteiro entre ${min} e ${max}`);
  }
  return parsed;
}

function booleanFromEnv(value: string | undefined, fallback: boolean): boolean {
  if (value === undefined) return fallback;
  return ['1', 'true', 'yes', 'sim'].includes(value.toLowerCase());
}

export function loadConfig(env: NodeJS.ProcessEnv = process.env): ClientConfig {
  const apiUrl = (env.LAMPADAS_API_URL ?? 'http://localhost:3000/api').trim().replace(/\/+$/, '');
  if (!/^https?:\/\//i.test(apiUrl)) {
    throw new Error('LAMPADAS_API_URL deve começar com http:// ou https://');
  }
  return {
    apiUrl,
    teamCode: integerFromEnv(env.COD_EQUIPE, 0, 'COD_EQUIPE', 0, 255),
    lampCount: integerFromEnv(env.LAMP_COUNT, 10, 'LAMP_COUNT', 1, 255),
    requestTimeoutMs: integerFromEnv(env.REQUEST_TIMEOUT_MS, 5_000, 'REQUEST_TIMEOUT_MS', 100, 120_000),
    dryRun: booleanFromEnv(env.DRY_RUN, false),
  };
}
