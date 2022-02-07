using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Route
{
    public class RouteAnalysisDao
    {
        private MongoDBContext mongoDBContext = new MongoDBContext();

        static public async Task CheckGeoFence(string id, double latitud, double longitud, DateTime fecha)
        {
            List<string> GeocercasIDs = new List<string>();
            List<string> GeocercasIDsIN = new List<string>();


            BsonDocument bsonDocument = new BsonDocument(new BsonDocument("_id", new BsonDocument("$in", bsonArray)));
            var build = bsonDocument;
            var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.Route>("Route");
            var result = stored.Find(build).ToList();


            IMongoDatabase db = clienteMongo.GetDatabase("SPIDER");

            var collectionGeoFences = db.GetCollection<BsonDocument>("Route");
            var filterDeviceGeoFences = new BsonDocument { { "device", device } };


            var filter = FindPosition(device, longitud, latitud);
            var pro = new BsonDocument {
                    //{ "_id", false},
                    {"fences._id", true },
                    {"trace.name", true },
                    //{"trace.active", true }
                };
            var resultado = await stored.Aggregate()
                    .Unwind("fences")
                    .Match(filter)
                    .Project(pro).ToListAsync();

            if (resultado != null)
            {
                foreach (var r in resultado)
                {
                    GeocercasIDsIN.Add(r["fences"]["_id"].ToString());
                }
            }


            string send_result_sql = string.Empty;
            //match geocercas
            foreach (string id_geo in GeocercasIDs)
            {
                send_result_sql += id_geo + (GeocercasIDsIN.Exists(item => item == id_geo) ? "1" : "0") + "|";
            }

            Console.WriteLine(send_result_sql);
            WriteGeoFenceInformation(device, latitud + "," + longitud, send_result_sql, fecha);
        }


        static private BsonDocument FindPosition(string id, double longitude, double latitude)
        {
            BsonDocument bsonDocument = new BsonDocument();
            bsonDocument.Add("_id", ObjectId.Parse(id));
            bsonDocument.Add("trace.polygon", new BsonDocument("$geoIntersects", new BsonDocument("$geometry", new BsonDocument
                {
                    { "type", "Point" },
                    { "coordinates", new BsonArray
                    {
                        longitude,
                        latitude
                    } }
                })));
            return bsonDocument;
        }
    }
}