using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo;
using SpiderFleetWebAPI.Models.Request.Itinearios;
using SpiderFleetWebAPI.Models.Response.Itinerarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Itinerarios
{
    public class ItinerariosUtils
    {
        private MongoDBContext mongoDBContext = new MongoDBContext();

        public ItinerariosListResponse GetItinerariosList(ItinerariosListRequest itinerariosListadoRequest)
        {
            ItinerariosListResponse itinerariosListadoResponse = new ItinerariosListResponse();

            try
            {
                var idVehiculo = itinerariosListadoRequest.idVehiculo;
                var dateInit = itinerariosListadoRequest.dateInit;
                var dateEnd = itinerariosListadoRequest.dateEnd;

                var buildTrips = Builders<Trips>.Filter;
                var filterTripsIdVehiculo = buildTrips.Eq("device", idVehiculo);
                var filterTripsDateInit = buildTrips.Gte("startDate", dateInit);
                var filterTripsDateEnd = buildTrips.Lte("endDate", dateEnd);

                var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<Trips>("Trips");
                List<Trips> result = StoredTripData.Find(
                    filterTripsIdVehiculo
                    & filterTripsDateInit
                    & filterTripsDateEnd).Sort("{startDate: 1}").ToList();

                itinerariosListadoResponse.storedTrips = result;
                itinerariosListadoResponse.messages.Add("Viajes encontrados: " + result.Count);
                itinerariosListadoResponse.success = true;

            }
            catch (Exception ex)
            {
                itinerariosListadoResponse.success = false;
                itinerariosListadoResponse.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                itinerariosListadoResponse.messages.Add(ex.Message);
            }

            return itinerariosListadoResponse;
        }

        public ItinerariosListResponse GetItinerariosRuta(ItinerariosListRequest itinerariosListadoRequest)
        {
            ItinerariosListResponse itinerariosListResponse = new ItinerariosListResponse();



            return itinerariosListResponse;
        }
    }
}