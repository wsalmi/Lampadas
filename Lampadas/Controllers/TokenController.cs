using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Lampadas.Controllers
{
    public class TokenController : ApiController
    {
        [HttpGet]
        public async Task<string> Get()
        {
            return await Task.Run(() =>
            {
                var token = Guid.NewGuid().ToString("D").Split('-').First();
                LampadasController.Tokens.Add(token);
                return token;
            });
        }

        [HttpDelete]
        public async Task<string> Delete([FromUri]string token)
        {
            return await Task.Run(() =>
            {
                LampadasController.Tokens.TryTake(out token);
                return token;
            });
        }
    }
}
