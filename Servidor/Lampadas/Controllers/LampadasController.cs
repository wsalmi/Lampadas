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

namespace Lampadas.Controllers
{
    [RoutePrefix("api/lampada")]
    public class LampadasController : ApiController
    {
        public static DateTime Last;
        public static ConcurrentDictionary<byte, bool> Lampadas;
        public static ConcurrentBag<string> Tokens = new ConcurrentBag<string>();
        public static ConcurrentDictionary<byte, TimeData> Times = new ConcurrentDictionary<byte, TimeData>();

        public static byte TimeAutorizado = 0;

        public LampadasController()
        {
            if (Lampadas == null)
            {
                Lampadas = new ConcurrentDictionary<byte, bool>();
                Lampadas.TryAdd(1, false);
                Lampadas.TryAdd(2, false);
                Lampadas.TryAdd(3, false);
                Lampadas.TryAdd(4, false);
                Lampadas.TryAdd(5, false);
                Lampadas.TryAdd(6, false);
                Lampadas.TryAdd(7, false);
                Lampadas.TryAdd(8, false);
                Lampadas.TryAdd(9, false);
                Lampadas.TryAdd(10, false);
            }
        }

        [HttpGet, Route("status")]
        public async Task<IEnumerable<object>> Get()
        {
            return await Task.Run<IEnumerable<object>>(() =>
            {
                return Lampadas.ToList().OrderBy(e => e.Key).Select(l => new { Lampada = l.Key, Status = l.Value });
            });
        }

        [HttpPut, Route("{idLampada}/status")]
        public IHttpActionResult Post([FromBody]PostData data, [FromUri]byte idLampada)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MainHub>();

            if (Lampadas.ContainsKey(idLampada))
            {
                //if (Tokens.IsEmpty || token == TestToken || Tokens.Contains(token))
                //if (Tokens.Contains(token))
                //{

                var agora = DateTime.Now;
                Lampadas[idLampada] = data.status;
                var seconds = (agora - Last).TotalMilliseconds;
                if (data.status)
                    hubContext.Clients.All.acender(idLampada, seconds);
                else
                    hubContext.Clients.All.apagar(idLampada, seconds);

                Last = agora;

                return Ok(new { lampada = idLampada, status = data.status ? "Acesa" : "Apagada" });

                //}
                //else
                //{
                //if (data.status)
                //hubContext.Clients.All.testeAcender(idLampada);
                //else
                //hubContext.Clients.All.testeApagar(idLampada);
                //}
            }
            else
            {
                return InternalServerError(new MalandraoException("Tsc tsc tsc... Essa lampada não existe!"));
            }

        }

        public struct PostData
        {
            public byte idEquipe;
            public bool status;
        }

        public struct TimeData
        {
            byte CodTime;
            string Nome;
        }
    }
}