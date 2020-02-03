using Lampadas.Exceptions;
using Lampadas.Store;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Http;

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

            if (Data.Lampadas.ContainsKey(idLampada))
            {
                if (Data.EquipeAutorizada == data.CodEquipe)
                {
                    var lampada = Data.Lampadas[idLampada];
                    var ultimaInteracao = Convert.ToUInt64(Math.Floor((DateTime.Now - (Data.UltimaInteracao.HasValue ? Data.UltimaInteracao.Value : DateTime.Now)).TotalMilliseconds / 100) * 100);
                    var ultimaInteracaoLampada = Convert.ToUInt64(Math.Floor((DateTime.Now - lampada.UltimaAlteracao).TotalMilliseconds / 100) * 100);

                    if (data.Status)
                        hubContext.Clients.All.acender(idLampada, ultimaInteracaoLampada, ultimaInteracao);
                    else
                        hubContext.Clients.All.apagar(idLampada, ultimaInteracaoLampada, ultimaInteracao);

                    Data.Lampadas[idLampada].Status = data.Status;
                    Data.UltimaInteracao = lampada.UltimaAlteracao;
                }

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
                hubContext.Clients.All.apagar(item.Key, 0, 0);
            }

            hubContext.Clients.All.reiniciar();
            Data.UltimaInteracao = null;
        }

        [DataContract]
        public struct PostData
        {
            [DataMember(Name = "idEquipe")]
            public byte CodEquipe { get; set; }

            [DataMember(Name = "status")]
            public bool Status { get; set; }
        }
    }
}