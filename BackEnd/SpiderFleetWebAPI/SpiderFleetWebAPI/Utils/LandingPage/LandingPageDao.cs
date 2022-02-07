using CredencialSpiderFleet.Models.Configuration;
using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.LandingPage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace SpiderFleetWebAPI.Utils.LandingPage
{
    public class LandingPageDao
    {
        private VariableConfiguration configuration = new VariableConfiguration();
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        

        public LandingPageResponse SendEmail(CredencialSpiderFleet.Models.LandingPage.LandingPage page)
        {
            LandingPageResponse response = new LandingPageResponse();
            try
            {

                if (string.IsNullOrEmpty(page.Email.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Email");
                    return response;
                }

                if (string.IsNullOrEmpty(page.Phone.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Telefono");
                    return response;
                }

                sendEmail(page.Email);
                response = Create(page);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }
        private LandingPageResponse Create(CredencialSpiderFleet.Models.LandingPage.LandingPage page)
        {
            LandingPageResponse response = new LandingPageResponse();

            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_landing_page", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(page.Name)));
                    cmd.Parameters.Add(new SqlParameter("@lastname", Convert.ToString(page.LastName)));
                    cmd.Parameters.Add(new SqlParameter("@phone", Convert.ToString(page.Phone)));
                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(page.Email)));
                    cmd.Parameters.Add(new SqlParameter("@subject", Convert.ToString(page.Subject)));
                    cmd.Parameters.Add(new SqlParameter("@comments", Convert.ToString(page.Comments)));
                    cmd.Parameters.Add(new SqlParameter("@company", Convert.ToString(page.Company)));

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


        private void sendEmail(string email)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.To.Add(email); //para quien va dirigido
                msg.Subject = "Asunto";
                msg.SubjectEncoding = Encoding.UTF8;
                msg.Bcc.Add(configuration.emailContacto);  //con copia

                StringBuilder mensaje = new StringBuilder("<html>");
                mensaje.Append("<body>");
                mensaje.Append("<h3>");
                mensaje.Append("SpiderFleet ");
                mensaje.Append("</h3> <br>");

                mensaje.Append("Ha recibido tu solicitud ");
                mensaje.Append("nos comunicaremos a la brevedad ");
                mensaje.Append("<br>");
                mensaje.Append("<br>");
                mensaje.Append("Ten un excelente dia.");
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