using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    interface ILoginService
    {
        UserType TryLogin(string userName, string password, out ILogin token, out FacadeBase facade);
    }
}
