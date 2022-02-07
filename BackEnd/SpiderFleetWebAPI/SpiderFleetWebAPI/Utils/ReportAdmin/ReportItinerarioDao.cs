using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Itineraries;
using CredencialSpiderFleet.Models.ReportAdmin;
using CredencialSpiderFleet.Models.Useful;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.Alarms;
using SpiderFleetWebAPI.Models.Mongo.GPS;
using SpiderFleetWebAPI.Models.Response.Itineraries;
using SpiderFleetWebAPI.Models.Response.ReportAdmin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SpiderFleetWebAPI.Utils.ReportAdmin
{
    public class ReportItinerarioDao
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private UseFul use = new UseFul();

        public ReportItinerarioDao()
        {

        }

        public ReportItinerarioSpecialResponse Read(string device, DateTime fechaInicio, DateTime fehaFin)
        {
            ReportItinerarioSpecialResponse response = new ReportItinerarioSpecialResponse();
            List<ReportItinerarioSpecial> listItinerario = new List<ReportItinerarioSpecial>();
            ReportItinerarioSpecial itinerario = new ReportItinerarioSpecial();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_report_vehicle_other", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    cmd.Parameters.Add(new SqlParameter("@fechainicio", fechaInicio));
                    cmd.Parameters.Add(new SqlParameter("@fechafin", fehaFin));
                    cmd.CommandTimeout = 400;


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itinerario = new ReportItinerarioSpecial();

                            itinerario.Viaje = Convert.ToInt32(reader["ID"].ToString());

                            itinerario.Fecha = Convert.ToString(reader["Fecha"]);
                            itinerario.Inicio = Convert.ToString(reader["HoraIni"]);
                            itinerario.Fin = Convert.ToString(reader["HoraFin"]);
                            itinerario.Tiempo = Convert.ToString(reader["Tiempo"]);
                            itinerario.Latitude = Convert.ToString(reader["Latitud"]);
                            itinerario.Longitude = Convert.ToString(reader["Longitud"]);
                            itinerario.Velocidad = Convert.ToDecimal(reader["Velocidad"]);

                            int totalMF = (reader["Milage"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["Milage"]));

                            itinerario.Distancia = totalMF;

                            listItinerario.Add(itinerario);
                        }
                        reader.Close();
                        response.success = true;
                        //Sort(ref listItinerario, "", "");
                        response.itinerarios = listItinerario;
                    }
                }
                else
                {
                    response.success = false;
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

        private void Sort(ref List<ReportItinerarioSpecial> list, string sortBy, string sortDirection)
        {
            //if (sortBy == "ID")
            //{
            list = list.OrderBy(x => x.Viaje).ToList<ReportItinerarioSpecial>();
            //}
        }


        public ReportItinerarioResponse Read(string device, DateTime fechaInicio, DateTime fehaFin, decimal maxVelocidad)
        {
            ReportItinerarioResponse response = new ReportItinerarioResponse();
            List<ReportItinerario> listItinerario = new List<ReportItinerario>();
            ReportItinerario itinerario = new ReportItinerario();
            
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_report_vehicle", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    cmd.Parameters.Add(new SqlParameter("@fechainicio", fechaInicio.ToString("yyyy-MM-dd HH:mm:ss")));
                    cmd.Parameters.Add(new SqlParameter("@fechafin", fehaFin.ToString("yyyy-MM-dd HH:mm:ss")));
                    cmd.Parameters.Add(new SqlParameter("@max_velocidad", Convert.ToDecimal(maxVelocidad)));
                    cmd.CommandTimeout = 500;


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itinerario = new ReportItinerario();                            

                            itinerario.Viaje = Convert.ToInt32(reader["ID"].ToString());

                            itinerario.Fecha = Convert.ToString(reader["Fecha"]);
                            itinerario.Inicio = Convert.ToString(reader["HoraIni"]);
                            itinerario.Fin = Convert.ToString(reader["HoraFin"]);
                            itinerario.Tiempo = Convert.ToInt32(reader["Tiempo"]);
                            itinerario.Aceleracion = Convert.ToInt32(reader["Aceleracion"]);
                            itinerario.Frenado = Convert.ToInt32(reader["Frenado"]);
                            itinerario.RPM = Convert.ToInt32(reader["RPM"]);
                            itinerario.Velocidad = Convert.ToInt32(reader["Velocidad"]);
                            int totalMF = (reader["totalMF"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["totalMF"]));
                            int totalMI = (reader["totalMI"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["totalMI"]));
                            itinerario.Distancia = totalMF - totalMI;// (Convert.ToInt32(reader["totalMF"]) - Convert.ToInt32(reader["totalMI"])); 
                            itinerario.Consumo = 0;//Convert.ToInt32(reader["motor"]);
                            itinerario.FechaInicio = Convert.ToDateTime(reader["FechaInicio"]);
                            itinerario.FechaFin = Convert.ToDateTime(reader["FechaFin"]);

                            itinerario.Latitud = Convert.ToString(reader["Latitud"]);
                            itinerario.Longitud = Convert.ToString(reader["Longitud"]);
                            itinerario.Responsable = Convert.ToString(reader["Responsible"]);

                            listItinerario.Add(itinerario);
                        }
                        reader.Close();
                        response.success = true;
                        Sort(ref listItinerario, "", "");
                        response.itinerarios = listItinerario;
                    }
                }
                else
                {
                    response.success = false;
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

        public ReportItinerarioResponse ReadNewLogical(string device, DateTime fechaInicio, DateTime fehaFin, decimal maxVelocidad)
        {
            ReportItinerarioResponse response = new ReportItinerarioResponse();
            List<ReportItinerario> listItinerario = new List<ReportItinerario>();
            ReportItinerario itinerario = new ReportItinerario();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_report_vehicle_new_logical", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));
                    cmd.Parameters.Add(new SqlParameter("@fecha_inicio", fechaInicio.ToString("yyyy-MM-dd HH:mm:ss")));
                    cmd.Parameters.Add(new SqlParameter("@fecha_fin", fehaFin.ToString("yyyy-MM-dd HH:mm:ss")));
                    //cmd.Parameters.Add(new SqlParameter("@max_velocidad", Convert.ToDecimal(maxVelocidad)));
                    cmd.CommandTimeout = 500;


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itinerario = new ReportItinerario();

                            itinerario.Viaje = Convert.ToInt32(reader["ID"].ToString());

                            itinerario.Fecha = Convert.ToString(reader["Fecha"]);
                            itinerario.Inicio = Convert.ToString(reader["HoraIni"]);
                            itinerario.Fin = Convert.ToString(reader["HoraFin"]);
                            itinerario.Tiempo = Convert.ToInt32(reader["Tiempo"]);
                            itinerario.Aceleracion = Convert.ToInt32(reader["Aceleracion"]);
                            itinerario.Frenado = Convert.ToInt32(reader["Frenado"]);
                            itinerario.RPM = Convert.ToInt32(reader["RPM"]);
                            itinerario.Velocidad = Convert.ToInt32(reader["Velocidad"]);

                            itinerario.Dongle = Convert.ToInt32(reader["Dongle"]);

                            if(itinerario.Dongle == 1)
                            {
                                int totalMF = (reader["totalMF"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["totalMF"]));
                                int totalMI = (reader["totalMI"] == DBNull.Value) ? 0 : (Convert.ToInt32(reader["totalMI"]));
                                itinerario.Distancia = totalMF - totalMI;// (Convert.ToInt32(reader["totalMF"]) - Convert.ToInt32(reader["totalMI"])); 

                                double totalFI = (reader["totalFI"] == DBNull.Value) ? 0 : (Convert.ToDouble(reader["totalFI"]));
                                double totalFF = (reader["totalFF"] == DBNull.Value) ? 0 : (Convert.ToDouble(reader["totalFF"]));
                                //itinerario.Gas = Convert.ToDecimal(totalFF - totalFI);//Convert.ToInt32(reader["motor"]);

                                itinerario.Consumo = totalFF - totalFI;
                            }
                            else
                            {
                                itinerario.Km = (Convert.ToDecimal(reader["totalMF"].ToString())); // - totalMI;// (Convert.ToInt32(reader["totalMF"]) - Convert.ToInt32(reader["totalMI"])); 
                                itinerario.Gas = (Convert.ToDecimal(reader["totalFI"])); //Convert.ToInt32(reader["motor"]);
                            }                          
                            
                            
                            itinerario.FechaInicio = Convert.ToDateTime(reader["FechaInicio"]);
                            itinerario.FechaFin = Convert.ToDateTime(reader["FechaFin"]);

                            itinerario.Latitud = Convert.ToString(reader["Latitud"]);
                            itinerario.Longitud = Convert.ToString(reader["Longitud"]);
                            itinerario.Responsable = Convert.ToString(reader["Responsible"]);
                            

                            listItinerario.Add(itinerario);
                        }
                        reader.Close();
                        response.success = true;
                        Sort(ref listItinerario, "", "");
                        response.itinerarios = listItinerario;
                    }
                }
                else
                {
                    response.success = false;
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


        private void Sort(ref List<ReportItinerario> list, string sortBy, string sortDirection)
        {
            //if (sortBy == "ID")
            //{
                list = list.OrderBy(x => x.Viaje).ToList<ReportItinerario>();
            //}
        }

    }
}