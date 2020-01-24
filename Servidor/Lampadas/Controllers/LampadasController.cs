using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using Lampadas.Models;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using Lampadas.Exceptions;
using System.Runtime.Serialization;
using Lampadas.Store;

namespace Lampadas.Controllers
{
    [RoutePrefix("api/lampada")]
    public class LampadasController : ApiController
    {
        [HttpGet, Route("status")]
        public IEnumerable<LampadaData> GetStatus()
        {
            return Data.Lampadas.ToList().OrderBy(e => e.Key).Select(l => l.Value);
        }

        [HttpPut, Route("status/limpar")]
        public IHttpActionResult PutStatusLimpar()
        {
            ApagarTodas();
            return Ok(new { mensagem = "Todas apagadas!" });
        }

        [HttpPut, Route("{idLampada}/status")]
        public IHttpActionResult PutStatus([FromBody]PostData data, [FromUri]byte idLampada)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MainHub>();

            if (Data.EquipeAutorizada != data.IdEquipe)
                return Unauthorized();

            if (Data.Lampadas.ContainsKey(idLampada))
            {
                var lampada = Data.Lampadas[idLampada];
                var seconds = Convert.ToUInt64((DateTime.Now - lampada.UltimaAlteracao).TotalMilliseconds);

                if (data.Status)
                    hubContext.Clients.All.acender(idLampada, seconds);
                else
                    hubContext.Clients.All.apagar(idLampada, seconds);

                Data.Lampadas[idLampada].Status = data.Status;

                return Ok(new { lampada = idLampada, status = data.Status ? "Acesa" : "Apagada" });
            }
            else
            {
                return InternalServerError(new MalandraoException("Tsc tsc tsc... Essa lampada não existe!"));
            }

        }

        public static void ApagarTodas()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MainHub>();

            foreach (var item in Data.Lampadas)
            {
                item.Value.Status = false;
                hubContext.Clients.All.apagar(item.Key, 0);
            }

            hubContext.Clients.All.reiniciar();
        }

        [DataContract]
        public struct PostData
        {
            [DataMember(Name = "idEquipe")]
            public byte IdEquipe { get; set; }

            [DataMember(Name = "status")]
            public bool Status { get; set; }
        }


    }
}