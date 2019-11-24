using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    public class LoginService : ILoginService
    {
        private IAirlineDAO _airlineDAO = new AirlineDAOMSSQL();
        private ICustomerDAO _customerDAO = new CustomerDAOMSSQL();
        private IAdministratorDAO _administratorDAO = new AdministratorDAOMSSQL();
        IUserDAO _userDAO = new UserDAOMSSQL();

        public UserType TryLogin(string userName, string password, out ILogin token, out FacadeBase facade)
        {
            token = null;
            facade = new AnonymousUserFacade();

            // Default Admin.
            if (userName.ToUpper() == FlyingCenterConfig.ADMIN_NAME.ToUpper())
            {
                if (password.ToUpper() == FlyingCenterConfig.ADMIN_PASSWORD.ToUpper())
                {
                    token = new LoginToken<Administrator>
                    {
                        User = new Administrator
                        (
                            0, //Admin Number 
                            0, //Id
                            FlyingCenterConfig.ADMIN_NAME,
                            FlyingCenterConfig.ADMIN_PASSWORD
                        )
                    };
                    facade = new LoggedInAdministratorFacade();
                    return UserType.Administrator;
                }
                else
                {
                    throw new WrongPasswordException("Sorry, But Your Password Isn't Match To Your User Name.");
                }
            }

            // DAO Users.
            User user = _userDAO.GetUserByUserName(userName);
            if (user != null)
            {
                if (user.User_Name == userName)
                {
                    if (password.ToUpper() == user.Password.ToUpper())
                    {
                        switch (user.MyType)
                        {
                            case UserType.Administrator:
                                {
                                    Administrator admin = _administratorDAO.GetById(user.Id);
                                    token = new LoginToken<Administrator>
                                    {
                                        User = new Administrator
                                    (
                                        admin.Admin_Number,
                                        user.Id,
                                        user.User_Name,
                                        user.Password
                                    )
                                    };
                                    facade = new LoggedInAdministratorFacade();
                                    return UserType.Administrator;
                                }
                            case UserType.Airline:
                                {
                                    AirlineCompany airline = _airlineDAO.GetById(user.Id);
                                    token = new LoginToken<AirlineCompany>
                                    {
                                        User = new AirlineCompany
                                    (
                                        airline.Airline_Number,
                                        user.Id,
                                        user.User_Name,
                                        user.Password,
                                        airline.Airline_Name,
                                        airline.Country_Code
                                    )
                                    };
                                    facade = new LoggedInAirlineFacade();
                                    return UserType.Airline;
                                }
                            case UserType.Customer:
                                {
                                    Customer customer = _customerDAO.GetById(user.Id);
                                    token = new LoginToken<Customer>
                                    {
                                        User = new Customer
                                        (
                                            customer.Customer_Number,
                                            user.Id,
                                            user.User_Name,
                                            user.Password,
                                            customer.First_Name,
                                            customer.Last_Name,
                                            customer.Address,
                                            customer.Phone_No,
                                            customer.Credit_Card_Number
                                        )
                                    };
                                    facade = new LoggedInCustomerFacade();
                                    return UserType.Customer;
                                }
                            default:
                                {
                                    return UserType.Anonymous;
                                }
                        }
                    }
                    else
                        throw new WrongPasswordException("Sorry, But Your Password Is Not Match To Your User Name.");
                }
                else
                    throw new UserNotExistException($"Sorry, But {userName} Does Not Exist.");

            }
            return UserType.Anonymous;
        }
    }
}
