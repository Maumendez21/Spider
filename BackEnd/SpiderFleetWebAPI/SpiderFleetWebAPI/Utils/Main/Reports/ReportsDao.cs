using CredencialSpiderFleet.Models.Main.Reports;
using CredencialSpiderFleet.Models.Main.TraceTrip;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo;
using SpiderFleetWebAPI.Models.Request.Main.LastTrip;
using SpiderFleetWebAPI.Utils.Catalog.InfoSubCompany;
using SpiderFleetWebAPI.Utils.Main.LastTrip;
using SpiderFleetWebAPI.Utils.Obds;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiderFleetWebAPI.Utils.Main.Reports
{
    public class ReportsDao
    {
        private const string login = "Login";
        private const string gps = "GPS";
        private const string sleep = "Sleep";
        private const string alarm = "Alarm";
        private const string speeding = "Speeding";
        private const string lowVoltage = "Low Voltage";
        private const string highEngine = "High Engine Coolant Temperature";
        private const string hardAcceleration = "Hard Acceleration";
        private const string hardDeceleration = "Hard Deceleration";
        private const string idleEngine = "Idle Engine";
        private const string towing = "Towing";
        private const string rpm = "High RPM";
        private const string powerOn = "Power On";
        private const string exhaust = "Exhaust Emission";
        private const string quick = "Quick Lane Change";
        private const string sharp = "Sharp Turn";
        private const string fatigue = "Fatigue Driving";
        private const string powerOff = "Power Off";
        private const string sos = "SOS";
        private const string tamper = "Tamper";
        private const string ignitionOn = "Ignition On";
        private const string ignitionOff = "Ignition Off";
        private const string mil = "MIL alarm";
        private const string dangerous = "Dangerous Driving";
        private const string vibration = "Vibration";
        private const string unlock = "Unlock Alarm";//no
        private const string card = "No Card Presented";//no
        private const string enter = "Illegal Enter";//no
        private const string ignition = "Illegal Ignition";//no
        private const string obd = "OBD Communication Error";//no
        private const string emergency = "Emergency";//no
        private const string geo = "Geo-fence";//no

        private Dictionary<string, string> alarms = new Dictionary<string, string>();
        private MongoDBContext mongoDBContext = new MongoDBContext();

        public ReportsDao() { }

        public List<ReportConduct> Conducta(ReportsFiltros filtros)
        {
            List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyListByUser> listSubCompany = new List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyListByUser>();
            List<CredencialSpiderFleet.Models.Obd.ObdHierarchy> listDevice = new List<CredencialSpiderFleet.Models.Obd.ObdHierarchy>();
            List<List<CredencialSpiderFleet.Models.Obd.ObdHierarchy>> list = new List<List<CredencialSpiderFleet.Models.Obd.ObdHierarchy>>();
            List<ReportConduct> listPoints = new List<ReportConduct>();

            try
            {
                listSubCompany = (new InfoSubCompanyDao()).Read(filtros.UserName);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            try
            {
                if (listSubCompany.Count > 0)
                {
                    foreach (CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyListByUser data in listSubCompany)
                    {
                        listDevice = (new ObdDao()).ReadIdSubCompanyHierarchy(data.IdSubCompany, data.SubCompany);
                        list.Add(listDevice);
                    }
                }
                else
                {
                    throw new Exception("No hay datos");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            try
            {
                if (list.Count > 0)
                {
                    foreach (List<CredencialSpiderFleet.Models.Obd.ObdHierarchy> listData in list)
                    {

                        if (listData.Count > 0)
                        {
                            foreach (CredencialSpiderFleet.Models.Obd.ObdHierarchy data in listData)
                            {
                                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("event", new BsonDocument("$in",
                                    new BsonArray { alarm })));

                                //new BsonArray { login, gps, sleep, alarm, speeding, lowVoltage, highEngine, hardAcceleration, hardDeceleration, idleEngine, towing, rpm,
                                //powerOn, exhaust, quick, sharp, fatigue, powerOff, sos, tamper, ignitionOn, ignitionOff, mil, dangerous, vibration, unlock, card, enter,
                                //ignition, obd, emergency, geo})));

                                bsonDocument.Add("device", data.Device);
                                bsonDocument.Add("date", new BsonDocument("$gte", filtros.fechaInicio).Add("$lte", filtros.fechaFin));

                                var _built = bsonDocument;

                                var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<SpiderData>("spiderData");
                                var result = StoredTripData.Find(
                                    _built
                                    ).Sort("{date: 1}")
                                    .ToList();

                                ReportConduct point = new ReportConduct();

                                if (result.Count > 0)
                                {
                                    foreach (SpiderData spider in result)
                                    {
                                        point = new ReportConduct();
                                        point.Device = Convert.ToString(spider.Device);
                                        point.Date = Convert.ToDateTime(spider._Date);
                                        point.Longitude = Convert.ToString(spider.Location.Coordinates[0].ToString());
                                        point.Latitude = Convert.ToString(spider.Location.Coordinates[1].ToString());
                                        point.Event = Convert.ToString(spider.Event);
                                        point.Speed = Convert.ToDouble(spider.Speed);
                                        point.NameSubCompany = data.NameSubCompany;

                                        if (point.Event.Equals(alarm))
                                        {
                                            point.Type = spider.Alarms[0].Type.ToString();
                                        }
                                        else
                                        {
                                            point.Type = "";
                                        }
                                        listPoints.Add(point);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listPoints;
        }
    
        public int Trips(ReportsFiltros filtros)
        {
            List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyListByUser> listSubCompany = new List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyListByUser>();
            List<CredencialSpiderFleet.Models.Obd.ObdHierarchy> listDevice = new List<CredencialSpiderFleet.Models.Obd.ObdHierarchy>();
            List<List<CredencialSpiderFleet.Models.Obd.ObdHierarchy>> list = new List<List<CredencialSpiderFleet.Models.Obd.ObdHierarchy>>();

            try
            {
                try
                {
                    listSubCompany = (new InfoSubCompanyDao()).Read(filtros.UserName);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                try
                {
                    if (listSubCompany.Count > 0)
                    {
                        foreach (CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyListByUser data in listSubCompany)
                        {
                            listDevice = (new ObdDao()).ReadIdSubCompanyHierarchy(data.IdSubCompany, data.SubCompany);
                            list.Add(listDevice);
                        }
                    }
                    else
                    {
                        throw new Exception("No hay datos");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                
                List<TraceTrip> listFiltros = new List<TraceTrip>();
                try
                {
                    if (list.Count > 0)
                    {
                        foreach (List<CredencialSpiderFleet.Models.Obd.ObdHierarchy> listObds in list)
                        {
                            foreach (CredencialSpiderFleet.Models.Obd.ObdHierarchy obd in listObds)
                            {
                                TraceTrip trace = new TraceTrip();
                                trace.Device = obd.Device;
                                trace.fechaInicio = filtros.fechaInicio;
                                trace.fechaFin = filtros.fechaFin;
                                listFiltros.Add(trace);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                try
                {
                    List<List<GeneralInformation>> generals = new List<List<GeneralInformation>>();
                    List<GeneralInformation> listGeneral = new List<GeneralInformation>();

                    foreach(TraceTrip trace in listFiltros)
                    {
                        listGeneral = (new TraceTripDao()).GetTraceTrip(trace);
                        generals.Add(listGeneral);
                    }

                    List<List<Point>> listPoints = new List<List<Point>>();
                    List<TripsInformation> listTrips = new List<TripsInformation>();
                    List<List<TripsInformation>> trips = new List<List<TripsInformation>>();
                    if (generals.Count > 0)
                    {
                        foreach(List<GeneralInformation> information in generals)
                        {
                            List<object> listObject = new List<object>();
                            listObject = (new TraceTripDao()).GetTrips(information);
                            listPoints = (List<List<Point>>)listObject[0];
                            listTrips = (List<TripsInformation>)listObject[1];
                            trips.Add(listTrips);
                        }                        
                    }

                    if (trips.Count > 0)
                    {
                        (new ReportsExcel()).excelTrips(trips);
                    }                   
                }
                catch(Exception ex)
                {

                }


                return 0;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    
    }
}