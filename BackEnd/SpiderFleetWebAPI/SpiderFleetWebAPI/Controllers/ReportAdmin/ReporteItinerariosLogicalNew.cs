using CredencialSpiderFleet.Models.ApiGoogle;
using CredencialSpiderFleet.Models.Configuration;
using CredencialSpiderFleet.Models.Useful;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using SpiderFleetWebAPI.Models.Response.ReportAdmin;
using SpiderFleetWebAPI.Models.Response.Responsible;
using SpiderFleetWebAPI.Utils.ReportAdmin;
using SpiderFleetWebAPI.Utils.Responsible;
using SpiderFleetWebAPI.Utils.VerifyUser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SpiderFleetWebAPI.Controllers.ReportAdmin
{
    public class ReporteItinerariosLogicalNew
    {

        private UseFul use = new UseFul();
        private VariableConfiguration configuration = new VariableConfiguration();

        public MemoryStream Reporte
           (int type, bool band, string tempOutput, string path,
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
                    reportePetrocarrier(path, fechaInicio, fechaFin,// maxVelocidad,
                        memoryStream, nombreVehiculo,
                        memoryImage, nameImage, rowIndex, colIndex, Height, Width,
                        filesExcel, devices);
                }
                else if (type == 1)
                {
                    reporteGeneral(band, path, fechaInicio, fechaFin, maxVelocidad,
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

        private void reportePetrocarrier(string path, DateTime fechaInicio, DateTime fechaFin, //decimal maxVelocidad,
            MemoryStream memoryStream, string nombreVehiculo,
            MemoryStream memoryImage, string nameImage, int rowIndex, int colIndex, int Height, int Width,
        List<string> filesExcel, List<string> devices)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                ReportItinerarioSpecialResponse information = new ReportItinerarioSpecialResponse();

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

                        int horas = VerifyUser.GetHours();

                        nowIni = nowIni.AddHours(horas);
                        nowFin = nowFin.AddHours(horas);
                        nowFin = nowFin.AddHours(horas);

                        information = (new ReportItinerarioDao()).Read(device, nowIni, nowFin);

                        excelWorksheet.Cells[3, 6].Value = responsible.responsible.Vehicle;
                        excelWorksheet.Cells[6, 4].Value = fechaInicio.ToString("dd-MM-yyyy") + " al " + fechaFin.ToString("dd-MM-yyyy");
                        excelWorksheet.Cells[7, 4].Value = responsible.responsible.Responsible;

                        if (information.itinerarios.Count > 0)
                        {
                            int count = information.itinerarios.Count;

                            totalMetros = information.itinerarios[count - 1].Distancia - information.itinerarios[0].Distancia;

                            totalSegundos = Convert.ToInt32(information.itinerarios[count - 1].Tiempo);

                            foreach (var data in information.itinerarios)
                            {
                                excelWorksheet.Cells[rows, columns].Value = data.Viaje; columns++;
                                excelWorksheet.Cells[rows, columns].Value = data.Fecha; columns++;
                                excelWorksheet.Cells[rows, columns].Value = data.Inicio; columns++;

                                excelWorksheet.Cells[rows, columns].Value = data.Latitude; columns++;
                                excelWorksheet.Cells[rows, columns].Value = data.Longitude; columns++;

                                string tiempo = string.Empty;
                                string time = string.Empty;
                                if (!data.Tiempo.Equals(""))
                                {
                                    tiempo = use.CalcularTiempo(Convert.ToInt32(data.Tiempo));
                                    time = data.Tiempo;

                                }
                                else
                                {
                                    time = "0";
                                    tiempo = "00:00";
                                }

                                excelWorksheet.Cells[rows, columns].Value = data.Velocidad; columns++;
                                columns++; columns++; columns++;

                                rows++;
                                columns = 2;
                            }

                        }

                        excelWorksheet.Cells[9, 4].Value = use.metrosKilometros(totalMetros) + " Km";
                        excelWorksheet.Cells[10, 4].Value = use.CalcularTiempo(totalSegundos) + " Hrs";
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

        private void reporteGeneral(bool band, string path, DateTime fechaInicio, DateTime fechaFin, decimal maxVelocidad,
            MemoryStream memoryStream, string nombreVehiculo,
            MemoryStream memoryImage, string nameImage, int rowIndex, int colIndex, int Height, int Width,
        List<string> filesExcel, List<string> devices)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                ReportItinerarioResponse information = new ReportItinerarioResponse();

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
                        double totalKm = 0.0;

                        TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                        DateTime nowIni = TimeZoneInfo.ConvertTimeFromUtc(fechaInicio, cstZone);

                        DateTime nowFin = TimeZoneInfo.ConvertTimeFromUtc(fechaFin, cstZone);
                        DateTime hoy = DateTime.UtcNow;
                        DateTime today = TimeZoneInfo.ConvertTimeFromUtc(hoy, cstZone);

                        int horas = VerifyUser.GetHours();
                        nowIni = nowIni.AddHours(horas);
                        nowFin = nowFin.AddHours(horas);
                        nowFin = nowFin.AddHours(horas);

                        information = (new ReportItinerarioDao()).ReadNewLogical(device, nowIni, nowFin, maxVelocidad);

                        excelWorksheet.Cells[3, 6].Value = responsible.responsible.Vehicle;
                        //fechaFin = fechaFin.AddDays(-1);
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

                        int dongle = 0;

                        foreach (var data in information.itinerarios)
                        {
                            excelWorksheet.Cells[rows, columns].Value = data.Viaje; columns++;
                            excelWorksheet.Cells[rows, columns].Value = data.Fecha; columns++;
                            excelWorksheet.Cells[rows, columns].Value = data.Inicio; columns++;
                            excelWorksheet.Cells[rows, columns].Value = data.Fin; columns++;


                            string tiempo = string.Empty;
                            dongle = data.Dongle;
                            if (data.Dongle ==1)
                            {
                                tiempo = use.CalcularTiempo(data.Tiempo);
                                totalSegundos = totalSegundos + (Convert.ToInt32(data.Tiempo));
                                excelWorksheet.Cells[rows, columns].Value = tiempo; columns++;
                                totalAceleracion = totalAceleracion + data.Aceleracion;
                                excelWorksheet.Cells[rows, columns].Value = data.Aceleracion; columns++;
                                totalFrenado = totalFrenado + data.Frenado;
                                excelWorksheet.Cells[rows, columns].Value = data.Frenado; columns++;
                                totalRPM = totalRPM + data.RPM;
                                excelWorksheet.Cells[rows, columns].Value = data.RPM; columns++;
                                totalVelocidad = totalVelocidad + data.Velocidad;
                                excelWorksheet.Cells[rows, columns].Value = data.Velocidad; columns++;

                                string distancia = string.Empty;
                                distancia = use.metrosKilometros(data.Distancia);
                                totalMetros = totalMetros + (data.Distancia);

                                excelWorksheet.Cells[rows, columns].Value = distancia; columns++;
                                
                                //double consumo = (Convert.ToDouble(distancia) * 1) / 10;
                                //excelWorksheet.Cells[rows, columns].Value = Math.Round(consumo, 2).ToString(); columns++;
                                //totalConsumo = totalConsumo + consumo;

                                excelWorksheet.Cells[rows, columns].Value = Math.Round(((data.Consumo / 1000) * 3.785), 2).ToString(); columns++;
                                totalConsumo += Math.Round(((data.Consumo / 1000) * 3.785), 2);
                            }
                            else
                            {
                                tiempo = use.CalcularTiempo(data.Tiempo);
                                totalSegundos = totalSegundos + (Convert.ToInt32(data.Tiempo));
                                excelWorksheet.Cells[rows, columns].Value = tiempo; columns++;

                                excelWorksheet.Cells[rows, columns].Value = data.Aceleracion; columns++; // data.Aceleracion; columns++;
                                excelWorksheet.Cells[rows, columns].Value = data.Frenado; columns++; // data.Frenado; columns++;
                                excelWorksheet.Cells[rows, columns].Value = data.RPM; columns++; // data.RPM; columns++;
                                excelWorksheet.Cells[rows, columns].Value = data.Velocidad; columns++; // data.Velocidad; columns++;

                                excelWorksheet.Cells[rows, columns].Value = Math.Round(Convert.ToDouble(data.Km.ToString()), 2); columns++;
                                excelWorksheet.Cells[rows, columns].Value = Math.Round(Convert.ToDouble(data.Gas.ToString()),2); columns++;

                                totalConsumo += Convert.ToDouble(data.Gas.ToString());
                                totalKm += Math.Round(Convert.ToDouble(data.Km.ToString()));
                            }

                            
                         

                            DateTime timeIni = data.FechaInicio;

                            string fechaini = timeIni.ToString("yyyy-MM-dd");
                            string formatIni = fechaini + "T" + data.FechaInicio.ToString("HH:mm:ss") + "Z";

                            DateTime timeFin = data.FechaFin;

                            string fechaFn = timeFin.ToString("yyyy-MM-dd");
                            string formatFin = fechaFn + "T" + data.FechaFin.ToString("HH:mm:ss") + "Z";
                            string link = "http://spiderfleetapi.azurewebsites.net/#/trip/" + device + "/" + formatIni + "/" + formatFin;

                            columns++; columns++; columns++;

                            ExcelHyperLink hl = new ExcelHyperLink(link);
                            hl.Display = "Link";

                            excelWorksheet.Cells[rows, columns].Hyperlink = hl; columns++; columns++;

                            if (band)
                            {
                                var url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + data.Latitud + "," + data.Longitud + "&key=" + configuration.snap;
                                var result = new WebClient().DownloadString(url);
                                GoogleGeoCodeResponse test = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(result);
                                var direccion = UseFul.ToUTF8(test.results[0].formatted_address.ToString());

                                excelWorksheet.Cells[rows, columns].Value = direccion; columns++;
                                excelWorksheet.Cells[rows, columns].Value = data.Responsable; columns++;
                            }
                            else
                            {
                                //responsable
                                excelWorksheet.Cells[rows, columns].Value = data.Responsable; columns++;
                            }

                            rows++;
                            columns = 2;
                        }

                        if(dongle == 1)
                        {
                            excelWorksheet.Cells[8, 4].Value = Convert.ToString(Math.Round(Convert.ToDouble(use.metrosKilometros(totalMetros)) / Math.Round(totalConsumo, 2), 2)) + " Km/L";
                            excelWorksheet.Cells[9, 4].Value = use.metrosKilometros(totalMetros) + " Km";
                            excelWorksheet.Cells[10, 4].Value = use.CalcularTiempo(totalSegundos) + " Hrs";
                            excelWorksheet.Cells[11, 4].Value = Math.Round(totalConsumo, 2).ToString() + " Litros";

                            excelWorksheet.Cells[8, 7].Value = totalAceleracion;
                            excelWorksheet.Cells[8, 8].Value = totalFrenado;
                            excelWorksheet.Cells[8, 9].Value = totalRPM;
                            excelWorksheet.Cells[8, 10].Value = totalVelocidad;
                        }
                        else
                        {
                            excelWorksheet.Cells[8, 4].Value = Convert.ToString(Math.Round(totalKm / Math.Round(totalConsumo, 2), 2)) + " Km/L";
                            excelWorksheet.Cells[9, 4].Value = Math.Round(totalKm , 2)  + " Km";
                            excelWorksheet.Cells[10, 4].Value = use.CalcularTiempo(totalSegundos) + " Hrs";
                            excelWorksheet.Cells[11, 4].Value = Math.Round(totalConsumo, 2).ToString() + " Litros";

                            excelWorksheet.Cells[8, 7].Value = totalAceleracion;
                            excelWorksheet.Cells[8, 8].Value = totalFrenado;
                            excelWorksheet.Cells[8, 9].Value = totalRPM;
                            excelWorksheet.Cells[8, 10].Value = totalVelocidad;
                        }
                        

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

    }
}