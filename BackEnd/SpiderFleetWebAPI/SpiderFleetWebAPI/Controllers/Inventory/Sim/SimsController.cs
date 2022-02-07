using SpiderFleetWebAPI.Models.Request.Inventory.Sim;
using SpiderFleetWebAPI.Models.Response.Inventory.Sim;
using SpiderFleetWebAPI.Utils.Inventory.Sim;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Inventory.Sim
{
    public class SimsController : ApiController
    {
        private const string Tag = "Mantenimiento de Sims";
        private const string BasicRoute = "api/";
        private const string ResourceName = "inventario/sims";
        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();
        private const int longitudSim = 12;

        /// <summary>
        /// Alta de Sims
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de los Sims
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla Sims
        /// ```
        /// {
        /// "sim" : "123456789015"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "sim" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="simRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Sims estructura similar a la tabla Sims</returns>
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimResponse))]
        public SimResponse Create([FromBody] SimRequest simRequest)
        {
            SimResponse response = new SimResponse();

            try
            {
                if (!(simRequest is SimRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {
                    if (string.IsNullOrEmpty(simRequest.Sim.Trim()))
                    {
                        response.success = false;
                        response.messages.Add("Ingrese el numero de Sim");
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                if (!string.IsNullOrEmpty(simRequest.Sim.Trim()))
                {
                    useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                    if (useful.hasSpecialChar(simRequest.Sim.Trim()))
                    {
                        response.success = false;
                        response.messages.Add("La cadena contiene caracteres especiales");
                        return response;
                    }

                    if (!useful.IsValidLength(simRequest.Sim.Trim(), longitudSim))
                    {
                        response.success = false;
                        response.messages.Add("La longitud excede de lo establecido rango maximo " + longitudSim + " caracteres");
                        return response;
                    }
                }

                CredencialSpiderFleet.Models.Inventory.Sim.Sims sim = new CredencialSpiderFleet.Models.Inventory.Sim.Sims();
                sim.Sim = simRequest.Sim;
                sim.Status = 1;
                sim.LastUploadDate = (DateTime?)null;

                try
                {
                    int respuesta = (new SimDao()).Create(sim);
                    if (respuesta == 1)
                    {
                        response.success = true;
                    }
                    else if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("Error al intenar dar de alta el registro el numero de SIM ya existe");
                        return response;
                    }
                    if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Error al intenar dar de alta el registro");
                        return response;
                    }
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
        /// Alta de Sims
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de los Sims
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla Sims
        /// ```
        /// {
        /// "idsim" : "4",
        /// "sim" : "123456789015"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "idsim" : "int",
        /// "sim" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="simRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Sims estructura similar a la tabla Sims</returns>
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimResponse))]
        public SimResponse Update([FromBody] SimUpdateRequest simRequest)
        {
            SimResponse response = new SimResponse();
            try
            {
                if (!(simRequest is SimRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {
                    if (simRequest.IdSim == 0)
                    {
                        response.success = false;
                        response.messages.Add("No tiene el parametro Id");
                        return response;
                    }

                    if (string.IsNullOrEmpty(simRequest.Sim.Trim()))
                    {
                        response.success = false;
                        response.messages.Add("Ingrese el numero de Sim");
                        return response;
                    }

                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                if (!string.IsNullOrEmpty(simRequest.Sim.Trim()))
                {
                    useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                    if (useful.hasSpecialChar(simRequest.Sim.Trim()))
                    {
                        response.success = false;
                        response.messages.Add("La cadena contiene caracteres especiales");
                        return response;
                    }

                    if (!useful.IsValidLength(simRequest.Sim.Trim(), longitudSim))
                    {
                        response.success = false;
                        response.messages.Add("La longitud excede de lo establecido rango maximo " + longitudSim + " caracteres");
                        return response;
                    }
                }

                CredencialSpiderFleet.Models.Inventory.Sim.Sims sim = new CredencialSpiderFleet.Models.Inventory.Sim.Sims();
                sim.IdSim = simRequest.IdSim;
                sim.Sim = simRequest.Sim;
               
                try
                {
                    int respuesta = (new SimDao()).Update(sim);

                    if (respuesta == 1)
                    {
                        response.success = true;
                    }
                    else if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("No se encuentra el registro");
                        return response;
                    }
                    else if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Error al tratar de actualizar el registro");
                        return response;
                    }
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
        /// Obtiene todos los registros de Sims
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla Sims
        /// <returns>Es una lista de Sims estructura similar a la tabla Sims</returns>
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimListResponse))]
        public SimListResponse GetListAsync()
        {
            SimListResponse response = new SimListResponse();
            try
            {
                SimDao dao = new SimDao();

                try
                {
                    response.listSims = dao.Read();
                    response.success = true;
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
        /// Obtiene un registro por Id de la tabla Sims
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// <param name="id">Id del registro a consultar</param>
        /// <returns>Es un objeto de Sims estructura similar a la tabla Sims</returns>
        [HttpGet]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimRegistryResponse))]
        public SimRegistryResponse GetListIdAsync([FromUri(Name = "id")]int id)
        {
            SimRegistryResponse response = new SimRegistryResponse();
            try
            {
                SimDao dao = new SimDao();

                try
                {
                    response.sim = dao.ReadId(id);
                    response.success = true;
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
        /// Elimina un registro por Id de la tabla Roles
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// #### Ejemplo de entrada
        /// ##### Regresa un registros de la tabla Roles
        /// <param name="id">Id del registro a eliminar</param>
        /// <returns></returns>
        [HttpDelete]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimDeleteResponse))]
        public SimDeleteResponse DeleteIdRole([FromUri(Name = "id")] int id)
        {
            SimDeleteResponse response = new SimDeleteResponse();
            try
            {
                try
                {
                    int respuesta = (new SimDao()).DeleteId(id);
                    if (respuesta == 1)
                    {
                        response.success = true;
                    }
                    else if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("El registro que desea eliminar no existe");
                        return response;
                    }
                    else if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Error al intenar eliminar el registro");
                        return response;
                    }
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


