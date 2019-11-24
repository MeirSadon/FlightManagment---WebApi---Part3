using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    // Basic Facade(Without Login) With The Simple Options.
    public class AnonymousUserFacade : FacadeBase, IAnonymousUserFacade
    {

        // Get Some Airline By Company Name.
        public AirlineCompany GetAirlineByAirlineName(string airlineName)
        {
            return _airlineDAO.GetByName(airlineName);
        }

        // Get All Airline Companies.
        public IList<AirlineCompany> GetAllAirlineCompanies()
        {
            return _airlineDAO.GetAll();
        }


        // Get Some Flight By Id.
        public Flight GetFlightById(int id)
        {
            return _flightDAO.GetById(id);
        }

        // Get All Flights With At Least One Ticket.
        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            return _flightDAO.GetAllFlightsVacancy();
        }

        //Get All Flights By Departure Time.
        public IList<Flight> GetFlightsByDepartureDate(DateTime departureDate)
        {
            return _flightDAO.GetFlightsByDepartureDate(departureDate);
        }

        //Get All Flights By Destination Country.
        public IList<Flight> GetFlightsByDestinationCountry(int countryCode)
        {
            return _flightDAO.GetFlightsByDestinationCountry(countryCode);
        }

        // Get All Flights By Landing Date.
        public IList<Flight> GetFlightsByLandingDate(DateTime landingDate)
        {
            return _flightDAO.GetFlightsByLandingDate(landingDate);
        }

        // Get All Flights By Origin Country.
        public IList<Flight> GetFlightsByOriginCountry(int countryCode)
        {
            return _flightDAO.GetFlightsByOriginCounty(countryCode);
        }

        // Get All Flights
        public IList<Flight> GetAllFlights()
        {
            return _flightDAO.GetAll();
        }


        //Get Country By Id.
        public Country GetCountryById(int id)
        {
            return _countryDAO.GetById(id);
        }

        // Get Country By Name.
        public Country GetCountryByName(string name)
        {
            return _countryDAO.GetByName(name);
        }

        //Get All Countries.
        public IList<Country> GetAllCountries()
        {
            return _countryDAO.GetAll();
        }
    }
}
