using CredencialSpiderFleet.Models.Configuration;
using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.Email;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Email
{
    public class EmailDao
    {
        private VariableConfiguration configuration = new VariableConfiguration();
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public EmailDao() { }

        public EmialResponse SendEmial(string email)
        {
            EmialResponse response = new EmialResponse();
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    response.success = false;
                    response.messages.Add("El campo Email se encuentra vacio favor de verificar");
                    return response;
                }

                if(ValidaEmail(email) == 1) 
                {
                    string password = (UseFul.GenerarPassword(5));
                    string nombreCompleto = GetUserData(email);
                    sendEmail(email, nombreCompleto, password);
                    UpdatePassword(email, UseFul.MD5Cifrar(password));
                    response.success = true;
                }
                else
                {
                    response.success = false;
                    response.messages.Add("El Email no existe, favor de verificar");
                }
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
            return response;
        }

        private int ValidaEmail(string email)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_valida_email", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@email ", Convert.ToString(email)));

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

        private string GetUserData(string email)
        {
            string datos = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_user_data", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(email)));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            datos = Convert.ToString(reader["nombre_completo"]);
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
            return datos;
        }

        private int UpdatePassword(string email, string password)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_password", cn);
                    cmd.CommandType = CommandType.StoredProcedure; 
                    cmd.Parameters.Add(new SqlParameter("@email ", Convert.ToString(email)));
                    cmd.Parameters.Add(new SqlParameter("@password ", Convert.ToString(password)));

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


        private void sendEmail(string email, string nombre_completo, string password)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.To.Add(email); //para quien va dirigido
                msg.Subject = "Asunto";
                msg.SubjectEncoding = Encoding.UTF8;
                msg.Bcc.Add(configuration.email);  //con copia

                StringBuilder mensaje = new StringBuilder("<html>");
                mensaje.Append("<body>");
                mensaje.Append("<h3>");
                mensaje.Append("Hola ");
                mensaje.Append(nombre_completo + "  ");
                mensaje.Append("</h3> <br>");

                mensaje.Append("Por este medio le informamos ");
                mensaje.Append("que su contraseña ha sido restablecida ");
                mensaje.Append("<b>").Append(password).Append(" </b><br>");

                mensaje.Append("");
                mensaje.Append("</body>");
                mensaje.Append("</html>");

                msg.Body = mensaje.ToString();
                msg.BodyEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;
                msg.From = new MailAddress("contacto.spiderfleet@gmail.com");

                //Cliente Correo
                //spider.fleet.contacto
                //contacto@spider.com.mx", "Mastersword25$"
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential("contacto.spiderfleet@gmail.com", "Contacto12345");
                smtp.Port = 587;
                smtp.EnableSsl = true;

                smtp.Host = "smtp.gmail.com";

                try
                {
                    smtp.Send(msg);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
