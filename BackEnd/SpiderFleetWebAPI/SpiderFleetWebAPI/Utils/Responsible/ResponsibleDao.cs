using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response;
using SpiderFleetWebAPI.Models.Response.Responsible;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Responsible
{
    public class ResponsibleDao
    {
        public ResponsibleDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private UseFul use = new UseFul();

        private const int longitudName = 50;
        private const int longitudEmail = 50;
        private const int longitudTelephone = 12;
        private const int longitudArea = 30;        

        public ResponsibleResponse Create(CredencialSpiderFleet.Models.Responsible.Responsible responsible)
        {
            ResponsibleResponse response = new ResponsibleResponse();

            if (string.IsNullOrEmpty(responsible.Name.Trim()))
            {
                response.success = false;
                response.messages.Add("Ingrese el numero de Dispositivo");
                return response;
            }

            if (string.IsNullOrEmpty(responsible.Email.Trim()))
            {
                response.success = false;
                response.messages.Add("Ingrese la el valor de Label");
                return response;
            }

            //Validaciones de longitud y Caracteres especiales
            try
            {
                response = IsValid(responsible);
                if (!response.success) { return response; }
                response.success = false;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_responsible", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(responsible.Hierarchy)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(responsible.Name)));
                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(responsible.Email)));
                    cmd.Parameters.Add(new SqlParameter("@phone", Convert.ToString(responsible.Phone)));
                    cmd.Parameters.Add(new SqlParameter("@area", Convert.ToString(responsible.Area)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    try
                    {
                        if (respuesta == 3)
                        {
                            response.success = false;
                            response.messages.Add("Error al intenar dar de alta el registro");
                            return response;
                        }
                        else
                        {
                            response.success = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.success = false;
                        response.messages.Add(ex.Message);
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

        public ResponsibleResponse Update(CredencialSpiderFleet.Models.Responsible.ResponsibleUpdate responsible)
        {
            ResponsibleResponse response = new ResponsibleResponse();

            if (string.IsNullOrEmpty(responsible.Name.Trim()))
            {
                response.success = false;
                response.messages.Add("Ingrese el numero de Dispositivo");
                return response;
            }

            if (string.IsNullOrEmpty(responsible.Email.Trim()))
            {
                response.success = false;
                response.messages.Add("Ingrese la el valor de Label");
                return response;
            }

            

            //Validaciones de longitud y Caracteres especiales
            try
            {
                response = IsValid(responsible);
                if (!response.success) { return response; }
                response.success = false;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_responsible", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(responsible.Id)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(responsible.Name)));
                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(responsible.Email)));
                    cmd.Parameters.Add(new SqlParameter("@phone", Convert.ToString(responsible.Phone)));
                    cmd.Parameters.Add(new SqlParameter("@area", Convert.ToString(responsible.Area)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    try
                    {
                        if (respuesta == 3)
                        {
                            response.success = false;
                            response.messages.Add("Error al actulizar el registro");
                            return response;
                        }
                        else
                        {
                            response.success = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.success = false;
                        response.messages.Add(ex.Message);
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

        public ResponsibleListResponse Read(string hierarchy, string search)
        {
            ResponsibleListResponse response = new ResponsibleListResponse();
            List<CredencialSpiderFleet.Models.Responsible.ResponsibleRegistry> listResponsible = new List<CredencialSpiderFleet.Models.Responsible.ResponsibleRegistry>();
            CredencialSpiderFleet.Models.Responsible.ResponsibleRegistry obd = new CredencialSpiderFleet.Models.Responsible.ResponsibleRegistry();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_responsible", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(hierarchy)));
                    cmd.Parameters.Add(new SqlParameter("@search", string.IsNullOrEmpty(search) ? "" : search));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obd = new CredencialSpiderFleet.Models.Responsible.ResponsibleRegistry();
                            obd.Id = Convert.ToInt32(reader["Id"]);
                            //obd.Hierarchy = (reader["Hierarchy"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Hierarchy"]));
                            obd.Name = Convert.ToString(reader["Name"]);
                            obd.Email = Convert.ToString(reader["Email"]);
                            obd.Phone = Convert.ToString(reader["Phone"]);
                            obd.Area = Convert.ToString(reader["Area"]);
                            //obd.Status = Convert.ToInt32(reader["Status"]);

                            listResponsible.Add(obd);
                        }
                        reader.Close();
                        response.success = true;
                        response.listResponsible = listResponsible;
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

        public ResponsibleRegistryResponse ReadId(string hierarchy, string id)
        {
            ResponsibleRegistryResponse response = new ResponsibleRegistryResponse();
            CredencialSpiderFleet.Models.Responsible.ResponsibleRegistry obd = new CredencialSpiderFleet.Models.Responsible.ResponsibleRegistry();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_responsible", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(hierarchy)));
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obd = new CredencialSpiderFleet.Models.Responsible.ResponsibleRegistry();
                            obd.Id = Convert.ToInt32(reader["Id"]);
                            //obd.Hierarchy = (reader["Hierarchy"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Hierarchy"]));
                            obd.Name = Convert.ToString(reader["Name"]);
                            obd.Email = Convert.ToString(reader["Email"]);
                            obd.Phone = Convert.ToString(reader["Phone"]);
                            obd.Area = Convert.ToString(reader["Area"]);
                            //obd.Status = Convert.ToInt32(reader["Status"]);
                        }
                        reader.Close();
                        response.success = true;
                        response.registry = obd;
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

        public ResponsibleVehicleResponse ReadVehicle(string device)
        {
            ResponsibleVehicleResponse response = new ResponsibleVehicleResponse();
            CredencialSpiderFleet.Models.Responsible.ResponsibleVehicle obd = new CredencialSpiderFleet.Models.Responsible.ResponsibleVehicle();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_device_responsable", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obd = new CredencialSpiderFleet.Models.Responsible.ResponsibleVehicle();
                            obd.Device = Convert.ToString(reader["ID_Vehiculo"]);
                            obd.Vehicle = Convert.ToString(reader["nombre_vehiculo"]);
                            obd.Responsible = Convert.ToString(reader["nombre_responsable"]);
                            obd.Email = Convert.ToString(reader["email"]);
                            obd.Phone = Convert.ToString(reader["phone"]);
                            obd.Area = Convert.ToString(reader["area"]);
                            obd.IdDongle = Convert.ToInt32(reader["Dongle"]);
                        }
                        reader.Close();
                        response.success = true;
                        response.responsible = obd;
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

        public ResponsibleVehicleResponse ReadNameVehicle(string device)
        {
            ResponsibleVehicleResponse response = new ResponsibleVehicleResponse();
            CredencialSpiderFleet.Models.Responsible.ResponsibleVehicle obd = new CredencialSpiderFleet.Models.Responsible.ResponsibleVehicle();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_information_vehicle", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obd = new CredencialSpiderFleet.Models.Responsible.ResponsibleVehicle();
                            obd.Vehicle = Convert.ToString(reader["name_vehicle"]);
                            obd.IdDongle = Convert.ToInt32(reader["Dongle"]);
                        }
                        reader.Close();
                        response.success = true;
                        response.responsible = obd;
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

        public ResponsibleVehicleResponse ReadNameVehicle(string device, DateTime start, DateTime end)
        {
            ResponsibleVehicleResponse response = new ResponsibleVehicleResponse();
            CredencialSpiderFleet.Models.Responsible.ResponsibleVehicle obd = new CredencialSpiderFleet.Models.Responsible.ResponsibleVehicle();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_responsable_detail", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    cmd.Parameters.Add(new SqlParameter("@startdate", start));
                    cmd.Parameters.Add(new SqlParameter("@enddate", end));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obd = new CredencialSpiderFleet.Models.Responsible.ResponsibleVehicle();
                            obd.Device = Convert.ToString(reader["Device"]);
                            obd.Vehicle = Convert.ToString(reader["nombre_vehiculo"]);
                            obd.Responsible = Convert.ToString(reader["nombre_responsable"]);
                            obd.Email = Convert.ToString(reader["email"]);
                            obd.Phone = Convert.ToString(reader["phone"]);
                            obd.Area = Convert.ToString(reader["area"]);
                            obd.IdDongle = Convert.ToInt32(reader["idType"]);
                            obd.Dongle = Convert.ToString(reader["dongle"]);
                            obd.Motor = Convert.ToInt32(reader["motor"]);
                            obd.Hierarchy = Convert.ToString(reader["hierarchy"]);
                        }
                        reader.Close();
                        response.success = true;
                        response.responsible = obd;
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

        public ResponsibleRegistryResponse ReadDevice(string device)
        {
            ResponsibleRegistryResponse response = new ResponsibleRegistryResponse();
            CredencialSpiderFleet.Models.Responsible.ResponsibleRegistry obd = new CredencialSpiderFleet.Models.Responsible.ResponsibleRegistry();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_responsible_by_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obd = new CredencialSpiderFleet.Models.Responsible.ResponsibleRegistry();
                            obd.Id = Convert.ToInt32(reader["Id"]);
                            obd.Name = Convert.ToString(reader["Name"]);
                            obd.Email = Convert.ToString(reader["Email"]);
                            obd.Phone = Convert.ToString(reader["Phone"]);
                            obd.Area = Convert.ToString(reader["Area"]);
                        }
                        reader.Close();
                        response.success = true;
                        response.registry = obd;
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
        /// <param name="responsible"></param>
        /// <returns></returns>
        private ResponsibleResponse IsValid(CredencialSpiderFleet.Models.Responsible.Responsible responsible)
        {
            ResponsibleResponse response = new ResponsibleResponse();

            //Campo Nombre
            if (!string.IsNullOrEmpty(responsible.Name.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(responsible.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(responsible.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Nombre excede de lo establecido rango maximo " + longitudName + " caracteres");
                    return response;
                }
            }

            //Campo Email
            if (!string.IsNullOrEmpty(responsible.Email.Trim()))
            {
                use = new UseFul();

                if (!use.IsValidLength(responsible.Email.Trim(), longitudEmail))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Email excede de lo establecido rango maximo " + longitudEmail + " caracteres");
                    return response;
                }
            }         

            //Campo Area
            if (!string.IsNullOrEmpty(responsible.Area.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(responsible.Area.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(responsible.Area.Trim(), longitudArea))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Password excede de lo establecido rango maximo " + longitudArea + " caracteres");
                    return response;
                }
            }

            //Campo Telefono
            if (!string.IsNullOrEmpty(responsible.Phone.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(responsible.Phone.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(responsible.Phone.Trim(), longitudTelephone))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Telefono excede de lo establecido rango maximo " + longitudTelephone + " caracteres");
                    return response;
                }

                response.success = true;
            }
            return response;
        }

        /// <summary>
        /// Metodo que valida las longitudes de los campos y Validacion de caratcteres extraños
        /// </summary>
        /// <param name="responsible"></param>
        /// <returns></returns>
        private ResponsibleResponse IsValid(CredencialSpiderFleet.Models.Responsible.ResponsibleUpdate responsible)
        {
            ResponsibleResponse response = new ResponsibleResponse();

            //Campo Nombre
            if (!string.IsNullOrEmpty(responsible.Name.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(responsible.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(responsible.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Nombre excede de lo establecido rango maximo " + longitudName + " caracteres");
                    return response;
                }
            }

            //Campo Email
            if (!string.IsNullOrEmpty(responsible.Email.Trim()))
            {
                use = new UseFul();

                if (!use.IsValidLength(responsible.Email.Trim(), longitudEmail))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Email excede de lo establecido rango maximo " + longitudEmail + " caracteres");
                    return response;
                }
            }

            //Campo Area
            if (!string.IsNullOrEmpty(responsible.Area.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(responsible.Area.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(responsible.Area.Trim(), longitudArea))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Area excede de lo establecido rango maximo " + longitudArea + " caracteres");
                    return response;
                }
            }

            //Campo Telefono
            if (!string.IsNullOrEmpty(responsible.Phone.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(responsible.Phone.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(responsible.Phone.Trim(), longitudTelephone))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Telefono excede de lo establecido rango maximo " + longitudTelephone + " caracteres");
                    return response;
                }

                response.success = true;
            }

            return response;
        }

        public ResponsibleResponse Delete(string id, string node)
        {
            ResponsibleResponse response = new ResponsibleResponse();

            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_delete_responsible", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    try
                    {
                        if (respuesta == 3)
                        {
                            response.success = false;
                            response.messages.Add("Error al intenar eliminar el registro");
                            return response;
                        }
                        else if (respuesta == 2)
                        {
                            response.success = false;
                            response.messages.Add("Error el registro que intenta eliminar no se encuentra, verifique");
                            return response;
                        }
                        else
                        {
                            response.success = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.success = false;
                        response.messages.Add(ex.Message);
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


    }
}