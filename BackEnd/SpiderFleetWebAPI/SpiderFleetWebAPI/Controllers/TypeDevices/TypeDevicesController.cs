using CredencialSpiderFleet.Models.DAO.TypeDevices;
using CredencialSpiderFleet.Models.Request.TypeDevices;
using CredencialSpiderFleet.Models.Response.TypeDevices;
using CredencialSpiderFleet.Models.Useful;
using System;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.TypeDevices
{
    public class TypeDevicesController : ApiController
    {
        private UseFul use = new UseFul();
        private const string Tag = "Mantenimiento de Tipo de Dispositivos";
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
        /// <param name="typeRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Company estructura similar a la tabla Companies</returns>
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/typedevices")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(GenericResponse))]
        public TypeDevicesResponse Create([FromBody] TypeDevicesRequest typeRequest)
        {
            TypeDevicesResponse response = new TypeDevicesResponse();

            try
            {
                if (!(typeRequest is TypeDevicesRequest))
                {
                    response.Success = false;
                    response.Messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {
                    if (string.IsNullOrEmpty(typeRequest.Name.Trim()))
                    {
                        response.Success = false;
                        response.Messages.Add("Ingrese el Nombre ");
                        return response;
                    }

                    if (string.IsNullOrEmpty(typeRequest.Description.Trim()))
                    {
                        response.Success = false;
                        response.Messages.Add("Ingrese Descripcion");
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                TypeDevicesDao dao = new TypeDevicesDao();
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
        [Route(BasicRoute + ResourceName + "/typedevices")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeDevicesResponse))]
        public TypeDevicesResponse Update([FromBody] TypeDevicesRequest typeRequest)
        {
            TypeDevicesResponse response = new TypeDevicesResponse();
            try
            {
                if (!(typeRequest is TypeDevicesRequest))
                {
                    response.Success = false;
                    response.Messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {
                    if (typeRequest.idTypeDevice == 0)
                    {
                        response.Success = false;
                        response.Messages.Add("No tiene el parametro Id");
                        return response;
                    }

                    if (string.IsNullOrEmpty(typeRequest.Name.Trim()))
                    {
                        response.Success = false;
                        response.Messages.Add("Ingrese el Nombre ");
                        return response;
                    }

                    if (string.IsNullOrEmpty(typeRequest.Description.Trim()))
                    {
                        response.Success = false;
                        response.Messages.Add("Ingrese Descripcion");
                        return response;
                    }

                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                TypeDevicesDao dao = new TypeDevicesDao();
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
        [Route(BasicRoute + ResourceName + "/typedevices")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeDevicesResponse))]
        public TypeDevicesResponse GetListAsync()
        {
            TypeDevicesResponse response = new TypeDevicesResponse();
            try
            {
                TypeDevicesDao dao = new TypeDevicesDao();

                try
                {
                    response.listType = dao.Read();
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
        [Route(BasicRoute + ResourceName + "/typedevices/{id}")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeDevicesResponse))]
        public TypeDevicesResponse GetListIdAsync(int id)
        {
            TypeDevicesResponse response = new TypeDevicesResponse();
            try
            {
                TypeDevicesDao dao = new TypeDevicesDao();

                try
                {
                    response.listType = dao.ReadId(id);
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

