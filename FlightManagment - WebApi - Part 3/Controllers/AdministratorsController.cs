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
    /// Controller For Administrator User.
    /// </summary>
    [Authorize(Roles = "Administrator")]
    [RoutePrefix("api/administrators")]
    public class AdministratorsController : ApiController
    {
        #region Create New Administrator.
        /// <summary>
        /// Create New Admin.
        /// </summary>
        /// <param name="admin"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Administrator))]
        [Route("create/admin", Name = "CreateNewAdmin")]
        [HttpPost]
        public IHttpActionResult CreateNewAdmin([FromBody]Administrator admin)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                admin.Admin_Number = myFacade.CreateNewAdmin(myToken, admin);
                return CreatedAtRoute("GetAdministratorById", new { id = admin.Id }, admin);
            });
            return result; // for debug - break point here        
        }
        #endregion

        #region Create New Company.
        /// <summary>
        /// Create New Company.
        /// </summary>
        /// <param name="company"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(AirlineCompany))]
        [Route("create/company", Name = "CreateNewAirline")]
        [HttpPost]
        public IHttpActionResult CreateNewAirlne([FromBody]AirlineCompany company)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                company.Airline_Number = myFacade.CreateNewAirline(myToken, company);
                return CreatedAtRoute("GetCompanyById", new { id = company.Id }, company);
            });
            return result; // for debug - break point here  
        }
        #endregion

        #region Create New Customer.
        /// <summary>
        /// Create New Customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Customer))]
        [Route("create/customer", Name = "CreateNewCustomer")]
        [HttpPost]
        public IHttpActionResult CreateNewCustomer([FromBody]Customer customer)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                customer.Customer_Number = myFacade.CreateNewCustomer(myToken, customer);
                return CreatedAtRoute("GetCustomerById", new { id = customer.Id }, customer);
            });
            return result; // for debug - break point here  
        }
        #endregion

        #region Create New Country.
        /// <summary>
        /// Create New Country.
        /// </summary>
        /// <param name="Country"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Country))]
        [Route("create/country", Name = "CreateNewCountry")]
        [HttpPost]
        public IHttpActionResult CreateNewCountry([FromBody]Country country)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                country.Id = myFacade.CreateNewCountry(myToken, country);
                return CreatedAtRoute("GetCountryById", new { id = country.Id }, country);
            });
            return result; // for debug - break point here  
        }
        #endregion

        #region Remove Some Admin.
        /// <summary>
        /// Remove Admin.
        /// </summary>
        /// <param name="admin"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("remove/admin/{id}", Name = "RemoveAdmin")]
        [HttpDelete]
        public IHttpActionResult RemoveAdministrator([FromUri]int id)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                Administrator admin = myFacade.GetAdminById(myToken, id);
                myFacade.RemoveAdministrator(myToken, admin);
                return Ok($"Admin Number: {id} Deleted Successfully.");
            });
            return result; // for debug - break point here  
        }
        #endregion

        #region Remove Some Airline.
        /// <summary>
        /// Remove Airline.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("remove/company/{id}", Name = "RemoveAirline")]
        [HttpDelete]
        public IHttpActionResult RemoveCompany([FromUri]int id)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                AirlineCompany airline = myFacade.GetAirlineById(myToken, id);
                myFacade.RemoveAirline(myToken, airline);
                return Ok($"Company Number: {id} Deleted Successfully.");
            });
            return result; // for debug - break point here  
        }
        #endregion

        #region Remove Some Customer.
        /// <summary>
        /// Remove Customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("remove/customer/{id}", Name = "RemoveCustomer")]
        [HttpDelete]
        public IHttpActionResult RemoveCustomer([FromUri]int id)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                Customer customer = myFacade.GetCustomerById(myToken, id);
                myFacade.RemoveCustomer(myToken, customer);
                return Ok($"Customer Number: {id} Deleted Successfully.");
            });
            return result; // for debug - break point here  

        }
        #endregion

        #region Remove Some Country.
        /// <summary>
        /// Remove Country.
        /// </summary>
        /// <param name="country"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("remove/country/{id}", Name = "RemoveCountry")]
        [HttpDelete]
        public IHttpActionResult RemoveCountry([FromUri]int id)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                Country country = myFacade.GetCountryById(id);
                myFacade.RemoveCountry(myToken, country);
                return Ok($"Country Number: {id} Deleted Successfully.");
            });
            return result; // for debug - break point here  
        }
        #endregion

        #region Update Some Admin Details.
        /// <summary>
        /// Update Some Admin Details.
        /// </summary>
        /// <param name="admin"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("update/admin/{id}", Name = "UpdateAdmin")]
        [HttpPut]
        public IHttpActionResult UpdateAdmin([FromUri]int id, [FromBody]Administrator admin)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                Administrator adminForUpdate = myFacade.GetAdminById(myToken, id);
                if (admin.Id != adminForUpdate.Id)
                    throw new IdIsNotMatchException("Your Administrator ID Does Not Match ID That Found In Your URL.");
                else
                {
                    adminForUpdate = admin;
                    myFacade.UpdateAdminDetails(myToken, adminForUpdate);
                }
                return Ok($"Admin Number: {admin.Id}) Updated Successfully.");
            });
            return result; // for debug - break point here        
        }
        #endregion

        #region Update Some Airline Details.
        /// <summary>
        /// Update Some Company Details.
        /// </summary>
        /// <param name="company"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("update/company/{id}", Name = "UpdateCompany")]
        [HttpPut]
        public IHttpActionResult UpdateCompany([FromUri]int id, [FromBody]AirlineCompany company)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                AirlineCompany companyForUpdate = myFacade.GetAirlineById(myToken, id);
                if (company.Id != companyForUpdate.Id)
                    throw new IdIsNotMatchException("Your Company ID Does Not Match ID That Found In Your URL.");
                else
                {
                    companyForUpdate = company;
                    myFacade.UpdateAirlineDetails(myToken, companyForUpdate);
                    return Ok($"Airline Company Number: {company.Id} Updated Successfully");
                }
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Update Some Customer Details.
        /// <summary>
        /// Update Customer Details.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("update/customer/{id}", Name = "UpdateCustomer")]
        [HttpPut]
        public IHttpActionResult UpdateCustomer([FromUri]int id, [FromBody]Customer customer)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                Customer customerForUpdate = myFacade.GetCustomerById(myToken, id);
                if (customer.Id != customerForUpdate.Id)
                    throw new IdIsNotMatchException("Your Customer ID Does Not Match ID That Found In Your URL.");
                else
                {
                    customerForUpdate = customer;
                    myFacade.UpdateCustomerDetails(myToken, customerForUpdate);
                }
                return Ok($"Customer Number: {customer.Id} Updated Successfully");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Update Some Country Details.
        /// <summary>
        /// Update Some Country Details.
        /// </summary>
        /// <param name="country"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("update/country/{id}", Name = "UpdateCountry")]
        [HttpPut]
        public IHttpActionResult UpdateCountry([FromUri]int id, [FromBody]Country country)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                Country countryForUpdate = myFacade.GetCountryById(id);
                if (country.Id != countryForUpdate.Id)
                    throw new IdIsNotMatchException("Your Country ID Does Not Match ID That Found In Your URL.");
                else
                {
                    countryForUpdate = country;
                    myFacade.UpdateCountryDetails(myToken, countryForUpdate);
                }
                return Ok($"Country Number: { country.Id} Successfully Updated To: { country.Country_Name}.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Change Password For Current Admin.
        /// <summary>
        /// Change Password For Current Admin.
        /// </summary>
        /// <param name="passwords"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("changemypassword", Name = "ChangePasswordForCurrentAdmin")]
        [HttpPut]
        public IHttpActionResult ChangePasswordForCurrentAdmin([FromBody]ChangePasswordModel passwords)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                myFacade.ChangeMyPassword(myToken, passwords.OldPassword, passwords.NewPassword);
                return Ok($"Your Password Changed Successfully.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Force Change Password For Some Company.
        /// <summary>
        /// Force Change Password For Some Company.
        /// </summary>
        /// <param name="id" name="passwords"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("changepassword/company/{id}", Name = "ForceChangePasswordForCompany")]
        [HttpPut]
        public IHttpActionResult ForceChangePasswordForCompany([FromUri]int id, [FromBody]ChangePasswordModel passwords)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                AirlineCompany company = myFacade.GetAirlineById(myToken, id);
                myFacade.ForceChangePasswordForAirline(myToken, company, passwords.NewPassword);
                return Ok($"The Password For Company Number: {id} Changed Successfully.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Force Change Password For Some Customer.
        /// <summary>
        /// Force Change Password For Some Customer.
        /// </summary>
        /// <param name="id" name="passwords"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(string))]
        [Route("changepassword/customer/{id}", Name = "ForceChangePasswordForCustomer")]
        [HttpPut]
        public IHttpActionResult ForceChangePasswordForCustomer([FromUri]int id, [FromBody]ChangePasswordModel passwords)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                Customer customer = myFacade.GetCustomerById(myToken, id);
                myFacade.ForceChangePasswordForCustomer(myToken, customer, passwords.NewPassword);
                return Ok($"The Password For Customer Number: {id} Changed Successfully.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get Administrator By Id.
        /// <summary>
        /// Search Administrator By Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Administrator))]
        [Route("{id}", Name = "GetAdministratorById")]
        [HttpGet]
        public IHttpActionResult GetAdministratorById([FromUri]int id)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                Administrator admin = myFacade.GetAdminById(myToken, id);
                return GetSuccessResponse(admin, "No Administrator With The Received Id Was Found.");
            });
            return result; // for debug - break point hered
        }
        #endregion

        #region Get Administrator By UserName(Query).
        /// <summary>
        /// Search Administrator By UserName.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Administrator))]
        [Route("search/admin/byusername", Name = "GetAdministratorByUserName")]
        [HttpGet]
        public IHttpActionResult GetAdministratorByUserName([FromUri] string userName = "")
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                Administrator admin = myFacade.GetAdminByUserName(myToken, userName);
                return GetSuccessResponse(admin, "No Administrator With The Received userName Was Found.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get Company By Id.
        /// <summary>
        /// Get Company By Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(AirlineCompany))]
        [Route("companies/{id}", Name = "GetCompanyById")]
        [HttpGet]
        public IHttpActionResult GetCompanyById([FromUri]int id)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                AirlineCompany company = myFacade.GetAirlineById(myToken, id);
                return GetSuccessResponse(company, "No Company With The Received Id Was Found.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get Customer By Id.
        /// <summary>
        /// Get Customer By Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Customer))]
        [Route("customers/{id}", Name = "GetCustomerById")]
        [HttpGet]
        public IHttpActionResult GetCustomerById([FromUri]int id)
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                Customer customer = myFacade.GetCustomerById(myToken, id);
                return GetSuccessResponse(customer, "No Customer With The Received Id Was Found.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get Customer By UserName(Query).
        /// <summary>
        /// Get Customer By UserName.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(Customer))]
        [Route("search/customers/byname", Name = "GetCustomerByUserName")]
        [HttpGet]
        public IHttpActionResult GetCustomerByUserName([FromUri]string username = "")
        {
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                Customer customer = myFacade.GetCustomerByUserName(myToken, username);
                return GetSuccessResponse(customer, "No Customer With The Received userName Was Found.");
            });
            return result; // for debug - break point here
        }
        #endregion

        #region Get All Customers.
        /// <summary>
        /// Get All Customers.
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IList<Customer>))]
        [Route("customers", Name = "GetAllCustomers")]
        [HttpGet]
        public IHttpActionResult GetAllCustomers()
        {
            IList<Customer> customers = new List<Customer>();
            IHttpActionResult result = ExecuteSafe(() =>
            {
                GetLoginToken(User.Identity.Name, out LoginToken<Administrator> myToken, out LoggedInAdministratorFacade myFacade);
                customers = myFacade.GetAllCustomers(myToken);
                if (customers.Count < 1)
                    return Content(HttpStatusCode.NoContent, "Customer List Is Empty.");
                return Content(HttpStatusCode.OK, customers);
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
        public IHttpActionResult ExecuteSafe(Func<IHttpActionResult> myFunc)
        {
            try
            {
                return myFunc.Invoke();
            }
            catch (UserNotExistException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            catch (UserAlreadyExistException ex)
            {
                return Content(HttpStatusCode.Conflict, ex.Message);
            }
            catch (WrongPasswordException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CentralAdministratorException ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (FlightNotMatchException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
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
        private void GetLoginToken(string userName, out LoginToken<Administrator> token, out LoggedInAdministratorFacade facade)
        {
            token = null;
            facade = null;
            Administrator admin = new LoggedInAdministratorFacade().GetAdminByUserName(FlyingCenterConfig.basicToken, userName);
            if (admin != null)
            {
                token = new LoginToken<Administrator> { User = admin };
                facade = new LoggedInAdministratorFacade();
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
