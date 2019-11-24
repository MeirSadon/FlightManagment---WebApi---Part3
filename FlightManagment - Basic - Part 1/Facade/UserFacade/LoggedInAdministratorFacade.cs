using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    // Class With All The Options That Admin Can Do.
    public class LoggedInAdministratorFacade : AnonymousUserFacade, ILoggedInAdministratorFacade
    {

        // Create New Administrator.
        public long CreateNewAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            long adminNumber = 0;
            if (token.User.User_Name.ToUpper() == FlyingCenterConfig.ADMIN_NAME && token.User.Password == FlyingCenterConfig.ADMIN_PASSWORD && admin != null)
            {
                if (admin.User_Name.ToUpper() == FlyingCenterConfig.ADMIN_NAME)
                {
                    throw new UserAlreadyExistException($"Sorry, But {admin.User_Name} Already Exist. Please Try Another User Name");
                }
                User adminUser = _userDAO.GetUserById(admin.Id);
                if (adminUser == null)
                {
                    _userDAO.AddUserName(new User(admin.User_Name, admin.Password, UserType.Administrator), out long userId);
                    admin.Id = userId;
                    adminNumber = _adminDAO.Add(admin);
                    _backgroundDAO.AddNewAction(Categories.Administrators | Categories.Adds, $"Admin {token.User.User_Name} Tried To Create New Administrator. Id: {admin.Id} ({admin.User_Name}).", false);
                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.Administrators | Categories.Adds, $"Admin {token.User.User_Name} Tried To Create New Administrator. Id: {admin.Id} ({admin.User_Name}).", false);
                    throw new UserAlreadyExistException($"Sorry, But '{admin.User_Name}' Already Exist.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.Administrators | Categories.Adds, $"Not Qualified User Tried To Create New Admin.", false);
            return adminNumber;
        }

        // Create New Airline Company.
        public long CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            long airlineNumber = 0;
            if (UserIsValid(token) && airline != null)
            {
                User airlineUser = _userDAO.GetUserById(airline.Id);
                if (airlineUser == null)
                {
                    _userDAO.AddUserName(new User(airline.User_Name, airline.Password, UserType.Airline), out long userId);
                    airline.Id = userId;
                    airlineNumber = _airlineDAO.Add(airline);
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Adds, $"Admin {token.User.User_Name} Tried To Create New Airline Company. Id: {airline.Id} ({airline.User_Name}).", true);
                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Adds, $"Admin {token.User.User_Name} Tried To Create New Airline Company. Id: {airline.Id} ({airline.User_Name}).", true);
                    throw new UserAlreadyExistException($"Sorry, But '{airline.User_Name}' Already Exist.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Adds, $"Anonymous User Tried To Create New Airline Company. Id: {airline.Id} ({airline.User_Name}).", false);
            return airlineNumber;
        }

        // Create New Customer.
        public long CreateNewCustomer(LoginToken<Administrator> token, Customer customer)
        {
            long customerNumber = 0;
            if (UserIsValid(token) && customer != null)
            {
                User customerUser = _userDAO.GetUserById(customer.Id);
                if (customerUser == null)
                {
                    _userDAO.AddUserName(new User(customer.User_Name, customer.Password, UserType.Customer), out long userId);
                    customer.Id = userId;
                    customerNumber = _customerDAO.Add(customer);
                    _backgroundDAO.AddNewAction(Categories.Customers | Categories.Adds, $"Admin {token.User.User_Name} Tried To Create New Customer. Id: {customer.Id} ({customer.User_Name}).", true);

                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.Customers | Categories.Adds, $"Admin {token.User.User_Name} Tried To Create New Customer. Id: {customer.Id} ({customer.User_Name}).", false);
                    throw new UserAlreadyExistException($"Sorry, But '{customer.User_Name}' Already Exist.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.Customers | Categories.Adds, $"Anonymous User Tried To Create New Customer. Id: {customer.Id} ({customer.User_Name}).", false);
            return customerNumber;
        }

        // Create New Country.
        public long CreateNewCountry(LoginToken<Administrator> token, Country country)
        {
            long newId = 0;
            if (UserIsValid(token) && country != null)
            {
                newId = _countryDAO.Add(country);
                _backgroundDAO.AddNewAction(Categories.Countries | Categories.Adds, $"Admin: {token.User.User_Name} Tried To Create New Country. Id: {newId} ({country.Country_Name}).", true);
            }
            else
                _backgroundDAO.AddNewAction(Categories.Countries | Categories.Adds, $"Anonymous User Tried To Create New Country. Id: {country.Id} ({country.Country_Name}).", false);

            return newId;
        }

        // Remove Some DAO Administrator.
        public void RemoveAdministrator(LoginToken<Administrator> token, Administrator admin)
        {
            if (token.User.User_Name.ToUpper() == FlyingCenterConfig.ADMIN_NAME && token.User.Password == FlyingCenterConfig.ADMIN_PASSWORD && admin != null)
            {
                User adminUser = _userDAO.GetUserById(admin.Id);
                if (adminUser != null)
                {
                    _adminDAO.Remove(admin);
                    _userDAO.RemoveUserName(adminUser);
                    _backgroundDAO.AddNewAction(Categories.Administrators | Categories.Deletions, $"Admin {token.User.User_Name} Tried To Delete Some Admin. Id: {admin.Id} ({admin.User_Name}).", true);
                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.Administrators | Categories.Deletions, $"Admin {token.User.User_Name} Tried To Delete Some Admin. Id: {admin.Id} ({admin.User_Name}).", false);
                    throw new UserNotExistException($"Sorry, But '{admin.User_Name}' Does Not Exist.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.Administrators | Categories.Deletions, $"Not Qualified User Tried To Delete Some Admin. Id: {admin.Id} ({admin.User_Name}).", false);
        }

        // Remove Some Airline Company.
        public void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (UserIsValid(token) && airline != null)
            {
                User airlineUser = _userDAO.GetUserById(airline.Id);
                if (airlineUser != null)
                {
                    _airlineDAO.Remove(airline);
                    _userDAO.RemoveUserName(airlineUser);
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Deletions, $"Admin {token.User.User_Name} Tried Delete Some Admin. Id: {airline.Id} ({airline.User_Name}).", true);

                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Deletions, $"Admin {token.User.User_Name} Tried Delete Some Admin. Id: {airline.Id} ({airline.User_Name}).", false);
                    throw new UserNotExistException($"Sorry, But '{airline.User_Name}' Does Not Exist.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Deletions, $"Anonymous User Tried Delete Some Airline Comapny. Id: {airline.Id} ({airline.User_Name}).", false);
        }

        //Remove Some Customer.
        public void RemoveCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (UserIsValid(token) && customer != null)
            {
                User customerUser = _userDAO.GetUserById(customer.Id);
                if (customerUser != null)
                {
                    _customerDAO.Remove(customer);
                    _userDAO.RemoveUserName(customerUser);
                    _backgroundDAO.AddNewAction(Categories.Customers | Categories.Deletions, $"Admin {token.User.User_Name} Tried Delete Some Admin. Id: {customer.Id} ({customer.User_Name}).", true);
                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.Customers | Categories.Deletions, $"Admin {token.User.User_Name} Tried Delete Some Admin. Id: {customer.Id} ({customer.User_Name}).", false);
                    throw new UserNotExistException($"Sorry, But '{customer.User_Name}' Does Not Exist.");
                }
                _backgroundDAO.AddNewAction(Categories.Customers | Categories.Deletions, $"Admin {token.User.User_Name} Tried Delete Some Admin. Id: {customer.Id} ({customer.User_Name}).", customerUser != null);
            }
            else
                _backgroundDAO.AddNewAction(Categories.Customers | Categories.Deletions, $"Anonymous User Tried Delete Some Airline Comapny. Id: {customer.Id} ({customer.User_Name}).", false);
        }

        //Remove Some Country.
        public void RemoveCountry(LoginToken<Administrator> token, Country country)
        {
            if (UserIsValid(token) && country != null)
            {
                if (_countryDAO.Remove(country))
                {
                    _backgroundDAO.AddNewAction(Categories.Countries | Categories.Deletions, $"Admin {token.User.User_Name} Tried Delete Some Country. Id: {country.Id} ({country.Country_Name}).", true);
                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.Countries | Categories.Deletions, $"Admin {token.User.User_Name} Tried Delete Some Country. Id: {country.Id} ({country.Country_Name}).", false);
                    throw new UserNotExistException($"Sorry, But '{country.Country_Name}' Does Not Exist.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.Countries | Categories.Deletions, $"Anonymous User Tried Delete Some Country. Id: {country.Id} ({country.Country_Name}).", false);
        }

        // Update Details For Some Administrator.
        public void UpdateAdminDetails(LoginToken<Administrator> token, Administrator admin)
        {
            if (UserIsValid(token))
            {
                if (admin != null && ReferenceEquals(token.User.User_Name, admin.User_Name) || token.User.User_Name.ToUpper() == FlyingCenterConfig.ADMIN_NAME)
                {
                    User adminUser = _userDAO.GetUserById(admin.Id);
                    if (adminUser != null)
                    {
                        if (admin.User_Name != FlyingCenterConfig.ADMIN_NAME)
                            _userDAO.UpdateUserName(adminUser.User_Name, admin.User_Name);
                        else
                            throw new CentralAdministratorException("The UserName Cannot Be Changed To The Same Name As The  Central Administrator UserName.");
                        _adminDAO.Update(admin);
                        _backgroundDAO.AddNewAction(Categories.Administrators | Categories.Updates, $"Admin: {token.User.User_Name} Tried Update Some Admin. Id: {admin.Id} ({admin.User_Name}).", true);
                    }
                    else
                    {
                        _backgroundDAO.AddNewAction(Categories.Administrators | Categories.Updates, $"Admin: {token.User.User_Name} Tried Update Some Admin. Id: {admin.Id} ({admin.User_Name}).", false);
                        throw new UserNotExistException($"Sorry, But '{admin.User_Name}' Does Not Exist.");
                    }
                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.Administrators | Categories.Updates, $"Not Qualified User Tried Update Some Admin. Id: {admin.Id} ({admin.User_Name}).", false);
                    throw new CentralAdministratorException("Only Central Administrator Or Who Updateds Himself Can Update Details.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.Administrators | Categories.Updates, $"Anonymous User Tried Update Some Admin. Id: {admin.Id} ({admin.User_Name}).", false);
        }

        // Update Details For Some Airline Company.
        public void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (UserIsValid(token) && airline != null)
            {
                User airlineUser = _userDAO.GetUserById(airline.Id);
                if (airlineUser != null)
                {
                    _userDAO.UpdateUserName(airlineUser.User_Name, airline.User_Name);
                    _airlineDAO.Update(airline);
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Updates, $"Admin: {token.User.User_Name} Tried To Update Details For Some Airline Company. Id: {airline.Id} ({airline.User_Name}).", true);

                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Updates, $"Admin: {token.User.User_Name} Tried To Update Details For Some Airline Company. Id: {airline.Id} ({airline.User_Name}).", false);
                    throw new UserNotExistException($"Sorry, But '{airline.User_Name}' Does Not Exist.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Updates, $"Anonymous User Tried To Update Details For Some Airline Company. Id: {airline.Id} ({airline.User_Name}).", false);
        }

        // Update Details For Some Customer.
        public void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer)
        {
            if (UserIsValid(token) && customer != null)
            {
                User customerUser = _userDAO.GetUserById(customer.Id);
                if (customerUser != null)
                {
                    _userDAO.UpdateUserName(customerUser.User_Name, customer.User_Name);
                    _customerDAO.Update(customer);
                    _backgroundDAO.AddNewAction(Categories.Customers | Categories.Updates, $"Admin: {token.User.User_Name} Tried To Update Some Customer. Id: {customer.Id} ({customer.User_Name}).", true);

                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.Customers | Categories.Updates, $"Admin: {token.User.User_Name} Tried To Update Some Customer. Id: {customer.Id} ({customer.User_Name}).", false);
                    throw new UserNotExistException($"Sorry, But '{customer.User_Name}' Does Not Exist.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.Customers | Categories.Updates, $"Anonymous User Tried To Update Some Customer. Id: {customer.Id} ({customer.User_Name}).", false);
        }

        // Update Details For Some Country.
        public void UpdateCountryDetails(LoginToken<Administrator> token, Country country)
        {
            if (UserIsValid(token) && country != null)
            {
                if (_countryDAO.Update(country))
                {
                    _backgroundDAO.AddNewAction(Categories.Countries | Categories.Updates, $"Admin: {token.User.User_Name} Tried To Update Some Country. Id: {country.Id} ({country.Country_Name}).", true);
                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.Countries | Categories.Updates, $"Admin: {token.User.User_Name} Tried To Update Some Country. Id: {country.Id} ({country.Country_Name}).", false);
                    throw new ArgumentException($"Sorry, But This Country Does Not Exist.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.Countries | Categories.Updates, $"Anonymous User Tried To Update Some Country. Id: {country.Id} ({country.Country_Name}).", false);
        }

        // Try Change Password For Admin.
        public void ChangeMyPassword(LoginToken<Administrator> token, string oldPassword, string newPassword)
        {
            if (token.User.User_Name == FlyingCenterConfig.ADMIN_NAME)
            {
                _backgroundDAO.AddNewAction(Categories.Administrators | Categories.Updates, $"{token.User.User_Name} Tried To Change His Password.", false);
                throw new CentralAdministratorException("It's Not possible To Change Password For Central Administrator");
            }
            if (UserIsValid(token))
            {
                User adminUser = _userDAO.GetUserById(token.User.Id);
                if (adminUser != null)
                {
                    if (_userDAO.TryChangePasswordForUser(adminUser, oldPassword, newPassword))
                    {
                        token.User.ChangePassword(newPassword);
                        _backgroundDAO.AddNewAction(Categories.Administrators | Categories.Updates, $"{token.User.User_Name} Tried To Change His Password.", true);
                    }
                    else
                    {
                        _backgroundDAO.AddNewAction(Categories.Administrators | Categories.Updates, $"{token.User.User_Name} Tried To Change His Password.", false);
                        throw new WrongPasswordException("Your Old Password Is Incorrect!");
                    }
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.Administrators | Categories.Updates, $"Anonymous User Tried To Change Password For Some Admin. Id: {token.User.Id} ({token.User.User_Name}).", false);

        }

        // Force Change Password For Some Airline.
        public void ForceChangePasswordForAirline(LoginToken<Administrator> token, AirlineCompany airline, string newPassword)
        {
            if (UserIsValid(token) && airline != null)
            {
                User airlineUser = _userDAO.GetUserById(airline.Id);
                if (airlineUser != null)
                {
                    _userDAO.ForceChangePasswordForUser(airlineUser, newPassword);
                    airline.ChangePassword(newPassword);
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Updates, $"Admin: {token.User.User_Name} Tried To Change Password In Force For Some Airline Company. Id: {airline.Id} ({airline.User_Name}).", true);

                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Updates, $"Admin: {token.User.User_Name} Tried To Change Password In Force For Some Airline Company. Id: {airline.Id} ({airline.User_Name}).", false);
                    throw new UserNotExistException($"Sorry, But '{airline.User_Name}' Is Not Exist.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.AirlineCompanies | Categories.Updates, $"Anonymous User Tried To Change Password In Force For Some Airline Company. Id: {airline.Id} ({airline.User_Name}).", false);
        }

        // Force Change Password For Some Customer.
        public void ForceChangePasswordForCustomer(LoginToken<Administrator> token, Customer customer, string newPassword)
        {
            if (UserIsValid(token) && customer != null)
            {
                User customerUser = _userDAO.GetUserById(customer.Id);
                if (customerUser != null)
                {
                    _userDAO.ForceChangePasswordForUser(customerUser, newPassword);
                    customer.ChangePassword(newPassword);
                    _backgroundDAO.AddNewAction(Categories.Customers | Categories.Updates, $"Admin: {token.User.User_Name} Tried To Change Password In Force For Some Customer. Id: {customer.Id} ({customer.User_Name}).", true);

                }
                else
                {
                    _backgroundDAO.AddNewAction(Categories.Customers | Categories.Updates, $"Admin: {token.User.User_Name} Tried To Change Password In Force For Some Customer. Id: {customer.Id} ({customer.User_Name}).", false);
                    throw new UserNotExistException($"Sorry, But '{customer.User_Name}' Is Not Exist.");
                }
            }
            else
                _backgroundDAO.AddNewAction(Categories.Customers | Categories.Updates, $"Anonymous User Tried To Change Password In Force For Some Customer. Id: {customer.Id} ({customer.User_Name}).", false);

        }

        // Search Admin By Id.
        public Administrator GetAdminById(LoginToken<Administrator> token, int id)
        {
            Administrator admin = new Administrator();
            if (UserIsValid(token))
            {
                admin = _adminDAO.GetById(id);
            }
            return admin;
        }

        // Search Admin By UserName.
        public Administrator GetAdminByUserName(LoginToken<Administrator> token, string userName)
        {
            Administrator admin = new Administrator();
            if (UserIsValid(token))
            {
                if (userName.ToUpper() == FlyingCenterConfig.ADMIN_NAME.ToUpper())
                {
                    return FlyingCenterConfig.basicToken.User;
                }
                User adminUser = _userDAO.GetUserByUserName(userName);
                if (adminUser != null)
                {
                    admin = _adminDAO.GetById(adminUser.Id);
                    admin.User_Name = adminUser.User_Name;
                }
            }
            return admin;
        }

        // Search Airline Company By Id.
        public AirlineCompany GetAirlineById(LoginToken<Administrator> token, int id)
        {
            AirlineCompany airline = new AirlineCompany();
            if (UserIsValid(token))
            {
                airline = _airlineDAO.GetById(id);
            }
            return airline;
        }

        // Search Airline By UserName.
        public AirlineCompany GetAirlineByUserName(LoginToken<Administrator> token, string userName)
        {
            AirlineCompany airline = new AirlineCompany();
            if (UserIsValid(token))
            {
                User airlineUser = _userDAO.GetUserByUserName(userName);
                if (airlineUser != null)
                {
                    airline = _airlineDAO.GetById(airlineUser.Id);
                }
            }
            return airline;
        }

        // Search Customer By Id.
        public Customer GetCustomerById(LoginToken<Administrator> token, int id)
        {
            Customer customer = new Customer();
            if (UserIsValid(token))
            {
                customer = _customerDAO.GetById(id);
            }
            return customer;
        }

        // Search Customer By UserName.
        public Customer GetCustomerByUserName(LoginToken<Administrator> token, string userName)
        {
            Customer customer = new Customer();
            if (UserIsValid(token))
            {
                User customerUser = _userDAO.GetUserByUserName(userName);
                if (customerUser != null)
                {
                    customer = _customerDAO.GetById(customerUser.Id);
                }
            }
            return customer;
        }

        // Search All Customers.
        public IList<Customer> GetAllCustomers(LoginToken<Administrator> token)
        {
            IList<Customer> customers = new List<Customer>();
            if (UserIsValid(token))
            {
                customers = _customerDAO.GetAll();
            }
            return customers;
        }

        // Check If User Admin That Sent Is Valid.
        public bool UserIsValid(LoginToken<Administrator> token)
        {
            if (token != null && token.User != null)
                return true;
            return false;
        }

    }
}
