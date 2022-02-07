using CredencialSpiderFleet.Models.Main.Reports;
using CredencialSpiderFleet.Models.Main.TraceTrip;
using CredencialSpiderFleet.Models.ReportAdmin;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpiderFleetWebAPI.Utils.Main.Reports
{
    public class ReportsExcel
    {

        public int excelDrivingBehaviors(List<ReportConduct> listPoint)
        {
            try
            {
                Dictionary<string, string> alarms = new Dictionary<string, string>();

                alarms = Alarms();

                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("Reporte");

                    var headerTitulo = new List<string[]>()
                    {
                        new string[] { "Reporte de eventos", "","","","" }
                    };
                    
                    int fila = 1;
                    string celda = "B" + fila + ":";
                    string headerRange = celda + Char.ConvertFromUtf32(headerTitulo[0].Length + 64) + fila;
                    fila++;

                    // Target a worksheet
                    var worksheet = excel.Workbook.Worksheets["Reporte"];
                    
                    worksheet.Cells[headerRange].Merge = true;
                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                    worksheet.Cells[headerRange].Style.Font.Size = 14;
                    worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkSeaGreen);
                    worksheet.Cells[headerRange].LoadFromArrays(headerTitulo);
                    worksheet.Cells[headerRange].AutoFitColumns();

                    var headerRow = new List<string[]>()
                    {
                        new string[] { "", "Fecha", "Velocidad", "Evento",  "Ubicacion" }
                    };

                    
                    celda = "A" + fila + ":";
                    headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;
                    fila++;

                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                    worksheet.Cells[headerRange].Style.Font.Size = 14;
                    worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                    worksheet.Cells[headerRange].AutoFitColumns();


                    string device = string.Empty;
                    string dato = string.Empty;
                    string ubicacion = "https://www.google.com/maps/search/?api=1&query=";

                    foreach (ReportConduct point in listPoint)
                    {
                        string evento = string.Empty;
                        if (point.Event.Equals("Alarm"))
                        {
                            evento = point.Type;
                        }
                        else
                        {
                            evento = "";
                        }

                        if (string.IsNullOrEmpty(device))
                        {
                            device = point.Device;

                            if (string.IsNullOrEmpty(dato))
                            {
                                dato = point.Device;

                                var gerenciaCell = new List<string[]>()
                                {
                                    new string[] { point.NameSubCompany, "", "", "" }
                                };

                                headerRange = string.Empty;
                                celda = string.Empty;
                                celda = "B" + fila + ":";
                                headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;

                                worksheet.Cells[headerRange].Merge = true;
                                worksheet.Cells[headerRange].Style.Font.Bold = true;
                                worksheet.Cells[headerRange].Style.Font.Size = 14;
                                worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                                worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PowderBlue);

                                worksheet.Cells[headerRange].AutoFitColumns();
                                worksheet.Cells[headerRange].LoadFromArrays(gerenciaCell);
                                fila++;

                                var deviceCell = new List<string[]>()
                                {
                                    new string[] { dato, "", "", "" }
                                };

                                headerRange = string.Empty;
                                celda = string.Empty;
                                celda = "B" + fila + ":";
                                headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;

                                worksheet.Cells[headerRange].Merge = true;
                                worksheet.Cells[headerRange].Style.Font.Bold = true;
                                worksheet.Cells[headerRange].Style.Font.Size = 14;
                                worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                                worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);

                                worksheet.Cells[headerRange].AutoFitColumns();
                                worksheet.Cells[headerRange].LoadFromArrays(deviceCell);
                                fila++;
                            }
                            else
                            {
                                dato = string.Empty;
                            }
                        }
                        else if (device.Equals(point.Device))
                        {
                            dato = string.Empty;
                        }
                        else if (!device.Equals(point.Device))
                        {
                            dato = point.Device;
                            device = string.Empty;

                            var gerenciaCell = new List<string[]>()
                                {
                                    new string[] { point.NameSubCompany, "", "", "" }
                                };

                            headerRange = string.Empty;
                            celda = string.Empty;
                            celda = "B" + fila + ":";
                            headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;

                            worksheet.Cells[headerRange].Merge = true;
                            worksheet.Cells[headerRange].Style.Font.Bold = true;
                            worksheet.Cells[headerRange].Style.Font.Size = 14;
                            worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                            worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PowderBlue);

                            worksheet.Cells[headerRange].AutoFitColumns();
                            worksheet.Cells[headerRange].LoadFromArrays(gerenciaCell);
                            fila++;


                            var deviceCell = new List<string[]>()
                            {
                                new string[] { dato, "", "", "" }
                            };

                            headerRange = string.Empty;
                            celda = string.Empty;
                            celda = "B" + fila + ":";
                            headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;

                            worksheet.Cells[headerRange].Merge = true;
                            worksheet.Cells[headerRange].Style.Font.Bold = true;
                            worksheet.Cells[headerRange].Style.Font.Size = 14;
                            worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                            worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);

                            worksheet.Cells[headerRange].AutoFitColumns();
                            worksheet.Cells[headerRange].LoadFromArrays(deviceCell);
                            fila++;
                        }

                        string typeAlarm = string.Empty;
                        decimal speed = Convert.ToDecimal(point.Speed);


                        if(alarms.ContainsKey(evento))
                        {
                            typeAlarm = alarms.Where(p => p.Key == evento).FirstOrDefault().Value;
                        }
                       
                        var data = new List<string[]>()
                        {
                            new string[] { "", Convert.ToString(point.Date.ToString("dd-MM-yyyy")), Decimal.Round(speed,2).ToString(), typeAlarm, ubicacion + point.Latitude + "," + point.Longitude }
                        };

                        headerRange = string.Empty;
                        celda = string.Empty;
                        celda = "A" + fila + ":";
                        headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;

                        //worksheet.Cells[headerRange].Style.WrapText = true;
                        worksheet.Cells[headerRange].AutoFitColumns();

                        worksheet.Cells[headerRange].AutoFitColumns();
                        worksheet.Cells[headerRange].LoadFromArrays(data);
                        fila++;
                    }
                    DateTime fecha = DateTime.Today;
                    FileInfo excelFile = new FileInfo(@"C:\Spider\ExcelReportes\Reporte.xlsx");
                    excel.SaveAs(excelFile);
                }
                return 0;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }           
        }

        private Dictionary<string, string> Alarms()
        {
            Dictionary<string, string> alarms = new Dictionary<string, string>();

            alarms.Add("Login", "Iniciar sesión");
            alarms.Add("GPS", "GPS");
            alarms.Add("Sleep", "Dormir");
            alarms.Add("Alarm", "Alarma");
            alarms.Add("Speeding", "Exceso de velocidad");
            alarms.Add("Low Voltage", "Baja tensión");
            alarms.Add("High Engine Coolant Temperature", "Temperatura alta del refrigerante del motor");
            alarms.Add("Hard Acceleration", "Aceleración dura");
            alarms.Add("Hard Deceleration", "Desaceleración dura");
            alarms.Add("Idle Engine", "Motor inactivo");
            alarms.Add("Towing", "Remolque");
            alarms.Add("High RPM", "RPM altas");
            alarms.Add("Power On", "Encendido");
            alarms.Add("Exhaust Emission", "Las emisiones de escape");
            alarms.Add("Quick Lane Change", "Cambio rápido de carril");
            alarms.Add("Sharp Turn", "Giro brusco");
            alarms.Add("Fatigue Driving", "Conducción de fatiga");
            alarms.Add("Power Off", "Apagado");
            alarms.Add("SOS", "llamada de socorro");
            alarms.Add("Tamper", "Manosear");
            alarms.Add("Ignition On", "Encendido conectado");
            alarms.Add("Ignition Off", "Encendido apagado");
            alarms.Add("MIL alarm", "Alarma MIL");
            alarms.Add("Dangerous Driving", "Conducción peligrosa");
            alarms.Add("Vibration", "Vibración");
            alarms.Add("Unlock Alarm", "Desbloquear alarma");
            alarms.Add("No Card Presented", "Ninguna tarjeta presentada");
            alarms.Add("Illegal Enter", "Entrar ilegal");
            alarms.Add("Illegal Ignition", "Encendido ilegal");
            alarms.Add("OBD Communication Error", "Error de comunicación OBD");
            alarms.Add("Emergency", "Emergencia");
            alarms.Add("Geo-fence", "Geo-cerca ");

            return alarms;
        }
    
        public int excelTrips(List<List<TripsInformation>> trips)
        {
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("Reporte");

                    var headerTitulo = new List<string[]>()
                    {
                        new string[] { "Reporte de Viajes", "","","","" }
                    };

                    int fila = 1;
                    string celda = "B" + fila + ":";
                    string headerRange = celda + Char.ConvertFromUtf32(headerTitulo[0].Length + 64) + fila;
                    fila++;


                    // Target a worksheet
                    var worksheet = excel.Workbook.Worksheets["Reporte"];

                    worksheet.Cells[headerRange].Merge = true;
                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                    worksheet.Cells[headerRange].Style.Font.Size = 14;
                    worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkSeaGreen);
                    worksheet.Cells[headerRange].LoadFromArrays(headerTitulo);
                    worksheet.Cells[headerRange].AutoFitColumns();

                    var headerRow = new List<string[]>()
                    {
                        new string[] { "", "Fecha", "Hora",  "Km", "Fuel" }
                    };


                    celda = "A" + fila + ":";
                    headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;
                    fila++;

                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                    worksheet.Cells[headerRange].Style.Font.Size = 14;
                    worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                    worksheet.Cells[headerRange].AutoFitColumns();

                    string device = string.Empty;
                    string dato = string.Empty;

                    foreach (List<TripsInformation> listTrips in trips)
                    {
                        foreach(TripsInformation data in listTrips)
                        {
                            if (string.IsNullOrEmpty(device))
                            {
                                device = data.Device;

                                if (string.IsNullOrEmpty(dato))
                                {

                                    var gerenciaCell = new List<string[]>()
                                    {
                                        new string[] { data.Device, "", "", "", ""}
                                    };

                                    headerRange = string.Empty;
                                    celda = string.Empty;
                                    celda = "B" + fila + ":";
                                    headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;

                                    worksheet.Cells[headerRange].Merge = true;
                                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                                    worksheet.Cells[headerRange].Style.Font.Size = 14;
                                    worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                                    worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);

                                    worksheet.Cells[headerRange].AutoFitColumns();
                                    worksheet.Cells[headerRange].LoadFromArrays(gerenciaCell);
                                    fila++;

                                    var totalViajesCell = new List<string[]>()
                                    {
                                        new string[] { "Total de Viajes : " + data.TotalTrips, "", "", "", ""}
                                    };

                                    headerRange = string.Empty;
                                    celda = string.Empty;
                                    celda = "B" + fila + ":";
                                    headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;

                                    worksheet.Cells[headerRange].Merge = true;
                                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                                    worksheet.Cells[headerRange].Style.Font.Size = 14;
                                    worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                                    worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PowderBlue);

                                    worksheet.Cells[headerRange].AutoFitColumns();
                                    worksheet.Cells[headerRange].LoadFromArrays(totalViajesCell);
                                    fila++;

                                }
                                else
                                {
                                    dato = string.Empty;
                                }
                            }
                            else if (device.Equals(data.Device))
                            {
                                dato = string.Empty;
                            }
                            else if (!device.Equals(data.Device))
                            {
                                dato = data.Device;
                                device = string.Empty;

                                var deviceCells = new List<string[]>()
                                {
                                    new string[] { dato, "", "", "" }
                                };

                                headerRange = string.Empty;
                                celda = string.Empty;
                                celda = "B" + fila + ":";
                                headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;

                                worksheet.Cells[headerRange].Merge = true;
                                worksheet.Cells[headerRange].Style.Font.Bold = true;
                                worksheet.Cells[headerRange].Style.Font.Size = 14;
                                worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                                worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);

                                worksheet.Cells[headerRange].AutoFitColumns();
                                worksheet.Cells[headerRange].LoadFromArrays(deviceCells);
                                fila++;

                                var totalViajesCell = new List<string[]>()
                                {
                                    new string[] { "Total de Viajes : " + data.TotalTrips, "", "", "", ""}
                                };

                                headerRange = string.Empty;
                                celda = string.Empty;
                                celda = "B" + fila + ":";
                                headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;

                                worksheet.Cells[headerRange].Merge = true;
                                worksheet.Cells[headerRange].Style.Font.Bold = true;
                                worksheet.Cells[headerRange].Style.Font.Size = 14;
                                worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                                worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PowderBlue);

                                worksheet.Cells[headerRange].AutoFitColumns();
                                worksheet.Cells[headerRange].LoadFromArrays(totalViajesCell);
                                fila++;
                            }


                            var deviceCell = new List<string[]>()
                            {
                                new string[] { data.Date.ToString("dd MMMM yyyy"), data.Date.ToString("hh:mm tt"), data.TotalMilage.ToString(), data.TotalFuel.ToString() }
                            };

                            headerRange = string.Empty;
                            celda = string.Empty;
                            celda = "B" + fila + ":";
                            headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;

                            worksheet.Cells[headerRange].AutoFitColumns();
                            worksheet.Cells[headerRange].LoadFromArrays(deviceCell);
                            fila++;
                        }
                    }

                    DateTime fecha = DateTime.Today;
                    FileInfo excelFile = new FileInfo(@"C:\Spider\ExcelReportes\Trips.xlsx");
                    excel.SaveAs(excelFile);
                }
                return 0;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int excelGps(List<ReportGPS> listGps)
        {
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("Reporte");

                    var headerTitulo = new List<string[]>()
                    {
                        new string[] { "Reporte de GPS", "","","","", ""}
                    };

                    int fila = 1;
                    string celda = "B" + fila + ":";
                    string headerRange = celda + Char.ConvertFromUtf32(headerTitulo[0].Length + 64) + fila;
                    fila++;


                    // Target a worksheet
                    var worksheet = excel.Workbook.Worksheets["Reporte"];

                    worksheet.Cells[headerRange].Merge = true;
                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                    worksheet.Cells[headerRange].Style.Font.Size = 14;
                    worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkSeaGreen);
                    worksheet.Cells[headerRange].LoadFromArrays(headerTitulo);
                    worksheet.Cells[headerRange].AutoFitColumns();


                    //Periodo
                    string periodo = DateTime.Today.ToString("dd/MM/yyyy");
                    var headerPeriodo = new List<string[]>()
                    {
                        new string[] { "Periodo: ", periodo, "" }
                    };

                    fila++;
                    celda = "A" + fila + ":";
                    headerRange = celda + Char.ConvertFromUtf32(headerTitulo[0].Length + 64) + fila;
                    fila++;

                    // Target a worksheet
                    //worksheet.Cells[headerRange].Merge = true;

                    
                    //worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    //worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkSeaGreen);
                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                    worksheet.Cells[headerRange].Style.Font.Size = 12;
                    worksheet.Cells[headerRange].AutoFitColumns();
                    worksheet.Cells[headerRange].LoadFromArrays(headerPeriodo);

                    fila++;

                    //Datos
                    var headerRow = new List<string[]>()
                    {
                        new string[] { "", "Fecha", "Latitud",  "Longitud", "Speed", "RPM" }
                    };


                    celda = "A" + fila + ":";
                    headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;
                    fila++;

                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                    worksheet.Cells[headerRange].Style.Font.Size = 14;
                    worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                    worksheet.Cells[headerRange].AutoFitColumns();

                    string device = string.Empty;
                    string dato = string.Empty;

                   
                    foreach (ReportGPS data in listGps)
                    {

                        var dataCell = new List<string[]>()
                            {
                                new string[] { "",data.Date.ToString(), data.Latitude, data.Longitude, data.Speed.ToString(), data.RPM}
                            };

                        headerRange = string.Empty;
                        celda = string.Empty;
                        celda = "A" + fila + ":";
                        headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;

                        worksheet.Cells[headerRange].AutoFitColumns();
                        worksheet.Cells[headerRange].LoadFromArrays(dataCell);




                        //var dataCell = new List<string[]>()
                        //{
                        //    new string[] { "", data.Date.ToString(), data.Latitude, data.Longitude, data.Speed.ToString(), data.RPM}
                        //};

                        //headerRange = string.Empty;
                        //celda = string.Empty;
                        //celda = "B" + fila + ":";
                        //headerRange = celda + Char.ConvertFromUtf32(headerRow[0].Length + 64) + fila;

                        ////worksheet.Cells[headerRange].Merge = true;
                        ////worksheet.Cells[headerRange].Style.Font.Bold = true;
                        //worksheet.Cells[headerRange].Style.Font.Size = 10;
                        ////worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        //worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ////worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);

                        //worksheet.Cells[headerRange].AutoFitColumns();
                        //worksheet.Cells[headerRange].LoadFromArrays(dataCell);
                        fila++;

                    }
                    

                    DateTime fecha = DateTime.Today;
                    FileInfo excelFile = new FileInfo(@"C:\Spider\ExcelReportes\GPS.xlsx");
                    excel.SaveAs(excelFile);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}