using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Request.User;
using SpiderFleetWebAPI.Models.Response.User;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Data;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.User
{
    public class UserController : ApiController
    {
        private UseFul use = new UseFul();
        private const string Tag = "Mantenimiento de Usuarios";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/users";

        /// <summary>
        /// Creacion de Usuario
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro 
        /// #### Ejemplo de entrada
        /// ```
        /// {
        ///"Name": "Karla",
        ///"LastName": "Zepeda",
        ///"Email": "karla@gmail.com",
        ///"UserName": "karla_zepeda",
        ///"Password": "123456",
        ///"Telephone": "2225898900",
        ///"CompanyName":"Spider Fleet",
        ///"TaxId":"RDGDKUYS3424",
        ///"TaxName":"Razon Social",
        ///"Type":1
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        ///"Name": "string",
        ///"LastName": "string",
        ///"Email": "string",
        ///"UserName": "string",
        ///"Password": "string",
        ///"Telephone": "string",
        ///"CompanyName":"string",
        ///"TaxId":"string",
        ///"TaxName":"string",
        ///"Type":"int"
        /// }
        /// ```
        /// </example>
        /// <param name="userRequest">Objeto de entrada para el Endpoint</param>
        /// <returns></returns>
        [HttpPost]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserResponse))]
        public UserResponse Create([FromBody] UserRequest userRequest)
        {
            UserResponse response = new UserResponse();

            try
            {
                if (!(userRequest is UserRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                use = new UseFul();
                CredencialSpiderFleet.Models.User.User user = new CredencialSpiderFleet.Models.User.User();
                user.IdRole = 2;
                user.Name = userRequest.Name;
                user.LastName = userRequest.LastName;
                user.Email = userRequest.Email;
                user.UserName = userRequest.UserName;
                user.Password = userRequest.Password;
                user.Telephone = userRequest.Telephone;
                user.IdStatus = 1;
                user.CompanyName = userRequest.CompanyName;
                user.TaxId = userRequest.TaxId;
                user.TaxName = userRequest.TaxName;
                user.Type = userRequest.Type;

                try
                {
                    response  = (new UserDao()).Create(user);
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
        /// Actualiza Usuario
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro 
        /// #### Ejemplo de entrada
        /// ```
        /// {
        ///"Name": "Karla",
        ///"LastName": "Zepeda",
        ///"Email": "karla@gmail.com",
        ///"Telephone": "2225898900"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        ///"Name": "string",
        ///"LastName": "string",
        ///"Email": "string",
        ///"Telephone": "string"
        /// }
        /// ```
        /// </example>
        /// <param name="userRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de User estructura similar a la tabla User</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserResponse))]
        public UserResponse Update([FromBody] UserUpdateRequest userRequest)
        {
            UserResponse response = new UserResponse();
            try
            {
                if (!(userRequest is UserUpdateRequest))
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

                CredencialSpiderFleet.Models.User.User user = new CredencialSpiderFleet.Models.User.User();
                user.Name = userRequest.Name;
                user.LastName = userRequest.LastName;
                user.Email = userRequest.Email;
                user.UserName = userRequest.UserName;
                user.Telephone = userRequest.Telephone;

                try
                {
                    response = (new UserDao()).Update(user);
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
        /// Obtiene todos los registros de User
        /// </summary>
        /// <remarks>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserResponse))]
        public UserListResponse GetListAsync()
        {
            UserListResponse response = new UserListResponse();

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
                try
                {
                    response = (new UserDao()).Read(username);
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
        /// Obtiene un registro de User
        /// </summary>
        /// <remarks>
        /// <param name="idusername" >Id del Usuario</param>
        /// <returns>Es un objeto de User </returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserRegistryResponse))]
        public UserRegistryResponse GetListIdAsync([FromUri (Name = "idusername")] string idusername)
        {
            UserRegistryResponse response = new UserRegistryResponse();
            
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
                try
                {
                    response = (new UserDao()).ReadId(username, idusername);
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