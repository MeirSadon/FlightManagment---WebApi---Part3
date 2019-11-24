using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    // Class With All Funtions(Of Administrator) For MSSQL Data Base.
    internal class AdministratorDAOMSSQL : IAdministratorDAO
    {

        //====== CRUD Commands ======//

        // Add New Admin.
        public long Add(Administrator t)
        {
            long adminNumber = 0;
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand($"Add_Administrator", conn))
                {
                    cmd.Connection.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", t.Id);

                    adminNumber = (long)cmd.ExecuteScalar();
                }
            }
            return adminNumber;

        }

        // Remove Admin
        public bool Remove(Administrator t)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Delete From Administrators where Id = {t.Id}", conn))
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
            throw new UserNotExistException("Sorry, But This Administrator Does Not Exist.");
        }

        // Update Current Admin Without Password.
        public bool Update(Administrator t)
        {
            // Don't Have What To Update For Administrator.
            return false; ;
        }


        //====== Search Administrators ======//

        // Search Admin By Id.
        public Administrator GetById(long id)
        {
            Administrator administrator = new Administrator();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select u.Id, u.User_Name , u.Password, ad.Admin_Number from Users u Right join Administrators ad On ad.Id = u.Id Where ad.Id = {id}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() == true)
                        {
                            administrator = new Administrator
                            (
                                (long)reader["Admin_Number"],
                                (long)reader["Id"],
                                (string)reader["User_Name"],
                                (string)reader["Password"]
                            );
                        }

                    }
                }
                return administrator;
            }
        }

        // Search All Administrators.
        public IList<Administrator> GetAll()
        {
            List<Administrator> adminisitrators = new List<Administrator>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select u.Id, u.User_Name , u.Password, ad.Admin_Number from Users u Right join Administrators ad On ad.Id = u.Id", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            adminisitrators.Add(new Administrator
                            (
                                (long)reader["Admin_Number"],
                                (long)reader["Id"],
                                (string)reader["User_Name"],
                                (string)reader["Password"]
                            ));
                        }

                    }
                }
                return adminisitrators;
            }
        }

        //====== Actions History ======//

        // Get All Actions History.
        public IList<string> GetAllActionsHistory()
        {
            List<string> allActionsHistory = new List<string>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"select * from ActionsHistory", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            allActionsHistory.Add($"*Action Number {(long)reader["Id"]}. Date: {(DateTime)reader["Date"]}. " +
                                $"Category: {(string)reader["Category"]}. Action: {(string)reader["Action"]}. Is Succeed: {(bool)reader["IsSucceed"]}.");
                        }
                    }
                }
            }
            return allActionsHistory;
        }

        // Get All Actions By Dates.
        public IList<string> GetAllActionHistoryByDate(DateTime startDate, DateTime endDate)
        {
            List<string> actionsBetweenDates = new List<string>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"select * from ActionsHistory where Date >= '{startDate.ToString()}' and Date <= '{endDate.ToString()}'", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            actionsBetweenDates.Add($"*Action Number: {(long)reader["Id"]}. Date: {(DateTime)reader["Date"]}. " +
                                $"Category: {(string)reader["Category"]}. Action: {(string)reader["Action"]}. Is Succeed: {(bool)reader["IsSucceed"]}.");
                        }
                    }
                }
            }
            return actionsBetweenDates;
        }

        // Get All Actions By Category.
        public IList<string> GetAllActionHistoryByCategory(Categories category)
        {
            List<string> actionsByCategories = new List<string>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select * from ActionsHistory where Category like '{category}'", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            actionsByCategories.Add($"*Action Number: {(long)reader["Id"]}. Date: {(DateTime)reader["Date"]}. " +
                                $"Category: {(string)reader["Category"]}. Action: {(string)reader["Action"]}. Is Succeed: {(bool)reader["IsSucceed"]}.");
                        }
                    }
                }
            }
            return actionsByCategories;
        }

        // Get All Actions By If IsSucceed Is True Or False.
        public IList<string> GetAllActionHistoryByIsSucceed(bool isSucceed)
        {
            List<string> actionsByIsSucceed = new List<string>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select * from ActionsHistory where IsSucceed = '{isSucceed}'", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            actionsByIsSucceed.Add($"*Action Number: {(long)reader["Id"]}. Date: {(DateTime)reader["Date"]}. " +
                                $"Category: {(string)reader["Category"]}. Action: {(string)reader["Action"]}. Is Succeed: {(bool)reader["IsSucceed"]}.");
                        }
                    }
                }
            }
            return actionsByIsSucceed;
        }
    }
}
