using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Logical;
using CredencialSpiderFleet.Models.Useful;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.GPS;
using SpiderFleetWebAPI.Models.Response.Logical;
using SpiderFleetWebAPI.Utils.Setting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Logical
{
    public class LogicalDao
    {

        private MongoDBContext mongoDBContext = new MongoDBContext();
        private const string start = "START";
        private const string end = "END";

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();


        public LogicalResponse GetRawData(string empresa, string device, string startdate, string enddate)
        {
            LogicalResponse response = new LogicalResponse();
            List<RawData> listRawData = new List<RawData>();
            List<RawData> listData = new List<RawData>();
            try
            {
                listData = ViajesGPS(empresa, device, startdate, enddate);

                if(listData.Count > 0)
                {
                    foreach (var item in listData)
                    {
                        if(!string.IsNullOrEmpty(item.Travel))
                        {
                            listRawData.Add(item);
                        }                                                
                    }
                    response.ListRawData = listRawData;
                    response.success = true;
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

        private List<RawData> ViajesGPS(string empresa, string device, string startdate, string enddate)
        {
            List<RawData> listData = new List<RawData>();

            int diff = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(empresa, "ITE", 1));

            try
            {
                DateTime inicio = Convert.ToDateTime(startdate);
                DateTime fin = Convert.ToDateTime(enddate);

                int horas = VerifyUser.VerifyUser.GetHours();

                inicio = inicio.AddHours(horas);
                fin = fin.AddHours(horas);

                string startConsult = inicio.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string endConsult = fin.ToString("yyyy-MM-ddTHH:mm:ssZ");

                BsonDocument bsonDocument = new BsonDocument();
                bsonDocument.Add("device", device);
                bsonDocument.Add("date", new BsonDocument("$gte", Convert.ToDateTime(startConsult)).Add("$lte", Convert.ToDateTime(endConsult)));

                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<GPS>("GPS");
                var result = stored.Find(build).Sort("{date:1}").ToList();

                RawData raw = new RawData();
                

                int count = 0;
                if (result.Count > 0)
                {
                    foreach (var data in result)
                    {
                        raw = new RawData();

                        raw.Date = data.Date.AddHours(-horas).ToString("yyyy-MM-dd H:mm:ss");

                        if (count == 0)
                        {
                            raw.Distance = 0;
                            raw.Seconds = "0";
                            raw.Travel = start;
                        }
                        else
                        {
                            GeoCoordinate lastCoord = new GeoCoordinate();
                            GeoCoordinate currentCoord = new GeoCoordinate();
                            lastCoord.Latitude = listData[count - 1].Latitude;
                            lastCoord.Longitude = listData[count - 1].Longitude;

                            currentCoord.Latitude = Convert.ToDouble(data.Location.Coordinates[1].ToString());
                            currentCoord.Longitude = Convert.ToDouble(data.Location.Coordinates[0].ToString());

                            raw.Distance = UseFul.GetDistanceDouble(lastCoord, currentCoord);
                            raw.Seconds = UseFul.GetDiferenceDates(Convert.ToDateTime(listData[count - 1].Date), Convert.ToDateTime(raw.Date)).ToString();
                        }

                        raw.Latitude = Convert.ToDouble(data.Location.Coordinates[1].ToString());
                        raw.Longitude = Convert.ToDouble(data.Location.Coordinates[0].ToString());

                        if (Convert.ToInt32(raw.Seconds) > diff)
                        {                            
                            raw.Travel = start;
                            if(listData.Count > 0)
                            {
                                int longitudFor = listData.Count();
                                listData[longitudFor - 1].Travel = end;
                            }
                        }

                        raw.Seconds = UseFul.CalcularTime(Convert.ToInt32(raw.Seconds));

                        listData.Add(raw);

                        count++;
                    }

                    int longitudFinal = listData.Count();
                    listData[longitudFinal - 1].Travel = end;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listData;
        }

    }
}