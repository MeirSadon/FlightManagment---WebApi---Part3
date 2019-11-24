using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    internal class UserDAOMSSQL : IUserDAO
    {
        // Add New UserName To Data Base.
        public void AddUserName(User user, out long userId)
        {
            userId = 0;
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand($"Add_User", conn))
                {
                    cmd.Connection.Open();

                    cmd.Parameters.AddWithValue("@User_Name", user.User_Name);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Type", user.MyType);
                    cmd.CommandType = CommandType.StoredProcedure;
                    userId = (long)cmd.ExecuteScalar();
                }
            }
        }

        // Remove Some UserName From The Data Base.
        public void RemoveUserName(User user)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Delete From Users Where User_Name = '{user.User_Name}'", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Change UserName For Some User.
        public void UpdateUserName(string oldUserName, string newUserName)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Update Users Set User_Name = '{newUserName}' Where User_Name = '{oldUserName}'", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Change Password Of Current Administrator.
        public bool TryChangePasswordForUser(User user, string oldPassword, string newPassword)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Update Users Set Password = '{newPassword}' Where User_Name = '{user.User_Name}' And Password = '{oldPassword}'", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.RecordsAffected > 0)
                            return true;
                    }
                }
            }
            return false;
        }

        //Force Change Password For Airline/Customer From Admin.
        public void ForceChangePasswordForUser(User user, string newPassword)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Update Users Set Password = '{newPassword}' Where User_Name = '{user.User_Name}'", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.RecordsAffected > 0)
                            return;
                    }
                }
            }
            throw new UserNotExistException("Sorry, But We Don't Found This User.");
        }

        // Get User By UserName.
        public User GetUserByUserName(string userName)
        {
            User user = null;
            if (userName.ToUpper() == FlyingCenterConfig.ADMIN_NAME)
                user = new User(-1, FlyingCenterConfig.ADMIN_NAME, FlyingCenterConfig.ADMIN_PASSWORD, UserType.Administrator);
            else
            {
                using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
                {

                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand($"Select * from Users where User_Name = '{userName}'", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() == true && user == null)
                            {
                                Enum.TryParse((string)reader["Type"], out UserType theType);
                                user = new User((long)reader["Id"], (string)reader["User_Name"], (string)reader["Password"], theType);
                            }
                        }
                    }
                }
            }
            return user;
        }

        // Get User By Id.
        public User GetUserById(long id)
        {
            User user = null;
            {
                using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand($"Select * from Users where Id = {id}", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() == true)
                            {
                                Enum.TryParse((string)reader["Type"], out UserType theType);
                                user = new User((long)reader["Id"], (string)reader["User_Name"], (string)reader["Password"], theType);
                            }
                        }
                    }
                }
            }
            return user;
        }
    }
}
