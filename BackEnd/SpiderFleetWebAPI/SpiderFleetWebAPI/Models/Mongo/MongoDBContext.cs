using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models
{
    public class MongoDBContext
    {
        private MongoClient mongoClient;
        public IMongoDatabase spiderMongoDatabase { get; }
        public MongoDBContext()
        {
            mongoClient = new MongoClient(ConfigurationManager.ConnectionStrings["DB_SpiderMongoDB"].ConnectionString);
            spiderMongoDatabase = mongoClient.GetDatabase(ConfigurationManager.AppSettings.Get("SpiderMongoDatabase"));
        }
    }
}