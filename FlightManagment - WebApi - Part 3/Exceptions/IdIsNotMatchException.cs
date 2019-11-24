using System;
using System.Runtime.Serialization;

namespace FlightManagment___WebApi___Part_3
{
    [Serializable]
    public class IdIsNotMatchException : ApplicationException
    {
        public IdIsNotMatchException()
        {
        }

        public IdIsNotMatchException(string message) : base(message)
        {
        }

        public IdIsNotMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IdIsNotMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}