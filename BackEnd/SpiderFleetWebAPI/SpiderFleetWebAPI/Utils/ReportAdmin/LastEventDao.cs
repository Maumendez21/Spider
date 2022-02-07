using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.ReportAdmin;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.Alarms;
using SpiderFleetWebAPI.Models.Mongo.GPS;
using SpiderFleetWebAPI.Models.Mongo.Login;
using SpiderFleetWebAPI.Models.Mongo.Sleep;
using SpiderFleetWebAPI.Models.Response.ReportAdmin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SpiderFleetWebAPI.Utils.ReportAdmin
{
    public class LastEventDao
    {
        public LastEventDao() { }

        private MongoDBContext mongoDBContext = new MongoDBContext();
        private const string url = "https://www.google.com/maps/search/?api=1&query=";

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private const string LOGIN = "Login";
        private const string GPS = "GPS";
        private const string ALARMS = "Alarms";
        private const string SLEEP = "Sleep";
        private const string DTC = "DTC";

        public LastEventResponse ReadLastEvent(string idempresa, string events)
        {
            LastEventResponse repsonse = new LastEventResponse();

            try
            {
                List<Vehicles> listVehicles = new List<Vehicles>();
                listVehicles = ReadVehicleListHierarchy(idempresa);

                if (listVehicles.Count > 0)
                {
                    if (events.Equals(LOGIN))
                    {
                        repsonse.listEvents = Login(listVehicles, events);
                    }
                    else if (events.Equals(GPS))
                    {
                        repsonse.listEvents = Gps(listVehicles, events);
                    }
                    else if (events.Equals(ALARMS))
                    {
                        repsonse.listEvents = Alarms(listVehicles, events);
                    }
                    else if (events.Equals(SLEEP))
                    {
                        repsonse.listEvents = Sleep(listVehicles, events);
                    }
                    //else if (events.Equals(DTC))
                    //{
                    //    repsonse.listEvents = Others(listVehicles, events);
                    //}
                }

            }
            catch (Exception ex)
            {
                repsonse.success = false;
                repsonse.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                repsonse.messages.Add(ex.Message);
            }

            return repsonse;
        }

        public List<Vehicles> ReadVehicleListHierarchy(string idempresa)
        {

            List<Vehicles> listVehicles = new List<Vehicles>();
            Vehicles vehicles = new Vehicles();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_device_hierarchy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@idempresa", Convert.ToString(idempresa)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vehicles = new Vehicles();
                            vehicles.Device = Convert.ToString(reader["device"]);
                            vehicles.Name = Convert.ToString(reader["Nombre"]);

                            listVehicles.Add(vehicles);
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
            return listVehicles;
        }

        private List<LastEvent> Login(List<Vehicles> listDevice, string events)
        {
            List<LastEvent> listPosition = new List<LastEvent>();
            try
            {

                foreach (Vehicles device in listDevice)
                {
                    var buildTrips = Builders<Login>.Filter;
                    var filterDevice = buildTrips.Eq(x => x.Device, device.Device);

                    var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<Login>("Login");
                    //List<Login> result = StoredTripData.Find(filterDevice & filterEvent).Sort("{date: -1}").ToList();
                    var result = StoredTripData.Find(filterDevice).Sort("{date: -1}").FirstOrDefault();

                    LastEvent position = new LastEvent();

                    if (result != null)
                    {
                        position.Device = device.Device;
                        position.Event = events;
                        position.Name = device.Name;
                        position.Url = url + result.Location.Coordinates[1].ToString() + "," + result.Location.Coordinates[0].ToString();
                        position.Date = result.Date;
                        listPosition.Add(position);
                    }

                }
                return listPosition;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<LastEvent> Gps(List<Vehicles> listDevice, string events)
        {
            List<LastEvent> listPosition = new List<LastEvent>();
            try
            {
                foreach (Vehicles device in listDevice)
                {
                    var buildTrips = Builders<GPS>.Filter;
                    var filterDevice = buildTrips.Eq(x => x.Device, device.Device);

                    var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<GPS>("GPS");
                    var result = StoredTripData.Find(filterDevice).Sort("{date: -1}").FirstOrDefault();

                    LastEvent position = new LastEvent();

                    if (result != null)
                    {
                        position.Device = device.Device;
                        position.Event = events;
                        position.Name = device.Name;
                        position.Url = url + result.Location.Coordinates[1].ToString() + "," + result.Location.Coordinates[0].ToString();
                        position.Date = result.Date;
                        listPosition.Add(position);
                    }

                }
                return listPosition;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<LastEvent> Alarms(List<Vehicles> listDevice, string events)
        {
            List<LastEvent> listPosition = new List<LastEvent>();
            try
            {
                foreach (Vehicles device in listDevice)
                {
                    var buildTrips = Builders<SpiderFleetWebAPI.Models.Mongo.Alarms.Alarms>.Filter;
                    var filterDevice = buildTrips.Eq(x => x.Device, device.Device);

                    var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<SpiderFleetWebAPI.Models.Mongo.Alarms.Alarms>("Alarms");
                    var result = StoredTripData.Find(filterDevice).Sort("{date: -1}").FirstOrDefault();

                    LastEvent position = new LastEvent();

                    if (result != null)
                    {
                        position.Device = device.Device;
                        position.Event = events;
                        position.Name = device.Name;
                        position.Url = url + result.Location.Coordinates[1].ToString() + "," + result.Location.Coordinates[0].ToString();
                        position.Date = result.Date;
                        listPosition.Add(position);
                    }

                }
                return listPosition;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<LastEvent> Sleep(List<Vehicles> listDevice, string events)
        {
            List<LastEvent> listPosition = new List<LastEvent>();
            try
            {
                foreach (Vehicles device in listDevice)
                {
                    var buildTrips = Builders<Sleep>.Filter;
                    var filterDevice = buildTrips.Eq(x => x.Device, device.Device);

                    var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<Sleep>("Sleep");
                    var result = StoredTripData.Find(filterDevice).Sort("{date: -1}").FirstOrDefault();

                    LastEvent position = new LastEvent();

                    if (result != null)
                    {
                        position.Device = device.Device;
                        position.Event = events;
                        position.Name = device.Name;
                        position.Url = url + result.Location.Coordinates[1].ToString() + "," + result.Location.Coordinates[0].ToString();
                        position.Date = result.Date;
                        listPosition.Add(position);
                    }

                }
                return listPosition;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //private List<LastEvent> Others(List<Vehicles> listDevice, string events)
        //{
        //    List<LastEvent> listPosition = new List<LastEvent>();
        //    try
        //    {
        //        foreach (Vehicles device in listDevice)
        //        {
        //            var buildTrips = Builders<DTC>.Filter;
        //            var filterDevice = buildTrips.Eq(x => x.Device, device.Device);

        //            var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<DTC>("DTC");
        //            var result = StoredTripData.Find(filterDevice).Sort("{date: -1}").FirstOrDefault();

        //            LastEvent position = new LastEvent();

        //            if (result != null)
        //            {
        //                position.Device = device.Device;
        //                position.Event = events;
        //                position.Name = device.Name;
        //                //position.Url = url + result.Location.Coordinates[1].ToString() + "," + result.Location.Coordinates[0].ToString();
        //                position.Date = result.Date;
        //                listPosition.Add(position);
        //            }

        //        }
        //        return listPosition;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}