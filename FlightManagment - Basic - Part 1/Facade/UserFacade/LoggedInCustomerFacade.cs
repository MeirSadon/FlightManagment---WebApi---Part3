using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    public class LoggedInCustomerFacade : AnonymousUserFacade, ILoggedInCustomerFacade
    {

        // Buy New Ticket For Current Customer.
        public long PurchaseTicket(LoginToken<Customer> token, Flight flight)
        {
            long newId = 0;
            if (UserIsValid(token) && flight != null)
            {
                if (flight.Remaining_Tickets > 0)
                {
                    newId = _ticketDAO.Add(new Ticket { Customer_Id = token.User.Id, Flight_Id = flight.Id });
                    flight.Remaining_Tickets--;
                    _flightDAO.Update(flight);
                    _backgroundDAO.AddNewAction(Categories.Customers | Categories.Tickets | Categories.Adds, $"Customer: {token.User.User_Name} Tried To Purchase New Ticket (Flight: {flight.Id}).", true);
                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.Customers | Categories.Tickets | Categories.Adds, $"Customer: {token.User.User_Name} Tried To Purchase New Ticket (Flight: {flight.Id}).", false);
                    throw new OutOfTicketsException($"Sorry But The Tickets Are Over For This Flight (Flight Number: {flight.Id}. From: {flight.Origin_Country_Code} To: {flight.Destination_Country_Code} At: {flight.Departure_Time}.)");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.Customers | Categories.Tickets | Categories.Adds, $"Anonymous User Tried To Purchase New Ticket (Flight: {flight.Id}).", true);
            return newId;
        }
        
        // Cancel Ticket From Current Customer.
        public void CancelTicket(LoginToken<Customer> token, Ticket ticket)
        {
            Flight flight = _flightDAO.GetById((int)ticket.Flight_Id);
            if (UserIsValid(token) && ticket != null)
            {
                if (token.User.Id == ticket.Customer_Id)
                {
                    if (flight.Departure_Time < DateTime.Now)
                        throw new TooLateToCancelTicketException("You Can't Cancel Your Ticket Because The Flight Has Already Begun");
                    if (_flightDAO.GetById((int)ticket.Flight_Id).Departure_Time < DateTime.Now.Add(TimeSpan.FromHours(1)))
                        throw new TooLateToCancelTicketException("You Can't Cancel Your Ticket One Hour Before The Flight");
                    _ticketDAO.Remove(ticket);
                    flight.Remaining_Tickets++;
                    _flightDAO.Update(flight);
                    _backgroundDAO.AddNewAction(Categories.Customers | Categories.Tickets | Categories.Deletions, $"Customer: {token.User.User_Name} Tried To Cancel His Ticket (Flight: {flight.Id}).", true);
                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.Customers | Categories.Tickets | Categories.Deletions, $"Customer: {token.User.User_Name} Tried To Cancel His Ticket (Flight: {flight.Id}).", false);
                    throw new TicketNotMatchException("Sorry But You Can't Cancel Ticket Of Another User.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.Customers | Categories.Tickets | Categories.Deletions, $"Anonymous User Tried To Cancel His Ticket (Flight: {flight.Id}).", false);
        }

        // Change Details Of Current Customer (Without Password).
        public void MofidyCustomerDetails(LoginToken<Customer> token)
        {
                if (UserIsValid(token))
                {
                    User customerUser = _userDAO.GetUserById(token.User.Id);
                    if (customerUser != null)
                    {
                        _userDAO.UpdateUserName(customerUser.User_Name, token.User.User_Name);
                        _customerDAO.Update(token.User);
                        _backgroundDAO.AddNewAction(Categories.Customers | Categories.Updates, $"Customer: {token.User.User_Name} Tried To Update His Details.", true);

                    }
                    else
                    {
                        _backgroundDAO.AddNewAction(Categories.Customers | Categories.Updates, $"Customer: {token.User.User_Name} Tried To Update His Details.", false);
                        throw new UserNotExistException($"Sorry, But '{token.User.User_Name}' Doe's Not Exist.");
                    }
                }
                else
                    _backgroundDAO.AddNewAction(Categories.Customers | Categories.Updates, $"Anonymous User Tried To Update Some Customer.", false);
            }

        // Change Password For Current Customer.
        public void ChangeMyPassword(LoginToken<Customer> token, string oldPassword, string newPassword)
        {
            if (UserIsValid(token))
            {
                User customerUser = _userDAO.GetUserById(token.User.Id);
                if (customerUser != null)
                {
                    if (_userDAO.TryChangePasswordForUser(customerUser, oldPassword, newPassword))
                    {
                        token.User.ChangePassword(newPassword);
                        _backgroundDAO.AddNewAction(Categories.Customers | Categories.Updates, $"Customer: {token.User.User_Name} Tried To Change His Password.", true);
                    }
                    else
                    {
                        _backgroundDAO.AddNewAction(Categories.Customers | Categories.Updates, $"Customer: {token.User.User_Name} Tried To Change His Password.", false);
                        throw new WrongPasswordException("Your Old Password Is Incorrect!");
                    }
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.Customers | Categories.Updates, $"Anonymous User Tried To Change Password For Some Custmer. Id: {token.User.Id} ({token.User.User_Name}).", false);

        }

        // Search Ticket For Current Customer By Id(If The Ticket Purchased By Current Customer).
        public Ticket GetPurchasedTicketById(LoginToken<Customer> token, int id)
        {
            Ticket ticket = new Ticket();
            if (UserIsValid(token))
            {
                ticket = _ticketDAO.GetById(id);
            }
            if (ticket != null && ticket.Customer_Id == token.User.Id)
                return ticket;
            else
                throw new TicketNotMatchException("No Flight Ticket Found In Your List Tickets With The Sent ID.");
        }

        // Search All The Flights For Current Customer.
        public IList<Flight> GetAllMyFlights(LoginToken<Customer> token)
        {
            IList<Flight> flightsByCustomer = new List<Flight>();
            if (UserIsValid(token))
            {
                flightsByCustomer = _flightDAO.GetFlightsByCustomer(token.User);
            }
            return flightsByCustomer;
        }

        // Search All The Tickets For Current Customer.
        public IList<Ticket> GetAllMyTickets(LoginToken<Customer> token)
        {
            IList<Ticket> ticketsByCustomer = new List<Ticket>();
            if (UserIsValid(token))
            {
                ticketsByCustomer = _ticketDAO.GetTicketsByCustomer(token.User);
            }
            return ticketsByCustomer;
        }

        // Check If Customer User That Sent Is Valid.
        public bool UserIsValid(LoginToken<Customer> token)
        {
            if (token != null && token.User != null)
                return true;
            return false;
        }
    }
}
