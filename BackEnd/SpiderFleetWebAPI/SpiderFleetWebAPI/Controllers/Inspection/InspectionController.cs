using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using SpiderFleetWebAPI.Models.Response.Inspection;
using SpiderFleetWebAPI.Models.Request.Inspection;
using SpiderFleetWebAPI.Utils.Inspection;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using SpiderFleetWebAPI.Utils.VerifyUser;
using SpiderFleetWebAPI.Utils.User;

namespace SpiderFleetWebAPI.Controllers.Inspection
{
    public class InspectionController : ApiController
    {
        private const string Tag = "Mantenimiento Inspeccion";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/inspection";
        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();

        /// <summary>
        /// Alta de Inspeccion
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera un registro en InspectionResults y InspectionPrincipal
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla Mechanics
        /// ```
        /// {
        /// "listinspectionresult" : [
        ///     {
        ///         "Folio" : "",
        ///         "yes" : 1,
        ///         "no" : 0,
        ///         "Good" : 1,
        ///         "Regular" : 0,
        ///         "Bad" : 0,
        ///         "Notes" : "Una fisura",
        ///         "idTemplate" : 1
        ///     }
        ///    ],
        ///    "node" : "/72/",
        ///    "Folio" : "",
        ///    "Date" : "2021/07/08",
        ///    "idMechanic" : 1,
        ///    "idType" : 2,
        ///    "device" : "213GDP2017010383",
        ///    "mileage" : "73200km",
        ///    "idResponsible" : 1
        ///    }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "listinspectionresult" : [
        ///     {
        ///         "Folio" : string
        ///         "yes" : int
        ///         "no" : int
        ///         "Good" : int
        ///         "Regular" : int
        ///         "Bad" : int
        ///         "Notes" : string
        ///         "idTemplate" : int
        ///     }
        ///    ],
        ///    "node" : string
        ///    "Folio" : string
        ///    "Date" : string
        ///    "idMechanic" : int
        ///    "idType" : int
        ///    "device" : string
        ///    "mileage" : string
        ///    "idResponsible" : int
        ///    }
        /// ```
        /// </example>
        /// <param name="inspectionrequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de que contiene los objetos de Inspeccionresults y inspeccionprincipal</returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(InspectionResponse))]
        public InspectionResponse Create([FromBody] InspectionRequest inspectionrequest)
        {
            InspectionResponse response = new InspectionResponse();

            try
            {
                if (!(inspectionrequest is InspectionRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

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

                List<CredencialSpiderFleet.Models.Inspection.InspectionResults> Results = new List<CredencialSpiderFleet.Models.Inspection.InspectionResults>();
                CredencialSpiderFleet.Models.Inspection.InspectionPrincipal principal = new CredencialSpiderFleet.Models.Inspection.InspectionPrincipal();
                Results = inspectionrequest.listinspectionresult;
                principal.node = hierarchy;
                principal.folio = inspectionrequest.Folio;
                principal.date = DateTime.Now.ToString("yyyy-MM-dd");// inspectionrequest.Date;
                principal.idMechanic = inspectionrequest.idMechanic;
                principal.idType = inspectionrequest.idType;
                principal.device = inspectionrequest.device;
                principal.mileage = inspectionrequest.mileage;
                principal.idResponsible = inspectionrequest.idResponsible;
                try
                {
                    response = (new InspectionDAO()).Create(Results,principal);

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
        /// Actualizacion en Inspection
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro de las tablas de inspeccion 
        /// #### Ejemplo de entrada
        /// ##### Actualiza los registros de inspection Results y Principal
        /// ```
        /// {
        /// "Folio" : "2021000006",
        /// "Date" : "2022/07/08",
        /// "device" : "213GDP2017010383",
        /// "mileage" : "120000km",
        /// "Results" : [
        ///     {
        ///         "idResults": 1,
        ///         "Folio" : "2021000006",
        ///         "yes" : 0,
        ///         "no" : 0,
        ///         "Good" : 1,
        ///         "Regular" : 0,
        ///         "Bad" : 0,
        ///         "Notes" : "Una fisura",
        ///         "idTemplate" : 1
        ///      }
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "Folio" : string,
        /// "Date" : string,
        /// "device" : string,
        /// "mileage" : string,
        /// "Results" : [
        ///     {
        ///         "idResults": int,
        ///         "Folio" : string,
        ///         "yes" : int,
        ///         "no" : int,
        ///         "Good" : int,
        ///         "Regular" : int,
        ///         "Bad" : int,
        ///         "Notes" : string,
        ///         "idTemplate" : int
        ///      }
        /// }
        /// ```
        /// </example>
        /// <param name="inspectionUpdateRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Inspection estructura que contiene estructura similar a inspeccion Results y Principal</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(InspectionResponseup))]
        public InspectionResponseup Update([FromBody] InspectionUpdateRequest inspectionUpdateRequest)
        {
            InspectionResponseup response = new InspectionResponseup();
            try
            {
                if (!(inspectionUpdateRequest is InspectionUpdateRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                List<CredencialSpiderFleet.Models.Inspection.InspectionResults> results = new List<CredencialSpiderFleet.Models.Inspection.InspectionResults>();
                CredencialSpiderFleet.Models.Inspection.InspectionPrincipal inspectionprincipal = new CredencialSpiderFleet.Models.Inspection.InspectionPrincipal();
                results = inspectionUpdateRequest.results;
                inspectionprincipal.folio = inspectionUpdateRequest.Folio;
                inspectionprincipal.date = inspectionUpdateRequest.Date;
                inspectionprincipal.device = inspectionUpdateRequest.device;
                inspectionprincipal.mileage = inspectionUpdateRequest.mileage;
                try
                {
                    response = (new InspectionDAO()).UpdateInspection(results, inspectionprincipal);
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
        /// Lista de Inspection por folio
        /// </summary>
        /// <remarks>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list/folio")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(InspectionResponseList))]
        public InspectionResponseList GetListAsync([FromUri(Name = "folio")] string folio)
        {
            InspectionResponseList response = new InspectionResponseList();
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

                try
                {

                    response = (new InspectionDAO()).ReadFolio(hierarchy,folio);


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
        /// Lista de Inspection
        /// </summary>
        /// <remarks>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(InspectionResponseList))]
        public InspectionResponseList GetListAsync()
        {
            InspectionResponseList response = new InspectionResponseList();
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

                try
                {

                   response  = (new InspectionDAO()).ReadGeneral(hierarchy);


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
        /// Lista de plantilla inspection
        /// </summary>
        /// <remarks>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list/new")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(InspectionResponseList))]
        public headersandTemplatesResponse GetListnewplantilla()
        {
            headersandTemplatesResponse response = new headersandTemplatesResponse();
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

                try
                {

                    response = (new InspectionDAO()).Readplantilla();


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