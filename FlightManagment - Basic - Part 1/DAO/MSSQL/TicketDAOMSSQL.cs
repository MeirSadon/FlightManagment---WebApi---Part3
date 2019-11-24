using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    // Class With All Funtions(Of Ticket) For MSSQL Data Base.
    internal class TicketDAOMSSQL : ITicketDAO
    {
        //====== Crud Commands ======//

        // Add New Ticket.
        public long Add(Ticket t)
        {
            long newId = 0;
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                using (SqlCommand cmd2 = new SqlCommand($"Add_Ticket", conn))
                {
                    cmd2.Connection.Open();

                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@Flight_Id", t.Flight_Id);
                    cmd2.Parameters.AddWithValue("@Customer_Id", t.Customer_Id);

                    newId = (long)cmd2.ExecuteScalar();
                }
            }
            return newId;
        }

        // Remove Ticket.
        public bool Remove(Ticket t)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Delete From Tickets where Id = {t.Id}", conn))
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

        // Update Ticket
        public bool Update(Ticket t)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Update Tickets Set Flight_Id = {t.Flight_Id}, Customer_Id = " +
                    $"{t.Customer_Id} where Id = {t.Id}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.RecordsAffected > 0)
                        {
                            cmd.ExecuteNonQuery();
                            return true;
                        }
                    }
                }
            }
            throw new ArgumentException($"Sorry, But We Don't Found Ticket With This Id.");
        }

        //====== Search Commands ======//

        // Search Ticket By Id.
        public Ticket GetById(long id)
        {
            Ticket ticket = new Ticket();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Select * From Tickets Where Id = {id}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() == true)
                        {
                            ticket = new Ticket
                            {
                                Id = (long)reader["Id"],
                                Customer_Id = (long)reader["Customer_Id"],
                                Flight_Id = (long)reader["Flight_Id"],
                            };
                        }

                    }
                }
            }
            return ticket;
        }

        // Search All Tickets.
        public IList<Ticket> GetAll()
        {
            List<Ticket> tickets = new List<Ticket>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"select * from tickets", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            tickets.Add(new Ticket
                            {
                                Id = (long)reader["Id"],
                                Customer_Id = (long)reader["Customer_Id"],
                                Flight_Id = (long)reader["Flight_Id"],
                            });
                        }
                    }
                }
            }
            return tickets;
        }

        // Search All Tickets By Airline Company.
        public IList<Ticket> GetTicketsByAirlineComapny(AirlineCompany airline)
        {
            List<Ticket> tickets = new List<Ticket>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"select * from Flights f join Tickets t on f.Id = t.Flight_Id where" +
                    $" f.AirlineCompany_Id = {airline.Id}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            tickets.Add(new Ticket
                            {
                                Id = (long)reader["Id"],
                                Customer_Id = (long)reader["Customer_Id"],
                                Flight_Id = (long)reader["Flight_Id"],
                            });
                        }
                    }
                }
            }
            return tickets;
        }

        // Search All Tickets By Customer.
        public IList<Ticket> GetTicketsByCustomer(Customer customer)
        {
            List<Ticket> tickets = new List<Ticket>();
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"select * from Tickets t where t.Customer_Id = {customer.Id}", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() == true)
                        {
                            tickets.Add(new Ticket
                            {
                                Id = (long)reader["Id"],
                                Customer_Id = (long)reader["Customer_Id"],
                                Flight_Id = (long)reader["Flight_Id"],
                            });
                        }
                    }
                }
            }
            return tickets;
        }
    }
}
