using SpiderFleetWebAPI.Models.Request.Inventory.Obd;
using SpiderFleetWebAPI.Models.Response.Inventory.Obd;
using SpiderFleetWebAPI.Utils.Inventory.Obd;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Inventory.Obd
{
    public class ObdController : ApiController
    {
        //private const string Tag = "Mantenimiento de Obds";
        //private const string BasicRoute = "api/";
        //private const string ResourceName = "inventario/obds";
        //private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();
        //private const int longitudLabel = 50;
        //private const int longitudDevice = 15;

        /// <summary>
        /// Alta de Obds
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de los Obds
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla Obds
        /// ```
        /// {
        /// "iddevice" : "123456789015",
        /// "label" : "123456789015",
        /// "idcompany" : 1,
        /// "idtype" : 2,
        /// "idsim" : 1
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "iddevice" : "string",
        /// "label" : "string",
        /// "idcompany" : "int",
        /// "idtype" : "int",
        /// "idsim" : "int"
        /// }
        /// ```
        /// </example>
        /// <param name="obdRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Obds estructura similar a la tabla Obds</returns>
        //[HttpPost]
        //[Route(BasicRoute + ResourceName)]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ObdResponse))]
        //public ObdResponse Create([FromBody] ObdRequest obdRequest)
        //{
        //    ObdResponse response = new ObdResponse();

        //    try
        //    {
        //        if (!(obdRequest is ObdRequest))
        //        {
        //            response.success = false;
        //            response.messages.Add("Objeto de entrada invalido");
        //            return response;
        //        }

        //        try
        //        {
        //            if (string.IsNullOrEmpty(obdRequest.IdDevice.Trim()))
        //            {
        //                response.success = false;
        //                response.messages.Add("Ingrese el numero de Dispositivo");
        //                return response;
        //            }

        //            if (string.IsNullOrEmpty(obdRequest.Label.Trim()))
        //            {
        //                response.success = false;
        //                response.messages.Add("Ingrese el Nombre del Status");
        //                return response;
        //            }

        //            //if (obdRequest.IdCompany < 0)
        //            //{
        //            //    response.success = false;
        //            //    response.messages.Add("Ingrese la Compañia");
        //            //    return response;
        //            //}

        //            if (obdRequest.IdType <= 0)
        //            {
        //                response.success = false;
        //                response.messages.Add("Ingrese el El Tipo");
        //                return response;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            response.success = false;
        //            response.messages.Add(ex.Message);
        //            return response;
        //        }

        //        if (!string.IsNullOrEmpty(obdRequest.IdDevice.Trim()))
        //        {
        //            useful = new CredencialSpiderFleet.Models.Useful.UseFul();

        //            if (useful.hasSpecialChar(obdRequest.IdDevice.Trim()))
        //            {
        //                response.success = false;
        //                response.messages.Add("La cadena contiene caracteres especiales");
        //                return response;
        //            }

        //            if (!useful.IsValidLength(obdRequest.IdDevice.Trim(), longitudDevice))
        //            {
        //                response.success = false;
        //                response.messages.Add("La longitud excede de lo establecido rango maximo " + longitudDevice + " caracteres");
        //                return response;
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(obdRequest.Label.Trim()))
        //        {
        //            useful = new CredencialSpiderFleet.Models.Useful.UseFul();

        //            if (useful.hasSpecialChar(obdRequest.Label.Trim()))
        //            {
        //                response.success = false;
        //                response.messages.Add("La cadena contiene caracteres especiales");
        //                return response;
        //            }

        //            if (!useful.IsValidLength(obdRequest.Label.Trim(), longitudLabel))
        //            {
        //                response.success = false;
        //                response.messages.Add("La longitud excede de lo establecido rango maximo " + longitudLabel + " caracteres");
        //                return response;
        //            }
        //        }

        //        CredencialSpiderFleet.Models.Inventory.Obd.Obd obd = new CredencialSpiderFleet.Models.Inventory.Obd.Obd();
        //        obd.IdDevice = obdRequest.IdDevice;
        //        obd.Label = obdRequest.Label;
        //        obd.IdCompany = (int?)obdRequest.IdCompany;
        //        obd.IdType = obdRequest.IdType;
        //        obd.IdSim = (int?)obdRequest.IdSim;

        //        try
        //        {
        //            int respuesta = (new ObdDao()).Create(obd);
        //            if (respuesta == 3)
        //            {
        //                response.success = false;
        //                response.messages.Add("Error al intenar dar de alta el registro");
        //                return response;
        //            }
        //            else if (respuesta == 1)
        //            {
        //                response.success = true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            response.success = false;
        //            response.messages.Add(ex.Message);
        //            return response;
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.success = false;
        //        response.messages.Add(ex.Message);
        //        return response;
        //    }
        //}

        /// <summary>
        /// Actualizacion de Obds
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de los Obds
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla Obds
        /// ```
        /// {
        /// "iddevice" : "123456789015",
        /// "label" : "123456789015",
        /// "idcompany" : 1,
        /// "idtype" : 2,
        /// "idsim" : 1
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "iddevice" : "string",
        /// "label" : "string",
        /// "idcompany" : "int",
        /// "idtype" : "int",
        /// "idsim" : "int"
        /// }
        /// ```
        /// </example>
        /// <param name="obdRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Obds estructura similar a la tabla Obds</returns>
        //[HttpPut]
        //[Route(BasicRoute + ResourceName)]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ObdResponse))]
        //public ObdResponse Update([FromBody] ObdRequest obdRequest)
        //{
        //    ObdResponse response = new ObdResponse();
        //    try
        //    {
        //        if (!(obdRequest is ObdRequest))
        //        {
        //            response.success = false;
        //            response.messages.Add("Objeto de entrada invalido");
        //            return response;
        //        }

        //        try
        //        {
        //            if (string.IsNullOrEmpty(obdRequest.IdDevice.Trim()))
        //            {
        //                response.success = false;
        //                response.messages.Add("Ingrese el numero de Dispositivo");
        //                return response;
        //            }


        //            if (string.IsNullOrEmpty(obdRequest.Label.Trim()))
        //            {
        //                response.success = false;
        //                response.messages.Add("Ingrese el Nombre del Status");
        //                return response;
        //            }

        //            //if (obdRequest.IdCompany < 0)
        //            //{
        //            //    response.success = false;
        //            //    response.messages.Add("Ingrese el la Compañia");
        //            //    return response;
        //            //}

        //            if (obdRequest.IdType <= 0)
        //            {
        //                response.success = false;
        //                response.messages.Add("Ingrese el El Tipo");
        //                return response;
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            response.success = false;
        //            response.messages.Add(ex.Message);
        //            return response;
        //        }

        //        if (!string.IsNullOrEmpty(obdRequest.IdDevice.Trim()))
        //        {
        //            useful = new CredencialSpiderFleet.Models.Useful.UseFul();

        //            if (useful.hasSpecialChar(obdRequest.IdDevice.Trim()))
        //            {
        //                response.success = false;
        //                response.messages.Add("La cadena contiene caracteres especiales");
        //                return response;
        //            }

        //            if (!useful.IsValidLength(obdRequest.IdDevice.Trim(), longitudDevice))
        //            {
        //                response.success = false;
        //                response.messages.Add("La longitud excede de lo establecido rango maximo " + longitudDevice + " caracteres");
        //                return response;
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(obdRequest.Label.Trim()))
        //        {
        //            useful = new CredencialSpiderFleet.Models.Useful.UseFul();

        //            if (useful.hasSpecialChar(obdRequest.Label.Trim()))
        //            {
        //                response.success = false;
        //                response.messages.Add("La cadena contiene caracteres especiales");
        //                return response;
        //            }

        //            if (!useful.IsValidLength(obdRequest.Label.Trim(), longitudLabel))
        //            {
        //                response.success = false;
        //                response.messages.Add("La longitud excede de lo establecido rango maximo " + longitudLabel + " caracteres");
        //                return response;
        //            }
        //        }

        //        CredencialSpiderFleet.Models.Inventory.Obd.Obd obd = new CredencialSpiderFleet.Models.Inventory.Obd.Obd();
        //        obd.IdDevice = obdRequest.IdDevice;
        //        obd.Label = obdRequest.Label;
        //        obd.IdCompany = (int?)obdRequest.IdCompany;
        //        obd.IdType = obdRequest.IdType;
        //        obd.IdSim = (int?)obdRequest.IdSim;

        //        try
        //        {
        //            int respuesta = (new ObdDao()).Update(obd);

        //            if (respuesta == 1)
        //            {
        //                response.success = true;
        //            }
        //            else if (respuesta == 2)
        //            {
        //                response.success = false;
        //                response.messages.Add("No se encuentra el registro");
        //                return response;
        //            }
        //            else if (respuesta == 3)
        //            {
        //                response.success = false;
        //                response.messages.Add("Error al tratar de actualizar el registro");
        //                return response;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            response.success = false;
        //            response.messages.Add(ex.Message);
        //            return response;
        //        }

        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        response.success = false;
        //        response.messages.Add(ex.Message);
        //        return response;
        //    }
        //}

        /// <summary>
        /// Obtiene todos los registros de Obds
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla Obds
        /// <returns>Es una lista de Obds</returns>
        //[HttpGet]
        //[Route(BasicRoute + ResourceName + "/list")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ObdResponse))]
        //public ObdListResponse GetListAsync()
        //{
        //    ObdListResponse response = new ObdListResponse();
        //    try
        //    {
        //        try
        //        {
        //            response.listObds = (new ObdDao()).Read();
        //            response.success = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            response.success = false;
        //            response.messages.Add(ex.Message);
        //            return response;
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.success = false;
        //        response.messages.Add(ex.Message);
        //        return response;
        //    }
        //}

        /// <summary>
        /// Obtiene un registro de Obds
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla Obds
        /// <returns>Es un objeto de Obd </returns>
        //[HttpGet]
        //[Route(BasicRoute + ResourceName)]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ObdRegistryResponse))]
        //public ObdRegistryResponse GetListIdAsync([FromUri(Name = "id")] string id)
        //{
        //    ObdRegistryResponse response = new ObdRegistryResponse();
        //    try
        //    {
        //        try
        //        {
        //            response.obd = new ObdDao().ReadId(id);
        //            response.success = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            response.success = false;
        //            response.messages.Add(ex.Message);
        //            return response;
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.success = false;
        //        response.messages.Add(ex.Message);
        //        return response;
        //    }
        //}
   
    }
}


