using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteLampada.Rest;

namespace TesteLampada
{
    class Program
    {
        static void Main(string[] args)
        {
            //while (true)
            //{
            //    for (byte i = 1; i <= 10; i++)
            //    {
            //        HttpClientSample.PostAsync(new HttpClientSample.Lampada { lampada = i, status = false });
            //    }

            //    Console.WriteLine("Limpo");
            //    Console.ReadKey();
            //    Console.Clear();
            //}


            //Fun();
            Fun2();

            Console.ReadKey();
            Environment.Exit(0);
        }

        private static void Fun()
        {
            Task.Factory.StartNew(() =>
            {
                var lampadas = (byte)11;
                var status = true;
                while (true)
                {
                    for (byte i = 1; i < lampadas; i++)
                    {
                        HttpClientSample.PostAsync(new HttpClientSample.Lampada { lampada = i, status = status });
                        //Console.WriteLine(i);
                        HttpClientSample.PostAsync(new HttpClientSample.Lampada { lampada = (byte)(lampadas - i), status = status });
                        //Console.WriteLine(lampadas - i);
                        Thread.Sleep(200);
                    }
                    status = !status;
                }
            });
        }

        private static void Fun2()
        {
            Task.Factory.StartNew(() =>
            {
                var status = true;
                var lampadas = new bool[10];
                var rdm = new Random();
                while (true)
                {
                    try
                    {
                        var val = (byte)rdm.Next(1, 11);
                        status = !lampadas[val - 1];
                        //Console.WriteLine(val);
                        HttpClientSample.PostAsync(new HttpClientSample.Lampada { lampada = val, status = status });
                        Thread.Sleep(50);
                        lampadas[val - 1] = status;
                    }
                    catch
                    {

                    }
                    
                }
            });
        }
    }
}
