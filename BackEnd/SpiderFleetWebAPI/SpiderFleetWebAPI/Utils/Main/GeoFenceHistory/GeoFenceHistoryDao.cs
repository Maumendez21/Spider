using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Main.GeoFenceHistory;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.Main.GeoFenceHistory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Main.GeoFenceHistory
{
    public class GeoFenceHistoryDao
    {

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public GeoFenceHistoryDao() { }

        public GeoFenceHistoryResponse GetTimeOutDevice(string device, string mongo, DateTime start, DateTime end)
        {
            GeoFenceHistoryResponse response = new GeoFenceHistoryResponse();

            var ListPoints = new List<PointsTimeOut>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_consult_time_out_device", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@device", device));
                        cmd.Parameters.Add(new SqlParameter("@mongo", mongo));
                        cmd.Parameters.Add(new SqlParameter("@start", start));
                        cmd.Parameters.Add(new SqlParameter("@end", end));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PointsTimeOut point = new PointsTimeOut();
                                string[] coordinates = reader["coordinates"].ToString().Split(',');

                                point.Latitude = coordinates[0].ToString();
                                point.Longitude = coordinates[1].ToString();
                                point.Date = UseFul.DateFormatdddd_ddMMMMyyyy(Convert.ToDateTime(reader["fecha"].ToString()));
                                point.TimeOut = UseFul.CalcularTime(Convert.ToInt32(reader["diff"].ToString()));
                                point.NameVehicle = reader["NameVehicle"].ToString();
                                ListPoints.Add(point);

                            }
                            reader.Close();
                        }

                        response.ListPointsTimeOut = ListPoints;
                        response.success = true;

                        return response;
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