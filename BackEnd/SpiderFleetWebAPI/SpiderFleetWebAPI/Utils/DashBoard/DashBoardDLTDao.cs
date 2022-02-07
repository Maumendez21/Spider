using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.DashBoard;
using CredencialSpiderFleet.Models.Itineraries;
using CredencialSpiderFleet.Models.Obd;
using CredencialSpiderFleet.Models.Useful;
using ICSharpCode.SharpZipLib.Zip;
using MongoDB.Bson;
using MongoDB.Driver;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.GPS;
using SpiderFleetWebAPI.Models.Response.DashBoard;
using SpiderFleetWebAPI.Models.Response.Itineraries;
using SpiderFleetWebAPI.Models.Response.Obd;
using SpiderFleetWebAPI.Utils.Itineraries;
using SpiderFleetWebAPI.Utils.Obd;
using SpiderFleetWebAPI.Utils.Setting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace SpiderFleetWebAPI.Utils.DashBoard
{
    public class DashBoardDLTDao
    {
        public DashBoardDLTDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private UseFul use = new UseFul();

        private MongoDBContext mongoDBContext = new MongoDBContext();
        private const string startS = "START";
        private const string endS = "END";
        private const string points = "POINTS";

        private decimal ODO = 0;
        private decimal Fuel = 0;
        private double Time = 0;
        private Dictionary<string, double> listRendimiento = new Dictionary<string, double>();
        //private Dictionary<string, PerformanceVehicle> listRendimientos = new Dictionary<string, PerformanceVehicle>();
        private List<PerformanceVehicle> listPerformance = new List<PerformanceVehicle>();

        #region

        public MemoryStream Reporte(string hierarchy, string grupo, string device,DateTime fechaInicio, DateTime fechaFin,
            string tempOutput, string path,            
            MemoryStream memoryStream, MemoryStream memoryImage, 
            string nameImage, int rowIndex, int colIndex, int Height, int Width,
            List<string> filesExcel)
        {
            MemoryStream memoryZip = new MemoryStream();

            try
            {
                GetExcel(hierarchy, fechaInicio, fechaFin, grupo, device,  path, memoryStream, memoryImage,
                        nameImage, rowIndex, colIndex, Height, Width, filesExcel);

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

        private void GetExcel(string hierarchy, DateTime fechaInicio, DateTime fechaFin, string grupo, string device,
            string path, MemoryStream memoryStream, MemoryStream memoryImage,
            string nameImage, int rowIndex, int colIndex, int Height, int Width, List<string> filesExcel)
        {
            try
            {
                DashBoardDLTGraficaBarraResponse information = new DashBoardDLTGraficaBarraResponse();
                information = GetGraficas(hierarchy, fechaInicio, fechaFin, grupo, device);

                List<string> mesT = information.graficas.graficaTiempo.label;
                List<string> dataT = information.graficas.graficaTiempo.data;
                string totalT = information.TotalTiempo;

                List<string> mesD = information.graficas.graficaDistancia.label;
                List<string> dataD = information.graficas.graficaDistancia.data;
                string totalD = information.TotalDistancia;

                List<string> mesC = information.graficas.graficaLitros.label;
                List<string> dataC = information.graficas.graficaLitros.data;
                string totalC = information.TotalLitros;

                List<string> mesR = information.graficas.graficaRendimiento.label;
                List<string> dataR = information.graficas.graficaRendimiento.data;
                string totalR = information.TotalRendimiento;

                List<Ranking> best = information.ListRankingBest;
                List<Ranking> lower = information.ListRankingLower;

                MemoryStream ms = new MemoryStream();

                reporteGeneral(
                    mesT, dataT, totalT, 1, 
                    mesD, dataD, totalD, 2,
                    mesC, dataC, totalC, 3,
                    mesR, dataR, totalR, 4,
                    best, lower, 5,
                    fechaInicio, fechaFin, path,  memoryStream, memoryImage, nameImage, rowIndex, colIndex, Height, Width, filesExcel, ms);

            }
            catch(Exception ex)
            {

            }
        }

        private void reporteGeneral(
            List<string> mesT, List<string> dataT, string totalT, int hojaT,
            List<string> mesD, List<string> dataD, string totalD, int hojaD,
            List<string> mesC, List<string> dataC, string totalC, int hojaC,
            List<string> mesR, List<string> dataR, string totalR, int hojaR,
            List<Ranking> best, List<Ranking> lower, int hoja,
            DateTime fechaInicio, DateTime fechaFin,
            string path, MemoryStream memoryStream, MemoryStream memoryImage, string nameImage, 
            int rowIndex, int colIndex, int Height, int Width, List<string> filesExcel, MemoryStream ms)
        {
            try
            {               
                //Envia datos a Excel
                using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
                {
                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets[hojaT];

                    #region Tiempo
                    //Imagen
                    System.Drawing.Image img = System.Drawing.Image.FromStream(memoryImage);
                    ExcelPicture pic = excelWorksheet.Drawings.AddPicture("imageVista-" + nameImage, img);
                    pic.SetPosition(rowIndex, 0, colIndex + ((1 - 1) * 4), 0);
                    pic.SetSize(Width, Height);

                    int rows = 9;
                    int columns = 2;

                    excelWorksheet.Cells[3, 8].Value = totalT; //Total
                    excelWorksheet.Cells[5, 8].Value = fechaInicio.ToString("dd-MM-yyyy") + " Al " + fechaFin.ToString("dd-MM-yyyy");  //Fecha

                    foreach (var label in mesT)
                    {
                        var fecha = GetDate(label);

                        excelWorksheet.Cells[rows, columns].Style.Numberformat.Format = "MMMM-dd";
                        excelWorksheet.Cells[rows, columns].Formula = "=DATE("+ fecha.Year +"," + fecha.Month+ ","+ fecha.Day  +")";
                        rows++;
                    }

                    rows = 9;
                    columns = 3;

                    foreach (var item in dataT)
                    {
                        var someTime = UseFul.CalcularTime(Convert.ToInt32(item));
                        var timeSpan = TimeSpan.Parse(someTime);
                        excelWorksheet.Cells[rows, columns].Value = timeSpan;
                        excelWorksheet.Cells[rows, columns].Style.Numberformat.Format = "hh:mm:ss";

                        rows++;
                    }

                    excelPackage.SaveAs(ms);

                    #endregion
                    
                    #region Distancia
                    excelWorksheet = excelWorkBook.Worksheets[hojaD];

                    //Imagen
                    System.Drawing.Image imgD = System.Drawing.Image.FromStream(memoryImage);
                    ExcelPicture picD = excelWorksheet.Drawings.AddPicture("imageVista-" + nameImage, imgD);
                    picD.SetPosition(rowIndex, 0, colIndex + ((1 - 1) * 4), 0);
                    picD.SetSize(Width, Height);

                    rows = 9;
                    columns = 2;

                    excelWorksheet.Cells[3, 8].Value = totalD; //Total
                    excelWorksheet.Cells[5, 8].Value = fechaInicio.ToString("dd-MM-yyyy") + " Al " + fechaFin.ToString("dd-MM-yyyy");  //Fecha

                    foreach (var label in mesD)
                    {
                        var fecha = GetDate(label);

                        excelWorksheet.Cells[rows, columns].Style.Numberformat.Format = "MMMM-dd";
                        excelWorksheet.Cells[rows, columns].Formula = "=DATE(" + fecha.Year + "," + fecha.Month + "," + fecha.Day + ")";
                        //excelWorksheet.Cells[rows, columns].Value = label;
                        rows++;
                    }

                    rows = 9;
                    columns = 3;

                    foreach (var item in dataD)
                    {
                        excelWorksheet.Cells[rows, columns].Value = Convert.ToDouble(item);
                        rows++;
                    }

                    excelPackage.SaveAs(ms);

                    #endregion

                    #region Consumo
                    excelWorksheet = excelWorkBook.Worksheets[hojaC];

                    //Imagen
                    System.Drawing.Image imgC = System.Drawing.Image.FromStream(memoryImage);
                    ExcelPicture picC = excelWorksheet.Drawings.AddPicture("imageVista-" + nameImage, imgC);
                    picC.SetPosition(rowIndex, 0, colIndex + ((1 - 1) * 4), 0);
                    picC.SetSize(Width, Height);

                    rows = 9;
                    columns = 2;

                    excelWorksheet.Cells[3, 8].Value = totalC; //Total
                    excelWorksheet.Cells[5, 8].Value = fechaInicio.ToString("dd-MM-yyyy") + " Al " + fechaFin.ToString("dd-MM-yyyy");  //Fecha

                    foreach (var label in mesC)
                    {
                        var fecha = GetDate(label);

                        excelWorksheet.Cells[rows, columns].Style.Numberformat.Format = "MMMM-dd";
                        excelWorksheet.Cells[rows, columns].Formula = "=DATE(" + fecha.Year + "," + fecha.Month + "," + fecha.Day + ")";
                        //excelWorksheet.Cells[rows, columns].Value = label;
                        rows++;
                    }

                    rows = 9;
                    columns = 3;

                    foreach (var item in dataC)
                    {
                        excelWorksheet.Cells[rows, columns].Value = Convert.ToDouble(item);
                        rows++;
                    }

                    excelPackage.SaveAs(ms);

                    #endregion

                    #region Rendimiento
                    excelWorksheet = excelWorkBook.Worksheets[hojaR];

                    //Imagen
                    System.Drawing.Image imgR = System.Drawing.Image.FromStream(memoryImage);
                    ExcelPicture picR = excelWorksheet.Drawings.AddPicture("imageVista-" + nameImage, imgR);
                    picR.SetPosition(rowIndex, 0, colIndex + ((1 - 1) * 4), 0);
                    picR.SetSize(Width, Height);

                    rows = 9;
                    columns = 2;

                    excelWorksheet.Cells[3, 8].Value = totalR; //Total
                    excelWorksheet.Cells[5, 8].Value = fechaInicio.ToString("dd-MM-yyyy") + " Al " + fechaFin.ToString("dd-MM-yyyy");  //Fecha

                    foreach (var label in mesR)
                    {
                        var fecha = GetDate(label);

                        excelWorksheet.Cells[rows, columns].Style.Numberformat.Format = "MMMM-dd";
                        excelWorksheet.Cells[rows, columns].Formula = "=DATE(" + fecha.Year + "," + fecha.Month + "," + fecha.Day + ")";
                        //excelWorksheet.Cells[rows, columns].Value = label;
                        rows++;
                    }

                    rows = 9;
                    columns = 3;

                    foreach (var item in dataR)
                    {                        
                        excelWorksheet.Cells[rows, columns].Value = Convert.ToDouble(item);
                        rows++;
                    }

                    excelPackage.SaveAs(ms);

                    #endregion

                    #region Top
                    excelWorksheet = excelWorkBook.Worksheets[hoja];

                    //Imagen
                    System.Drawing.Image imgTop = System.Drawing.Image.FromStream(memoryImage);
                    ExcelPicture picTop = excelWorksheet.Drawings.AddPicture("imageVista-" + nameImage, imgTop);
                    picTop.SetPosition(rowIndex, 0, colIndex + ((1 - 1) * 4), 0);
                    picTop.SetSize(Width, Height);

                    rows = 10;
                    columns = 5;

                    excelWorksheet.Cells[5, 7].Value = fechaInicio.ToString("dd-MM-yyyy") + " Al " + fechaFin.ToString("dd-MM-yyyy");  //Fecha

                    foreach (var label in best)
                    {
                        excelWorksheet.Cells[rows, columns].Value = label.Name; columns++;
                        excelWorksheet.Cells[rows, columns].Value = Convert.ToDouble(label.Consume);

                        columns = 5;
                        rows++;
                    }

                    rows = 10;
                    columns = 8;

                    foreach (var item in lower)
                    {
                        excelWorksheet.Cells[rows, columns].Value = item.Name; columns++;
                        excelWorksheet.Cells[rows, columns].Value = Convert.ToDouble(item.Consume);

                        columns = 8;
                        rows++;
                    }

                    excelPackage.SaveAs(ms);

                    #endregion

                    string ruta = "" + path + "Report_Analitica" + ".xlsx";
                    FileInfo fi = new FileInfo(ruta);
                    excelPackage.SaveAs(fi);
                    filesExcel.Add(ruta);
                                        
                }

                ms.Position = 0;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private DateTime GetDate(string value)
        {
            DateTime now = DateTime.Today;

            string[] data = value.Split(' ');

            int x = now.Month;

            int mes = 0;

            switch (data[0].ToLower())
            {
                case "enero":
                    mes = 1;
                    break;
                case "febrero":
                    mes = 2;
                    break;
                case "marzo":
                    mes = 3;
                    break;
                case "abril":
                    mes = 4;
                    break;
                case "mayo":
                    mes = 5;
                    break;
                case "junio":
                    mes = 6;
                    break;
                case "julio":
                    mes = 7;
                    break;
                case "agosto":
                    mes = 8;
                    break;
                case "septiembre":
                    mes = 9;
                    break;
                case "octubre":
                    mes = 10;
                    break;
                case "noviembre":
                    mes = 11;
                    break;
                case "diciembre":
                    mes = 12;
                    break;
                
            }

            int day = Convert.ToInt32(data[1].ToString());

            var time = new DateTime(now.Year, mes, day, 0, 0, 0);

            return time;
        }


        #endregion

        #region DashBoard

        public DashBoardDLTGraficaBarraResponse GetGraficas(string hierarchy, DateTime fechaInicio, DateTime fechaFin,
            string grupo, string device)
        {
            DashBoardDLTGraficaBarraResponse response = new DashBoardDLTGraficaBarraResponse();
            ListObdResponse listDevice = new ListObdResponse();
            List<string> listObd = new List<string>();
            ItinerariesListResponse data = new ItinerariesListResponse();
            int countBest = 0;
            int countLower = 0;

            decimal totalDistancia = 0;
            decimal totalLitros = 0;
            double totalTiempo = 0;

            Graficas litros = new Graficas();
            Graficas distancia = new Graficas();
            Graficas tiempo = new Graficas();
            Graficas rendimiento = new Graficas();

            tiempo.label = new List<string>();
            tiempo.data = new List<string>();

            distancia.label = new List<string>();
            distancia.data = new List<string>();

            litros.label = new List<string>();
            litros.data = new List<string>();

            rendimiento.label = new List<string>();
            rendimiento.data = new List<string>();

            string subGrupo = string.Empty;
            int agrega = 1;

            double m = UseFul.diferencia(fechaFin, fechaInicio);
            
            DateTime nowIni = fechaInicio;            
            var inico = new DateTime(nowIni.Year, nowIni.Month, nowIni.Day, 0, 0, 0);
            var fin = new DateTime(nowIni.Year, nowIni.Month, nowIni.Day, 23, 59, 59);

            int horas = VerifyUser.VerifyUser.GetHours();
            inico = inico.AddHours(horas);
            fin = fin.AddHours(horas);



            CultureInfo ci = new CultureInfo("es-MX");
            ci = new CultureInfo("es-MX");

            try
            {

                string node = use.hierarchyPrincipalToken(hierarchy);
                int diff = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(node, "ITE", 1));
                
                countBest = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(node, "RBT", 2));
                countLower = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(node, "RLW", 2));

                subGrupo = !string.IsNullOrEmpty(grupo) ? grupo : hierarchy;

                if (!string.IsNullOrEmpty(device))
                {
                    ObdCompany obd = new ObdCompany();
                    obd.Company = "";
                    obd.Device = device;
                    obd.Hierarchy = "";
                    obd.Name = "";
                    listDevice.listObd.Add(obd);
                }
                else
                {
                    listDevice = ListDevice(subGrupo);
                }

                foreach(var datas in listDevice.listObd)
                {
                    listObd.Add(datas.Device);
                }


                int n = 0;
                while (n < Convert.ToInt32(m + 1))
                {
                    foreach (var vehiculo in listObd)
                    {
                        
                        ReadGeneralDeviceList(diff, vehiculo, inico, fin);
                    }
                    n++;

                    //Inserta los datos                    
                    tiempo.label.Add(inico.ToString("MMMM dd", ci));
                    //string time = UseFul.Calcular(Convert.ToInt32(Time));
                    tiempo.data.Add(UseFul.CalcularTime(Convert.ToInt32(Time.ToString())).Replace(":",""));

                    distancia.label.Add(inico.ToString("MMMM dd", ci));
                    distancia.data.Add(ODO.ToString());

                    litros.label.Add(inico.ToString("MMMM dd", ci));
                    litros.data.Add(Fuel.ToString());

                    rendimiento.label.Add(inico.ToString("MMMM dd", ci));
                    if(ODO == 0 & Fuel == 0)
                    {
                        rendimiento.data.Add(0+"");
                    }
                    else
                    {
                        rendimiento.data.Add(Convert.ToString(Math.Round((ODO / Fuel), 2)));
                    }
                    

                    inico = inico.AddDays(agrega);
                    fin = fin.AddDays(agrega);

                    totalDistancia = totalDistancia + ODO;
                    totalLitros = totalLitros + Fuel;
                    totalTiempo = totalTiempo + Time;

                    ODO = 0;
                    Fuel = 0;
                    Time = 0;                   

                }

                response.graficas.graficaDistancia = distancia;
                response.graficas.graficaLitros = litros;
                response.graficas.graficaTiempo = tiempo;
                response.graficas.graficaRendimiento = rendimiento;
                response.TotalDistancia = totalDistancia.ToString() + " Km";
                response.TotalLitros = totalLitros.ToString() + " Lts";
                response.TotalTiempo = UseFul.CalcularTime(Convert.ToInt32(totalTiempo)) + " Horas";
                response.TotalRendimiento = Convert.ToString(Math.Round((totalDistancia / totalLitros),2)) + " Km / Lt";


                List<Ranking> ListRankingBest = new List<Ranking>();
                List<Ranking> ListRankingLower = new List<Ranking>();
                listRendimiento = new Dictionary<string, double>();

                List<string> listData = new List<string>();
                string VehicleName = string.Empty;

                foreach (var dispositivo in listObd)
                {
                    VehicleName = string.Empty;
                    listData = ReadOperatorData(dispositivo);
                    if (listData.Count > 0)
                    {
                        VehicleName = listData[2];
                    }

                    VehicleName = (string.IsNullOrEmpty(VehicleName) ? dispositivo : VehicleName);

                    decimal odo = listPerformance.Where(y => y.Device.Equals(dispositivo)).Sum(x => x.Obd);
                    decimal fuel = listPerformance.Where(y => y.Device.Equals(dispositivo)).Sum(x => x.Fuel);
                    decimal result = 0;
                    if (odo == 0 & fuel == 0)
                    {
                        result = 0;
                    }
                    else
                    {                        
                        if(odo == 0 | fuel == 0)
                        {
                            result = 0;
                        }
                        else
                        {
                            result = Math.Round(odo / fuel, 2);
                        }                        
                    }

                    if (!listRendimiento.ContainsKey(dispositivo))
                    {
                        listRendimiento.Add(VehicleName, Convert.ToDouble(result.ToString()) );
                    }
                }

                var rankinkBest = from entry in listRendimiento orderby entry.Value descending select entry;
                var rankinkLower = from entry in listRendimiento orderby entry.Value ascending select entry;

                foreach (var best in rankinkBest)
                {
                    if (ListRankingBest.Count < countBest)
                    {
                        Ranking ranking = new Ranking();
                        ranking.Name = best.Key;
                        ranking.Consume = best.Value.ToString();
                        ListRankingBest.Add(ranking);
                    }
                    else
                    {
                        break;
                    }
                }

                foreach (var lower in rankinkLower)
                {
                    if (ListRankingLower.Count < countLower)
                    {
                        Ranking ranking = new Ranking();
                        ranking.Name = lower.Key;
                        ranking.Consume = lower.Value.ToString();
                        ListRankingLower.Add(ranking);
                    }
                    else
                    {
                        break;
                    }
                }

                response.ListRankingBest = ListRankingBest;
                response.ListRankingLower = ListRankingLower;

                response.success = true;
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
            return response;
        }

        public ListObdResponse ListDevice(string hierarchy)
        {
            ListObdResponse response = new ListObdResponse();

            try
            {
                response = (new SubCompanyAssignmentObdsDao()).ListDeviceHierarchy(hierarchy);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
            return response;
        }

        public ItinerariesGeneralResponse ReadGeneralDeviceList(int diff, string device, DateTime startdate, DateTime enddate)
        {
            CredencialSpiderFleet.Models.Itineraries.Itineraries general = new CredencialSpiderFleet.Models.Itineraries.Itineraries();
            ItinerariesGeneralResponse response = new ItinerariesGeneralResponse();
            ItinerariesListResponse data = new ItinerariesListResponse();
            List<string> listData = new List<string>();
            PerformanceVehicle performance = new PerformanceVehicle();

            decimal odo = 0;
            decimal fuel = 0;

            try
            {
                string VehicleName = string.Empty;
                string DriverData = string.Empty;
                string Image = string.Empty;

                //listData = ReadOperatorData(device);
                //if (listData.Count > 0)
                //{
                //    DriverData = listData[0];
                //    Image = listData[1];
                //    VehicleName = listData[2];
                //}

                data.listItineraries = ItinerariesProcess(device, startdate, enddate, diff);
                if (data.listItineraries.Count > 0)
                {
                    foreach (List<CredencialSpiderFleet.Models.Itineraries.Itineraries> list in data.listItineraries)
                    {
                        foreach (CredencialSpiderFleet.Models.Itineraries.Itineraries info in list)
                        {
                            general.VehicleName = info.VehicleName;
                            general.Device = info.Device;
                            general.DriverData = info.DriverData;
                            general.Score = info.Score;
                            general.StartDate = startdate;
                            general.EndDate = enddate;
                            general.Image = info.Image;

                            ODO = ODO + Convert.ToDecimal(info.ODO.Equals("") ? "0" : info.ODO);
                            Fuel = Fuel + Convert.ToDecimal(info.Fuel.Equals("") ? "0" : info.Fuel);
                            Time = Time + Convert.ToInt32(info.Time);
                            odo = odo + Convert.ToDecimal(info.ODO.Equals("") ? "0" : info.ODO);
                            fuel = fuel + Convert.ToDecimal(info.Fuel.Equals("") ? "0" : info.Fuel);                            
                        }
                    }

                    performance = new PerformanceVehicle();
                    performance.Device = device;
                    performance.Fuel = fuel;
                    performance.Obd = odo;
                    performance.Name = VehicleName;
                    listPerformance.Add(performance);

                    //if (!listRendimiento.ContainsKey(device))
                    //{
                    //    listRendimiento.Add(device, Convert.ToString(Math.Round((odo / fuel), 2)));
                    //}
                    //else
                    //{
                    //    decimal val = Math.Round((odo / fuel), 2);
                    //    string va = string.Empty;
                    //    listRendimiento.TryGetValue(device, out va);
                    //    listRendimiento[device] = Convert.ToString(val + Convert.ToDecimal(va));
                    //}

                    response.Itineraries = general;
                    response.success = true;
                }
                else
                {
                    performance = new PerformanceVehicle();
                    general = new CredencialSpiderFleet.Models.Itineraries.Itineraries();
                    general.VehicleName = VehicleName;
                    general.Device = device;
                    general.DriverData = DriverData;
                    general.Score = "";
                    general.StartDate = startdate;
                    general.EndDate = enddate;
                    general.Image = Image;

                    ODO = ODO + 0;
                    Fuel = Fuel + 0;
                    Time = Time + 0;

                    odo = odo + 0;
                    fuel = fuel + 0;

                    //if (!listRendimiento.ContainsKey(device))
                    //{                     
                    //    listRendimiento.Add(device, "0");
                    //}
                    //else
                    //{
                    //    decimal val = 0;
                    //    string va = string.Empty;
                    //    listRendimiento.TryGetValue(device, out va);
                    //    listRendimiento[device] = Convert.ToString(val + Convert.ToDecimal(va));
                    //}

                    performance = new PerformanceVehicle();
                    performance.Device = device;
                    performance.Fuel = 0;
                    performance.Obd = 0;
                    performance.Name = VehicleName;
                    listPerformance.Add(performance);

                    response.Itineraries = general;
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
                        if (data.Event.Equals(startS))
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
                                if (data.Event.Equals(endS))
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
                                else if (data.Event.Equals(startS))
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
                                if (data.Event.Equals(endS))
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
                            if (data.Diff <= 0)
                            {
                                int vals = UseFul.GetDiferenceDates(fechaAnterior, data.Date);

                                if (vals <= diff)
                                {
                                    keys.Event = startS;
                                }
                                else
                                {
                                    int rows = listKey.Count;
                                    listKey[rows - 1].Event = endS;
                                    keys.Event = startS;
                                }
                            }
                            else
                            {
                                keys.Event = startS;
                            }
                        }
                        else
                        {
                            if (listKey.Count > 0)
                            {
                                int vals = UseFul.GetDiferenceDates(fechaAnterior, data.Date);
                                if (vals <= diff)
                                {
                                    keys.Event = startS;
                                }
                                else
                                {
                                    int rows = listKey.Count;
                                    listKey[rows - 1].Event = endS;
                                    keys.Event = startS;
                                }
                            }
                            else
                            {
                                keys.Event = startS;
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
                    listKey[row - 1].Event = endS;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listKey;

        }

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

        #endregion
    }
}