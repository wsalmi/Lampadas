using System;
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
        public class Lampada
        {
            public byte lampada { get; set; }
            public bool status { get; set; }
        }

        static WebClient client = new WebClient();

        static async Task<Lampada> GetProductAsync(string path)
        {
            Lampada lamp = null;

            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                lamp = await response.Content.ReadAsAsync<Lampada>();
            }
            return lamp;
        }
        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task<Uri> PostAsync(Lampada lamp)
        {
            string data = string.Format("?lampada={0}&status={1}", lamp.lampada, lamp.status);
            StringContent obj = new StringContent(data);
            HttpResponseMessage response = client.("api/lampadas", new { });
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:64195/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                Lampada lamp = new Lampada
                {
                    lampada = 1,
                    status = true
                };

                var url = await PostAsync(lamp);
                Console.WriteLine($"Created at {url}");

                lamp = await GetProductAsync(url.PathAndQuery);
                Console.WriteLine(lamp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }

    
}
