using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using FlightManagment___Basic___Part_1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestForFlightManagmentProject
{
    [TestClass]
    public class TestForWebApiControllers
    {

        TestCenter tc = new TestCenter();

        #region All Url For Test Client Http Requests.
        // Current Local Host.
        const string localHost = "https://localhost:44368";
        // Url To Get Token For All Controllers.
        private string GetTokenUrl = $"{localHost}/api/Auth";

        //For Administrators Controller Test.
        private string CreateAdminUrl = $"{localHost}/api/administrators/create/admin";
        private string GetAdminUrl = $"{localHost}/api/administrators/search/admin/byusername?username=";

        //For Companies Controller Test.
        private string CreateFlightUrl = $"{localHost}/api/companies/create/flight";
        readonly string formatDate = "yyyy-MM-dd HH:mm:ss";
        private string GetAllFlightsUrl = $"{localHost}/api/companies/flights";

        //For Customers Controller Test.
        private string UpdateCustomerDetailsUrl = $"{localHost}/api/customers/updatemydetails";

        //For Anonymous Controller Test.
        private string GetAllCompaniesUrl = $"{localHost}/api/search/companies";
        #endregion

        // Test Functions.

        #region Test Function For Administrators Controller With.
        /// <summary>
        /// Test Function That Checked Two Functions From Administrator Controller(Create&Get Admin).
        /// </summary>
        [TestMethod]
        public void TestForAdministratorsController()
        {
            tc.PrepareDBForTests();
            User user = new User(FlyingCenterConfig.ADMIN_NAME, FlyingCenterConfig.ADMIN_PASSWORD, UserType.Administrator);
            string token = GetTokenAsync(user);
            Administrator admin = tc.CreateRandomAdministrator();
            CreateAdmin(token, CreateAdminUrl, admin);
            Administrator myAdmin = GetAdminByUserName(token, GetAdminUrl + admin.User_Name);
            Assert.AreEqual(myAdmin.User_Name, admin.User_Name);
        }
        #endregion

        #region Test Function For Companies Controller With.
        /// <summary>
        /// Test Function That Checked Two Functions From Administrator Controller(Create&Get Admin).
        /// </summary>
        [TestMethod]
        public void TestForCompaniesController()
        {
            tc.PrepareDBForTests();
            LoggedInAdministratorFacade adminFacade = new LoggedInAdministratorFacade();
            AirlineCompany company = tc.CreateRandomCompany();
            adminFacade.CreateNewAirline(FlyingCenterConfig.basicToken, company);
            string companyToken = GetTokenAsync(new User(company.User_Name, company.Password, UserType.Airline));
            CreateFlight(companyToken, CreateFlightUrl, new Flight(company.Id, (int)adminFacade.GetCountryByName("Israel").Id, (int)adminFacade.GetCountryByName("Israel").Id, DateTime.Now, DateTime.Now + TimeSpan.FromHours(3), 150));
            IList<Flight> flights = GetAllFlightsForCurrentCompany(companyToken, GetAllFlightsUrl);
            Assert.AreNotEqual(flights, null);
            Assert.AreEqual(flights.Count, 1);
        }
        #endregion

        #region Test Function For Customers Controller (Update Current Customer).
        /// <summary>
        /// Test Function That Checked One Function From Customer Controller(Update Current Customer).
        /// </summary>
        [TestMethod]
        public void TestForCustomersController()
        {
            tc.PrepareDBForTests();
            LoggedInAdministratorFacade adminFacade = new LoggedInAdministratorFacade();
            Customer customer = tc.CreateRandomCustomer();
            customer.Customer_Number = adminFacade.CreateNewCustomer(FlyingCenterConfig.basicToken, customer);
            string customerToken = GetTokenAsync(new User(customer.User_Name, customer.Password, UserType.Customer));
            Customer afterUpdateCustomer = new Customer(customer.Customer_Number, customer.Id, "Changed", "Changed", "Changed", "Changed", "Changed", "Changed", "Changed");
            UpdateDetailsForCustomer(customerToken, UpdateCustomerDetailsUrl, afterUpdateCustomer);
            customer = adminFacade.GetCustomerById(FlyingCenterConfig.basicToken, (int)customer.Id);
            Assert.AreEqual(afterUpdateCustomer.First_Name, customer.First_Name);
        }
        #endregion

        #region Test Function For Anonymous Controller With.
        /// <summary>
        /// Test Function That Checked Get All Companies Function From Anonymous Controller.
        /// </summary>
        [TestMethod]
        public void TestForAnonymousController()
        {
            tc.PrepareDBForTests();
            LoggedInAdministratorFacade adminFacade = new LoggedInAdministratorFacade();
            AirlineCompany company = tc.CreateRandomCompany();
            company.Airline_Number = adminFacade.CreateNewAirline(FlyingCenterConfig.basicToken, company);
            List<AirlineCompany> companies = GetAllCompanies(GetAllCompaniesUrl);
            Assert.IsTrue(companies.Contains(company));
        }
        #endregion

        // Function Related To The Test Functions.

        #region Create New Admin With Client Request (For Test Of Administrators Controller).
        /// <summary>
        /// Function To Create New Admin With HttpClient Request(Post).
        /// </summary>
        /// <param name="token" name="url" name="admin"></param>
        private void CreateAdmin(string token, string url, Administrator admin)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(url);
            string clientAdmin =
                "{" +
                $"\"User_Name\":  \"{admin.User_Name}\"," +
                $" \"Password\": \"{admin.Password}\"" +
                "}";
            HttpContent httpContent = new StringContent(clientAdmin, Encoding.UTF8, "application/json");
            response = client.PostAsync(url, httpContent).Result;
        }
        #endregion

        #region Get Admin By UserName With Client Request (For Test Of Administrators Controller)..
        /// <summary>
        /// Function To Get Admin By UserName With HttpClient Request(Get)
        /// </summary>
        /// <param name="token"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private Administrator GetAdminByUserName(string token, string url)
        {
            Administrator admin = new Administrator();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        if (admin != null)
                            return admin = content.ReadAsAsync<Administrator>().Result;
                    }
                }
            }
            return null;
        }
        #endregion

        #region Create New Flight With Client Request (For Test Of Companies Controller)..
        /// <summary>
        /// Function To Create New Flight With HttpClient Request(Post).
        /// </summary>
        /// <param name="token" name="url" name="flight"></param>
        private void CreateFlight(string token, string url, Flight flight)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(url);
            string clientFlight =
                "{" +
                $" \"AirlineCompany_Id\": \"{flight.AirlineCompany_Id}\"," +
                $" \"Origin_Country_Code\": \"{flight.Origin_Country_Code}\"," +
                $" \"Destination_Country_Code\": \"{flight.Destination_Country_Code}\"," +
                $" \"Departure_Time\": \"{flight.Departure_Time.ToString(formatDate)}\"," +
                $" \"Landing_Time\": \"{flight.Landing_Time.ToString(formatDate)}\"," +
                $" \"Remaining_Tickets\": \"{flight.Remaining_Tickets}\"" +
                "}";
            HttpContent httpContent = new StringContent(clientFlight, Encoding.UTF8, "application/json");
            response = client.PostAsync(url, httpContent).Result;
        }
        #endregion

        #region Get All Flight For Current Comapny With Http Client Request (For Test Of Companies Controller).
        /// <summary>
        /// Function To Get All Flight For Current Comapny With HttpClien Request(Get).
        /// </summary>
        /// <param name="token" name="url"></param>
        /// <returns>IList</returns>
        private IList<Flight> GetAllFlightsForCurrentCompany(string token, string url)
        {
            IList<Flight> flights = new List<Flight>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        flights = content.ReadAsAsync<IList<Flight>>().Result;
                    }
                }
            }
            return flights;
        }
        #endregion

        #region Update Current Customer(By Id) With Http Clint Requet (For Test Of Customers Controller).
        /// <summary>
        /// Function For Current Customer(By Id) With HttpClient Request(Get).
        /// </summary>
        /// <param name="token"name=url" name="customer></param>
        private void UpdateDetailsForCustomer(string token, string url, Customer customer)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(url);
            string clientCustomer =
                "{" +
                $"\"Id\": \"{customer.Id}\"," +
                $" \"Customer_Number\": \"{customer.Customer_Number}\"," +
                $"\"First_Name\": \"{customer.First_Name}\"," +
                $" \"Last_Name\": \"{customer.Last_Name}\"," +
                $"\"User_Name\": \"{customer.User_Name}\"," +
                $" \"Password\": \"{customer.Password}\"," +
                $"\"Address\": \"{customer.Address}\"," +
                $" \"Phone_No\": \"{customer.Phone_No}\"," +
                $"\"Credit_Card_Number\": \"{customer.Credit_Card_Number}\"" +
                "}";
            HttpContent httpContent = new StringContent(clientCustomer, Encoding.UTF8, "application/json");
            response = client.PutAsync(url, httpContent).Result;
        }
        #endregion

        #region Get All Companies With HttpClient Request (For Test Of Anonymous Controller).
        /// <summary>
        /// Function To Get All Companies With HttpClien Request(Get).
        /// </summary>
        /// <param name="token" name="url"></param>
        /// <returns>IList</returns>
        private List<AirlineCompany> GetAllCompanies(string url)
        {
            List<AirlineCompany> companies = new List<AirlineCompany>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        companies = content.ReadAsAsync<List<AirlineCompany>>().Result;
                    }
                }
            }
            return companies;
        }
        #endregion

        #region Get Token From Server With Client Request (For Any Test Function).
        /// <summary>
        /// Function That Recieve jwt from the server with client request and remove unnecessary characters.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>string</returns>
        private string GetTokenAsync(User user)
        {
            string token = "";
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            client.BaseAddress = new Uri(GetTokenUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string userDetails =
                "{" +
                $"\"User_Name\":  \"{user.User_Name}\"," +
                $" \"Password\": \"{user.Password}\"" +
                "}";
            HttpContent httpContent = new StringContent(userDetails, Encoding.UTF8, "application/json");
            response = client.PostAsync(GetTokenUrl, httpContent).Result;
            token = response.Content.ReadAsStringAsync().Result;
            return FixApiResponseString(token);
        }
        #endregion

        #region Remove Unnecessary Characters (For Any Test Function).
        /// <summary>
        /// Function That Recieve string(From Client Response) And Remove From Him All Unnecessary Characters
        /// </summary>
        /// <param name="input"></param>
        /// <returns>string</returns>
        private string FixApiResponseString(string input)
        {
            input = input.Replace("\\", string.Empty);
            input = input.Trim('"');
            return input;
        }
        #endregion

    }
}
