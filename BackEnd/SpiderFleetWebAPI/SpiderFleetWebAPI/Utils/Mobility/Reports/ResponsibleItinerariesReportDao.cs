using CredencialSpiderFleet.Models.Useful;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using SpiderFleetWebAPI.Models.Response.Mobility.InfoResponsibles;
using SpiderFleetWebAPI.Utils.Logo;
using SpiderFleetWebAPI.Utils.Mobility.InfoResponsibles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Hosting;
using ICSharpCode.SharpZipLib.Zip;
using System.Threading.Tasks;
using OfficeOpenXml.Style;

namespace SpiderFleetWebAPI.Utils.Mobility.Reports
{
    public class ResponsibleItinerariesReportDao
    {
        private readonly string path = HostingEnvironment.MapPath("/TempFiles/");
        private UseFul use = new UseFul();

        public async Task<MemoryStream> GeneraExcel(DateTime start, DateTime end, string device, string hierarchy, string fileName, string nombreVehiculo)
        {
            InfoResponsiblesResponse response = new InfoResponsiblesResponse();
            response = await (new InfoResponsiblesDao()).GetAllResponsibles(device, start, end, hierarchy);

            var tempOutput = string.Empty;

            #region 
            MemoryStream memoryStream = new MemoryStream();
            WebClient client = new WebClient();
            string parametro = use.nodePrincipal(hierarchy);

            try
            {
                memoryStream = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/reportes/TemplateItinerariesResponsibles.xlsx"));
            }
            finally
            {
                client.Dispose();
            }

            #endregion

            List<string> filesExcel = new List<string>();

            tempOutput = path + fileName;

            #region Logo de Empresa

            //Imagen           
            int Height = 0;
            int Width = 0;
            int rowIndex = 0;
            int colIndex = 0;
            string nameImage = string.Empty;

            WebClient clientImage = new WebClient();
            MemoryStream memoryImage = new MemoryStream();
            try
            {

                CredencialSpiderFleet.Models.Logo.Logo logo = new CredencialSpiderFleet.Models.Logo.Logo();
                string company = UseFul.NumberEmpresa(parametro);

                logo = (new LogoDao()).ReadId(company);
                if (logo.Name == null)
                {
                    logo = (new LogoDao()).ReadId("0");
                }

                memoryImage = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/logo/" + logo.Name));
                nameImage = "Spider_" + company;
                Height = 80;
                Width = 270;
                rowIndex = 0;
                colIndex = 0;
            }
            catch (Exception ex)
            {
                clientImage.Dispose();
            }
            #endregion

            MemoryStream memoryZip = new MemoryStream();

            Excel(response.ListResponsibles, tempOutput, path, start, end, device, memoryStream, nombreVehiculo,
                memoryImage, nameImage, rowIndex, colIndex, Height, Width, filesExcel);

            memoryZip = ZipDocuments(tempOutput, filesExcel);

            return memoryZip;
        }

        private void Excel(List<CredencialSpiderFleet.Models.Mobility.InfoResponsibles.InfoResponsibles> ListResponsibles,
            string tempOutput, string path,
            DateTime fechaInicio, DateTime fechaFin, string device,
            MemoryStream memoryStream, string nombreVehiculo,
            MemoryStream memoryImage, string nameImage, int rowIndex, int colIndex, int Height, int Width,
        List<string> filesExcel)
        {
            try
            {
                MemoryStream ms = new MemoryStream();

                //Envia datos a Excel
                using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
                {

                    ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets[1];

                    //Imagen
                    System.Drawing.Image img = System.Drawing.Image.FromStream(memoryImage);
                    ExcelPicture pic = excelWorksheet.Drawings.AddPicture("imageVista-" + nameImage, img);
                    pic.SetPosition(rowIndex, 0, colIndex + ((1 - 1) * 4), 0);
                    pic.SetSize(Width, Height);

                    int rows = 7;
                    int columns = 5;

                    TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                    DateTime nowIni = TimeZoneInfo.ConvertTimeFromUtc(fechaInicio, cstZone);

                    DateTime nowFin = TimeZoneInfo.ConvertTimeFromUtc(fechaFin, cstZone);

                    excelWorksheet.Cells[3, 6].Value = nombreVehiculo;
                    excelWorksheet.Cells[4, 6].Value = fechaInicio.ToString("dd-MM-yyyy") + " al " + fechaFin.ToString("dd-MM-yyyy");

                    foreach (var data in ListResponsibles)
                    {
                        excelWorksheet.Cells[rows, columns].Style.Font.Bold = true;
                        excelWorksheet.Cells[rows, columns].Value = data.Name; columns++;
                        excelWorksheet.Cells[rows, columns].Style.Font.Bold = true;
                        excelWorksheet.Cells[rows, columns].Value = data.Notes; columns++;
                        excelWorksheet.Cells[rows, columns].Style.Font.Bold = true;
                        excelWorksheet.Cells[rows, columns].Value = data.Start; columns++;
                        excelWorksheet.Cells[rows, columns].Style.Font.Bold = true;
                        excelWorksheet.Cells[rows, columns].Value = data.End; columns++;
                        excelWorksheet.Cells[rows, columns].Style.Font.Bold = true;
                        excelWorksheet.Cells[rows, columns].Value = data.ListItineraries.Count(); columns++;

                        if (data.ListItineraries.Count > 0 )
                        {
                            int count = 1;
                            foreach (var viaje in data.ListItineraries)
                            {
                                columns = 6;
                                rows++; 

                                DateTime timeIni = viaje.StartDate;

                                string fechaini = timeIni.ToString("yyyy-MM-dd");
                                string formatIni = fechaini + "T" + viaje.StartDate.ToString("HH:mm:ss") + "Z";

                                DateTime timeFin = viaje.EndDate;

                                string fechaFn = timeFin.ToString("yyyy-MM-dd");
                                string formatFin = fechaFn + "T" + viaje.EndDate.ToString("HH:mm:ss") + "Z";
                                string link = "http://spiderfleetapi.azurewebsites.net/#/trip/" + device + "/" + formatIni + "/" + formatFin;
                                ExcelHyperLink hl = new ExcelHyperLink(link);
                                hl.Display = "Link";

                                //excelWorksheet.Cells[rows, columns].Style.Font.Color.SetColor(Color.Silver);
                                excelWorksheet.Cells[rows, columns].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                excelWorksheet.Cells[rows, columns].Style.Font.Bold = false;
                                excelWorksheet.Cells[rows, columns].Value = count.ToString(); columns++;
                                excelWorksheet.Cells[rows, columns].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                excelWorksheet.Cells[rows, columns].Style.Font.Bold = false;
                                excelWorksheet.Cells[rows, columns].Hyperlink = hl; columns++;
                                count++;
                            }
                        }
                        else
                        {
                            rows++;
                            excelWorksheet.Cells[rows, columns].Value = "Viajes"; columns++;
                            excelWorksheet.Cells[rows, columns].Value = data.ListItineraries.Count(); columns++;
                        }
                        

                        rows++;
                        columns = 5;
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public MemoryStream ZipDocuments(string tempOutput, List<string> filesExcel)
        {
            MemoryStream memoryZip = new MemoryStream();
            try
            {
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
    }
}