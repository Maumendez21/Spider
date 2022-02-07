using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.DashBoard;
using CredencialSpiderFleet.Models.Main.LastPositionDevice;
using CredencialSpiderFleet.Models.Useful;
using ICSharpCode.SharpZipLib.Zip;
using MongoDB.Driver;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Response.DashBoard;
using SpiderFleetWebAPI.Models.Response.Itineraries;
using SpiderFleetWebAPI.Models.Response.Main.LastPositionDevice;
using SpiderFleetWebAPI.Utils.General;
using SpiderFleetWebAPI.Utils.Itineraries;
using SpiderFleetWebAPI.Utils.Main.LastPositionDevice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SpiderFleetWebAPI.Utils.DashBoard
{
    public class DashBoardActivityDayDao
    {
        private MongoDBContext mongoDBContext = new MongoDBContext();
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private UseFul use = new UseFul();

        #region SQL

        public async Task<DashBoardActivityDayResponse> DashBoardDay(string hierarchy, string busqueda)
        {
            DashBoardActivityDayResponse response = new DashBoardActivityDayResponse();
            try
            {
                List<CurrentPositionDevice> ListLastPosition = new List<CurrentPositionDevice>();
                ListLastPosition = await ListStatus(hierarchy, busqueda);

                List<DashBoardActivityDay> ListDay = new List<DashBoardActivityDay>();

                DateTime now = DateTime.Today;
                var startdate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                var enddate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
                

                if (ListLastPosition.Count > 0)
                {
                    foreach (var item in ListLastPosition)
                    {
                        if(item.statusEvent == 1 | item.statusEvent == 5)
                        {
                            DashBoardActivityDay dash = new DashBoardActivityDay();
                            dash.Device = item.dispositivo;
                            dash.VehicleName = item.nombre;
                            dash.ListItineraries = ListItineraries(hierarchy, dash.Device, startdate, enddate);

                            ListDay.Add(dash);
                        }

                        response.Total = ListLastPosition.Count();
                    }

                    response.Actives = ListLastPosition.Count(x => x.statusEvent == 1);
                    response.Inactives = ListLastPosition.Count(x => x.statusEvent != 1);
                    response.ListDay = ListDay;
                    response.success = true;
                }
                else
                {
                    response.success = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }

        private async Task<List<CurrentPositionDevice>> ListStatus(string hierarchy, string busqueda)
        {
            List<CurrentPositionDevice> ListLastPosition = new List<CurrentPositionDevice>();
            LastPositionDeviceResponse response = new LastPositionDeviceResponse();
            try
            {
                response = await (new LastPositionDevicesDao()).ReadCurrentPositionDevicesHierarchy(hierarchy, busqueda);
                ListLastPosition = response.ListLastPosition;
                return ListLastPosition;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return ListLastPosition;
            }
        }

        private List<CredencialSpiderFleet.Models.Itineraries.Itineraries> ListItineraries(string hierarchy, string device, DateTime startdate, DateTime enddate)
        {
            List<CredencialSpiderFleet.Models.Itineraries.Itineraries> ListItineraries = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
            ItinerariesResponse response = new ItinerariesResponse();
            try
            {
                response = (new ItinerariesDao()).ReadItinerariosDeviceList(hierarchy, device, startdate, enddate);

                ListItineraries = response.listItineraries;
                return ListItineraries;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return ListItineraries;
            }
        }

        public NotificationsResponse NotificationsPriority(string node, DateTime start, DateTime end)
        {

            NotificationsResponse response = new NotificationsResponse();
            List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority> listNotifications = new List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority>();
            CredencialSpiderFleet.Models.Itineraries.NotificationsPriority notifications = new CredencialSpiderFleet.Models.Itineraries.NotificationsPriority();

            try
            {

                int value = Convert.ToInt32((new GeneralDao()).ReadIdHerarchy(use.hierarchyPrincipalToken(node), "PN", 2));
                if (start > end)
                {
                    response.success = false;
                    response.messages.Add("La fecha de Inicio es mayor que la fecha final, favor de verificar los datos.");
                    return response;
                }

                int months = UseFul.MonthDiff(start, end);

                if (months >= value)
                {
                    response.success = false;
                    response.messages.Add("La fecha de inicio ha excedido el parametro de consulta de " + value + " meses, favor de contactar a su Administrador.");
                    return response;
                }



                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_notifications_priority_by_hierarchy_date", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    cmd.Parameters.Add(new SqlParameter("@start", start));
                    cmd.Parameters.Add(new SqlParameter("@end", end));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notifications = new CredencialSpiderFleet.Models.Itineraries.NotificationsPriority();
                            notifications.Id = Convert.ToInt32(reader["id"]);
                            notifications.Device = Convert.ToString(reader["device"]);
                            notifications.Alarm = Convert.ToString(reader["alarm"]).Trim();
                            notifications.Name = Convert.ToString(reader["name"]).Trim();
                            notifications.Latitud = Convert.ToString(reader["latitude"]).Trim();
                            notifications.Longitude = Convert.ToString(reader["longitude"]).Trim();
                            notifications.DateGenerated = Convert.ToDateTime(reader["date_generated"]);
                            notifications.View = Convert.ToInt32(reader["band"]);
                            string mongo = Convert.ToString(reader["mongo"]);

                            if (notifications.Alarm.Equals("S.O.S."))
                            {
                                notifications.Description = "Se ha activado el botón de Panico de la unidad " + notifications.Name + ",ejecutar el protocolo para esta Alarma";
                            }
                            else if (notifications.Alarm.Equals("Desconexion"))
                            {
                                notifications.Description = "Se ha desconectado el dispositivo de la unidad " + notifications.Name + "";
                            }
                            else if (notifications.Alarm.Equals("Geo Cerca"))
                            {
                                CoordinatesGeoFence coordinates = new CoordinatesGeoFence();
                                coordinates = GetNameGeoCerca(mongo);
                                notifications.Description = "La Unidad " + notifications.Name + " se ha salido de la Geo Cerca " + coordinates.name;
                                notifications.Coordinates = coordinates.Coordinates;
                            }

                            listNotifications.Add(notifications);
                        }
                        reader.Close();
                        response.success = true;
                        response.listNotifications = listNotifications;
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
            finally
            {
                cn.Close();
            }
            return response;
        }


        //Excel


        #endregion

        #region Reporte
        public MemoryStream Reporte(string hierarchy, 
            //string grupo, string device, 
            DateTime fechaInicio, DateTime fechaFin,
           string tempOutput, string path,
           MemoryStream memoryStream, MemoryStream memoryImage,
           string nameImage, int rowIndex, int colIndex, int Height, int Width,
           List<string> filesExcel)
        {
            MemoryStream memoryZip = new MemoryStream();

            try
            {
                GetExcel(hierarchy, fechaInicio, fechaFin, 
                    //grupo, device, 
                    path, memoryStream, memoryImage,
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

        private void GetExcel(string hierarchy, DateTime fechaInicio, DateTime fechaFin, 
            //string grupo, string device,
           string path, MemoryStream memoryStream, MemoryStream memoryImage,
           string nameImage, int rowIndex, int colIndex, int Height, int Width, List<string> filesExcel)
        {
            try
            {

                NotificationsResponse responseInfo = new NotificationsResponse();
                responseInfo = (new DashBoardActivityDayDao()).NotificationsPriority(hierarchy, fechaInicio, fechaFin);
                MemoryStream ms = new MemoryStream();
                List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority> listNotifications = new List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority>();

                if(responseInfo.listNotifications.Count > 0)
                {
                    listNotifications = responseInfo.listNotifications;

                    reporteGeneral(listNotifications,
                    fechaInicio, fechaFin, path, memoryStream, memoryImage, nameImage, rowIndex, colIndex, Height, Width, filesExcel, ms);
                }

            }
            catch (Exception ex)
            {

            }
        }


        private void reporteGeneral(
              List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority> listNotifications,
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
                    ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets[1];

                    #region Tiempo
                    //Imagen
                    System.Drawing.Image img = System.Drawing.Image.FromStream(memoryImage);
                    ExcelPicture pic = excelWorksheet.Drawings.AddPicture("imageVista-" + nameImage, img);
                    pic.SetPosition(rowIndex, 0, colIndex + ((1 - 1) * 4), 0);
                    pic.SetSize(Width, Height);

                    int rows = 10;
                    int columns = 2;

                    excelWorksheet.Cells[6, 5].Value = fechaInicio.ToString("dd-MM-yyyy") + " Al " + fechaFin.ToString("dd-MM-yyyy");  //Fecha

                    foreach (var item in listNotifications)
                    {
                        
                        excelWorksheet.Cells[rows, columns].Value = UseFul.DateFormatdddd_ddMMMMyyyy(item.DateGenerated); columns++;
                        excelWorksheet.Cells[rows, columns].Value = item.Device; columns++;
                        excelWorksheet.Cells[rows, columns].Value = item.Name; columns++;
                        excelWorksheet.Cells[rows, columns].Value = item.Alarm; columns++;
                        excelWorksheet.Cells[rows, columns].Value = item.Description; columns++;

                        rows++;
                        columns = 2;

                    }                   

                    excelPackage.SaveAs(ms);

                    #endregion

                    string ruta = "" + path + "Report_Analitica_Actividad" + ".xlsx";
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


        #endregion


        #region Mongo
        private CoordinatesGeoFence GetNameGeoCerca(string id)
        {
            CoordinatesGeoFence coordinates = new CoordinatesGeoFence();
            string nameGeoCerca = string.Empty;

            try
            {
                var build = Builders<Models.Mongo.GeoFence.GeoFence>.Filter;
                var filter = build.Eq(x => x.Id, id);

                var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence");
                var result = StoredTripData.Find(filter).FirstOrDefault();

                if (result != null)
                {
                    coordinates.name = result.Name;
                    coordinates.Coordinates = result.Polygon.Coordinates[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return coordinates;
        }

        #endregion

    }
}