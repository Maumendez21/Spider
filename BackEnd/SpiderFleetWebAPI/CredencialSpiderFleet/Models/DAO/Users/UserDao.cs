using CredencialSpiderFleet.Models.Connection;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CredencialSpiderFleet.Models.DAO.Users
{
    public class UserDao
    {
        public UserDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        /// <summary>
        /// Creacion de Usurio
        /// </summary>
        public int Create(Request.User.UserRequest user)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_ad_create_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@idcompany", Convert.ToInt32(user.IdCompany)));
                    cmd.Parameters.Add(new SqlParameter("@idrole", Convert.ToInt32(user.IdRole)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(user.Name)));
                    cmd.Parameters.Add(new SqlParameter("@lastname", Convert.ToString(user.LastName)));
                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(user.Email)));
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(user.UserName)));
                    cmd.Parameters.Add(new SqlParameter("@password", Convert.ToString(user.Password)));
                    cmd.Parameters.Add(new SqlParameter("@telephone", Convert.ToString(user.Telephone)));
                    cmd.Parameters.Add(new SqlParameter("@porcentage", Convert.ToDecimal(user.Porcentage)));
                    cmd.Parameters.Add(new SqlParameter("@hierarchy", user.Hierarchy));
                    cmd.Parameters.Add(new SqlParameter("@idstatus", Convert.ToInt32(user.IdStatus)));

                    //numRes = cmd.ExecuteNonQuery();

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
        /// Actualizacion de Usuario
        /// </summary>
        public int Update(Request.User.UserRequest user)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_ad_update_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(user.IdUser)));
                    //cmd.Parameters.Add(new SqlParameter("@idcompany", Convert.ToInt32(user.IdCompany)));
                    //cmd.Parameters.Add(new SqlParameter("@idrole", Convert.ToInt32(user.IdRole)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(user.Name)));
                    cmd.Parameters.Add(new SqlParameter("@lastname", Convert.ToString(user.LastName)));
                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(user.Email)));
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(user.UserName)));
                    //cmd.Parameters.Add(new SqlParameter("@password", Convert.ToString(user.Password)));
                    cmd.Parameters.Add(new SqlParameter("@telephone", Convert.ToString(user.Telephone)));
                    //cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(user.Hierarchy)));
                    //cmd.Parameters.Add(new SqlParameter("@status", Convert.ToInt32(user.Status)));

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
        /// Consulta de Usuarios con estatus 1
        /// </summary>
        public DataSet Read()
        {
            DataSet dsConsulta = new DataSet();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_ad_consult_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter sqlData = new SqlDataAdapter(cmd);
                    sqlData.Fill(dsConsulta);
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
            return dsConsulta;
        }

        /// <summary>
        /// Consulta de Ususario por id
        /// </summary>
        public DataSet ReadId(int id)
        {

            DataSet dsConsulta = new DataSet();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_ad_consult_id_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter sqlData = new SqlDataAdapter(cmd);
                    sqlData.Fill(dsConsulta);

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
            return dsConsulta;
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
        public DataSet ReadEmail(string email)
        {
            DataSet dsConsulta = new DataSet();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_ad_consult_email_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(email)));
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter sqlData = new SqlDataAdapter(cmd);
                    //DataTable table = new DataTable("DATA");
                    sqlData.Fill(dsConsulta);
                    //dsConsulta.Tables.Add(table);

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
            return dsConsulta;
        }

        /// <summary>
        /// Consulta de Ususario por email
        /// </summary>
        public DataSet ReadUserName(string username)
        {
            DataSet dsConsulta = new DataSet();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_ad_consult_username_user", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter sqlData = new SqlDataAdapter(cmd);
                    sqlData.Fill(dsConsulta);

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
            return dsConsulta;
        }

    }
}