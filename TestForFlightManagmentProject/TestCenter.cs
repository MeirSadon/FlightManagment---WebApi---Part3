using FlightManagment___Basic___Part_1;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForFlightManagmentProject
{
    public class TestCenter
    {
        public LoggedInAdministratorFacade adminFacade;
        public LoginToken<Administrator> adminToken;
        public LoggedInAirlineFacade airlineFacade;
        public LoginToken<AirlineCompany> airlineToken;
        public LoggedInCustomerFacade customerFacade;
        public LoginToken<Customer> customerToken;

        #region Ctor For Get Facade And Token For Basic Users.
        /// <summary>
        /// Constractor Of Test Center To Create Facade And Token For Basic Users.
        /// </summary>
        public TestCenter()
        {
            adminFacade = new LoggedInAdministratorFacade();
            adminToken = FlyingCenterConfig.basicToken;

            adminFacade.CreateNewCountry(adminToken, new Country() { Country_Name = "Israel", });


            airlineFacade = new LoggedInAirlineFacade();
            airlineToken = new LoginToken<AirlineCompany> { User = new AirlineCompany(GetRandomNameForTestUsers(), "Airline " + GetRandomNameForTestUsers(), "123", (int)adminFacade.GetCountryByName("Israel").Id) };
            adminFacade.CreateNewAirline(adminToken, airlineToken.User);
            airlineToken.User = adminFacade.GetAirlineByUserName(adminToken, airlineToken.User.User_Name);

            customerFacade = new LoggedInCustomerFacade();
            customerToken = new LoginToken<Customer> { User = new Customer("TestCustomer", "Ben Sadon", "Customer" + "Customer" + GetRandomNameForTestUsers(), "123", "Neria 28", "050", "3317") };
            adminFacade.CreateNewCustomer(adminToken, customerToken.User);
            customerToken.User = adminFacade.GetCustomerByUserName(adminToken, customerToken.User.User_Name);
        }
        #endregion

        #region Create Random Adminisitrator.
        /// <summary>
        /// Function That Given Random Admin For All Test Functions.
        /// </summary>
        /// <returns>Administrator</returns>
        public Administrator CreateRandomAdministrator()
        {
            return new Administrator("Admin: " + GetRandomNameForTestUsers(), "123");
        }
        #endregion

        #region Create Random Company.
        /// <summary>
        /// Function That Given Random Company For All Test Functions.
        /// </summary>
        /// <returns>AirlineCompany</returns>
        public AirlineCompany CreateRandomCompany()
        {
            return new AirlineCompany(GetRandomNameForTestUsers(), "Airline " + GetRandomNameForTestUsers(), "123", (int)adminFacade.GetCountryByName("Israel").Id);
        }
        #endregion

        #region Create Random Customer.
        /// <summary>
        /// Function That Given Random Customer For All Test Functions.
        /// </summary>
        /// <returns>Customer</returns>
        public Customer CreateRandomCustomer()
        {
            return new Customer("Shiran", "Ben Sadon", "Customer " + GetRandomNameForTestUsers(), "123", "Neria 28", "050", "3317");
        }
        #endregion

        #region Create New Random Name.
        /// <summary>
        /// Function To Create Random strings For Names Of Users Frm The Test Functions.
        /// </summary>
        /// <returns>string</returns>
        private string GetRandomNameForTestUsers()
        {
            string randomName = Guid.NewGuid().ToString();
            randomName = randomName.Replace('-'.ToString(), "");
            for (int i = 0; i < 10; i++)
            {
                randomName = randomName.Replace(i.ToString(), i % 2 == 0 ? "" : i.ToString());
            }
            return randomName; // Break Point Here - For Debug.
        }
        #endregion

        #region Remove All Table Of Data Base.
        /// <summary>
        /// Function That Remove All DataBase For All Test Functions.
        /// </summary>
        public void PrepareDBForTests()
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("Delete From Tickets;" +
                    "Delete From Flights;" +
                    "Delete From AirlineCompanies;" +
                    "Delete From Customers;" +
                    "Delete From Countries;" +
                    "Delete From Administrators;" +
                    "Delete From Users", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            adminFacade.CreateNewCountry(adminToken, new Country() { Country_Name = "Israel", });
            adminFacade.CreateNewAirline(adminToken, airlineToken.User);
            adminFacade.CreateNewCustomer(adminToken, customerToken.User);
        }
        #endregion
    }
}
