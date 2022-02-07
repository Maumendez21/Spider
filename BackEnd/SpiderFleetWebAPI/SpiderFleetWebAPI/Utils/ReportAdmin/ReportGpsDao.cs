using CredencialSpiderFleet.Models.ReportAdmin;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.GPS;
using SpiderFleetWebAPI.Models.Response.ReportAdmin;
using SpiderFleetWebAPI.Utils.Main.Reports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiderFleetWebAPI.Utils.ReportAdmin
{
    public class ReportGpsDao
    {
        public ReportGpsDao() { }
        private MongoDBContext mongoDBContext = new MongoDBContext();

        public ReportGPSResponse ReadGps(DateTime start, DateTime end, string device)
        {
            List<ReportGPS> listGPS = new List<ReportGPS>();
            ReportGPSResponse response = new ReportGPSResponse();
            try
            {
              
                var buildTrips = Builders<GPS>.Filter;
                var filterDevice = buildTrips.Eq(x => x.Device, device);
                var filterDateStart = buildTrips.Gte(x =>x.Date, start);
                var filterDateEnd = buildTrips.Lte(x => x.Date, end);

                var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<GPS>("GPS");
                var result = StoredTripData.Find(filterDevice & filterDateStart & filterDateEnd).Sort("{date: -1}").ToList();

                ReportGPS gps = new ReportGPS();

                if (result.Count > 0)
                {
                    foreach(var data in result)
                    {
                        gps = new ReportGPS();
                        gps.Date = data.Date;
                        gps.Latitude = data.Location.Coordinates[1].ToString();
                        gps.Longitude = data.Location.Coordinates[0].ToString();
                        gps.Speed = data.Speed;
                        gps.RPM = "dato faltante";// data.Bearing;
                        listGPS.Add(gps);
                    }

                    response.success = true;
                    response.listGps = listGPS;

                    if(listGPS.Count > 0)
                    {
                        (new ReportsExcel()).excelGps(listGPS);
                    }
                }
                else
                {
                    response.success = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}