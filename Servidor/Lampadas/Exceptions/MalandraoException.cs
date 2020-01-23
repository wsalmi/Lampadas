using System;

namespace Lampadas.Exceptions
{
    [Serializable]
    public class MalandraoException : Exception
    {
        public MalandraoException() { }
        public MalandraoException(string message) : base(message) { }
        public MalandraoException(string message, Exception inner) : base(message, inner) { }
        protected MalandraoException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}