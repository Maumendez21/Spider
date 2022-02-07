using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Logo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace SpiderFleetWebAPI.Utils.Logo
{
    public class LogoDao
    {
        public LogoDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private string path = HostingEnvironment.MapPath("/templates/logo/");
        //private string path = HostingEnvironment.MapPath(@"D:\Plantillas\Logo");

        public LogoResponse Create(HttpPostedFile inputStream, string empresa)
        {
            LogoResponse response = new LogoResponse();

            if (string.IsNullOrEmpty(empresa.Trim()))
            {
                response.success = false;
                response.messages.Add("Ingrese el identificador de la Empresa");
                return response;
            }

            var fileName = empresa + ".png";
            //string path = @"D:\Plantillas\Logo\";
            try
            {
                
                var server = Path.Combine(path, fileName);
                inputStream.SaveAs(server);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add("A ocurrido un error al tratar de subir la Imagen " + ex.Message );
                return response;
            }

            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_image_server", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@key", Convert.ToString(empresa)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(fileName)));
                    cmd.Parameters.Add(new SqlParameter("@path", Convert.ToString(path)));

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

        public LogoResponse Update(HttpPostedFile inputStream, string empresa)
        {
            LogoResponse response = new LogoResponse();

            var fileName = empresa + ".png";
            //string path = @"D:\Plantillas\Logo\";
            try
            {
                var server = Path.Combine(path, fileName);
                inputStream.SaveAs(server);
                response.success = true;

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add("A ocurrido un error al tratar de subir la Imagen " + ex.Message);
                return response;
            }
            finally
            {
                cn.Close();
            }
            return response;
        }

        public CredencialSpiderFleet.Models.Logo.Logo ReadId(string empresa)
        {
            CredencialSpiderFleet.Models.Logo.Logo imagen = new CredencialSpiderFleet.Models.Logo.Logo();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_id_image_server", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@key", Convert.ToString(empresa)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            imagen = new CredencialSpiderFleet.Models.Logo.Logo();
                            imagen.Name = Convert.ToString(reader["Name"]);
                            imagen.Path = Convert.ToString(reader["Path"]);
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
            return imagen;
        }

    }
}
