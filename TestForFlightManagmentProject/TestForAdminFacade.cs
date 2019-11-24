using System;
using FlightManagment___Basic___Part_1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestForFlightManagmentProject
{
    [TestClass]
    public class TestForAdminFacade
    {
        /* ========   All Tests ========

           1. CreateNewAdmin                  -- "TestLogin Class" (LoginSuccesfullyAsDAOAdmin).
           2. CreateNewAirline                -- "TestLogin Class" (LoginSuccesfullyAsAirline).
           3. CreateNewCustomer               -- "TestLogin Class" (LoginSuccesfullyAsCustomer).
           4. CreateNewCountry                -- "TestCenter" (PrepareDBForTests).
           5. RemoveAdministrator             -- RemoveAdministratorSuccessfully + TryRemoveAdministratorUserThatNotExist.
           6. RemoveAirline                   -- RemoveAirlineSuccessfully + TryRemoveAirlineUserThatNotExist.
           7. RemoveCustomer                  -- RemoveCustomerSuccessfully + TryRemoveCustomerUserThatNotExist.
           8. RemoveCountry                   -- RemoveCustomerSuccessfully + TryRemoveCustomerUserThatNotExist.
           9. UpdateAirlineDetails            -- UpdateAirline.
           10. UpdateCustomerDetails          -- UpdateCustomer.
           11. UpdateCountryDetails           -- UpdateCountry.
           12. ChangeMyPassword               -- TryChangePasswordForAdministrator + WrongPasswordWhenTryChangePasswordForCentralAdmin + WrongPasswordWhenTryChangePasswordForSomeAdmin.
           13. ForceChangePasswordForAirline  -- ChangePasswordForSomeAirline.
           14. ForceChangePasswordForCustomer -- ChangePasswordForSomeCustomer.
           15. GetAdminByUserName             -- GetAdminByUserName.
           16. GetAdminById                   -- GetAdminById.
           17. GetAirlineById                 -- GetAirlineById.
           18. GetAirlineByUserName           -- GetAirlineByUserName.
           19. GetCustomerByUserName          -- GetCustomerByUserName.
           20. GetCustomerById                -- GetCustomerById.
           21. GetAllCustomers                -- GetAllCustomers.


           ========   All Tests ======== */

        private TestCenter tc = new TestCenter();



        // ===== Remove Successfully =====/

        // Remove Administrator Successfully.
        [TestMethod]
        public void RemoveAdministratorSuccessfully()
        {
            tc.PrepareDBForTests();
            Administrator admin = tc.CreateRandomAdministrator();
            admin.Admin_Number = tc.adminFacade.CreateNewAdmin(tc.adminToken, admin);
            tc.adminFacade.RemoveAdministrator(tc.adminToken, admin);
            Assert.AreEqual(tc.adminFacade.GetAdminByUserName(tc.adminToken, admin.User_Name).Id, 0);
        }

        // Remove Airline Successfully.
        [TestMethod]
        public void RemoveAirlineSuccessfully()
        {
            tc.PrepareDBForTests();
            AirlineCompany airline = tc.CreateRandomCompany();
            airline.Airline_Number = tc.adminFacade.CreateNewAirline(tc.adminToken, airline);
            tc.adminFacade.RemoveAirline(tc.adminToken, airline);
            Assert.AreEqual(tc.adminFacade.GetAdminByUserName(tc.adminToken, airline.User_Name).Id, 0);
        }

        // Remove Customer Successfully.
        [TestMethod]
        public void RemoveCustomerSuccessfully()
        {
            tc.PrepareDBForTests();
            Customer customer = tc.CreateRandomCustomer();
            customer.Customer_Number = tc.adminFacade.CreateNewCustomer(tc.adminToken, customer);
            tc.adminFacade.RemoveCustomer(tc.adminToken, customer);
            Assert.AreEqual(tc.adminFacade.GetCustomerByUserName(tc.adminToken, customer.User_Name).Id, 0);
        }

        // Remove Country Successfully.
        [TestMethod]
        public void RemoveCountrySuccessfully()
        {
            tc.PrepareDBForTests();
            tc.adminFacade.RemoveCountry(tc.adminToken, tc.adminFacade.GetCountryByName("Israel"));
            Assert.AreEqual(tc.adminFacade.GetCountryByName("Israel").Id, 0);
        }


        // ===== Get "UserNotExist" When Try Remove =====//

        // Supposed To Get "UserNotExistException" When Try Remove Administrator.
        [TestMethod]
        [ExpectedException(typeof(UserNotExistException))]
        public void TryRemoveAdministratorUserThatNotExist()
        {
            tc.PrepareDBForTests();
            Administrator admin = tc.CreateRandomAdministrator();
            tc.adminFacade.RemoveAdministrator(tc.adminToken, admin);
        }

        // Supposed To Get "UserNotExistException" When Try Remove Airline.
        [TestMethod]
        [ExpectedException(typeof(UserNotExistException))]
        public void TryRemoveAirlineUserThatNotExist()
        {
            tc.PrepareDBForTests();
            AirlineCompany airline = tc.CreateRandomCompany();
            tc.adminFacade.RemoveAirline(tc.adminToken, airline);
        }

        // Supposed To Get "UserNotExistException" When Try Remove Customer.
        [TestMethod]
        [ExpectedException(typeof(UserNotExistException))]
        public void TryRemoveCustomerUserThatNotExist()
        {
            tc.PrepareDBForTests();
            Customer customer = tc.CreateRandomCustomer();
            tc.adminFacade.RemoveCustomer(tc.adminToken, customer);
        }

        // Supposed To Get "UserNotExistException" When Try Remove Country.
        [TestMethod]
        [ExpectedException(typeof(UserNotExistException))]
        public void TryRemoveCountryUserThatNotExist()
        {
            tc.PrepareDBForTests();
            Country country = new Country("USA");
            tc.adminFacade.RemoveCountry(tc.adminToken, country);
        }

        // ===== Update Details ===== //

        // Update Details For Airline Company.
        [TestMethod]
        public void UpdateAirline()
        {
            tc.PrepareDBForTests();
            AirlineCompany airline = tc.CreateRandomCompany();
            airline.Airline_Number = tc.adminFacade.CreateNewAirline(tc.adminToken, airline);
            airline.Airline_Name = "CHANGED!";
            tc.adminFacade.UpdateAirlineDetails(tc.adminToken, airline);
            Assert.AreEqual(tc.adminFacade.GetAirlineByUserName(tc.adminToken, airline.User_Name).Airline_Name, "CHANGED!");
        }

        // Update Details For Customer.
        [TestMethod]
        public void UpdateCustomer()
        {
            tc.PrepareDBForTests();
            Customer customer = tc.CreateRandomCustomer();
            customer.Customer_Number = tc.adminFacade.CreateNewCustomer(tc.adminToken, customer);
            customer = tc.adminFacade.GetCustomerByUserName(tc.adminToken, customer.User_Name);
            customer.First_Name = "CHANGED!";
            tc.adminFacade.UpdateCustomerDetails(tc.adminToken, customer);
            Assert.AreEqual(tc.adminFacade.GetCustomerByUserName(tc.adminToken, customer.User_Name).First_Name, "CHANGED!");
        }

        // Update Details For Country.
        [TestMethod]
        public void UpdateCountry()
        {
            tc.PrepareDBForTests();
            Country country = new Country("USA");
            country.Id = tc.adminFacade.CreateNewCountry(tc.adminToken, country);
            country.Country_Name = "China";
            tc.adminFacade.UpdateCountryDetails(tc.adminToken, country);
            Assert.AreEqual(tc.adminFacade.GetCountryByName(country.Country_Name).Country_Name, "China");
        }


        //  Change Password Succesfully =====//

        // Try Change Password Successfuly For Administrator.
        [TestMethod]
        public void TryChangePasswordForAdministrator()
        {
            tc.PrepareDBForTests();
            Administrator admin = tc.CreateRandomAdministrator();
            tc.adminFacade.CreateNewAdmin(tc.adminToken, admin);
            FlyingCenterSystem.GetUserAndFacade(admin.User_Name, admin.Password, out ILogin token, out FacadeBase facade);
            LoginToken<Administrator> newToken = token as LoginToken<Administrator>;
            LoggedInAdministratorFacade newFacade = facade as LoggedInAdministratorFacade;
            newFacade.ChangeMyPassword(newToken, "123".ToUpper(), "newPassword".ToUpper());
            Assert.AreEqual(newToken.User.Password.ToUpper(), "newPassword".ToUpper());
        }

        // Change Password Successfuly For Airline Company.
        [TestMethod]
        public void ChangePasswordForSomeAirline()
        {
            tc.PrepareDBForTests();
            AirlineCompany airline = tc.CreateRandomCompany();
            airline.Airline_Number = tc.adminFacade.CreateNewAirline(tc.adminToken, airline);
            tc.adminFacade.ForceChangePasswordForAirline(tc.adminToken, airline, "newPassword".ToUpper());
            Assert.AreEqual(tc.adminFacade.GetAirlineByUserName(tc.adminToken, airline.User_Name).Password, "newPassword".ToUpper());
        }

        // Change Password Successfuly For Customer.
        [TestMethod]
        public void ChangePasswordForSomeCustomer()
        {
            tc.PrepareDBForTests();
            Customer customer = tc.CreateRandomCustomer();
            customer.Customer_Number = tc.adminFacade.CreateNewCustomer(tc.adminToken, customer);
            tc.adminFacade.ForceChangePasswordForCustomer(tc.adminToken, customer, "newPassword".ToUpper());
            Assert.AreEqual(tc.adminFacade.GetCustomerByUserName(tc.adminToken, customer.User_Name).Password, "newPassword".ToUpper());
        }

        // ===== Get "WrongPasswordException" When Try Change Password =====//

        // Supposed To Get "WrongPasswordException" When Try Change Password For Central Administrator.
        [TestMethod]
        [ExpectedException(typeof(CentralAdministratorException))]
        public void WrongPasswordWhenTryChangePasswordForCentralAdmin()
        {
            tc.PrepareDBForTests();
            tc.adminFacade.ChangeMyPassword(tc.adminToken, "123456", "newPassword");
        }

        // Supposed To Get "WrongPasswordException" When Try Change Password For Some Administrator.
        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void WrongPasswordWhenTryChangePasswordForSomeAdmin()
        {
            tc.PrepareDBForTests();
            Administrator admin = tc.CreateRandomAdministrator();
            tc.adminFacade.CreateNewAdmin(tc.adminToken, admin);
            FlyingCenterSystem.GetUserAndFacade(admin.User_Name, admin.Password, out ILogin token, out FacadeBase facade);
            LoginToken<Administrator> newToken = token as LoginToken<Administrator>;
            LoggedInAdministratorFacade newFacade = facade as LoggedInAdministratorFacade;
            tc.adminFacade.ChangeMyPassword(newToken, "123345", "newPassword");
        }


        // ===== Search Functions =====//

        // Search Some Admin By User Name.
        [TestMethod]
        public void GetAdminByUserName()
        {
            tc.PrepareDBForTests();
            Administrator admin = tc.CreateRandomAdministrator();
            admin.Admin_Number = tc.adminFacade.CreateNewAdmin(tc.adminToken, admin);
            Assert.AreNotEqual(tc.adminFacade.GetAdminByUserName(tc.adminToken, admin.User_Name), null);
        }

        // Search Some Admin By Id.
        [TestMethod]
        public void GetAdminById()
        {
            tc.PrepareDBForTests();
            Administrator admin = tc.CreateRandomAdministrator();
            admin.Admin_Number = tc.adminFacade.CreateNewAdmin(tc.adminToken, admin);
            Assert.AreNotEqual(tc.adminFacade.GetAdminById(tc.adminToken, (int)admin.Id), null);
        }

        // Search Some Airline By Id.
        [TestMethod]
        public void GetAirlineById()
        {
            tc.PrepareDBForTests();
            AirlineCompany airline = tc.CreateRandomCompany();
            airline.Airline_Number = tc.adminFacade.CreateNewAirline(tc.adminToken, airline);
            Assert.AreNotEqual(tc.adminFacade.GetAirlineById(tc.adminToken, (int)airline.Id), null);
        }

        // Search Some Customer By User Name.
        [TestMethod]
        public void GetAirlineByUserName()
        {
            tc.PrepareDBForTests();
            AirlineCompany airline = tc.CreateRandomCompany();
            airline.Airline_Number = tc.adminFacade.CreateNewAirline(tc.adminToken, airline);
            Assert.AreNotEqual(tc.adminFacade.GetAirlineByUserName(tc.adminToken, airline.User_Name), null);
        }

        // Search Some Customer By User Name.
        [TestMethod]
        public void GetCustomerByUserName()
        {
            tc.PrepareDBForTests();
            Customer customer = tc.CreateRandomCustomer();
            customer.Customer_Number = tc.adminFacade.CreateNewCustomer(tc.adminToken, customer);
            Assert.AreNotEqual(tc.adminFacade.GetCustomerByUserName(tc.adminToken, customer.User_Name), null);
        }

        // Search Some Customer By Id.
        [TestMethod]
        public void GetCustomerById()
        {
            tc.PrepareDBForTests();
            Customer customer = tc.CreateRandomCustomer();
            customer.Customer_Number = tc.adminFacade.CreateNewCustomer(tc.adminToken, customer);
            Assert.AreNotEqual(tc.adminFacade.GetCustomerById(tc.adminToken, (int)customer.Id), null);
        }

        // Get All Customers.
        [TestMethod]
        public void GetAllCustomers()
        {
            tc.PrepareDBForTests();
            Customer customer = tc.CreateRandomCustomer();
            customer.Customer_Number = tc.adminFacade.CreateNewCustomer(tc.adminToken, customer);
            Assert.AreEqual(tc.adminFacade.GetAllCustomers(tc.adminToken).Count, 2);
        }
    }
}
