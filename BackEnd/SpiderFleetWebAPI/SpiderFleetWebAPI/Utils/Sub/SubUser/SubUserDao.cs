using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.Sub.SubUser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Sub.SubUser
{
    public class SubUserDao
    {
        public SubUserDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private UseFul use = new UseFul();
        private const int longitudName = 50;
        private const int longitudLastName = 50;
        private const int longitudEmail = 50;
        private const int longitudUserName = 50;
        private const int longitudPassword = 50;
        private const int longitudTelephone = 15;

        /// <summary>
        /// Creacion de Usurio
        /// </summary>
        public SubUserResponse Create(CredencialSpiderFleet.Models.Sub.SubUser.SubUser user)
        {
            SubUserResponse response = new SubUserResponse();
            int respuesta = 0;
            try
            {
                //if (user.IdCompany == 0)
                //{
                //    response.success = false;
                //    response.messages.Add("Seleccione la Compañia a la que pertenece");
                //    return response;
                //}

                //if (user.IdRole == 0)
                //{
                //    response.success = false;
                //    response.messages.Add("Seleccione un Rol");
                //    return response;
                //}

                if (string.IsNullOrEmpty(user.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Nombre del Usuario");
                    return response;
                }

                if (string.IsNullOrEmpty(user.Email.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Email");
                    return response;
                }

                if (string.IsNullOrEmpty(user.UserName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el User Name del Usuario");
                    return response;
                }

                if (string.IsNullOrEmpty(user.Password.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el password");
                    return response;
                }

                if (string.IsNullOrEmpty(user.Hierarchy.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Grupo");
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
            try
            {
                response = IsValid(user);
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
                //Si quita validacion
                //List<string> email = new List<string>();
                //email = (new SubUserDao()).ReadEmail(user.Email);

                //if (email.Count != 0)
                //{
                //    if (user.Email.Equals(email[0]))
                //    {
                //        response.success = false;
                //        response.messages.Add("Ya existe el email, ingrese otro por favor");
                //        return response;
                //    }
                //}

                List<string> username = new List<string>();
                username = (new SubUserDao()).ReadUserName(user.UserName);
                if (username.Count != 0)
                {
                    if (user.UserName.Equals(username[0].Trim()))
                    {
                        response.success = false;
                        response.messages.Add("Ya existe ese usuario, ingrese otro por favor");
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

            user.Password = UseFul.MD5Cifrar(user.Password);

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_hierarchy_sub_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@usernamelevel", Convert.ToString(user.UserNameLevel)));
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(user.UserName)));

                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(user.Name)));

                    if (!string.IsNullOrEmpty(user.LastName)) { cmd.Parameters.Add(new SqlParameter("@lastname", Convert.ToString(user.LastName))); }
                    else { cmd.Parameters.Add(new SqlParameter("@lastname", DBNull.Value)); }

                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(user.Email)));


                    if (!string.IsNullOrEmpty(user.Password)) { cmd.Parameters.Add(new SqlParameter("@password", Convert.ToString(user.Password))); }
                    else { cmd.Parameters.Add(new SqlParameter("@password", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(user.Telephone)) { cmd.Parameters.Add(new SqlParameter("@telephone", Convert.ToString(user.Telephone))); }
                    else { cmd.Parameters.Add(new SqlParameter("@telephone", DBNull.Value)); }

                    cmd.Parameters.Add(new SqlParameter("@idstatus", Convert.ToInt32(user.IdStatus)));

                    //cmd.Parameters.Add(new SqlParameter("@idcompany", Convert.ToInt32(user.IdCompany)));
                    cmd.Parameters.Add(new SqlParameter("@idrole", Convert.ToInt32(user.IdRole)));
                    //cmd.Parameters.Add(new SqlParameter("@porcentage", Convert.ToDecimal(user.Porcentage)));
                    cmd.Parameters.Add(new SqlParameter("@node", user.Hierarchy));

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
        public SubUserResponse Update(CredencialSpiderFleet.Models.Sub.SubUser.SubUser user)
        {
            SubUserResponse response = new SubUserResponse();
            int respuesta = 0;

            try
            {
                //if (string.IsNullOrEmpty(user.IdUser.Trim()))
                //{
                //    response.success = false;
                //    response.messages.Add("No tiene el parametro IdUser");
                //    return response;
                //}

                if (string.IsNullOrEmpty(user.Hierarchy))
                {
                    response.success = false;
                    response.messages.Add("Seleccione el Grupo al que debe pertenecer");
                    return response;
                }

                if (user.IdRole == 0)
                {
                    response.success = false;
                    response.messages.Add("Seleccione un Rol");
                    return response;
                }

                if (string.IsNullOrEmpty(user.UserName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el User Name del Usuario");
                    return response;
                }

                if (string.IsNullOrEmpty(user.Email.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese Emial del Usuario");
                    return response;
                }

                //if (string.IsNullOrEmpty(userRequest.LastName.Trim()))
                //{
                //    response.success = false;
                //    response.messages.Add("Ingrese el El apelldio");
                //    return response;
                //}

                //if (string.IsNullOrEmpty(userRequest.LastName.Trim()))
                //{
                //    response.success = false;
                //    response.messages.Add("Ingrese el User Name del Usuario");
                //    return response;
                //}
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            if (!string.IsNullOrEmpty(user.Email.Trim()))
            {
                use = new UseFul();

                if (!use.IsValidLength(user.Email.Trim(), longitudEmail))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Emial excede de lo establecido rango maximo " + longitudEmail + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(user.UserName.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(user.UserName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(user.UserName.Trim(), longitudUserName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo UserName excede de lo establecido rango maximo " + longitudUserName + " caracteres");
                    return response;
                }
            }

            //try
            //{
            //    //List<string> email = new List<string>();
            //    //email = (new SubUserDao()).ReadEmail(user.Email);

            //    //if (email.Count != 0)
            //    //{
            //    //    if (user.Email.Equals(email[0].ToString().Trim()))
            //    //    {
            //    //        if (!user.IdUser.Equals((email[1].ToString().Trim())))
            //    //        {
            //    //            response.success = false;
            //    //            response.messages.Add("Ya existe el Email, ingrese otro por favor");
            //    //            return response;
            //    //        }
            //    //    }
            //    //}

            //    //List<string> username = new List<string>();
            //    //username = (new SubUserDao()).ReadUserName(user.UserName);
            //    //if (username.Count != 0)
            //    //{

            //    //    if (user.UserName.Equals(username[0].ToString().Trim()))
            //    //    {
            //    //        if (!user.IdUser.Equals(username[1].ToString().Trim()))
            //    //        {
            //    //            response.success = false;
            //    //            response.messages.Add("Ya existe el Username, ingrese otro por favor");
            //    //            return response;
            //    //        }
            //    //    }
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    response.success = false;
            //    response.messages.Add(ex.Message);
            //    return response;
            //}

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_sub_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(user.IdUser)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(user.Name)));

                    if (!string.IsNullOrEmpty(user.LastName)) { cmd.Parameters.Add(new SqlParameter("@lastname", Convert.ToString(user.LastName))); }
                    else { cmd.Parameters.Add(new SqlParameter("@lastname", DBNull.Value)); }

                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(user.Email)));
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(user.UserName)));

                    if (!string.IsNullOrEmpty(user.Telephone)) { cmd.Parameters.Add(new SqlParameter("@telephone", Convert.ToString(user.Telephone))); }
                    else { cmd.Parameters.Add(new SqlParameter("@telephone", DBNull.Value)); }

                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(user.Hierarchy)));
                    cmd.Parameters.Add(new SqlParameter("@idstatus", Convert.ToInt32(user.IdStatus)));
                    cmd.Parameters.Add(new SqlParameter("@idrole", Convert.ToInt32(user.IdRole)));

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
        public SubUserListResponse Read(string username, string search)
        {
            SubUserListResponse response = new SubUserListResponse();
            List<CredencialSpiderFleet.Models.Sub.SubUser.SubUserRegistry> listUsers = new List<CredencialSpiderFleet.Models.Sub.SubUser.SubUserRegistry>();
            CredencialSpiderFleet.Models.Sub.SubUser.SubUserRegistry user = new CredencialSpiderFleet.Models.Sub.SubUser.SubUserRegistry();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_sub_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    cmd.Parameters.Add(new SqlParameter("@search", string.IsNullOrEmpty(search) ? "" : search));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new CredencialSpiderFleet.Models.Sub.SubUser.SubUserRegistry();
                            user.UserName = (reader["ID_user"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["ID_user"]));
                            user.IdRole = (reader["idRole"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idRole"]));
                            user.DescripcionRol = (reader["descripcion_roles"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["descripcion_roles"]));
                            user.IdImage = (reader["idImg"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idImg"]));
                            user.Image = (reader["image"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["image"]));
                            user.IdStatus = (reader["idStatus"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idStatus"]));
                            user.DescripcionStatus = (reader["descripcion_status"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["descripcion_status"]));
                            user.Name = (reader["name"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["name"]));
                            user.LastName = (reader["lastName"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["lastName"]));
                            user.Email = (reader["email"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["email"]));
                            user.Telephone = (reader["telephone"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["telephone"]));
                            user.Hierarchy = (reader["jerarquia"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["jerarquia"]));
                            user.Porcentage = (reader["porcentage"] == DBNull.Value) ? 0 : Convert.ToDecimal(reader["porcentage"]);
                            user.Grupo = (reader["subempresa"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["subempresa"]));
                            user.Node = (reader["node"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["node"]));
                            listUsers.Add(user);

                        }
                        reader.Close();
                        response.listUsers = listUsers;
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
        /// Consulta de Ususario por id
        /// </summary>
        public SubUserRegistryResponse ReadId(string id)
        {
            SubUserRegistryResponse response = new SubUserRegistryResponse();
            CredencialSpiderFleet.Models.Sub.SubUser.SubUserRegistry user = new CredencialSpiderFleet.Models.Sub.SubUser.SubUserRegistry();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_id_sub_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new CredencialSpiderFleet.Models.Sub.SubUser.SubUserRegistry();
                            user.UserName = (reader["ID_user"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["ID_user"]));
                            user.IdRole = (reader["idRole"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idRole"]));
                            user.DescripcionRol = (reader["descripcion_roles"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["descripcion_roles"]));
                            user.IdImage = (reader["idImg"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idImg"]));
                            user.Image = (reader["image"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["image"]));
                            user.IdStatus = (reader["idStatus"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idStatus"]));
                            user.DescripcionStatus = (reader["descripcion_status"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["descripcion_status"]));
                            user.Name = (reader["name"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["name"]));
                            user.LastName = (reader["lastName"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["lastName"]));
                            user.Email = (reader["email"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["email"]));
                            user.Telephone = (reader["telephone"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["telephone"]));
                            user.Hierarchy = (reader["jerarquia"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["jerarquia"]));
                            user.Porcentage = (reader["porcentage"] == DBNull.Value) ? 0 : Convert.ToDecimal(reader["porcentage"]);
                            user.Grupo = (reader["subempresa"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["subempresa"]));
                            user.Node = (reader["node"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["node"]));
                        }
                        reader.Close();
                        response.user = user;
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
        /// Consulta que regresa lista de Descendencia del usuario
        /// </summary>
        public List<CredencialSpiderFleet.Models.Sub.SubUser.SubUser> ReadDescendant(string username)
        {
            List<CredencialSpiderFleet.Models.Sub.SubUser.SubUser> listUsers = new List<CredencialSpiderFleet.Models.Sub.SubUser.SubUser>();
            CredencialSpiderFleet.Models.Sub.SubUser.SubUser user = new CredencialSpiderFleet.Models.Sub.SubUser.SubUser();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_sub_user_descendant", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new CredencialSpiderFleet.Models.Sub.SubUser.SubUser();
                            user.IdRole = Convert.ToInt32(reader["idRole"]);
                            user.IdStatus = Convert.ToInt32(reader["idStatus"]);
                            user.IdImage = Convert.ToInt32(reader["idImg"]);
                            user.Image = Convert.ToString(reader["image"]);
                            user.Name = Convert.ToString(reader["name"]);
                            user.LastName = (reader["lastName"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["lastName"]));
                            user.Email = Convert.ToString(reader["email"]);
                            user.UserName = Convert.ToString(reader["username"]);
                            user.Telephone = (reader["telephone"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["telephone"]));
                            user.Hierarchy = Convert.ToString(reader["hierarchy"]);
                            listUsers.Add(user);
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
            return listUsers;
        }

        /// <summary>
        /// Eliminacion de Usuario por id, cambio de estatus a 0
        /// </summary>
        public int Delete(int id)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("sp_ad_delete_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());
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

        /// <summary>
        /// Consulta de Ususario por email
        /// </summary>
        public List<string> ReadEmail(string email)
        {
            List<string> listData = new List<string>();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_ad_consult_email_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(email)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listData.Add(Convert.ToString(reader["email"]));
                            listData.Add(Convert.ToString(reader["hierarchy"]));
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
            return listData;
        }

        /// <summary>
        /// Consulta de Ususario por email
        /// </summary>
        public List<string> ReadUserName(string username)
        {
            List<string> listData = new List<string>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_ad_consult_username_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listData.Add(Convert.ToString(reader["ID_user"]));
                            listData.Add(Convert.ToString(reader["hierarchy"]));
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
            return listData;
        }


        /// <summary>
        /// Metodo que valida las longitudes de los campos y Validacion de caratcteres extraños
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        private SubUserResponse IsValid(CredencialSpiderFleet.Models.Sub.SubUser.SubUser userRequest)
        {
            SubUserResponse response = new SubUserResponse();

            if (!string.IsNullOrEmpty(userRequest.Name.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(userRequest.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(userRequest.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Nombre excede de lo establecido rango maximo " + longitudName + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(userRequest.LastName.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(userRequest.LastName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(userRequest.LastName.Trim(), longitudLastName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Apellidos excede de lo establecido rango maximo " + longitudLastName + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(userRequest.Email.Trim()))
            {
                use = new UseFul();

                if (!use.IsValidLength(userRequest.Email.Trim(), longitudEmail))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Emial excede de lo establecido rango maximo " + longitudEmail + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(userRequest.UserName.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(userRequest.UserName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(userRequest.UserName.Trim(), longitudUserName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo UserName excede de lo establecido rango maximo " + longitudUserName + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(userRequest.Password.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(userRequest.Password.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(userRequest.Password.Trim(), longitudPassword))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Password excede de lo establecido rango maximo " + longitudPassword + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(userRequest.Telephone.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(userRequest.Telephone.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(userRequest.Telephone.Trim(), longitudTelephone))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Telefono excede de lo establecido rango maximo " + longitudTelephone + " caracteres");
                    return response;
                }

                response.success = true;

            }

            return response;
        }

    }
}