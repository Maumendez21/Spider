using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo;
using SpiderFleetWebAPI.Models.Request.Status;
using SpiderFleetWebAPI.Models.Response.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Status
{
    public class StatusUtils
    {
        private MongoDBContext mongoDBContext = new MongoDBContext();
        public StatusFleetResponse GetStatusFlotilla(StatusFleetRequest statusFleetRequest)
        {
            StatusFleetResponse statusFleetResponse = new StatusFleetResponse();

            try
            {
                var id_empresa = statusFleetRequest.idEmpresa;
                var infoDevicesData = mongoDBContext.spiderMongoDatabase.GetCollection<InfoDevices>("InfoDevices");
                List<InfoDevices> result = infoDevicesData.AsQueryable<InfoDevices>().ToList();

                statusFleetResponse.locations = result;
                statusFleetResponse.messages.Add("Resultados encontrados: " + result.Count);
                statusFleetResponse.success = true;
            }
            catch (Exception ex)
            {
                statusFleetResponse.success = false;
                statusFleetResponse.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                statusFleetResponse.messages.Add(ex.Message);
            }

            return statusFleetResponse;
        }

        public StatusVehiculoResponse GetStatusVehiculo(StatusVehiculoRequest statusVehiculoRequest)
        {
            StatusVehiculoResponse statusVehiculoResponse = new StatusVehiculoResponse();

            try
            {
                var id_vehiculo = statusVehiculoRequest.idVehiculo;
                var infoDevicesData = mongoDBContext.spiderMongoDatabase.GetCollection<InfoDevices>("InfoDevices");
                InfoDevices result = infoDevicesData.AsQueryable<InfoDevices>().SingleOrDefault(x => x._id == statusVehiculoRequest.idVehiculo);

                statusVehiculoResponse.location = result;
                statusVehiculoResponse.messages.Add("Vehiculo encontrado: " + result.name);
                statusVehiculoResponse.success = true;
            }
            catch (Exception ex)
            {
                statusVehiculoResponse.success = false;
                statusVehiculoResponse.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                statusVehiculoResponse.messages.Add(ex.Message);
            }

            return statusVehiculoResponse;
        }
    }
}