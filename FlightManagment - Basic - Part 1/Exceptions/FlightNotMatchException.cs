using System;
using System.Runtime.Serialization;

namespace FlightManagment___Basic___Part_1
{
    [Serializable]
    public class FlightNotMatchException : Exception
    {
        public FlightNotMatchException()
        {
        }

        public FlightNotMatchException(string message) : base(message)
        {
        }

        public FlightNotMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FlightNotMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}