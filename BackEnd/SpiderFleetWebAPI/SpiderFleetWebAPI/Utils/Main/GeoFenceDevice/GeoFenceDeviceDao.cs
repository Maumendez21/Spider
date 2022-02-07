using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.GeoFence;
using SpiderFleetWebAPI.Models.Response.Main.GeoFenceDevice;
using SpiderFleetWebAPI.Utils.Main.GeoFence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SpiderFleetWebAPI.Utils.Main.GeoFenceDevice
{
    public class GeoFenceDeviceDao
    {
        private MongoDBContext mongoDBContext = new MongoDBContext();
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public GeoFenceDeviceDao() { }

        public GeoFenceDeviceResponse Create(CredencialSpiderFleet.Models.Main.GeoFenceDevice.GeoFenceDevice geoFence)
        {
            GeoFenceDeviceResponse response = new GeoFenceDeviceResponse();
            Dictionary<string, string> resultados = new Dictionary<string, string>();
            try
            {
                foreach (string device in geoFence.ListDevice)
                {
                    var build = Builders<Models.Mongo.GeoFenceDevice.GeoFenceDevice>.Filter.And(
                            Builders<Models.Mongo.GeoFenceDevice.GeoFenceDevice>.Filter.Eq(x => x.Device, device)
                            & Builders<Models.Mongo.GeoFenceDevice.GeoFenceDevice>.Filter.Eq(x => x.IdGeoFence, geoFence.IdGeoFence)
                        );
                    var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFenceDevice.GeoFenceDevice>("GeoFenceDevice");
                    var resultEquals = stored.Find(build).ToList();

                    if (resultEquals.Count > 0)
                    {
                        resultados.Add(device, "La combinacion de Geo Cerca y dispositivo ya existe");
                    }
                    else
                    {

                        var buildGeo = Builders<Models.Mongo.GeoFence.GeoFence>.Filter.Eq(x => x.Id, geoFence.IdGeoFence);
                        var storedGeo = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence");
                        var resultGeo = storedGeo.Find(buildGeo).FirstOrDefault();

                        if(resultGeo != null)
                        {
                            Models.Mongo.GeoFenceDevice.GeoFenceDevice geo = new Models.Mongo.GeoFenceDevice.GeoFenceDevice();
                            geo.IdGeoFence = geoFence.IdGeoFence;
                            geo.Device = device;
                            mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFenceDevice.GeoFenceDevice>("GeoFenceDevice").InsertOne(geo);

                            var buildCon = Builders<GeoFenceConsola>.Filter.Eq(x => x.Device, device);
                            var storedCon = mongoDBContext.spiderMongoDatabase.GetCollection<GeoFenceConsola>("GeoFenceConsola");
                            var resultCon = storedCon.Find(buildCon).FirstOrDefault();

                            GeoFenceConsolas fence = new GeoFenceConsolas();
                            fence.Id = resultGeo.Id;
                            fence.Name = resultGeo.Name;
                            fence.Description = resultGeo.Description;
                            fence.Active = resultGeo.Active;
                            fence.Polygon = resultGeo.Polygon;

                            if (resultCon != null)
                            {
                                //int valida = resultCon.Fences.Where(x => x.Id.Equals(geoFence.IdGeoFence)).Count();

                                GeoFenceConsola consola = new GeoFenceConsola();
                                consola.Device = device;

                                //if (valida == 0)
                                //{
                                    var filter = Builders<GeoFenceConsola>.Filter.Eq(x => x.Device, device);
                                    resultCon.Fences.Add(fence);
                                    mongoDBContext.spiderMongoDatabase.GetCollection<GeoFenceConsola>("GeoFenceConsola").ReplaceOneAsync(filter, resultCon);
                                //}
                                //else
                                //{
                                //    resultCon.Fences.Where(x => x.Id.Equals(geoFence.IdGeoFence)).Count();
                                //}
                            }
                            else
                            {
                                GeoFenceConsola consola = new GeoFenceConsola();
                                consola.Fences = new List<GeoFenceConsolas>();
                                consola.Device = device;
                                consola.Fences.Add(fence);
                                mongoDBContext.spiderMongoDatabase.GetCollection<GeoFenceConsola>("GeoFenceConsola").InsertOne(consola);
                            }
                        }
                        else
                        {
                            resultados.Add(device, "No se encuentra la Geo Cerca seleccionada");
                        }
                    }
                }

                if(resultados.Count == 0)
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

        public GeoFenceDeviceListIdResponse Delete(CredencialSpiderFleet.Models.Main.GeoFenceDevice.GeoFenceDeviceId geoFence)
        {
            GeoFenceDeviceListIdResponse response = new GeoFenceDeviceListIdResponse();
            Dictionary<string, string> resultados = new Dictionary<string, string>();
            try
            {
                foreach (string data in geoFence.ListDeviceId)
                {
                    var build = Builders<Models.Mongo.GeoFenceDevice.GeoFenceDevice>.Filter.Eq(x => x.Id, data);
                    var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFenceDevice.GeoFenceDevice>("GeoFenceDevice");
                    var result = stored.Find(build).FirstOrDefault();

                    if (result != null)
                    {
                        mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFenceDevice.GeoFenceDevice>("GeoFenceDevice").DeleteOne(x => x.Id == data);

                        if(result != null)
                        {
                            var filterConsola = Builders<GeoFenceConsola>.Filter.Eq(x => x.Device, result.Device);
                            var storedConsola = mongoDBContext.spiderMongoDatabase.GetCollection<GeoFenceConsola>("GeoFenceConsola");
                            var resultConsola = storedConsola.Find(filterConsola).FirstOrDefault();

                            GeoFenceConsola consola = new GeoFenceConsola();
                            consola.Id = resultConsola.Id;
                            consola.Device = resultConsola.Device;
                            List<GeoFenceConsolas> Fences = new List<GeoFenceConsolas>();

                            foreach(var geo in resultConsola.Fences)
                            {
                                if(!geo.Id.Equals(result.IdGeoFence))
                                {
                                    Fences.Add(geo);
                                }
                            }

                            if (Fences.Count > 0)
                            {
                                consola.Fences = Fences;
                            }
                            else
                            {
                                consola.Fences = null;
                            }
                            var filter = Builders<GeoFenceConsola>.Filter.Eq(x => x.Device, result.Device);                               
                            mongoDBContext.spiderMongoDatabase.GetCollection<GeoFenceConsola>("GeoFenceConsola").ReplaceOneAsync(filter, consola);
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

        public GeoFenceDeviceListResponse Read(string hierarchy)
        {
            GeoFenceDeviceListResponse response = new GeoFenceDeviceListResponse();

            GeoFenceList fence = new GeoFenceList();
            List<GeoFenceDeviceList> listGeoFence = new List<GeoFenceDeviceList>();

            try
            {
                try
                {

                    List<string> listIds = new List<string>();
                    listIds = (new GeoFenceDao()).GetMongoId(hierarchy);

                    var bsonArray = new BsonArray();
                    foreach (var id in listIds)
                    {
                        bsonArray.Add(ObjectId.Parse(id));
                    }

                    BsonDocument bsonDocument = new BsonDocument(new BsonDocument("_id", new BsonDocument("$in", bsonArray)));
                    var build = bsonDocument;
                    var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence");
                    var result = stored.Find(build).ToList();

                    //var build = Builders<Models.Mongo.GeoFence.GeoFence>.Filter.Regex(x => x.Hierarchy, ($"/{hierarchy}/i"));
                    //var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence");
                    //var result = stored.Find(build).ToList();

                    if (result == null)
                    {
                        response.fence = fence;
                        response.success = false;
                        return response;
                    }
                    else
                    {
                        foreach (Models.Mongo.GeoFence.GeoFence data in result)
                        {
                            List<CoordenatesData> listCoordenates = new List<CoordenatesData>();
                            GeoFenceDeviceList geo = new GeoFenceDeviceList();
                            geo.Id = data.Id;
                            geo.Name = data.Name;
                            geo.Hierarchy = data.Hierarchy;

                            List<List<double>> Coordinates = data.Polygon.Coordinates[0];
                            foreach (List<double> coor in Coordinates)
                            {
                                CoordenatesData coordenates = new CoordenatesData();
                                coordenates.lng = coor[0];
                                coordenates.lat = coor[1];

                                listCoordenates.Add(coordenates);
                            }

                            Polygons polygons = new Polygons();
                            polygons.Type = data.Polygon.Type;
                            polygons.Coordinates = listCoordenates;
                            geo.Polygon = polygons;
                            listGeoFence.Add(geo);
                        }
                        fence.GeoFence = listGeoFence;
                    }
                }
                catch(Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                try
                {
                    if(listGeoFence.Count > 0)
                    {
                        GeoFenceDeviceRegistry geoFence = new GeoFenceDeviceRegistry();
                        List<GeoFenceDeviceRegistry> ListDevice = new List<GeoFenceDeviceRegistry>();
                        foreach (GeoFenceDeviceList data in listGeoFence)
                        {
                            var buildX = Builders<Models.Mongo.GeoFenceDevice.GeoFenceDevice>.Filter.Eq(x => x.IdGeoFence, data.Id);
                            var storedx = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFenceDevice.GeoFenceDevice>("GeoFenceDevice");
                            var resultEquals = storedx.Find(buildX).ToList();

                            if (resultEquals.Count > 0)
                            {
                                foreach (Models.Mongo.GeoFenceDevice.GeoFenceDevice device in resultEquals)
                                {
                                    geoFence = new GeoFenceDeviceRegistry();
                                    geoFence.Id = device.Id;
                                    geoFence.Device = device.Device;
                                    geoFence.Name = NameDevice(device.Device);
                                    ListDevice.Add(geoFence);
                                }
                                data.ListDevice = ListDevice;
                                ListDevice = new List<GeoFenceDeviceRegistry>();
                            }
                        }
                        response.success = true;
                        response.fence = fence;
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


        /// <summary>
        /// Consulta de Usuarios con estatus 1
        /// </summary>
        public string NameDevice(string device)
        {
            string name = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_name_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            name = Convert.ToString(reader["Nombre"]);
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }
            return name;
        }

    }
}