using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace Lampadas.Controllers
{
    public class MainHub : Hub
    {
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }
}