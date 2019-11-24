using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    public class LoginToken<T> : ILogin where T : IUser
    {
        public T User { get; set; }
    }
}
