using System;
using System.Runtime.Serialization;

namespace FlightManagment___Basic___Part_1
{
    [Serializable]
    public class TicketNotMatchException : ApplicationException
    {
        public TicketNotMatchException()
        {
        }

        public TicketNotMatchException(string message) : base(message)
        {
        }

        public TicketNotMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TicketNotMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}