using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteLampada.Rest;

namespace TesteLampada
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("teste");
            var objOut = HttpClientSample.GetProduct(new HttpClientSample.Lampada { lampada=1, status = true});
            HttpClientSample.PostAsync(new HttpClientSample.Lampada { lampada = 5, status = true });
            //Console.WriteLine(objOut.ToString());
            Console.ReadLine();
           
        }
    }
}
