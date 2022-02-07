using CredencialSpiderFleet.Models.DAO.SuscriptionsType;
using CredencialSpiderFleet.Models.Request.SuscriptionsType;
using CredencialSpiderFleet.Models.Response.SuscriptionsType;
using CredencialSpiderFleet.Models.Useful;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.SuscriptionsType
{
    public class SuscriptionTypeController : ApiController
    {
        private UseFul use = new UseFul();
        private const string Tag = "Mantenimiento de Tipo de Suscripciones";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration";

        /// <summary>
        /// Alta de Tipo de Suscripciones
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
        /// <param name="typeRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Company estructura similar a la tabla Companies</returns>
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/suscriptionstype")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(SuscriptionsTypeResponse))]
        public SuscriptionsTypeResponse Create([FromBody] SuscriptionTypeRequest typeRequest)
        {
            SuscriptionsTypeResponse response = new SuscriptionsTypeResponse();

            try
            {

                if (!(typeRequest is SuscriptionTypeRequest))
                {
                    response.Success = false;
                    response.Messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {

                    if (string.IsNullOrEmpty(typeRequest.Description.Trim()))
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

                SuscriptionTypeDao dao = new SuscriptionTypeDao();
                try
                {
                    int respuesta = dao.Create(typeRequest);
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
        [Route(BasicRoute + ResourceName + "/suscriptionstype")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(SuscriptionsTypeResponse))]
        public SuscriptionsTypeResponse Update([FromBody] SuscriptionTypeRequest typeRequest)
        {
            SuscriptionsTypeResponse response = new SuscriptionsTypeResponse();
            try
            {
                if (!(typeRequest is SuscriptionTypeRequest))
                {
                    response.Success = false;
                    response.Messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {
                    if (typeRequest.IdSuscriptionType == 0)
                    {
                        response.Success = false;
                        response.Messages.Add("No tiene el parametro idSuscriptionType");
                        return response;
                    }

                    if (string.IsNullOrEmpty(typeRequest.Description.Trim()))
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

                SuscriptionTypeDao dao = new SuscriptionTypeDao();
                try
                {
                    int respuesta = dao.Update(typeRequest);

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
        [Route(BasicRoute + ResourceName + "/suscriptionstype")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(SuscriptionsTypeResponse))]
        public SuscriptionsTypeResponse GetListAsync()
        {
            SuscriptionsTypeResponse response = new SuscriptionsTypeResponse();
            try
            {
                SuscriptionTypeDao dao = new SuscriptionTypeDao();
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
        [Route(BasicRoute + ResourceName + "/suscriptionstype/{id}")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(SuscriptionsTypeResponse))]
        public SuscriptionsTypeResponse GetListIdAsync(int id)
        {
            SuscriptionsTypeResponse response = new SuscriptionsTypeResponse();
            try
            {
                SuscriptionTypeDao dao = new SuscriptionTypeDao();

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
