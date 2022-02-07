using CredencialSpiderFleet.Models.Configuration;
using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Itineraries;
using CredencialSpiderFleet.Models.SnappedPoints;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.GPS;
using SpiderFleetWebAPI.Models.Response.Itineraries;
using SpiderFleetWebAPI.Utils.Setting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.Responsible;
using SpiderFleetWebAPI.Utils.Responsible;
using SpiderFleetWebAPI.Models.Response.AddtionalVehicleData;
using SpiderFleetWebAPI.Utils.AddtionalVehicleData;
using System.Globalization;
using SpiderFleetWebAPI.Models.Response.ReportAdmin;
using SpiderFleetWebAPI.Utils.ReportAdmin;
using CredencialSpiderFleet.Models.Logical;
using SpiderFleetWebAPI.Models.Response.Details;
using SpiderFleetWebAPI.Utils.Details;
using SpiderFleetWebAPI.Utils.Address;
using CredencialSpiderFleet.Models.Address;

namespace SpiderFleetWebAPI.Utils.Itineraries
{
    public class ItinerariesDao
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private UseFul use = new UseFul();

        private MongoDBContext mongoDBContext = new MongoDBContext();
        private const string start = "START";
        private const string end = "END";
        private const string points = "POINTS";
        private VariableConfiguration configuration = new VariableConfiguration();
        private int sustraer = -1;
        private List<CredencialSpiderFleet.Models.Itineraries.Itineraries> listRecursive = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
        private int intentos = 0;
        private const string MXV = "MXV";
        private const string WTC = "WTC";

        public ItinerariesGeneralResponse ReadGeneralDeviceList(string username, string device)//, DateTime startdate, DateTime enddate)
        {

            CredencialSpiderFleet.Models.Itineraries.Itineraries general = new CredencialSpiderFleet.Models.Itineraries.Itineraries();
            ItinerariesGeneralResponse response = new ItinerariesGeneralResponse();
            ItinerariesListResponse data = new ItinerariesListResponse();

            DateTime utc = DateTime.UtcNow;

            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(utc, cstZone);

            var startdate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var enddate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            DateTime start = now;
            DateTime end = enddate;
            int horas = VerifyUser.VerifyUser.GetHours();

            startdate = startdate.AddHours(horas);
            enddate = enddate.AddHours(horas);

            decimal ODO = 0;
            decimal Fuel = 0;
            double Time = 0;
            int respuesta = 0;

            try
            {
                string node = use.hierarchyPrincipalToken(username);
                int diff = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(node, "ITE", 1));
                

                //int diff = Convert.ToInt32((new SettingConfig()).ReadId(username, "ITE", 1));
                data.listItineraries = ItinerariesProcess(device, startdate, enddate, diff);

                string driverData = string.Empty;
                int typeDevice = 0;
                int motor = 0;
                string hierarchy = string.Empty;
                ResponsibleVehicleResponse responsible = new ResponsibleVehicleResponse();

                responsible = (new ResponsibleDao()).ReadNameVehicle(device, start, end);
                if (!string.IsNullOrEmpty(responsible.responsible.Vehicle))
                {
                    driverData = responsible.responsible.Responsible;
                    typeDevice = responsible.responsible.IdDongle;
                    motor = responsible.responsible.Motor;
                    hierarchy = responsible.responsible.Hierarchy;
                }
                else
                {
                    driverData = device;
                }

                //Datos Adicionales del vehiculo
                AddtionalVehicleDataRegistryResponse addtional = new AddtionalVehicleDataRegistryResponse();
                addtional = (new AddtionalVehicleDataDao()).ReadId(device);

                if (data.listItineraries.Count > 0)
                {
                    foreach (List<CredencialSpiderFleet.Models.Itineraries.Itineraries> list in data.listItineraries)
                    {
                        foreach (CredencialSpiderFleet.Models.Itineraries.Itineraries info in list)
                        {

                            general.VehicleName = info.VehicleName;
                            general.Device = info.Device;
                            general.DriverData = driverData;// info.DriverData;
                            general.Score = info.Score;
                            general.StartDate = startdate.AddHours(-horas);
                            general.EndDate = enddate.AddHours(-horas);
                            general.Image = info.Image;

                            ODO = ODO + Convert.ToDecimal(info.ODO.Equals("") ? "0" : info.ODO);
                            Fuel = Fuel + Convert.ToDecimal(info.Fuel.Equals("") ? "0" : info.Fuel);

                            respuesta = respuesta + Convert.ToInt32(info.Time);
                        }
                    }

                    general.Time = UseFul.CalcularTime(respuesta) + " Horas";
                    general.ODO = ODO.ToString() + " Km";
                    general.Fuel = Fuel.ToString() + " Litros";
                }
                else
                {
                    general = new CredencialSpiderFleet.Models.Itineraries.Itineraries();
                    List<string> listData = new List<string>();

                    string VehicleName = string.Empty;
                    string DriverData = string.Empty;
                    string Image = string.Empty;

                    listData = ReadOperatorData(device);
                    if (listData.Count > 0)
                    {
                        DriverData = listData[0];
                        Image = listData[1];
                        VehicleName = listData[2];
                    }

                    general.VehicleName = VehicleName;
                    general.Device = device;
                    general.DriverData = driverData;//  ;
                    general.Score = "";
                    general.StartDate = startdate;
                    general.EndDate = enddate;
                    general.Image = Image;

                    general.Time = "00:00:00 HH:mm:ss";
                    general.ODO = "0 Km";
                    general.Fuel = "0.00 Litros";
                }

                general.EngineStop = motor;
                general.TypeDevice = typeDevice;
                general.Hierarchy = hierarchy;
                general.Marca = string.IsNullOrEmpty(addtional.addtional.Marca) ? "" : addtional.addtional.Marca;
                general.Modelo = string.IsNullOrEmpty(addtional.addtional.Modelo) ? "" : addtional.addtional.Modelo;
                general.Version = string.IsNullOrEmpty(addtional.addtional.Version) ? "" : addtional.addtional.Version;
                general.TipoVehiculo = string.IsNullOrEmpty(addtional.addtional.TipoVehiculo) ? "" : addtional.addtional.TipoVehiculo;
                general.VIN = string.IsNullOrEmpty(addtional.addtional.VIN) ? "" : addtional.addtional.VIN;
                general.Placas = string.IsNullOrEmpty(addtional.addtional.Placas) ? "" : addtional.addtional.Placas;
                general.Poliza = string.IsNullOrEmpty(addtional.addtional.Poliza) ? "" : addtional.addtional.Poliza;
                if (data.listItineraries.Count > 0)
                {
                    int count = data.listItineraries.Count;
                    if(data.listItineraries[count - 1][0].Batery.Equals(""))
                    {
                        general.Batery = string.Empty;
                        general.Label = string.Empty;
                    }
                    else
                    {
                        general.Label = data.listItineraries[count - 1][0].Label;
                        general.Batery = data.listItineraries[count - 1][0].Batery;
                    }                    
                }
                else
                {
                    general.Batery = "";
                }

                response.lastAlarms = ReadLastAlarms(device, response);

                response.Itineraries = general;
                response.success = true;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                response.messages.Add(ex.Message);
            }

