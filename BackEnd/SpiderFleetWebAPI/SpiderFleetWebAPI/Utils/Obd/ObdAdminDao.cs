using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Obd;
using SpiderFleetWebAPI.Models.Response.Obd;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Obd
{
    public class ObdAdminDao
    {
        public ObdAdminDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private const string URL = "https://www.google.com/maps/search/?api=1&query=";

        public ObdAdminResponse ReadGeneralStatusDevice(string empresa)
        {
            ObdAdminResponse response = new ObdAdminResponse();
            List<ObdAdmin> listObd = new List<ObdAdmin>();
            ObdAdmin obd = new ObdAdmin();
            Dictionary<string, string> valuePairs = new Dictionary<string, string>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_general_status", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@empresa", Convert.ToString(empresa)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obd = new ObdAdmin();
                            obd.Hierarchy = Convert.ToString(reader["hierarchy"]);
                            obd.Device = Convert.ToString(reader["ID_Vehiculo"]);
                            obd.VehicleName = Convert.ToString(reader["Nombre"]);
                            obd.LastDate = Convert.ToDateTime(reader["date_gps"]).ToString("dd-MM-yyyy HH:mm:ss");
                            obd.LastGPS = Convert.ToString(URL + reader["latitude_gps"] + "," + reader["longitude_gps"]); 
                            obd.Event = Convert.ToString(reader["event_gps"]);

                            obd.LastDateAlarm = Convert.ToDateTime(reader["date_alaram"]).ToString("dd-MM-yyyy HH:mm:ss");
                            obd.LastAlarm = Convert.ToString(URL + reader["latitude_alarm"] + "," + reader["longitude_alarm"]);
                            
                            obd.LastDateLogin = Convert.ToDateTime(reader["date_login"]).ToString("dd-MM-yyyy HH:mm:ss");
                            obd.LastLogin = Convert.ToString(URL + reader["latitude_login"] + "," + reader["longitude_login"]);
                            obd.Mode = Convert.ToString(reader["mode"]);

                            //obd.LastDateSleep = Convert.ToDateTime(reader["date_sleep"]).ToString("dd-MM-yyyy HH:mm:ss");
                            //obd.LastSleep = Convert.ToString(URL + reader["latitude_sleep"] + "," + reader["longitude_sleep"]);
                            obd.AlarmType = Convert.ToString(reader["tipo_alarma"]);


                            if (!valuePairs.ContainsKey(obd.Device))
                            {
                                valuePairs.Add(obd.Device, obd.Device);
                                listObd.Add(obd);
                            }
                        }

                        reader.Close();
                        response.success = true;
                        response.ListObdAdmins = listObd;
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