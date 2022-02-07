using CredencialSpiderFleet.Models.Sims;
using SpiderFleetWebAPI.Models.Response.Sims;
using SpiderFleetWebAPI.Utils.Message;
using SpiderFleetWebAPI.Utils.Sims;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.MaintenanceSims
{
    public class SimMaintenanceController : ApiController
    {
        private const string Tag = "Mantenimiento de Sim Pagina";
        private const string BasicRoute = "api/";
        private const string ResourceName = "maintenance/sims";

        /// <summary>
        /// Actualiza los sims en Base de Datos
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actulaiza  los sims en base de datos
        /// #### Ejemplo de entrada
        /// </remarks>
        /// <example>
        /// </example>
        /// <param >Objeto de entrada para el Endpoint</param>
        /// <returns>success = true o false;</returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/bulking")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimMaintenanceResponse))]
        public SimMaintenanceResponse Bulking()
        {
            SimMaintenanceResponse response = new SimMaintenanceResponse();

            try
            {
                try
                {
                    response = (new SimsMaintenanceDao()).bulking();
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
        /// Actualiza credito en los sims
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actulaiza credito en los sims de la pagina 
        /// #### Ejemplo de entrada
        /// </remarks>
        /// <example>
        /// </example>
        /// <param >Objeto de entrada para el Endpoint</param>
        /// <returns>success = true o false;</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName + "/credit")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimMaintenanceResponse))]
        public SimMaintenanceResponse Credit()
        {
            SimMaintenanceResponse response = new SimMaintenanceResponse();

            try
            {
                try
                {
                    response = (new SimsMaintenanceDao()).Credit();
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
        /// Regresa el credito a la cuenta
        /// </summary>
        /// <remarks>
        /// Este EndPoint que devuelve el dinero a la cuenta
        /// #### Ejemplo de entrada
        /// </remarks>
        /// <example>
        /// </example>
        /// <param >Objeto de entrada para el Endpoint</param>
        /// <returns>success = true o false;</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName + "/transfer/credit")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimMaintenanceResponse))]
        public TransferCreditResponse TransferCredit()
        {
            TransferCreditResponse response = new TransferCreditResponse();

            try
            {
                try
                {
                    string username = string.Empty;
                    username = (new VerifyUser()).verifyTokenUser(User);

                    response = (new SimsMaintenanceDao()).TransferCreditDebitAccount();
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
                response.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                response.messages.Add(ex.Message);
                return response;
            }
        }


        /// <summary>
        /// Pone credito por Empresa
        /// </summary>
        /// <remarks>
        /// Este EndPoint que devuelve el dinero a la cuenta
        /// #### Ejemplo de entrada
        /// </remarks>
        /// <example>
        /// </example>
        /// <param >Objeto de entrada para el Endpoint</param>
        /// <returns>success = true o false;</returns>
        //[Authorize] 
        [HttpPut]
        [AllowAnonymous]
        [Route(BasicRoute + ResourceName + "/company/credit")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimMaintenanceResponse))]
        public SimMaintenanceResponse CreditSimCompany([FromUri(Name = "hierarchy")] string hierarchy)
        {
            SimMaintenanceResponse response = new SimMaintenanceResponse();

            try
            {
                try
                {
                    //string username = string.Empty;
                    //username = (new VerifyUser()).verifyTokenUser(User);

                    response = (new SimsMaintenanceDao()).CreditCompany(hierarchy);

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
                response.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                response.messages.Add(ex.Message);
                return response;
            }
        }


        [HttpPut]
        [Route(BasicRoute + ResourceName + "/company/transfer/credit")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimMaintenanceResponse))]
        public SimMaintenanceResponse TransferCreditSimCompany([FromUri(Name = "hierarchy")] string hierarchy)
        {
            SimMaintenanceResponse response = new SimMaintenanceResponse();

            try
            {
                try
                {
                    //string username = string.Empty;
                    //username = (new VerifyUser()).verifyTokenUser(User);

                    response = (new SimsMaintenanceDao()).TransferCreditCompany(hierarchy);
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
                response.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                response.messages.Add(ex.Message);
                return response;
            }
        }


        /// <summary>
        /// Lista de Sims, Credito,Dispositivos,Empresa y subempresa pertenece
        /// </summary>
        /// <remarks>
        /// Este EndPoint que devuelve sims y el credito que tienen
        /// #### Ejemplo de entrada
        /// </remarks>
        /// <example>
        /// </example>
        /// <param >Objeto de entrada para el Endpoint</param>
        /// <returns>success = true o false;</returns>
        //[Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/report/credit/sims")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ReportCreditSimsResponse))]
        public ReportCreditSimsResponse ReportCreditSim()
        {
            ReportCreditSimsResponse response = new ReportCreditSimsResponse();

            try
            {
                try
                {
                    //string username = string.Empty;
                    //username = (new VerifyUser()).verifyTokenUser(User);

                    response = (new SimsMaintenanceDao()).ReporteCreditoSims();
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
                response.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                response.messages.Add(ex.Message);
                return response;
            }
        }

        [HttpPut]
        [AllowAnonymous]
        [Route(BasicRoute + ResourceName + "/all/credit")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimMaintenanceResponse))]
        public SimMaintenanceResponse CreditSimAllCompany()
        {
            SimMaintenanceResponse response = new SimMaintenanceResponse();

            try
            {
                try
                {
                    response = (new SimsMaintenanceDao()).CreditSimsAllCompany();
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
                response.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                response.messages.Add(ex.Message);
                return response;
            }
        }


    }
}