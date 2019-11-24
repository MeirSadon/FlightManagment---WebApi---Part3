using System;
using System.Runtime.Serialization;

namespace FlightManagment___Basic___Part_1
{
    [Serializable]
    public class UserNotExistException : ApplicationException
    {
        public UserNotExistException()
        {
        }

        public UserNotExistException(string message) : base(message)
        {
        }

        public UserNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}