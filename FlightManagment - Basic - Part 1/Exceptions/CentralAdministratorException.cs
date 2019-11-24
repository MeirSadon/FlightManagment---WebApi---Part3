using System;
using System.Runtime.Serialization;

namespace FlightManagment___Basic___Part_1
{
    [Serializable]
    public class CentralAdministratorException : ApplicationException
    {
        public CentralAdministratorException()
        {
        }

        public CentralAdministratorException(string message) : base(message)
        {
        }

        public CentralAdministratorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CentralAdministratorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}