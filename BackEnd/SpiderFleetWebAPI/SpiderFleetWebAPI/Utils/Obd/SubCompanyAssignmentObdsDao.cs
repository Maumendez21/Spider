using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Obd;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.Obd;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Obd
{
    public class SubCompanyAssignmentObdsDao
    {

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private UseFul use = new UseFul();

        private const int longitudDevice = 30;
        private const int longitudName = 100;

        /// <summary>
        /// Metodo que Actualiza ell jerarquia para la asignacion del usuario hacia ls subempresa
        /// </summary>
        public ListErrorObdResponse Update(CredencialSpiderFleet.Models.Obd.SubCompanyAssignmentObds usersList)
        {
            ListErrorObdResponse response = new ListErrorObdResponse();
            int respuesta = 0;
            Dictionary<string, string> listError = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(usersList.IdSubCompany))
                {
                    response.success = false;
                    response.messages.Add("Verifique el campo de la sub empresa se encuentra vacio");
                    return response;
                }

                if (usersList.AssignmentObds.Count == 0)
                {
                    response.success = false;
                    response.messages.Add("No ingreso ningun Dispositivo");
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
                    int position = 1;
                    foreach (string device in usersList.AssignmentObds)
                    {

                        if (string.IsNullOrEmpty(device))
                        {
                            listError.Add("El dato de la posion " + position + " se encuntra vacio", "Verificar los datos ingresados");
                            position++;
                            continue;
                        }

                        cn = sql.Connection();
                        SqlCommand cmd = new SqlCommand("ad.sp_update_subcompany_assignment_obd", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@idsubcompany", Convert.ToString(usersList.IdSubCompany)));
                        cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));

                        SqlParameter sqlParameter = new SqlParameter();
                        sqlParameter.ParameterName = "@cMensaje";
                        sqlParameter.SqlDbType = SqlDbType.Int;
                        sqlParameter.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(sqlParameter);
                        cmd.ExecuteNonQuery();

                        respuesta = Convert.ToInt32(sqlParameter.Value.ToString());
                        if (respuesta != 1)
                        {
                            if (respuesta == 2)
                            {
                                listError.Add(device + "", "No existe el dispositivo seleccionado");
                            }
                            else if (respuesta == 3)
                            {
                                listError.Add(device + "", "Error al intentar asignar el dispositivo");
                            }
                            else if (respuesta == 4)
                            {
                                listError.Add(usersList.IdSubCompany + "", "No existe la SubCompañia verifique");
                                break;
                            }
                        }

                        position++;
                    }

                    if (listError.Count == 0)
                    {
                        response.listError = listError;
                        response.success = true;
                    }
                    else if (listError.Count > 0)
                    {
                        response.success = false;
                        response.messages.Add("Verifique la lista de Errores");
                        response.listError = listError;
                        return response;
                    }
                }
                else
                {
                    response.success = false;
                    response.messages.Add("Problemas con la conexión vuelva a intentarlo");
                    response.listError = listError;
                    return response;
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


        public ObdResponse Update(CredencialSpiderFleet.Models.Obd.ObdAssignmentUpdate obd)
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
                    string principal = use.nodePrincipal(obd.Hierarchy);

                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_obd_assignment", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(obd.IdDevice)));
                    
                    if (!string.IsNullOrEmpty(obd.Name)) { cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(obd.Name))); }
                    else { cmd.Parameters.Add(new SqlParameter("@name", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(obd.Hierarchy)) { cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(obd.Hierarchy))); }
                    else { cmd.Parameters.Add(new SqlParameter("@hierarchy", DBNull.Value)); }


                    cmd.Parameters.Add(new SqlParameter("@responsable", Convert.ToString(obd.Responsable))); 
                    cmd.Parameters.Add(new SqlParameter("@principal", Convert.ToString(principal))); 

                    string empresa = string.Empty;
                    if(obd.Hierarchy.Length == 1)
                    {
                        empresa = "0";
                    }
                    else
                    {
                        empresa = obd.Hierarchy.Replace("/", "-");
                        empresa = empresa.Substring(1, empresa.Length - 2);
                    }                    

                    if (!string.IsNullOrEmpty(empresa)) { cmd.Parameters.Add(new SqlParameter("@empresa", Convert.ToString(empresa))); }
                    else { cmd.Parameters.Add(new SqlParameter("@empresa", DBNull.Value)); }

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


        public ListObdResponse ListDeviceHierarchy(string hierarchy)
        {
            ListObdResponse response = new ListObdResponse();
            List<ObdCompany> listObd = new List<ObdCompany>();
            ObdCompany obd = new ObdCompany();

            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_device_by_hierarchy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(hierarchy)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obd = new ObdCompany();
                            obd.Hierarchy = Convert.ToString(reader["hierarchy"]);
                            obd.Device = Convert.ToString(reader["ID_Vehiculo"]);
                            obd.Name = Convert.ToString(reader["name"]);
                            obd.Company = Convert.ToString(reader["company"]);
                            obd.IdResponsable = Convert.ToString(reader["Id"]);
                            obd.Responsable = Convert.ToString(reader["responsable"]);

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


        /// <summary>
        /// Metodo que valida las longitudes de los campos y Validacion de caratcteres extraños
        /// </summary>
        /// <param name="obdRequest"></param>
        /// <returns></returns>
        private ObdResponse IsValidUpdate(CredencialSpiderFleet.Models.Obd.ObdAssignmentUpdate obdRequest)
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