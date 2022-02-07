using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.RouteDiary;
using CredencialSpiderFleet.Models.Useful;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Response.RouteDiary;
using SpiderFleetWebAPI.Utils.Route;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.RouteDiary
{
    public class RouteDiaryDao
    {
        /// <summary>
        /// 
        /// </summary>
        public RouteDiaryDao() { }

        private MongoDBContext mongoDBContext = new MongoDBContext();
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();        
        private const string SEMANAL = "Semanal";

        private DateTime ObtenerUltimoDiaSemanaDelMes(int anio, int mes, DayOfWeek dia)
        {
            DateTime ultimoDiaMes = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));
            int diferencia = ultimoDiaMes.DayOfWeek - dia;
            return diferencia > 0 ? ultimoDiaMes.AddDays(-1 * diferencia) : ultimoDiaMes.AddDays(-1 * (7 + diferencia));
        }

        public RouteDiaryResponse Create(CredencialSpiderFleet.Models.RouteDiary.RouteDiary events)
        {
            RouteDiaryResponse response = new RouteDiaryResponse();
            string respuesta = string.Empty;

            int time = 1;
            List<Models.Mongo.RouteConsola.RouteData> ListConsola = new List<Models.Mongo.RouteConsola.RouteData>();

            if (events.Frecuency.Equals(SEMANAL))
            {
                DateTime lastDay = ObtenerUltimoDiaSemanaDelMes(events.StartDate.Year, events.StartDate.Month, events.StartDate.DayOfWeek);

                while (true)
                {
                    int compare = UseFul.compare(lastDay, events.StartDate);

                    if (compare <= 0)
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
                        SqlCommand cmd = new SqlCommand("ad.sp_create_route_diary", cn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@node", events.Node));
                        cmd.Parameters.Add(new SqlParameter("@startdate", events.StartDate));
                        cmd.Parameters.Add(new SqlParameter("@enddate", events.EndDate));
                        cmd.Parameters.Add(new SqlParameter("@notes", Convert.ToString(events.Notes)));
                        cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(events.Device)));
                        cmd.Parameters.Add(new SqlParameter("@route", Convert.ToString(events.Route)));

                        SqlParameter sqlParameter = new SqlParameter();
                        sqlParameter.ParameterName = "@cMensaje";
                        sqlParameter.SqlDbType = SqlDbType.VarChar;
                        sqlParameter.Size = 30;
                        sqlParameter.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(sqlParameter);
                        cmd.ExecuteNonQuery();

                        //respuesta = Convert.ToInt32(sqlParameter.Value.ToString());
                        respuesta = sqlParameter.Value.ToString();

                        if (respuesta.Contains("fail"))
                        {

                            if (respuesta.Contains("3"))
                            {
                                response.success = false;
                                response.messages.Add("Error al intenar dar de alta el Evento");
                            }
                            else
                            {
                                response.success = false;
                                response.messages.Add("El Evento que tratas de dar de alta ya existe con las siguientes fechas "
                                    + events.StartDate.ToString("dd-MM-yyyy HH:mm") + " y " + events.EndDate.ToString("dd-MM-yyyy HH:mm") +
                                    ", verifique por favor");
                            }                               
                        }
                        else if (Convert.ToInt32(respuesta) > 0)
                        {
                            response.success = true;
                            Models.Mongo.RouteConsola.RouteData consola = new Models.Mongo.RouteConsola.RouteData();
                            consola.IdRegistry = Convert.ToInt32(respuesta) - 1;
                            consola.Device = events.Device;
                            consola.IdRoute = events.Route;
                            consola.Start = events.StartDate;
                            consola.End = events.EndDate;
                            ListConsola.Add(consola);
                        }

                        if (time != 1)
                        {
                            events.StartDate = events.StartDate.AddDays(7);
                            events.EndDate = events.EndDate.AddDays(7);
                        }
                    }
                }

                //Consola
                if(ListConsola.Count > 0)
                {
                    (new RouteDao()).CreateConsola(ListConsola);
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

        public RouteDiaryResponse Update(RouteDiaryRegistry events)
        {
            RouteDiaryResponse response = new RouteDiaryResponse();
            int respuesta = 0;
            int time = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_route_diary", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@idstart", Convert.ToInt32(events.IdStart)));
                    cmd.Parameters.Add(new SqlParameter("@startdate", events.StartDate));
                    cmd.Parameters.Add(new SqlParameter("@idend", Convert.ToInt32(events.IdEnd)));
                    cmd.Parameters.Add(new SqlParameter("@enddate", events.EndDate));
                    cmd.Parameters.Add(new SqlParameter("@notes", Convert.ToString(events.Notes)));
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(events.Device)));
                    cmd.Parameters.Add(new SqlParameter("@route", Convert.ToString(events.Route)));

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
                        //Consola
                        Models.Mongo.RouteConsola.RouteData consola = new Models.Mongo.RouteConsola.RouteData();
                        consola.IdRegistry = events.IdStart;
                        consola.Device = events.Device;
                        consola.IdRoute = events.Route;
                        consola.Start = events.StartDate;
                        consola.End = events.EndDate;

                        (new RouteDao()).UpdateConsola(consola);
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

        public RouteDiaryListResponse Read(DateTime startdate, DateTime enddate, string node)
        {
            RouteDiaryListResponse response = new RouteDiaryListResponse();
            List<RouteDiaryResgitrys> listEvents = new List<RouteDiaryResgitrys>();
            RouteDiaryResgitrys events = new RouteDiaryResgitrys();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_ad_consult_route_diary", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", node));
                    cmd.Parameters.Add(new SqlParameter("@startdate", startdate));
                    cmd.Parameters.Add(new SqlParameter("@enddate", enddate));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events = new RouteDiaryResgitrys();
                            events.IdStart = Convert.ToInt32(reader["IdI"]);
                            events.StartDate = Convert.ToDateTime(reader["StartDate"]);
                            events.IdEnd = Convert.ToInt32(reader["IdF"]);
                            events.EndDate = Convert.ToDateTime(reader["EndDate"]);
                            events.Device = Convert.ToString(reader["Device"]);
                            events.Vehicle = Convert.ToString(reader["NombreVehiculo"]);
                            events.Route = Convert.ToString(reader["IdRoute"]);
                            events.Name = GetRouteName(events.Route);
                            events.Notes = Convert.ToString(reader["Notas"]);
                            listEvents.Add(events);
                        }
                        reader.Close();

                        if(listEvents.Count > 0)
                        {
                            response.success = true;
                            response.ListEvents = listEvents;
                        }
                        else
                        {
                            response.success = false;
                        }
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

        public RouteDiaryResponse Delete(int IdStar, int IdEnd)
        {
            int respuesta = 0;
            RouteDiaryResponse response = new RouteDiaryResponse();
            RouteDiaryResgitrys registry = new RouteDiaryResgitrys();
            try
            {
                if (sql.IsConnection)
                {

                    registry = ReadId(IdStar);

                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_delete_route_diary", cn);
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
                        //Consola                       
                        Models.Mongo.RouteConsola.RouteData consola = new Models.Mongo.RouteConsola.RouteData();
                        consola.IdRegistry = IdStar;
                        consola.Device = registry.Device;
                        consola.IdRoute = registry.Route;
                        consola.Start = DateTime.Now;
                        consola.End = DateTime.Now;

                        (new RouteDao()).DeleteConsola(consola);
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

        private RouteDiaryResgitrys ReadId(int id)
        {            
            RouteDiaryResgitrys events = new RouteDiaryResgitrys();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_route_diary_id", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events = new RouteDiaryResgitrys();
                            events.Device = Convert.ToString(reader["Device"]);
                            events.Route = Convert.ToString(reader["IdRoute"]);
                            
                        }
                        reader.Close();
                    }
                }
                else
                {
                    events = new RouteDiaryResgitrys();
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
                //cn.Close();
            }
            return events;
        }


        private string GetRouteName(string id)
        {
            string name = string.Empty;             

            try
            {

                var bsonArray = new BsonArray();                
                bsonArray.Add(ObjectId.Parse(id));

                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("_id", new BsonDocument("$in", bsonArray)));
                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.Route>("Route");
                var result = stored.Find(build).FirstOrDefault();

                if (result !=null)
                {
                    name = result.Name;
                }                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return name;
        }

        public ListRouteDataResponse GetMongoData(string node)
        {
            ListRouteDataResponse response = new ListRouteDataResponse();
            List<RouteData> list = new List<RouteData>();
            RouteData route = new RouteData();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_routes_by_hierarchy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            route = new RouteData();
                            route.IdMongo = Convert.ToString(reader["MongoId"]);
                            route.Name = GetRouteName(route.IdMongo);
                            list.Add(route);
                        }
                        reader.Close();

                        response.ListRoutes = list;
                        if(list.Count > 0)
                        {
                            response.success = true;
                            response.ListRoutes = list;
                        }
                        else
                        {
                            response.success = false;
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
    }
}