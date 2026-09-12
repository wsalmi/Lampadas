import axios, { AxiosInstance } from 'axios';
import type { ClientConfig } from './config';

export interface LampUpdateResponse {
  lampada: number;
  status: string;
}

export interface LampClient {
  updateLamp(idLampada: number, status: boolean): Promise<LampUpdateResponse | undefined>;
  clearLamps(): Promise<void>;
}

export class LampadasApiClient implements LampClient {
  private readonly http: AxiosInstance;
  private readonly config: ClientConfig;
  private readonly log: (message: string) => void;

  public constructor(config: ClientConfig, log: (message: string) => void = console.log) {
    this.config = config;
    this.log = log;
    this.http = axios.create({
      baseURL: config.apiUrl,
      timeout: config.requestTimeoutMs,
      headers: { 'Content-Type': 'application/json' },
    });
  }

  public async updateLamp(idLampada: number, status: boolean): Promise<LampUpdateResponse | undefined> {
    if (!Number.isInteger(idLampada) || idLampada < 1 || idLampada > this.config.lampCount) {
      throw new Error(`idLampada deve estar entre 1 e ${this.config.lampCount}`);
    }
    if (this.config.dryRun) {
      this.log(`[dry-run] lâmpada ${idLampada}: ${status ? 'acesa' : 'apagada'}`);
      return undefined;
    }
    try {
      const response = await this.http.put<LampUpdateResponse>(`/lampada/${idLampada}/status`, {
        idEquipe: this.config.teamCode,
        status,
      });
      this.log(`lâmpada ${idLampada}: ${response.data.status}`);
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error)) {
        const serverMessage = typeof error.response?.data?.error?.message === 'string'
          ? error.response.data.error.message
          : error.message;
        throw new Error(`Falha ao atualizar a lâmpada ${idLampada}: ${serverMessage}`);
      }
      throw error;
    }
  }

  public async clearLamps(): Promise<void> {
    if (this.config.dryRun) {
      this.log('[dry-run] todas as lâmpadas: apagadas');
      return;
    }
    await this.http.put('/lampada/status/limpar');
  }
}
