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
        const string Url = "http://localhost:64195/api/lampadas";
        public class Lampada
        {
            public byte lampada { get; set; }
            public bool status { get; set; }
        }

        public static async Task<Lampada> GetProductAsync(Lampada lamp)
        {
            try
            {
                IDictionary<string, string> parametros = null;
                parametros.Add("lampada", lamp.lampada.ToString());
                parametros.Add("status", lamp.status.ToString());

                Network.HTTP.GET(Url, parametros);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception ", e.InnerException);
            }
            
            return null;
        }

        public static async void PostAsync(Lampada lamp)
        {
            try
            {
                IDictionary<string, string> parametros = null;
                parametros.Add("lampada", lamp.lampada.ToString());
                parametros.Add("status", lamp.status.ToString());

                Network.HTTP.POST(Url, parametros);
            }
            catch (Exception e) {
                Console.WriteLine("Exception ", e.InnerException);
            }
        }

    }

}
