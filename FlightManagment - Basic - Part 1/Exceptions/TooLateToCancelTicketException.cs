using System;
using System.Runtime.Serialization;

namespace FlightManagment___Basic___Part_1
{
    [Serializable]
    public class TooLateToCancelTicketException : ApplicationException
    {
        public TooLateToCancelTicketException()
        {
        }

        public TooLateToCancelTicketException(string message) : base(message)
        {
        }

        public TooLateToCancelTicketException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TooLateToCancelTicketException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}