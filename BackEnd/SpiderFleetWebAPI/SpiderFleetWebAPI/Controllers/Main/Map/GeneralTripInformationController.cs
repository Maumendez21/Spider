using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Request.Main.Map;
using SpiderFleetWebAPI.Models.Request.User;
using SpiderFleetWebAPI.Models.Response.Main.Map;
using SpiderFleetWebAPI.Utils.Main.Map;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Main.Map
{
    public class GeneralTripInformationController : ApiController
    {
        private const string Tag = "Datos Generales de dispositivo de los Viajes";
        private const string BasicRoute = "api/";
        private const string ResourceName = "general/trip/information";
        private UseFul use = new UseFul();

        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GeneralTripsResponse))]
        public GeneralTripsResponse Create(
            //[FromUri(Name = "device")] string device,
            //[FromUri(Name = "fecha_inicio")] DateTime fecha_inicio,
            //[FromUri(Name = "fecha_fin")] DateTime fecha_fin)
            [FromBody] GeneralTripsRequest tripRequest)
        {
            GeneralTripsResponse response = new GeneralTripsResponse();

            try
            {
                if (!(tripRequest is GeneralTripsRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {

                    if (string.IsNullOrEmpty(tripRequest.IdDevicce))
                    {
                        response.success = false;
                        response.messages.Add("Ingrese el numero de Dispositivo");
                        return response;
                    }

                    if (string.IsNullOrEmpty(tripRequest.fecha_inicio.ToString()))
                    {
                        response.success = false;
                        response.messages.Add("Ingrese la fecha de inicio");
                        return response;
                    }

                    if (string.IsNullOrEmpty(tripRequest.fecha_fin.ToString()))
                    {
                        response.success = false;
                        response.messages.Add("Ingrese la fecha final");
                        return response;
                    }

                    //if (string.IsNullOrEmpty(device))
                    //{
                    //    response.success = false;
                    //    response.messages.Add("Ingrese el numero de Dispositivo");
                    //    return response;
                    //}

                    //if (string.IsNullOrEmpty(fecha_inicio.ToString()))
                    //{
                    //    response.success = false;
                    //    response.messages.Add("Ingrese la fecha de inicio");
                    //    return response;
                    //}

                    //if (string.IsNullOrEmpty(fecha_fin.ToString()))
                    //{
                    //    response.success = false;
                    //    response.messages.Add("Ingrese la fecha final");
                    //    return response;
                    //}

                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                int compare = UseFul.compare(tripRequest.fecha_inicio, tripRequest.fecha_fin);
                //int compare = use.compare(fecha_inicio, fecha_fin);

                if (compare > 0)
                {
                    //estado = false;//la fechaInicio es mayor a la fecha final
                    response.success = false;
                    response.messages.Add("La Fecha Inicio es mayor a la Fecha Final");
                    return response;
                }

                CredencialSpiderFleet.Models.Main.Map.GeneralTrips user = new CredencialSpiderFleet.Models.Main.Map.GeneralTrips();
                user.IdDevice = tripRequest.IdDevicce;
                user.begin_date = use.formatddMMyyyyToyyyyMMdd(tripRequest.fecha_inicio);
                user.end_date = use.formatddMMyyyyToyyyyMMdd(tripRequest.fecha_fin);

                //user.IdDevice = device;
                //user.begin_date = use.formatddMMyyyyToyyyyMMdd(fecha_inicio);
                //user.end_date = use.formatddMMyyyyToyyyyMMdd(fecha_fin);

                try
                {
                    response = (new GeneralTripInformationDao()).Read(user);
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
