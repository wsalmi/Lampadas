const http = require('node:http');
const fs = require('node:fs');
const path = require('node:path');
const { URL } = require('node:url');
const {
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
} = require('./store');

const PUBLIC_DIR = path.join(__dirname, 'public');
const CONTENT_TYPES = {
  '.css': 'text/css; charset=utf-8',
  '.html': 'text/html; charset=utf-8',
  '.js': 'text/javascript; charset=utf-8',
  '.json': 'application/json; charset=utf-8',
  '.svg': 'image/svg+xml',
};

function sendJson(response, status, payload) {
  const body = JSON.stringify(payload);
  response.writeHead(status, {
    'Content-Type': 'application/json; charset=utf-8',
    'Cache-Control': 'no-store',
  });
  response.end(body);
}

function sendError(response, status, message, code = 'BAD_REQUEST') {
  sendJson(response, status, { error: { code, message } });
}

function parseJsonBody(request) {
  return new Promise((resolve, reject) => {
    let body = '';
    request.on('data', (chunk) => {
      body += chunk;
      if (body.length > 1_000_000) {
        reject(Object.assign(new Error('Corpo da requisição excede 1 MB'), { code: 'PAYLOAD_TOO_LARGE' }));
        request.destroy();
      }
    });
    request.on('end', () => {
      if (!body.trim()) {
        resolve({});
        return;
      }
      try {
        resolve(JSON.parse(body));
      } catch {
        reject(Object.assign(new Error('JSON inválido'), { code: 'INVALID_JSON' }));
      }
    });
    request.on('error', reject);
  });
}

function routeError(error) {
  switch (error.code) {
    case 'LAMP_NOT_FOUND':
    case 'TEAM_NOT_FOUND':
      return [404, error.code];
    case 'TEAM_NOT_AUTHORIZED':
      return [403, error.code];
    case 'TEAM_EXISTS':
      return [409, error.code];
    case 'PAYLOAD_TOO_LARGE':
      return [413, error.code];
    case 'INVALID_JSON':
      return [400, error.code];
    default:
      return [400, 'BAD_REQUEST'];
  }
}

function apiPath(url) {
  return url.pathname.replace(/\/+$/, '') || '/';
}

function serveStatic(request, response, pathname) {
  const requested = pathname === '/' ? '/index.html' : pathname;
  const filePath = path.resolve(PUBLIC_DIR, `.${requested}`);
  if (!filePath.startsWith(`${PUBLIC_DIR}${path.sep}`)) {
    sendError(response, 404, 'Arquivo não encontrado', 'NOT_FOUND');
    return;
  }
  fs.readFile(filePath, (error, content) => {
    if (error) {
      sendError(response, 404, 'Arquivo não encontrado', 'NOT_FOUND');
      return;
    }
    response.writeHead(200, {
      'Content-Type': CONTENT_TYPES[path.extname(filePath)] ?? 'application/octet-stream',
      'Cache-Control': pathname === '/' ? 'no-store' : 'public, max-age=3600',
    });
    response.end(content);
  });
}

