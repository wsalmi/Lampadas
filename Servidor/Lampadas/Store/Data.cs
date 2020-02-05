using Lampadas.Controllers;
using Lampadas.Store;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Lampadas
{
    public static class Data
    {
        private static byte equipeAutorizada = 0;
        public static ConcurrentDictionary<byte, EquipeData> Equipes = new ConcurrentDictionary<byte, EquipeData>(SeedEquipes());
        public static ConcurrentDictionary<byte, LampadaData> Lampadas = new ConcurrentDictionary<byte, LampadaData>(SeedLampadas());
        public static DateTime? UltimaInteracao { get; set; }
        public static byte EquipeAutorizada
        {
            get => equipeAutorizada;
            set
            {
                equipeAutorizada = value;
                DefinirNomeEquipe(equipeAutorizada);
            }
        }


        private static void DefinirNomeEquipe(byte equipeAutorizada)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MainHub>();

            hubContext.Clients.All.mudarTime(Equipes[equipeAutorizada]);
        }

        private static IEnumerable<KeyValuePair<byte, EquipeData>> SeedEquipes()
        {
            yield return new EquipeData { CodEquipe = 0, Nome = "squad37" };
        }
        private static IEnumerable<KeyValuePair<byte, LampadaData>> SeedLampadas()
        {
            yield return new LampadaData { IdLampada = 1, Status = false };
            yield return new LampadaData { IdLampada = 2, Status = false };
            yield return new LampadaData { IdLampada = 3, Status = false };
            yield return new LampadaData { IdLampada = 4, Status = false };
            yield return new LampadaData { IdLampada = 5, Status = false };
            yield return new LampadaData { IdLampada = 6, Status = false };
            yield return new LampadaData { IdLampada = 7, Status = false };
            yield return new LampadaData { IdLampada = 8, Status = false };
            yield return new LampadaData { IdLampada = 9, Status = false };
            yield return new LampadaData { IdLampada = 10, Status = false };
        }
    }
}