            return response;
        }

        private List<LastAlarms> ReadLastAlarms(string device, ItinerariesGeneralResponse response)
        {
            List<LastAlarms> value = new List<LastAlarms>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_last_alarmas", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LastAlarms wait = new LastAlarms();
                            wait.Alarm = GetAlarm(Convert.ToString(reader["alarm"]));
                            wait.Date = Convert.ToDateTime(reader["date"]);
                            wait.Latitude = Convert.ToString(reader["latitude"]);
                            wait.Longitude = Convert.ToString(reader["longitude"]);
                            value.Add(wait);
                        }

                        reader.Close();
                    }
                }
                else
                {
                    value = new List<LastAlarms>();
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
            return value;
        }

        private string CalcularTiempo(Int32 tsegundos)
        {
            int horas = (tsegundos / 3600);
            int minutos = ((tsegundos - horas * 3600) / 60);
            int segundos = tsegundos - (horas * 3600 + minutos * 60);

            string resultHoras = (horas < 10) ? ("0" + horas) : horas.ToString();
            string resultMin = (minutos < 10) ? ("0" + minutos) : minutos.ToString();
            string resultSeg = (segundos < 10) ? ("0" + segundos) : segundos.ToString();

            return resultHoras + ":" + resultMin + ":" + resultSeg;
        }

        #region Viajes

        public ItinerariesListLastResponse ReadTripsRecursive(ItinerariesListLastResponse resp, string hierarchy, string device)
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

                response = (new ItinerariesDao()).ReadTripsRecursive(response, hierarchy, device, startdate, enddate);

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

        public ItinerariesResponse ReadItinerariosDeviceList(string hierarchy, string device, DateTime startdate, DateTime enddate)
        {
            ItinerariesResponse response = new ItinerariesResponse();

            int horas = VerifyUser.VerifyUser.GetHours();
            response.messages.Add(" Horas " + horas);

            DateTime now = enddate;
            enddate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            startdate = startdate.AddHours(horas);
            enddate = enddate.AddHours(horas);

            try
            {               
                //int diff = Convert.ToInt32((new SettingConfig()).ReadId(username, "ITE", 1));
                string node = use.hierarchyPrincipalToken(hierarchy);
                int diff = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(node, "ITE", 1));

                List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> listTrips = ItinerariesProcess(device, startdate, enddate, diff);
                //listTrips = ItinerariesProcess(device, startdate, enddate, diff);

                List<CredencialSpiderFleet.Models.Itineraries.Itineraries> listItineraries = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();

                if (listTrips.Count > 0)
                {
                    foreach (List<CredencialSpiderFleet.Models.Itineraries.Itineraries> list in listTrips)
                    {
                        foreach (CredencialSpiderFleet.Models.Itineraries.Itineraries itineraries in list)
                        {
                            string values = string.Empty;

                            itineraries.StartDate = itineraries.StartDate.AddHours(-horas);
                            itineraries.StartHour = itineraries.StartDate.ToString("HH:mm:ss");
                            itineraries.EndDate = itineraries.EndDate.AddHours(-horas);
                            itineraries.EndHour = itineraries.EndDate.ToString("HH:mm:ss");
                            CultureInfo ci = new CultureInfo("es-MX");
                            ci = new CultureInfo("es-MX");
                            itineraries.TravelDate = new CultureInfo("es-MX", true).TextInfo.ToTitleCase(itineraries.EndDate.ToString("dddd, dd MMMM yyyy", ci));

                            itineraries.Time = UseFul.CalcularTime(string.IsNullOrEmpty(itineraries.Time) ? 0 : Convert.ToInt32(itineraries.Time)) + " Hrs";
                            itineraries.ODO = itineraries.ODO + " Kms";
                            itineraries.Fuel = itineraries.Fuel + " Lts";

                            listItineraries.Add(itineraries);
                        }
                    }

                    //response.listItineraries = listItineraries.OrderByDescending(x => x.StartDate).ToList();
                    //response.listItineraries = listItineraries.OrderBy(x => x.StartDate).OrderBy(x => x.StartHour).ToList();
                    
                    //List<CredencialSpiderFleet.Models.Itineraries.Itineraries> sort = (from user in listItineraries orderby user.StartDate descending select user).ToList();
                    //response.listItineraries = sort;

                    response.listItineraries = listItineraries.OrderBy(x => x.StartDate).Reverse().ToList();
                    response.success = true;
                }
                else
                {
                    response.success = false;
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

        public ItinerariesResponse ReadItinerariosList(string hierarchy, string device, DateTime startdate, DateTime enddate)
        {
            ItinerariesResponse response = new ItinerariesResponse();

            int horas = VerifyUser.VerifyUser.GetHours();
            response.messages.Add(" Horas " + horas);

            startdate = startdate.AddHours(horas);
            enddate = enddate.AddHours(horas);

            try
            {
                //int diff = Convert.ToInt32((new SettingConfig()).ReadId(username, "ITE", 1));
                string node = use.hierarchyPrincipalToken(hierarchy);
                int diff = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(node, "ITE", 1));

                

                List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> listTrips = ItinerariesProcess(device, startdate, enddate, diff);

                List<CredencialSpiderFleet.Models.Itineraries.Itineraries> listItineraries = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();

                if (listTrips.Count > 0)
                {
                    foreach (List<CredencialSpiderFleet.Models.Itineraries.Itineraries> list in listTrips)
                    {
                        foreach (CredencialSpiderFleet.Models.Itineraries.Itineraries itineraries in list)
                        {
                            string values = string.Empty;

                            itineraries.StartDate = itineraries.StartDate.AddHours(-horas);
                            itineraries.StartHour = itineraries.StartDate.ToString("HH:mm:ss");
                            itineraries.EndDate = itineraries.EndDate.AddHours(-horas);
                            itineraries.EndHour = itineraries.EndDate.ToString("HH:mm:ss");
                            CultureInfo ci = new CultureInfo("es-MX");
                            ci = new CultureInfo("es-MX");
                            itineraries.TravelDate = new CultureInfo("es-MX", true).TextInfo.ToTitleCase(itineraries.EndDate.ToString("dddd, dd MMMM yyyy", ci));

                            itineraries.Time = UseFul.CalcularTime(string.IsNullOrEmpty(itineraries.Time) ? 0 : Convert.ToInt32(itineraries.Time)) + " Hrs";
                            itineraries.ODO = itineraries.ODO + " Kms";
                            itineraries.Fuel = itineraries.Fuel + " Lts";

                            listItineraries.Add(itineraries);
                        }
                    }

                    response.listItineraries = listItineraries.ToList();
                    response.success = true;
                }
                else
                {
                    response.success = false;
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

        public List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> ItinerariesProcess(string device, DateTime startdate, DateTime enddate, int diff)
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
                listData = ReadOperatorData(device);
                if (listData.Count > 0)
                {
                    DriverData = listData[0];
                    Image = listData[1];
                    VehicleName = listData[2];
                }

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
                        if (data.Event.Equals(ItinerariesDao.start))
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
                                if (data.Event.Equals(ItinerariesDao.end)) 
                                {
                                    if(list.Count > 0)
                                    {
                                        count = list.Count;
                                        list[count - 1].EndDate = data.EndDate;

                                        milageFin = data.totalM;
                                        list[count - 1].ODO = Convert.ToString(metrosKilometros(milageFin - milageIni));

                                        fuelFin = Convert.ToDouble(data.totalF);
                                        list[count - 1].Fuel = Convert.ToString(litros(fuelFin - fuelIni));

                                        listItineraries.Add(list);
                                        list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
                                        fecha = string.Empty;
                                    }                                    
                                }
                                else if (data.Event.Equals(ItinerariesDao.start))
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

                                    itineraries.ODO = Convert.ToString(metrosKilometros(milageFin - milageIni));
                                    itineraries.Fuel = Convert.ToString(litros(fuelFin - fuelIni));
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
                                if (data.Event.Equals(ItinerariesDao.end))
                                {
                                    if(listItineraries.Count > 0)
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

                        if(hours > 0)
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

        //Modificaicon calculo de tiempo-gasolina-km
        public List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> ItinerariesProcess(List<ItinerariesKey> listKey, string device)
        {
            List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> listItineraries = new List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>>();
            List<CredencialSpiderFleet.Models.Itineraries.Itineraries> list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
            List<string> listData = new List<string>();

            string VehicleName = string.Empty;
            string DriverData = string.Empty;
            string Image = string.Empty;

            try
            {
                listData = ReadOperatorData(device);
                //if (listData.Count > 0)
                //{
                //    DriverData = listData[0];
                //    Image = listData[1];
                //    VehicleName = listData[2];
                //}

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
                        if (data.Event.Equals(ItinerariesDao.start))
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
                                if (data.Event.Equals(ItinerariesDao.end))
                                {
                                    if (list.Count > 0)
                                    {
                                        count = list.Count;
                                        list[count - 1].EndDate = data.EndDate;

                                        milageFin = data.totalM;
                                        list[count - 1].ODO = Convert.ToString(metrosKilometros(milageFin - milageIni));

                                        fuelFin = Convert.ToDouble(data.totalF);
                                        list[count - 1].Fuel = Convert.ToString(litros(fuelFin - fuelIni));

                                        listItineraries.Add(list);
                                        list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
                                        fecha = string.Empty;
                                    }
                                }
                                else if (data.Event.Equals(ItinerariesDao.start))
                                {
                                    count = list.Count;

                                    milageFin = listKey[rows - 1].totalM;

                                    fuelFin = Convert.ToDouble(listKey[rows - 1].totalF);

                                    itineraries.Device = data.Device;
                                    itineraries.Batery = data.batery;
                                    itineraries.Label = data.label;
                                    itineraries.StartDate = data.StartDate;
                                    itineraries.EndDate = data.EndDate;

                                    itineraries.ODO = Convert.ToString(metrosKilometros(milageFin - milageIni));
                                    itineraries.Fuel = Convert.ToString(litros(fuelFin - fuelIni));
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
                                if (data.Event.Equals(ItinerariesDao.end))
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


                        if(rows == 0)
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

        private string metrosKilometros(int metros)
        {
            string respuesta = string.Empty;
            double value = Math.Round((metros / 1000.0), 2, MidpointRounding.ToEven);
            return respuesta = value.ToString();                
        }

        private double litros(double litros)
        {
            string respuesta = string.Empty;
            double value = Math.Round(litros, 2, MidpointRounding.ToEven);
            return value;
        }

        #endregion

        #region Rutas

        public async Task<StrokeResponse> ReadStrokeDeviceList(string device, string startdate, string enddate, string hierarchy)
        {
            StrokeResponse response = new StrokeResponse();

            DateTime inicio = Convert.ToDateTime(startdate);
            DateTime fin = Convert.ToDateTime(enddate);

            int horas = VerifyUser.VerifyUser.GetHours();

            response.messages.Add("Horas servidor :: " + horas);


            inicio = inicio.AddHours(horas);
            fin = fin.AddHours(horas);
            
            string nombreVehiculo = string.Empty;
            string nombreResponsable = string.Empty;
            int tipoDevice = 0;
            //int maxVelocidad = 0;
            ResponsibleVehicleResponse responsible = new ResponsibleVehicleResponse();
            responsible = (new ResponsibleDao()).ReadVehicle(device);


            if (!string.IsNullOrEmpty(responsible.responsible.Vehicle))
            {
                nombreVehiculo = responsible.responsible.Vehicle;
                nombreResponsable = responsible.responsible.Responsible;
                tipoDevice = responsible.responsible.IdDongle;
            }
            else
            {
                responsible = (new ResponsibleDao()).ReadNameVehicle(device);
                nombreVehiculo = responsible.responsible.Vehicle;
                tipoDevice = responsible.responsible.IdDongle;
            }

            response.VehicleName = string.IsNullOrEmpty(nombreVehiculo) ? device : nombreVehiculo;
            response.ResponsibleName = string.IsNullOrEmpty(nombreResponsable) ? "" : nombreResponsable;
            response.DeviceType = (tipoDevice == 0) ? 0 : tipoDevice;
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

                string consumeFuel = string.Empty;
                string consumeOdo = string.Empty;
                string consumeTime = string.Empty;
                double totalDistanciaDouble = 0;

                if (itineraries.Count > 0 )
                {
                    consumeFuel = itineraries[0][0].Fuel + " Lts";
                    consumeOdo = itineraries[0][0].ODO + " Kms";
                    consumeTime = UseFul.CalcularTime(Convert.ToInt32(itineraries[0][0].Time)) + " Hrs";
                    totalDistanciaDouble = itineraries[0][0].totalDistanciaDouble;

                }
                else
                {
                    consumeFuel = "0 Lts";
                    consumeOdo = "0 Kms";
                    consumeTime = UseFul.CalcularTime(0) + " Hrs";
                }

                response.FuelConsumption = consumeFuel;
                response.OdoConsumption = consumeOdo;
                response.ElapsedTime = consumeTime;
                response.TotalDistanciaDouble = Math.Round(totalDistanciaDouble, 2);

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
                        double litros = distancia / details.Registry.Performance;
                        response.FuelConsumption = Math.Round(litros, 2) + " Lts";
                    }
                    else
                    {
                        response.FuelConsumption = "0 Lts";
                    }
                }

                decimal maxVelocidad = Convert.ToDecimal((new SettingConfig()).ReadIdHerarchy(use.hierarchyPrincipalToken(hierarchy), MXV, 1));
                ReportItinerarioResponse information = new ReportItinerarioResponse();
                information = (new ReportItinerarioDao()).Read(device, inicio.AddHours(horas), fin.AddHours(horas), maxVelocidad);

                int totalFrenado = 0;
                int totalAceleracion = 0;
                int totalVelocidad = 0;
                int totalRPM = 0;

                if(information.itinerarios.Count > 0)
                {
                    totalFrenado = information.itinerarios.Sum(x => x.Frenado);
                    totalAceleracion = information.itinerarios.Sum(x => x.Aceleracion);
                    totalVelocidad = information.itinerarios.Sum(x => x.Velocidad);
                    totalRPM = information.itinerarios.Sum(x => x.RPM);
                }
                
                response.Braking = totalFrenado;
                response.Acceleration = totalAceleracion;
                response.Speed = totalVelocidad;
                response.RPM = totalRPM;


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


                //Grafica
                if(listTime.Count > 0)
                {
                    GraficaTiempoVelocidad grafica = new GraficaTiempoVelocidad();

                    List<string> data = new List<string>();
                    List<string> label = new List<string>();

                    foreach (Time time in listTime)
                    {
                        data.Add(time.speed);
                        label.Add(time.Date);
                    }

                    grafica.label = label;
                    grafica.data = data;

                    response.Grafica.data = new List<string>();
                    response.Grafica.label = new List<string>();

                    response.Grafica.data = data;
                    response.Grafica.label = label;
                    response.Grafica.MaximumSpeed = maxVelocidad.ToString();
                }

                SnappedPoints snappedPoints = await getPathSnapAsync(listPoints);

                if(snappedPoints.snappedPoints.Count > 0)
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
                response.listIcons = AlarmsReport(device, inicio, fin);

                response.StartDate = inicio.ToString("dd/MM/yyyy HH:mm:ss");
                response.EndDate = fin.ToString("dd/MM/yyyy HH:mm:ss");

                if (listTime.Count > 0)
                {

                    string respuesta = string.Empty;
                    respuesta = (new SettingConfig()).ReadIdHerarchy(hierarchy, "DIR", 2);

                    if (string.IsNullOrEmpty(respuesta))
                    {
                        string parametro = use.nodePrincipal(hierarchy);
                        respuesta = (new SettingConfig()).ReadIdHerarchy(parametro, "DIR", 2);
                    }
                    
                    //string respuesta = (new SettingConfig()).ReadIdHerarchy(parametro, "DIR", 2);
                    bool band = false; 

                    band = string.IsNullOrEmpty(respuesta) | respuesta.Equals("0") ? false : true;

                    if (band)
                    {
                        int last = listTime.Count;

                        AddressConsult addressIni = (new AddressDao()).GetAddress(device, inicio, listTime[0].lat, listTime[0].lng);
                        AddressConsult addressFin = (new AddressDao()).GetAddress(device, fin, listTime[last - 1].lat, listTime[last - 1].lng);
                        var addressI = string.Empty;
                        var addressF = string.Empty;

                        if (string.IsNullOrEmpty(addressIni.Address))
                        {                            
                            var urlStart = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + listTime[0].lat + "," +
                            listTime[0].lng + "&key=" + configuration.snap;
                            var resultStart = new WebClient().DownloadString(urlStart);
                            CredencialSpiderFleet.Models.ApiGoogle.GoogleGeoCodeResponse starting =
                                JsonConvert.DeserializeObject<CredencialSpiderFleet.Models.ApiGoogle.GoogleGeoCodeResponse>(resultStart);

                            addressI = UseFul.ToUTF8(starting.results[0].formatted_address.ToString());
                            response.messages.Add("Inserta Inicio");

                            CredencialSpiderFleet.Models.Address.Address direccion = new CredencialSpiderFleet.Models.Address.Address();
                            direccion.Device = device;
                            direccion.Date = inicio;
                            direccion.Point = addressI;
                            direccion.Latitude = listTime[0].lat;
                            direccion.Longitude = listTime[0].lng;

                            (new AddressDao()).Create(direccion);
                        }
                        else
                        {
                            response.messages.Add("Consulta Inicio");
                            addressI = addressIni.Address;
                        }

                        if (string.IsNullOrEmpty(addressFin.Address))
                        {
                            var urlFinal = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + listTime[last - 1].lat + "," +
                                listTime[last - 1].lng + "&key=" + configuration.snap;
                            var resultFinal = new WebClient().DownloadString(urlFinal);
                            CredencialSpiderFleet.Models.ApiGoogle.GoogleGeoCodeResponse final =
                                JsonConvert.DeserializeObject<CredencialSpiderFleet.Models.ApiGoogle.GoogleGeoCodeResponse>(resultFinal);
                            addressF = UseFul.ToUTF8(final.results[0].formatted_address.ToString());
                            response.messages.Add("Inserta Fin");

                            CredencialSpiderFleet.Models.Address.Address direccion = new CredencialSpiderFleet.Models.Address.Address();
                            direccion.Device = device;
                            direccion.Date = fin;
                            direccion.Point = addressF;
                            direccion.Latitude = listTime[last - 1].lat;
                            direccion.Longitude = listTime[last - 1].lng;

                            (new AddressDao()).Create(direccion);
                        }
                        else
                        {
                            response.messages.Add("Consulta Fin");
                            addressF = addressFin.Address;
                        }
                            
                        response.StartingPoint = addressI;
                        response.FinalPoint = addressF;

                    }
                }
                
                int segundos = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(use.hierarchyPrincipalToken(hierarchy), WTC, 1));

                var talvez =  getWaitTime(listKey, segundos);

                //ListWaitTime = ReadAlarms(device, inicio, fin, segundos, maxVelocidad, response);
                response.listWaitTime = talvez;// ListWaitTime;

                response.listIcons = AlarmsProcess(device, inicio, fin);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add($"Falla en el modulo: " + MethodBase.GetCurrentMethod().Name);
                response.messages.Add(ex.Message);
            }

            return response;
        }

        private List<WaitTime> getWaitTime(List<ItinerariesKey> listKey, int segundos)
        {
            List<WaitTime> value = new List<WaitTime>();
            try
            {
                Boolean band = false;
                DateTime fechaAnterior = DateTime.Now;
                foreach (var item in listKey)
                {
                    if(Convert.ToDecimal(item.VelocidadMaxima) == 0)
                    {
                        if(!band)
                        {
                            band = true;
                            fechaAnterior = item.StartDate;
                        }
                    }
                    else
                    {
                        if(band)
                        {
                            double diferencia = UseFul.diferenciaSeconds(item.StartDate, fechaAnterior);
                            WaitTime wait = new WaitTime();
                            wait.events = "WT";
                            wait.Date = item.StartDate;
                            wait.lat = item.Latitude;
                            wait.lng = item.Longitude;
                            wait.time = UseFul.CalcularTime(Convert.ToInt32(diferencia.ToString()));
                            wait.wastedTime = Convert.ToInt32(diferencia.ToString());

                            if (Convert.ToInt32(diferencia.ToString()) > 30)
                            {
                                value.Add(wait);
                            }                          

                            band = false;
                        }
                    }
                }

                return value;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<WaitTime> ReadAlarms(string device, DateTime start, DateTime end, int time, decimal speed, StrokeResponse response)
        {
            List<WaitTime> value = new List<WaitTime>();

            response.messages.Add(speed + "  " + time + "  ");
            response.messages.Add(start.ToString("yyyy-MM-dd HH:mm:ss") + "  " + end.ToString("yyyy-MM-dd HH:mm:ss"));

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_calculo_alarmas", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    cmd.Parameters.Add(new SqlParameter("@inicio", start));
                    cmd.Parameters.Add(new SqlParameter("@fin", end));
                    cmd.Parameters.Add(new SqlParameter("@time", Convert.ToInt32(time)));
                    cmd.Parameters.Add(new SqlParameter("@max_velocidad", Convert.ToDecimal(speed)));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            WaitTime wait = new WaitTime();
                            wait.events = GetAlarm(Convert.ToString(reader["Alarm"]));
                            wait.Date = Convert.ToDateTime(reader["Fecha"]);
                            wait.lat = Convert.ToString(reader["Latitud"]);
                            wait.lng = Convert.ToString(reader["Longitud"]);
                            wait.time = UseFul.CalcularTime(Convert.ToInt32(reader["Diferencia"]));
                            value.Add(wait);
                        }
                    }
                }
                else
                {
                    value = new List<WaitTime>();
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
            return value;
        }

        private List<Points> PointsProcess(string device, DateTime startdate, DateTime enddate, int diff)
        {
            List<Points> listPoints = new List<Points>();
            List<ItinerariesKey> listKey = new List<ItinerariesKey>();

            try
            {

                listKey = GetDataProcess(device, startdate, enddate, diff);

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
                            point.events = start;
                            point.Date = data.StartDate;
                            point.lng = data.Longitude;
                            point.lat = data.Latitude;
                            point.speed = data.VelocidadMaxima;
                            listPoints.Add(point);

                            point = new Points();
                            point.events = points;
                            point.Date = data.StartDate;
                            point.lng = data.Longitude;
                            point.lat = data.Latitude;
                            point.speed = data.VelocidadMaxima;
                            listPoints.Add(point);
                        }
                        else
                        {
                            point.events = points;
                            point.Date = data.StartDate;
                            point.lng = data.Longitude;
                            point.lat = data.Latitude;
                            point.speed = data.VelocidadMaxima;
                            listPoints.Add(point);
                        }
                        
                        rows++;
                    }

                    point = new Points();
                    point.events = ItinerariesDao.end; 
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

        private List<Points> PointsProcess(List<ItinerariesKey> listKey)
        {
            List<Points> listPoints = new List<Points>();
            //List<ItinerariesKey> listKey = new List<ItinerariesKey>();

            try
            {

                //listKey = GetDataProcess(device, startdate, enddate, diff);

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
                            point.events = start;
                            point.Date = data.StartDate;
                            point.lng = data.Longitude;
                            point.lat = data.Latitude;
                            point.speed = data.VelocidadMaxima;
                            listPoints.Add(point);

                            point = new Points();
                            point.events = points;
                            point.Date = data.StartDate;
                            point.lng = data.Longitude;
                            point.lat = data.Latitude;
                            point.speed = data.VelocidadMaxima;
                            listPoints.Add(point);
                        }
                        else
                        {
                            point.events = points;
                            point.Date = data.StartDate;
                            point.lng = data.Longitude;
                            point.lat = data.Latitude;
                            point.speed = data.VelocidadMaxima;
                            listPoints.Add(point);
                        }

                        rows++;
                    }

                    point = new Points();
                    point.events = ItinerariesDao.end;
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


        private List<Icons> AlarmsProcess(string device, DateTime startdate, DateTime enddate)
        {
            List<Icons> listIcons = new List<Icons>();
            try
            {

                string start = startdate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string end = enddate.ToString("yyyy-MM-ddTHH:mm:ssZ");

                BsonDocument bsonDocument = new BsonDocument();
                bsonDocument.Add("device", device);
                bsonDocument.Add("date", new BsonDocument("$gte", Convert.ToDateTime(start)).Add("$lte", Convert.ToDateTime(end)));

                var build = bsonDocument;

                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<SpiderFleetWebAPI.Models.Mongo.Alarms.Alarms>("Alarms");
                var result = stored.Find(build).Sort("{date:1}").ToList();

                List<string> alarmas = new List<string>();

                if (result.Count > 0)
                {
                    Icons icons = new Icons();

                    foreach (SpiderFleetWebAPI.Models.Mongo.Alarms.Alarms data in result)
                    {
                        icons = new Icons();

                        if (data.Alarmsn[0].Type.Equals("Ignition On"))
                        {
                            icons = new Icons();
                            icons.Name = data.Alarmsn[0].Type;
                            icons.Date = data.Date;
                            icons.lng = data.Location.Coordinates[0].ToString();
                            icons.lat = data.Location.Coordinates[1].ToString();
                            listIcons.Add(icons);
                        }
                        if (data.Alarmsn[0].Type.Equals("Ignition Off"))
                        {
                            icons = new Icons();
                            icons.Name = data.Alarmsn[0].Type;
                            icons.Date = data.Date;
                            icons.lng = data.Location.Coordinates[0].ToString();
                            icons.lat = data.Location.Coordinates[1].ToString();
                            listIcons.Add(icons);
                        }

                        //if (listIcons.Count == 0)
                        //{
                        //    icons.Name = ItinerariesDao.start;
                        //    icons.Date = data.Date;
                        //    icons.lng = data.Location.Coordinates[0].ToString();
                        //    icons.lat = data.Location.Coordinates[1].ToString();
                        //    listIcons.Add(icons);

                        //    icons = new Icons();
                        //    icons.Name = data.Alarmsn[0].Type;
                        //    icons.Date = data.Date;
                        //    icons.lng = data.Location.Coordinates[0].ToString();
                        //    icons.lat = data.Location.Coordinates[1].ToString();
                        //    listIcons.Add(icons);
                        //}
                        //else
                        //{
                        //    icons.Name = data.Alarmsn[0].Type;
                        //    icons.Date = data.Date;
                        //    icons.lng = data.Location.Coordinates[0].ToString();
                        //    icons.lat = data.Location.Coordinates[1].ToString();
                        //}
                        

                        //listIcons.Add(icons);
                    }

                    //icons = new Icons();
                    //icons.Name = ItinerariesDao.end;
                    //icons.Date = result[result.Count -1].Date;
                    //icons.lng = result[result.Count - 1].Location.Coordinates[0].ToString();
                    //icons.lat = result[result.Count - 1].Location.Coordinates[1].ToString();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listIcons;
        }

        private List<Icons> AlarmsReport(string device, DateTime startdate, DateTime enddate)
        {
            List<Icons> listIcons = new List<Icons>();
            Icons icons = new Icons();
            int count = 0;
            try
            {
                try
                {
                    if (sql.IsConnection)
                    {
                        cn = sql.Connection();
                        SqlCommand cmd = new SqlCommand("ad.sp_consult_reporte_alarmas", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                        cmd.Parameters.Add(new SqlParameter("@fechainicio", startdate));
                        cmd.Parameters.Add(new SqlParameter("@fechafin", enddate));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                icons = new Icons();

                                icons.Name = GetAlarm(Convert.ToString(reader["Alarm"]));
                                icons.Date = Convert.ToDateTime(reader["Fecha"]);
                                icons.lat = Convert.ToString(reader["Latitud"]);
                                icons.lng = Convert.ToString(reader["Longitud"]);

                                listIcons.Add(icons);
                            }
                        }
                    }
                    else
                    {
                        listIcons = new List<Icons>();
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
                return listIcons;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listIcons;
        }

        private string GetAlarm(string code)
        {
            string labelAlarm = string.Empty;
            switch (code)
            {
                case "01":
                    labelAlarm = "Speeding";
                    break;
                case "02":
                    labelAlarm = "Low Voltage";
                    break;
                case "03":
                    labelAlarm = "High Engine Coolant Temperature";
                    break;
                case "04":
                    labelAlarm = "Hard Acceleration";
                    break;
                case "05":
                    labelAlarm = "Hard Deceleration";
                    break;
                case "06":
                    labelAlarm = "Idle Engine";
                    break;
                case "07":
                    labelAlarm = "Towing";
                    break;
                case "08":
                    labelAlarm = "High RPM";
                    break;
                case "09":
                    labelAlarm = "Power On";
                    break;
                case "0A":
                    labelAlarm = "Exhaust Emission";
                    break;
                case "0B":
                    labelAlarm = "Quick Lane Change";
                    break;
                case "0C":
                    labelAlarm = "Sharp Turn";
                    break;
                case "0D":
                    labelAlarm = "Fatigue Driving";
                    break;
                case "0E":
                    labelAlarm = "Power Off";
                    break;
                case "0F":
                    labelAlarm = "Geo-fence";
                    break;
                case "10":
                    labelAlarm = "Exhaust Emission";
                    break;
                case "11":
                    labelAlarm = "Emergency";
                    break;
                case "12":
                    labelAlarm = "Tamper";
                    break;
                case "13":
                    labelAlarm = "Illegal Enter";
                    break;
                case "14":
                    labelAlarm = "Illegal Ignition";
                    break;
                case "15":
                    labelAlarm = "OBD Communication Error";
                    break;
                case "16":
                    labelAlarm = "Ignition On";
                    break;
                case "17":
                    labelAlarm = "Ignition Off";
                    break;
                case "18":
                    labelAlarm = "MIL alarm";
                    break;
                case "19":
                    labelAlarm = "Unlock Alarm";
                    break;
                case "1A":
                    labelAlarm = "No Card Presented";
                    break;
                case "1B":
                    labelAlarm = "Dangerous Driving";
                    break;
                case "1C":
                    labelAlarm = "Vibration";
                    break;
                case "EX":
                    labelAlarm = "Speed Excess";
                    break;
                case "WT":
                    labelAlarm = "Wait Time";
                    break;
                case "ENG":
                    labelAlarm = "ENG";
                    break;
                default:
                    labelAlarm = "Unknow Alarm";
                    break;
            }
            return labelAlarm;
        }

        public async Task<SnappedPoints> getPathSnapAsync(List<Points> listPoints)
        {

            int total = listPoints.Count;

            SnappedPoints snappedPoints = new SnappedPoints
            {
                snappedPoints = new List<SnappedPoint>()
            };

            SnappedPoints snappedPointPaso = new SnappedPoints
            {
                snappedPoints = new List<SnappedPoint>()
            };

            SnappedPoint snappedPoint = new SnappedPoint
            {
                location = new Location()
            };

            if (total == 0)
            {

                snappedPoint = null;

                return snappedPoints;
            }


            if (total <= 100)
            {
                var salida = string.Empty;

                foreach (var result in listPoints)
                {

                    salida = salida + result.lat + "," + result.lng + "|";

                }

                int lChain = salida.Length;

                salida = salida.Substring(0, lChain - 1);

                snappedPoints = SnappedAPI(salida);

            }
            else
            {
                int iterations = total / 100;
                int Contador = 1;

                while (iterations >= Contador - 1)
                {
                    var salida = string.Empty;

                    for (int x = (Contador * 100) - 100; x < (Contador * 100); x++)
                    {
                        if (x >= total - 1)
                        {
                            break;
                        }
                        salida = salida + listPoints [x].lat  + "," + listPoints[x].lng + "|";
                    }

                    try
                    {
                        int lChain = salida.Length;

                        salida = salida.Substring(0, lChain - 1);

                        snappedPointPaso = SnappedAPI(salida);

                        snappedPoints.snappedPoints.AddRange(snappedPointPaso.snappedPoints);
                    }
                    catch { }


                    Contador++;
                }

            }

            return snappedPoints;

        }

        public SnappedPoints SnappedAPI(string path)
        {

            //string URL = "https://roads.googleapis.com/v1/snapToRoads?path=" + path + "&interpolate=true&key=AIzaSyDEw9Cw96wQgmfLqZF6nWMGXdnHVv9azd0";
            string URL = "https://roads.googleapis.com/v1/snapToRoads?path=" + path + "&interpolate=true&key=" + configuration.snap;

            var salida = GetReleases(URL);
            /*
            JObject jObject = JObject.Parse(salida);
            //string displayName = (string)jObject.SelectToken("displayName");
            return jObject;//.SelectToken("snappedPoints");
            */

            SnappedPoints snappedPoints = JsonConvert.DeserializeObject<SnappedPoints>(salida);

            return snappedPoints;

        }

        private string GetReleases(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            //request.UserAgent = RequestConstants.UserAgentValue;
            //request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var content = string.Empty;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
            }

            return content;
        }

        #endregion

        private List<ItinerariesKey> GetDataProcess(string device, DateTime startdate, DateTime enddate, int diff)
        {
            List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> listItineraries = new List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>>();
            List<CredencialSpiderFleet.Models.Itineraries.Itineraries> list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
            List<ItinerariesKey> listKey = new List<ItinerariesKey>();

            try
            {
                //string start = "2020-10-28T06:00:00Z";// startdate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                //string end = "2020-10-29T05:59:59Z";// enddate.ToString("yyyy-MM-ddTHH:mm:ssZ");

                string start = startdate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string end = enddate.ToString("yyyy-MM-ddTHH:mm:ssZ");

                BsonDocument bsonDocument = new BsonDocument();
                bsonDocument.Add("device", device);
                bsonDocument.Add("date", new BsonDocument("$gte", Convert.ToDateTime(start)).Add("$lte", Convert.ToDateTime(end)));

                var buildGPS = bsonDocument;
                var storedGPS = mongoDBContext.spiderMongoDatabase.GetCollection<GPS>("GPS");
                var resultGPS = storedGPS.Find(buildGPS).Sort("{date:1}").ToList();
                DateTime fechaAnterior = DateTime.Now;

                if (resultGPS.Count > 0)
                {
                    int count = 0;
                    foreach (GPS data in resultGPS)
                    {
                        ItinerariesKey keys = new ItinerariesKey();
                        if (data.Diff <= diff)
                        {
                            //keys.Event = ItinerariesDao.start;
                            if(data.Diff <= 0)
                            {
                                int vals = UseFul.GetDiferenceDates(fechaAnterior, data.Date);

                                if (vals <= diff)
                                {
                                    keys.Event = ItinerariesDao.start;
                                }
                                else
                                {
                                    int rows = listKey.Count;
                                    listKey[rows - 1].Event = ItinerariesDao.end;
                                    keys.Event = ItinerariesDao.start;
                                }
                            }
                            else
                            {
                                keys.Event = ItinerariesDao.start;
                            }
                        }
                        else
                        {
                            if(listKey.Count > 0 )
                            {
                                int vals = UseFul.GetDiferenceDates(fechaAnterior, data.Date);
                                if (vals <= diff)
                                {
                                    keys.Event = ItinerariesDao.start;
                                }
                                else
                                {
                                    int rows = listKey.Count;
                                    listKey[rows - 1].Event = ItinerariesDao.end;
                                    keys.Event = ItinerariesDao.start;
                                }                               
                            }
                            else
                            {
                                keys.Event = ItinerariesDao.start;
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
                        

                        if(data.Protocol.Equals("CELDA"))
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

                if(listKey.Count > 0)
                {
                    int row = listKey.Count;
                    listKey[row - 1].Event = ItinerariesDao.end;
                }                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listKey;
        }

        //public static int GetDiferenceDates(DateTime inicio, DateTime final)
        //{
        //    TimeSpan duracion = final - inicio;

        //    int hours = duracion.Hours;
        //    int minutes = duracion.Minutes;
        //    int segundos = duracion.Seconds;

        //    int totalSegundos = 0;

        //    if (hours > 0)
        //    {
        //        totalSegundos = (hours * 60) * 60;
        //    }

        //    if (minutes > 0)
        //    {
        //        totalSegundos += (minutes * 60);
        //    }

        //    totalSegundos += segundos;

        //    return totalSegundos;
        //}


        public List<string> ReadOperatorData(string device)
        {
            List<string> value = new List<string>();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_driver_data", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            value.Add(Convert.ToString(reader["operator_name"]));
                            value.Add(Convert.ToString(reader["idImg"]));
                            value.Add(Convert.ToString(reader["vehicle_name"]));
                        }
                    }
                }
                else
                {
                    value = new List<string>();
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
            return value;
        }
    }
}