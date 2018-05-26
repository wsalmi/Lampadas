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

namespace Lampadas.Controllers
{
    public class LampadasController : ApiController
    {
        const string TestToken = "TesteMeu";

        public static DateTime Last { get; set; }

        public static ConcurrentDictionary<byte, bool> Lampadas { get; set; }
        public static ConcurrentBag<string> Tokens { get; set; } = new ConcurrentBag<string>();

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

        // GET api/Me
        public async Task<IEnumerable<object>> Get()
        {
            return await Task.Run<IEnumerable<object>>(() =>
            {
                return Lampadas.ToList().OrderBy(e => e.Key).Select(l => new { Lampada = l.Key, Status = l.Value });
            });
        }

        [HttpPost]
        public void Post([FromBody]PostData data, string token)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MainHub>();

            if (Lampadas.ContainsKey(data.lampada))
            {
                if (Tokens.IsEmpty || token == TestToken || Tokens.Contains(token))
                //if (Tokens.Contains(token))
                {
                    var agora = DateTime.Now;
                    Lampadas[data.lampada] = data.status;
                    var seconds = (agora - Last).TotalMilliseconds;
                    if (data.status)
                        hubContext.Clients.All.acender(data.lampada, seconds);
                    else
                        hubContext.Clients.All.apagar(data.lampada, seconds);

                    Last = agora;
                }
                else
                {
                    if (data.status)
                        hubContext.Clients.All.testeAcender(data.lampada);
                    else
                        hubContext.Clients.All.testeApagar(data.lampada);
                }
            }
            else
            {
                throw new Exception("Tsc tsc tsc... Essa lampada não existe!");
            }

        }

        [HttpPost]
        public void CriarToken([FromBody]string Token)
        {
            if (!string.IsNullOrEmpty(Token))
            {
                if (Tokens.Any(e => e == Token))
                    throw new HttpException(400, "O Token já existe na lista !");

                Tokens.Add(Token);
            }
            else
            {
                throw new HttpException(400, "O Token não pode ser vazio ou nulo !");
            }
        }

        public struct PostData
        {
            public byte lampada;
            public bool status;
        }
    }
}