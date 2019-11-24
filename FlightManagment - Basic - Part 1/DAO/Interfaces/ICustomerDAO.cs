using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    public interface ICustomerDAO : IBasicDB<Customer>
    {
        void ChangePassword(Customer customer);
    }
}
