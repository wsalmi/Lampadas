using Newtonsoft.Json;
using STF.SI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TesteLampada.Rest
{
   
    public static class HttpClientSample
    {
        const string Url = "http://10.14.11.28/hack/api/lampadas";
        const string UrlPost = "http://10.14.11.28/hack/api/lampadas?token=" + token;
        const string token = "TesteMeu";

        public class Lampada
        {
            public byte lampada { get; set; }
            public bool status { get; set; }
        }

        public static List<Lampada> GetProduct(Lampada lamp)
        {
            List<Lampada> obj = new List<Lampada>();
            try
            {
                var parametros = new Dictionary<string, string>();
                parametros.Add("lampada", lamp.lampada.ToString());
                parametros.Add("status", lamp.status.ToString());
                var resultado = Network.HTTP.GET(Url, parametros);
                obj = JsonConvert.DeserializeObject<List<Lampada>>(resultado);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception ", e.InnerException);
            }

            return obj;
        }

        public static void PostAsync(Lampada lamp)
        {
            try
            {
                var parametros = new Dictionary<string, string>();
                parametros.Add("lampada", lamp.lampada.ToString());
                parametros.Add("status", lamp.status.ToString());

                Network.HTTP.POST_Json(UrlPost, parametros);

            }
            catch (Exception e) {
                Console.WriteLine("Exception ", e.InnerException);
            }
        }

    }

}
