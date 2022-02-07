using CredencialSpiderFleet.Models.Configuration;
using CredencialSpiderFleet.Models.Itineraries;
using CredencialSpiderFleet.Models.Useful;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.GPS;
using SpiderFleetWebAPI.Models.Response.CardGraphics;
using SpiderFleetWebAPI.Models.Response.Itineraries;
using SpiderFleetWebAPI.Utils.Itineraries;
using SpiderFleetWebAPI.Utils.Setting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SpiderFleetWebAPI.Utils.CardGraphics
{
    public class CardGraphicsDao
    {
        private MongoDBContext mongoDBContext = new MongoDBContext();
        private VariableConfiguration configuration = new VariableConfiguration();
        private UseFul use = new UseFul();
        private int sustraer = -1;
        private int intentos = 0;
        private List<CredencialSpiderFleet.Models.Itineraries.Itineraries> listRecursive = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
        private const string START_TRIP = "START";
        private const string END_TRIP = "END";
        private const string POINT_TRIP = "POINTS";

        public CardGraphicsResponse Graphics(string hierarchy, string device)
        {
            CardGraphicsResponse response = new CardGraphicsResponse();
            ItinerariesListLastResponse responseIti = new ItinerariesListLastResponse();
            try
            {

                responseIti = ReadTripsRecursive(hierarchy, device);

                CredencialSpiderFleet.Models.CardGraphics.CardGraphics Odo = new CredencialSpiderFleet.Models.CardGraphics.CardGraphics();
                CredencialSpiderFleet.Models.CardGraphics.CardGraphics Fuel = new CredencialSpiderFleet.Models.CardGraphics.CardGraphics();
                CredencialSpiderFleet.Models.CardGraphics.CardGraphics Time = new CredencialSpiderFleet.Models.CardGraphics.CardGraphics();

                foreach (var item in responseIti.listItineraries)
                {
                    //label == dia      //data == valores  MM-dd
                    Odo.label.Add(item.StartDate.ToString("MMMM-dd"));
                    Odo.data.Add(item.ODO);

                    Fuel.label.Add(item.StartDate.ToString("MMMM-dd"));
                    Fuel.data.Add(item.Fuel);

                    Time.label.Add(item.StartDate.ToString("MMMM-dd"));
                    Time.data.Add(item.Time);
                }

                if(responseIti.listItineraries.Count > 0)
                {
                    response.success = true;
                    response.Odo = Odo;
                    response.Fuel = Fuel;
                    response.Time = Time;
                }
                else
                {
                    response.success = false;
                }

                return response;
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }
        public ItinerariesListLastResponse ReadTripsRecursive(string hierarchy, string device)
        {
            ItinerariesListLastResponse response = new ItinerariesListLastResponse();
            try
            {
                DateTime now = DateTime.Today;
                var startdate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                var enddate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
                int horas = VerifyUser.VerifyUser.GetHours();
                startdate = startdate.AddHours(horas);
                enddate = enddate.AddHours(horas);

                response = ReadTripsRecursive(response, hierarchy, device, startdate, enddate);

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                response.messages.Add(ex.Message);
            }
            return response;
        }
        public ItinerariesListLastResponse ReadTripsRecursive(ItinerariesListLastResponse resp, string hierarchy, string device, DateTime startdate, DateTime enddate)
        {

            ItinerariesListLastResponse response = new ItinerariesListLastResponse();

            try
            {
                string node = use.hierarchyPrincipalToken(hierarchy);
                int diff = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(node, "ITE", 1));

                DateTime newStart = new DateTime();
                DateTime newEnd = new DateTime();

                if (listRecursive.Count > 0)
                {
                    newStart = startdate.AddDays(sustraer);
                    newEnd = enddate.AddDays(sustraer);
                }
                else
                {
                    newStart = startdate;
                    newEnd = enddate;
                }

                resp = ReadItinerariosDeviceListRecursivo(resp, diff, device, newStart, newEnd);

                if (listRecursive.Count == 0)
                {
                    if (resp.listItineraries.Count > 6)
                    {
                        foreach (CredencialSpiderFleet.Models.Itineraries.Itineraries x in resp.listItineraries)
                        {
                            if (listRecursive.Count < 5)
                            {
                                listRecursive.Add(x);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        listRecursive = resp.listItineraries;
                    }
                }
                else
                {
                    foreach (CredencialSpiderFleet.Models.Itineraries.Itineraries x in resp.listItineraries)
                    {
                        if (listRecursive.Count < 5)
                        {
                            listRecursive.Add(x);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (listRecursive.Count < configuration.totalTrips)
                {

                    ItinerariesListLastResponse list = new ItinerariesListLastResponse();
                    if (intentos < 5)
                    {
                        intentos++;

                        if (listRecursive.Count == 0)
                        {
                            newStart = startdate.AddDays(sustraer);
                            newEnd = enddate.AddDays(sustraer);
                        }

                        list = ReadTripsRecursive(response, hierarchy, device, newStart, newEnd);
                    }
                    else
                    {
                        response.listItineraries = listRecursive.OrderByDescending(x => x.StartDate).ToList();
                        response.success = true;
                        return response;
                    }


                    if (list.success)
                    {
                        response.listItineraries = listRecursive.OrderByDescending(x => x.StartDate).ToList();
                        response.success = true;
                    }
                }
                else
                {
                    response.listItineraries = listRecursive.OrderByDescending(x => x.StartDate).ToList();
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

        public ItinerariesListLastResponse ReadItinerariosDeviceListRecursivo(ItinerariesListLastResponse resp, int diff, string device, DateTime startdate, DateTime enddate)
        {
            ItinerariesListResponse dataResponse = new ItinerariesListResponse();
            ItinerariesListLastResponse response = new ItinerariesListLastResponse();
            List<CredencialSpiderFleet.Models.Itineraries.Itineraries> listItineraries = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();

            try

            {
                int horas = VerifyUser.VerifyUser.GetHours();

                dataResponse.listItineraries = ItinerariesProcess(device, startdate, enddate, diff);

                foreach (List<CredencialSpiderFleet.Models.Itineraries.Itineraries> itineraries in dataResponse.listItineraries)
                {
                    foreach (CredencialSpiderFleet.Models.Itineraries.Itineraries data in itineraries)
                    {
                        //string calculoTime = UseFul.CalcularTime(Convert.ToInt32(data.Time));

                        //data.Time = data.Time;
                        data.ODO = data.ODO;
                        data.Fuel = data.Fuel;

                        data.StartDate = data.StartDate.AddHours(-horas);
                        data.StartHour = data.StartDate.ToString("HH:mm:ss");
                        data.EndDate = data.EndDate.AddHours(-horas);
                        data.EndHour = data.EndDate.ToString("HH:mm:ss");

                        CultureInfo ci = new CultureInfo("es-MX");
                        ci = new CultureInfo("es-MX");
                        data.TravelDate = new CultureInfo("es-MX", true).TextInfo.ToTitleCase(data.EndDate.ToString("dddd, dd MMMM yyyy", ci));

                        listItineraries.Add(data);
                    }
                }

                response.listItineraries = listItineraries;

            }
            catch (Exception ex)
            {

                response.success = false;
                response.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                response.messages.Add(ex.Message);
            }

            return response;
        }
        public List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> ItinerariesProcess(string device, DateTime startdate, DateTime enddate, int diff)
        {
            List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> listItineraries = new List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>>();
            List<CredencialSpiderFleet.Models.Itineraries.Itineraries> list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
            List<List<ItinerariesKey>> listKey = new List<List<ItinerariesKey>>();

            string VehicleName = string.Empty;
            string DriverData = string.Empty;
            string Image = string.Empty;

            try
            {
                listKey = GetDataProcess(device, startdate, enddate, diff);

                int rows = 0;
                int count = 0;

                if (listKey.Count > 0)
                {
                    string fecha = string.Empty;
                    int milageIni = 0;
                    int milageFin = 0;

                    double fuelIni = 0;
                    double fuelFin = 0;

                    bool bandera = false;

                    foreach (ItinerariesKey data in listKey[0])
                    {
                        int diferencia = data.Diff;

                        CredencialSpiderFleet.Models.Itineraries.Itineraries itineraries = new CredencialSpiderFleet.Models.Itineraries.Itineraries();
                        if (data.Event.Equals(START_TRIP))
                        {
                            if (string.IsNullOrEmpty(fecha))
                            {
                                itineraries.Device = data.Device;
                                itineraries.Batery = data.batery;
                                itineraries.Label = data.label;
                                itineraries.StartDate = data.StartDate;
                                itineraries.EndDate = data.EndDate;
                                itineraries.DriverData = DriverData;
                                itineraries.Image = Image;
                                itineraries.VehicleName = VehicleName;

                                itineraries.ODO = string.Empty;
                                itineraries.Fuel = string.Empty;
                                itineraries.Score = string.Empty;
                                milageIni = data.totalM;
                                fuelIni = data.totalF;

                                fecha = data.StartDate.ToString("dd-MM-yyyy");

                                list.Add(itineraries);

                                bandera = true;
                            }
                        }
                        else
                        {
                            if (bandera)
                            {
                                if (data.Event.Equals(END_TRIP))
                                {
                                    if (list.Count > 0)
                                    {
                                        count = list.Count;
                                        list[count - 1].EndDate = data.EndDate;

                                        milageFin = data.totalM;
                                        list[count - 1].ODO = use.metrosKilometros(milageFin - milageIni).ToString();

                                        fuelFin = Convert.ToDouble(data.totalF);
                                        list[count - 1].Fuel = use.litros(fuelFin - fuelIni).ToString();

                                        listItineraries.Add(list);
                                        list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
                                        fecha = string.Empty;
                                    }
                                }
                            }
                        }
                        rows++;
                    }

                    list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();

                    foreach (ItinerariesKey data in listKey[1])
                    {
                        int diferencia = data.Diff;

                        CredencialSpiderFleet.Models.Itineraries.Itineraries itineraries = new CredencialSpiderFleet.Models.Itineraries.Itineraries();
                        if (data.Event.Equals(START_TRIP))
                        {
                            if (string.IsNullOrEmpty(fecha))
                            {
                                itineraries.Device = data.Device;
                                itineraries.Batery = data.batery;
                                itineraries.Label = data.label;
                                itineraries.StartDate = data.StartDate;
                                itineraries.EndDate = data.EndDate;
                                itineraries.DriverData = DriverData;
                                itineraries.Image = Image;
                                itineraries.VehicleName = VehicleName;

                                itineraries.ODO = string.Empty;
                                itineraries.Fuel = string.Empty;
                                itineraries.Score = string.Empty;
                                milageIni = data.totalM;
                                fuelIni = data.totalF;

                                fecha = data.StartDate.ToString("dd-MM-yyyy");

                                list.Add(itineraries);

                                bandera = true;
                            }
                        }
                        else
                        {
                            if (bandera)
                            {
                                if (data.Event.Equals(END_TRIP))
                                {
                                    if (list.Count > 0)
                                    {
                                        count = list.Count;
                                        list[count - 1].EndDate = data.EndDate;

                                        milageFin = data.totalM;
                                        list[count - 1].ODO = Convert.ToString(use.metrosKilometros(milageFin - milageIni));

                                        fuelFin = Convert.ToDouble(data.totalF);
                                        list[count - 1].Fuel = Convert.ToString(use.litros(fuelFin - fuelIni));

                                        fecha = string.Empty;
                                    }
                                }
                                else if (data.Event.Equals(START_TRIP))
                                {
                                    count = list.Count;


                                    itineraries.Device = data.Device;
                                    itineraries.Batery = data.batery;
                                    itineraries.Label = data.label;
                                    itineraries.StartDate = data.StartDate;
                                    itineraries.EndDate = data.EndDate;

                                    itineraries.ODO = Convert.ToString(use.metrosKilometros(milageFin - milageIni));
                                    itineraries.Fuel = Convert.ToString(use.litros(fuelFin - fuelIni));
                                    milageIni = data.totalM;
                                    fuelIni = data.totalF;

                                    itineraries.DriverData = DriverData;
                                    itineraries.Image = Image;
                                    itineraries.VehicleName = VehicleName;

                                    itineraries.Score = string.Empty;

                                    list.Add(itineraries);
                                }
                            }
                            else
                            {
                                if (data.Event.Equals(END_TRIP))
                                {
                                    if (listItineraries.Count > 0)
                                    {
                                        itineraries.Device = data.Device;
                                        itineraries.Batery = data.batery;
                                        itineraries.Label = data.label;
                                        itineraries.StartDate = data.StartDate;
                                        itineraries.EndDate = data.EndDate;

                                        itineraries.DriverData = DriverData;
                                        itineraries.Image = Image;
                                        itineraries.VehicleName = VehicleName;

                                        itineraries.ODO = "0";
                                        itineraries.Fuel = "0";
                                        itineraries.Score = string.Empty;

                                        list.Add(itineraries);
                                        fecha = string.Empty;
                                    }
                                }
                            }
                        }
                      
                        rows++;
                    }


                }

                string valor = string.Empty;

                int totalSegundos = 0;
                foreach (var item in list)
                {
                    totalSegundos += UseFul.GetDiferenceDates( item.StartDate, item.EndDate);
                }

                string valor2 = string.Empty;
                listItineraries[0][0].Time = totalSegundos.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listItineraries;
        }
        public List<List<ItinerariesKey>> GetDataProcess(string device, DateTime startdate, DateTime enddate, int diff)
        {
            List<List<ItinerariesKey>> listData = new List<List<ItinerariesKey>>();
            List<ItinerariesKey> listKey = new List<ItinerariesKey>();
            List<ItinerariesKey> listTime = new List<ItinerariesKey>();

            try
            {
                string start = startdate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string end = enddate.ToString("yyyy-MM-ddTHH:mm:ssZ");

                BsonDocument bsonDocument = new BsonDocument();
                bsonDocument.Add("device", device);
                bsonDocument.Add("date", new BsonDocument("$gte", Convert.ToDateTime(start)).Add("$lte", Convert.ToDateTime(end)));

                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<GPS>("GPS");
                var result = stored.Find(build).Sort("{date:1}").ToList();
                DateTime fechaAnterior = DateTime.Now;

                if (result.Count > 0)
                {
                    ItinerariesKey keys = new ItinerariesKey();
                    keys.Event = START_TRIP;
                    fechaAnterior = result[0].Date;
                    keys.Device = result[0].Device;
                    keys.StartDate = result[0].Date;
                    keys.EndDate = result[0].Date;
                    keys.Diff = result[0].Diff;
                    keys.ODO = string.Empty;
                    keys.Fuel = string.Empty;
                    keys.VelocidadMaxima = Convert.ToString(result[0].Speed);
                    keys.NoAlarmas = string.Empty;
                    keys.Longitude = result[0].Location.Coordinates[0].ToString();
                    keys.Latitude = result[0].Location.Coordinates[1].ToString();
                    keys.totalM = result[0].TotalMilage + result[0].CurrentMilage;
                    keys.totalF = result[0].TotalFuel + result[0].CurrentFuel;

                    keys.label = string.Empty;
                    keys.batery = string.Empty;

                    listKey.Add(keys);

                    int count = result.Count - 1;

                    keys = new ItinerariesKey();
                    keys.Event = END_TRIP;
                    fechaAnterior = result[count].Date;
                    keys.Device = result[count].Device;
                    keys.StartDate = result[count].Date;
                    keys.EndDate = result[count].Date;
                    keys.Diff = result[count].Diff;
                    keys.ODO = string.Empty;
                    keys.Fuel = string.Empty;
                    keys.VelocidadMaxima = Convert.ToString(result[count].Speed);
                    keys.NoAlarmas = string.Empty;
                    keys.Longitude = result[count].Location.Coordinates[0].ToString();
                    keys.Latitude = result[count].Location.Coordinates[1].ToString();
                    keys.totalM = result[count].TotalMilage + result[0].CurrentMilage;
                    keys.totalF = result[count].TotalFuel + result[0].CurrentFuel;

                    keys.label = string.Empty;
                    keys.batery = string.Empty;

                    listKey.Add(keys);


                    //tiempo
                    fechaAnterior = DateTime.Now;
                    count = 0;
                    foreach (GPS data in result)
                    {
                        keys = new ItinerariesKey();
                        if (data.Diff <= diff)
                        {
                            if (data.Diff <= 0)
                            {
                                int vals = UseFul.GetDiferenceDates(fechaAnterior, data.Date);

                                if (vals <= diff)
                                {
                                    keys.Event = START_TRIP;
                                }
                                else
                                {
                                    int rows = listTime.Count;
                                    listTime[rows - 1].Event = END_TRIP;
                                    keys.Event = START_TRIP;
                                }
                            }
                            else
                            {
                                keys.Event = START_TRIP;
                            }
                        }
                        else
                        {
                            if (listTime.Count > 0)
                            {
                                int vals = UseFul.GetDiferenceDates(fechaAnterior, data.Date);
                                if (vals <= diff)
                                {
                                    keys.Event = START_TRIP;
                                }
                                else
                                {
                                    int rows = listTime.Count;
                                    listTime[rows - 1].Event = END_TRIP;
                                    keys.Event = START_TRIP;
                                }
                            }
                            else
                            {
                                keys.Event = START_TRIP;
                            }
                        }

                        fechaAnterior = data.Date;
                        keys.Device = data.Device;
                        keys.StartDate = data.Date;
                        keys.EndDate = data.Date;
                        keys.Diff = data.Diff;
                        keys.ODO = string.Empty;
                        keys.Fuel = string.Empty;
                        keys.VelocidadMaxima = Convert.ToString(data.Speed);
                        keys.NoAlarmas = string.Empty;
                        keys.Longitude = data.Location.Coordinates[0].ToString();
                        keys.Latitude = data.Location.Coordinates[1].ToString();
                        keys.totalM = data.TotalMilage + data.CurrentMilage;
                        keys.totalF = data.TotalFuel + data.CurrentFuel;


                        if (data.Protocol.Equals("CELDA"))
                        {
                            keys.label = "Nivel de Bateria";
                            keys.batery = data.VehiculeState + "%";
                        }
                        else
                        {
                            keys.label = string.Empty;
                            keys.batery = string.Empty;
                        }

                        listTime.Add(keys);
                        count++;
                    }

                    if (listTime.Count > 0)
                    {
                        int row = listKey.Count;
                        listTime[row - 1].Event = END_TRIP;
                    }
                }

                string datas = string.Empty;

                if (result.Count > 0)
                {
                    listData.Add(listKey);
                    listData.Add(listTime);
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