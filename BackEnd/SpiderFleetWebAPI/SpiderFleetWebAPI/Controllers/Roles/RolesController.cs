using CredencialSpiderFleet.Models.DAO.Roles;
using CredencialSpiderFleet.Models.Request.Roles;
using CredencialSpiderFleet.Models.Response.Roles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Roles
{
    public class RolesController : ApiController
    {
        private const string Tag = "Mantenimiento de Roles";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration";

        /// <summary>
        /// Alta de Roles
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de la comañia
        /// #### Ejemplo de entrada
        /// ##### Busca los datos de un vehiculo con ID XXXX
        /// ```
        /// {
        /// "idsuscriptiontype" : 3
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "idVehiculo" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="rolRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Roles estructura similar a la tabla Roles</returns>
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/roles")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(RolesResponse))]
        public RolesResponse Create([FromBody] RolesRequest rolRequest)
        {
            RolesResponse response = new RolesResponse();

            try
            {
                if (!(rolRequest is RolesRequest))
                {
                    response.Success = false;
                    response.Messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {
                    if (string.IsNullOrEmpty(rolRequest.Description.Trim()))
                    {
                        response.Success = false;
                        response.Messages.Add("Ingrese la Descripción");
                        return response;
                    }

                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                RolesDao dao = new RolesDao();
                rolRequest.Status = 1;
                try
                {
                    int respuesta = dao.Create(rolRequest);
                    if (respuesta == 3)
                    {
                        response.Success = false;
                        response.Messages.Add("Error al intenar dar de alta el registro");
                        return response;
                    }
                    else if (respuesta == 1)
                    {
                        response.Success = true;
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Messages.Add(ex.Message);
                return response;
            }
        }

        /// <summary>
        /// Actuliazacion de Roles
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro de la tabla roles
        /// #### Ejemplo de entrada
        /// ##### Busca los datos de un vehiculo con ID XXXX
        /// ```
        /// {
        /// "idrole":1,
        /// "description":"administradorssss"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "idrole" : "int",
        /// "description":"string"
        /// }
        /// ```
        /// </example>
        /// <param name="rolRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Roles estructura similar a la tabla Roles</returns>
        [HttpPut]
        [Route(BasicRoute + ResourceName + "/roles")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(RolesResponse))]
        public RolesResponse Update([FromBody] RolesRequest rolRequest)
        {
            RolesResponse response = new RolesResponse();
            try
            {
                if (!(rolRequest is RolesRequest))
                {
                    response.Success = false;
                    response.Messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {
                    if (rolRequest.IdRole == 0)
                    {
                        response.Success = false;
                        response.Messages.Add("No tiene el parametro idRol");
                        return response;
                    }

                    if (string.IsNullOrEmpty(rolRequest.Description.Trim()))
                    {
                        response.Success = false;
                        response.Messages.Add("Ingrese la Descripción");
                        return response;
                    }

                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                RolesDao dao = new RolesDao();
                try
                {
                    int respuesta = dao.Update(rolRequest);

                    if (respuesta == 1)
                    {
                        response.Success = true;
                    }
                    else if (respuesta == 2)
                    {
                        response.Success = false;
                        response.Messages.Add("No se encuentra el registro");
                        return response;
                    }
                    else if (respuesta == 3)
                    {
                        response.Success = false;
                        response.Messages.Add("Error al tratar de actualizar el registro");
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Messages.Add(ex.Message);
                return response;
            }
        }

        /// <summary>
        /// Obtiene todos los Roles
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros
        /// #### Ejemplo de entrada
        /// ##### Busca los datos de la tabla Roles
        /// <param name="rolRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Roles estructura similar a la tabla Roles</returns>
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/roles")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(RolesResponse))]
        public RolesResponse GetListAsync()
        {
            RolesResponse response = new RolesResponse();
            try
            {
                RolesDao dao = new RolesDao();
                DataSet ds = new DataSet();

                try
                {
                    response.ds = dao.Read();
                    response.Success = false;
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Messages.Add(ex.Message);
                return response;
            }

        }

        /// <summary>
        /// Obtiene un registro porid de la tabla Roles
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registros
        /// #### Ejemplo de entrada
        /// ##### Busca un registro por id de la tabla Roles
        /// <param name="rolRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Roles estructura similar a la tabla Roles</returns>
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/roles/{id}")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(RolesResponse))]
        public RolesResponse GetListIdAsync(int id)
        {
            RolesResponse response = new RolesResponse();
            try
            {
                RolesDao dao = new RolesDao();

                try
                {
                    response.ds = dao.ReadId(id);
                    response.Success = true;
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Messages.Add(ex.Message);
                return response;
            }
        }

    }
}