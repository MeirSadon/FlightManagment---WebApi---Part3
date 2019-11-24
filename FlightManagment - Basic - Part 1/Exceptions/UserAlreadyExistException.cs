using System;
using System.Runtime.Serialization;

namespace FlightManagment___Basic___Part_1
{
    [Serializable]
    public class UserAlreadyExistException : ApplicationException
    {
        public UserAlreadyExistException()
        {
        }

        public UserAlreadyExistException(string message) : base(message)
        {
        }

        public UserAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}