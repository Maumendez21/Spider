using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.User;
using SpiderFleetWebAPI.Utils.Company;
using SpiderFleetWebAPI.Utils.Setting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.User
{
    public class UserDao
    {
        public UserDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private UseFul use = new UseFul();
        private const int longitudName = 50;
        private const int longitudLastName = 50;
        private const int longitudEmail = 50;
        private const int longitudUserName = 50;
        private const int longitudPassword = 20;
        private const int longitudTelephone = 15;
        private const int longitudCompanyName = 100;
        private const int longitudTaxId = 15;
        private const int longitudTaxName = 50;




       /// <summary>
       /// Metodo que da de alta al usuario y la Emresa
       /// </summary>
       /// <param name="user"></param>
       /// <returns></returns>
        public UserResponse Create(CredencialSpiderFleet.Models.User.User user)
        {
            UserResponse response = new UserResponse();

            try
            {

                if (string.IsNullOrEmpty(user.CompanyName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Nombre de la Empresa");
                    return response;
                }

                if (string.IsNullOrEmpty(user.TaxId.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el RFC de la Empresa");
                    return response;
                }

                if (string.IsNullOrEmpty(user.TaxName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese la Razon Social de la Empresa");
                    return response;
                }


                if (user.IdRole == 0)
                {
                    response.success = false;
                    response.messages.Add("Seleccione un Rol");
                    return response;
                }

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

            if (!string.IsNullOrEmpty(user.Email.Trim()))
            {
                use = new UseFul();
                if (!use.IsValidEmail(user.Email.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Email invalido");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(user.Telephone.Trim()))
            {
                use = new UseFul();
                if (!use.IsValidTelephone(user.Telephone.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Telefono invalido");
                    return response;
                }
            }

            //Validacion de usuario
            try
            {
                if (user.UserName.Equals(ReadUserNameExist(user.UserName).Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ya existe un ese usuario, ingrese otro por favor");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            //Crear empresa  regresar Id para el User
            string idCompany = string.Empty;
            
            CredencialSpiderFleet.Models.Company.Company company = new CredencialSpiderFleet.Models.Company.Company();
            company.IdImagen = 0;
            company.IdSuscriptionType = 0;
            company.Name = user.CompanyName;
            company.TaxId = user.TaxId;
            company.TaxName = user.TaxName;
            company.Address = "";
            company.Telephone = "";
            company.Email = "";
            company.City = "";
            company.Country = "";
            company.Porcentage = 0;
            company.Hierarchy = "/1/1/1/";

            idCompany = (new CompanyDao()).Create(company);

            if (idCompany.Contains("ERROR"))
            {
                response.success = false;
                response.messages.Add("Error al intenar dar de alta la Compañia");
                return response;
            }

            try
            {
                (new SettingConfig()).Create("ITE", 1, idCompany, "100", "Tiempo de diferencia");
                (new SettingConfig()).Create("MXV", 1, idCompany, "40", "Maxima Velocidad");
                (new SettingConfig()).Create("RBT", 2, idCompany, "5", "Best Ranking");
                (new SettingConfig()).Create("RLW", 2, idCompany, "5", "Lower Ranking");
                (new SettingConfig()).Create("MC", 2, idCompany, "4", "Mapa de Calor");
                (new SettingConfig()).Create("WTC", 1, idCompany, "300", "Tiempo de espera Auto");
            }
            catch (Exception ex)
            {

            }

            user.Hierarchy = idCompany;
            user.Password = UseFul.MD5Cifrar(user.Password);

            string respuesta = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@idrole", Convert.ToInt32(user.IdRole)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(user.Name)));
                    
                    if (!string.IsNullOrEmpty(user.LastName)) { cmd.Parameters.Add(new SqlParameter("@lastname", Convert.ToString(user.LastName))); }
                    else { cmd.Parameters.Add(new SqlParameter("@lastname", DBNull.Value)); }

                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(user.Email)));
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(user.UserName)));

                    if (!string.IsNullOrEmpty(user.Password)) { cmd.Parameters.Add(new SqlParameter("@password", Convert.ToString(user.Password))); }
                    else { cmd.Parameters.Add(new SqlParameter("@password", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(user.Telephone)) { cmd.Parameters.Add(new SqlParameter("@telephone", Convert.ToString(user.Telephone))); }
                    else { cmd.Parameters.Add(new SqlParameter("@telephone", DBNull.Value)); }

                    cmd.Parameters.Add(new SqlParameter("@porcentage", Convert.ToDecimal(user.Porcentage)));
                    cmd.Parameters.Add(new SqlParameter("@hierarchy", user.Hierarchy));
                    
                    cmd.Parameters.Add(new SqlParameter("@idstatus", Convert.ToInt32(user.IdStatus)));
                    cmd.Parameters.Add(new SqlParameter("@spider", Convert.ToInt32(user.Type)));

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
       /// Metodo que actualiza los datos del Usuario
       /// </summary>
       /// <param name="user"></param>
       /// <returns></returns>
        public UserResponse Update(CredencialSpiderFleet.Models.User.User user)
        {
            UserResponse response = new UserResponse();

            try
            {
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

            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(user.Telephone)) { cmd.Parameters.Add(new SqlParameter("@telephone", Convert.ToString(user.Telephone))); }
                    else { cmd.Parameters.Add(new SqlParameter("@telephone", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(user.LastName)) { cmd.Parameters.Add(new SqlParameter("@lastname", Convert.ToString(user.LastName))); }
                    else { cmd.Parameters.Add(new SqlParameter("@lastname", DBNull.Value)); }

                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(user.Name)));
                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(user.Email)));
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(user.UserName)));
                   

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
       /// Metodo que devuelve los registros dados de alta por el username padre
       /// </summary>
       /// <param name="username"></param>
       /// <returns></returns>
        public UserListResponse Read(string username)
        {
            UserListResponse response = new UserListResponse();
            List<CredencialSpiderFleet.Models.User.UserRegistry> listUsers = new List<CredencialSpiderFleet.Models.User.UserRegistry>();
            CredencialSpiderFleet.Models.User.UserRegistry user = new CredencialSpiderFleet.Models.User.UserRegistry();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new CredencialSpiderFleet.Models.User.UserRegistry();
                            user.IdUser = Convert.ToString(reader["ID_user"]);
                            user.IdRole = (reader["idRole"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idRole"])); 
                            user.DescripcionRole = (reader["descripcion_role"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["descripcion_role"]));
                            user.IdImage = (reader["idImg"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idImg"])); 
                            user.Image = (reader["image"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["image"]));
                            user.Name = (reader["name"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["name"])); 
                            user.LastName = (reader["lastName"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["lastName"]));
                            user.Email = (reader["email"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["email"])); 
                            user.Telephone = (reader["telephone"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["telephone"]));
                            user.Hierarchy = (reader["hierarchy"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["hierarchy"])); 
                            user.IdStatus = (reader["idStatus"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idStatus"]));  
                            user.DescriptionStatus = (reader["descripcion_status"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["descripcion_status"]));
                            user.Porcentage = (reader["porcentage"] == DBNull.Value) ? 0 : (Convert.ToDecimal(reader["porcentage"]));
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
        /// Metodo que consulta por id del usuario y username padre 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="idusermane"></param>
        /// <returns></returns>
        public UserRegistryResponse ReadId(string username, string idusermane)
        {
            UserRegistryResponse response = new UserRegistryResponse();
            CredencialSpiderFleet.Models.User.UserRegistry user = new CredencialSpiderFleet.Models.User.UserRegistry();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_id_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    cmd.Parameters.Add(new SqlParameter("@idusername", Convert.ToString(idusermane)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new CredencialSpiderFleet.Models.User.UserRegistry();
                            user.IdUser = Convert.ToString(reader["ID_user"]);
                            user.IdRole = (reader["idRole"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idRole"]));
                            user.DescripcionRole = (reader["descripcion_role"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["descripcion_role"]));
                            user.IdImage = (reader["idImg"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idImg"]));
                            user.Image = (reader["image"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["image"]));
                            user.Name = (reader["name"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["name"]));
                            user.LastName = (reader["lastName"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["lastName"]));
                            user.Email = (reader["email"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["email"]));
                            user.Telephone = (reader["telephone"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["telephone"]));
                            user.Hierarchy = (reader["hierarchy"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["hierarchy"]));
                            user.IdStatus = (reader["idStatus"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idStatus"]));
                            user.DescriptionStatus = (reader["descripcion_status"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["descripcion_status"]));
                            user.Porcentage = (reader["porcentage"] == DBNull.Value) ? 0 : (Convert.ToDecimal(reader["porcentage"]));
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
        /// Metodo que elimina de al usuario por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// Metodo que devuelve la Jerarquia por medio del usuario
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string ReadUserName(string username)
        {
            string user = string.Empty;
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
                            user = Convert.ToString(reader["hierarchy"]);
                        }
                        reader.Close();
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
            return user;
        }

        /// <summary>
        /// Metodo que regresa el usuario si existe
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string ReadUserNameExist(string username)
        {
            string user = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_username", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = Convert.ToString(reader["ID_user"]);
                        }
                        reader.Close();
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
            return user;
        }

        /// <summary>
        /// Actualizacion el idUser de Stripe
        /// </summary>
        public int UpdateStripeId(int idUser, string idStripe)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_user_id_stripe", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(idUser)));
                    cmd.Parameters.Add(new SqlParameter("@idstripe", Convert.ToString(idStripe)));

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
        /// Metodo que Consulta de Ususario por email
        /// </summary>
        public string ReadUserHierarchy(string username)
        {
            string hierarchy = string.Empty;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_ad_consult_user_hierarchy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            hierarchy = Convert.ToString(reader["hierarchy"]);
                        }
                        reader.Close();
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
            return hierarchy;
        }


        public bool Exists(string username)
        {
            bool exists = false;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_exists_username", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            exists = Convert.ToInt32(reader["valor"]) == 1 ? true : false;
                        }
                        reader.Close();
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
            return exists;
        }


        /// <summary>
        /// Metodo que valida las longitudes de los campos y Validacion de caratcteres extraños
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        private UserResponse IsValid(CredencialSpiderFleet.Models.User.User userRequest)
        {
            UserResponse response = new UserResponse();

            ////Validacion de datos de la Empresa
            //Campo Nombre
            if (!string.IsNullOrEmpty(userRequest.CompanyName.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(userRequest.CompanyName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(userRequest.CompanyName.Trim(), longitudCompanyName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Nombre de la Empresa excede de lo establecido rango maximo " + longitudCompanyName + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(userRequest.TaxId.Trim()))
            {
                use = new UseFul();

                if (!use.IsValidLength(userRequest.TaxId.Trim(), longitudTaxId))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo RFC de la Empresa excede de lo establecido rango maximo " + longitudTaxId + " caracteres");
                    return response;
                }
            }
        
            if (!string.IsNullOrEmpty(userRequest.TaxName.Trim()))
            {
                use = new UseFul();

                if (!use.IsValidLength(userRequest.TaxName.Trim(), longitudTaxName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Razon Social de la Empresa excede de lo establecido rango maximo " + longitudTaxName + " caracteres");
                    return response;
                }
            }

            ////Validacion de Usuario
            //Campo Nombre
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

            //Campo Apellido 
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

            //Campo Email
            if (!string.IsNullOrEmpty(userRequest.Email.Trim()))
            {
                use = new UseFul();

                if (!use.IsValidLength(userRequest.Email.Trim(), longitudEmail))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Email excede de lo establecido rango maximo " + longitudEmail + " caracteres");
                    return response;
                }
            }

            //Campo Usuario
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

            //Campo Password
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

            //Campo Telefono
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