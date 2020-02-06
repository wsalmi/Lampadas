using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lampadas.Controllers;

namespace Lampadas.Store
{
    public class EquipeData
    {
        public byte CodEquipe { get; set; }
        public string Nome { get; set; }
        public bool Autorizada { get; set; }

        public static implicit operator KeyValuePair<byte, EquipeData>(EquipeData v)
        {
            return new KeyValuePair<byte, EquipeData>(v.CodEquipe, v);
        }
    }
}