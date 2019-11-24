using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    interface ILoggedInAirlineFacade
    {
        long CreateFlight(LoginToken<AirlineCompany> token, Flight flight);
        void CancelFlight(LoginToken<AirlineCompany> token, Flight flight);
        void MofidyAirlineDetails(LoginToken<AirlineCompany> token);
        void ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword);
        void UpdateFlight(LoginToken<AirlineCompany> token, Flight flight);
        Ticket GetSoldTicketById(LoginToken<AirlineCompany> token, int id);
        IList<Flight> GetAllFlightsByAirline(LoginToken<AirlineCompany> token);
        IList<Ticket> GetAllTicketsByAirline(LoginToken<AirlineCompany> token);
        bool UserIsValid(LoginToken<AirlineCompany> token);
    }
}
