using CredencialSpiderFleet.Models.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Setting
{
    public class SettingConfig
    {
        public SettingConfig() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public string Create(string keys,int tipo, string hierarchy, string value, string drescripcion)
        {
            string respuesta = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_setting", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@keys", Convert.ToString(keys)));
                    cmd.Parameters.Add(new SqlParameter("@tipo", Convert.ToInt32(tipo)));
                    cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(hierarchy)));
                    cmd.Parameters.Add(new SqlParameter("@value", Convert.ToString(value)));
                    cmd.Parameters.Add(new SqlParameter("@descripcion", Convert.ToString(drescripcion)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToString(sqlParameter.Value.ToString());

                    try
                    {
                        if (respuesta.Contains("ERROR"))
                        {
                            return respuesta;
                        }
                        else
                        {
                            return respuesta;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
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
            return respuesta;
        }

        public string ReadId(string username, string key, int tipo)
        {
            string value = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_setting", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    cmd.Parameters.Add(new SqlParameter("@key", Convert.ToString(key)));
                    cmd.Parameters.Add(new SqlParameter("@tipo", tipo));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            value  = Convert.ToString(reader["Value"]);
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