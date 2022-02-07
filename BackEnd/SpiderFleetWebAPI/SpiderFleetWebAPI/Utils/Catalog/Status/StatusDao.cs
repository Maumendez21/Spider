using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Catalog.Status;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Catalog.Status
{
    public class StatusDao
    {
        public StatusDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();
        private const int longitudDescription = 50;

        /// <summary>
        /// Creacion de Usurio
        /// </summary>
        public StatusResponse Create(CredencialSpiderFleet.Models.Catalogs.Status.Status status)
        {
            StatusResponse response = new StatusResponse();
            int respuesta = 0;

            try
            {
                if (string.IsNullOrEmpty(status.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese la Descripción");
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            if (!string.IsNullOrEmpty(status.Description.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(status.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(status.Description.Trim(), longitudDescription))
                {
                    response.success = false;
                    response.messages.Add("La longitud excede de lo establecido rango maximo " + longitudDescription + " caracteres");
                    return response;
                }
            }

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_status", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(status.Description)));

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
                        response.messages.Add("Error al intenar dar de alta el registro");
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

        /// <summary>
        /// Actualizacion de Usuario
        /// </summary>
        public StatusResponse Update(CredencialSpiderFleet.Models.Catalogs.Status.Status status)
        {
            StatusResponse response = new StatusResponse();
            int respuesta = 0;
            try
            {
                if (status.IdStatus == 0)
                {
                    response.success = false;
                    response.messages.Add("No tiene el parametro idRol");
                    return response;
                }

                if (string.IsNullOrEmpty(status.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese la Descripción");
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            if (!string.IsNullOrEmpty(status.Description.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(status.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(status.Description.Trim(), longitudDescription))
                {
                    response.success = false;
                    response.messages.Add("La longitud excede de lo establecido rango maximo " + longitudDescription + " caracteres");
                    return response;
                }
            }
            
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_status", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(status.IdStatus)));
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(status.Description)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    if (respuesta == 1)
                    {
                        response.success = true;
                    }
                    else if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("No se encuentra el registro");
                        return response;
                    }
                    else if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Error al tratar de actualizar el registro");
                        return response;
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

        /// <summary>
        /// Consulta de Usuarios con estatus 1
        /// </summary>
        public StatusListResponse Read()
        {
            StatusListResponse response = new StatusListResponse();
            List<CredencialSpiderFleet.Models.Catalogs.Status.Status> listStatus = new List<CredencialSpiderFleet.Models.Catalogs.Status.Status>();
            CredencialSpiderFleet.Models.Catalogs.Status.Status status = new CredencialSpiderFleet.Models.Catalogs.Status.Status();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_status", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            status = new CredencialSpiderFleet.Models.Catalogs.Status.Status();
                            status.IdStatus = Convert.ToInt32(reader["idStatus"]);
                            status.Description = Convert.ToString(reader["description"]);
                            listStatus.Add(status);
                        }
                        reader.Close();
                        response.success = true;
                        response.listStatus = listStatus;
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

        /// <summary>
        /// Consulta de Ususario por id
        /// </summary>
        public StatusRegistryResponse ReadId(int id)
        {
            StatusRegistryResponse response = new StatusRegistryResponse();
            CredencialSpiderFleet.Models.Catalogs.Status.Status status = new CredencialSpiderFleet.Models.Catalogs.Status.Status();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_id_status", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            status = new CredencialSpiderFleet.Models.Catalogs.Status.Status();
                            status.IdStatus = Convert.ToInt32(reader["idStatus"]);
                            status.Description = Convert.ToString(reader["description"]);
                        }
                        reader.Close();
                        response.success = true;
                        response.status = status;
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