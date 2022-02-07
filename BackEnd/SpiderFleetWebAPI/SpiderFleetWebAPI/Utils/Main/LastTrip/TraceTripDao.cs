using CredencialSpiderFleet.Models.Main.HeatPoints;
using CredencialSpiderFleet.Models.Main.TraceTrip;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo;
using SpiderFleetWebAPI.Models.Request.Main.LastTrip;
using SpiderFleetWebAPI.Models.Response.Main.TraceTrip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SpiderFleetWebAPI.Utils.Main.LastTrip
{
    public class TraceTripDao
    {
        private MongoDBContext mongoDBContext = new MongoDBContext();

        private const string login = "Login";
        private const string gps = "GPS";
        private const string sleep = "Sleep";
        private const string alarm = "Alarm";

        //private const string speeding = "Speeding";  
        //private const string lowVoltage = "Low Voltage";
        //private const string highEngine = "High Engine Coolant Temperature";
        //private const string hardAcceleration = "Hard Acceleration";
        //private const string hardDeceleration = "Hard Deceleration";
        //private const string idleEngine = "Idle Engine";
        //private const string towing = "Towing";
        //private const string rpm = "High RPM";
        //private const string powerOn = "Power On";
        //private const string exhaust = "Exhaust Emission";
        //private const string quick = "Quick Lane Change";
        //private const string sharp = "Sharp Turn";
        //private const string fatigue = "Fatigue Driving";
        //private const string powerOff = "Power Off";
        //private const string sos = "SOS";
        //private const string tamper = "Tamper";
        //private const string ignitionOn = "Ignition On";
        //private const string ignitionOff = "Ignition Off";
        //private const string mil = "MIL alarm";
        //private const string dangerous = "Dangerous Driving";
        //private const string vibration = "Vibration";

        //private string[] alarms = new string[] { "Unlock Alarm", "No Card Presented", "Illegal Enter", "Illegal Ignition", "OBD Communication Error", "Emergency", "Geo-fence" };
        private Dictionary<string, string> alarms = new Dictionary<string, string>();
        //private const string unlock = "Unlock Alarm";//no
        //private const string card = "No Card Presented";//no
        //private const string enter = "Illegal Enter";//no
        //private const string ignition = "Illegal Ignition";//no
        //private const string obd = "OBD Communication Error";//no
        //private const string emergency = "Emergency";//no
        //private const string geo = "Geo-fence";//no
        private const string start = "START";
        private const string end = "END";

        private List<GeneralInformation> listInformation = new List<GeneralInformation>(); 


        public TraceTripDao() 
        {
            alarms.Add("Unlock Alarm", "Unlock Alarm");
            alarms.Add("No Card Presented", "No Card Presented");
            alarms.Add("Illegal Enter", "Illegal Enter");
            alarms.Add("Illegal Ignition", "Illegal Ignition");
            alarms.Add("OBD Communication Error", "OBD Communication Error");
            alarms.Add("Emergency", "Emergency");
            alarms.Add("Geo-fence", "Geo-fence");
        }

        public TraceTripResponse GetInformation(TraceTrip lastTrip)
        {
            TraceTripResponse response = new TraceTripResponse();
            List<GeneralInformation> listGeneral = new List<GeneralInformation>();

            try
            {
                listGeneral = GetTraceTrip(lastTrip);
                response.listMarkers = GetMakers(lastTrip);

                if (listGeneral.Count > 0)
                {
                    List<object> listObject = new List<object>();
                    listObject = GetTrips(listGeneral);
                    response.listPoints = (List<List<Point>>)listObject[0];
                    response.listTrips = (List<TripsInformation>)listObject[1];
                }
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        public List<object> GetTrips(List<GeneralInformation> listGeneral)
        {

            List<object> listObject = new List<object>();

            List<List<Point>> points = new List<List<Point>>();

            TripsInformation trips = new TripsInformation();
            List<TripsInformation> listTrips = new List<TripsInformation>();
            
            Point point = new Point();
            List<Point> list = new List<Point>();

            int milageStart = 0;
            int milageEnd = 0;
            double fuelStart = 0.0;
            double fuelEnd = 0.0;

            foreach (GeneralInformation data in listGeneral)
            {

                if (data.Event.Trim().Equals(end))
                {
                    point = new Point();
                    point.Device = data.Device;
                    point.Date = data.Date;
                    point.Type = data.Type;
                    point.Speed = data.Speed;
                    point.Event = data.Event;
                    point.Latitude = data.Latitude;
                    point.Longitude = data.Longitude;

                    milageEnd = data.milageEnd;
                    fuelEnd = data.fuelEnd;

                    trips.Date = data.Date;
                    trips.Device = data.Device;
                    trips.TotalFuel = (fuelEnd - fuelStart);
                    trips.TotalMilage = (milageEnd - milageStart) / 1000;

                    listTrips.Add(trips);

                    list.Add(point);
                    points.Add(list);
                    list = new List<Point>();

                    milageStart = 0;
                    milageEnd = 0;
                    fuelStart = 0.0;
                    fuelEnd = 0.0;

                }
                else
                {
                    point = new Point();

                    if (data.Event.Trim().Equals(start))
                    {
                        trips = new TripsInformation();
                        trips.Date = data.Date;
                        trips.Device = data.Device;
                        milageStart = data.milageStart;
                        fuelStart = data.fuelStart;
                    }

                    point.Device = data.Device;
                    point.Date = data.Date;
                    point.Type = data.Type;
                    point.Speed = data.Speed;
                    point.Event = data.Event;
                    point.Latitude = data.Latitude;
                    point.Longitude = data.Longitude;

                    list.Add(point);
                }
            }

            foreach (TripsInformation data in listTrips)
            {
                data.TotalTrips = points.Count;
            }

            listObject.Add(points);
            listObject.Add(listTrips);

            return listObject;
        }

        public List<GeneralInformation> GetTraceTrip(TraceTrip lastTrip) 
        {
            List<GeneralInformation> listPoints = new List<GeneralInformation>();
            try
            {

                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("event", new BsonDocument("$in", new BsonArray { login, gps })));
                bsonDocument.Add("device", lastTrip.Device);
                bsonDocument.Add("date", new BsonDocument("$gte", lastTrip.fechaInicio).Add("$lte", lastTrip.fechaFin));

                var _built = bsonDocument;

                var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<SpiderData>("spiderData");
                var result = StoredTripData.Find(
                    _built
                    ).Sort("{date: 1}")
                    .ToList();

                GeneralInformation point = new GeneralInformation();
                bool startIni = true;
                int countLogin = 0;

                int temMilageEnd = 0;
                double tempFuelEnd = 0.0;

                if (result.Count > 0)
                {
                    foreach (SpiderData data in result)
                    {
                        point = new GeneralInformation();

                        if (startIni)
                        {
                            point.Event = Convert.ToString(start);
                            point.Device = Convert.ToString(data.Device);
                            point.Date = Convert.ToDateTime(data._Date);
                            point.Longitude = Convert.ToString(data.Location.Coordinates[0].ToString());
                            point.Latitude = Convert.ToString(data.Location.Coordinates[1].ToString());
                            point.Speed = Convert.ToDouble(data.Speed);
                            point.Type = "";

                            startIni = false;
                            point.milageStart = Convert.ToInt32(data.TotalMilage) + Convert.ToInt32(data.CurrentMilage);
                            point.fuelStart = Convert.ToDouble(data.TotalFuel) + Convert.ToDouble(data.CurrentFuel);
                            countLogin++;
                            listPoints.Add(point);
                        }
                        else
                        {
                            if (data.Event.Equals(gps))
                            {
                                point.Event = Convert.ToString(data.Event);
                                point.Device = Convert.ToString(data.Device);
                                point.Date = Convert.ToDateTime(data._Date);
                                point.Longitude = Convert.ToString(data.Location.Coordinates[0].ToString());
                                point.Latitude = Convert.ToString(data.Location.Coordinates[1].ToString());
                                point.Speed = Convert.ToDouble(data.Speed);
                                point.Type = "";
                                countLogin = 1;
                                
                                temMilageEnd = Convert.ToInt32(data.TotalMilage) + Convert.ToInt32(data.CurrentMilage);
                                tempFuelEnd = Convert.ToDouble(data.TotalFuel) + Convert.ToDouble(data.CurrentFuel);

                                listPoints.Add(point);
                            }
                            else if (data.Event.Equals(login) & countLogin == 1)
                            {
                                point.Event = Convert.ToString(start);
                                point.Device = Convert.ToString(data.Device);
                                point.Date = Convert.ToDateTime(data._Date);
                                point.Longitude = Convert.ToString(data.Location.Coordinates[0].ToString());
                                point.Latitude = Convert.ToString(data.Location.Coordinates[1].ToString());
                                point.Speed = Convert.ToDouble(data.Speed);
                                point.Type = "";

                                point.milageStart = Convert.ToInt32(data.TotalMilage) + Convert.ToInt32(data.CurrentMilage);
                                point.fuelStart = Convert.ToDouble(data.TotalFuel) + Convert.ToDouble(data.CurrentFuel);

                                countLogin++;
                                
                                int row = listPoints.Count - 1;
                                listPoints[row].Event = end;
                                listPoints[row].milageEnd = temMilageEnd;
                                listPoints[row].fuelEnd = tempFuelEnd;

                                listPoints.Add(point);

                                temMilageEnd = 0;
                                tempFuelEnd = 0.0;
                            }
                        }
                    }
                    int lastItem = listPoints.Count - 1;
                    listPoints[lastItem].Event = end;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listPoints;
        }

        public List<Makers> GetMakers(TraceTrip lastTrip)
        {
            List<Makers> listMarkers = new List<Makers>();

            try
            {
                //BsonDocument bsonDocument = new BsonDocument(new BsonDocument("event", alarm));
                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("event", new BsonDocument("$in", new BsonArray { alarm, login })));
                bsonDocument.Add("device", lastTrip.Device);
                bsonDocument.Add("date", new BsonDocument("$gte", lastTrip.fechaInicio).Add("$lte", lastTrip.fechaFin));

                var _built = bsonDocument;

                var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<SpiderData>("spiderData");
                var result = StoredTripData.Find(
                    _built
                    ).Sort("{date: 1}")
                    .ToList();

                
                Makers markers = new Makers();
                bool startIni = true;
                int countLogin = 0;
                int total = 1;

                if (result.Count > 0)
                {
                    foreach (SpiderData data in result)
                    {
                        markers = new Makers();

                        if (startIni)
                        {
                            if(!data.Event.Equals(login) | data.Event.Equals(login))
                            {
                                Makers startData = new Makers();
                                startData.Event = Convert.ToString(start);
                                startData.Device = Convert.ToString(data.Device);
                                startData.Date = Convert.ToDateTime(data._Date);
                                startData.Longitude = Convert.ToString(data.Location.Coordinates[0].ToString());
                                startData.Latitude = Convert.ToString(data.Location.Coordinates[1].ToString());
                                listMarkers.Add(startData);
                            }

                            if (!data.Event.Equals(login))
                            {
                                markers.Event = Convert.ToString(data.Event);
                                markers.Device = Convert.ToString(data.Device);
                                markers.Date = Convert.ToDateTime(data._Date);
                                markers.Longitude = Convert.ToString(data.Location.Coordinates[0].ToString());
                                markers.Latitude = Convert.ToString(data.Location.Coordinates[1].ToString());
                                listMarkers.Add(markers);
                            }

                            startIni = false;
                            countLogin++;
                            
                        }
                        else
                        {
                            if (data.Alarms != null)
                            {
                                markers.Event = data.Alarms[0].Type.ToString();
                                markers.Device = Convert.ToString(data.Device);
                                markers.Date = Convert.ToDateTime(data._Date);
                                markers.Longitude = Convert.ToString(data.Location.Coordinates[0].ToString());
                                markers.Latitude = Convert.ToString(data.Location.Coordinates[1].ToString());
                                countLogin = 1;

                                listMarkers.Add(markers);
                            }
                            else if (data.Event.Equals(login) & countLogin == 1)
                            {
                                Makers startData = new Makers();
                                startData.Event = Convert.ToString(end);
                                startData.Device = Convert.ToString(data.Device);
                                startData.Date = Convert.ToDateTime(data._Date);
                                startData.Longitude = Convert.ToString(data.Location.Coordinates[0].ToString());
                                startData.Latitude = Convert.ToString(data.Location.Coordinates[1].ToString());
                                listMarkers.Add(startData);

                                if(total != result.Count)
                                {
                                    markers.Event = Convert.ToString(start);
                                    markers.Device = Convert.ToString(data.Device);
                                    markers.Date = Convert.ToDateTime(data._Date);
                                    markers.Longitude = Convert.ToString(data.Location.Coordinates[0].ToString());
                                    markers.Latitude = Convert.ToString(data.Location.Coordinates[1].ToString());
                                    countLogin++;

                                    listMarkers.Add(markers);                                    
                                }
                            }
                        }
                        total++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listMarkers;
        }

        public List<HeatPoints> GetHeatPoints(TraceTripRequest lastTrip)
        {

            List<HeatPoints> listMarkers = new List<HeatPoints>();
            Dictionary<string, string> hashEvent = new Dictionary<string, string>();
            hashEvent.Add(login, login);
            hashEvent.Add(gps, gps);
            hashEvent.Add(sleep, sleep);
            hashEvent.Add(alarm, alarm);

            try
            {
                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("event", new BsonDocument("$in", new BsonArray { login, gps, sleep, alarm })));
                bsonDocument.Add("device", lastTrip.Device);
                bsonDocument.Add("date", new BsonDocument("$gte", lastTrip.fechaInicio).Add("$lte", lastTrip.fechaFin));

                var _built = bsonDocument;

                var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<SpiderData>("spiderData");
                var result = StoredTripData.Find(
                    _built
                    ).Sort("{date: 1}")
                    .ToList();


                HeatPoints markers = new HeatPoints();
                bool start = true;
                if (result.Count > 0)
                {
                    foreach (SpiderData data in result)
                    {
                        bool contiene = false;

                        if (data.Alarms != null)
                        {
                            if (alarms.ContainsKey(data.Alarms[0].Type.ToString()))
                            {
                                contiene = true;
                            }
                        }

                        if (!contiene)
                        {
                            markers = new HeatPoints();
                            markers.Device = Convert.ToString(data.Device);
                            markers.Date = Convert.ToDateTime(data._Date);
                            markers.Longitude = Convert.ToString(data.Location.Coordinates[0].ToString());
                            markers.Latitude = Convert.ToString(data.Location.Coordinates[1].ToString());

                            if (start)
                            {
                                markers.Event = Convert.ToString("start");
                                start = false;
                            }
                            else
                            {
                                if (data.Alarms != null)
                                {
                                    markers.Event = Convert.ToString(data.Alarms[0].Type.ToString());
                                }
                                else
                                {
                                    markers.Event = Convert.ToString(data.Event);
                                }
                            }

                            listMarkers.Add(markers);
                        }
                    }
                }

                int lastItem = listMarkers.Count - 1;

                listMarkers[lastItem].Event = "end";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listMarkers;
        }


    }
}