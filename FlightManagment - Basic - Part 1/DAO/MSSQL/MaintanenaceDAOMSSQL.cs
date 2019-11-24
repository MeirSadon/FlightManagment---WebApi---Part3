using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagment___Basic___Part_1
{
    internal class MaintenanceDAOMSSQL : IMaintenanceDAO
    {
        readonly string formatDate = "yyyy-MM-dd HH:mm:ss";

        // Add New Action To ActionsHistory Table.
        public void AddNewAction(Categories category, string action, bool isSucceed)
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand($"Insert Into ActionsHistory(date, Category, Action, IsSucceed) Values('{DateTime.Now.ToString(formatDate)}', '{category}', '{action}', '{isSucceed}' )", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Move All Old Tickets And Flights To History.
        public void UpdateTicketsAndFlightsHistory()
        {
            using (SqlConnection conn = new SqlConnection(FlyingCenterConfig.CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand($"RemovePastTicketsAndFlights", conn))
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                }
            }
        }
    }
}
