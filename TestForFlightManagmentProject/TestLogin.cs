using System;
using FlightManagment___Basic___Part_1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestForFlightManagmentProject
{
    [TestClass]
    public class TestLogin
    {
        /* ========   All Tests ========

           1. GetPasswordExceptionForDefaultAdmin -- WrongPasswordWhenTryLoginAsDefaultAdmin.
           2. GetPasswordExceptionForDAOAdmin     -- WrongPasswordWhenTryLoginAsAdmin.
           3. LoginSuccessfulyForDefaultAdmin     -- LoginSuccesfullyAsDefaultAdmin.
           4. LoginSuccessfulyForDAOAdmin         -- LoginSuccesfullyAsDAOAdmin.
           5. GetPasswordExceptionForAirline      -- WrongPasswordWhenTryLoginAsAirline.
           6. LoginSuccessfulyForAirline          -- LoginSuccesfullyAsAirline.
           7. GetPasswordExceptionForCustomer     -- WrongPasswordWhenTryLoginAsCustomer.
           8. LoginSuccessfulyForCustomer         -- LoginSuccesfullyAsCustomer.

           ======= All Tests ======= */

        TestCenter tc = new TestCenter();
        // Supposed To Get Password Exception For Login To Default Admin.
        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void WrongPasswordWhenTryLoginAsDefaultAdmin()
        {
            FlyingCenterSystem.GetUserAndFacade(FlyingCenterConfig.ADMIN_NAME, "WrongPassword!", out ILogin token, out FacadeBase facade);
        }

        // Supposed To Get Password Exception For Login To DAO Admin.
        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void WrongPasswordWhenTryLoginAsAdmin()
        {
            tc.PrepareDBForTests();
            Administrator admin = tc.CreateRandomAdministrator();
            admin.Admin_Number = tc.adminFacade.CreateNewAdmin(tc.adminToken, admin);
            FlyingCenterSystem.GetUserAndFacade(admin.User_Name, "ErrorPassword", out ILogin token, out FacadeBase facade);
        }

        // Login Succesfully As Dfault Admin.
        [TestMethod]
        public void LoginSuccesfullyAsDefaultAdmin()
        {
            FlyingCenterSystem.GetUserAndFacade(FlyingCenterConfig.ADMIN_NAME, FlyingCenterConfig.ADMIN_PASSWORD, out ILogin token, out FacadeBase facade);
            LoginToken<Administrator> newAdminToken = token as LoginToken<Administrator>;
            Assert.AreNotEqual(newAdminToken, null);
        }

        // Login Succesfully As DAO Admin.
        [TestMethod]
        public void LoginSuccesfullyAsDAOAdmin()
        {
            tc.PrepareDBForTests();
            Administrator admin = tc.CreateRandomAdministrator();
            admin.Admin_Number = tc.adminFacade.CreateNewAdmin(tc.adminToken, admin);
            FlyingCenterSystem.GetUserAndFacade(admin.User_Name, admin.Password, out ILogin token, out FacadeBase facade);
            LoginToken<Administrator> newAdminToken = token as LoginToken<Administrator>;
            LoggedInAdministratorFacade newAdminFacade = facade as LoggedInAdministratorFacade;
            Assert.AreNotEqual(newAdminToken, null);
            Assert.AreNotEqual(newAdminFacade, null);
        }

        // Supposed To Get Password Exception For Login To Airline.
        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void WrongPasswordWhenTryLoginAsAirline()
        {
            tc.PrepareDBForTests();
            AirlineCompany airline = tc.CreateRandomCompany();
            airline.Airline_Number = tc.adminFacade.CreateNewAirline(tc.adminToken, airline);
            FlyingCenterSystem.GetUserAndFacade(airline.User_Name, "ErrorPassword", out ILogin token, out FacadeBase facade);
        }

        // Login Succesfully As Airline.
        [TestMethod]
        public void LoginSuccesfullyAsAirline()
        {
            tc.PrepareDBForTests();
            AirlineCompany airline = tc.CreateRandomCompany();
            airline.Airline_Number = tc.adminFacade.CreateNewAirline(tc.adminToken, airline);
            FlyingCenterSystem.GetUserAndFacade(airline.User_Name, "123", out ILogin token, out FacadeBase facade);
            LoginToken<AirlineCompany> newAirlineToken = token as LoginToken<AirlineCompany>;
            LoggedInAirlineFacade newAirlineFacade = facade as LoggedInAirlineFacade;
            Assert.AreNotEqual(newAirlineToken, null);
            Assert.AreNotEqual(newAirlineFacade, null);
        }

        // Supposed To Get Password Exception For Login To Customer.
        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void WrongPasswordWhenTryLoginAsCustomer()
        {
            tc.PrepareDBForTests();
            Customer customer = tc.CreateRandomCustomer();
            tc.adminFacade.CreateNewCustomer(tc.adminToken, customer);
            FlyingCenterSystem.GetUserAndFacade(customer.User_Name, "ErrorPassword", out ILogin token, out FacadeBase facade);
        }

        // Login Succesfully As Customer.
        [TestMethod]
        public void LoginSuccesfullyAsCustomer()
        {
            tc.PrepareDBForTests();
            Customer customer = tc.CreateRandomCustomer();
            tc.adminFacade.CreateNewCustomer(tc.adminToken, customer);
            FlyingCenterSystem.GetUserAndFacade(customer.User_Name, "123", out ILogin token, out FacadeBase facade);
            LoggedInCustomerFacade newCustomerFacade = facade as LoggedInCustomerFacade;
            Assert.AreNotEqual(customer, null);
            Assert.AreNotEqual(newCustomerFacade, null);
        }

        [TestMethod]
        public void GetAnonymousUserFacade()
        {
            tc.PrepareDBForTests();
            Customer customer = tc.CreateRandomCustomer();
            tc.adminFacade.CreateNewCustomer(tc.adminToken, customer);
            FlyingCenterSystem.GetUserAndFacade(customer.User_Name, "123", out ILogin token, out FacadeBase facade);
            LoginToken<IUser> myToken = token as LoginToken<IUser>;
            Assert.AreEqual(myToken, null);
            Assert.AreNotEqual(facade, null);
        }
    }
}
