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
    /// Controller For All Functions Of Company Facade.
    /// </summary>
    [RoutePrefix("api/companies")]
    [Authorize(Roles = "Airline")]
    public class CompaniesController : ApiController
    {
        #region Change Password For Current Company.
        /// <summary>
        /// Change Password For Current Company.
        /// </summary>
        /// <param name="passwords"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("changemypassword", Name = "ChangePasswordForCurrentCompany")]
        [HttpPut]
        public IHttpActionResult ChangePasswordForCurrentCompany([FromBody]ChangePasswordModel passwords)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<AirlineCompany> myToken, out LoggedInAirlineFacade myFacade);
                myFacade.ChangeMyPassword(myToken, passwords.OldPassword, passwords.NewPassword);
                return Ok($"The Password Of Your Company Changed Successfully.");
            });
            return result;
        }
        #endregion

        #region Create New Flight.
        /// <summary>
        /// Create New Flight.
        /// </summary>
        /// <param name="flight"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Flight))]
        [Route("create/flight", Name = "CreateNewFlight")]
        [HttpPost]
        public IHttpActionResult CreateNewFlight([FromBody]Flight flight)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<AirlineCompany> myToken, out LoggedInAirlineFacade myFacade);
                flight.Id = myFacade.CreateFlight(myToken, flight);
                return CreatedAtRoute("GetFlightById", new { id = flight.Id }, flight);
            });
            return result;
        }
        #endregion

        #region Remove Some Flight.
        /// <summary>
        /// Remove Some Flight.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("remove/flight/{id}", Name = "RemoveSomeFlight")]
        [HttpDelete]
        public IHttpActionResult RemoveSomeFlight([FromUri]int id)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<AirlineCompany> myToken, out LoggedInAirlineFacade myFacade);
                Flight flight = myFacade.GetFlightById(id);
                myFacade.CancelFlight(myToken, flight);
                return Ok($"Flight Number: {flight.Id} From: {flight.Origin_Country_Code} To: {flight.Destination_Country_Code} At: {flight.Departure_Time} Deleted Successfuly.");
            });
            return result;
        }
        #endregion

        #region Update Current Airline.
        /// <summary>
        /// Update Current Airline.
        /// </summary>
        /// <param name="comapny"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("updatedetails", Name = "UpdateCurrentAirline")]
        [HttpPut]
        public IHttpActionResult UpdateCurrentAirline([FromBody]AirlineCompany company)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<AirlineCompany> myToken, out LoggedInAirlineFacade myFacade);
                if (myToken.User.Id != company.Id)
                    throw new IdIsNotMatchException("The ID That Sent Does Not Match To Your ID.");
                else
                {
                    myToken.User = company;
                    myFacade.MofidyAirlineDetails(myToken);
                }
                return Ok($"The Details Of Your Company Updated Successfully.");
            });
            return result;
        }
        #endregion

        #region Update Some Flight Details.
        /// <summary>
        /// Update Some Flight Details.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("update/flight/{id}", Name = "UpdateFlight")]
        [HttpPut]
        public IHttpActionResult UpdateFlight([FromUri]int id, [FromBody]Flight flight)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<AirlineCompany> myToken, out LoggedInAirlineFacade myFacade);
                Flight flightForUpdate = myFacade.GetFlightById(id);
                if (flight.Id != flightForUpdate.Id)
                    throw new IdIsNotMatchException("Your Flight ID Does Not Match ID That Found In Your URL.");
                else
                {
                    flightForUpdate = flight;
                    myFacade.UpdateFlight(myToken, flightForUpdate);
                }
                return Ok($"Flight Number: { flight.Id} Successfully Updated.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get Sold Ticket By Id.
        /// <summary>
        /// Get Sold Ticket By Id.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Ticket))]
        [Route("ticket/{id}", Name = "GetSoldTicketById")]
        [HttpGet]
        public IHttpActionResult GetSoldTicketById([FromUri]int id)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<AirlineCompany> myToken, out LoggedInAirlineFacade myFacade);
                Ticket ticket = myFacade.GetSoldTicketById(myToken, id);
                return GetSuccessResponse(ticket, "No Ticket With The Received ID Was Found That Sold From Current Company.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get All Flights Of Current Company.
        /// <summary>
        /// Get All Flights Of Current Company.
        /// </summary>
        /// <param name="company"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IList<Flight>))]
        [Route("flights", Name = "GetAllFlightsOfCurrentCompany")]
        [HttpGet]
        public IHttpActionResult GetAllFlightsOfCurrentCompany()
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<AirlineCompany> myToken, out LoggedInAirlineFacade myFacade);
                IList<Flight> flights = myFacade.GetAllFlightsByAirline(myToken);
                if (flights.Count < 1)
                    return Content(HttpStatusCode.NoContent, "Flight List Is Empty For This Company.");
                return Content(HttpStatusCode.OK, flights);
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get All Tickets Of Current Company.
        /// <summary>
        /// Get All Tickets Of Current Company.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IList<Ticket>))]
        [Route("tickets", Name = "GetAllTicketsOfCurrentCompany")]
        [HttpGet]
        public IHttpActionResult GetAllTicketsOfCurrentCompany()
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<AirlineCompany> myToken, out LoggedInAirlineFacade myFacade);
                IList<Ticket> tickets = myFacade.GetAllTicketsByAirline(myToken);
                if (tickets.Count < 1)
                    return Content(HttpStatusCode.NoContent, "Ticket List Is Empty For This Company.");
                return Content(HttpStatusCode.OK, tickets);
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
            catch (DepartureTimeTooLateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (WrongPasswordException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TicketNotMatchException ex)
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

        #region GetTokenAndFacade.
        /// <summary>
        ///  Function That Return Token And Facade
        /// </summary>
        /// <param name="userName" name="token" name="facade"></param>
        private void GetLoginToken(string userName, out LoginToken<AirlineCompany> token, out LoggedInAirlineFacade facade)
        {
            token = null;
            facade = null;
            AirlineCompany company = new LoggedInAdministratorFacade().GetAirlineByUserName(FlyingCenterConfig.basicToken, userName);
            if (company != null)
            {
                token = new LoginToken<AirlineCompany> { User = company };
                facade = new LoggedInAirlineFacade();
            }
        }
        #endregion

        #region Get The Search Result.
        /// <summary>
        ///  Function That Return If There Is Something In The Search.
        /// </summary>
        /// <param name="poco" name="notFoundResponse"></param>
        /// <returns>IHttpActionResult</returns>
        private IHttpActionResult GetSuccessResponse(IPoco poco, string notFoundResponse)
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
