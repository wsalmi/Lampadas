using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Lampadas.Controllers
{
    [AllowAnonymous]
    public class MainHub : Hub
    {
        public static IHubCallerConnectionContext<dynamic> StaticClients
        {
            get
            {
                lock (_staticClients)
                {
                    return _staticClients;
                }
            }
            set
            {
                _staticClients = value;
            }
        }
        public static IHubCallerConnectionContext<dynamic> _staticClients;

        public MainHub() : base()
        {
            StaticClients = Clients;
        }

        [AllowAnonymous]
        public void Hello(string nomeDoCara)
        {
            Clients.All.hello(nomeDoCara);
        }
    }
}