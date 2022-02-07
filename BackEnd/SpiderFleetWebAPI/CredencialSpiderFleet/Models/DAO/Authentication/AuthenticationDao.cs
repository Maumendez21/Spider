using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Request.Authentication;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CredencialSpiderFleet.Models.DAO.Authentication
{
    public class AuthenticationDao
    {
        public AuthenticationDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public (int, DataSet) ReadLogin(AuthenticationRequest authentication)
        {
            int respuesta = 0;
            DataSet dsConsulta = new DataSet();

            try
            {
                if (sql.IsConnection)
                {

                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_authentication", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@login", Convert.ToString(authentication.Login)));
                    cmd.Parameters.Add(new SqlParameter("@password", Convert.ToString(authentication.Password)));
                    cmd.Parameters.Add(new SqlParameter("@type", Convert.ToInt32(authentication.type)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter sqlData = new SqlDataAdapter(cmd);
                    sqlData.Fill(dsConsulta);

                    if (dsConsulta.Tables.Count == 0)
                    {
                        respuesta = Convert.ToInt32(sqlParameter.Value.ToString());
                    }
                    else
                    {
                        respuesta = 1;
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
            return (respuesta, dsConsulta);
        }
    }
}