using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Diary;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.Diary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Diary
{
    public class DiaryDao
    {
        /// <summary>
        /// 
        /// </summary>
        public DiaryDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private UseFul useful = new UseFul();
        private const string SEMANAL = "Semanal";

        private DateTime ObtenerUltimoDiaSemanaDelMes(int anio, int mes, DayOfWeek dia)
        {
            DateTime ultimoDiaMes = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));
            int diferencia = ultimoDiaMes.DayOfWeek - dia;
            return diferencia > 0 ? ultimoDiaMes.AddDays(-1 * diferencia) : ultimoDiaMes.AddDays(-1 * (7 + diferencia));
        }

        private DateTime sxx(int anio, int mes, DayOfWeek dia, int time)
        {
            DateTime lastDayMouth = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));

            //es un loop pequeño sin problemas de performance
            while (lastDayMouth.DayOfWeek != dia)
            {
                time++;
                lastDayMouth = lastDayMouth.AddDays(-1);
            }

            return lastDayMouth;
        }

        public DiaryResponse Create(CredencialSpiderFleet.Models.Diary.Diary events)
        {
            DiaryResponse response = new DiaryResponse();
            int respuesta = 0;

            int time = 1;

            if (events.Frecuency.Equals(SEMANAL))
            {                
                DateTime lastDay = ObtenerUltimoDiaSemanaDelMes(events.StartDate.Year, events.StartDate.Month, events.StartDate.DayOfWeek);

                while (true)
                {
                    int compare = UseFul.compare(lastDay, events.StartDate);
                    
                    if(compare <= 0)
                    {
                        break;
                    }
                    else
                    {
                        time++;
                        lastDay = lastDay.AddDays(-7);
                    }
                }
            }

            try
            {

                for (int i = 0; i < time; i++)
                {
                    if (sql.IsConnection)
                    {
                        cn = sql.Connection();
                        SqlCommand cmd = new SqlCommand("ad.sp_create_diary", cn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@node", events.Node));
                        cmd.Parameters.Add(new SqlParameter("@startdate", events.StartDate));
                        cmd.Parameters.Add(new SqlParameter("@enddate", events.EndDate));
                        cmd.Parameters.Add(new SqlParameter("@notes", Convert.ToString(events.Notes)));
                        cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(events.Device)));
                        cmd.Parameters.Add(new SqlParameter("@responsable", Convert.ToInt32(events.Responsable)));

                        SqlParameter sqlParameter = new SqlParameter();
                        sqlParameter.ParameterName = "@cMensaje";
                        sqlParameter.SqlDbType = SqlDbType.Int;
                        sqlParameter.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(sqlParameter);
                        cmd.ExecuteNonQuery();

                        respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                        if (respuesta == 3)
                        {
                            response.success = false;
                            response.messages.Add("Error al intenar dar de alta el Evento");
                        }
                        if (respuesta == 2)
                        {
                            response.success = false;
                            response.messages.Add("El Evento que tratas de dar de alta ya existe con las siguientes fechas " 
                                + events.StartDate.ToString("dd-MM-yyyy HH:mm") + " y " + events.EndDate.ToString("dd-MM-yyyy HH:mm") +
                                ", verifique por favor");
                        }
                        else if (respuesta == 1)
                        {
                            response.success = true;
                        }

                        if(time != 1)
                        {
                            events.StartDate = events.StartDate.AddDays(7);
                            events.EndDate = events.EndDate.AddDays(7);
                        }
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

        public DiaryResponse Update(DiaryRegistry events)
        {
            DiaryResponse response = new DiaryResponse();
            int respuesta = 0;
            int time = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_diary", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@idstart", Convert.ToInt32(events.IdStart)));
                    cmd.Parameters.Add(new SqlParameter("@startdate", events.StartDate));
                    cmd.Parameters.Add(new SqlParameter("@idend", Convert.ToInt32(events.IdEnd)));
                    cmd.Parameters.Add(new SqlParameter("@enddate", events.EndDate));
                    cmd.Parameters.Add(new SqlParameter("@notes", Convert.ToString(events.Notes)));
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(events.Device)));
                    cmd.Parameters.Add(new SqlParameter("@responsable", Convert.ToInt32(events.Responsable)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    if (respuesta == 4)
                    {
                        response.success = false;
                        response.messages.Add("Error al intenar actualizar el Evento, verificarlo con su administrador");
                        return response;
                    }
                    if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Error al intenar actualizar el Evento, ya existe la combinacion");
                        return response;
                    }
                    if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("El Evento que tratas de actualizar no existe, verifique por favor");
                        return response;
                    }
                    else if (respuesta == 1)
                    {
                        response.success = true;
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

        public DiaryListResponse Read(DateTime startdate,  DateTime enddate, string node)
        {
            DiaryListResponse response = new DiaryListResponse();
            List<DiaryResgitrys> listEvents = new List<DiaryResgitrys>();
            DiaryResgitrys events = new DiaryResgitrys();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_ad_consult_diary", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", node));
                    cmd.Parameters.Add(new SqlParameter("@startdate", startdate));
                    cmd.Parameters.Add(new SqlParameter("@enddate", enddate));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events = new DiaryResgitrys();
                            events.IdStart = Convert.ToInt32(reader["IdI"]);
                            events.StartDate = Convert.ToDateTime(reader["StartDate"]);
                            events.IdEnd = Convert.ToInt32(reader["IdF"]);
                            events.EndDate = Convert.ToDateTime(reader["EndDate"]);
                            events.Device = Convert.ToString(reader["Device"]);
                            events.Vehicle = Convert.ToString(reader["NombreVehiculo"]);
                            events.Responsable = Convert.ToInt32(reader["IdResponsable"]);
                            events.Name = Convert.ToString(reader["Responsable"]);
                            events.Notes = Convert.ToString(reader["Notas"]);
                            listEvents.Add(events);
                        }
                        reader.Close();
                        response.success = true;
                        response.ListEvents = listEvents;
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

        public DiaryResponse Delete(int IdStar, int IdEnd)
        {
            int respuesta = 0;
            DiaryResponse response = new DiaryResponse();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_delete_diary", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@idstart", Convert.ToInt32(IdStar)));
                    cmd.Parameters.Add(new SqlParameter("@idend", Convert.ToInt32(IdEnd)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Error al intenar eliminar el Evento");
                    }
                    if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("El Evento que tratas de eliminar no existe, verifique por favor");
                    }
                    else if (respuesta == 1)
                    {
                        response.success = true;
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
    }
}