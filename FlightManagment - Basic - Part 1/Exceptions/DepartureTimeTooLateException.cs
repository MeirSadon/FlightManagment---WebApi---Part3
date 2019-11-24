using System;
using System.Runtime.Serialization;

namespace FlightManagment___Basic___Part_1
{
    [Serializable]
    public class DepartureTimeTooLateException : ApplicationException
    {
        public DepartureTimeTooLateException()
        {
        }

        public DepartureTimeTooLateException(string message) : base(message)
        {
        }

        public DepartureTimeTooLateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DepartureTimeTooLateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}