using CredencialSpiderFleet.Models.Bot;
using CredencialSpiderFleet.Models.Configuration;
using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Itineraries;
using CredencialSpiderFleet.Models.Logical;
using CredencialSpiderFleet.Models.SnappedPoints;
using CredencialSpiderFleet.Models.TravelReport;
using CredencialSpiderFleet.Models.Useful;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.GPS;
using SpiderFleetWebAPI.Models.Response.Bot;
using SpiderFleetWebAPI.Models.Response.Details;
using SpiderFleetWebAPI.Models.Response.Itineraries;
using SpiderFleetWebAPI.Utils.Details;
using SpiderFleetWebAPI.Utils.Itineraries;
using SpiderFleetWebAPI.Utils.Setting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Bot
{
    public class BotDao
    {
        private const string START_TRIP = "START";
        private const string END_TRIP = "END";
        private const string POINTS_TRIP = "POINTS";
        private const string MXV = "MXV";
        private const string WTC = "WTC";

        private MongoDBContext mongoDBContext = new MongoDBContext();
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private UseFul use = new UseFul();
        private int sustraer = -1;
        private int intentos = 0;
        private List<CredencialSpiderFleet.Models.Itineraries.Itineraries> listRecursive = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
        private VariableConfiguration configuration = new VariableConfiguration();

        #region

        public async Task<StrokeResponse> LastTrip(string device, string hierarchy)
        {
            StrokeResponse response = new StrokeResponse();
            try
            {
                ItinerariesListLastResponse trip = ReadTripsRecursive(hierarchy, device);
                if(trip.listItineraries.Count > 0)
                {
                    foreach (var item in trip.listItineraries)
                    {
                        response = await ReadStrokeDeviceList(device, 
                            item.StartDate.ToString("yyyy-MM-ddTHH:mm:ssZ"), item.EndDate.ToString("yyyy-MM-ddTHH:mm:ssZ"), hierarchy);
                        response.success = true;
                    }
                }
                else
                {
                    response.success = false;
                }
            }
            catch (Exception ex)
            {
                response.success = false;                
                response.messages.Add(ex.Message);
            }
            return response;
        }

        private ItinerariesListLastResponse ReadTripsRecursive(string hierarchy, string device)
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
        private ItinerariesListLastResponse ReadTripsRecursive(ItinerariesListLastResponse resp, string hierarchy, string device, DateTime startdate, DateTime enddate)
        {

            ItinerariesListLastResponse response = new ItinerariesListLastResponse();

            try
            {
                //int diff = Convert.ToInt32((new SettingConfig()).ReadId(username, "ITE", 1));
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
                    if (resp.listItineraries.Count > 1)
                    {
                        foreach (CredencialSpiderFleet.Models.Itineraries.Itineraries x in resp.listItineraries)
                        {
                            if (listRecursive.Count < 1)
                            {
                                listRecursive.Add(x);
                                break;
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
                        if (listRecursive.Count < 1)
                        {
                            listRecursive.Add(x);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (listRecursive.Count < 1)
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
        private ItinerariesListLastResponse ReadItinerariosDeviceListRecursivo(ItinerariesListLastResponse resp, int diff, string device, DateTime startdate, DateTime enddate)
        {
            ItinerariesListResponse dataResponse = new ItinerariesListResponse();
            ItinerariesListLastResponse response = new ItinerariesListLastResponse();
            List<CredencialSpiderFleet.Models.Itineraries.Itineraries> listItineraries = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();

            try

            {
                int horas = VerifyUser.VerifyUser.GetHours();

                dataResponse.listItineraries = ItinerariesProcess(device, startdate, enddate, diff);

                if(dataResponse.listItineraries.Count > 0)
                {
                    foreach (List<CredencialSpiderFleet.Models.Itineraries.Itineraries> itineraries in dataResponse.listItineraries)
                    {
                        foreach (CredencialSpiderFleet.Models.Itineraries.Itineraries data in itineraries)
                        {
                            string calculoTime = UseFul.CalcularTime(Convert.ToInt32(data.Time));

                            data.Time = calculoTime + " Hrs";
                            data.ODO = data.ODO + " Kms";
                            data.Fuel = data.Fuel + " Lts";


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
                    
                    listItineraries = listItineraries.OrderByDescending(x => x.StartDate).ToList<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
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
        private List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> ItinerariesProcess(string device, DateTime startdate, DateTime enddate, int diff)
        {
            List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> listItineraries = new List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>>();
            List<CredencialSpiderFleet.Models.Itineraries.Itineraries> list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
            List<ItinerariesKey> listKey = new List<ItinerariesKey>();
            List<string> listData = new List<string>();

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


                    foreach (ItinerariesKey data in listKey)
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

                                        listItineraries.Add(list);
                                        list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
                                        fecha = string.Empty;
                                    }
                                }
                                else if (data.Event.Equals(START_TRIP))
                                {
                                    count = list.Count;
                                    //list[count - 1].EndDate = listKey[rows - 1].EndDate;

                                    milageFin = listKey[rows - 1].totalM;
                                    //list[count - 1].ODO = Convert.ToString(metrosKilometros(milageFin - milageIni));

                                    fuelFin = Convert.ToDouble(listKey[rows - 1].totalF);
                                    //list[count - 1].Fuel = Convert.ToString(litros(fuelFin - fuelIni));


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
                                        listItineraries.Add(list);
                                        list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
                                        fecha = string.Empty;
                                    }
                                }
                            }
                        }
                        rows++;
                    }
                }

                string valor = string.Empty;

                foreach (List<CredencialSpiderFleet.Models.Itineraries.Itineraries> dataList in listItineraries)
                {
                    foreach (CredencialSpiderFleet.Models.Itineraries.Itineraries itineraries in dataList)
                    {

                        TimeSpan span = (itineraries.EndDate - itineraries.StartDate);

                        int hours = span.Hours;
                        int minutes = span.Minutes;
                        int segundos = span.Seconds;

                        int totalSegundos = 0;

                        if (hours > 0)
                        {
                            totalSegundos = (hours * 60) * 60;
                        }


                        if (minutes > 0)
                        {
                            totalSegundos = totalSegundos + (minutes * 60);
                        }

                        totalSegundos = totalSegundos + segundos;
                        itineraries.Time = totalSegundos.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listItineraries;
        }
        private List<ItinerariesKey> GetDataProcess(string device, DateTime startdate, DateTime enddate, int diff)
        {
            List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> listItineraries = new List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>>();
            List<CredencialSpiderFleet.Models.Itineraries.Itineraries> list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
            List<ItinerariesKey> listKey = new List<ItinerariesKey>();

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
                    int count = 0;
                    foreach (GPS data in result)
                    {
                        ItinerariesKey keys = new ItinerariesKey();
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
                                    int rows = listKey.Count;
                                    listKey[rows - 1].Event = END_TRIP;
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
                            if (listKey.Count > 0)
                            {
                                int vals = UseFul.GetDiferenceDates(fechaAnterior, data.Date);
                                if (vals <= diff)
                                {
                                    keys.Event = START_TRIP;
                                }
                                else
                                {
                                    int rows = listKey.Count;
                                    listKey[rows - 1].Event = END_TRIP;
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

                        listKey.Add(keys);
                        count++;
                    }
                }

                if (listKey.Count > 0)
                {
                    int row = listKey.Count;
                    listKey[row - 1].Event = END_TRIP;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listKey;
        }

        public BotConsultsResponse ConsultsSpeeding(string device, string hierarchy)
        {
            BotConsultsResponse response = new BotConsultsResponse();
            try
            {
                string node = use.hierarchyPrincipalToken(hierarchy);
                int diff = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(node, "ITE", 1));

                DateTime now = DateTime.Today;
                var startdate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                var enddate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

                //var startdate = new DateTime(now.Year, now.Month, 8, 0, 0, 0);
                //var enddate = new DateTime(now.Year, now.Month, 9, 23, 59, 59);

                int horas = VerifyUser.VerifyUser.GetHours();
                startdate = startdate.AddHours(horas);
                enddate = enddate.AddHours(horas);

                var listTrip = GetDataProcess(device, startdate, enddate, diff);
                decimal maxVelocidad = Convert.ToDecimal((new SettingConfig()).ReadIdHerarchy(node, MXV, 1));

                response.Data = ExcesoVelocidad(listTrip, maxVelocidad);

                response.success = true;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
            }
            return response;
        }

        private int ExcesoVelocidad(List<ItinerariesKey> listKey, decimal maximaVelocidad)
        {
            int exceso = 0;
            try
            {
                List<int> count = new List<int>();
                Boolean band = false;
                DateTime fechaAnt = DateTime.Now;
                foreach (var item in listKey)
                {
                    if (Convert.ToDecimal(item.VelocidadMaxima) > maximaVelocidad)
                    {
                        if (!band)
                        {
                            fechaAnt = item.StartDate;
                            count.Add(0);
                            band = true;
                        }
                    }
                    else
                    {
                        if (band)
                        {
                            double secs = UseFul.diferenciaSeconds(item.StartDate, fechaAnt);
                            count.Add(Convert.ToInt32(secs.ToString()));
                            band = false;
                        }
                    }
                }

                exceso = count.Count(x => x > 30);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return exceso;
        }
        
        public BotConsultsResponse Consults(string device, string node, int type)
        {
            BotConsultsResponse response = new BotConsultsResponse();
            try
            {

                DateTime now = DateTime.Today;
                var startdate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                var enddate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
                int horas = VerifyUser.VerifyUser.GetHours();
                startdate = startdate.AddHours(horas);
                enddate = enddate.AddHours(horas);

                response.Data = GetAlarmasProcess(device, startdate, enddate, type);
                response.success = true;
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
            }
            return response;
        }

        private int GetAlarmasProcess(string device, DateTime startdate, DateTime enddate, int type)
        {
            int count = 0;

            try
            {
                string start = startdate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string end = enddate.ToString("yyyy-MM-ddTHH:mm:ssZ");

                BsonDocument bsonDocument = new BsonDocument();
                bsonDocument.Add("device", device);
                bsonDocument.Add("date", new BsonDocument("$gte", Convert.ToDateTime(start)).Add("$lte", Convert.ToDateTime(end)));

                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Alarms.Alarms>("Alarms");
                var result = stored.Find(build).Sort("{date:1}").ToList();

                if (result.Count > 0)
                {
                    if (type == 1)
                    {
                        count = result.Where(x => x.Alarmsn[0].Type.Equals("Hard Acceleration")).Count();
                    }
                    else if (type == 2)
                    {
                        count = result.Where(x => x.Alarmsn[0].Type.Equals("Hard Deceleration")).Count();
                    }
                    else if (type == 3)
                    {
                        count = result.Where(x => x.Alarmsn[0].Type.Equals("High RPM")).Count();
                    }
                    
                    //count = result.Count(x => x.Alarmsn[0].Type.Equals("Hard Acceleration"));
                    //count = result.Count(x => x.Alarmsn[0].Type.Equals("Hard Deceleration"));
                    //count = result.Count(x => x.Alarmsn[0].Type.Equals("High RPM"));

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return count;
        }
        #endregion

        public LastPositionResponse LastPosition(string device)
        {
            LastPosition position = new LastPosition();
            LastPositionResponse response = new LastPositionResponse();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_bot_last_position", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            position.Longitude = Convert.ToString(reader["longitude"]);
                            position.Latitude = Convert.ToString(reader["latitude"]);
                            
                        }

                        reader.Close();
                        response.position = position;
                        response.success = true;
                    }
                }
                else
                {
                    position = new LastPosition();
                    response.position = position;
                    response.success = false;
                }
            }
            catch (Exception ex)
            {
                response.messages.Add(ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }
            return response;
        }


        #region Ultima ruta
        public async Task<StrokeResponse> ReadStrokeDeviceList(string device, string startdate, string enddate, string hierarchy)
        {
            StrokeResponse response = new StrokeResponse();

            DateTime inicio = Convert.ToDateTime(startdate);
            DateTime fin = Convert.ToDateTime(enddate);

            int horas = VerifyUser.VerifyUser.GetHours();

            inicio = inicio.AddHours(horas);
            fin = fin.AddHours(horas);

            //string nombreVehiculo = string.Empty;
            //string nombreResponsable = string.Empty;
            //int tipoDevice = 0;
            //int maxVelocidad = 0;
            //ResponsibleVehicleResponse responsible = new ResponsibleVehicleResponse();
            //responsible = (new ResponsibleDao()).ReadVehicle(device);


            //if (!string.IsNullOrEmpty(responsible.responsible.Vehicle))
            //{
            //    nombreVehiculo = responsible.responsible.Vehicle;
            //    nombreResponsable = responsible.responsible.Responsible;
            //    tipoDevice = responsible.responsible.IdDongle;
            //}
            //else
            //{
            //    responsible = (new ResponsibleDao()).ReadNameVehicle(device);
            //    nombreVehiculo = responsible.responsible.Vehicle;
            //    tipoDevice = responsible.responsible.IdDongle;
            //}

            //response.VehicleName = string.IsNullOrEmpty(nombreVehiculo) ? device : nombreVehiculo;
            //response.ResponsibleName = string.IsNullOrEmpty(nombreResponsable) ? "" : nombreResponsable;
            //response.DeviceType = (tipoDevice == 0) ? 0 : tipoDevice;
            List<ItinerariesKey> listKey = new List<ItinerariesKey>();

            try
            {
                List<Points> listPoints = new List<Points>();
                List<Time> listTime = new List<Time>();
                List<WaitTime> ListWaitTime = new List<WaitTime>();
                List<Points> listSpeeding = new List<Points>();

                string node = use.hierarchyPrincipalToken(hierarchy);
                int diff = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(node, "ITE", 1));

                //listKey = GetDataProcess(device, inicio, fin, diff);
                listKey = GetDataProcess(device, inicio, fin, diff);

                //listPoints = PointsProcess(device, inicio, fin, diff);
                listPoints = PointsProcess(listKey);
                ///////////////////////////////////////////////
                List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> itineraries = ItinerariesProcess(listKey, device);

                //string consumeFuel = string.Empty;
                //string consumeOdo = string.Empty;
                //string consumeTime = string.Empty;
                //double totalDistanciaDouble = 0;

                //if (itineraries.Count > 0)
                //{
                //    consumeFuel = itineraries[0][0].Fuel + " Lts";
                //    consumeOdo = itineraries[0][0].ODO + " Kms";
                //    consumeTime = UseFul.CalcularTime(Convert.ToInt32(itineraries[0][0].Time)) + " Hrs";
                //    totalDistanciaDouble = itineraries[0][0].totalDistanciaDouble;

                //}
                //else
                //{
                //    consumeFuel = "0 Lts";
                //    consumeOdo = "0 Kms";
                //    consumeTime = UseFul.CalcularTime(0) + " Hrs";
                //}

                //response.FuelConsumption = consumeFuel;
                //response.OdoConsumption = consumeOdo;
                //response.ElapsedTime = consumeTime;
                //response.TotalDistanciaDouble = Math.Round(totalDistanciaDouble, 2);

                //DetailsRegistryResponse details = new DetailsRegistryResponse();
                //details = (new DetailsDao()).ReadId(device);

                //if (details.Registry.Performance != 0)
                //{
                //    double distancia = Math.Round(totalDistanciaDouble, 2);
                //    response.OdoConsumption = distancia + " Kms";

                //    double valida = 0;
                //    bool canConvert = double.TryParse(distancia.ToString(), out valida);

                //    if (canConvert)
                //    {
                //        double litros = distancia / details.Registry.Performance;
                //        response.FuelConsumption = Math.Round(litros, 2) + " Lts";
                //    }
                //    else
                //    {
                //        response.FuelConsumption = "0 Lts";
                //    }
                //}

                decimal maxVelocidad = Convert.ToDecimal((new SettingConfig()).ReadIdHerarchy(use.hierarchyPrincipalToken(hierarchy), MXV, 1));
                //ReportItinerarioResponse information = new ReportItinerarioResponse();
                //information = (new ReportItinerarioDao()).Read(device, inicio.AddHours(horas), fin.AddHours(horas), maxVelocidad);

                //int totalFrenado = 0;
                //int totalAceleracion = 0;
                //int totalVelocidad = 0;
                //int totalRPM = 0;

                //if (information.itinerarios.Count > 0)
                //{
                //    totalFrenado = information.itinerarios.Sum(x => x.Frenado);
                //    totalAceleracion = information.itinerarios.Sum(x => x.Aceleracion);
                //    totalVelocidad = information.itinerarios.Sum(x => x.Velocidad);
                //    totalRPM = information.itinerarios.Sum(x => x.RPM);
                //}

                //response.Braking = totalFrenado;
                //response.Acceleration = totalAceleracion;
                //response.Speed = totalVelocidad;
                //response.RPM = totalRPM;


                foreach (var time in listPoints)
                {

                    Time data = new Time();
                    time.Date = time.Date.AddHours(-horas);

                    string formatTime = time.Date.ToString("dd/MM/yyyy HH:mm:ss");
                    data.Date = formatTime;
                    data.events = time.events;
                    data.lat = time.lat;
                    data.lng = time.lng;
                    data.speed = Math.Round(Convert.ToDecimal(time.speed), 2).ToString();

                    listTime.Add(data);
                }

                response.listTime = listTime;


                ////Grafica
                //if (listTime.Count > 0)
                //{
                //    GraficaTiempoVelocidad grafica = new GraficaTiempoVelocidad();

                //    List<string> data = new List<string>();
                //    List<string> label = new List<string>();

                //    foreach (Time time in listTime)
                //    {
                //        data.Add(time.speed);
                //        label.Add(time.Date);
                //    }

                //    grafica.label = label;
                //    grafica.data = data;

                //    response.Grafica.data = new List<string>();
                //    response.Grafica.label = new List<string>();

                //    response.Grafica.data = data;
                //    response.Grafica.label = label;
                //    response.Grafica.MaximumSpeed = maxVelocidad.ToString();
                //}

                SnappedPoints snappedPoints = await (new ItinerariesDao()).getPathSnapAsync(listPoints);

                if (snappedPoints.snappedPoints.Count > 0)
                {
                    listPoints = new List<Points>();
                    Points points = new Points();
                    foreach (var data in snappedPoints.snappedPoints)
                    {
                        points = new Points();
                        points.Date = DateTime.Now;
                        points.events = "POINTS";
                        points.lat = data.location.latitude.ToString();
                        points.lng = data.location.longitude.ToString();

                        listPoints.Add(points);
                    }

                    listPoints[0].events = "START";
                    listPoints[listPoints.Count - 1].events = "END";
                    response.success = true;
                }

                response.listPoints = listPoints;

                //Alarmas


                inicio = inicio.AddHours(-horas);
                fin = fin.AddHours(-horas);
                //response.listIcons = AlarmsReport(device, inicio, fin);

                response.StartDate = inicio.ToString("dd/MM/yyyy HH:mm:ss");
                response.EndDate = fin.ToString("dd/MM/yyyy HH:mm:ss");

                //if (listTime.Count > 0)
                //{

                    //string respuesta = string.Empty;
                    //respuesta = (new SettingConfig()).ReadIdHerarchy(hierarchy, "DIR", 2);

                    //if (string.IsNullOrEmpty(respuesta))
                    //{
                    //    string parametro = use.nodePrincipal(hierarchy);
                    //    respuesta = (new SettingConfig()).ReadIdHerarchy(parametro, "DIR", 2);
                    //}

                    //string respuesta = (new SettingConfig()).ReadIdHerarchy(parametro, "DIR", 2);
                    //bool band = false;
                    //band = string.IsNullOrEmpty(respuesta) | respuesta.Equals("0") ? false : true;

                    //if (band)
                    //{
                    //    int last = listTime.Count;

                    //    AddressConsult addressIni = (new AddressDao()).GetAddress(device, inicio, listTime[0].lat, listTime[0].lng);
                    //    AddressConsult addressFin = (new AddressDao()).GetAddress(device, fin, listTime[last - 1].lat, listTime[last - 1].lng);
                    //    var addressI = string.Empty;
                    //    var addressF = string.Empty;

                    //    if (string.IsNullOrEmpty(addressIni.Address))
                    //    {
                    //        var urlStart = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + listTime[0].lat + "," +
                    //        listTime[0].lng + "&key=" + configuration.snap;
                    //        var resultStart = new WebClient().DownloadString(urlStart);
                    //        CredencialSpiderFleet.Models.ApiGoogle.GoogleGeoCodeResponse starting =
                    //            JsonConvert.DeserializeObject<CredencialSpiderFleet.Models.ApiGoogle.GoogleGeoCodeResponse>(resultStart);

                    //        addressI = UseFul.ToUTF8(starting.results[0].formatted_address.ToString());
                    //        response.messages.Add("Inserta Inicio");

                    //        CredencialSpiderFleet.Models.Address.Address direccion = new CredencialSpiderFleet.Models.Address.Address();
                    //        direccion.Device = device;
                    //        direccion.Date = inicio;
                    //        direccion.Point = addressI;
                    //        direccion.Latitude = listTime[0].lat;
                    //        direccion.Longitude = listTime[0].lng;

                    //        (new AddressDao()).Create(direccion);
                    //    }
                    //    else
                    //    {
                    //        response.messages.Add("Consulta Inicio");
                    //        addressI = addressIni.Address;
                    //    }

                    //    if (string.IsNullOrEmpty(addressFin.Address))
                    //    {
                    //        var urlFinal = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + listTime[last - 1].lat + "," +
                    //            listTime[last - 1].lng + "&key=" + configuration.snap;
                    //        var resultFinal = new WebClient().DownloadString(urlFinal);
                    //        CredencialSpiderFleet.Models.ApiGoogle.GoogleGeoCodeResponse final =
                    //            JsonConvert.DeserializeObject<CredencialSpiderFleet.Models.ApiGoogle.GoogleGeoCodeResponse>(resultFinal);
                    //        addressF = UseFul.ToUTF8(final.results[0].formatted_address.ToString());
                    //        response.messages.Add("Inserta Fin");

                    //        CredencialSpiderFleet.Models.Address.Address direccion = new CredencialSpiderFleet.Models.Address.Address();
                    //        direccion.Device = device;
                    //        direccion.Date = fin;
                    //        direccion.Point = addressF;
                    //        direccion.Latitude = listTime[last - 1].lat;
                    //        direccion.Longitude = listTime[last - 1].lng;

                    //        (new AddressDao()).Create(direccion);
                    //    }
                    //    else
                    //    {
                    //        response.messages.Add("Consulta Fin");
                    //        addressF = addressFin.Address;
                    //    }

                    //    response.StartingPoint = addressI;
                    //    response.FinalPoint = addressF;

                    //}
                
                //}

                int segundos = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(use.hierarchyPrincipalToken(hierarchy), WTC, 1));

                //var talvez = getWaitTime(listKey, segundos);

                //ListWaitTime = ReadAlarms(device, inicio, fin, segundos, maxVelocidad, response);
                //response.listWaitTime = talvez;// ListWaitTime;

                //response.listIcons = AlarmsProcess(device, inicio, fin);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                response.messages.Add(ex.Message);
            }

            return response;
        }

        private List<Points> PointsProcess(List<ItinerariesKey> listKey)
        {
            List<Points> listPoints = new List<Points>();

            try
            {
                int rows = 0;
                int count = 0;

                if (listKey.Count > 0)
                {
                    Points point = new Points();
                    foreach (ItinerariesKey data in listKey)
                    {

                        point = new Points();
                        if (listPoints.Count == 0)
                        {
                            point.events = START_TRIP;
                            point.Date = data.StartDate;
                            point.lng = data.Longitude;
                            point.lat = data.Latitude;
                            point.speed = data.VelocidadMaxima;
                            listPoints.Add(point);

                            point = new Points();
                            point.events = POINTS_TRIP;
                            point.Date = data.StartDate;
                            point.lng = data.Longitude;
                            point.lat = data.Latitude;
                            point.speed = data.VelocidadMaxima;
                            listPoints.Add(point);
                        }
                        else
                        {
                            point.events = POINTS_TRIP;
                            point.Date = data.StartDate;
                            point.lng = data.Longitude;
                            point.lat = data.Latitude;
                            point.speed = data.VelocidadMaxima;
                            listPoints.Add(point);
                        }

                        rows++;
                    }

                    point = new Points();
                    point.events = END_TRIP;
                    point.Date = listKey[listKey.Count - 1].StartDate;
                    point.lng = listKey[listKey.Count - 1].Longitude;
                    point.lat = listKey[listKey.Count - 1].Latitude;
                    point.speed = listKey[listKey.Count - 1].VelocidadMaxima;
                    listPoints.Add(point);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listPoints;
        }

        private List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> ItinerariesProcess(List<ItinerariesKey> listKey, string device)
        {
            List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> listItineraries = new List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>>();
            List<CredencialSpiderFleet.Models.Itineraries.Itineraries> list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
            List<string> listData = new List<string>();

            string VehicleName = string.Empty;
            string DriverData = string.Empty;
            string Image = string.Empty;

            try
            {
                int rows = 0;
                int count = 0;
                double calculoDistanciaDouble = 0;
                double calculoDistanciaDouble2 = 0;
                int calculoDistanciaInt = 0;

                if (listKey.Count > 0)
                {
                    string fecha = string.Empty;
                    int milageIni = 0;
                    int milageFin = 0;

                    double fuelIni = 0;
                    double fuelFin = 0;

                    bool bandera = false;

                    GeoCoordinate point1 = new GeoCoordinate();
                    GeoCoordinate point2 = new GeoCoordinate();

                    foreach (ItinerariesKey data in listKey)
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

                                        listItineraries.Add(list);
                                        list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
                                        fecha = string.Empty;
                                    }
                                }
                                else if (data.Event.Equals(START_TRIP))
                                {
                                    count = list.Count;

                                    milageFin = listKey[rows - 1].totalM;

                                    fuelFin = Convert.ToDouble(listKey[rows - 1].totalF);

                                    itineraries.Device = data.Device;
                                    itineraries.Batery = data.batery;
                                    itineraries.Label = data.label;
                                    itineraries.StartDate = data.StartDate;
                                    itineraries.EndDate = data.EndDate;

                                    itineraries.ODO =  Convert.ToString(use.metrosKilometros(milageFin - milageIni));
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
                                        listItineraries.Add(list);
                                        list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
                                        fecha = string.Empty;
                                    }
                                }
                            }
                        }


                        if (rows == 0)
                        {
                            point1 = new GeoCoordinate();
                            point1.Latitude = Convert.ToDouble(data.Latitude);
                            point1.Longitude = Convert.ToDouble(data.Longitude);
                        }
                        else
                        {
                            point2 = new GeoCoordinate();
                            point2.Latitude = Convert.ToDouble(data.Latitude);
                            point2.Longitude = Convert.ToDouble(data.Longitude);

                            calculoDistanciaDouble += UseFul.GetDistanceDouble(point1, point2);

                            point1 = new GeoCoordinate();
                            point1.Latitude = Convert.ToDouble(data.Latitude);
                            point1.Longitude = Convert.ToDouble(data.Longitude);
                        }
                        rows++;
                    }
                }

                string valor = string.Empty;

                foreach (List<CredencialSpiderFleet.Models.Itineraries.Itineraries> dataList in listItineraries)
                {
                    foreach (CredencialSpiderFleet.Models.Itineraries.Itineraries itineraries in dataList)
                    {

                        TimeSpan span = (itineraries.EndDate - itineraries.StartDate);

                        int hours = span.Hours;
                        int minutes = span.Minutes;
                        int segundos = span.Seconds;

                        int totalSegundos = 0;

                        if (hours > 0)
                        {
                            totalSegundos = (hours * 60) * 60;
                        }


                        if (minutes > 0)
                        {
                            totalSegundos = totalSegundos + (minutes * 60);
                        }

                        totalSegundos = totalSegundos + segundos;
                        itineraries.Time = totalSegundos.ToString();
                        itineraries.totalDistanciaDouble = calculoDistanciaDouble;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listItineraries;
        }


        #endregion


        #region KM

        public TotalResponse reporteGeneral(string hierarchy, string device, int type)
        {

            TotalResponse response = new TotalResponse();
            try
            {
                List<TravelReport> listTravelReport = new List<TravelReport>();

                int totalSegundos = 0;
                int totalMetros = 0;
                double totalConsumo = 0.0;

                DateTime utc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                DateTime now = TimeZoneInfo.ConvertTimeFromUtc(utc, cstZone);
                var startdate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                var enddate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

                int horas = VerifyUser.VerifyUser.GetHours();
                listTravelReport = Travels(hierarchy, device, startdate, enddate, horas);

                foreach (var data in listTravelReport)
                {
                    string tiempo = string.Empty;
                    tiempo = data.Time;

                    totalSegundos = totalSegundos + Convert.ToInt32((UseFul.CalcularTime(tiempo.Replace(" Hrs", ""))));

                    string distancia = string.Empty;
                    distancia = data.Distance.ToString();
                    totalMetros = totalMetros + (Convert.ToInt32(data.Distance));

                    double consumo = data.Distance / 10;

                    totalConsumo = totalConsumo + consumo;
                }

                //excelWorksheet.Cells[8, 4].Value = Convert.ToString(Math.Round(Convert.ToDouble(totalMetros) / Math.Round(totalConsumo, 2), 2)) + " Km/L";

                if(type == 1)
                {
                    response.Total = totalMetros + " Km";
                }
                else if (type == 2)
                {
                    response.Total = use.CalcularTiempo(totalSegundos) + " Hrs";
                }
                else if (type == 3)
                {
                    response.Total = Math.Round(totalConsumo, 2).ToString() + " Litros";
                }

                response.success = true;

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
            }

            return response;
        }

        private List<TravelReport> Travels(string hierarchy, string device, DateTime startdate, DateTime enddate, int horas)
        {
            List<TravelReport> listTravelReport = new List<TravelReport>();
            try
            {
                ItinerariesResponse response = new ItinerariesResponse();
                response = (new ItinerariesDao()).ReadItinerariosDeviceList(hierarchy, device, startdate, enddate);
                List<CredencialSpiderFleet.Models.Itineraries.Itineraries> listItineraries = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();

                if (response.listItineraries.Count > 0)
                {
                    listItineraries = response.listItineraries;
                    int count = listItineraries.Count;

                    string nombreVehiculo = string.Empty;
                    string nombreResponsable = string.Empty;
                    int tipoDevice = 0;

                    foreach (var item in listItineraries)
                    {
                        TravelReport report = new TravelReport();

                        StrokeResponse link = new StrokeResponse();
                        link = ReadStrokeDeviceList(device
                            , item.StartDate.ToString("yyyy-MM-dd HH:mm:ss"), item.EndDate.ToString("yyyy-MM-dd HH:mm:ss")
                            , hierarchy, horas, nombreVehiculo, nombreResponsable, tipoDevice);

                        report.Number = count;

                        report.TravelDate = item.StartDate;
                        report.StartDate = item.StartDate;
                        report.EndDate = item.EndDate;
                        report.Time = link.ElapsedTime;
                        report.Speeding = link.Speed;

                        report.Distance = link.TotalDistanciaDouble;
                        report.Consumption = link.FuelConsumption;
                        report.Responsable = link.ResponsibleName;
                        report.Latitud = link.Latitude;
                        report.Longitud = link.Longitude;

                        listTravelReport.Add(report);

                        count--;
                    }

                    listTravelReport = listTravelReport.OrderBy(x => x.Number).ToList<TravelReport>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listTravelReport;
        }

        public StrokeResponse ReadStrokeDeviceList(string device, string startdate, string enddate, string hierarchy, int horas,
           string nombreVehiculo, string nombreResponsable, int tipoDevice)
        {
            StrokeResponse response = new StrokeResponse();

            DateTime inicio = Convert.ToDateTime(startdate);
            DateTime fin = Convert.ToDateTime(enddate);

            inicio = inicio.AddHours(horas);
            fin = fin.AddHours(horas);

            response.VehicleName = string.IsNullOrEmpty(nombreVehiculo) ? device : nombreVehiculo;
            response.ResponsibleName = string.IsNullOrEmpty(nombreResponsable) ? "" : nombreResponsable;
            response.DeviceType = (tipoDevice == 0) ? 0 : tipoDevice;

            try
            {
                string node = use.hierarchyPrincipalToken(hierarchy);
                int diff = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(node, "ITE", 1));

                List<ItinerariesKey> listKey = new List<ItinerariesKey>();
                listKey = GetDataProcess(device, inicio, fin, diff);

                List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> itineraries = ItinerariesProcess(listKey, device);

                string consumeFuel = string.Empty;
                string consumeOdo = string.Empty;
                string consumeTime = string.Empty;
                double totalDistanciaDouble = 0;
                string latitud = string.Empty;
                string longitud = string.Empty;

                if (itineraries.Count > 0)
                {
                    consumeFuel = itineraries[0][0].Fuel + " Lts";
                    consumeOdo = itineraries[0][0].ODO + " Kms";
                    consumeTime = UseFul.CalcularTime(Convert.ToInt32(itineraries[0][0].Time)) + " Hrs";
                    totalDistanciaDouble = itineraries[0][0].totalDistanciaDouble;
                    latitud = listKey[0].Latitude;
                    longitud = listKey[0].Longitude;

                }
                else
                {
                    consumeFuel = "0 Lts";
                    consumeOdo = "0 Kms";
                    consumeTime = UseFul.CalcularTime(0) + " Hrs";
                    latitud = "0";
                    longitud = "0";
                }

                response.FuelConsumption = consumeFuel;
                response.OdoConsumption = consumeOdo;
                response.ElapsedTime = consumeTime;
                response.TotalDistanciaDouble = Math.Round(totalDistanciaDouble, 2);
                response.Latitude = latitud;
                response.Longitude = longitud;

                DetailsRegistryResponse details = new DetailsRegistryResponse();
                details = (new DetailsDao()).ReadId(device);

                if (details.Registry.Performance != 0)
                {
                    double distancia = Math.Round(totalDistanciaDouble, 2);
                    response.OdoConsumption = distancia + " Kms";

                    double valida = 0;
                    bool canConvert = double.TryParse(distancia.ToString(), out valida);

                    if (canConvert)
                    {
                        double litros = details.Registry.Performance == 0 ? 0 : distancia / details.Registry.Performance;
                        response.FuelConsumption = Math.Round(litros, 2) + " Lts";
                    }
                    else
                    {
                        response.FuelConsumption = "0 Lts";
                    }
                }

                decimal maxVelocidad = Convert.ToDecimal((new SettingConfig()).ReadIdHerarchy(use.hierarchyPrincipalToken(hierarchy), MXV, 1));

                response.Speed = ExcesoVelocidad(listKey, maxVelocidad); ;

                inicio = inicio.AddHours(-horas);
                fin = fin.AddHours(-horas);

                response.StartDate = inicio.ToString("dd/MM/yyyy HH:mm:ss");
                response.EndDate = fin.ToString("dd/MM/yyyy HH:mm:ss");

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                response.messages.Add(ex.Message);
            }

            return response;
        }
        #endregion

        #region Gasolina

        #endregion
    }
}