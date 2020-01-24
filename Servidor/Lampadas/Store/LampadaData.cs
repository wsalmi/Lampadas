using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Lampadas.Store
{
    [DataContract]
    public class LampadaData
    {
        private bool _status;

        [DataMember(Name = "id-lampada")]
        public byte IdLampada { get; set; }

        [DataMember(Name = "status")]
        public bool Status
        {
            get { return _status; }
            set
            {
                _status = value;
                UltimaAlteracao = DateTime.Now;
            }
        }

        [DataMember(Name = "ultima-alteracao")]
        public DateTime UltimaAlteracao { get; private set; }

        public static implicit operator KeyValuePair<byte, LampadaData>(LampadaData v)
        {
            return new KeyValuePair<byte, LampadaData>(v.IdLampada, v);
        }
    }
}