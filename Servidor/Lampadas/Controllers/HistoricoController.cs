using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Lampadas.Controllers
{
    [RoutePrefix("api/historico")]
    public class HistoricoController : ApiController
    {
        // GET: Historico
        [HttpGet, Route("exibir")]
        public IHttpActionResult Exibir()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MainHub>();

            hubContext.Clients.All.verHistorico(true);

            return Ok(new { msg = "🤘" });
        }

        [HttpGet, Route("ocultar")]
        public IHttpActionResult Ocultar()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MainHub>();

            hubContext.Clients.All.verHistorico(false);

            return Ok(new { msg = "🐱‍👤" });
        }
    }
}