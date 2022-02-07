using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.Obd;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Obds
{
    public class ObdDao
    {
        public ObdDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private UseFul use = new UseFul();
        
        private const int longitudDevice = 30;
        private const int longitudLabel = 50;
        private const int longitudName = 100;

        public ObdResponse Create(CredencialSpiderFleet.Models.Obd.Obd obd)
        {
            ObdResponse response = new ObdResponse();

            if (string.IsNullOrEmpty(obd.Hierarchy.Trim()))
            {
                response.success = false;
                response.messages.Add("Ingrese la Empresa");
                return response;
            }

            if (string.IsNullOrEmpty(obd.IdDevice.Trim()))
            {
                response.success = false;
                response.messages.Add("Ingrese el numero de Dispositivo");
                return response;
            }

            if (string.IsNullOrEmpty(obd.Label.Trim()))
            {
                response.success = false;
                response.messages.Add("Ingrese la el valor de Label");
                return response;
            }

            if(ReadVerify(obd.IdDevice))
            {
                response.success = false;
                response.messages.Add("El Device que ingresaste ya existe, Ingrese uno diferente");
                return response;
            }

            //Validaciones de longitud y Caracteres especiales
            try
            {
                response = IsValid(obd);
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
                    SqlCommand cmd = new SqlCommand("ad.sp_create_obd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(obd.IdDevice)));
                    cmd.Parameters.Add(new SqlParameter("@label", Convert.ToString(obd.Label)));

                    if (!string.IsNullOrEmpty(obd.Name)) { cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(obd.Name))); }
                    else { cmd.Parameters.Add(new SqlParameter("@name", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(obd.Hierarchy)) { cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(obd.Hierarchy))); }
                    else { cmd.Parameters.Add(new SqlParameter("@hierarchy", DBNull.Value)); }

                    if (obd.IdType != null) { cmd.Parameters.Add(new SqlParameter("@idtype", Convert.ToString(obd.IdType))); }
                    else { cmd.Parameters.Add(new SqlParameter("@idtype", DBNull.Value)); }

                    if (obd.IdSim != null) { cmd.Parameters.Add(new SqlParameter("@idsim", Convert.ToString(obd.IdSim))); }
                    else { cmd.Parameters.Add(new SqlParameter("@idsim", DBNull.Value)); }

                    cmd.Parameters.Add(new SqlParameter("@status", Convert.ToString(obd.Status)));

                    cmd.Parameters.Add(new SqlParameter("@motor", Convert.ToInt32(obd.Motor)));
                    cmd.Parameters.Add(new SqlParameter("@panico", Convert.ToInt32(obd.Panico)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    try
                    {
                        if (respuesta ==3)
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

        public ObdResponse Update(CredencialSpiderFleet.Models.Obd.ObdUpdate obd)
        {
            ObdResponse response = new ObdResponse();

            if (string.IsNullOrEmpty(obd.Hierarchy.Trim()))
            {
                response.success = false;
                response.messages.Add("Ingrese la Empresa");
                return response;
            }

            if (string.IsNullOrEmpty(obd.IdDevice.Trim()))
            {
                response.success = false;
                response.messages.Add("Ingrese el numero de Dispositivo");
                return response;
            }

            if (string.IsNullOrEmpty(obd.Label.Trim()))
            {
                response.success = false;
                response.messages.Add("Ingrese la el valor de Label");
                return response;
            }


            //Validaciones de longitud y Caracteres especiales
            try
            {
                response = IsValidUpdate(obd);
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
                    SqlCommand cmd = new SqlCommand("ad.sp_update_obd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(obd.IdDevice)));
                    if (!string.IsNullOrEmpty(obd.IdDeviceAnt)) { cmd.Parameters.Add(new SqlParameter("@device_ant", Convert.ToString(obd.IdDeviceAnt))); }
                    else { cmd.Parameters.Add(new SqlParameter("@device_ant", "")); }

                    cmd.Parameters.Add(new SqlParameter("@label", Convert.ToString(obd.Label)));

                    if (!string.IsNullOrEmpty(obd.Name)) { cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(obd.Name))); }
                    else { cmd.Parameters.Add(new SqlParameter("@name", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(obd.Hierarchy)) { cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(obd.Hierarchy))); }
                    else { cmd.Parameters.Add(new SqlParameter("@hierarchy", DBNull.Value)); }

                    if (obd.IdType != null) { cmd.Parameters.Add(new SqlParameter("@idtype", Convert.ToString(obd.IdType))); }
                    else { cmd.Parameters.Add(new SqlParameter("@idtype", DBNull.Value)); }

                    if (obd.IdSim != null) { cmd.Parameters.Add(new SqlParameter("@idsim", Convert.ToString(obd.IdSim))); }
                    else { cmd.Parameters.Add(new SqlParameter("@idsim", DBNull.Value)); }

                    cmd.Parameters.Add(new SqlParameter("@status", Convert.ToString(obd.Status)));

                    cmd.Parameters.Add(new SqlParameter("@motor", Convert.ToInt32(obd.Motor)));
                    cmd.Parameters.Add(new SqlParameter("@panico", Convert.ToInt32(obd.Panico)));

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

        public ObdListResponse Read(string hierarchy)
        {
            ObdListResponse response = new ObdListResponse();
            List<CredencialSpiderFleet.Models.Obd.ObdRegistry> listObd = new List<CredencialSpiderFleet.Models.Obd.ObdRegistry>();
            CredencialSpiderFleet.Models.Obd.ObdRegistry obd = new CredencialSpiderFleet.Models.Obd.ObdRegistry();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_ad_consult_obd_hyerarchy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(hierarchy)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obd = new CredencialSpiderFleet.Models.Obd.ObdRegistry();
                            obd.IdDevice = Convert.ToString(reader["idDevice"]);
                            obd.Label = Convert.ToString(reader["label"]);
                            obd.Hierarchy = (reader["hierarchy"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["hierarchy"]));
                            obd.IdType = (reader["idType"] == DBNull.Value) ? (int?)null : ((int)reader["idType"]);
                            obd.Description = Convert.ToString(reader["description"]);
                            obd.IdSim = (reader["idSim"] == DBNull.Value) ? (int?)null : ((int)reader["idSim"]);
                            obd.Sim = Convert.ToString(reader["sim"]);
                            obd.Status = Convert.ToInt32(reader["status"]);
                            obd.Name = (reader["Nombre"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Nombre"]));
                            obd.Panico = Convert.ToInt32(reader["panico"]);
                            obd.Motor = Convert.ToInt32(reader["motor"]);

                            listObd.Add(obd);
                        }
                        reader.Close();
                        response.success = true;
                        response.listObd = listObd;
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

        public ObdRegistryResponse ReadId(string hierarchy, string device)
        {
            ObdRegistryResponse response = new ObdRegistryResponse();
            CredencialSpiderFleet.Models.Obd.ObdRegistry obd = new CredencialSpiderFleet.Models.Obd.ObdRegistry();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_ad_consult_obd_hyerarchy_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(hierarchy)));
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obd = new CredencialSpiderFleet.Models.Obd.ObdRegistry();
                            obd.IdDevice = Convert.ToString(reader["idDevice"]);
                            obd.Label = Convert.ToString(reader["label"]);
                            obd.Hierarchy = (reader["hierarchy"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["hierarchy"]));
                            obd.IdType = (reader["idType"] == DBNull.Value) ? (int?)null : ((int)reader["idType"]);
                            obd.Description = Convert.ToString(reader["description"]);
                            obd.IdSim = (reader["idSim"] == DBNull.Value) ? (int?)null : ((int)reader["idSim"]);
                            obd.Sim = Convert.ToString(reader["sim"]);                            
                            obd.Status = Convert.ToInt32(reader["status"]);
                            obd.Name = (reader["Nombre"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Nombre"]));
                            obd.Panico = Convert.ToInt32(reader["panico"]);
                            obd.Motor = Convert.ToInt32(reader["motor"]);
                        }
                        reader.Close();
                        response.success = true;
                        response.obd = obd;
                    }
                }
                else
                {
                    response.success = false;
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
            return response;
        }

        private bool ReadVerify(string device)
        {
            bool respuesta = false;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_obd_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {                            
                            if(Convert.ToInt32(reader["existe"]) > 1)
                            {
                                respuesta = true;
                            }
                        }
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




        ///// <summary>
        ///// Metodo que obtiene los dispositivos por el id de la sub compañia
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        public List<CredencialSpiderFleet.Models.Obd.ObdHierarchy> ReadIdSubCompanyHierarchy(string id, string name)
        {
            List<CredencialSpiderFleet.Models.Obd.ObdHierarchy> listDevice = new List<CredencialSpiderFleet.Models.Obd.ObdHierarchy>();
            CredencialSpiderFleet.Models.Obd.ObdHierarchy obd = new CredencialSpiderFleet.Models.Obd.ObdHierarchy();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_device_hierarchy_list", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@idsubcompany", Convert.ToString(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obd = new CredencialSpiderFleet.Models.Obd.ObdHierarchy();
                            obd.Device = Convert.ToString(reader["idDevice"]);
                            obd.IdSubCompany = Convert.ToString(reader["idSubCompany"]);
                            obd.NameSubCompany = name;

                            listDevice.Add(obd);
                        }
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
            return listDevice;
        }

        /// <summary>
        /// Metodo que valida las longitudes de los campos y Validacion de caratcteres extraños
        /// </summary>
        /// <param name="obdRequest"></param>
        /// <returns></returns>
        private ObdResponse IsValid(CredencialSpiderFleet.Models.Obd.Obd obdRequest)
        {
            ObdResponse response = new ObdResponse();

            //Campo Device
            if (!string.IsNullOrEmpty(obdRequest.IdDevice.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(obdRequest.IdDevice.Trim()))
                {
                    response.success = false;
                    response.messages.Add("El campo Device contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(obdRequest.IdDevice.Trim(), longitudDevice))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Device excede de lo establecido rango maximo " + longitudDevice + " caracteres");
                    return response;
                }
            }

            //Campo Label 
            if (!string.IsNullOrEmpty(obdRequest.Label.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(obdRequest.Label.Trim()))
                {
                    response.success = false;
                    response.messages.Add("El campo Label contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(obdRequest.Label.Trim(), longitudLabel))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Label excede de lo establecido rango maximo " + longitudLabel + " caracteres");
                    return response;
                }
            }

            //Campo Name
            if (!string.IsNullOrEmpty(obdRequest.Name.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(obdRequest.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("El campo Name contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(obdRequest.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Name excede de lo establecido rango maximo " + longitudName + " caracteres");
                    return response;
                }
            }

            response.success = true;

            return response;
        }

        /// <summary>
        /// Metodo que valida las longitudes de los campos y Validacion de caratcteres extraños
        /// </summary>
        /// <param name="obdRequest"></param>
        /// <returns></returns>
        private ObdResponse IsValidUpdate(CredencialSpiderFleet.Models.Obd.ObdUpdate obdRequest)
        {
            ObdResponse response = new ObdResponse();

            //Campo Device
            if (!string.IsNullOrEmpty(obdRequest.IdDevice.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(obdRequest.IdDevice.Trim()))
                {
                    response.success = false;
                    response.messages.Add("El campo IdDevice cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(obdRequest.IdDevice.Trim(), longitudDevice))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Device excede de lo establecido rango maximo " + longitudDevice + " caracteres");
                    return response;
                }
            }

            //Campo Ant Device
            if (!string.IsNullOrEmpty(obdRequest.IdDeviceAnt.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(obdRequest.IdDeviceAnt.Trim()))
                {
                    response.success = false;
                    response.messages.Add("El campo IdDeviceAnt cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(obdRequest.IdDeviceAnt.Trim(), longitudDevice))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo IdDeviceAnt excede de lo establecido rango maximo " + longitudDevice + " caracteres");
                    return response;
                }
            }

            //Campo Label 
            if (!string.IsNullOrEmpty(obdRequest.Label.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(obdRequest.Label.Trim()))
                {
                    response.success = false;
                    response.messages.Add("El campo Label contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(obdRequest.Label.Trim(), longitudLabel))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Label excede de lo establecido rango maximo " + longitudLabel + " caracteres");
                    return response;
                }
            }

            //Campo Name
            if (!string.IsNullOrEmpty(obdRequest.Name.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(obdRequest.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("El Campo Name contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(obdRequest.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Name excede de lo establecido rango maximo " + longitudName + " caracteres");
                    return response;
                }
            }
           
            response.success = true;

            return response;
        }

    }
}