using System.Threading;
using System;

namespace _NET
{
    class Program
    {
        private static readonly Network lampadas = new Network();
        private static void lampadaStatus(byte lampada, bool status) => Console.WriteLine(lampadas.AtualizaStatus(lampada, status));

        static void Main(string[] args)
        {
            var delay = 500;
            while (true)
            {
                lampadaStatus(1, false);
                Thread.Sleep(delay);
                lampadaStatus(1, true);
                Thread.Sleep(delay);
                lampadaStatus(2, false);
                Thread.Sleep(delay);
                lampadaStatus(2, true);
                Thread.Sleep(delay);
                lampadaStatus(3, false);
                Thread.Sleep(delay);
                lampadaStatus(3, true);
                Thread.Sleep(delay);
                lampadaStatus(4, false);
                Thread.Sleep(delay);
                lampadaStatus(4, true);
                Thread.Sleep(delay);
                lampadaStatus(5, false);
                Thread.Sleep(delay);
                lampadaStatus(5, true);
                Thread.Sleep(delay);
                lampadaStatus(6, false);
                Thread.Sleep(delay);
                lampadaStatus(6, true);
                Thread.Sleep(delay);
                lampadaStatus(7, false);
                Thread.Sleep(delay);
                lampadaStatus(7, true);
                Thread.Sleep(delay);
                lampadaStatus(8, false);
                Thread.Sleep(delay);
                lampadaStatus(8, true);
                Thread.Sleep(delay);
                lampadaStatus(9, false);
                Thread.Sleep(delay);
                lampadaStatus(9, true);
                Thread.Sleep(delay);
                lampadaStatus(10, false);
                Thread.Sleep(delay);
                lampadaStatus(10, true);
                Thread.Sleep(delay);
            }
        }
    }
}