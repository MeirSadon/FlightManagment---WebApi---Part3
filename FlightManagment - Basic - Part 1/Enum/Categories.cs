using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    [Flags]
    public enum Categories
    {
        Administrators = 1,
        AirlineCompanies = 2,
        Customers = 4,
        Flights = 8,
        Tickets = 16,
        Countries = 32,
        Adds = 64,
        Logins = 128,
        Deletions = 256,
        Updates = 512,

        MovedToHIstory = 1024
    }
}
