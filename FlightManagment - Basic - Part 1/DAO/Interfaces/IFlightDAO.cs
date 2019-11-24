using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    public interface IFlightDAO : IBasicDB<Flight>
    {
        Dictionary<Flight, int> GetAllFlightsVacancy();
        IList<Flight> GetFlightsByCustomer(Customer customer);
        IList<Flight> GetFlightsByAirlineCompany(AirlineCompany airline);
        IList<Flight> GetFlightsByOriginCounty(int countryCode);
        IList<Flight> GetFlightsByDestinationCountry(int countryCode);
        IList<Flight> GetFlightsByDepartureDate(DateTime date);
        IList<Flight> GetFlightsByLandingDate(DateTime date);
    }
}
