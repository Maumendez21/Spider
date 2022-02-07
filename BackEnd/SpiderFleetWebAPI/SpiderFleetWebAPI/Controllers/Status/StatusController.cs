using CredencialSpiderFleet.Models.DAO.Status;
using CredencialSpiderFleet.Models.Request.Status;
using CredencialSpiderFleet.Models.Response.Status;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Status
{
    public class StatusController : ApiController
    {
        private const string Tag = "Mantenimiento de Status";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration";

        /// <summary>
        /// Alta de Estatus
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
        /// <param name="statusRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Company estructura similar a la tabla Companies</returns>
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/status")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(StatusResponse))]
        public StatusResponse Create([FromBody] StatusRequest statusRequest)
        {
            StatusResponse response = new StatusResponse();

            try
            {
                if (!(statusRequest is StatusRequest))
                {
                    response.Success = false;
                    response.Messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {
                    if (string.IsNullOrEmpty(statusRequest.Description.Trim()))
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

                StatusDao dao = new StatusDao();
                try
                {
                    int respuesta = dao.Create(statusRequest);
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

        [HttpPut]
        [Route(BasicRoute + ResourceName + "/status")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(StatusResponse))]
        public StatusResponse Update([FromBody] StatusRequest statusRequest)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                if (!(statusRequest is StatusRequest))
                {
                    response.Success = false;
                    response.Messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {
                    if (statusRequest.IdStatus == 0)
                    {
                        response.Success = false;
                        response.Messages.Add("No tiene el parametro idRol");
                        return response;
                    }

                    if (string.IsNullOrEmpty(statusRequest.Description.Trim()))
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

                StatusDao dao = new StatusDao();
                try
                {
                    int respuesta = dao.Update(statusRequest);

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

        [HttpGet]
        [Route(BasicRoute + ResourceName + "/status")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(StatusResponse))]
        public StatusResponse GetListAsync()
        {
            StatusResponse response = new StatusResponse();
            try
            {
                StatusDao dao = new StatusDao();
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

        [HttpGet]
        [Route(BasicRoute + ResourceName + "/status/{id}")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(StatusResponse))]
        public StatusResponse GetListIdAsync(int id)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                StatusDao dao = new StatusDao();

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
