using System;
using System.Runtime.Serialization;

namespace FlightManagment___Basic___Part_1
{
    [Serializable]
    public class OutOfTicketsException : ApplicationException
    {
        public OutOfTicketsException()
        {
        }

        public OutOfTicketsException(string message) : base(message)
        {
        }

        public OutOfTicketsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OutOfTicketsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}