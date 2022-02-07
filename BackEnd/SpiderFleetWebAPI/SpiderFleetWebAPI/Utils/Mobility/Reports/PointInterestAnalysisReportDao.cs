using CredencialSpiderFleet.Models.Mobility.PointInterestAnalysis;
using CredencialSpiderFleet.Models.Useful;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using SpiderFleetWebAPI.Models.Response.Mobility.PointInterestAnalysis;
using SpiderFleetWebAPI.Utils.Logo;
using SpiderFleetWebAPI.Utils.Mobility.PointInterestAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;

namespace SpiderFleetWebAPI.Utils.Mobility.Reports
{
    public class PointInterestAnalysisReportDao
    {

        private readonly string path = HostingEnvironment.MapPath("/TempFiles/");
        private UseFul use = new UseFul();

        public MemoryStream GeneraExcel(DateTime start, DateTime end, string mongo, string device, string hierarchy, string fileName, string nombreVehiculo)
        {

            PointInterestAnalysisResponse response = new PointInterestAnalysisResponse();
            response = (new PointInterestAnalysisDao()).Analysis(mongo, start, end, device);

            var tempOutput = string.Empty;

            #region 
            MemoryStream memoryStream = new MemoryStream();
            WebClient client = new WebClient();
            
            string parametro = use.nodePrincipal(hierarchy);

            try
            {
                memoryStream = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/reportes/TemplatePointInterestAnalysis.xlsx"));
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

            Excel(response.ListAnalysis, tempOutput, path, start, end, device, memoryStream, nombreVehiculo,
                memoryImage, nameImage, rowIndex, colIndex, Height, Width, filesExcel);

            memoryZip = (new ResponsibleItinerariesReportDao()).ZipDocuments(tempOutput, filesExcel);

            return memoryZip;
        }

        private void Excel(List<PointInterestAnalysisRegistry> ListAnalysis,
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

                    foreach (var data in ListAnalysis)
                    {
                        //excelWorksheet.Cells[rows, columns].Style.Font.Bold = true;
                        excelWorksheet.Cells[rows, columns].Value = data.VehicleName; columns++;
                        excelWorksheet.Cells[rows, columns].Value = data.Date; columns++;
                        excelWorksheet.Cells[rows, columns].Value = data.Time + " Hrs"; columns++;
                        
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

    }
}