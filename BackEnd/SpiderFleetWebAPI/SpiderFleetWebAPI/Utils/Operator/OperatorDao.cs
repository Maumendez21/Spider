using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.Operator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Operator
{
    public class OperatorDao
    {
        public OperatorDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private UseFul use = new UseFul();

        /// <summary>
        /// Creacion de Usurio
        /// </summary>
        public OperatorResponse Create(CredencialSpiderFleet.Models.Operator.Operator operators)
        {
            OperatorResponse response = new OperatorResponse();

            try
            {

                if (string.IsNullOrEmpty(operators.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Nombre del Operador");
                    return response;
                }

                if (string.IsNullOrEmpty(operators.Email.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Email del Operador");
                    return response;
                }

                if (string.IsNullOrEmpty(operators.Address.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese la direccion del Operador");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            //Validaciones de longitud y Caracteres especiales
            //try
            //{
            //    response = IsValid(operators);
            //    if (!response.success) { return response; }
            //    response.success = false;
            //}
            //catch (Exception ex)
            //{
            //    response.success = false;
            //    response.messages.Add(ex.Message);
            //    return response;
            //}

            if (!string.IsNullOrEmpty(operators.Email.Trim()))
            {
                use = new UseFul();
                if (!use.IsValidEmail(operators.Email.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Email invalido");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(operators.Telephone.Trim()))
            {
                use = new UseFul();
                if (!use.IsValidTelephone(operators.Telephone.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Telefono invalido");
                    return response;
                }
            }

            //int respuesta = 0;
            string respuesta = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_operator", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(operators.Name)));
                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(operators.Email)));
                    cmd.Parameters.Add(new SqlParameter("@address", Convert.ToString(operators.Address)));

                    if (!string.IsNullOrEmpty(operators.Device)) { cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(operators.Device))); }
                    else { cmd.Parameters.Add(new SqlParameter("@device", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(operators.Position)) { cmd.Parameters.Add(new SqlParameter("@position", Convert.ToString(operators.Position))); }
                    else { cmd.Parameters.Add(new SqlParameter("@position", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(operators.Telephone)) { cmd.Parameters.Add(new SqlParameter("@telephone", Convert.ToString(operators.Telephone))); }
                    else { cmd.Parameters.Add(new SqlParameter("@telephone", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(operators.Location)) { cmd.Parameters.Add(new SqlParameter("@location", Convert.ToString(operators.Location))); }
                    else { cmd.Parameters.Add(new SqlParameter("@location", DBNull.Value)); }

                    cmd.Parameters.Add(new SqlParameter("@img", Convert.ToInt32(0)));
                    cmd.Parameters.Add(new SqlParameter("@status", Convert.ToInt32(1)));

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
                throw new Exception(ex.Message);
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
        public OperatorResponse Update(CredencialSpiderFleet.Models.Operator.Operator operators)
        {
            OperatorResponse response = new OperatorResponse();

            try
            {

                if (string.IsNullOrEmpty(operators.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Nombre del Operador");
                    return response;
                }

                if (string.IsNullOrEmpty(operators.Email.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Email del Operador");
                    return response;
                }

                if (string.IsNullOrEmpty(operators.Address.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese la direccion del Operador");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            //if (!string.IsNullOrEmpty(user.Email.Trim()))
            //{
            //    use = new UseFul();

            //    if (!use.IsValidLength(user.Email.Trim(), longitudEmail))
            //    {
            //        response.success = false;
            //        response.messages.Add("La longitud del campo Emial excede de lo establecido rango maximo " + longitudEmail + " caracteres");
            //        return response;
            //    }
            //}

            //if (!string.IsNullOrEmpty(user.UserName.Trim()))
            //{
            //    use = new UseFul();

            //    if (use.hasSpecialChar(user.UserName.Trim()))
            //    {
            //        response.success = false;
            //        response.messages.Add("La cadena contiene caracteres especiales");
            //        return response;
            //    }

            //    if (!use.IsValidLength(user.UserName.Trim(), longitudUserName))
            //    {
            //        response.success = false;
            //        response.messages.Add("La longitud del campo UserName excede de lo establecido rango maximo " + longitudUserName + " caracteres");
            //        return response;
            //    }
            //}

            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_operator", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(operators.Id)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(operators.Name)));
                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(operators.Email)));
                    cmd.Parameters.Add(new SqlParameter("@address", Convert.ToString(operators.Address)));

                    if (!string.IsNullOrEmpty(operators.Device)) { cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(operators.Device))); }
                    else { cmd.Parameters.Add(new SqlParameter("@device", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(operators.Position)) { cmd.Parameters.Add(new SqlParameter("@position", Convert.ToString(operators.Position))); }
                    else { cmd.Parameters.Add(new SqlParameter("@position", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(operators.Telephone)) { cmd.Parameters.Add(new SqlParameter("@telephone", Convert.ToString(operators.Telephone))); }
                    else { cmd.Parameters.Add(new SqlParameter("@telephone", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(operators.Location)) { cmd.Parameters.Add(new SqlParameter("@location", Convert.ToString(operators.Location))); }
                    else { cmd.Parameters.Add(new SqlParameter("@location", DBNull.Value)); }

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
                    else if (respuesta == 4)
                    {
                        response.success = false;
                        response.messages.Add("Ya existe el dispositivo asignado a otro operador");
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


        public OperatorResponse UpdateDeviceOperador(CredencialSpiderFleet.Models.Operator.Operator operators)
        {
            OperatorResponse response = new OperatorResponse();

            try
            {

                if (string.IsNullOrEmpty(operators.Device.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Dispositivo");
                    return response;
                }

                if (string.IsNullOrEmpty(operators.Id.ToString()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Operador");
                    return response;
                }
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
                    SqlCommand cmd = new SqlCommand("ad.sp_update_device_operator", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(operators.Id)));
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(operators.Device)));

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
                    //else if (respuesta == 4)
                    //{
                    //    response.success = false;
                    //    response.messages.Add("Ya existe el dispositivo asignado a otro operador");
                    //    return response;
                    //}
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
        public OperatorListResponse Read(string username)
        {
            OperatorListResponse response = new OperatorListResponse();
            List<CredencialSpiderFleet.Models.Operator.OperatorRegistry> listOperator = new List<CredencialSpiderFleet.Models.Operator.OperatorRegistry>();
            CredencialSpiderFleet.Models.Operator.OperatorRegistry operators = new CredencialSpiderFleet.Models.Operator.OperatorRegistry();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_operator", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            operators = new CredencialSpiderFleet.Models.Operator.OperatorRegistry();
                            operators.Id = Convert.ToInt32(reader["ID_Operador"]);  
                            operators.Device = (reader["device"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["device"]));
                            operators.IdImg = (reader["idImg"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idImg"])); 
                            operators.Image = (reader["image"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["image"]));
                            operators.Name = (reader["Nombre"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Nombre"])); 
                            operators.Address = (reader["Direccion"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Direccion"]));
                            operators.Email = (reader["Email"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Email"])); 
                            operators.Telephone = (reader["Telefono"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Telefono"]));
                            operators.Location = (reader["Localidad"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Localidad"]));
                            operators.Position = (reader["Puesto"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Puesto"]));
                            listOperator.Add(operators);
                        }

                        reader.Close();
                        response.listOperator = listOperator;
                        response.success = true;
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
        /// Consulta de Operador por id
        /// </summary>
        public OperatorRegistryResponse ReadId(int id)
        {
            OperatorRegistryResponse response = new OperatorRegistryResponse();
            CredencialSpiderFleet.Models.Operator.OperatorRegistry operators = new CredencialSpiderFleet.Models.Operator.OperatorRegistry();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_operator_id", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            operators = new CredencialSpiderFleet.Models.Operator.OperatorRegistry();
                            operators.Id = Convert.ToInt32(reader["ID_Operador"]);
                            operators.Device = (reader["device"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["device"]));
                            operators.IdImg = (reader["idImg"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idImg"]));
                            operators.Image = (reader["image"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["image"]));
                            operators.Name = (reader["Nombre"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Nombre"]));
                            operators.Address = (reader["Direccion"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Direccion"]));
                            operators.Email = (reader["Email"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Email"]));
                            operators.Telephone = (reader["Telefono"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Telefono"]));
                            operators.Location = (reader["Localidad"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Localidad"]));
                            operators.Position = (reader["Puesto"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Puesto"]));
                        }
                        response.operators = operators;
                        response.success = true;
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
        /// Consulta de Operador por Device
        /// </summary>
        public OperatorRegistryResponse ReadDevice(string device)
        {
            OperatorRegistryResponse response = new OperatorRegistryResponse();
            CredencialSpiderFleet.Models.Operator.OperatorRegistry operators = new CredencialSpiderFleet.Models.Operator.OperatorRegistry();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_operator_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            operators = new CredencialSpiderFleet.Models.Operator.OperatorRegistry();
                            operators.Id = Convert.ToInt32(reader["ID_Operador"]);
                            operators.Device = (reader["device"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["device"]));
                            operators.IdImg = (reader["idImg"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idImg"]));
                            operators.Image = (reader["image"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["image"]));
                            operators.Name = (reader["Nombre"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Nombre"]));
                            operators.Address = (reader["Direccion"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Direccion"]));
                            operators.Email = (reader["Email"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Email"]));
                            operators.Telephone = (reader["Telefono"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Telefono"]));
                            operators.Location = (reader["Localidad"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Localidad"]));
                            operators.Position = (reader["Puesto"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Puesto"]));
                        }
                        response.operators = operators;
                        response.success = true;
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
        /// Eliminacion de Usuario por id, cambio de estatus a 0
        /// </summary>
        public OperatorResponse Delete(int id)
        {
            OperatorResponse response = new OperatorResponse();
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_delete_operator", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));

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
                        response.messages.Add("Error al tratar de eliminar el registro");
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