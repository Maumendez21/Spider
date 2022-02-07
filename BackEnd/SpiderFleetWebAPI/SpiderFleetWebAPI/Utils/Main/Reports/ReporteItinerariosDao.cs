using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Main.Filtros;
using CredencialSpiderFleet.Models.Main.Itineracios;
using CredencialSpiderFleet.Models.Main.Reports;
using SpiderFleetWebAPI.Models.Response.Main.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Main.Reports
{
    public class ReporteItinerariosDao
    {
        public ReporteItinerariosDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
                
        public ReportItinerariesResponse Read(FilterItineraries filtro)
        {
            ReportItinerariesResponse response = new ReportItinerariesResponse();

            ReportItineraries itinerarios = new ReportItineraries();
            List<ReportItineraries> listReporte = new List<ReportItineraries>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("sfl.itinerario", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Vehiculo", Convert.ToString(filtro.Device)));
                    cmd.Parameters.Add(new SqlParameter("@Inicio", Convert.ToDateTime(filtro.StartDate)));
                    cmd.Parameters.Add(new SqlParameter("@Fin", Convert.ToDateTime(filtro.EndDate)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itinerarios = new ReportItineraries();
                            itinerarios.Fecha = Convert.ToDateTime(reader["Fecha"]);
                            itinerarios.ODO = Convert.ToInt32(reader["ODO"]);
                            itinerarios.Valor = Convert.ToDecimal(reader["Valor"]);
                            itinerarios.DIFF = (reader["DIFF"] == DBNull.Value) ? 0 : Convert.ToInt32(reader["DIFF"]);
                            itinerarios.Company = Convert.ToInt32(reader["Company"]);
                            itinerarios.Latitud = Convert.ToString(reader["Latitud"]);
                            itinerarios.Longitud = Convert.ToString(reader["Longitud"]);
                            listReporte.Add(itinerarios);

                        }
                    }
                }

                response.listHeader = headerTrip(listReporte, filtro.Device, Convert.ToDateTime(filtro.StartDate), Convert.ToDateTime(filtro.EndDate));
                response.listTrip = calculoReporte(listReporte, filtro.Device, Convert.ToDateTime(filtro.StartDate), Convert.ToDateTime(filtro.EndDate));
                response.success = true;

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

        private List<ReportHeaderTrip> headerTrip(List<ReportItineraries> listReporte, string device, DateTime fechaInicio, DateTime fechaFin)
        {

            List<ReportHeaderTrip> listHeader = new List<ReportHeaderTrip>();
            try
            {
                if (listReporte.Count > 0)
                {
                    int num1 = listReporte.Count;
                    int num2 = 0;
                    TimeSpan timeSpan1 = new TimeSpan();
                    for (int index = 0; index < num1 - 1; ++index)
                    {
                        TimeSpan timeSpan2 = listReporte[index + 1].Fecha.Value - listReporte[index].Fecha.Value;
                        num2 += (int)timeSpan2.TotalMinutes - listReporte[index].DIFF.Value;
                    }
                    ReportHeaderTrip headerTrip1 = new ReportHeaderTrip();
                    headerTrip1.Viajes = num1 - 1;
                    if (num2 > 59)
                    {
                        headerTrip1.tiempo = (num2 / 60).ToString() + " H " + (object)(num2 % 60) + " min";
                    }
                    else
                    {
                        headerTrip1.tiempo = num2.ToString() + " min";
                    }
                    int? company = listReporte[0].Company;
                    int num3 = 1;
                    if ((company.GetValueOrDefault() == num3 ? (company.HasValue ? 1 : 0) : 0) != 0)
                    {
                        List<ItinerariosAlarmas> listAlarms = ReadAlarmas(device, fechaInicio, fechaFin);
                        headerTrip1.Vel = listAlarms[0].Velocidad;
                        headerTrip1.Rpm = listAlarms[0].RPM;
                        headerTrip1.Ace = listAlarms[0].Aceleracion;
                        headerTrip1.Des = listAlarms[0].Desaceleracion;

                        Decimal? valor1 = listReporte[num1 - 1].Valor;
                        Decimal? valor2 = listReporte[0].Valor;
                        Decimal num4 = (valor1.HasValue & valor2.HasValue ? new Decimal?(valor1.GetValueOrDefault() - valor2.GetValueOrDefault()) : new Decimal?()).Value;
                        int? odo1 = listReporte[num1 - 1].ODO;
                        int? odo2 = listReporte[0].ODO;
                        int num5 = (odo1.HasValue & odo2.HasValue ? new int?(odo1.GetValueOrDefault() - odo2.GetValueOrDefault()) : new int?()).Value / 1000;
                        if (num4 == Decimal.Zero)
                        {
                            if (device == "213GL2015023045")
                            {
                                headerTrip1.Rendimiento = 5.86.ToString() + " Km/l";
                                headerTrip1.Litros = this.Truncate((float)num5 / 5.86f, 2).ToString() + " Litros";
                                headerTrip1.ODO = num5.ToString() + " Km";
                            }
                            else
                            {
                                headerTrip1.Rendimiento = 10.7.ToString() + " Km/l";
                                headerTrip1.Litros = this.Truncate((float)num5 / 10.7f, 2).ToString() + " Litros";
                                headerTrip1.ODO = num5.ToString() + " Km";
                            }
                        }
                        else
                        {
                            headerTrip1.Rendimiento = this.Truncate((float)((Decimal)num5 / num4), 2).ToString() + " Km/l";
                            headerTrip1.Litros = num4.ToString() + " Litros";
                            headerTrip1.ODO = num5.ToString() + " Km";
                        }
                    }
                    else
                    {
                        List<ItinerariosAlarmas> listAlarms = ReadAlarmasTablas(device, fechaInicio, fechaFin);
                        headerTrip1.Vel = listAlarms[0].Velocidad;
                        headerTrip1.Rpm = listAlarms[0].RPM;
                        headerTrip1.Ace = listAlarms[0].Aceleracion;
                        headerTrip1.Des = listAlarms[0].Desaceleracion;

                        int num4 = listReporte[num1 - 1].ODO.HasValue ? listReporte[num1 - 1].ODO.Value : 0;
                        ReportHeaderTrip headerTrip2 = headerTrip1;
                        int num5 = num4;
                        int? nullable1 = listReporte[0].ODO;
                        double? nullable2 = nullable1.HasValue ? new double?((double)(num5 - nullable1.GetValueOrDefault()) * 1.609) : new double?();
                        string str1 = ((int)nullable2.Value).ToString() + " Km";
                        headerTrip2.ODO = str1;
                        int num6 = num4;
                        int? odo = listReporte[0].ODO;
                        nullable1 = odo.HasValue ? new int?(num6 - odo.GetValueOrDefault()) : new int?();
                        int num7 = 0;
                        if ((nullable1.GetValueOrDefault() == num7 ? (nullable1.HasValue ? 1 : 0) : 0) != 0)
                        {
                            headerTrip1.Litros = 0.ToString() + " Litros";
                            headerTrip1.Rendimiento = 0.ToString() + " Km/l";
                        }
                        else
                        {
                            nullable2 = ReadXirgoRendimiento(device, fechaInicio, fechaFin);
                            float num8 = this.Truncate((float)nullable2.Value * 0.425144f, 2);
                            ReportHeaderTrip headerTrip3 = headerTrip1;
                            nullable1 = listReporte[num1 - 1].ODO;
                            odo = listReporte[0].ODO;
                            nullable2 = nullable1.HasValue & odo.HasValue ? new double?((double)(nullable1.GetValueOrDefault() - odo.GetValueOrDefault()) * 1.609) : new double?();
                            double num9 = (double)num8;
                            string str2 = Math.Round((Decimal)(nullable2.HasValue ? new double?(nullable2.GetValueOrDefault() / num9) : new double?()).Value, 2).ToString() + " Litros";
                            headerTrip3.Litros = str2;
                            headerTrip1.Rendimiento = num8.ToString() + " Km/l";
                            if ((double)num8 == 0.0)
                            {
                                headerTrip1.Rendimiento = !(device == "213GL2015023045") ? 10.7f.ToString() + " Km/l" : 5.86f.ToString() + " Km/l";
                            }
                        }
                }
                listHeader.Add(headerTrip1);
            }
                else
                {
                    listHeader.Add(new ReportHeaderTrip()
                    {
                        Ace = 0,
                        Des = 0,
                        Litros = "0 Litros",
                        ODO = "0 Km",
                        Rendimiento = "0 Km/l",
                        Rpm = 0,
                        tiempo = "Sin actividad",
                        Vel = 0,
                        Viajes = 0
                    });
                }
            }
            catch (Exception ex)
            {
                listHeader.Add(new ReportHeaderTrip()
                {

                    Ace = 0,
                    Des = 0,
                    Litros = "0 Litros",
                    ODO = "0 Km",
                    Rendimiento = "0 Km/l",
                    Rpm = 0,
                    tiempo = "Sin actividad",
                    Vel = 0,
                    Viajes = 0
                });
            }
              return listHeader;
        }

        public float Truncate(float value, int digits)
        {
            double num = Math.Pow(10.0, (double)digits);
            return (float)(Math.Truncate(num * (double)value) / num);
        }

        private List<ReportTrip> calculoReporte(List<ReportItineraries> datos, string device, DateTime startDate, DateTime endDate)
        {
            List<ReportTrip> listTrip = new List<ReportTrip>();
            string nombre_operador = ReadNombreOperador(device);
            try
            {
                DateTime? fecha;
                DateTime dateTime3;
                Decimal? nullable1;
                int? nullable2;
                int? nullable3;
                int? nullable4;
                double? nullable5;

                if (datos.Count < 2)
                {
                    ReportTrip tripVacio = new ReportTrip();
                    listTrip.Add(tripVacio);
                }
                else if (datos.Count == 2)
                {
                    ReportTrip trip = new ReportTrip();
                    trip.Dispositivo = device;
                    trip.NombreOperador = nombre_operador;
                    trip.I = datos[0].Fecha.Value;
                    trip.F = datos[1].Fecha.Value;
                    trip.Inicio = datos[0].Fecha.ToString();
                    ReportTrip reportesTrip1 = trip;
                    fecha = datos[1].Fecha;
                    string str1 = fecha.ToString();
                    reportesTrip1.Fin = str1;
                    trip.FI = trip.I.ToString("dd/MMM/yyyy");
                    trip.HI = trip.I.ToString("hh:mm tt");
                    ReportTrip reportesTrip2 = trip;
                    dateTime3 = trip.F;
                    string str2 = dateTime3.ToString("hh:mm tt");
                    reportesTrip2.HF = str2;
                    fecha = datos[1].Fecha;
                    DateTime dateTime4 = fecha.Value;
                    fecha = datos[0].Fecha;
                    DateTime dateTime5 = fecha.Value;
                    TimeSpan timeSpan = dateTime4 - dateTime5;
                    trip.Transcurrido = timeSpan.Hours.ToString() + " H " + (object)timeSpan.Minutes + " min";
                    int? company = datos[0].Company;
                    int num2 = 1;
                    if ((company.GetValueOrDefault() == num2 ? (company.HasValue ? 1 : 0) : 0) != 0)
                    {
                        ReportTrip reportesTrip3 = trip;
                        Decimal? valor1 = datos[1].Valor;
                        Decimal? valor2 = datos[0].Valor;
                        nullable1 = valor1.HasValue & valor2.HasValue ? new Decimal?(valor1.GetValueOrDefault() - valor2.GetValueOrDefault()) : new Decimal?();
                        Decimal num3 = nullable1.Value;
                        reportesTrip3.Litros = num3;
                        ReportTrip reportesTrip4 = trip;
                        nullable2 = datos[1].ODO;
                        nullable3 = datos[0].ODO;
                        nullable4 = nullable2.HasValue & nullable3.HasValue ? new int?(nullable2.GetValueOrDefault() - nullable3.GetValueOrDefault()) : new int?();
                        int num4 = nullable4.Value;
                        reportesTrip4.ODO = num4;
                        List<ItinerariosAlarmas> listAlarms = ReadAlarmas(device, trip.I, trip.F);
                        trip.Vel = listAlarms[0].Velocidad;     
                        trip.Rpm = listAlarms[0].RPM;           
                        trip.Ace = listAlarms[0].Aceleracion;   
                        trip.Des = listAlarms[0].Desaceleracion;
                        trip.VELmax = "";
                        trip.RPMmax = "";
                        trip.Ralenti = "";
                        trip.Latitud = datos[1].Latitud;
                        trip.Longitud = datos[1].Longitud;
                    }
                    else
                    {
                        ReportTrip reportesTrip3 = trip;
                        int? odo1 = datos[1].ODO;
                        int? odo2 = datos[0].ODO;
                        nullable5 = odo1.HasValue & odo2.HasValue ? new double?((double)(odo1.GetValueOrDefault() - odo2.GetValueOrDefault()) * 1.609) : new double?();
                        int num3 = (int)nullable5.Value;
                        reportesTrip3.ODO = num3;
                        List<ItinerariosAlarmas> listAlarms = ReadAlarmasTablas(device, trip.I, trip.F);
                        trip.Vel = listAlarms[0].Velocidad;     
                        trip.Rpm = listAlarms[0].RPM;           
                        trip.Ace = listAlarms[0].Aceleracion;   
                        trip.Des = listAlarms[0].Desaceleracion;
                        trip.Litros = trip.ODO != 0 ? (!(device == "213GL2015023045") ? Math.Round((Decimal)((double)trip.ODO / 10.7), 2) : Math.Round((Decimal)((double)trip.ODO / 5.86), 2)) : Decimal.Zero;
                        Maximos list2 = ReadMaximos(device, trip.I, trip.F);
                        trip.VELmax = string.Concat((object)list2.Velocidad);
                        trip.RPMmax = string.Concat((object)list2.RPM);
                        trip.Ralenti = string.Concat((object)list2.Ralenti);
                        trip.Latitud = datos[1].Latitud;
                        trip.Longitud = datos[1].Longitud;
                        trip.id = 1;
                    }
                    listTrip.Add(trip);
                }
                else if (datos.Count > 2)
                {
                    for (int index = 0; index < datos.Count; ++index)
                    {
                        ReportTrip trip = new ReportTrip();
                        trip.Dispositivo = device;
                        trip.NombreOperador = nombre_operador;
                        ReportTrip reportesTrip1 = trip;
                        fecha = datos[index].Fecha;
                        DateTime dateTime4 = fecha.Value;
                        reportesTrip1.I = dateTime4;
                        ReportTrip reportesTrip2 = trip;
                        fecha = datos[index + 1].Fecha;
                        DateTime dateTime5 = fecha.Value;
                        reportesTrip2.F = dateTime5;
                        ReportTrip reportesTrip3 = trip;
                        fecha = datos[index + 1].Fecha;
                        string str1 = fecha.ToString();
                        reportesTrip3.Fin = str1;
                        TimeSpan timeSpan;
                        if (index == 0)
                        {
                            fecha = datos[index + 1].Fecha;
                            DateTime dateTime6 = fecha.Value;
                            fecha = datos[index].Fecha;
                            DateTime dateTime7 = fecha.Value;
                            timeSpan = dateTime6 - dateTime7;
                            ReportTrip reportesTrip4 = trip;
                            dateTime3 = trip.I;
                            string str2 = dateTime3.ToString("dd/MMM/yyyy");
                            reportesTrip4.FI = str2;
                            ReportTrip reportesTrip5 = trip;
                            dateTime3 = trip.I;
                            string str3 = dateTime3.ToString("hh:mm tt");
                            reportesTrip5.HI = str3;
                            ReportTrip reportesTrip6 = trip;
                            dateTime3 = trip.F;
                            string str4 = dateTime3.ToString("hh:mm tt");
                            reportesTrip6.HF = str4;
                            ReportTrip reportesTrip7 = trip;
                            fecha = datos[index].Fecha;
                            string str5 = fecha.ToString();
                            reportesTrip7.Inicio = str5;
                        }
                        else
                        {
                            fecha = datos[index + 1].Fecha;
                            DateTime dateTime6 = fecha.Value;
                            fecha = datos[index].Fecha;
                            dateTime3 = fecha.Value;
                            ref DateTime local1 = ref dateTime3;
                            nullable3 = datos[index].DIFF;
                            double num2 = (double)nullable3.Value;
                            DateTime dateTime7 = local1.AddMinutes(num2);
                            timeSpan = dateTime6 - dateTime7;
                            ReportTrip reportesTrip4 = trip;
                            dateTime3 = trip.I;
                            ref DateTime local2 = ref dateTime3;
                            nullable3 = datos[index].DIFF;
                            double num3 = (double)nullable3.Value;
                            dateTime3 = local2.AddMinutes(num3);
                            string str2 = dateTime3.ToString("dd/MMM/yyyy hh:mm:ss tt");
                            reportesTrip4.Inicio = str2;
                            ReportTrip reportesTrip5 = trip;
                            dateTime3 = trip.I;
                            ref DateTime local3 = ref dateTime3;
                            nullable3 = datos[index].DIFF;
                            double num4 = (double)nullable3.Value;
                            dateTime3 = local3.AddMinutes(num4);
                            string str3 = dateTime3.ToString("dd/MMM/yyyy");
                            reportesTrip5.FI = str3;
                            ReportTrip reportesTrip6 = trip;
                            dateTime3 = trip.I;
                            ref DateTime local4 = ref dateTime3;
                            nullable3 = datos[index].DIFF;
                            double num5 = (double)nullable3.Value;
                            dateTime3 = local4.AddMinutes(num5);
                            string str4 = dateTime3.ToString("hh:mm tt");
                            reportesTrip6.HI = str4;
                            ReportTrip reportesTrip7 = trip;
                            dateTime3 = trip.F;
                            string str5 = dateTime3.ToString("hh:mm tt");
                            reportesTrip7.HF = str5;
                        }
                        if ((int)timeSpan.TotalMinutes > 59)
                            trip.Transcurrido = timeSpan.Hours.ToString() + " H " + (object)timeSpan.Minutes + " min";
                        else
                            trip.Transcurrido = timeSpan.Minutes.ToString() + " min";
                        nullable3 = datos[0].Company;
                        int num6 = 1;
                        if ((nullable3.GetValueOrDefault() == num6 ? (nullable3.HasValue ? 1 : 0) : 0) != 0)
                        {
                            ReportTrip reportesTrip4 = trip;
                            nullable1 = datos[index + 1].Valor;
                            Decimal? valor = datos[index].Valor;
                            Decimal? nullable6;
                            Decimal? nullable7;
                            if (!(nullable1.HasValue & valor.HasValue))
                            {
                                nullable6 = new Decimal?();
                                nullable7 = nullable6;
                            }
                            else
                                nullable7 = new Decimal?(nullable1.GetValueOrDefault() - valor.GetValueOrDefault());
                            nullable6 = nullable7;
                            Decimal num2 = nullable6.Value;
                            reportesTrip4.Litros = num2;
                            ReportTrip reportesTrip5 = trip;
                            nullable3 = datos[index + 1].ODO;
                            nullable4 = datos[index].ODO;
                            int? nullable8;
                            if (!(nullable3.HasValue & nullable4.HasValue))
                            {
                                nullable2 = new int?();
                                nullable8 = nullable2;
                            }
                            else
                                nullable8 = new int?(nullable3.GetValueOrDefault() - nullable4.GetValueOrDefault());
                            nullable2 = nullable8;
                            int num3 = nullable2.Value / 1000;
                            reportesTrip5.ODO = num3;
                            List<ItinerariosAlarmas> listAlarms = ReadAlarmas(device, trip.I, trip.F);
                            trip.Vel = listAlarms[0].Velocidad; 
                            trip.Rpm = listAlarms[0].RPM; //0; 
                            trip.Ace = listAlarms[0].Aceleracion; 
                            trip.Des = listAlarms[0].Desaceleracion; 
                            trip.VELmax = "";
                            trip.RPMmax = "";
                            trip.Ralenti = "";
                            trip.Latitud = datos[index + 1].Latitud;
                            trip.Longitud = datos[index + 1].Longitud;
                        }
                        else
                        {
                            ReportTrip reportesTrip4 = trip;
                            nullable2 = datos[index + 1].ODO;
                            nullable4 = datos[index].ODO;
                            double? nullable6;
                            if (!(nullable2.HasValue & nullable4.HasValue))
                            {
                                nullable5 = new double?();
                                nullable6 = nullable5;
                            }
                            else
                                nullable6 = new double?((double)(nullable2.GetValueOrDefault() - nullable4.GetValueOrDefault()) * 1.609);
                            nullable5 = nullable6;
                            int num2 = (int)nullable5.Value;
                            reportesTrip4.ODO = num2;
                            List<ItinerariosAlarmas> listAlarms = ReadAlarmasTablas(device, trip.I, trip.F);
                            trip.Vel = listAlarms[0].Velocidad; 
                            trip.Rpm = listAlarms[0].RPM; 
                            trip.Ace = listAlarms[0].Aceleracion; 
                            trip.Des = listAlarms[0].Desaceleracion; 
                            trip.Litros = trip.ODO != 0 ? (!(device == "213GL2015023045") ? Math.Round((Decimal)((double)trip.ODO / 10.7), 2) : Math.Round((Decimal)((double)trip.ODO / 5.86), 2)) : Decimal.Zero;
                            Maximos list2 = ReadMaximos(device, trip.I, trip.F);
                            trip.VELmax = string.Concat((object)list2.Velocidad);
                            trip.RPMmax = string.Concat((object)list2.RPM);
                            trip.Ralenti = string.Concat((object)list2.Ralenti);
                            trip.Latitud = datos[index + 1].Latitud;
                            trip.Longitud = datos[index + 1].Longitud;
                            trip.id = index + 1;
                        }
                        trip.I = Convert.ToDateTime(trip.Inicio);
                        listTrip.Add(trip);
                    }
                }

                return listTrip;
            }
            catch(Exception ex)
            {
                ReportTrip tripVacio = new ReportTrip();
                listTrip.Add(tripVacio);
                return listTrip;
            }
        }

        private Maximos ReadMaximos(string device, DateTime inicio, DateTime fin)
        {
            Maximos maximos = new Maximos();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("sfl.maximos", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Vehiculo", Convert.ToString(device)));
                    cmd.Parameters.Add(new SqlParameter("@Inicio", inicio ));
                    cmd.Parameters.Add(new SqlParameter("@Fin", fin));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            maximos = new Maximos();
                            maximos.Velocidad = Convert.ToInt32(reader["Velocidad"]);
                            maximos.RPM = Convert.ToInt32(reader["RPM"]);
                            maximos.Ralenti = Convert.ToInt32(reader["Ralenti"]);
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
            return maximos;
        }

        private List<ItinerariosAlarmas> ReadAlarmas(string device, DateTime inicio, DateTime fin)
        {
            List<ItinerariosAlarmas> listAlarms = new List<ItinerariosAlarmas>();
            ItinerariosAlarmas alarmas = new ItinerariosAlarmas();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.reporte_itinerarios_alarmas", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    cmd.Parameters.Add(new SqlParameter("@fecha_inicio", inicio));
                    cmd.Parameters.Add(new SqlParameter("@fecha_fin", fin));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            alarmas = new ItinerariosAlarmas();
                            alarmas.Velocidad = Convert.ToInt32(reader["Velocidad"]);
                            alarmas.RPM = Convert.ToInt32(reader["RPM"]);
                            alarmas.Aceleracion = Convert.ToInt32(reader["Aceleracion"]);
                            alarmas.Desaceleracion = Convert.ToInt32(reader["Desaceleracion"]);
                            listAlarms.Add(alarmas);
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
            return listAlarms;
        }

        private List<ItinerariosAlarmas> ReadAlarmasTablas(string device, DateTime inicio, DateTime fin)
        {
            List<ItinerariosAlarmas> listAlarms = new List<ItinerariosAlarmas>();
            ItinerariosAlarmas alarmas = new ItinerariosAlarmas();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.reporte_itinerarios_alarmas_tablas", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    cmd.Parameters.Add(new SqlParameter("@fecha_inicio", inicio));
                    cmd.Parameters.Add(new SqlParameter("@fecha_fin", fin));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            alarmas = new ItinerariosAlarmas();
                            alarmas.Velocidad = Convert.ToInt32(reader["Velocidad"]);
                            alarmas.RPM = Convert.ToInt32(reader["RPM"]);
                            alarmas.Aceleracion = Convert.ToInt32(reader["Aceleracion"]);
                            alarmas.Desaceleracion = Convert.ToInt32(reader["Desaceleracion"]);
                            listAlarms.Add(alarmas);
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
            return listAlarms;
        }

        private string ReadNombreOperador(string device)
        {
            string nombreOperador = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.nombre_operador", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nombreOperador = Convert.ToString(reader["nombre_operador"]);
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
            return nombreOperador;
        }

        private float ReadXirgoRendimiento(string device, DateTime inicio, DateTime fin)
        {
            float rendimiento = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("sfl.XirgoRendimiento", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Vehiculo", Convert.ToString(device)));
                    cmd.Parameters.Add(new SqlParameter("@FI", Convert.ToString(device)));
                    cmd.Parameters.Add(new SqlParameter("@FF", Convert.ToString(device)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rendimiento = float.Parse(Convert.ToString(reader["Rendimiento"]));
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
            return rendimiento;
        }

    }
}