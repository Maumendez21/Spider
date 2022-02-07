using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.DashBoard;
using CredencialSpiderFleet.Models.Itineraries;
using CredencialSpiderFleet.Models.Main.LastPositionDevice;
using CredencialSpiderFleet.Models.Obd;
using CredencialSpiderFleet.Models.Useful;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.GPS;
using SpiderFleetWebAPI.Models.Response.DashBoard;
using SpiderFleetWebAPI.Models.Response.Itineraries;
using SpiderFleetWebAPI.Models.Response.Main.LastPositionDevice;
using SpiderFleetWebAPI.Models.Response.Obd;
using SpiderFleetWebAPI.Utils.Obd;
using SpiderFleetWebAPI.Utils.Setting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SpiderFleetWebAPI.Utils.DashBoard
{
    public class DashBoardGeneralDao
    {
        public DashBoardGeneralDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private UseFul use = new UseFul();

        private List<CurrentPositionDevice> listLastPosition = new List<CurrentPositionDevice>();

        private MongoDBContext mongoDBContext = new MongoDBContext();
        private const string startS = "START";
        private const string endS = "END";
        private const string points = "POINTS";

        private decimal ODO = 0;
        private decimal Fuel = 0;
        private double Time = 0;

        private decimal totalDistancia = 0;
        private decimal totalLitros = 0;
        private double totalTiempo = 0;

        private Graficas Consumo = new Graficas();

        public DashBoardGeneralResponse ReadData(string hierarchy, string valor)
        {
            DashBoardGeneralResponse response = new DashBoardGeneralResponse();
            try
            {
                int totalUnidades = ReadTotalVehicles(hierarchy);

                #region Proceso de Calculo de Vehiculos Activos

                ReadCurrentPositionDevices("Flota", valor, "");

                int totalActivas = listLastPosition.Where(y => y.statusEvent == 1).Count();

                if(totalActivas == 0)
                {
                    response.TotalActivas = "0/" + totalUnidades ;
                }
                else
                {
                    //double calculo = Convert.ToDouble((totalActivas * 100) / totalUnidades);
                    //response.TotalActivas = Convert.ToString(Math.Round(calculo, 2));
                    response.TotalActivas = totalActivas + "/" + totalUnidades;
                }

                #endregion

                #region Proceso Horas de Actividad de Vehiculos
                DateTime utc = DateTime.UtcNow;

                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                DateTime now = TimeZoneInfo.ConvertTimeFromUtc(utc, cstZone);

                var start = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                var end = new DateTime(now.Year, now.Month, now.Day, 2, 0, 0);

                Graficas graficas = new Graficas();
                graficas.label.Add("0");
                graficas.label.Add("2.00");
                graficas.label.Add("4.00");
                graficas.label.Add("6.00");
                graficas.label.Add("8.00");
                graficas.label.Add("10.00");
                graficas.label.Add("12.00");
                graficas.label.Add("14.00");
                graficas.label.Add("16.00");
                graficas.label.Add("18.00");
                graficas.label.Add("20.00");
                graficas.label.Add("22.00");
                graficas.label.Add("24.00");

                graficas.data.Add("0");

                List<string> listPorcentajeVehiculos = new List<string>();

                for (int i = 0; i < 12; i++)
                {
                    int respuesta = ReadNumberVehiclesByHours(hierarchy, start, end);

                    if (respuesta == 0)
                    {
                        graficas.data.Add("0.00");
                    }
                    else
                    {
                        //double calculo = Convert.ToDouble((respuesta * 100) / totalUnidades);
                        //graficas.data.Add(Convert.ToString(Math.Round(calculo, 2)));
                        //double calculo = Convert.ToDouble((respuesta * 100) / totalUnidades);
                        graficas.data.Add(Convert.ToString(respuesta));
                    }

                    start = start.AddHours(2);
                    if (i == 10)
                    {
                        end = end.AddHours(1);
                        end = new DateTime(now.Year, now.Month, now.Day, end.Hour, 59, 59);
                    }
                    else
                    {
                        end = end.AddHours(2);
                    }
                }

                response.Graficas = graficas;
                #endregion

                #region Proceso de Calculo Kilometros Recorridos

                int numeroSemana = GetSemanaAnio(now);
                DateTime firstDayOfWeek = GetFechaInicial(now.Year, numeroSemana, CultureInfo.CurrentCulture);

                var fechaInicio = new DateTime(firstDayOfWeek.Year, firstDayOfWeek.Month, firstDayOfWeek.Day, 0, 0, 0);
                var fechaFin = new DateTime(firstDayOfWeek.Year, firstDayOfWeek.Month, firstDayOfWeek.Day, 23, 59, 59);
                fechaFin = fechaFin.AddDays(6);

                GetCards(hierarchy, fechaInicio, fechaFin, hierarchy, "");

                response.TotalDistancia = totalDistancia.ToString() + " Km";
                //string l = totalLitros.ToString() + " Lts";
                response.TotalTiempo = UseFul.CalcularTime(Convert.ToInt32(totalTiempo)) + " Horas";

                response.success = true;

                #endregion

                #region Proceso de Calculo Consumo de Combustible

                #endregion

                
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
            return response;
            
        }

        public DashBoardGeneralVehicleConsumptionResponse ReadDataConsumption(string hierarchy, string group, string device)
        {
            DashBoardGeneralVehicleConsumptionResponse response = new DashBoardGeneralVehicleConsumptionResponse();
            try
            {
                DateTime utc = DateTime.UtcNow;

                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                DateTime now = TimeZoneInfo.ConvertTimeFromUtc(utc, cstZone);

                Graficas graficas = new Graficas();

                #region Proceso de Calculo Consumo de Combustible

                int numeroSemana = GetSemanaAnio(now);
                DateTime firstDayOfWeek = GetFechaInicial(now.Year, numeroSemana, CultureInfo.CurrentCulture);

                var fechaInicio = new DateTime(firstDayOfWeek.Year, firstDayOfWeek.Month, firstDayOfWeek.Day, 0, 0, 0);
                var fechaFin = new DateTime(firstDayOfWeek.Year, firstDayOfWeek.Month, firstDayOfWeek.Day, 23, 59, 59);
                fechaFin = fechaFin.AddDays(6);

                GetVehicleConsumtion(hierarchy, fechaInicio, fechaFin, group, device);

                response.GraficasConsumption = Consumo;

                response.success = true;

                #endregion
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
            return response;

        }



        private static int GetSemanaAnio(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private static DateTime GetFechaInicial(int year, int weekOfYear, CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }

        private void ReadCurrentPositionDevices(string tipo, string valor, string busqueda)
        {
            listLastPosition = new List<CurrentPositionDevice>();
            CurrentPositionDevice position = new CurrentPositionDevice();
            Dictionary<string, string> map = new Dictionary<string, string>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_last_position_device_spider", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Tipo", Convert.ToString(tipo)));
                    cmd.Parameters.Add(new SqlParameter("@Valor", Convert.ToString(valor)));
                    if (!string.IsNullOrEmpty(busqueda)) { cmd.Parameters.Add(new SqlParameter("@search", Convert.ToString(busqueda))); }
                    else { cmd.Parameters.Add(new SqlParameter("@search", "")); }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string Events = string.Empty;

                            position = new CurrentPositionDevice();
                            position.dispositivo = Convert.ToString(reader["device"]);
                            position.Events = Convert.ToString(reader["event"]);
                            position.nombre = Convert.ToString(reader["name"]);
                            position.fecha = Convert.ToDateTime(reader["date"]);
                            position.latitud = Convert.ToString(reader["latitude"]);
                            position.longitud = Convert.ToString(reader["longitude"]);
                            position.Hierarchy = Convert.ToString(reader["hierarchy"]);
                            position.empresa = Convert.ToString(reader["empresa"]);
                            position.nombreEmpresa = Convert.ToString(reader["nombre_empresa"]);

                            position.operador = string.Empty;
                            position.velocidad = string.Empty;
                            position.direccion = string.Empty;
                            position.odo = string.Empty;

                            DateTime timeUtc = DateTime.UtcNow;
                            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
                            int secs = (int)cstTime.Subtract(Convert.ToDateTime(position.fecha)).TotalSeconds;
                            //1. Activo
                            //2. Inactivo
                            //3. Warning
                            //4. Falla
                            //5. Activo sin Movimiento
                            //6. Paro de Motor
                            //7. Panico
                            //8. Desconexion  E0

                            if (position.Events.Equals("GPS"))
                            {
                                if (secs <= 300)//5 Minutos
                                {
                                    position.statusEvent = 1;
                                }
                                else if (secs > 300 & secs <= 3600) //Menor a 1 Hora
                                {
                                    string evento = LastStatusAlarm(position.dispositivo);
                                    if (evento.Equals("Ignition Off"))
                                    {
                                        position.statusEvent = 2;
                                    }
                                    else
                                    {
                                        position.statusEvent = 5;
                                    }

                                }
                                else if (secs > 3600) //Mayor a 1 Hora
                                {
                                    string evento = LastStatusAlarm(position.dispositivo);
                                    if (evento.Equals("Ignition Off"))
                                    {
                                        position.statusEvent = 2;
                                    }
                                    else
                                    {
                                        position.statusEvent = 3;
                                    }
                                }
                            }
                            else if (position.Events.Equals("Sleep"))
                            {
                                if (secs < 3600)
                                {
                                    position.statusEvent = 2;
                                }
                                else if (secs > 3600 & secs < 4200) //Mayor a 1 hora y Menor a 1 hora 10 minutos
                                {
                                    position.statusEvent = 2;
                                }
                                else if (secs > 4200 & secs < 86400) // 3 Horas 10800
                                {
                                    position.statusEvent = 3;
                                }
                                else if (secs > 86400) // 24 horas 86400
                                {
                                    position.statusEvent = 4;
                                }
                            }
                            else
                            {
                                position.statusEvent = 2;
                            }

                            if (!map.ContainsKey(position.dispositivo))
                            {
                                //1. Activo
                                //2. Inactivo
                                //3. Warning
                                //4. Falla
                                //5. Activo sin Movimiento
                                //6. Paro de Motor
                                //7. Panico
                                map.Add(position.dispositivo, position.dispositivo);
                                listLastPosition.Add(position);
                            }
                        }

                        if (listLastPosition.Count > 0)
                        {
                            foreach (var data in listLastPosition)
                            {
                                int alarm = StatusEventNotificationsPriority(data.dispositivo);
                                if (alarm > 0)
                                {
                                    data.statusEvent = 7;
                                }
                            }
                        }

                        //reader.Close();
                        //response.ListLastPosition = listLastPosition;

                        //List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority> ListNotificationsPriority = new List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority>();

                        //if (listLastPosition.Count > 0)
                        //{
                        //    ListNotificationsPriority = NotificationsPriority(listLastPosition);
                        //}

                        //response.ListNotificationsPriority = ListNotificationsPriority;

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
                throw new Exception(ex.Message);
                //response.success = false;
                //response.messages.Add(ex.Message);
                //return response;
            }
            finally
            {
                cn.Close();
            }
            //return response;
        }

        private string LastStatusAlarm(string device)
        {
            string alarm = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_last_status_alarms_by_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string date = Convert.ToString(reader["date"]);
                            string alarms = Convert.ToString(reader["alarm"]).Trim();
                            if (alarms.Equals("17"))
                            {
                                alarm = "Ignition Off";
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //finally
            //{
            //    //cn.Close();
            //}
            return alarm;
        }

        private int StatusEventNotificationsPriority(string device)
        {
            int alarm = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_status_event_notifications_priority", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            alarm = Convert.ToInt32(reader["count"]);
                        }
                    }
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
            return alarm;
        }

        private int ReadTotalVehicles(string hierarchy)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_total_vehiculos", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@searchNode", Convert.ToString(hierarchy)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            respuesta = Convert.ToInt32(reader["total_unidades"]);
                        }
                    }
                }
                else
                {
                    respuesta = 0;
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
            return respuesta;
        }

        private int ReadNumberVehiclesByHours(string hierarchy, DateTime start, DateTime end)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_actividad_vehiculo_por horas", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@searchNode", Convert.ToString(hierarchy)));
                    cmd.Parameters.Add(new SqlParameter("@start", Convert.ToDateTime(start)));
                    cmd.Parameters.Add(new SqlParameter("@end", Convert.ToDateTime(end)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            respuesta++;
                        }
                    }
                }
                else
                {
                    respuesta = 0;
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
            return respuesta;
        }

        #region Proceso de Calculo Consumo de Combustible y Kilometraje

        public void GetCards(string hierarchy, DateTime fechaInicio, DateTime fechaFin,
           string grupo, string device)
        {
            ListObdResponse listDevice = new ListObdResponse();
            List<string> listObd = new List<string>();
            ItinerariesListResponse data = new ItinerariesListResponse();

            string subGrupo = string.Empty;
            int agrega = 1;

            double m = UseFul.diferencia(fechaFin, fechaInicio);

            DateTime nowIni = fechaInicio;
            var inico = new DateTime(nowIni.Year, nowIni.Month, nowIni.Day, 0, 0, 0);
            var fin = new DateTime(nowIni.Year, nowIni.Month, nowIni.Day, 23, 59, 59);
            
            int horas = VerifyUser.VerifyUser.GetHours();
            inico = inico.AddHours(horas);
            fin = fin.AddHours(horas);

            try
            {

                string node = use.hierarchyPrincipalToken(hierarchy);
                int diff = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(node, "ITE", 1));

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

                foreach (var datas in listDevice.listObd)
                {
                    listObd.Add(datas.Device);
                }


                int n = 0;
                while (n < Convert.ToInt32(m))
                {
                    foreach (var vehiculo in listObd)
                    {
                        ReadGeneralDeviceList(diff, vehiculo, inico, fin);
                    }
                    n++;

                    inico = inico.AddDays(agrega);
                    fin = fin.AddDays(agrega);

                    totalDistancia = totalDistancia + ODO;
                    totalLitros = totalLitros + Fuel;
                    totalTiempo = totalTiempo + Time;

                    ODO = 0;
                    Fuel = 0;
                    Time = 0;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GetVehicleConsumtion(string hierarchy, DateTime fechaInicio, DateTime fechaFin,
           string group, string device)
        {
            ListObdResponse listDevice = new ListObdResponse();
            List<string> listObd = new List<string>();
            ItinerariesListResponse data = new ItinerariesListResponse();

            Consumo.label = new List<string>();
            Consumo.data = new List<string>();

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

                subGrupo = !string.IsNullOrEmpty(group) ? group : hierarchy;

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

                foreach (var datas in listDevice.listObd)
                {
                    listObd.Add(datas.Device);
                }

                int n = 0;
                while (n < Convert.ToInt32(m))
                {
                    foreach (var vehiculo in listObd)
                    {
                        ReadGeneralDeviceList(diff, vehiculo, inico, fin);
                    }
                    n++;

                    Consumo.label.Add(inico.ToString("MMMM dd", ci));
                    Consumo.data.Add(Fuel.ToString());

                    inico = inico.AddDays(agrega);
                    fin = fin.AddDays(agrega);

                    totalLitros = totalLitros + Fuel;

                    Fuel = 0;
                }

                List<string> listData = new List<string>();
                string VehicleName = string.Empty;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void GetFuelConsumption(string hierarchy, DateTime fechaInicio, DateTime fechaFin,
           string grupo, string device)
        {
            //ResponseDashBoardDLTGraficaBarra response = new ResponseDashBoardDLTGraficaBarra();
            ListObdResponse listDevice = new ListObdResponse();
            List<string> listObd = new List<string>();
            ItinerariesListResponse data = new ItinerariesListResponse();
            //int countBest = 0;
            //int countLower = 0;

            //decimal totalDistancia = 0;
            //decimal totalLitros = 0;
            //double totalTiempo = 0;

            //Graficas litros = new Graficas();
            //Graficas distancia = new Graficas();
            //Graficas tiempo = new Graficas();
            //Graficas rendimiento = new Graficas();

            //tiempo.label = new List<string>();
            //tiempo.data = new List<string>();

            //distancia.label = new List<string>();
            //distancia.data = new List<string>();

            //litros.label = new List<string>();
            //litros.data = new List<string>();

            //rendimiento.label = new List<string>();
            //rendimiento.data = new List<string>();

            string subGrupo = string.Empty;
            int agrega = 1;

            double m = UseFul.diferencia(fechaFin, fechaInicio);

            DateTime nowIni = fechaInicio;
            var inico = new DateTime(nowIni.Year, nowIni.Month, nowIni.Day, 0, 0, 0);
            var fin = new DateTime(nowIni.Year, nowIni.Month, nowIni.Day, 23, 59, 59);

            int horas = VerifyUser.VerifyUser.GetHours();
            inico = inico.AddHours(horas);
            fin = fin.AddHours(horas);

            //CultureInfo ci = new CultureInfo("es-MX");
            //ci = new CultureInfo("es-MX");

            try
            {

                string node = use.hierarchyPrincipalToken(hierarchy);
                int diff = Convert.ToInt32((new SettingConfig()).ReadIdHerarchy(node, "ITE", 1));

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

                foreach (var datas in listDevice.listObd)
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
                    //tiempo.label.Add(inico.ToString("MMMM dd", ci));
                    ////string time = UseFul.Calcular(Convert.ToInt32(Time));
                    //tiempo.data.Add(UseFul.CalcularTime(Convert.ToInt32(Time.ToString())).Replace(":", ""));

                    //distancia.label.Add(inico.ToString("MMMM dd", ci));
                    //distancia.data.Add(ODO.ToString());

                    //litros.label.Add(inico.ToString("MMMM dd", ci));
                    //litros.data.Add(Fuel.ToString());

                    //rendimiento.label.Add(inico.ToString("MMMM dd", ci));
                    //if (ODO == 0 & Fuel == 0)
                    //{
                    //    rendimiento.data.Add(0 + "");
                    //}
                    //else
                    //{
                    //    rendimiento.data.Add(Convert.ToString(Math.Round((ODO / Fuel), 2)));
                    //}


                    inico = inico.AddDays(agrega);
                    fin = fin.AddDays(agrega);

                    totalDistancia = totalDistancia + ODO;
                    totalLitros = totalLitros + Fuel;
                    totalTiempo = totalTiempo + Time;

                    ODO = 0;
                    Fuel = 0;
                    Time = 0;

                }

                //response.graficas.graficaDistancia = distancia;
                //response.graficas.graficaLitros = litros;
                //response.graficas.graficaTiempo = tiempo;
                //response.graficas.graficaRendimiento = rendimiento;
                //response.TotalDistancia = totalDistancia.ToString() + " Km";
                //response.TotalLitros = totalLitros.ToString() + " Lts";
                //response.TotalTiempo = UseFul.CalcularTime(Convert.ToInt32(totalTiempo)) + " Horas";
                //response.TotalRendimiento = Convert.ToString(Math.Round((totalDistancia / totalLitros), 2)) + " Km / Lt";


                //List<Ranking> ListRankingBest = new List<Ranking>();
                //List<Ranking> ListRankingLower = new List<Ranking>();

                List<string> listData = new List<string>();
                string VehicleName = string.Empty;

                //foreach (var dispositivo in listObd)
                //{
                //    VehicleName = string.Empty;
                //    listData = ReadOperatorData(dispositivo);
                //    if (listData.Count > 0)
                //    {
                //        VehicleName = listData[2];
                //    }

                //    VehicleName = (string.IsNullOrEmpty(VehicleName) ? dispositivo : VehicleName);

                //    decimal odo = listPerformance.Where(y => y.Device.Equals(dispositivo)).Sum(x => x.Obd);
                //    decimal fuel = listPerformance.Where(y => y.Device.Equals(dispositivo)).Sum(x => x.Fuel);
                //    decimal result = 0;
                //    if (odo == 0 & fuel == 0)
                //    {
                //        result = 0;
                //    }
                //    else
                //    {
                //        result = Math.Round(odo / fuel, 2);
                //    }

                //    if (!listRendimiento.ContainsKey(dispositivo))
                //    {
                //        listRendimiento.Add(VehicleName, Convert.ToDouble(result.ToString()));
                //    }
                //}

                //var rankinkBest = from entry in listRendimiento orderby entry.Value descending select entry;
                //var rankinkLower = from entry in listRendimiento orderby entry.Value ascending select entry;

                //foreach (var best in rankinkBest)
                //{
                //    if (ListRankingBest.Count < countBest)
                //    {
                //        Ranking ranking = new Ranking();
                //        ranking.Name = best.Key;
                //        ranking.Consume = best.Value.ToString();
                //        ListRankingBest.Add(ranking);
                //    }
                //    else
                //    {
                //        break;
                //    }
                //}

                //foreach (var lower in rankinkLower)
                //{
                //    if (ListRankingLower.Count < countLower)
                //    {
                //        Ranking ranking = new Ranking();
                //        ranking.Name = lower.Key;
                //        ranking.Consume = lower.Value.ToString();
                //        ListRankingLower.Add(ranking);
                //    }
                //    else
                //    {
                //        break;
                //    }
                //}

                //response.ListRankingBest = ListRankingBest;
                //response.ListRankingLower = ListRankingLower;

                //response.success = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //response.success = false;
                //response.messages.Add(ex.Message);
                //return response;
            }
            //return response;
        }

        public ListObdResponse ListDevice(string hierarchy)
        {
            ListObdResponse response = new ListObdResponse();

            try
            {
                response = (new SubCompanyAssignmentObdsDao()).ListDeviceHierarchy(hierarchy);
            }
            catch (Exception ex)
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
            //PerformanceVehicle performance = new PerformanceVehicle();

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

                    //performance = new PerformanceVehicle();
                    //performance.Device = device;
                    //performance.Fuel = fuel;
                    //performance.Obd = odo;
                    //performance.Name = VehicleName;
                    //listPerformance.Add(performance);

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
                    //performance = new PerformanceVehicle();
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

                    //performance = new PerformanceVehicle();
                    //performance.Device = device;
                    //performance.Fuel = 0;
                    //performance.Obd = 0;
                    //performance.Name = VehicleName;
                    //listPerformance.Add(performance);

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
                                if (listKey[rows - 1].Event.Equals(startS))
                                {
                                    count = list.Count;
                                    list[count - 1].EndDate = listKey[rows - 1].EndDate;

                                    milageFin = listKey[rows - 1].totalM;
                                    list[count - 1].ODO = Convert.ToString(metrosKilometros(milageFin - milageIni));

                                    fuelFin = Convert.ToDouble(listKey[rows - 1].totalF);
                                    list[count - 1].Fuel = Convert.ToString(litros(fuelFin - fuelIni));

                                    listItineraries.Add(list);
                                    list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();

                                    itineraries.Device = data.Device;
                                    itineraries.StartDate = data.StartDate;
                                    itineraries.EndDate = data.EndDate;

                                    itineraries.ODO = string.Empty;
                                    itineraries.Fuel = string.Empty;
                                    milageIni = data.totalM;
                                    fuelIni = data.totalF;

                                    itineraries.DriverData = DriverData;
                                    itineraries.Image = Image;
                                    itineraries.VehicleName = VehicleName;

                                    itineraries.Score = string.Empty;

                                    list.Add(itineraries);

                                }
                                else if (listKey[rows - 1].Event.Equals(endS))
                                {
                                    count = list.Count;
                                    list[count - 1].EndDate = listKey[rows - 1].EndDate;
                                    milageFin = listKey[rows - 1].totalM;
                                    list[count - 1].ODO = Convert.ToString(metrosKilometros(milageFin - milageIni));

                                    fuelFin = Convert.ToDouble(listKey[rows - 1].totalF);
                                    list[count - 1].Fuel = Convert.ToString(litros(fuelFin - fuelIni));

                                    listItineraries.Add(list);
                                    list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();

                                    itineraries.Device = data.Device;
                                    itineraries.StartDate = data.StartDate;
                                    itineraries.EndDate = data.EndDate;

                                    itineraries.DriverData = DriverData;
                                    itineraries.Image = Image;
                                    itineraries.VehicleName = VehicleName;


                                    itineraries.ODO = string.Empty;
                                    itineraries.Fuel = string.Empty;
                                    milageIni = data.totalM;
                                    fuelIni = data.totalF;

                                    itineraries.Score = string.Empty;

                                    list.Add(itineraries);
                                }
                            }
                            else
                            {
                                if (listKey[rows + 1].Event.Equals(startS))
                                {
                                    if (list.Count > 0)
                                    {
                                        if (listKey[rows - 1].Event.Equals(startS))
                                        {
                                            count = list.Count;
                                            list[count - 1].EndDate = listKey[rows - 1].EndDate;
                                            milageFin = listKey[rows - 1].totalM;
                                            list[count - 1].ODO = Convert.ToString(metrosKilometros(milageFin - milageIni));

                                            fuelFin = Convert.ToDouble(listKey[rows - 1].totalF);
                                            list[count - 1].Fuel = Convert.ToString(litros(fuelFin - fuelIni));

                                            listItineraries.Add(list);
                                            list = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();

                                            itineraries.Device = data.Device;
                                            itineraries.StartDate = data.StartDate;
                                            itineraries.EndDate = data.EndDate;

                                            itineraries.DriverData = DriverData;
                                            itineraries.Image = Image;
                                            itineraries.VehicleName = VehicleName;

                                            itineraries.ODO = string.Empty;
                                            itineraries.Fuel = string.Empty;
                                            milageIni = data.totalM;
                                            fuelIni = data.totalF;
                                            itineraries.Score = string.Empty;

                                            list.Add(itineraries);
                                        }
                                    }
                                    else
                                    {
                                        itineraries.Device = data.Device;
                                        itineraries.StartDate = data.StartDate;
                                        itineraries.EndDate = data.EndDate;

                                        itineraries.DriverData = DriverData;
                                        itineraries.Image = Image;
                                        itineraries.VehicleName = VehicleName;

                                        itineraries.ODO = string.Empty;
                                        itineraries.Fuel = string.Empty;
                                        milageIni = data.totalM;
                                        fuelIni = data.totalF;

                                        itineraries.Score = string.Empty;

                                        list.Add(itineraries);
                                        fecha = data.StartDate.ToString("dd-MM-yyyy");
                                        bandera = true;
                                    }
                                }
                                else if (listKey[rows + 1].Event.Equals(endS))
                                {
                                    itineraries.Device = data.Device;
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
                                }
                            }
                        }
                        rows++;
                    }

                    count = list.Count;
                    list[count - 1].EndDate = listKey[rows - 1].EndDate;


                    milageFin = listKey[rows - 1].totalM;
                    list[count - 1].ODO = Convert.ToString(metrosKilometros(milageFin - milageIni));

                    fuelFin = Convert.ToDouble(listKey[rows - 1].totalF);
                    list[count - 1].Fuel = Convert.ToString(litros(fuelFin - fuelIni));

                    listItineraries.Add(list);
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
                string start = startdate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string end = enddate.ToString("yyyy-MM-ddTHH:mm:ssZ");

                BsonDocument bsonDocument = new BsonDocument();
                bsonDocument.Add("device", device);
                bsonDocument.Add("date", new BsonDocument("$gte", Convert.ToDateTime(start)).Add("$lte", Convert.ToDateTime(end)));

                var build = bsonDocument;

                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<GPS>("GPS");
                var result = stored.Find(build).Sort("{date:1}").ToList();

                if (result.Count > 0)
                {
                    foreach (GPS data in result)
                    {
                        ItinerariesKey keys = new ItinerariesKey();
                        if (data.Diff <= diff)
                        {
                            keys.Event = startS;
                        }
                        else
                        {
                            keys.Event = endS;
                        }

                        keys.Device = data.Device;
                        keys.StartDate = data.Date;
                        keys.EndDate = data.Date;
                        keys.Diff = data.Diff;
                        keys.ODO = string.Empty;
                        keys.Fuel = string.Empty;
                        keys.VelocidadMaxima = string.Empty;
                        keys.NoAlarmas = string.Empty;
                        keys.Longitude = data.Location.Coordinates[0].ToString();
                        keys.Latitude = data.Location.Coordinates[1].ToString();
                        keys.totalM = data.TotalMilage + data.CurrentMilage;
                        keys.totalF = data.TotalFuel + data.CurrentFuel;

                        listKey.Add(keys);
                    }
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