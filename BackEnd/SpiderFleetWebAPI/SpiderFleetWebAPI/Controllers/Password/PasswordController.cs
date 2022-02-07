using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Request.Password;
using SpiderFleetWebAPI.Models.Response.Password;
using SpiderFleetWebAPI.Utils.Password;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Password
{
    public class PasswordController : ApiController
    {
        private UseFul use = new UseFul();
        private const string Tag = "Cambio de Contraseña";
        private const string BasicRoute = "api/";
        private const string ResourceName = "password/changepassword";

        /// <summary>
        /// Cambio de Contraseña de Usuario
        /// </summary>
        /// <remarks>
        /// Este EndPoint hece el cambio de la contraseña por usuario ingresado ya sea por username o email
        /// #### Ejemplo de entrada
        /// ##### Busca el emial o username de usuario
        /// ```
        /// {
        /// "login" : "laurag",
        /// "password" : "123456"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "login" : "string",
        /// "password" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="passwordRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Password </returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PasswordResponse))]
        public PasswordResponse Create([FromBody] PasswordRequest passwordRequest)
        {
            PasswordResponse response = new PasswordResponse();

            try
            {
                if (!(passwordRequest is PasswordRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                string username = string.Empty;

                try
                {
                    username = (new VerifyUser()).verifyTokenUser(User);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                PasswordDao dao = new PasswordDao();
                CredencialSpiderFleet.Models.Password.Passwords passwords = new CredencialSpiderFleet.Models.Password.Passwords();
                passwords.Login = username;
                passwords.OldPassword = passwordRequest.OldPassword;
                passwords.Password = passwordRequest.Password;

                //use = new UseFul();
                //passwordRequest.Password = UseFul.MD5(passwordRequest.Password);

                try
                {
                    response = dao.ChangePassword(passwords);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }
    }
}
