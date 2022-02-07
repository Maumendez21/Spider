using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Itineraries;
using SpiderFleetWebAPI.Models.Response.Mobility.InfoResponsibles;
using SpiderFleetWebAPI.Utils.Itineraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Mobility.InfoResponsibles
{
    public class InfoResponsiblesDao
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public InfoResponsiblesDao() { }

        public async Task<InfoResponsiblesResponse> GetAllResponsibles(string device, DateTime start, DateTime end, string hierarchy)
        {
            var listResponsibles = new List<CredencialSpiderFleet.Models.Mobility.InfoResponsibles.InfoResponsibles>();
            InfoResponsiblesResponse response = new InfoResponsiblesResponse();

            end = end.AddMinutes(1);

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_ad_consult_list_responsibles_by_device", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@device", device));
                        cmd.Parameters.Add(new SqlParameter("@start", start));
                        cmd.Parameters.Add(new SqlParameter("@end", end));

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listResponsibles.Add(MapToValue(reader));
                            }
                            

                            if(listResponsibles.Count > 0)
                            {
                                response.success = true;
                                response.ListResponsibles = listResponsibles;
                                ItinerariesResponse itineraries = new ItinerariesResponse();

                                foreach (var item in listResponsibles)
                                {
                                    itineraries = new ItinerariesResponse();
                                    itineraries = (new ItinerariesDao()).ReadItinerariosList(hierarchy, device, Convert.ToDateTime(item.Start), Convert.ToDateTime(item.End));
                                    item.ListItineraries = itineraries.listItineraries;
                                    item.NumberTrips = itineraries.listItineraries.Count();
                                }
                                
                            }
                            else
                            {
                                response.success = false;
                            }

                            
                        }

                        if(listResponsibles.Count == 0)
                        {
                            response.success = false;
                        }

                    }
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

        private CredencialSpiderFleet.Models.Mobility.InfoResponsibles.InfoResponsibles MapToValue(SqlDataReader reader)
        {
            return new CredencialSpiderFleet.Models.Mobility.InfoResponsibles.InfoResponsibles()
            {
                Start = Convert.ToDateTime(reader["startdate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                End = Convert.ToDateTime(reader["enddate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                Notes = reader["Notes"].ToString(),
                Name = reader["Name"].ToString()                
            };
        }
    }
}