using CredencialSpiderFleet.Models.DAO.Sim;
using CredencialSpiderFleet.Models.Request.Sim;
using CredencialSpiderFleet.Models.Response.Sim;
using CredencialSpiderFleet.Models.Useful;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Sim
{
    public class SimsController : ApiController
    {
        private UseFul use = new UseFul();
        private const string Tag = "Mantenimiento de Sims";
        private const string BasicRoute = "api/";
        private const string ResourceName = "warehouse";

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
        /// <param name="simRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Company estructura similar a la tabla Companies</returns>
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/sims")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(GenericResponse))]
        public SimResponse Create([FromBody] SimRequest simRequest)
        {
            SimResponse response = new SimResponse();

            try
            {
                if (!(simRequest is SimRequest))
                {
                    response.Success = false;
                    response.Messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {
                    if (string.IsNullOrEmpty(simRequest.Sim.Trim()))
                    {
                        response.Success = false;
                        response.Messages.Add("Ingrese el numero de Sim");
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                if (simRequest.Sim.Trim().Length > 12)
                {
                    response.Success = false;
                    response.Messages.Add("El numero de Sim solo es de 12 digitos");
                    return response;
                }


                SimDao dao = new SimDao();
                simRequest.Status = 1;
                simRequest.LastUploadDate = (DateTime?) null;// DateTime.Now;
                try
                {
                    int respuesta = dao.Create(simRequest);
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
        [Route(BasicRoute + ResourceName + "/sims")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimResponse))]
        public SimResponse Update([FromBody] SimRequest simRequest)
        {
            SimResponse response = new SimResponse();
            try
            {
                if (!(simRequest is SimRequest))
                {
                    response.Success = false;
                    response.Messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {
                    if (simRequest.IdSim == 0)
                    {
                        response.Success = false;
                        response.Messages.Add("No tiene el parametro Id");
                        return response;
                    }

                    if (string.IsNullOrEmpty(simRequest.Sim.Trim()))
                    {
                        response.Success = false;
                        response.Messages.Add("Ingrese el numero de Sim");
                        return response;
                    }

                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                if (simRequest.Sim.Trim().Length > 12)
                {
                    response.Success = false;
                    response.Messages.Add("El numero de Sim solo es de 12 digitos");
                    return response;
                }

                SimDao dao = new SimDao();
                try
                {
                    int respuesta = dao.Update(simRequest);

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
        [Route(BasicRoute + ResourceName + "/sims")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimResponse))]
        public SimResponse GetListAsync()
        {
            SimResponse response = new SimResponse();
            try
            {
                SimDao dao = new SimDao();

                try
                {
                    response.listSims = dao.Read();
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

        [HttpGet]
        [Route(BasicRoute + ResourceName + "/sims/{id}")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimResponse))]
        public SimResponse GetListIdAsync(int id)
        {
            SimResponse response = new SimResponse();
            try
            {
                SimDao dao = new SimDao();

                try
                {
                    response.listSims = dao.ReadId(id);
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

