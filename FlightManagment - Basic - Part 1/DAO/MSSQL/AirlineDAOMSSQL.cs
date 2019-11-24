using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    // Class With All Funtions(Of Airline) For MSSQL Data Base.
    internal class AirlineDAOMSSQL : IAirlineDAO
    {
        //====== Crud Commands ======//

        // Add New Airline Company.
        public long Add(AirlineCompany t)
        {
            long airlineNumber = 0;
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand($"Add_Airline", conn))
                {
                    cmd.Connection.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", t.Id);
                    cmd.Parameters.AddWithValue("@Airline_Name", t.Airline_Name);
                    cmd.Parameters.AddWithValue("@Country_Code", t.Country_Code);

                    airlineNumber = (long)cmd.ExecuteScalar();
                }
            }
            return airlineNumber;
        }

        // Remove Airline Company.
        public bool Remove(AirlineCompany t)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Delete from Tickets Where Flight_Id = (select  TOP 1 Id from Flights where AirlineCompany_Id = (select Id from AirlineCompanies where Id = {t.Id})); " +
                    $"Delete from Flights Where AirlineCompany_Id = (select Id from AirlineCompanies where Id = {t.Id});" +
                    $"Delete from AirlineCompanies where Id = {t.Id}", conn))
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            throw new UserNotExistException("Sorry, But This Company Does Not Exist.");
        }

        // Update Airline Company (Except Password).
        public bool Update(AirlineCompany t)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Update AirLineCompanies Set Airline_Name = '{t.Airline_Name}'," +
                 $"Country_Code = '{t.Country_Code}' Where Id = {t.Id}", conn))
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
            throw new UserNotExistException("Sorry, But This Airline Does Not Exist.");
        }

        // Change Password Of Current Airline.
        public void ChangePassword(AirlineCompany airline)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Update AirLineCompanies Set Password = '{airline.Password}' Where Id = {airline.Id}", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }


        //====== Search Airline ======//

        // Serach AirLine Company By Id.
        public AirlineCompany GetById(long id)
        {
            AirlineCompany airline = new AirlineCompany();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select u.Id, u.User_Name, u.Password, ai.Airline_Number, ai.Airline_Name, ai.Country_Code " +
                    $"from Users u Right join AirlineCompanies ai On ai.Id = u.Id Where ai.Id = {id}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() == true)
                        {
                            airline = new AirlineCompany
                            (
                                (long)reader["Airline_Number"],
                                (long)reader["Id"],
                                (string)reader["Airline_Name"],
                                (string)reader["User_Name"],
                                (string)reader["Password"],
                                (long)reader["Country_Code"]
                            );
                        }

                    }
                }
                return airline;
            }
        }

        // Search Some Airline By Company Name.
        public AirlineCompany GetByName(string airlineName)
        {
            AirlineCompany airline = new AirlineCompany();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select u.Id, u.User_Name, u.Password, ai.Airline_Number, ai.Airline_Name, ai.Country_Code " +
                    $"from Users u Right join AirlineCompanies ai On ai.Id = u.Id Where ai.Airline_Name = '{airlineName}'", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() == true)
                        {
                            airline = new AirlineCompany
                            (
                                (long)reader["Airline_Number"],
                                (long)reader["Id"],
                                (string)reader["Airline_Name"],
                                (string)reader["User_Name"],
                                (string)reader["Password"],
                                (long)reader["Country_Code"]
                            );
                        }

                    }
                }
                return airline;
            }
        }

        // Search All Airline Company.
        public IList<AirlineCompany> GetAll()
        {
            List<AirlineCompany> airlineCompanies = new List<AirlineCompany>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select u.Id, u.User_Name, u.Password, Airline_Number, ai.Airline_Name, ai.Country_Code " +
                    $"from Users u Right join AirlineCompanies ai On ai.Id = u.Id", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            airlineCompanies.Add(new AirlineCompany
                            (
                                (long)reader["Airline_Number"],
                                (long)reader["Id"],
                                (string)reader["Airline_Name"],
                                (string)reader["User_Name"],
                                (string)reader["Password"],
                                (long)reader["Country_Code"]
                            ));
                        }

                    }
                }
            }
            return airlineCompanies;
        }

        // Search All Airline Company By Country Code.
        public IList<AirlineCompany> GetAllAirlinesByCountry(int countryId)
        {
            List<AirlineCompany> airlineCompanies = new List<AirlineCompany>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select u.Id, u.User_Name, u.Password, Airline_Number, ai.Airline_Name, ai.Country_Code " +
                    $"from Users u Right join AirlineCompanies ai On ai.Id = u.Id where ai.Country_Code = {countryId}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            airlineCompanies.Add(new AirlineCompany
                            (
                                (long)reader["Airline_Number"],
                                (long)reader["Id"],
                                (string)reader["Airline_Name"],
                                (string)reader["User_Name"],
                                (string)reader["Password"],
                                (long)reader["Country_Code"]
                            ));
                        }

                    }
                }
                return airlineCompanies;
            }
        }
    }
}
