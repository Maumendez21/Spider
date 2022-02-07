using CredencialSpiderFleet.Models.Address;
using CredencialSpiderFleet.Models.ApiGoogle;
using CredencialSpiderFleet.Models.Configuration;
using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Itineraries;
using CredencialSpiderFleet.Models.Logical;
using CredencialSpiderFleet.Models.TravelReport;
using CredencialSpiderFleet.Models.Useful;
using ICSharpCode.SharpZipLib.Zip;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.GPS;
using SpiderFleetWebAPI.Models.Response.Details;
using SpiderFleetWebAPI.Models.Response.Itineraries;
using SpiderFleetWebAPI.Models.Response.Responsible;
using SpiderFleetWebAPI.Utils.Address;
using SpiderFleetWebAPI.Utils.Details;
using SpiderFleetWebAPI.Utils.Itineraries;
using SpiderFleetWebAPI.Utils.Responsible;
using SpiderFleetWebAPI.Utils.Setting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace SpiderFleetWebAPI.Utils.ReportAdmin
{
    public class ReporteItinerariosMongo
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private MongoDBContext mongoDBContext = new MongoDBContext();
        private const string START_TRIP = "START";
        private const string END_TRIP = "END";
        private const string POINTS_TRIP = "POINTS";
        private const string MXV = "MXV";
        private const string WTC = "WTC";
        private UseFul use = new UseFul();
        private VariableConfiguration configuration = new VariableConfiguration();

        public MemoryStream Reporte(string hierarchy, int type, bool band, string tempOutput, string path,
           DateTime fechaInicio, DateTime fechaFin, decimal maxVelocidad,
           MemoryStream memoryStream, string nombreVehiculo,
           MemoryStream memoryImage, string nameImage, int rowIndex, int colIndex, int Height, int Width,
       List<string> filesExcel, List<string> devices)
        {
            MemoryStream memoryZip = new MemoryStream();

            try
            {
                if (type == 2)
                {
                    //reportePetrocarrier(path, fechaInicio, fechaFin,// maxVelocidad,
                    //    memoryStream, nombreVehiculo,
                    //    memoryImage, nameImage, rowIndex, colIndex, Height, Width,
                    //    filesExcel, devices);
                }
                else if (type == 1)
                {
                    reporteGeneral(hierarchy, band, path, fechaInicio, fechaFin, maxVelocidad,
                        memoryStream, nombreVehiculo,
                        memoryImage, nameImage, rowIndex, colIndex, Height, Width,
                        filesExcel, devices);
                }

                //Zip el archivo u archivos
                using (ZipOutputStream zipStream = new ZipOutputStream(File.Create(tempOutput)))
                {
                    zipStream.SetLevel(9);
                    byte[] buffer = new byte[4096];

                    for (int i = 0; i < filesExcel.Count; i++)
                    {
                        ZipEntry entry = new ZipEntry(Path.GetFileName(filesExcel[i]));
                        entry.DateTime = DateTime.Now;
                        entry.IsUnicodeText = true;
                        zipStream.PutNextEntry(entry);

                        using (FileStream fileStream = File.OpenRead(filesExcel[i]))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                                zipStream.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }
                    zipStream.Finish();
                    zipStream.Flush();
                    zipStream.Close();
                }

                //Archivo zip a MemoryStream
                using (FileStream file = new FileStream(tempOutput, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    memoryZip.Write(bytes, 0, (int)file.Length);
                    filesExcel.Add(tempOutput);
                }

                //Eliminando Archivos de Excel Generados
                if (File.Exists(tempOutput))
                {
                    foreach (var archivos in filesExcel)
                    {
                        File.Delete(archivos);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return memoryZip;
        }

        private void reporteGeneral(string hierarchy, bool band, string path, DateTime fechaInicio, DateTime fechaFin, decimal maxVelocidad,
           MemoryStream memoryStream, string nombreVehiculo,
           MemoryStream memoryImage, string nameImage, int rowIndex, int colIndex, int Height, int Width,
           List<string> filesExcel, List<string> devices)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                
                List<TravelReport> listTravelReport = new List<TravelReport>();

                foreach (var device in devices)
                {
                    //Envia datos a Excel
                    using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
                    {

                        ResponsibleVehicleResponse responsible = new ResponsibleVehicleResponse();
                        responsible = (new ResponsibleDao()).ReadVehicle(device);

                        if (!string.IsNullOrEmpty(responsible.responsible.Vehicle))
                        {
                            nombreVehiculo = responsible.responsible.Vehicle;
                        }
                        else
                        {
                            responsible = (new ResponsibleDao()).ReadNameVehicle(device);
                            nombreVehiculo = responsible.responsible.Vehicle;
                        }

                        ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                        ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets[1];

                        //Imagen
                        System.Drawing.Image img = System.Drawing.Image.FromStream(memoryImage);
                        ExcelPicture pic = excelWorksheet.Drawings.AddPicture("imageVista-" + nameImage, img);
                        pic.SetPosition(rowIndex, 0, colIndex + ((1 - 1) * 4), 0);
                        pic.SetSize(Width, Height);

                        int rows = 15;
                        int columns = 2;
                        int totalSegundos = 0;
                        int totalMetros = 0;
                        int totalFrenado = 0;
                        int totalAceleracion = 0;
                        double totalConsumo = 0.0;
                        int totalVelocidad = 0;
                        int totalRPM = 0;

                        TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                        DateTime nowIni = TimeZoneInfo.ConvertTimeFromUtc(fechaInicio, cstZone);

                        DateTime nowFin = TimeZoneInfo.ConvertTimeFromUtc(fechaFin, cstZone);
                        DateTime hoy = DateTime.UtcNow;
                        DateTime today = TimeZoneInfo.ConvertTimeFromUtc(hoy, cstZone);

                        int horas = VerifyUser.VerifyUser.GetHours();
                        listTravelReport = Travels(hierarchy, device, fechaInicio, fechaFin, horas);

                        excelWorksheet.Cells[3, 6].Value = responsible.responsible.Vehicle;
                        
                        excelWorksheet.Cells[6, 4].Value = fechaInicio.ToString("dd-MM-yyyy") + " al " + fechaFin.ToString("dd-MM-yyyy");
                        excelWorksheet.Cells[7, 4].Value = responsible.responsible.Responsible;
                        excelWorksheet.Cells[6, 16].Value = today.ToString("dd-MM-yyyy HH:mm");

                        if (band)
                        {
                            excelWorksheet.Cells[14, 18].Value = "Destino";
                            excelWorksheet.Cells[14, 19].Value = "Responsable";
                        }
                        else
                        {
                            excelWorksheet.Cells[14, 18].Value = "Responsable";
                        }

                        foreach (var data in listTravelReport)
                        {
                            excelWorksheet.Cells[rows, columns].Value = data.Number ; columns++;
                            excelWorksheet.Cells[rows, columns].Value = data.TravelDate.ToString("dd/MM/yyyy"); columns++;
                            excelWorksheet.Cells[rows, columns].Value = data.StartDate.ToString("hh:mm tt").ToUpper(); columns++;
                            excelWorksheet.Cells[rows, columns].Value = data.EndDate.ToString("hh:mm tt").ToUpper(); columns++;

                            string tiempo = string.Empty;
                            tiempo = data.Time;// use.CalcularTiempo(data.Tiempo);
                            
                            totalSegundos = totalSegundos + Convert.ToInt32((UseFul.CalcularTime(tiempo.Replace(" Hrs", ""))));

                            excelWorksheet.Cells[rows, columns].Value = tiempo.Replace(" Hrs", ""); columns++;

                            totalAceleracion = totalAceleracion + data.HardAcceleration;

                            excelWorksheet.Cells[rows, columns].Value = data.HardAcceleration; columns++;

                            totalFrenado = totalFrenado + data.HardDeceleration;

                            excelWorksheet.Cells[rows, columns].Value = data.HardDeceleration; columns++;

                            totalRPM = totalRPM + data.HighRPM;
                            excelWorksheet.Cells[rows, columns].Value = data.HighRPM; columns++;

                            totalVelocidad = totalVelocidad + data.Speeding;

                            excelWorksheet.Cells[rows, columns].Value = data.Speeding; columns++;

                            string distancia = string.Empty;
                            distancia = data.Distance.ToString();// use.metrosKilometros(data.Distancia);
                            totalMetros = totalMetros + (Convert.ToInt32(data.Distance));

                            excelWorksheet.Cells[rows, columns].Value = distancia; columns++;
                            //double consumo = (data.Distance * 1) / 10;
                            double consumo = data.Distance / 10;

                            excelWorksheet.Cells[rows, columns].Value = Math.Round(consumo, 2).ToString(); columns++;

                            totalConsumo = totalConsumo + consumo;

                            DateTime timeIni = data.StartDate;

                            string fechaini = timeIni.ToString("yyyy-MM-dd");
                            string formatIni = fechaini + "T" + data.StartDate.ToString("HH:mm:ss") + "Z";

                            DateTime timeFin = data.EndDate;

                            string fechaFn = timeFin.ToString("yyyy-MM-dd");
                            string formatFin = fechaFn + "T" + data.EndDate.ToString("HH:mm:ss") + "Z";
                            string link = "http://spiderfleetapi.azurewebsites.net/#/trip/" + device + "/" + formatIni + "/" + formatFin;

                            columns++; columns++; columns++;

                            ExcelHyperLink hl = new ExcelHyperLink(link);
                            hl.Display = "Link";

                            excelWorksheet.Cells[rows, columns].Hyperlink = hl; columns++; columns++;

                            if (band)
                            {
                                AddressConsult address = (new AddressDao()).GetAddress(device, timeFin, data.Latitud, data.Longitud);
                                var addressI = string.Empty;
                                var addressF = string.Empty;

                                if (string.IsNullOrEmpty(address.Address))
                                {
                                    var url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + data.Latitud + "," + data.Longitud + "&key=" + configuration.snap;
                                    var result = new WebClient().DownloadString(url);
                                    GoogleGeoCodeResponse test = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(result);
                                    var direccion = UseFul.ToUTF8(test.results[0].formatted_address.ToString());

                                    excelWorksheet.Cells[rows, columns].Value = direccion; columns++;
                                    excelWorksheet.Cells[rows, columns].Value = data.Responsable; columns++;

                                    CredencialSpiderFleet.Models.Address.Address dir = new CredencialSpiderFleet.Models.Address.Address();
                                    dir.Device = device;
                                    dir.Date = timeFin;
                                    dir.Point = direccion;
                                    dir.Latitude = data.Latitud;
                                    dir.Longitude = data.Longitud;

                                    (new AddressDao()).Create(dir);
                                }
                                else
                                {
                                    excelWorksheet.Cells[rows, columns].Value = address.Address; columns++;
                                    excelWorksheet.Cells[rows, columns].Value = data.Responsable; columns++;
                                }
                            }
                            else
                            {
                                //responsable
                                string responsable = GetResponsable(device, data.StartDate, data.EndDate);
                                excelWorksheet.Cells[rows, columns].Value = responsable; columns++;
                            }

                            rows++;
                            columns = 2;
                        }

                        excelWorksheet.Cells[8, 4].Value = Convert.ToString(Math.Round(Convert.ToDouble(totalMetros) / Math.Round(totalConsumo, 2), 2)) + " Km/L";
                        excelWorksheet.Cells[9, 4].Value = totalMetros + " Km";
                        excelWorksheet.Cells[10, 4].Value = use.CalcularTiempo(totalSegundos) + " Hrs";
                        excelWorksheet.Cells[11, 4].Value = Math.Round(totalConsumo, 2).ToString() + " Litros";

                        excelWorksheet.Cells[8, 7].Value = totalAceleracion;
                        excelWorksheet.Cells[8, 8].Value = totalFrenado;
                        excelWorksheet.Cells[8, 9].Value = totalRPM;
                        excelWorksheet.Cells[8, 10].Value = totalVelocidad;

                        excelPackage.SaveAs(ms);

                        nombreVehiculo = string.IsNullOrEmpty(nombreVehiculo) ? device : nombreVehiculo;

                        string ruta = "" + path + nombreVehiculo + ".xlsx";
                        FileInfo fi = new FileInfo(ruta);
                        excelPackage.SaveAs(fi);
                        filesExcel.Add(ruta);
                    }

                    ms.Position = 0;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<TravelReport> Travels(string hierarchy, string device, DateTime startdate, DateTime enddate, int horas)
        {
            List<TravelReport> listTravelReport = new List<TravelReport>();
            try
            {
                ItinerariesResponse response = new ItinerariesResponse();
                response = (new ItinerariesDao()).ReadItinerariosDeviceList(hierarchy, device, startdate, enddate);
                List<CredencialSpiderFleet.Models.Itineraries.Itineraries> listItineraries = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();

                if(response.listItineraries.Count > 0)
                {
                    listItineraries = response.listItineraries;
                    int count = listItineraries.Count;

                    string nombreVehiculo = string.Empty;
                    string nombreResponsable = string.Empty;
                    int tipoDevice = 0;

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

                    foreach (var item in listItineraries)
                    {
                        TravelReport report = new TravelReport();                        

                        List<Models.Mongo.Alarms.Alarms> listAlarms = new List<Models.Mongo.Alarms.Alarms>();
                        listAlarms = GetDataAlarms(device, item.StartDate.AddHours(horas), item.EndDate.AddHours(horas));
                                              
                        StrokeResponse link = new StrokeResponse();
                        link = ReadStrokeDeviceList(device
                            ,item.StartDate.ToString("yyyy-MM-dd HH:mm:ss"), item.EndDate.ToString("yyyy-MM-dd HH:mm:ss")
                            , hierarchy, horas, nombreVehiculo, nombreResponsable, tipoDevice);

                        report.Number = count;

                        report.TravelDate = item.StartDate;
                        report.StartDate = item.StartDate;
                        report.EndDate = item.EndDate;
                        report.Time = link.ElapsedTime;
                            //Convert.ToInt32(UseFul.diferenciaSeconds(item.StartDate, item.EndDate));

                        report.HardAcceleration = listAlarms.Count(x => x.Alarmsn[0].Type.Equals("Hard Acceleration"));
                        report.HardDeceleration = listAlarms.Count(x => x.Alarmsn[0].Type.Equals("Hard Deceleration"));
                        report.HighRPM = listAlarms.Count(x => x.Alarmsn[0].Type.Equals("High RPM"));
                        report.Speeding = link.Speed;

                        report.Distance = link.TotalDistanciaDouble;
                        report.Consumption = link.FuelConsumption;
                        report.Responsable = link.ResponsibleName;
                        report.Latitud = link.Latitude;
                        report.Longitud = link.Longitude;

                        listTravelReport.Add(report);
                        
                        count--;
                    }

                    string data = string.Empty;

                    listTravelReport = listTravelReport.OrderBy(x => x.Number).ToList<TravelReport>();
                }
            }
            catch(Exception ex)
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

        private List<Models.Mongo.Alarms.Alarms> GetDataAlarms(string device, DateTime startdate, DateTime enddate)
        {
            List<Models.Mongo.Alarms.Alarms> listAlarms = new List<Models.Mongo.Alarms.Alarms>();

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
                    listAlarms = result;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listAlarms;
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
                    if(Convert.ToDecimal(item.VelocidadMaxima) > maximaVelocidad)
                    {
                        if(!band)
                        {
                            fechaAnt = item.StartDate;
                            count.Add(0);
                            band = true;
                        }                        
                    }
                    else
                    {
                        if(band)
                        {
                            double secs = UseFul.diferenciaSeconds(item.StartDate, fechaAnt);
                            count.Add(Convert.ToInt32(secs.ToString()));
                            band = false;
                        }
                    }
                }

                exceso = count.Count(x => x > 30);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return exceso;
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

        //Modificaicon calculo de tiempo-gasolina-km
        private List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> ItinerariesProcess(List<ItinerariesKey> listKey, string device)
        {
            List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> listItineraries = new List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>>();
            List<CredencialSpiderFleet.Models.Itineraries.Itineraries> list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();

            string VehicleName = string.Empty;
            string DriverData = string.Empty;
            string Image = string.Empty;

            try
            {
                int rows = 0;
                int count = 0;
                double calculoDistanciaDouble = 0;

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

        private string GetResponsable(string device, DateTime start, DateTime end)
        {
            string respuesta = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_responsable_detail_report", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", device));
                    cmd.Parameters.Add(new SqlParameter("@startdate", start));
                    cmd.Parameters.Add(new SqlParameter("@enddate", end));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@responsable";
                    sqlParameter.SqlDbType = SqlDbType.VarChar;
                    sqlParameter.Size = 50;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = sqlParameter.Value.ToString();

                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                cn.Close();
            }
            return respuesta;
        }

    }
}