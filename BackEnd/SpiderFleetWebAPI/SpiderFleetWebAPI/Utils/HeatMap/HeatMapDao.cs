using CredencialSpiderFleet.Models.HeatMap;
using CredencialSpiderFleet.Models.Useful;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.GPS;
using SpiderFleetWebAPI.Models.Response.HeatMap;
using SpiderFleetWebAPI.Models.Response.Obd;
using SpiderFleetWebAPI.Utils.General;
using SpiderFleetWebAPI.Utils.Obd;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiderFleetWebAPI.Utils.HeatMap
{
    public class HeatMapDao
    {

        private MongoDBContext mongoDBContext = new MongoDBContext();
        private UseFul use = new UseFul();


        /// <summary>
        /// Metodo que devuelve la lista de coordenadas para el mapa de Calor
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="group"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public HeatMapResponse Coordenates(string hierarchy, string group, DateTime startdate, DateTime enddate, string device)
        {
            HeatMapResponse response = new HeatMapResponse();

            int horas = VerifyUser.VerifyUser.GetHours();
            enddate = enddate.AddHours(horas);
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(enddate, zone);

            enddate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            startdate = startdate.AddHours(horas);
            enddate = enddate.AddHours(horas);

            try
            {
                int value = Convert.ToInt32((new GeneralDao()).ReadIdHerarchy(use.hierarchyPrincipalToken(hierarchy), "MC", 2));

                if (startdate > enddate)
                {
                    response.success = false;
                    response.messages.Add("La fecha de Inicio es mayor que la fecha final, favor de verificar los datos.");
                    return response;
                }
                
                int months = UseFul.MonthDiff(startdate, enddate);

                if(months >= value)
                {
                    response.success = false;
                    response.messages.Add("La fecha de inicio ha excedido el parametro de consulta, favor de contactar a su Administrador.");
                    return response;
                }

                List<Coordinates> listCoordinates = new List<Coordinates>();
                List<string> listDevice = new List<string>();

                if(!string.IsNullOrEmpty(device))
                {
                    listDevice.Add(device);
                }
                else
                {
                    listDevice = ListDevice(group);
                }                

                listCoordinates = GetDataProcessCoordinates(listDevice, startdate, enddate);

                if(listCoordinates.Count > 0)
                {
                    response.Coords = listCoordinates;
                    response.success = true;
                }
                else
                {
                    response.Coords = listCoordinates;
                    response.success = false;
                    response.messages.Add("No se encontraron resultados");
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
        /// Metodo que regresa los dispositivos dependiendo del grupo
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private List<string> ListDevice(string group)
        {
            ListObdResponse response = new ListObdResponse();
            List<string> listDevice = new List<string>();

            try
            {
                response = (new SubCompanyAssignmentObdsDao()).ListDeviceHierarchy(group);
                if(response.listObd.Count > 0)
                {
                    foreach(var device in response.listObd)
                    {
                        listDevice.Add(device.Device);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listDevice;
        }

        /// <summary>
        /// Metodo que regresa la lista de coordenadas por rango de fechas y dispositivo o dispositivos
        /// </summary>
        /// <param name="listDevice"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        private List<Coordinates> GetDataProcessCoordinates(List<string> listDevice ,DateTime startdate, DateTime enddate)
        {

            try
            {
                List<Coordinates> listCoords = new List<Coordinates>();
                Coordinates coords = new Coordinates();
                //string start = "2020-10-28T06:00:00Z";// startdate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                //string end = "2020-10-29T05:59:59Z";// enddate.ToString("yyyy-MM-ddTHH:mm:ssZ");

                string start = startdate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string end = enddate.ToString("yyyy-MM-ddTHH:mm:ssZ");

                var bsonArray = new BsonArray();
                foreach (var device in listDevice)
                {
                    bsonArray.Add(device);
                }

                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("device", new BsonDocument("$in", bsonArray )));
                bsonDocument.Add("date", new BsonDocument("$gte", Convert.ToDateTime(start)).Add("$lte", Convert.ToDateTime(end)));
                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<GPS>("GPS");
                var result = stored.Find(build).Sort("{date:1}").ToList();

                if (result.Count > 0)
                {
                    foreach (GPS data in result)
                    {
                        coords = new Coordinates();
                        coords.Longitude = data.Location.Coordinates[0].ToString();
                        coords.Latitude= data.Location.Coordinates[1].ToString();
                        listCoords.Add(coords);
                    }
                }
                return listCoords;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}