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

        // GET api/Me
        public async Task<IEnumerable<object>> Get(string nome = "Marcola")
        {
            return await Task.Run<IEnumerable<object>>(() =>
            {
                return Lampadas.Select(l => new { l });
            });           

            //var user = UserManager.FindById(User.Identity.GetUserId());
        }

        public void Post(byte lampada, bool status)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MainHub>();
            hubContext.Clients.All.hello("");
        }
    }
}