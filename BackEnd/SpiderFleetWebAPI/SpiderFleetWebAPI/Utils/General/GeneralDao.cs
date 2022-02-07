using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.General
{
    public class GeneralDao
    {
        public GeneralDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public DeviceResponse ReadGroupName(string device)
        {
            DeviceResponse response = new DeviceResponse();
            List<string> listDevice = new List<string>();
            string nombreGrupo = string.Empty;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_name_by_hierarchy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nombreGrupo = Convert.ToString(reader["grupo"]);
                        }
                        reader.Close();
                        response.success = true;
                        response.ListDevice = listDevice;
                        response.Grupo = nombreGrupo;
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

        public DeviceResponse ReadDeviceByGrupo(string hierarchy)
        {
            DeviceResponse response = new DeviceResponse();
            List<string> listDevice = new List<string>();
            string nombreGrupo = string.Empty;
            
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_device_by_hierarchy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@searchNode", Convert.ToString(hierarchy)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string device = Convert.ToString(reader["ID_Vehiculo"]);
                            nombreGrupo = Convert.ToString(reader["grupo"]);
                            listDevice.Add(device);
                        }
                        reader.Close();
                        response.success = true;
                        response.ListDevice = listDevice;
                        response.Grupo = nombreGrupo;
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


        public string ReadIdHerarchy(string hierarchy, string key, int tipo)
        {
            string value = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_setting_hierarchy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(hierarchy)));
                    cmd.Parameters.Add(new SqlParameter("@key", Convert.ToString(key)));
                    cmd.Parameters.Add(new SqlParameter("@tipo", tipo));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            value = Convert.ToString(reader["Value"]);
                        }
                    }
                }
                else
                {
                    value = "0";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }
            return value;
        }

    }
}