using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Inventory.TypeDevice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Inventory.TypeDevice
{
    public class TypeDeviceDao
    {
        public TypeDeviceDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();
        private const int longitudName = 50;
        private const int longitudDescripcion = 50;

        /// <summary>
        /// Creacion de Usurio
        /// </summary>
        public TypeDeviceResponse Create(CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices typeDevice)
        {
            TypeDeviceResponse response = new TypeDeviceResponse();
            int respuesta = 0;

            try
            {
                if (string.IsNullOrEmpty(typeDevice.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Nombre ");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            if (!string.IsNullOrEmpty(typeDevice.Name.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(typeDevice.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(typeDevice.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud excede de lo establecido rango maximo " + longitudName + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(typeDevice.Description.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(typeDevice.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(typeDevice.Description.Trim(), longitudDescripcion))
                {
                    response.success = false;
                    response.messages.Add("La longitud excede de lo establecido rango maximo " + longitudDescripcion + " caracteres");
                    return response;
                }
            }

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_type_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(typeDevice.Name)));
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(typeDevice.Description)));

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
        public TypeDeviceResponse Update(CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices typeDevice)
        {
            TypeDeviceResponse response = new TypeDeviceResponse();
            int respuesta = 0;

            try
            {
                if (typeDevice.idTypeDevice == 0)
                {
                    response.success = false;
                    response.messages.Add("No tiene el parametro Id");
                    return response;
                }

                if (string.IsNullOrEmpty(typeDevice.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Nombre ");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            if (!string.IsNullOrEmpty(typeDevice.Name.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(typeDevice.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(typeDevice.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud excede de lo establecido rango maximo " + longitudName + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(typeDevice.Description.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(typeDevice.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(typeDevice.Description.Trim(), longitudDescripcion))
                {
                    response.success = false;
                    response.messages.Add("La longitud excede de lo establecido rango maximo " + longitudDescripcion + " caracteres");
                    return response;
                }
            }

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_type_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(typeDevice.idTypeDevice)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(typeDevice.Name)));
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(typeDevice.Description)));

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
        public TypeDeviceListResponse Read()
        {
            TypeDeviceListResponse response = new TypeDeviceListResponse();
            List<CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices> listTypeDevice = new List<CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices>();
            CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices typeDevice = new CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_type_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            typeDevice = new CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices();
                            typeDevice.idTypeDevice = Convert.ToInt32(reader["idTypeDevice"]);
                            typeDevice.Name = Convert.ToString(reader["name"]);
                            typeDevice.Description = Convert.ToString(reader["description"]);
                            listTypeDevice.Add(typeDevice);
                        }
                        reader.Close();
                        response.success = true;
                        response.listTypeDevice = listTypeDevice;
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
        public TypeDeviceRegistryResponse ReadId(int id)
        {
            TypeDeviceRegistryResponse response = new TypeDeviceRegistryResponse();
            CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices typeDevice = new CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_id_type_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            typeDevice = new CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices();
                            typeDevice.idTypeDevice = Convert.ToInt32(reader["idTypeDevice"]);
                            typeDevice.Name = Convert.ToString(reader["name"]);
                            typeDevice.Description = Convert.ToString(reader["description"]);
                        }
                        reader.Close();
                        response.success = true;
                        response.typeDevice = typeDevice;
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