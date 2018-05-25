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
    //[Authorize]
    public class MeController : ApiController
    {
        private ApplicationUserManager _userManager;

        public MeController()
        {
        }

        public MeController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET api/Me
        public GetViewModel Get(string nome = "Marcola")
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MainHub>();
            hubContext.Clients.All.hello(nome);


            //Task.Factory.StartNew(() =>
            //{

            //    while (true)
            //    {
            //        System.Threading.Thread.Sleep(1000);

            //    }
            //});

            //var user = UserManager.FindById(User.Identity.GetUserId());
            return new GetViewModel() { Hometown = "Casa da Mãe Joana" };
        }
    }
}