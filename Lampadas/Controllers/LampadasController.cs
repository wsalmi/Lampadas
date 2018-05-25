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

namespace Lampadas.Controllers
{
    public class LampadasController : ApiController
    {
        public LampadasController()
        {

        }

        // GET api/Me
        public GetViewModel Get(string nome = "Marcola")
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MainHub>();
            hubContext.Clients.All.hello(nome);


            //var user = UserManager.FindById(User.Identity.GetUserId());
            return new GetViewModel() { Hometown = "Casa da Mãe Joana" };
        }
    }
}