using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    public class LoggedInAirlineFacade : AnonymousUserFacade, ILoggedInAirlineFacade
    {

        // Create New Flight For Current Airline.
        public long CreateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            long newId = 0;
            if (UserIsValid(token) && flight != null)
            {
                if (token.User.Id != flight.AirlineCompany_Id)
                {
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Flights | Categories.Adds, $"Company: {token.User.User_Name} Tried To Add New Flight (Flight: {flight.Id}).", false);
                    throw new FlightNotMatchException("The ID Of Your New Flight Isn't Match To Your Company ID");
                }
                if (flight.Departure_Time >= flight.Landing_Time)
                {
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Flights | Categories.Adds, $"Company: {token.User.User_Name} Tried To Add New Flight (Flight: {flight.Id}).", false);
                    throw new DepartureTimeTooLateException("Departue Time Must Be Earlier Than Landing Time");
                }
                newId = _flightDAO.Add(flight);
                _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Flights | Categories.Adds, $"Company: {token.User.User_Name} Tried To Add New Flight (Flight: {flight.Id}).", true);
            }
            else
                _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Flights | Categories.Adds, $"Anonymous User Tried To Add Some Flight.", false);
            return newId;
        }

        // Cancel Flight From This Airline.
        public void CancelFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            if (UserIsValid(token) && flight != null)
            {
                if (token.User.Id == flight.AirlineCompany_Id)
                {
                    _flightDAO.Remove(flight);
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Flights | Categories.Deletions, $"Company: {token.User.User_Name} Tried To Cancel Flight From Her Flights List.", true);
                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Flights | Categories.Deletions, $"Company: {token.User.User_Name} Tried To Cancel Flight From Some Flights List.", false);
                    throw new TicketNotMatchException("Sorry But You Can't Cancel Flight Of Another Comapny.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Flights | Categories.Deletions, $"Anonymous User Tried To Cancel Some Flight.", false);
        }

        // Change Details Of Current Airline (Without Password).
        public void MofidyAirlineDetails(LoginToken<AirlineCompany> token)
        {
            if (UserIsValid(token))
            {
                User airlineUser = _userDAO.GetUserById(token.User.Id);
                if (airlineUser != null)
                {
                    _userDAO.UpdateUserName(airlineUser.User_Name, token.User.User_Name);
                    _airlineDAO.Update(token.User);
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Updates, $"Company: {token.User.User_Name} Tried To Update Her Details.", true);

                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Updates, $"Company: {token.User.User_Name} Tried To Update Her Details.", false);
                    throw new UserNotExistException($"Sorry, But '{token.User.User_Name}' Does Not Exist.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Updates, $"Anonymous User Tried To Update Some Airline Company.", false);

        }

        // Change Password For Current Company.
        public void ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword)
        {
            if (UserIsValid(token))
            {
                User airlineUser = _userDAO.GetUserById(token.User.Id);
                if (airlineUser != null)
                {
                    if (_userDAO.TryChangePasswordForUser(airlineUser, oldPassword, newPassword))
                    {
                        token.User.ChangePassword(newPassword);
                        _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Updates, $"{token.User.User_Name} Company Tried To Change Her Password.", true);
                    }
                    else
                    {
                        _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Updates, $"{token.User.User_Name} Company Tried To Change Her Password.", false);
                        throw new WrongPasswordException("Your Old Password Is Incorrect!");
                    }
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Updates, $"Anonymous User Tried To Change Password For Some Airline Company. Id: {token.User.Id} ({token.User.User_Name}).", false);
        }

        //Update Flight Of Current Airline.
        public void UpdateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            if (UserIsValid(token) && flight != null)
            {
                if (token.User.Id == flight.AirlineCompany_Id)
                {
                    _flightDAO.Update(flight);
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Flights | Categories.Updates, $"Comapny: {token.User.User_Name} Tried To UPDATE Some Flight From Her Flights List.", true);
                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Flights | Categories.Updates, $"Comapny: {token.User.User_Name} Tried To UPDATE Some Flight Of Another Company.", false);
                    throw new FlightNotMatchException($"Sorry, But You Can Update Only Flights Of This Company.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Flights | Categories.Updates, $"Anonymous User Tried To UPDATE Some Flight From Some Company.", false);
        }

        // Search Ticket For Current Airline By Id(If The Ticket Sold By Current Airline).
        public Ticket GetSoldTicketById(LoginToken<AirlineCompany> token, int id)
        {
            Ticket ticket = new Ticket();
            if (UserIsValid(token))
            {
                ticket = _ticketDAO.GetById(id);
                Flight flight = GetFlightById((int)ticket.Flight_Id);
                if (flight == new Flight() || flight.AirlineCompany_Id != token.User.Id)
                    throw new TicketNotMatchException("No Flight Ticket Was Found That Sold By Current Company.");
                else
                    return ticket;
            }
            return ticket;

        }

        // Search All Flights Of Current Airline.
        public IList<Flight> GetAllFlightsByAirline(LoginToken<AirlineCompany> token)
        {
            List<Flight> flights = new List<Flight>();
            if (UserIsValid(token))
            {
                flights = _flightDAO.GetFlightsByAirlineCompany(token.User).ToList();
            }
            return flights;
        }

        // Search All Tickets Of Current Airline.
        public IList<Ticket> GetAllTicketsByAirline(LoginToken<AirlineCompany> token)
        {
            List<Ticket> tickets = new List<Ticket>();
            if (UserIsValid(token))
            {
                tickets = _ticketDAO.GetTicketsByAirlineComapny(token.User).ToList();
            }
            return tickets;
        }

        // Check If User Admin That Sent Is Valid.
        public bool UserIsValid(LoginToken<AirlineCompany> token)
        {
            if (token != null && token.User != null)
                return true;
            return false;
        }
    }
}
