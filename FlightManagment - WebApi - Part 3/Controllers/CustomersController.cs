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
    /// Controller For All Functions Of Customer Facade.
    /// </summary>
    [RoutePrefix("api/customers")]
    [Authorize(Roles = "Customer")]
    public class CustomersController : ApiController
    {
        #region Purchase New Ticket.
        /// <summary>
        /// Purchase New Ticket.
        /// </summary>
        /// <param name="flightId"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Ticket))]
        [Route("purchase/ticket/{flightId}", Name = "PurchaseTicket")]
        [HttpPost]
        public IHttpActionResult PurchaseTicket([FromUri]int flightId)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Customer> myToken, out LoggedInCustomerFacade myFacade);
                Flight flight = myFacade.GetFlightById(flightId);
                long ticketId = myFacade.PurchaseTicket(myToken, flight);
                return CreatedAtRoute("GetPurchasedTicketById", new { id = ticketId }, myFacade.GetPurchasedTicketById(myToken, (int)ticketId));
            });
            return result;
        }
        #endregion

        #region Cancel Some Ticket.
        /// <summary>
        /// Cancel Some Ticket.
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("api/customer/cancel/ticket", Name = "CancelTicket")]
        [HttpDelete]
        public IHttpActionResult CancelSomeTicket([FromUri]int ticketId)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Customer> myToken, out LoggedInCustomerFacade myFacade);
                Ticket ticket = myFacade.GetPurchasedTicketById(myToken, ticketId);
                myFacade.CancelTicket(myToken, ticket);
                return Ok($"Your Ticket Number: {ticket.Id} Has Been Successfully Deleted From Your Account.");
            });
            return result;
        }
        #endregion

        #region Update Current Customer.
        /// <summary>
        /// Update Current Customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("updatemydetails", Name = "UpdateCurrentCustomer")]
        [HttpPut]
        public IHttpActionResult UpdateCurrentAirline([FromBody]Customer customer)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Customer> myToken, out LoggedInCustomerFacade myFacade);
                if (myToken.User.Id != customer.Id)
                    throw new IdIsNotMatchException("The ID That Sent Does Not Match To Your ID.");
                else
                {
                    myToken.User = customer;
                    myFacade.MofidyCustomerDetails(myToken);
                }
                return Ok($"Your Details Have Been Successfully Updated.");
            });
            return result;
        }
        #endregion

        #region Change Password For Current Customer.
        /// <summary>
        /// Change Password For Current Customer.
        /// </summary>
        /// <param name="passwords"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Customer))]
        [Route("changemypassword", Name = "ChangePasswordForCurrentCustomer")]
        [HttpPut]
        public IHttpActionResult ChangePasswordForCurrentCustomer([FromBody]ChangePasswordModel passwords)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Customer> myToken, out LoggedInCustomerFacade myFacade);
                myFacade.ChangeMyPassword(myToken, passwords.OldPassword, passwords.NewPassword);
                return Ok($"Your Password Have Been Successfully Changed.");
            });
            return result;
        }
        #endregion

        #region Get Purchased Ticket By Id.
        /// <summary>
        /// Get Purchased Ticket By Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Ticket))]
        [Route("tickets/{id}", Name = "GetPurchasedTicketById")]// i love you baby!!!!
        [HttpGet]
        public IHttpActionResult GetPurchasedTicketById([FromUri]int id)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Customer> myToken, out LoggedInCustomerFacade myFacade);
                Ticket ticket = myFacade.GetPurchasedTicketById(myToken, id);
                return GetSuccessResponse(ticket, "No Ticket With The Received ID Was Found That You Purchased.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get All Flights Of Current Customer.
        /// <summary>
        /// Get All Flights Of Current Customer.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IList<Flight>))]
        [Route("flights", Name = "GetAllFlightsOfCurrentCustomer")]
        [HttpGet]
        public IHttpActionResult GetAllFlightsOfCurrentCustomer()
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Customer> myToken, out LoggedInCustomerFacade myFacade);
                IList<Flight> flights = myFacade.GetAllMyFlights(myToken);
                if (flights.Count < 1)
                    return Content(HttpStatusCode.NoContent, "Your Flights List Is Empty.");
                return Content(HttpStatusCode.OK, flights);
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get All Tickets Of Current Customer.
        /// <summary>
        /// Get All Tickets Of Current Customer.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IList<Ticket>))]
        [Route("tickets", Name = "GetAllTicketsOfCurrentCustomer")]
        [HttpGet]
        public IHttpActionResult GetAllTicketsOfCurrentCustomer()
        {
            IList<Ticket> tickets = new List<Ticket>();
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Customer> myToken, out LoggedInCustomerFacade myFacade);
                tickets = myFacade.GetAllMyTickets(myToken);
                if (tickets.Count < 1)
                    return Content(HttpStatusCode.NoContent, "Your Tickets List Is Empty.");
                return Content(HttpStatusCode.OK, tickets);
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Execute Any Action With All Catch Cases.
        /// <summary>
        /// One Function For All Catch Cases.
        /// </summary>
        /// <param name="myAction"></param>
        /// <returns>IHttpActionResult</returns>
        private IHttpActionResult ExecuteSafe(Func<IHttpActionResult> myFunc)
        {
            try
            {
                return myFunc.Invoke();
            }
            catch (OutOfTicketsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UserNotExistException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (TooLateToCancelTicketException ex)
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
        /// Function For Check If The Token Is Valid And Get Full Token&Facade.
        /// </summary>
        /// <param name="userName" name="token" name="facade"></param>
        private void GetLoginToken(string userName, out LoginToken<Customer> token, out LoggedInCustomerFacade facade)
        {
            token = null;
            facade = null;
            Customer customer = new LoggedInAdministratorFacade().GetCustomerByUserName(FlyingCenterConfig.basicToken, userName);
            if (customer != null)
            {
                token = new LoginToken<Customer> { User = customer };
                facade = new LoggedInCustomerFacade();
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
