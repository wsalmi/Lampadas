using System;

namespace _NET
{
    class Program
    {
        private static readonly Network lampadas = new Network();
        private static void lampadaStatus(byte lampada, bool status) => Console.WriteLine(lampadas.AtualizaStatus(lampada, status));

        static void Main(string[] args)
        {
            lampadaStatus(1, false);
            lampadaStatus(1, true);
            lampadaStatus(2, false);
            lampadaStatus(2, true);
            lampadaStatus(3, false);
            lampadaStatus(3, true);
            lampadaStatus(4, false);
            lampadaStatus(4, true);
            lampadaStatus(5, false);
            lampadaStatus(5, true);
            lampadaStatus(6, false);
            lampadaStatus(6, true);
            lampadaStatus(7, false);
            lampadaStatus(7, true);
            lampadaStatus(8, false);
            lampadaStatus(8, true);
            lampadaStatus(9, false);
            lampadaStatus(9, true);
            lampadaStatus(10, false);
            lampadaStatus(10, true);
        }
    }
}