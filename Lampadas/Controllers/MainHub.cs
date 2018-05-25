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
    public class MainHub : Hub
    {
        public void Hello(string nomeDoCara)
        {
            Clients.All.hello(nomeDoCara);
        }

        public void Acender(int lampada)
        {
            Clients.All.acender(lampada);
        }

        public void Apagar(int lampada)
        {
            Clients.All.apagar(lampada);
        }
    }
}