function createServer(options = {}) {
  const state = options.state ?? createState(options);
  const corsOrigin = options.corsOrigin ?? process.env.CORS_ORIGIN ?? '*';
  const log = options.logger === false ? () => {} : (message) => console.log(message);

  const server = http.createServer(async (request, response) => {
    const requestUrl = new URL(request.url, `http://${request.headers.host ?? 'localhost'}`);
    const pathname = apiPath(requestUrl);
    response.setHeader('Access-Control-Allow-Origin', corsOrigin);
    response.setHeader('Access-Control-Allow-Methods', 'GET,POST,PUT,DELETE,OPTIONS');
    response.setHeader('Access-Control-Allow-Headers', 'Content-Type');

    if (request.method === 'OPTIONS') {
      response.writeHead(204);
      response.end();
      return;
    }

    try {
      if (request.method === 'GET' && pathname === '/health') {
        sendJson(response, 200, { status: 'ok', service: 'lampadas-dashboard' });
        return;
      }
      if (request.method === 'GET' && pathname === '/api/snapshot') {
        sendJson(response, 200, snapshot(state));
        return;
      }
      if (request.method === 'GET' && pathname === '/api/lampada/status') {
        sendJson(response, 200, listLamps(state));
        return;
      }
      if (request.method === 'PUT' && pathname === '/api/lampada/status/limpar') {
        sendJson(response, 200, { mensagem: 'Todas apagadas!', lampadas: clearLamps(state) });
        return;
      }

      const lampMatch = pathname.match(/^\/api\/lampada\/(\d+)\/status$/);
      if (request.method === 'PUT' && lampMatch) {
        const data = await parseJsonBody(request);
        const result = setLampStatus(state, lampMatch[1], data.idEquipe, data.status);
        sendJson(response, 200, result);
        return;
      }

      if (request.method === 'GET' && pathname === '/api/equipe') {
        const code = requestUrl.searchParams.get('codEquipe');
        const teams = listTeams(state).filter((team) => code === null || team.codEquipe === Number(code));
        sendJson(response, 200, Object.fromEntries(teams.map((team) => [team.codEquipe, team])));
        return;
      }
      if (request.method === 'POST' && pathname === '/api/equipe') {
        const team = createTeam(state, await parseJsonBody(request));
        sendJson(response, 201, { mensagem: 'Equipe adicionada com sucesso', equipe: team });
        return;
      }

      const teamMatch = pathname.match(/^\/api\/equipe\/(\d+)$/);
      if (teamMatch && request.method === 'PUT') {
        const team = updateTeam(state, teamMatch[1], await parseJsonBody(request));
        sendJson(response, 200, { mensagem: 'Equipe atualizada com sucesso', equipe: team });
        return;
      }
      if (teamMatch && request.method === 'DELETE') {
        const team = removeTeam(state, teamMatch[1]);
        sendJson(response, 200, { mensagem: 'Equipe removida com sucesso', equipe: team });
        return;
      }

      if (request.method === 'GET' && pathname === '/api/historico/exibir') {
        sendJson(response, 200, { mensagem: 'Histórico exibido', visivel: setHistoryVisible(state, true) });
        return;
      }
      if (request.method === 'GET' && pathname === '/api/historico/ocultar') {
        sendJson(response, 200, { mensagem: 'Histórico ocultado', visivel: setHistoryVisible(state, false) });
        return;
      }
      if (request.method === 'GET' && pathname === '/api/events') {
        response.writeHead(200, {
          'Content-Type': 'text/event-stream; charset=utf-8',
          'Cache-Control': 'no-cache, no-transform',
          Connection: 'keep-alive',
          'X-Accel-Buffering': 'no',
        });
        response.write(`event: snapshot\ndata: ${JSON.stringify(snapshot(state))}\n\n`);
        const unsubscribe = subscribe(state, (payload) => {
          response.write(`event: update\ndata: ${payload}\n\n`);
        });
        const keepAlive = setInterval(() => response.write(': keep-alive\n\n'), 15_000);
        request.on('close', () => {
          clearInterval(keepAlive);
          unsubscribe();
        });
        return;
      }

      if (request.method === 'GET') {
        serveStatic(request, response, pathname);
        return;
      }
      sendError(response, 404, 'Rota não encontrada', 'NOT_FOUND');
    } catch (error) {
      const [status, code] = routeError(error);
      log(`[${status}] ${request.method} ${pathname}: ${error.message}`);
      sendError(response, status, error.message, code);
    }
  });

  server.state = state;
  return server;
}

if (require.main === module) {
  const port = Number(process.env.PORT ?? 3000);
  const server = createServer();
  server.listen(port, '0.0.0.0', () => {
    console.log(`Painel Lampadas disponível em http://localhost:${port}`);
  });
}

module.exports = { createServer };
