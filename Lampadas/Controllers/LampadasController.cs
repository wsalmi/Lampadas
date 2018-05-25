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
        public ConcurrentDictionary<byte, bool> Lampadas { get; set; }
        public ConcurrentBag<string> Tokens { get; set; }

        public LampadasController()
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

            Tokens.Add(DateTime.Today.ToString("d"));
        }

        // GET api/Me
        public async Task<IEnumerable<object>> Get()
        {
            return await Task.Run<IEnumerable<object>>(() =>
            {
                return Lampadas.ToList().OrderBy(e => e.Key).Select(l => new { Lampada = l.Key, Status = l.Value });
            });            
        }

        [HttpGet]
        public void Post(byte lampada, bool status, string token)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MainHub>();

            if (Tokens.Contains(token))
            {
                if (Lampadas.ContainsKey(lampada))
                {
                    Lampadas[lampada] = status;

                    if (status)
                        hubContext.Clients.All.acender(lampada);
                    else
                        hubContext.Clients.All.apagar(lampada);
                  
                }
            }

          
        }
    }
}