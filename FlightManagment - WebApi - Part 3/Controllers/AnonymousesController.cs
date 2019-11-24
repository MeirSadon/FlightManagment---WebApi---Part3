using FlightManagment___Basic___Part_1;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FlightManagment___WebApi___Part_3
{
    /// <summary>
    /// Controller For All Functions Of Anonymous Facade.
    /// </summary>
    [RoutePrefix("api/search")]
    public class AnonymousController : ApiController
    {
        private AnonymousUserFacade facade = new AnonymousUserFacade();
        const string DEFAULT_DATE = "2000-01-01 00:00:00.000";

        #region Get Company By Company Name.
        /// <summary>
        /// Get Company By Company Name.
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(AirlineCompany))]
        [Route("company/byname", Name = "GetCompanyByAirlineName")]
        [HttpGet]
        public IHttpActionResult GetCompanyByCompanyName([FromUri]string companyName)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                AirlineCompany company = facade.GetAirlineByAirlineName(companyName);
                return GetSuccessResponseForSingle(company, "No Company With The Received UserName Was Found.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get All Companies.
        /// <summary>
        /// Get All Companies.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IList<AirlineCompany>))]
        [Route("companies", Name = "GetAllCompanies")]
        [HttpGet]
        public IHttpActionResult GetAllCompanies()
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                IList<AirlineCompany> companies = facade.GetAllAirlineCompanies();
                if (companies.Count < 1)
                    return Content(HttpStatusCode.NoContent, "Company List Is Empty.");
                return Content(HttpStatusCode.OK, companies);
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get Flight By Id.
        /// <summary>
        /// Get Flight By Id.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Flight))]
        [Route("flights/{id}", Name = "GetFlightById")]
        [HttpGet]
        public IHttpActionResult GetFlightById([FromUri]int id)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                Flight flight = facade.GetFlightById(id);
                return GetSuccessResponseForSingle(flight, "No Flight With The Received ID Was Found.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get All Flights Vacancy.
        /// <summary>
        /// Get All Flights Vacancy.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Dictionary<Flight, int>))]
        [Route("flights/vacancy", Name = "GetAllFlightsVacancy")]
        [HttpGet]
        public IHttpActionResult GetAllFlightsVacancy()
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                Dictionary<Flight, int> vacancyFlights = facade.GetAllFlightsVacancy();
                if (vacancyFlights.Count < 1)
                    return Content(HttpStatusCode.NoContent, "Sorry, But Currently, Has No Flights With Tickets To Buy... Please Try Later.");
                return Content(HttpStatusCode.OK, vacancyFlights);
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get Flights By Few Optional Parameters(OriginCountry,DestinationCountry,DepartureDate(From&To)).
        /// <summary>
        /// Get Flights By Few Optional Parameters(OriginCountry,DestinationCountry,DepartureDate(From&To)).
        /// /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IList<Flight>))]
        [Route("flights/byfiter", Name = "GetFlightsByFewParameters")]
        [HttpGet]
        public IHttpActionResult GetFlightsByFewParameters([FromUri]int fromcountry = 0, [FromUri]int tocountry = 0, [FromUri]string fromdate = DEFAULT_DATE, [FromUri]string todate = DEFAULT_DATE, [FromUri]double flightduration = 0d)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                //DateTime fromDate = DateTime.Parse(fromdate);
                //DateTime toDate = DateTime.Parse(todate);
                List<Flight> allFoundFlights = (List<Flight>)facade.GetAllFlights();
                allFoundFlights = RemoveUnmatchedFlights(allFoundFlights, fromcountry, tocountry, DateTime.Parse(fromdate), DateTime.Parse(todate), flightduration);
                if (allFoundFlights.Count < 1)
                    return Content(HttpStatusCode.NoContent, "No Flight Found Matching Sent Parameters.");
                return Content(HttpStatusCode.OK, allFoundFlights);
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get All Flights.
        /// <summary>
        /// Get All Flights.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IList<Flight>))]
        [Route("flights", Name = "GetAllFlights")]
        [HttpGet]
        public IHttpActionResult GetAllFlights()
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                IList<Flight> flights = facade.GetAllFlights();
                if (flights.Count < 1)
                    return Content(HttpStatusCode.NoContent, "Sorry, But Currently, Has No Flights... Please Try Later.");
                return Content(HttpStatusCode.OK, flights);
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get Country By Id.
        /// <summary>
        /// Get Country By Id.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Country))]
        [Route("countries/{id}", Name = "GetCountryById")]
        [HttpGet]
        public IHttpActionResult GetCountryById([FromUri]int id)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                Country country = facade.GetCountryById(id);
                return GetSuccessResponseForSingle(country, "No Country Found With ID That Recived...");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get Country By Name.
        /// <summary>
        /// Get Country By Name.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Country))]
        [Route("countries/byname", Name = "GetCountryByName")]
        [HttpGet]
        public IHttpActionResult GetCountryByName([FromUri]string name = "")
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                Country country = facade.GetCountryByName(name);
                return GetSuccessResponseForSingle(country, "No Country Found With Name That Recived...");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get All Countries.
        /// <summary>
        /// Get All Countries.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IList<Country>))]
        [Route("countries", Name = "GetAllCountries")]
        [HttpGet]
        public IHttpActionResult GetAllCountries()
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                IList<Country> countries = facade.GetAllCountries();
                if (countries.Count < 1)
                    return Content(HttpStatusCode.NoContent, "Sorry, But No Countries Have Been Added To The Site Yet.");
                return Content(HttpStatusCode.OK, countries);
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Execute Any Action With All Catch Cases.
        /// <summary>
        /// One Function For All Catch Cases.
        /// </summary>
        /// <param name="myFunc"></param>
        /// <returns>IHttpActionResult</returns>
        private IHttpActionResult ExecuteSafe(Func<IHttpActionResult> myFunc)
        {
            try
            {
                return myFunc.Invoke();
            }
            catch (UserNotExistException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                return Content(HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region Function To Remove All Unmatches Flights On Current Search.
        /// <summary>
        ///  Function For Search Flight By Few Parameters(FromCountry, ToCountry, FromDate, ToDate, FlightDuration)
        /// </summary>
        /// <param name="allFlights" name="fromCountry" name="toCountry" name="fromDate" name="ToDate" name="flightDuration"></param>
        /// <returns>List</returns>
        private List<Flight> RemoveUnmatchedFlights(List<Flight> allFlights, int fromCountry, int toCountry, DateTime fromDate, DateTime ToDate, double flightDuration)
        {
            if (fromCountry != 0)
                allFlights.RemoveAll(f => f.Origin_Country_Code != fromCountry);
            if (toCountry != 0)
                allFlights.RemoveAll(f => f.Destination_Country_Code != toCountry);
            if (fromDate != Convert.ToDateTime(DEFAULT_DATE))
                allFlights.RemoveAll(f => f.Departure_Time < fromDate);
            if (ToDate != Convert.ToDateTime(DEFAULT_DATE))
                allFlights.RemoveAll(f => f.Departure_Time > ToDate);
            if (flightDuration != 0d)
                allFlights.RemoveAll(f => f.Departure_Time.AddHours(flightDuration) > f.Landing_Time);
            return allFlights;
        }
        #endregion

        #region Get The Search Result For Single Instance.
        /// <summary>
        ///  Function That Return If There Is Something In The Search Of Single Instance.
        /// </summary>
        /// <param name="poco" name="notFoundResponse"></param>
        /// <returns>IHttpActionResult</returns>
        private IHttpActionResult GetSuccessResponseForSingle(IPoco poco, string notFoundResponse)
        {
            IHttpActionResult successResponse;
            if (poco.GetHashCode() == 0)
                successResponse = Content(HttpStatusCode.NoContent, notFoundResponse);
            else
                successResponse = Content(HttpStatusCode.OK, poco);
            return successResponse;
        }
        #endregion
    }
}
