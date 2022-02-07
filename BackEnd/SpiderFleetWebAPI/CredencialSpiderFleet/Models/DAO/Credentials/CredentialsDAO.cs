using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Credentials;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.DAO.Credentials
{
    public class CredentialsDAO
    {
        public CredentialsDAO() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public string ReadLogin(AuthenticationRequest authentication)
        {
            string role = string.Empty;
            
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_authentication", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@login", Convert.ToString(authentication.Login)));
                    cmd.Parameters.Add(new SqlParameter("@password", Convert.ToString(authentication.Password)));

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        role = reader.GetValue(0).ToString() == null ? "" : reader.GetValue(0).ToString();
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
            return role;
        }
    }
}