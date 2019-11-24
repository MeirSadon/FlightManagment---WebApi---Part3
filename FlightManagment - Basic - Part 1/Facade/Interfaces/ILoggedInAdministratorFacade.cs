using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    interface ILoggedInAdministratorFacade
    {
        long CreateNewAdmin(LoginToken<Administrator> token, Administrator admin);
        long CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline);
        long CreateNewCustomer(LoginToken<Administrator> token, Customer customer);
        long CreateNewCountry(LoginToken<Administrator> token, Country country);

        void RemoveAdministrator(LoginToken<Administrator> token, Administrator admin);
        void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline);
        void RemoveCustomer(LoginToken<Administrator> token, Customer customer);
        void RemoveCountry(LoginToken<Administrator> token, Country country);

        void UpdateAdminDetails(LoginToken<Administrator> token, Administrator admin);
        void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany customer);
        void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer);
        void UpdateCountryDetails(LoginToken<Administrator> token, Country country);

        void ChangeMyPassword(LoginToken<Administrator> token, string oldPassword, string newPassword);
        void ForceChangePasswordForAirline(LoginToken<Administrator> token, AirlineCompany airline, string newPassword);
        void ForceChangePasswordForCustomer(LoginToken<Administrator> token, Customer customer, string newPassword);

        Administrator GetAdminById(LoginToken<Administrator> token, int id);
        Administrator GetAdminByUserName(LoginToken<Administrator> token, string userName);
        AirlineCompany GetAirlineById(LoginToken<Administrator> token, int id);
        AirlineCompany GetAirlineByUserName(LoginToken<Administrator> token, string userName);
        Customer GetCustomerById(LoginToken<Administrator> token, int id);
        Customer GetCustomerByUserName(LoginToken<Administrator> token, string userName);
        IList<Customer> GetAllCustomers(LoginToken<Administrator> token);

        bool UserIsValid(LoginToken<Administrator> token);
    }
}
