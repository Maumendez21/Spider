using CredencialSpiderFleet.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Message
{
    public class Message
    {
        private VariableConfiguration configuration = new VariableConfiguration();

        public void sendEmail(string nombre_contacto, List<string> listSim, string fecha, string mensajeCredito)
        {
            try
            {
                //https://www.youtube.com/watch?v=WKyVqmbCVc0
                MailMessage msg = new MailMessage();
                msg.To.Add(configuration.email); //para quien va dirigido
                msg.Subject = "Asunto";
                msg.SubjectEncoding = Encoding.UTF8;
                //msg.Bcc.Add(configuration.email);  //con copia

                StringBuilder mensaje = new StringBuilder("<html>");
                mensaje.Append("<body>");
                mensaje.Append("<img src=\"https://drive.google.com/open?id=1DB6rhADbeCVFFpwFQjAXmDM7lyqj5G0m\" width=\"140\" height=\"210\" border=\"0\" alt=\"SpiderFleet\" />");

                mensaje.Append("<h3>");
                mensaje.Append("Hola ");
                mensaje.Append(nombre_contacto + "  ");
                mensaje.Append("</h3> <br>");

                mensaje.Append("Por este medio le informamos ");
                //mensaje.Append("que se ha puesto credito a los siguientes Sims ");
                mensaje.Append("que se ha ").Append(mensajeCredito).Append(" credito a los siguientes Sims ");
                mensaje.Append("<b>");

                mensaje.Append("<ul>");

                foreach(string sim in listSim)
                {
                    mensaje.Append("<li>").Append(sim).Append("</li><br>");
                }                
                mensaje.Append("</ul>");

                mensaje.Append(" </b><br>");

                mensaje.Append("Saludos. <br>");
                mensaje.Append("Tus amigos de Spider Fleet. <br>");
                mensaje.Append("");
                mensaje.Append("</body>");
                mensaje.Append("</html>");
                //logo //https://drive.google.com/open?id=1DB6rhADbeCVFFpwFQjAXmDM7lyqj5G0m

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