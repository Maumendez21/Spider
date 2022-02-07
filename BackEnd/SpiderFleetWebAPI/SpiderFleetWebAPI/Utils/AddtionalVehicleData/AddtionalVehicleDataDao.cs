using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.AddtionalVehicleData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.AddtionalVehicleData
{
    public class AddtionalVehicleDataDao
    {

        public AddtionalVehicleDataDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();

        private const int longitudId = 4;
        private const int longitudVin = 17;
        private const int longitudPlaca = 12;
        private const int longitudPoliza = 50;

        public AddtionalVehicleDataResponse Create(CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleData additional)
        {
            AddtionalVehicleDataResponse response = new AddtionalVehicleDataResponse();
            int respuesta = 0;

            try
            {
                if (string.IsNullOrEmpty(additional.Device.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Dispositivo");
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
                response = IsValid(additional);
                if (!response.success) { return response; }
                response.success = false;
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
                    SqlCommand cmd = new SqlCommand("ad.sp_create_or_update_datos_adicionales_vehiculo", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(additional.Device)));
                    
                    if (!string.IsNullOrEmpty(additional.IdMarca)) { cmd.Parameters.Add(new SqlParameter("@idmark", Convert.ToString(additional.IdMarca))); }
                    else { cmd.Parameters.Add(new SqlParameter("@idmark", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(additional.IdModelo)) { cmd.Parameters.Add(new SqlParameter("@idmodel", Convert.ToString(additional.IdModelo))); }
                    else { cmd.Parameters.Add(new SqlParameter("@idmodel", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(additional.IdVersion)) { cmd.Parameters.Add(new SqlParameter("@idversion", Convert.ToString(additional.IdVersion))); }
                    else { cmd.Parameters.Add(new SqlParameter("@idversion", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(additional.VIN)) { cmd.Parameters.Add(new SqlParameter("@vin", Convert.ToString(additional.VIN))); }
                    else { cmd.Parameters.Add(new SqlParameter("@vin", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(additional.Placas)) { cmd.Parameters.Add(new SqlParameter("@plate", Convert.ToString(additional.Placas))); }
                    else { cmd.Parameters.Add(new SqlParameter("@plate", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(additional.Poliza)) { cmd.Parameters.Add(new SqlParameter("@policy", Convert.ToString(additional.Poliza))); }
                    else { cmd.Parameters.Add(new SqlParameter("@policy", DBNull.Value)); }

                    cmd.Parameters.Add(new SqlParameter("@idtypevehicle", Convert.ToInt32(additional.IdTipoVehiculo)));

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

        public AddtionalVehicleDataListResponse Read(string hierarchy, string search)
        {
            AddtionalVehicleDataListResponse response = new AddtionalVehicleDataListResponse();
            List<CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleDataRegistry> ListVehicleData = new List<CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleDataRegistry>(); 
            CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleDataRegistry vehicleData = new CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleDataRegistry();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_datos_adicionales", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@searchNode", Convert.ToString(hierarchy)));
                    cmd.Parameters.Add(new SqlParameter("@search", string.IsNullOrEmpty(search) ? "" : search));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            vehicleData = new CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleDataRegistry();
                            vehicleData.Device = Convert.ToString(reader["ID_Vehiculo"]);
                            vehicleData.Name = Convert.ToString(reader["name"]);
                            vehicleData.IdMarca = Convert.ToString(reader["IdMark"]);
                            vehicleData.Marca = Convert.ToString(reader["descripcion_marca"]);
                            vehicleData.IdModelo = Convert.ToString(reader["IdModel"]);
                            vehicleData.Modelo = Convert.ToString(reader["descripcion_modelo"]);
                            vehicleData.IdVersion = Convert.ToString(reader["IdVersion"]);
                            vehicleData.Version = Convert.ToString(reader["descripcion_version"]);
                            vehicleData.VIN = Convert.ToString(reader["VIN"]).ToUpper();
                            vehicleData.Placas = Convert.ToString(reader["Plate"]).ToUpper();
                            vehicleData.Poliza = Convert.ToString(reader["Policy"]).ToUpper();
                            vehicleData.IdTipoVehiculo = Convert.ToInt32(reader["IdTypeVehicle"]);
                            vehicleData.TipoVehiculo = Convert.ToString(reader["tipo_vehiculo"]).ToUpper();
                            ListVehicleData.Add(vehicleData);
                        }
                        reader.Close();
                        response.success = true;
                        response.ListAddtional = ListVehicleData;
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

        public AddtionalVehicleDataRegistryResponse ReadId(string device)
        {
            AddtionalVehicleDataRegistryResponse response = new AddtionalVehicleDataRegistryResponse();
            CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleDataRegistry vehicleData = new CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleDataRegistry();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_datos_adicionales_by_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vehicleData = new CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleDataRegistry();
                            vehicleData.Device = Convert.ToString(reader["ID_Vehiculo"]);
                            vehicleData.Name = Convert.ToString(reader["name"]);
                            vehicleData.IdMarca = Convert.ToString(reader["IdMark"]);
                            vehicleData.Marca = Convert.ToString(reader["descripcion_marca"]);
                            vehicleData.IdModelo = Convert.ToString(reader["IdModel"]);
                            vehicleData.Modelo = Convert.ToString(reader["descripcion_modelo"]);
                            vehicleData.IdVersion = Convert.ToString(reader["IdVersion"]);
                            vehicleData.Version = Convert.ToString(reader["descripcion_version"]);
                            vehicleData.VIN = Convert.ToString(reader["VIN"]).ToUpper();
                            vehicleData.Placas = Convert.ToString(reader["Plate"]).ToUpper();
                            vehicleData.Poliza = Convert.ToString(reader["Policy"]).ToUpper();
                            vehicleData.IdTipoVehiculo = Convert.ToInt32(reader["IdTypeVehicle"]);
                            vehicleData.TipoVehiculo = Convert.ToString(reader["tipo_vehiculo"]).ToUpper();
                        }
                        reader.Close();
                        response.success = true;
                        response.addtional = vehicleData;
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
        /// Metodo que valida las longitudes de los campos y Validacion de caratcteres extraños
        /// </summary>
        /// <param name="additional"></param>
        /// <returns></returns>
        private AddtionalVehicleDataResponse IsValid(CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleData additional)
        {
            AddtionalVehicleDataResponse response = new AddtionalVehicleDataResponse();

            if (!string.IsNullOrEmpty(additional.IdMarca.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(additional.IdMarca.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(additional.IdMarca.Trim(), longitudId))
                {
                    response.success = false;
                    response.messages.Add("La longitud del ID Marca excede de lo establecido rango maximo " + longitudId + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(additional.IdModelo.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(additional.IdModelo.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(additional.IdModelo.Trim(), longitudId))
                {
                    response.success = false;
                    response.messages.Add("La longitud ID Modelo excede de lo establecido rango maximo " + longitudId + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(additional.IdVersion.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(additional.IdVersion.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(additional.IdVersion.Trim(), longitudId))
                {
                    response.success = false;
                    response.messages.Add("La longitud ID Version excede de lo establecido rango maximo " + longitudId + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(additional.VIN.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(additional.VIN.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(additional.VIN.Trim(), longitudVin))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo VIN excede de lo establecido rango maximo " + longitudVin + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(additional.Placas.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(additional.Placas.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(additional.Placas.Trim(), longitudPlaca))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Placa excede de lo establecido rango maximo " + longitudPlaca + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(additional.Poliza.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(additional.Poliza.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(additional.Poliza.Trim(), longitudPoliza))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Placa excede de lo establecido rango maximo " + longitudPoliza + " caracteres");
                    return response;
                }
            }
            
            response.success = true;
            return response;
        }
    }
}