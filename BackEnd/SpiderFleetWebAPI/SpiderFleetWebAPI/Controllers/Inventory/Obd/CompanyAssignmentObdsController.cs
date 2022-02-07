using SpiderFleetWebAPI.Models.Request.Inventory.Obd;
using SpiderFleetWebAPI.Models.Response.Inventory.Obd;
using SpiderFleetWebAPI.Utils.Inventory.Obd;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Inventory.Obd
{
    public class CompanyAssignmentObdsController : ApiController
    {

        private const string Tag = "Asignacion de Obds";
        private const string BasicRoute = "api/";
        private const string ResourceName = "inventario/obdsasissignment";

        /// <summary>
        /// Actualizacion de Obds
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de los Obds
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla Obds
        /// ```
        /// {
        /// "IdCompany": 1,
        /// "AssignmentObds": [null, "EIRPFK858", "EIRPFK859", "dsfdsf", "."]
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "IdCompany": "int",
        /// "AssignmentObds": ["string"]
        /// }
        /// ```
        /// </example>
        /// <param name="obdListRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es una lista de errores</returns>
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ListErrorsObdsResponse))]
        public ListErrorsObdsResponse Update([FromBody] CompanyAssignmentObdsRequest obdListRequest)
        {
            ListErrorsObdsResponse response = new ListErrorsObdsResponse();
            try
            {
                if (!(obdListRequest is CompanyAssignmentObdsRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {
                    if (obdListRequest.IdCompany == 0)
                    {
                        response.success = false;
                        response.messages.Add("No se ha seleccionado la Compañia");
                        return response;
                    }

                    if (obdListRequest.AssignmentObds.Count == 0)
                    {
                        response.success = false;
                        response.messages.Add("No ingreso ningun dispositivo");
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                CredencialSpiderFleet.Models.Inventory.Obd.CompanyAssignmentObds obdList = new CredencialSpiderFleet.Models.Inventory.Obd.CompanyAssignmentObds();
                obdList.IdCompany = obdListRequest.IdCompany;
                obdList.AssignmentObds = obdListRequest.AssignmentObds;

                try
                {
                    Dictionary<string, string> listError = new Dictionary<string, string>();
                    listError = (new CompanyAssignmentObdsDao()).Update(obdList);

                    if (listError.Count == 0)
                    {
                        response.success = true;
                    }
                    if (listError.Count > 0)
                    {
                        response.success = false;
                        response.messages.Add("Verifique la lista de Errores");
                        response.listError = listError;
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
        /// Obtiene todos los registros de Obds
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla Obds
        /// <returns>Es una lista de Obds</returns>
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CompanyAssignmentObdsResponse))]
        public CompanyAssignmentObdsResponse GetListAsync()
        {
            CompanyAssignmentObdsResponse response = new CompanyAssignmentObdsResponse();
            try
            {
                try
                {
                    response.listObds = (new CompanyAssignmentObdsDao()).Read();
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
        /// Obtiene una lista de los Obds asignados a una Compañia un registro de Obds
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla Obds Asignados a una Compañia
        /// <returns>Es un objeto de Obd </returns>
        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CompanyAssignmentObdsResponse))]
        public CompanyAssignmentObdsResponse GetCompanyListId([FromUri(Name = "id")] int id)
        {
            CompanyAssignmentObdsResponse response = new CompanyAssignmentObdsResponse();
            try
            {
                try
                {
                    response.listObds = (new CompanyAssignmentObdsDao()).ReadId(id);
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

    }
}
