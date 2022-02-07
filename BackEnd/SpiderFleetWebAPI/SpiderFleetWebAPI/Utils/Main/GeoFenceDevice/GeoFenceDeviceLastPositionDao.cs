using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.GeoFence;
using SpiderFleetWebAPI.Models.Response.Main.GeoFenceDevice;
using SpiderFleetWebAPI.Utils.Main.GeoFence;
using SpiderFleetWebAPI.Utils.Main.LastPositionDevice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SpiderFleetWebAPI.Utils.Main.GeoFenceDevice
{
    public class GeoFenceDeviceLastPositionDao
    {

        private MongoDBContext mongoDBContext = new MongoDBContext();
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();


        public GeoFenceListResponse ReadGeoFences(string hierarchy)
        {
            GeoFenceListResponse response = new GeoFenceListResponse();

            GeoFenceList fence = new GeoFenceList();
            List<GeoFences> listGeoFence = new List<GeoFences>();


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
                    response.success = false;
                    return response;
                }
                else
                {
                    foreach (Models.Mongo.GeoFence.GeoFence data in result)
                    {
                        List<CoordenatesData> listCoordenates = new List<CoordenatesData>();
                        GeoFences geo = new GeoFences();
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

                    if(listGeoFence.Count > 0)
                    {
                        response.success = true;
                        response.ListGeoFences = listGeoFence;
                    }
                    else
                    {
                        response.success = false;
                    }
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

        public LastStatusDeviceGeoFenceResponse ReadLastPositionDevice(string id)
        {
            LastStatusDeviceGeoFenceResponse response = new LastStatusDeviceGeoFenceResponse();

            GeoFenceDeviceLastPosition position = new GeoFenceDeviceLastPosition();
            List<GeoFenceDeviceLastPosition> ListLastStatusDevice = new List<GeoFenceDeviceLastPosition>();

            try
            {
                
                var build = Builders<Models.Mongo.GeoFenceDevice.GeoFenceDevice>.Filter.Eq(x => x.IdGeoFence, id);
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFenceDevice.GeoFenceDevice>("GeoFenceDevice");
                var resultEquals = stored.Find(build).ToList();

                if (resultEquals.Count > 0)
                {
                    foreach (Models.Mongo.GeoFenceDevice.GeoFenceDevice device in resultEquals)
                    {
                        position = new GeoFenceDeviceLastPosition();
                        position.Device = device.Device;
                        LastPosition(position);
                        ListLastStatusDevice.Add(position);
                    }
                    response.success = true;
                    response.ListLastStatusDevice = ListLastStatusDevice;
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
            return response;
        }


        private void LastPosition(GeoFenceDeviceLastPosition lastPosition)
        {
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_geo_fence_device_last_position", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(lastPosition.Device)));
                    string events = string.Empty;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lastPosition.Name = Convert.ToString(reader["Nombre"]);
                            lastPosition.Date = Convert.ToDateTime(reader["date"]);
                            events = Convert.ToString(reader["event"]);
                            lastPosition.Latitude = Convert.ToString(reader["latitude"]);
                            lastPosition.Longitude = Convert.ToString(reader["longitude"]);
                        }
                        reader.Close();
                    }
                    ReadCurrentPositionDevices(lastPosition.Device, lastPosition);
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
        }

        private void ReadCurrentPositionDevices(string device, GeoFenceDeviceLastPosition lastPosition)
        {

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_last_position_device_spider_geo_fence", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@devices", Convert.ToString(device)));
                    string events = string.Empty;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lastPosition.StatusEvent = Convert.ToInt32(reader["status_event"].ToString());                            
                        }
                        reader.Close();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private void ReadCurrentPositionDevices(GeoFenceDeviceLastPosition lastPosition, string events)
        {

            try
            {
                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
                int secs = (int)cstTime.Subtract(Convert.ToDateTime(lastPosition.Date)).TotalSeconds;

                if (events.Equals("GPS"))
                {
                    if (secs <= 300)//5 Minutos
                    {
                        lastPosition.StatusEvent = 1;
                    }
                    else if (secs > 300 & secs <= 3600) //Menor a 1 Hora
                    {
                        string evento = (new LastPositionDevicesDao()).LastStatusAlarm(lastPosition.Device);
                        if (evento.Equals("Ignition Off"))
                        {
                            lastPosition.StatusEvent = 2;
                        }
                        else
                        {
                            lastPosition.StatusEvent = 5;
                        }

                    }
                    else if (secs > 3600) //Mayor a 1 Hora
                    {
                        string evento = (new LastPositionDevicesDao()).LastStatusAlarm(lastPosition.Device);
                        if (evento.Equals("Ignition Off"))
                        {
                            lastPosition.StatusEvent = 2;
                        }
                        else
                        {
                            lastPosition.StatusEvent = 3;
                        }
                    }
                }
                else if (events.Equals("Sleep"))
                {
                    if (secs < 3600)
                    {
                        lastPosition.StatusEvent = 2;
                    }
                    else if (secs > 3600 & secs < 4200) //Mayor a 1 hora y Menor a 1 hora 10 minutos
                    {
                        lastPosition.StatusEvent = 2;
                    }
                    else if (secs > 4200 & secs < 86400) // 3 Horas 10800
                    {
                        lastPosition.StatusEvent = 3;
                    }
                    else if (secs > 86400) // 24 horas 86400
                    {
                        lastPosition.StatusEvent = 4;
                    }
                }
                else
                {
                    lastPosition.StatusEvent = 2;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}