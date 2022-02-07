using CredencialSpiderFleet.Models.ReportAdmin;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.Alarms;
using SpiderFleetWebAPI.Models.Response.ReportAdmin;
using SpiderFleetWebAPI.Utils.Main.LastPositionDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;

namespace SpiderFleetWebAPI.Utils.ReportAdmin
{
    public class ReportDeviceLastDataDao
    {
        private MongoDBContext mongoDBContext = new MongoDBContext();

        public ReportDeviceLastDataResponse GetDataMongo(string device)
        {

            ReportDeviceLastDataResponse response = new ReportDeviceLastDataResponse();

            try
            {
                DateTime startdate = DateTime.Now;
                DateTime enddate = DateTime.Now;
                //enddate = enddate.AddDays(-10);

                string start = startdate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string end = enddate.ToString("yyyy-MM-ddTHH:mm:ssZ");

                BsonDocument bson = new BsonDocument();
                bson.Add("device", device);
                bson.Add("date", new BsonDocument("$gte", "new Date(" + start + ")").Add("$lte", "new Date(" + end + ")"));

                var builds = bson;

                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GPS.GPS>("GPS");
                var result = stored.Find(builds).Sort("{date:-1}").FirstOrDefault();



                

                BsonDocument bsonDocument = new BsonDocument();
                bsonDocument.Add("device", device);
                bsonDocument.Add("date", new BsonDocument("$gte", Convert.ToDateTime(start)).Add("$lte", Convert.ToDateTime(end)));
                //bsonDocument.Add("date", new BsonDocument("$gte", Convert.ToDateTime(startdate)).Add("$lte", Convert.ToDateTime(enddate)));

                var build = bsonDocument;
              
                var storedAlarm = mongoDBContext.spiderMongoDatabase.GetCollection< SpiderFleetWebAPI.Models.Mongo.Alarms.Alarms> ("Alarms");
                //var resultAlarm = storedAlarm.Find(filterAlarm).Sort("{date:-1}").ToList();
                var resultAlarm = storedAlarm.Find(build).Sort("{date:-1}").ToList();

                List<ReportDeviceLastData> listAlarm = new List<ReportDeviceLastData>();
                List<ReportDeviceLastData> listLogin = new List<ReportDeviceLastData>();

                ReportDeviceLastData report = new ReportDeviceLastData();

                if (resultAlarm != null)
                {
                    
                }

                var buildLog = Builders<Login>.Filter;
                var filterLog = buildLog.Eq("device", device);

                var storedLog = mongoDBContext.spiderMongoDatabase.GetCollection<Login>("Login");
                var resultLog = storedLog.Find(filterLog).Sort("{date:-1}").ToListAsync();

                if (resultLog != null)
                {

                }

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                response.messages.Add(ex.Message);
            }

            return response;
        }

    }
}