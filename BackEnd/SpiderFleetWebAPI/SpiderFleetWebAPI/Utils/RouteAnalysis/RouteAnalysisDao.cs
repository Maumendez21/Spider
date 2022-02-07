
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Response.RouteAnalysis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpiderFleetWebAPI.Utils.RouteAnalysis
{
    public class RouteAnalysisDao
    {
        private MongoDBContext mongoDBContext = new MongoDBContext();

        public void Analysis(string id)
        {
            

        }

        public RouteAnalysisResponse CheckGeoFence(CredencialSpiderFleet.Models.RouteAnalysis.RouteAnalysis analysis)
        //public async Task CheckGeoFence(string id, double latitud, double longitud)
        {
            RouteAnalysisResponse response = new RouteAnalysisResponse();
            try
            {
                List<string> GeocercasIDs = new List<string>();
                List<string> GeocercasIDsIN = new List<string>();

                BsonDocument bsonDocument = new BsonDocument { { "_id", ObjectId.Parse(analysis.Id) } };
                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.Route>("Route");
                var result = stored.Find(build).FirstOrDefault();

                if (result != null)
                {

                    //foreach(var data in result.Trace)
                    //{
                    //    GeocercasIDs.Add(data.Id);
                    //}
                }

                foreach(var data in analysis.Coordinates)
                {
                    var filter = FindPosition(analysis.Id, data[0], data[1]);
                    var pro = new BsonDocument {
                    { "_id", false},
                    //{"trace._id", true }
                    //{"trace.name", true },
                    //{"trace.active", true }
                    };
                    var resultado = stored.Aggregate()
                            .Unwind("trace")
                            .Match(filter)
                            .Project(pro).ToList();

                    if (resultado != null)
                    {
                        foreach (var r in resultado)
                        {
                            GeocercasIDsIN.Add(r["trace"]["_id"].ToString());
                        }
                    }
                }



                string send_result_sql = string.Empty;
                //match geocercas
                foreach (string id_geo in GeocercasIDs)
                {
                    send_result_sql += id_geo + (GeocercasIDsIN.Exists(item => item == id_geo) ? "1" : "0") + "|";
                }

                //WriteGeoFenceInformation(device, latitud + "," + longitud, send_result_sql, fecha);
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;

            }
            
        }

        //static public void WriteGeoFenceInformation(string device, string coordenadas, string valor, DateTime ahora)
        //{
        //    string connectionString = "Data Source=doxuxzppx8.database.windows.net;Initial Catalog=spiderSQL;User ID=spiderfleet;Password=Magica2016";
        //    SqlConnection sqlConnect = new SqlConnection(connectionString);
        //    sqlConnect.Open();
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("ad.recordGeofencev2", sqlConnect);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //        cmd.Parameters.Add("@valor", System.Data.SqlDbType.NVarChar).Value = valor;
        //        cmd.Parameters.Add("@device", System.Data.SqlDbType.NVarChar).Value = device;
        //        cmd.Parameters.Add("@coordenates", System.Data.SqlDbType.NVarChar).Value = coordenadas;
        //        cmd.Parameters.Add("@fecha", System.Data.SqlDbType.DateTime).Value = ahora;


        //        SqlDataReader dr = cmd.ExecuteReader();


        //        sqlConnect.Close();
        //        sqlConnect.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        sqlConnect.Close();
        //        sqlConnect.Dispose();
        //    }
        //}

        private BsonDocument FindPosition(string id, double longitude, double latitude)
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