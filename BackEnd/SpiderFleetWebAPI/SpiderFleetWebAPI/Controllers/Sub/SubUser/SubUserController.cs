using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Request.Sub.SubUser;
using SpiderFleetWebAPI.Models.Response.Sub.SubUser;
using SpiderFleetWebAPI.Utils.Sub.SubUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Security.Claims;
using SpiderFleetWebAPI.Utils.VerifyUser;
using SpiderFleetWebAPI.Utils.User;

namespace SpiderFleetWebAPI.Controllers.Sub.SubUser
{
    public class SubUserController : ApiController
    {
        private const string Tag = "Mantenimiento de Sub Usuarios";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/subusers";
        private UseFul use = new UseFul();


        /// <summary>
        /// Alta de Sub Usuario
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de la Compañia
        /// #### Ejemplo de entrada
        /// ```
        /// {
        /// "Name": "Rodolfo",
        /// "LastName": "Garcia",
        /// "Email": "rodolfo@gmail.com",
        /// "UserName": "rodolfo",
        /// "Password": "98765",
        /// "Telephone": "7733776622"
        /// "Grupo": "/72/2/"
        /// "Role": 3
        /// "Status": 1
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "Name": "string",
        /// "LastName": "string",
        /// "Email": "string",
        /// "UserName": "string",
        /// "Password": "string",
        /// "Telephone": "string",
        /// "Grupo": "string",
        /// "Role": int
        /// "Status": int
        /// }
        /// ```
        /// </example>
        /// <param name="userRequest">Objeto de entrada para el Endpoint</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubUserResponse))]
        public SubUserResponse Create([FromBody] SubUserRequest userRequest)
        {
            SubUserResponse response = new SubUserResponse();

            try
            {
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

                if (!(userRequest is SubUserRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                use = new UseFul();
                CredencialSpiderFleet.Models.Sub.SubUser.SubUser user = new CredencialSpiderFleet.Models.Sub.SubUser.SubUser();
                user.UserNameLevel = username;
                user.Name = userRequest.Name;
                user.LastName = userRequest.LastName;
                user.Email = userRequest.Email;
                user.UserName = userRequest.UserName;
                //user.Password = use.MD5(userRequest.Password);
                user.Password = userRequest.Password;
                user.Telephone = userRequest.Telephone;
                user.IdStatus = userRequest.Status;
                user.IdRole = userRequest.Role;
                user.Hierarchy = userRequest.Grupo;//( "/1000000/";

                try
                {
                    response = (new SubUserDao()).Create(user);
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

        /// <summary>
        /// Actualiza los datos de Sub Usuario
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro 
        /// #### Ejemplo de entrada
        /// ```
        /// {
        /// "UserName":"gerardo",
        /// "Name": "Rodolfo",
        /// "LastName": "Garcia",
        /// "Email": "rodolfo@gmail.com",
        /// "Telephone": "7733776622"
        /// "Grupo": "/72/2/"
        /// "Role": 3
        /// "Status": 1
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "UserName":"string",
        /// "Name": "string",
        /// "LastName": "string",
        /// "Email": "string",
        /// "Telephone": "string"
        /// "Grupo": "string",
        /// "Role": int
        /// "Status": int
        /// }
        /// ```
        /// </example>
        /// <param name="userRequest">Objeto de entrada para el Endpoint</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubUserResponse))]
        public SubUserResponse Update([FromBody] SubUserUpdateRequest userRequest)
        {
            SubUserResponse response = new SubUserResponse();
            try
            {
                string username = string.Empty;
                string hierarchy = string.Empty;
                try
                {
                    username = (new VerifyUser()).verifyTokenUser(User);
                    hierarchy = (new UserDao()).ReadUserHierarchy(username);

                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                if (!(userRequest is SubUserUpdateRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Sub.SubUser.SubUser user = new CredencialSpiderFleet.Models.Sub.SubUser.SubUser();
                user.IdUser = hierarchy; 
                user.Name = userRequest.Name;
                user.LastName = userRequest.LastName;
                user.Email = userRequest.Email;
                user.UserName = userRequest.UserName;
                user.Telephone = userRequest.Telephone;
                user.IdRole = userRequest.Role;
                user.IdStatus = userRequest.Status;
                user.Hierarchy = userRequest.Grupo;

                try
                {
                    response = (new SubUserDao()).Update(user);
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

        /// <summary>
        /// Obtiene una lista de Sub Users
        /// </summary>
        /// <remarks>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubUserResponse))]
        public SubUserListResponse GetListAsync([FromUri(Name = "search")] string search)
        {
            SubUserListResponse response = new SubUserListResponse();
            try
            {
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

                try
                {
                    response = (new SubUserDao()).Read(username, search);
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

        /// <summary>
        /// Obtiene un registro de un Sub User por Id Usuario
        /// </summary>
        /// <remarks>
        /// <returns>Es un objeto de User </returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubUserRegistryResponse))]
        public SubUserRegistryResponse GetListIdAsync([FromUri(Name = "id")]string id)
        {
            SubUserRegistryResponse response = new SubUserRegistryResponse();
            try
            {
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

                try
                {
                    response = (new SubUserDao()).ReadId(id);
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