using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    // Class With All Funtions(Of Country) For MSSQL Data Base.
    internal class CountryDAOMSSQL : ICountryDAO
    {
        //====== Crud Commands ======//

        // Add New Country.
        public long Add(Country t)
        {
            long newId = 0;
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand($"Add_Country", conn))
                {
                    cmd.Connection.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Country_Name", t.Country_Name);

                    newId = (long)cmd.ExecuteScalar();
                }
            }
            return newId;
        }

        // Remove Country.
        public bool Remove(Country t)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Delete From Countries where Id = {t.Id}", conn))
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
            return false;
        }

        // Update Country.
        public bool Update(Country t)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Update Countries Set Country_Name = '{t.Country_Name}' where Id = {t.Id}", conn))
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
            return false;
        }


        //====== Search Commands ======//

        // Search Country By Id.
        public Country GetById(long id)
        {
            Country country = new Country();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select * From Countries Where Id = {id}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() == true)
                        {
                            country = new Country
                            {
                                Id = (long)reader["Id"],
                                Country_Name = (string)reader["Country_Name"],
                            };
                        }

                    }
                }
            }
            return country;
        }

        // Search Country By Name.
        public Country GetByName(string name)
        {
            Country country = new Country();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select * From Countries Where Country_Name = '{name}'", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() == true)
                        {
                            country = new Country
                            {
                                Id = (long)reader["Id"],
                                Country_Name = (string)reader["Country_Name"],
                            };
                        }

                    }
                }
            }
            return country;
        }

        // Search All Countries.
        public IList<Country> GetAll()
        {
            List<Country> countries = new List<Country>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"select * from countries", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read() == true)
                    {
                        countries.Add(new Country
                        {
                            Id = (long)reader["Id"],
                            Country_Name = (string)reader["Country_Name"],
                        });
                    }
                }
            }
            return countries;
        }
    }
}
