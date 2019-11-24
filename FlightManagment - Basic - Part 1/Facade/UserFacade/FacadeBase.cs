using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    public abstract class FacadeBase
    {
        protected IAdministratorDAO _adminDAO;
        protected IAirlineDAO _airlineDAO;
        protected ICustomerDAO _customerDAO;
        protected IUserDAO _userDAO;
        protected IFlightDAO _flightDAO;
        protected ITicketDAO _ticketDAO;
        protected ICountryDAO _countryDAO;
        protected IMaintenanceDAO _backgroundDAO;

        public FacadeBase()
        {
            _adminDAO = new AdministratorDAOMSSQL();
            _airlineDAO = new AirlineDAOMSSQL();
            _customerDAO = new CustomerDAOMSSQL();
            _userDAO = new UserDAOMSSQL();
            _flightDAO = new FlightDAOMSSQL();
            _ticketDAO = new TicketDAOMSSQL();
            _countryDAO = new CountryDAOMSSQL();
            _backgroundDAO = new MaintenanceDAOMSSQL();
        }
    }
}
