using CredencialSpiderFleet.Models.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace CredencialSpiderFleet.Models.Credentials
{
    public class Token
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();


        public async System.Threading.Tasks.Task<TokenCredentials> GetToken(string Username, string Password)
        {
            HttpClient httpClient = new HttpClient();

            //var responseMessage = await httpClient.PostAsync("http://spiderfleetapi.azurewebsites.net/token", new FormUrlEncodedContent(
            var responseMessage = await httpClient.PostAsync("https://spiderv3.azurewebsites.net/token", new FormUrlEncodedContent(
            //var responseMessage = await httpClient.PostAsync("https://localhost:44379/token", new FormUrlEncodedContent(
            new[]
                {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", Username),
                        new KeyValuePair<string, string>("password", Password),                        
                }
                ));

            var tokenModel = await responseMessage.Content.ReadAsAsync<TokenCredentials>();

            return tokenModel;
        }

        public async System.Threading.Tasks.Task<TokenCredentials> GetToken(string Username, string Password, string idu, string role, string spider)
        {
            HttpClient httpClient = new HttpClient();

            //var responseMessage = await httpClient.PostAsync("http://spiderfleetapi.azurewebsites.net/token", new FormUrlEncodedContent(
            var responseMessage = await httpClient.PostAsync("https://localhost:44379/token", new FormUrlEncodedContent(
            new[]
                {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", Username),
                        new KeyValuePair<string, string>("password", Password),
                        new KeyValuePair<string, string>("idu", idu),
                        new KeyValuePair<string, string>("role", role),
                        new KeyValuePair<string, string>("spider", spider),
                }
                ));

            var tokenModel = await responseMessage.Content.ReadAsAsync<TokenCredentials>();

            return tokenModel;
        }


        public User.UserLogin GetDataUser(string username)
        {
            User.UserLogin user = new User.UserLogin();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_username_login", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new User.UserLogin();
                            user.IdUser = Convert.ToString(reader["ID_user"]);
                            user.IdRole = (reader["idRole"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["idRole"])); 
                            user.DescripcionRole = (reader["descripcion_role"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["descripcion_role"]));
                            user.Image = (reader["image"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["image"]));
                            user.Name = (reader["name"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["name"])); 
                            user.LastName = (reader["lastName"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["lastName"]));
                            user.Email = (reader["email"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["email"])); 
                            user.Hierarchy = (reader["hierarchy"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["hierarchy"]));
                            user.IdU = (reader["ID_U"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["ID_U"]));
                            user.Spider = (reader["Spider"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["Spider"]));
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

        public List<string> GetPermission(string username)
        {
            List<string> list = new List<string>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_permission", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(Convert.ToString(reader["ID_Module"]));
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
            return list;
        }

        public int Validation(string username)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_validation_acceso", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            respuesta = Convert.ToInt32(reader["status"]);
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
            return respuesta;
        }
    }
}