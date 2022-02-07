using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Main.Reports;
using SpiderFleetWebAPI.Models.Response.Sims;
using SpiderFleetWebAPI.Utils.Main.LastPositionDevice;
using SpiderFleetWebAPI.Utils.Sims;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Main.Reports
{
    public class ReportByCompanyDao
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public List<ReportByCompany> ReadReportLastEstatus(string empresa)
        {
            //LastPositionDeviceResponse response = new LastPositionDeviceResponse();
            List<ReportByCompany> listLastPosition = new List<ReportByCompany>();
            ReportByCompany data = new ReportByCompany();
            Dictionary<string, string> map = new Dictionary<string, string>();
            Dictionary<string, string> credits = new Dictionary<string, string>();

            ReportCreditSimsResponse response = new ReportCreditSimsResponse();

            try
            {
                response = (new SimsMaintenanceDao()).ReporteCreditoSims();

                if (response.listReportSims.Count > 0)
                {
                    foreach (CredencialSpiderFleet.Models.Sims.ReportSims credit in response.listReportSims)
                    {
                        if (!credits.ContainsKey(credit.Device))
                        {
                            credits.Add(credit.Device, credit.Saldo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(credits.Count + "");
            }


            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_report_last_estatus", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@empresa", Convert.ToString(empresa)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            data = new ReportByCompany();
                            data.Hierarchy = Convert.ToString(reader["hierarchy"]);
                            data.Device = Convert.ToString(reader["ID_Vehiculo"]);
                            data.Name = Convert.ToString(reader["Nombre"]);
                            data.Date = Convert.ToDateTime(reader["date_gps"]);
                            data.Latitude = Convert.ToString(reader["latitude_gps"]);
                            data.Longitude = Convert.ToString(reader["longitude_gps"]);
                            data.Event = Convert.ToString(reader["event_gps"]);
                            data.Empresa = Convert.ToString(reader["Empresa"]);
                            data.Mode = Convert.ToString(reader["mode"]);
                            data.Alarma = alarms(Convert.ToString(reader["tipo_alarma"]));
                            data.IdSim = Convert.ToString(reader["id_sim"]);
                            data.Sim = Convert.ToString(reader["sim"]);

                            //string[] version = new string[2];
                            //version = (new LastPositionDevicesDao()).LastLogin(data.Device);
                            data.VersionSW = "";// version[0];
                            data.VersionHW = "";//version[1];

                            //data.Saldo = credits.Where(p => p.Value == data.Device).FirstOrDefault().Key;
                            if (credits.ContainsKey(data.Device))
                            {
                                data.Saldo = credits[data.Device].ToString();
                            }
                            else
                            {
                                data.Saldo = "0.00";
                            }

                            //int secs = (int)DateTime.Now.Subtract(bases).TotalSeconds;
                            DateTime timeUtc = DateTime.UtcNow;
                            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
                            int secs = (int)cstTime.Subtract(Convert.ToDateTime(data.Date)).TotalSeconds;


                            //int secs = (int)DateTime.Now.Subtract(Convert.ToDateTime(data.Date)).TotalSeconds;

                            //1. Activo
                            //2. Inactivo
                            //3. Warning
                            //4. Falla
                            //5. Activo sin Movimiento
                            //6. Paro de Motor
                            //7. Panico

                            if (data.Event.Equals("GPS"))
                            {
                                if (secs <= 300)
                                {
                                    data.StatusEvents = "Activo";// 1;
                                }
                                else if (secs > 300 & secs <= 3600) //Menor a 1 Hora
                                {
                                    data.StatusEvents = "Activo sin Movimiento";//5;
                                }
                                else if (secs > 3600) //Mayor a 1 Hora
                                {
                                    string evento = (new LastPositionDevicesDao()).LastAlarm(data.Device);
                                    if (evento == "Ignition Off")
                                    {
                                        data.StatusEvents = "Inactivo";//2;
                                    }
                                    else
                                    {
                                        data.StatusEvents = "Warning";//3;
                                    }
                                }
                            }
                            else if (data.Event.Equals("Sleep"))
                            {
                                if (secs < 3600 )
                                {
                                    data.StatusEvents = "Inactivo";//2;
                                }
                                else if (secs > 3600 & secs < 4200) //Mayor a 1 hora y Menor a 1 hora 10 minutos
                                {
                                    data.StatusEvents = "Inactivo";//2;
                                }
                                else if (secs > 4200 & secs < 86400) // 3 Horas 10800
                                {
                                    data.StatusEvents = "Warning";//3;
                                }
                                else if (secs > 86400) // 24 horas 86400
                                {
                                    data.StatusEvents = "Falla";//4;
                                }
                            }

                            if (!map.ContainsKey(data.Device))
                            {
                                map.Add(data.Device, data.Device);
                                listLastPosition.Add(data);
                            }
                        }
                        reader.Close();
                        //response.ListLastPosition = listLastPosition;
                        //response.success = true;
                    }
                }
                else
                {
                    //response.success = false;
                }
            }
            catch (Exception ex)
            {
                //response.success = false;
                //response.messages.Add(ex.Message);
                //return response;
            }
            finally
            {
                cn.Close();
            }
            return listLastPosition;
        }

        private string alarms(string alarm)
        {
            string valor = string.Empty;

            switch (alarm)
            {
                case "01":
                    valor = "Speeding";
                    break;
                case "02":
                    valor = "Low Voltage";
                    break;
                case "03":
                    valor = "High Engine Coolant Temperature";
                    break;
                case "04":
                    valor = "Hard Acceleration";
                    break;
                case "05":
                    valor = "Hard Deceleration";
                    break;
                case "06":
                    valor = "Idle Engine";
                    break;
                case "07":
                    valor = "Towing";
                    break;
                case "08":
                    valor = "High RPM";
                    break;
                case "09":
                    valor = "Power On";
                    break;
                case "0A":
                    valor = "Exhaust Emission";
                    break;
                case "0B":
                    valor = "Quick Lane Change";
                    break;
                case "0C":
                    valor = "Sharp Turn";
                    break;
                case "0D":
                    valor = "Fatigue Driving";
                    break;
                case "0E":
                    valor = "Power Off";
                    break;
                case "0F":
                    valor = "Geo-fence";
                    break;
                case "10":
                    valor = "Exhaust Emission";
                    break;
                case "11":
                    valor = "Emergency";
                    break;
                case "12":
                    valor = "Tamper";
                    break;
                case "13":
                    valor = "Illegal Enter";
                    break;
                case "14":
                    valor = "Illegal Ignition";
                    break;
                case "15":
                    valor = "OBD Communication Error";
                    break;
                case "16":
                    valor = "Ignition On";
                    break;
                case "17":
                    valor = "Ignition Off";
                    break;
                case "18":
                    valor = "MIL alarm";
                    break;
                case "19":
                    valor = "Unlock Alarm";
                    break;
                case "1A":
                    valor = "No Card Presented";
                    break;
                case "1B":
                    valor = "Dangerous Driving";
                    break;
                case "1C":
                    valor = "Vibration";
                    break;
                default:
                    valor = "Unknow Alarm";
                    break;
            }
            return valor;
        }
    }
}