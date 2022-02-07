using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Inventory.CatalogStatusDevices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Inventory.CatalogStatusDevices
{
    public class CatalogStatusDevicesDao
    {
        public CatalogStatusDevicesDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        /// <summary>
        /// Creacion de Usurio
        /// </summary>
        public CatalogStatusDevicesResponse Create(CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices statusDevice)
        {
            CatalogStatusDevicesResponse response = new CatalogStatusDevicesResponse();
            int respuesta = 0;

            try
            {
                if (string.IsNullOrEmpty(statusDevice.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Nombre del Status");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }


            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_catalog_status_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(statusDevice.Name)));
                    
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
        public CatalogStatusDevicesResponse Update(CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices statusDevice)
        {
            CatalogStatusDevicesResponse response = new CatalogStatusDevicesResponse();
            int respuesta = 0;

            try
            {
                if (statusDevice.IdStatus == 0)
                {
                    response.success = false;
                    response.messages.Add("No tiene el parametro Id");
                    return response;
                }

                if (string.IsNullOrEmpty(statusDevice.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Nombre del Status");
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_catalog_status_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(statusDevice.IdStatus)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(statusDevice.Name)));

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
        public CatalogStatusDevicesListResponse Read()
        {
            CatalogStatusDevicesListResponse response = new CatalogStatusDevicesListResponse();
            List<CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices> lisDevices = new List<CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices>();
            CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices device = new CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_catalog_status_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            device = new CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices();
                            device.IdStatus = Convert.ToInt32(reader["idStatus"]);
                            device.Name = Convert.ToString(reader["name"]);
                            lisDevices.Add(device);
                        }
                        reader.Close();
                        response.success = true;
                        response.listStatusDevices = lisDevices;
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
        public CatalogStatusDevicesRegistryResponse ReadId(int id)
        {
            CatalogStatusDevicesRegistryResponse response = new CatalogStatusDevicesRegistryResponse();
            CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices status = new CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_id_catalog_status_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            status = new CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices();
                            status.IdStatus = Convert.ToInt32(reader["idStatus"]);
                            status.Name = Convert.ToString(reader["name"]);
                        }
                        reader.Close();
                        response.success = true;
                        response.statusDevice = status;
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