using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.Password;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Password
{
    public class PasswordDao
    {
        public PasswordDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private UseFul use = new UseFul();

        public PasswordResponse ChangePassword(CredencialSpiderFleet.Models.Password.Passwords password)
        {
            PasswordResponse response = new PasswordResponse();
            int respuesta = 0;

            try
            {
                if (string.IsNullOrEmpty(password.Login.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Login");
                    return response;
                }
                 
                if (string.IsNullOrEmpty(password.Password.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese la nueva Contraseña");
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            string passwordAnterior = Read(password.Login);
            password.OldPassword = UseFul.MD5Cifrar(password.OldPassword);


            if (!passwordAnterior.Equals(password.OldPassword))
            {
                response.success = false;
                response.messages.Add("La contraseña anterior no coincide");
                return response;
            }

            password.Password = UseFul.MD5Cifrar(password.Password);

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_change_password", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@login", Convert.ToString(password.Login)));
                    cmd.Parameters.Add(new SqlParameter("@password", Convert.ToString(password.Password)));
                    //cmd.Parameters.Add(new SqlParameter("@type", Convert.ToInt32(password.type)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("El Email ingresado no existe, favor de verificar");
                        return response;
                    }
                    else if (respuesta == 4)
                    {
                        response.success = false;
                        response.messages.Add("El Usuario ingresado no existe, favor de verificar");
                        return response;
                    }
                    else if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Ocurrio un problema notificar a su Administrador");
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


        private string Read(string user)
        {
            string response = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_passwor_by_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@user", Convert.ToString(user)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response = Convert.ToString(reader["Password"]);
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                cn.Close();
            }
            return response;
        }

    }
}