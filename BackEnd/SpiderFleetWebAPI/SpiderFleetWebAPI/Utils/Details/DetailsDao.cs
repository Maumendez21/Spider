using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Details;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Details
{
    public class DetailsDao
    {
        /// <summary>
        /// 
        /// </summary>
        public DetailsDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        /// <summary>
        /// Creacion de 
        /// </summary>
        public DetailsResponse Create(CredencialSpiderFleet.Models.Details.Details details)
        {
            DetailsResponse response = new DetailsResponse();
            int respuesta = 0;

            if (string.IsNullOrEmpty(details.Device.Trim()))
            {
                response.success = false;
                response.messages.Add("Ingrese el numero de dispositivo");
                return response;
            }


            if (details.TypeDevice == 0)
            {
                response.success = false;
                response.messages.Add("Ingrese el tipo de dispositivo");
                return response;
            }

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_update_details_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(details.Device)));
                    cmd.Parameters.Add(new SqlParameter("@id_type_device", Convert.ToInt32(details.TypeDevice)));
                    cmd.Parameters.Add(new SqlParameter("@model", Convert.ToString(details.Model)));
                    cmd.Parameters.Add(new SqlParameter("@id_communication_method", Convert.ToInt32(details.IdCommunicationMethod)));
                    cmd.Parameters.Add(new SqlParameter("@batery", Convert.ToString(details.Batery)));
                    cmd.Parameters.Add(new SqlParameter("@battery_duration", Convert.ToString(details.BatteryDuration)));
                    cmd.Parameters.Add(new SqlParameter("@id_sampling_time", Convert.ToInt32(details.IdSamplingTime)));
                    cmd.Parameters.Add(new SqlParameter("@motorized", Convert.ToString(details.Motorized)));
                    cmd.Parameters.Add(new SqlParameter("@performance", Convert.ToString(details.Performance)));

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
                        response.messages.Add("Error al intenar dar de alta el la imagen");
                        return response;
                    }
                    if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("La Compañia no existe, verifique por favor");
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

        private CredencialSpiderFleet.Models.Details.DetailsRegistry MapToValue(SqlDataReader reader)
        {
            return new CredencialSpiderFleet.Models.Details.DetailsRegistry()
            {
                Device = reader["device"].ToString(),
                Name = reader["name"].ToString(),
                TypeDevice = Convert.ToInt32(reader["type_device"].ToString()),
                DescriptionType = reader["description_type"].ToString(),
                Model = reader["model"].ToString(),
                IdCommunicationMethod = Convert.ToInt32(reader["communication_method"].ToString()),
                DescriptionCommunicationMethod = reader["description_method"].ToString(),
                Batery = Convert.ToInt32(reader["batery"].ToString()),
                BatteryDuration = reader["battery_duration"].ToString(),
                IdSamplingTime = Convert.ToInt32(reader["sampling_time"].ToString()),
                DescriptionSamplingTime = reader["description_time"].ToString(),
                Motorized = Convert.ToInt32(reader["motorized"].ToString()),
                Performance = Convert.ToDouble(reader["performance"].ToString())
            };
        }

        public DetailsListResponse Read(string node, string search)
        {
            DetailsListResponse response = new DetailsListResponse();

            var listDetails = new List<CredencialSpiderFleet.Models.Details.DetailsRegistry>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_consult_list_details", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@node", node));
                        cmd.Parameters.Add(new SqlParameter("@search", string.IsNullOrEmpty(search)? "" : search));

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listDetails.Add(MapToValue(reader));
                            }

                            reader.Close();
                        }

                        if(listDetails.Count > 0)
                        {
                            response.ListDetails = listDetails;
                            response.success = true;
                        }                        

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

        public DetailsRegistryResponse ReadId(string device)
        {
            DetailsRegistryResponse response = new DetailsRegistryResponse();

            var detail = new CredencialSpiderFleet.Models.Details.DetailsRegistry();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_consult_detailsby_device", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@device", device));

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                detail = MapToValue(reader);
                                response.Registry = detail;
                                response.success = true;
                            }

                            reader.Close();
                        }
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