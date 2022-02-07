using CredencialSpiderFleet.Models.Configuration;
using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Configuration
{
    public class ConfigurationDao
    {
        public ConfigurationDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private UseFul use = new UseFul();

        public ConfigurationResponse Update(CredencialSpiderFleet.Models.Configuration.Configuration configuration)
        {
            ConfigurationResponse response = new ConfigurationResponse();
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_configuration", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    if(configuration.Value.Contains(":"))
                    {
                        configuration.Value = UseFul.CalcularTime(configuration.Value);
                    }

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(configuration.Id)));
                    cmd.Parameters.Add(new SqlParameter("@Value", configuration.Value));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Error al intenar actualizar el Evento, ya existe la combinacion");
                        return response;
                    }
                    if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("El Evento que tratas de actualizar no existe, verifique por favor");
                        return response;
                    }
                    else if (respuesta == 1)
                    {
                        response.success = true;
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

        public ConfigurationListResponse Read(string node)
        {
            ConfigurationListResponse response = new ConfigurationListResponse();
            List<ConfigurationRegistry> listConfiguration = new List<ConfigurationRegistry>();
            ConfigurationRegistry configuration = new ConfigurationRegistry();
            node = use.hierarchyPrincipalToken(node);

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_configuration", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", node));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            configuration = new ConfigurationRegistry();
                            configuration.Id = Convert.ToInt32(reader["Id"]);
                            configuration.Value = Convert.ToString(reader["Value"]);
                            configuration.Description = Convert.ToString(reader["Descripcion"]);
                            
                            if (Convert.ToString(reader["Keys"]).Equals("ITE") | Convert.ToString(reader["Keys"]).Equals("WTC"))
                            {
                                configuration.Value = UseFul.CalcularTime(Convert.ToInt32(configuration.Value));
                            }
                            
                            listConfiguration.Add(configuration);
                        }
                        reader.Close();
                        response.success = true;
                        response.ListResgistry= listConfiguration;
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

        public ConfigurationRegistryResponse Read(int Id)
        {
            ConfigurationRegistryResponse response = new ConfigurationRegistryResponse();
            ConfigurationRegistry configuration = new ConfigurationRegistry();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_id_configuration", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(Id)));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            configuration = new ConfigurationRegistry();
                            configuration.Id = Convert.ToInt32(reader["Id"]);
                            configuration.Value = Convert.ToString(reader["Value"]);
                            configuration.Description = Convert.ToString(reader["Descripcion"]);

                            if (Convert.ToString(reader["Keys"]).Equals("ITE") | Convert.ToString(reader["Keys"]).Equals("WTC"))
                            {
                                configuration.Value = UseFul.CalcularTime(Convert.ToInt32(configuration.Value));
                            }
                        }
                        reader.Close();
                        response.success = true;
                        response.Registry = configuration;
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