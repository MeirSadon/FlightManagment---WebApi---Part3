using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    // Class With All Funtions(Of Customer) For MSSQL Data Base.
    internal class CustomerDAOMSSQL : ICustomerDAO
    {
        //====== CRUD Commands ======//

        // Add New Customer.
        public long Add(Customer t)
        {

            long customerNumber = 0;
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand($"Add_Customer", conn))
                {
                    cmd.Connection.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", t.Id);
                    cmd.Parameters.AddWithValue("@First_Name", t.First_Name);
                    cmd.Parameters.AddWithValue("@Last_Name", t.Last_Name);
                    cmd.Parameters.AddWithValue("@Address", t.Address);
                    cmd.Parameters.AddWithValue("@Phone_No", t.Phone_No);
                    cmd.Parameters.AddWithValue("@Credit_Card_Number", t.Credit_Card_Number);

                    customerNumber = (long)cmd.ExecuteScalar();
                }
            }
            return customerNumber;
        }

        //Remove Customer.
        public bool Remove(Customer t)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Delete from Tickets Where Customer_Id = (select Top 1 Id from Customers where Id  = {t.Id});" +
                    $"Delete From Customers where Id  = {t.Id}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.RecordsAffected > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            throw new UserNotExistException("Sorry, But This Customer Does Not Exist.");
        }

        //Update Details Of Current Customer (Without Password).
        public bool Update(Customer t)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Update Customers Set First_Name = '{t.First_Name}', Last_Name = '{t.Last_Name}'," +
                    $"Address = '{t.Address}', Phone_No = '{t.Phone_No}', Credit_Card_Number = '{t.Credit_Card_Number}' Where Id = {t.Id}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.RecordsAffected > 0)
                        {
                            return true;
                        }
                    }
                }
                throw new UserNotExistException("Sorry, But This Customer Does Not Exist.");
            }
        }

        // Change Password Of Current Customer.
        public void ChangePassword(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Update Customers Set Password = '{customer.Password}' Where Id = {customer.Id}", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }


        //====== Search Customer ======//

        //Get Customer By Id.
        public Customer GetById(long id)
        {
            Customer customer = new Customer();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select u.Id, u.User_Name, u.Password, cu.Customer_Number, cu.First_Name, cu.Last_Name, cu.Address," +
                    $" cu.Phone_No, cu.Credit_Card_Number from Users u Right join Customers cu On cu.Id = u.Id Where cu.Id = {id}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() == true)
                        {
                            customer = new Customer
                            (
                                (long)reader["Customer_Number"],
                                (long)reader["Id"],
                                (string)reader["First_Name"],
                                (string)reader["Last_Name"],
                                (string)reader["User_Name"],
                                (string)reader["Password"],
                                (string)reader["Address"],
                                (string)reader["Phone_No"],
                                (string)reader["Credit_Card_Number"]
                            );
                        }
                    }
                }
            }
            return customer;
        }

        // Get All Customers.
        public IList<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select u.Id, u.User_Name, u.Password, cu.Customer_Number, cu.First_Name, cu.Last_Name, cu.Address," +
                    $" cu.Phone_No, cu.Credit_Card_Number from Users u Right join Customers cu On cu.Id = u.Id", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            customers.Add(new Customer
                            (
                                (long)reader["Customer_Number"],
                                (long)reader["Id"],
                                (string)reader["First_Name"],
                                (string)reader["Last_Name"],
                                (string)reader["User_Name"],
                                (string)reader["Password"],
                                (string)reader["Address"],
                                (string)reader["Phone_No"],
                                (string)reader["Credit_Card_Number"]
                            ));
                        }
                    }
                }
            }
            return customers;
        }
    }
}
