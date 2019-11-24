using System;
using System.Collections.Generic;
using FlightManagment___Basic___Part_1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestForFlightManagmentProject
{
    [TestClass]
    public class TestForAnonymouseFacade
    {
        /*  ======= All Tests =======

        1. GetAirlineByAirlineName        -- GetAirlineCompanyByCompanyName
        2. GetAllAirlineCompanies         -- GetAllArlineCompanies.
        3. GetFlightById                  -- GetFlightById.
        4. GetAllFlightsVacancy           -- GetAllFlightsVacancy.
        5. GetFlightsByDepatrureDate      -- GetFlightsByDepartureDate.
        6. GetFlightsByLandingDate        -- GetFlightsByLandingDate.
        7. GetFlightsByOriginCountry      -- GetFlightsByOriginCountry.
        8. GetFlightsByDestinationCountry -- GetFlightsByDestinationCountry.
        9. GetAllFlights                  -- GetAllFlights.
        10.GetCountryById                 -- GetCountryById.
        11.GetCountryByName               -- GetCountryByName.
        12.GetAllCountries                -- GetAllCountries.

        ======= All Tests ======= */


        TestCenter tc = new TestCenter();

        // Supposed To Get Airline By Company Name.
        [TestMethod]
        public void GetAirlineCompanyByCompanyName()
        {
            tc.PrepareDBForTests();
            AirlineCompany airline = new AnonymousUserFacade().GetAirlineByAirlineName(tc.airlineToken.User.Airline_Name);
            Assert.AreNotEqual(airline, null);
        }

        // Supposed To Get All Airline Companies.
        [TestMethod]
        public void GetAllArlineCompanies()
        {
            tc.PrepareDBForTests();
            AirlineCompany airline = tc.CreateRandomCompany();
            airline.Id = tc.adminFacade.CreateNewAirline(tc.adminToken, airline);
            IList<AirlineCompany> AirlineCompanies = new AnonymousUserFacade().GetAllAirlineCompanies();
            Assert.AreEqual(AirlineCompanies.Count, 2);
        }


        // Supposed To Get Flight By Id.
        [TestMethod]
        public void GetFlightById()
        {
            tc.PrepareDBForTests();
            Flight flight = new Flight
            {
                AirlineCompany_Id = tc.airlineToken.User.Id,
                Departure_Time = DateTime.Now,
                Landing_Time = DateTime.Now + TimeSpan.FromHours(1),
                Origin_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Destination_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Remaining_Tickets = 10
            };
            flight.Id = tc.airlineFacade.CreateFlight(tc.airlineToken, flight);
            Assert.AreEqual(tc.airlineFacade.GetFlightById((int)flight.Id), flight);
        }

        // Supposed To Get All Flights With At List One Ticket.
        [TestMethod]
        public void GetAllFlightsVacancy()
        {
            tc.PrepareDBForTests();
            Flight f1 = new Flight
            {
                AirlineCompany_Id = tc.airlineToken.User.Id,
                Departure_Time = DateTime.Now,
                Landing_Time = DateTime.Now + TimeSpan.FromHours(1),
                Origin_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Destination_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Remaining_Tickets = 10
            };
            Flight f2 = new Flight
            {
                AirlineCompany_Id = tc.airlineToken.User.Id,
                Departure_Time = DateTime.Now,
                Landing_Time = DateTime.Now + TimeSpan.FromHours(1),
                Origin_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Destination_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Remaining_Tickets = 0
            };
            f1.Id = tc.airlineFacade.CreateFlight(tc.airlineToken, f1);
            f2.Id = tc.airlineFacade.CreateFlight(tc.airlineToken, f2);
            Dictionary<Flight, int> TicketsByFlight = new AnonymousUserFacade().GetAllFlightsVacancy();
            Assert.AreEqual(TicketsByFlight.Count, 1);
        }

        // Supposed To Get Flight By Departure Time.
        [TestMethod]
        public void GetFlightsByDepartureDate()
        {
            tc.PrepareDBForTests();
            Flight flight = new Flight
            {
                AirlineCompany_Id = tc.airlineToken.User.Id,
                Departure_Time = DateTime.Now,
                Landing_Time = DateTime.Now + TimeSpan.FromHours(4),
                Origin_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Destination_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Remaining_Tickets = 10
            };
            flight.Id = tc.airlineFacade.CreateFlight(tc.airlineToken, flight);
            Assert.AreEqual(tc.airlineFacade.GetFlightsByDepartureDate(flight.Departure_Time).Count, 1);
        }

        // Supposed To Get Flight By Landing Time.
        [TestMethod]
        public void GetFlightsByLandingDate()
        {
            tc.PrepareDBForTests();
            Flight flight = new Flight
            {
                AirlineCompany_Id = tc.airlineToken.User.Id,
                Departure_Time = new DateTime(2018, 10, 05),
                Landing_Time = new DateTime(2018, 10, 08),
                Origin_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Destination_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Remaining_Tickets = 10
            };
            flight.Id = tc.airlineFacade.CreateFlight(tc.airlineToken, flight);
            Assert.AreEqual(tc.airlineFacade.GetFlightsByLandingDate(new DateTime(2018, 10, 08))[0], flight);
        }

        // Supposed To Get Flight By Origin Country.
        [TestMethod]
        public void GetFlightsByOriginCountry()
        {
            tc.PrepareDBForTests();
            Flight flight = new Flight
            {
                AirlineCompany_Id = tc.airlineToken.User.Id,
                Departure_Time = new DateTime(2018, 10, 05),
                Landing_Time = new DateTime(2018, 10, 08),
                Origin_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Destination_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Remaining_Tickets = 10
            };
            flight.Id = tc.airlineFacade.CreateFlight(tc.airlineToken, flight);
            Assert.AreEqual(tc.adminFacade.GetFlightsByOriginCountry((int)tc.adminFacade.GetCountryByName("Israel").Id).Count, 1);
        }

        // Supposed To Get Flight By Destination Country.
        [TestMethod]
        public void GetFlightsByDestinationCountry()
        {
            tc.PrepareDBForTests();
            Flight flight = new Flight
            {
                AirlineCompany_Id = tc.airlineToken.User.Id,
                Departure_Time = new DateTime(2018, 10, 05),
                Landing_Time = new DateTime(2018, 10, 08),
                Origin_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Destination_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Remaining_Tickets = 10
            };
            flight.Id = tc.airlineFacade.CreateFlight(tc.airlineToken, flight);
            Assert.AreEqual(tc.adminFacade.GetFlightsByDestinationCountry((int)tc.adminFacade.GetCountryByName("Israel").Id).Count, 1);
        }

        // Supposed To Get All Flights.
        [TestMethod]
        public void GetAllFlights()
        {
            tc.PrepareDBForTests();
            Flight flight = new Flight
            {
                AirlineCompany_Id = tc.airlineToken.User.Id,
                Departure_Time = DateTime.Now,
                Landing_Time = DateTime.Now + TimeSpan.FromHours(1),
                Origin_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Destination_Country_Code = tc.adminFacade.GetCountryByName("Israel").Id,
                Remaining_Tickets = 100
            };
            flight.Id = tc.airlineFacade.CreateFlight(tc.airlineToken, flight);
            IList<Flight> flights = new AnonymousUserFacade().GetAllFlights();
            Assert.AreEqual(flights.Count, 1);
        }

        // Supposed To Get Country By Id.
        [TestMethod]
        public void GetCountryById()
        {
            tc.PrepareDBForTests();
            Country country = new Country("USA");
            country.Id = tc.adminFacade.CreateNewCountry(tc.adminToken, country);
            Assert.AreEqual(tc.airlineFacade.GetCountryById((int)country.Id), country);
        }

        // Supposed To Get Country By Name.
        [TestMethod]
        public void GetCountryByName()
        {
            tc.PrepareDBForTests();
            Country country = new Country("USA");
            country.Id = tc.adminFacade.CreateNewCountry(tc.adminToken, country);
            Assert.AreEqual(tc.airlineFacade.GetCountryByName("USA"), country);
        }

        // Supposed To Get All Countries.
        [TestMethod]
        public void GetAllCountries()
        {
            tc.PrepareDBForTests();
            Country country = new Country("USA");
            country.Id = tc.adminFacade.CreateNewCountry(tc.adminToken, country);
            IList<Country> countries = new AnonymousUserFacade().GetAllCountries();
            Assert.AreEqual(countries.Count, 2);
        }
    }
}
