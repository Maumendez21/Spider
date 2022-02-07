using CredencialSpiderFleet.Models.Alarm;
using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Logical;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.Alarm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Alarms
{
    public class AlarmDao
    {
        public AlarmDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public AlarmResponse GetAllAlarms(string device, DateTime start, DateTime end)
        {
            AlarmResponse response = new AlarmResponse();

            var alarms = new List<Alarm>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_consult_list_alarms_by_device", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@device", device));
                        cmd.Parameters.Add(new SqlParameter("@start", start));
                        cmd.Parameters.Add(new SqlParameter("@end", end));
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                alarms.Add(MapToValue(reader));
                            }
                        }

                        response.ListAlarms = alarms;
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

        private Alarm MapToValue(SqlDataReader reader)
        {
            return new Alarm()
            {
                TypeAlarm = reader["Alarma"].ToString(),
                Repet = Convert.ToInt32(reader["Repeticion"].ToString()),
                Description = UseFul.GetAlarm(reader["Alarma"].ToString())
            };
        }


        public DeviceDataResponse GetAllDevices(string node)
        {
            DeviceDataResponse response = new DeviceDataResponse();

            var devices = new List<DeviceData>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_consult_list_device_by_company", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@searchNode", node));

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                devices.Add(MapToValueDevice(reader));
                            }
                        }

                        response.ListDevices = devices;
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

        private DeviceData MapToValueDevice(SqlDataReader reader)
        {
            return new DeviceData()
            {
                Device = reader["ID_Vehiculo"].ToString(),         
                Name = reader["Nombre"].ToString()
            };
        }

    }
}