using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.ReportAdmin;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.Login;
using SpiderFleetWebAPI.Models.Response.ReportAdmin;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.ReportAdmin
{
    public class ReportLastStatusDao
    {
        private MongoDBContext mongoDBContext = new MongoDBContext();
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public ReportLastStatusDao()
        {

        }

        public ReportLastStatusDevice ReadLastStatusDevice(string device)
        {
            ReportLastStatusDevice statusDevice = new ReportLastStatusDevice();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_reporte_lino_login", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            statusDevice = new ReportLastStatusDevice();
                            statusDevice.Login = Convert.ToString(reader["login"]);
                            statusDevice.Nombre = Convert.ToString(reader["nombre"]);
                            statusDevice.FechaLogin = Convert.ToDateTime(reader["fecha_login"]).ToString("dd/MM/yyyy HH:mm:ss");
                            statusDevice.Mode = Convert.ToString(reader["Mode"]);
                            statusDevice.GPS = Convert.ToString(reader["gps"]);
                            statusDevice.FechaGPS = Convert.ToDateTime(reader["fecha_gps"]).ToString("dd/MM/yyyy HH:mm:ss");
                            //string[] version = new string[2];
                            //version = GetVersion(device);
                            GetVersion(device, statusDevice);
                            //statusDevice.VersionSW = version[0];
                            //statusDevice.VersionHW = version[1];
                        }
                        reader.Close();
                    }
                }
                else
                {
                    statusDevice = new ReportLastStatusDevice();
                    statusDevice.Login = string.Empty;
                    statusDevice.Nombre = string.Empty;
                    statusDevice.FechaLogin = string.Empty;
                    statusDevice.Mode = string.Empty;
                    statusDevice.GPS = string.Empty;
                    statusDevice.FechaGPS = string.Empty;
                    statusDevice.VersionSW = string.Empty;
                    statusDevice.VersionHW = string.Empty;
                }
            }
            catch (Exception ex)
            {
                //response.success = false;
                //response.messages.Add(ex.Message);
                return statusDevice;
            }
            finally
            {
                cn.Close();
            }
            return statusDevice;
        }

        private void GetVersion(string device, ReportLastStatusDevice statusDevice)
        {
            string[] version = new string[2];

            try
            {
                var idVehiculo = device;

                var buildTrips = Builders<Login>.Filter;
                var filterTripsIdVehiculo = buildTrips.Eq("device", idVehiculo);

                var StoredTripData = mongoDBContext.spiderMongoDatabase.GetCollection<Login>("Login");
                var result = StoredTripData.Find(filterTripsIdVehiculo).Sort("{date: -1}").FirstOrDefault();

                if (result != null)
                {
                    statusDevice.VersionSW = result.Version_sw;
                    statusDevice.VersionHW = result.Version_hw;
                }
                else
                {
                    statusDevice.VersionSW = string.Empty;
                    statusDevice.VersionHW = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            //return version;
        }

        public List<ReportLastStatusAlarms> ReadLastStatusAlarms(string device)
        {
            ReportLastStatusAlarms statusAlarm = new ReportLastStatusAlarms();
            List<ReportLastStatusAlarms> listAlarms = new List<ReportLastStatusAlarms>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_reporte_lino_alarms", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            statusAlarm = new ReportLastStatusAlarms();
                            statusAlarm.Alarm = Convert.ToString(reader["alarm"]);
                            statusAlarm.TypeAlarm = alarm(statusAlarm.Alarm);
                            statusAlarm.FechaAlarma = Convert.ToDateTime(reader["fecha_alarma"]).ToString("dd/MM/yyyy HH:mm:ss");
                            listAlarms.Add(statusAlarm);
                        }
                        reader.Close();
                    }
                }
                else
                {
                    listAlarms = new List<ReportLastStatusAlarms>();
                }
            }
            catch (Exception ex)
            {
                //response.success = false;
                //response.messages.Add(ex.Message);
                return listAlarms;
            }
            finally
            {
                cn.Close();
            }
            return listAlarms;
        }


        private string alarm(string code)
        {
            string typeAlarm = string.Empty;
            switch (code)
            {
                case "01":
                    typeAlarm = "Speeding";
                    break;
                case "02":
                    typeAlarm = "Low Voltage";
                    break;
                case "03":
                    typeAlarm = "High Engine Coolant Temperature";
                    break;
                case "04":
                    typeAlarm = "Hard Acceleration";
                    break;
                case "05":
                    typeAlarm = "Hard Deceleration";
                    break;
                case "06":
                    typeAlarm = "Idle Engine";
                    break;
                case "07":
                    typeAlarm = "Towing";
                    break;
                case "08":
                    typeAlarm = "High RPM";
                    break;
                case "09":
                    typeAlarm = "Power On";
                    break;
                case "0A":
                    typeAlarm = "Exhaust Emission";
                    break;
                case "0B":
                    typeAlarm = "Quick Lane Change";
                    break;
                case "0C":
                    typeAlarm = "Sharp Turn";
                    break;
                case "0D":
                    typeAlarm = "Fatigue Driving";
                    break;
                case "0E":
                    typeAlarm = "Power Off";
                    break;
                case "0F":
                    typeAlarm = "Geo-fence";
                    break;
                case "10":
                    typeAlarm = "Exhaust Emission";
                    break;
                case "11":
                    typeAlarm = "Emergency";
                    break;
                case "12":
                    typeAlarm = "Tamper";
                    break;
                case "13":
                    typeAlarm = "Illegal Enter";
                    break;
                case "14":
                    typeAlarm = "Illegal Ignition";
                    break;
                case "15":
                    typeAlarm = "OBD Communication Error";
                    break;
                case "16":
                    typeAlarm = "Ignition On";
                    break;
                case "17":
                    typeAlarm = "Ignition Off";
                    break;
                case "18":
                    typeAlarm = "MIL alarm";
                    break;
                case "19":
                    typeAlarm = "Unlock Alarm";
                    break;
                case "1A":
                    typeAlarm = "No Card Presented";
                    break;
                case "1B":
                    typeAlarm = "Dangerous Driving";
                    break;
                case "1C":
                    typeAlarm = "Vibration";
                    break;
                default:
                    typeAlarm = "Unknow Alarm";
                    break;
            }
            return typeAlarm;
        }
    }
}