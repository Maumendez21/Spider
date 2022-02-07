using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Main.Map;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Main.Map
{
    public class GeneralTripInformationDao
    {
        public GeneralTripInformationDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        /// <summary>
        /// Consulta de los viajes realizados por Device y rango de fechas
        /// </summary>
        public GeneralTripsResponse Read(CredencialSpiderFleet.Models.Main.Map.GeneralTrips trips)
        {
            GeneralTripsResponse response = new GeneralTripsResponse();
            List<CredencialSpiderFleet.Models.Main.Map.GeneralTrips> listTrips = new List<CredencialSpiderFleet.Models.Main.Map.GeneralTrips>();
            CredencialSpiderFleet.Models.Main.Map.GeneralTrips trip = new CredencialSpiderFleet.Models.Main.Map.GeneralTrips();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_general_travel_information", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(trips.IdDevice)));
                    cmd.Parameters.Add(new SqlParameter("@begin", Convert.ToString(trips.begin_date)));
                    cmd.Parameters.Add(new SqlParameter("@end", Convert.ToString(trips.end_date)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            trip = new CredencialSpiderFleet.Models.Main.Map.GeneralTrips();
                            trip.IdTrip = Convert.ToInt32(reader["idTrip"]);
                            trip.IdDevice = Convert.ToString(reader["idDevice"]);
                            trip.begin_date = Convert.ToString(reader["begin_date"]);
                            trip.end_date = Convert.ToString(reader["end_date"]);
                            trip.distance = Convert.ToString(reader["distance"]);
                            trip.alarm = Convert.ToString(reader["alarm"]);
                            trip.fuel = Convert.ToString(reader["fuel"]);
                            listTrips.Add(trip);
                        }

                        response.listTrips = listTrips;
                        response.success = true;
                    }
                }
                else
                {
                    response.success = false;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
            finally
            {
                cn.Close();
            }
            return response;
        }

    }
}