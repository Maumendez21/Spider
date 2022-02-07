using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.PointInterest;
using SpiderFleetWebAPI.Models.Response.PointsInterest;
using SpiderFleetWebAPI.Utils.Main.GeoFenceDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.PointInterest
{
    public class PointInterestDeviceDao
    {
        private MongoDBContext mongoDBContext = new MongoDBContext();

        public PointInterestDeviceDao() { }

        public PointInterestDeviceResponse Create(CredencialSpiderFleet.Models.PointInterest.PointInterestDevice point)
        {
            PointInterestDeviceResponse response = new PointInterestDeviceResponse();
            Dictionary<string, string> resultados = new Dictionary<string, string>();
            try
            {
                foreach (string device in point.ListDevice)
                {
                    var build = Builders<PointsInterestDevice>.Filter.And(
                            Builders<PointsInterestDevice>.Filter.Eq(x => x.Device, device)
                            & Builders<PointsInterestDevice>.Filter.Eq(x => x.IdPoint, point.IdPointInterest)
                        );
                    var stored = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestDevice>("PointsInterestDevice");
                    var resultEquals = stored.Find(build).ToList();

                    if (resultEquals.Count > 0)
                    {
                        resultados.Add(device, "La combinacion del Punto de Interes y el dispositivo ya existe");
                    }
                    else
                    {

                        var buildGeo = Builders<PointsInterest>.Filter.Eq(x => x.Id, point.IdPointInterest);
                        var storedGeo = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterest>("PointsInterest");
                        var resultGeo = storedGeo.Find(buildGeo).FirstOrDefault();

                        if (resultGeo != null)
                        {
                            PointsInterestDevice geo = new PointsInterestDevice();
                            geo.IdPoint = point.IdPointInterest;
                            geo.Device = device;
                            mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestDevice>("PointsInterestDevice").InsertOne(geo);

                            var buildCon = Builders<PointsInterestConsola>.Filter.Eq(x => x.Device, device);
                            var storedCon = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestConsola>("PointsInterestConsola");
                            var resultCon = storedCon.Find(buildCon).FirstOrDefault();

                            PointInterestConsola fence = new PointInterestConsola();
                            fence.Id = resultGeo.Id;
                            fence.Name = resultGeo.Name;
                            fence.Description = resultGeo.Description;
                            fence.Active = resultGeo.Active;
                            fence.InterestPoint = resultGeo.InterestPoint;

                            if (resultCon != null)
                            {
                                int countList = resultCon.PointsInterest.Count();

                                PointsInterestConsola consola = new PointsInterestConsola();
                                consola.Device = device;

                                var filter = Builders<PointsInterestConsola>.Filter.Eq(x => x.Device, device);
                                if(countList ==  0)
                                {
                                    resultCon.PointsInterest = new List<PointInterestConsola>();
                                }
                                
                                resultCon.PointsInterest.Add(fence);
                                InterestPoint InterestPoint = new InterestPoint();

                                mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestConsola>("PointsInterestConsola").ReplaceOneAsync(filter, resultCon);
                            }
                            else
                            {
                                PointsInterestConsola consola = new PointsInterestConsola();
                                consola.PointsInterest = new List<PointInterestConsola>();
                                consola.Device = device;
                                consola.PointsInterest.Add(fence);
                                mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestConsola>("PointsInterestConsola").InsertOne(consola);
                            }
                        }
                        else
                        {
                            resultados.Add(device, "No se encuentra en Punto de Interes seleccionada");
                        }
                    }
                }

                if (resultados.Count == 0)
                {
                    response.success = true;
                }
                else if (resultados.Count > 0)
                {
                    response.success = false;
                    response.resultados = resultados;
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

        public PointInsterestDeviceListIdResponse Delete(CredencialSpiderFleet.Models.PointInterest.PointInterestDeviceId point)
        {
            PointInsterestDeviceListIdResponse response = new PointInsterestDeviceListIdResponse();
            Dictionary<string, string> resultados = new Dictionary<string, string>();
            try
            {
                foreach (string data in point.ListDeviceId)
                {
                    var build = Builders<PointsInterestDevice>.Filter.Eq(x => x.Id, data);
                    var stored = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestDevice>("PointsInterestDevice");
                    var result = stored.Find(build).FirstOrDefault();

                    if (result != null)
                    {
                        mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestDevice>("PointsInterestDevice").DeleteOne(x => x.Id == data);

                        if (result != null)
                        {
                            var filterConsola = Builders<PointsInterestConsola>.Filter.Eq(x => x.Device, result.Device);
                            var storedConsola = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestConsola>("PointsInterestConsola");
                            var resultConsola = storedConsola.Find(filterConsola).FirstOrDefault();

                            PointsInterestConsola consola = new PointsInterestConsola();
                            consola.Id = resultConsola.Id;
                            consola.Device = resultConsola.Device;
                            List<PointInterestConsola> Fences = new List<PointInterestConsola>();

                            foreach (var geo in resultConsola.PointsInterest)
                            {
                                if (!geo.Id.Equals(result.IdPoint))
                                {
                                    Fences.Add(geo);
                                }
                            }

                            if (Fences.Count > 0)
                            {
                                consola.PointsInterest = Fences;
                            }
                            else
                            {
                                consola.PointsInterest = Fences;
                            }
                            var filter = Builders<PointsInterestConsola>.Filter.Eq(x => x.Device, result.Device);
                            mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestConsola>("PointsInterestConsola").ReplaceOneAsync(filter, consola);
                        }
                    }
                    else
                    {
                        resultados.Add(data, "No existe la combinacion de Geo Cerca y dispositivo");
                    }
                }

                if (resultados.Count == 0)
                {
                    response.success = true;
                }
                else if (resultados.Count > 0)
                {
                    response.success = false;
                    response.resultados = resultados;
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

        public PointInterestDeviceListResponse Read(string hierarchy)
        {
            PointInterestDeviceListResponse response = new PointInterestDeviceListResponse();

            PointInterestList PointInterest = new PointInterestList();
            List<PointInterestDeviceList> listPointInterest = new List<PointInterestDeviceList>();

            try
            {
                try
                {

                    List<string> ListPoint = new List<string>();
                    ListPoint = (new PointInterestDao()).ListPointsByHierarchy(hierarchy);

                    var bsonArray = new BsonArray();
                    foreach (var id in ListPoint)
                    {
                        bsonArray.Add(ObjectId.Parse(id));
                    }

                    BsonDocument bsonDocument = new BsonDocument(new BsonDocument("_id", new BsonDocument("$in", bsonArray)));
                    var build = bsonDocument;
                    var stored = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterest>("PointsInterest");
                    var result = stored.Find(build).ToList();

                    if (result == null)
                    {
                        response.PointInterest = PointInterest;
                        response.success = false;
                        return response;
                    }
                    else
                    {
                        foreach (var data in result)
                        {
                            List<CoordenatesData> listCoordenates = new List<CoordenatesData>();
                            PointInterestDeviceList point = new PointInterestDeviceList();
                            point.Id = data.Id;
                            point.Name = data.Name;
                            point.Hierarchy = data.Hierarchy;
                            point.Latitude = data.InterestPoint.Coordinate[1].ToString();
                            point.Longitude = data.InterestPoint.Coordinate[0].ToString();
                            point.Radius = data.InterestPoint.Radius;

                            listPointInterest.Add(point);
                        }
                        PointInterest.PointInterest = listPointInterest;
                    }
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                try
                {
                    if (listPointInterest.Count > 0)
                    {
                        PointInterestDeviceRegistry geoFence = new PointInterestDeviceRegistry();
                        List<PointInterestDeviceRegistry> ListDevice = new List<PointInterestDeviceRegistry>();
                        foreach (PointInterestDeviceList data in listPointInterest)
                        {
                            var buildX = Builders<PointsInterestDevice>.Filter.Eq(x => x.IdPoint, data.Id);
                            var storedx = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestDevice>("PointsInterestDevice");
                            var resultEquals = storedx.Find(buildX).ToList();

                            if (resultEquals.Count > 0)
                            {
                                foreach (PointsInterestDevice device in resultEquals)
                                {
                                    geoFence = new PointInterestDeviceRegistry();
                                    geoFence.Id = device.Id;
                                    geoFence.Device = device.Device;
                                    geoFence.Name = (new GeoFenceDeviceDao()).NameDevice(device.Device);
                                    ListDevice.Add(geoFence);
                                }
                                data.ListDevice = ListDevice;
                                ListDevice = new List<PointInterestDeviceRegistry>();
                            }
                        }
                        response.success = true;
                        response.PointInterest = PointInterest;
                    }
                    else
                    {
                        response.success = false;
                    }
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
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

    }
}