using SpiderFleetWebAPI.Models.Request.Catalog.SuscriptionsType;
using SpiderFleetWebAPI.Models.Response.Catalog.SuscriptionsType;
using SpiderFleetWebAPI.Utils.Catalog.SuscriptionsType;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Catalog.SuscriptionsType
{
    public class SuscriptionsTypeController : ApiController
    {
        private const string Tag = "Mantenimiento de Tipo de Suscripciones";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/suscriptionstype";

        /// <summary>
        /// Alta de Tipo de Suscripciones
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de la Tipo de Suscripciones
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla Tipo de Suscripciones
        /// ```
        /// {
        /// "description" : "Trimestralssss"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "description" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="typeRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Tipo de Suscripciones estructura similar a la tabla Tipo de Suscripciones</returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SuscriptionsTypeResponse))]
        public SuscriptionsTypeResponse Create([FromBody] SuscriptionTypeRequest typeRequest)
        {
            SuscriptionsTypeResponse response = new SuscriptionsTypeResponse();

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

                if (!(typeRequest is SuscriptionTypeRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                SuscriptionsTypeDao dao = new SuscriptionsTypeDao();
                CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType suscription = new CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType();
                suscription.Description = typeRequest.Description;

                try
                {
                    response = dao.Create(suscription);
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
        /// Actuliazacion de Tipo de Suscripciones
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro de la tabla status
        /// #### Ejemplo de entrada
        /// ##### Actualiza un resgistro en la Tabla Tipo de Suscripciones
        /// ```
        /// {
        /// "idsuscriptiontype":1,
        /// "description":"Trimestral"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "idstatus" : "int",
        /// "description":"string"
        /// }
        /// ```
        /// </example>
        /// <param name="typeRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Tipo de Suscripciones estructura similar a la tabla Tipo de Suscripciones</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SuscriptionsTypeResponse))]
        public SuscriptionsTypeResponse Update([FromBody] SuscriptionTypeUpdateRequest typeRequest)
        {
            SuscriptionsTypeResponse response = new SuscriptionsTypeResponse();

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
                if (!(typeRequest is SuscriptionTypeRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                SuscriptionsTypeDao dao = new SuscriptionsTypeDao();
                CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType suscription = new CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType();
                suscription.IdSuscriptionType = typeRequest.IdSuscriptionType;
                suscription.Description = typeRequest.Description;

                try
                {
                    response= dao.Update(suscription);
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
        /// Obtiene todos los registros de Tipo de Suscripciones
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla Tipo de Suscripciones
        /// <returns>Es una lista de Tipo de Suscripciones estructura similar a la tabla Tipo de Suscripciones</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SuscriptionsTypeResponse))]
        public SuscriptionsTypeListResponse GetListAsync()
        {
            SuscriptionsTypeListResponse response = new SuscriptionsTypeListResponse();

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
                    response = (new SuscriptionsTypeDao()).Read();
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
        /// Obtiene un registro por Id de la tabla Tipo de Suscripciones
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// <param name="id">Id del registro a consultar</param>
        /// <returns>Es un objeto de Tipo de Suscripciones estructura similar a la tabla Tipo de Suscripciones</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SuscriptionTypeRegistryResponse))]
        public SuscriptionTypeRegistryResponse GetListIdAsync([FromUri(Name = "id")] int id)
        {
            SuscriptionTypeRegistryResponse response = new SuscriptionTypeRegistryResponse();
            
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
                SuscriptionsTypeDao dao = new SuscriptionsTypeDao();

                try
                {
                    response = dao.ReadId(id);
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

