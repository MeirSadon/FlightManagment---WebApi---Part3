using System;
using System.Runtime.Serialization;

namespace FlightManagment___Basic___Part_1
{
    [Serializable]
    public class WrongPasswordException : ApplicationException
    {
        public WrongPasswordException()
        {
        }

        public WrongPasswordException(string message) : base(message)
        {
        }

        public WrongPasswordException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}