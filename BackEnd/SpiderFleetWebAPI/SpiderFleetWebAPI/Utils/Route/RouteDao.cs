using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Route;
using CredencialSpiderFleet.Models.Useful;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.Route;
using SpiderFleetWebAPI.Models.Mongo.RouteConsola;
using SpiderFleetWebAPI.Models.Response.Route;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Route
{
    public class RouteDao
    {

        private MongoDBContext mongoDBContext = new MongoDBContext();
        private UseFul use = new UseFul();
        private const int longitudName = 100;
        private const int longitudDescription = 100;

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public RouteDao() { }

        #region Mongo
        public RouteResponse Create(Models.Mongo.Route.Route trace, string hierarchy)
        {
            RouteResponse response = new RouteResponse();

            if (string.IsNullOrEmpty(trace.Name))
            {
                response.success = false;
                response.messages.Add("El campo nombre no puede estar vacio favor de verificar");
                return response;
            }

            if (string.IsNullOrEmpty(trace.Description))
            {
                response.success = false;
                response.messages.Add("El campo Descripcion no puede estar vacio favor de verificar");
                return response;
            }
           

            if (!string.IsNullOrEmpty(trace.Name.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(trace.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(trace.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Nombre excede de lo establecido rango maximo " + longitudName + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(trace.Description.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(trace.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(trace.Description.Trim(), longitudDescription))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Descripcion excede de lo establecido rango maximo " + longitudDescription + " caracteres");
                    return response;
                }
            }

            try
            {
                int valida = ValidaNombre(hierarchy, trace.Name);

                if (valida == 2)
                {
                    response.success = false;
                    response.messages.Add("El nombre de la Ruta ya se encuentra registrado, trate con un nombre diferente.");
                }
                else
                {
                    mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.Route>("Route").InsertOne(trace);
                    Create(hierarchy, trace.Name, trace.Id, trace.Description);
                    response.success = true;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        public RouteResponse Update(Models.Mongo.Route.Route trace, string hierarchy)
        {
            RouteResponse response = new RouteResponse();

            try
            {
                if (string.IsNullOrEmpty(trace.Id))
                {
                    response.success = false;
                    response.messages.Add("El campo Id no puede estar vacio favor de verificar");
                    return response;
                }

                if (string.IsNullOrEmpty(trace.Name))
                {
                    response.success = false;
                    response.messages.Add("El campo nombre no puede estar vacio favor de verificar");
                    return response;
                }

                if (string.IsNullOrEmpty(trace.Description))
                {
                    response.success = false;
                    response.messages.Add("El campo Descripcion no puede estar vacio favor de verificar");
                    return response;
                }

                if (!string.IsNullOrEmpty(trace.Name.Trim()))
                {
                    use = new UseFul();

                    if (use.hasSpecialChar(trace.Name.Trim()))
                    {
                        response.success = false;
                        response.messages.Add("La cadena contiene caracteres especiales");
                        return response;
                    }

                    if (!use.IsValidLength(trace.Name.Trim(), longitudName))
                    {
                        response.success = false;
                        response.messages.Add("La longitud del campo Nombre excede de lo establecido rango maximo " + longitudName + " caracteres");
                        return response;
                    }
                }

                if (!string.IsNullOrEmpty(trace.Description.Trim()))
                {
                    use = new UseFul();

                    if (use.hasSpecialChar(trace.Description.Trim()))
                    {
                        response.success = false;
                        response.messages.Add("La cadena contiene caracteres especiales");
                        return response;
                    }

                    if (!use.IsValidLength(trace.Description.Trim(), longitudName))
                    {
                        response.success = false;
                        response.messages.Add("La longitud del campo Descripcion excede de lo establecido rango maximo " + longitudDescription + " caracteres");
                        return response;
                    }
                }

                if (!string.IsNullOrEmpty(trace.Id))
                {
                    var build = Builders<Models.Mongo.Route.Route>.Filter.Eq(u => u.Id, trace.Id);
                    var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.Route>("Route");
                    var result = stored.Find(build).FirstOrDefault();

                    if (result == null)
                    {
                        response.success = false;
                        response.messages.Add("El registro que intenta actualizar no existe ,verifique la información");
                        return response;
                    }
                    else
                    {
                        result.Name = trace.Name;
                        result.Description = trace.Description;
                        result.Active = trace.Active;
                        result.Polygon = trace.Polygon;
                        //result.Trace = trace.Trace;
                        mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.Route>("Route").ReplaceOneAsync(build, result);
                        Update(hierarchy, trace.Name, trace.Id, trace.Description, trace.Active == true ? 1 : 0);

                        //Consola                       
                        //var filterRouteDevice = Builders<Models.Mongo.Route.RouteDevice>.Filter.Eq(x => x.IdRoute, trace.Id);
                        //var storedRouteDevice = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.RouteDevice>("RouteDevice");
                        //var resultRouteDevice = storedRouteDevice.Find(filterRouteDevice).ToList();

                        //if (resultRouteDevice.Count > 0)
                        //{
                        //    string deviceAnterior = string.Empty;

                        //    foreach (var item in resultRouteDevice)
                        //    {
                        //        if (!deviceAnterior.Equals(item.Device))
                        //        {
                        //            var filterConsola = Builders<RouteConsola>.Filter.Eq(x => x.Device, item.Device);
                        //            var storedConsola = mongoDBContext.spiderMongoDatabase.GetCollection<RouteConsola>("RouteConsola");
                        //            var resultConsola = storedConsola.Find(filterConsola).FirstOrDefault();

                        //            if (resultConsola != null)
                        //            {
                        //                foreach (var fence in resultConsola.Fences)
                        //                {
                        //                    if (fence.Id.Equals(trace.Id))
                        //                    {
                        //                        fence.Name = trace.Name;
                        //                        fence.Description = trace.Description;
                        //                        fence.Active = trace.Active;
                        //                        fence.Polygon = trace.Polygon;
                        //                    }
                        //                }
                        //                mongoDBContext.spiderMongoDatabase.GetCollection<RouteConsola>("RouteConsola").ReplaceOneAsync(filterConsola, resultConsola);
                        //            }
                        //        }
                        //        deviceAnterior = item.Device;
                        //    }
                        //}
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

            return response;
        }

        public RoutesListResponse Read(string hierarchy)
        {
            List<Routes> listRoutes = new List<Routes>();
            RoutesListResponse response = new RoutesListResponse();
            List<string> listIds = new List<string>();

            try
            {

                listIds = GetMongoId(hierarchy);

                var bsonArray = new BsonArray();
                foreach (var id in listIds)
                {
                    bsonArray.Add(ObjectId.Parse(id));
                }

                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("_id", new BsonDocument("$in", bsonArray)));
                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.Route>("Route");
                var result = stored.Find(build).ToList();

                if (result.Count > 0)
                {
                    foreach (Models.Mongo.Route.Route data in result)
                    {
                        Routes routes = new Routes();
                        routes.Id = data.Id;
                        routes.Name = data.Name;
                        routes.Description = data.Description;
                        routes.Active = data.Active;
                        routes.ListPoints = data.Polygon.Coordinates[0];

                        listRoutes.Add(routes);
                    }

                    response.success = true;
                    response.routes = listRoutes;
                }
                else
                {
                    response.success = false;
                    response.messages.Add("No se encontraron registros");
                    response.routes = listRoutes;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        public RouteRegistryResponse ReadId(string id)
        {
            RouteRegistryResponse response = new RouteRegistryResponse();
            Routes routes = new Routes();
            try
            {

                var bsonArray = new BsonArray();
                bsonArray.Add(ObjectId.Parse(id));

                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("_id", new BsonDocument("$in", bsonArray)));
                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.Route>("Route");
                var result = stored.Find(build).ToList();

                if (result.Count > 0)
                {
                    foreach (Models.Mongo.Route.Route data in result)
                    {
                        
                        routes.Id = data.Id;
                        routes.Name = data.Name;
                        routes.Description = data.Description;
                        routes.Active = data.Active;
                        routes.ListPoints = data.Polygon.Coordinates[0];
                    }

                    response.success = true;
                    response.routes = routes;
                }
                else
                {
                    response.success = false;
                    response.messages.Add("No se encontraron registros");
                    response.routes = routes;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        public RouteResponse DeleteId(string id)
        {
            RouteResponse response = new RouteResponse();
            Routes routes = new Routes();
            try
            {

                var bsonArray = new BsonArray();
                bsonArray.Add(ObjectId.Parse(id));

                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("_id", new BsonDocument("$in", bsonArray)));
                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.Route>("Route");
                var result = stored.Find(build).ToList();

                if (result.Count > 0)
                {
                    var delete = Builders<Models.Mongo.Route.Route>.Filter.Eq(x => x.Id, id);
                    mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.Route>("Route").DeleteOne(delete);
                    Delete(id);
                    response.success = true;

                    //Consola                       
                    var filterRouteDevice = Builders<Models.Mongo.Route.RouteDevice>.Filter.Eq(x => x.IdRoute, id);
                    var storedRouteDevice = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.RouteDevice>("RouteDevice");
                    var resultRouteDevice = storedRouteDevice.Find(filterRouteDevice).ToList();

                    //if (resultRouteDevice.Count > 0)
                    //{
                    //    foreach (var item in resultRouteDevice)
                    //    {
                    //        var filterConsola = Builders<RouteConsola>.Filter.Eq(x => x.Device, item.Device);
                    //        var storedConsola = mongoDBContext.spiderMongoDatabase.GetCollection<RouteConsola>("RouteConsola");
                    //        var resultConsola = storedConsola.Find(filterConsola).FirstOrDefault();

                    //        if (resultConsola != null)
                    //        {

                    //            List<Schedule> schedules = new List<Schedule>();

                    //            foreach (var schedul in resultConsola.Schedule)
                    //            {
                    //                if (!schedul.IdRoute.Equals(id))
                    //                {
                    //                    Schedule schedule = new Schedule();
                    //                    schedule = schedul;
                    //                    schedules.Add(schedule);
                    //                }
                    //            }
                    //            resultConsola.Schedule = schedules;

                    //            List<RouteConsolas> Fences = new List<RouteConsolas>();
                    //            foreach (var fence in resultConsola.Fences)
                    //            {
                    //                if (!fence.Id.Equals(id))
                    //                {
                    //                    RouteConsolas consolas = new RouteConsolas();
                    //                    consolas = fence;
                    //                    Fences.Add(consolas);
                    //                }
                    //            }
                    //            resultConsola.Fences = Fences;
                    //            mongoDBContext.spiderMongoDatabase.GetCollection<RouteConsola>("RouteConsola").ReplaceOneAsync(filterConsola, resultConsola);
                    //        }
                    //    }

                    //    //Eliminar RouteDevice
                    //    var deleteRouteDevice = Builders<Models.Mongo.Route.RouteDevice>.Filter.Eq(x => x.IdRoute, id);
                    //    mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.RouteDevice>("RouteDevice").DeleteMany(deleteRouteDevice);
                        
                    //}
                }
                else
                {
                    response.success = false;
                    response.messages.Add("No se encontraron registros");
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        private string abreviaDia(DateTime dia)
        {
            string salida = string.Empty;
            int numero = (int)dia.DayOfWeek;

            switch (numero)
            {
                case 1:
                    salida = "L";
                    break;
                case 2:
                    salida = "M";
                    break;
                case 3:
                    salida = "MM";
                    break;
                case 4:
                    salida = "J";
                    break;
                case 5:
                    salida = "V";
                    break;
                case 6:
                    salida = "S";
                    break;
                case 0:
                    salida = "D";
                    break;
                default:
                    salida = "D";
                    break;

            }

            return salida;
        }

        public RouteResponse CreateConsola(List<RouteData> consola)
        {
            RouteResponse response = new RouteResponse();

            try
            {
                int horas = VerifyUser.VerifyUser.GetHours();

                var bsonArray = new BsonArray();
                bsonArray.Add(ObjectId.Parse(consola[0].IdRoute));

                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("_id", new BsonDocument("$in", bsonArray)));
                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.Route>("Route");
                var result = stored.Find(build).FirstOrDefault();
                
                if(result != null)
                {                    
                    var filterConsola = Builders<RouteConsola>.Filter.Eq(x => x.Device, consola[0].Device);
                    var storedConsola = mongoDBContext.spiderMongoDatabase.GetCollection<RouteConsola>("RouteConsola");
                    var resultConsola = storedConsola.Find(filterConsola).FirstOrDefault();

                    if (resultConsola != null)
                    {
                        int find = resultConsola.Routes.Where(x => x.Id == consola[0].IdRoute).Count();

                        if(find > 0)
                        {
                            foreach (var item in resultConsola.Routes)
                            {
                                if (item.Id.Equals(consola[0].IdRoute))
                                {

                                    Schedule schedule = new Schedule();
                                    schedule.Id = consola[0].IdRegistry;
                                    schedule.concurrent = abreviaDia(consola[0].Start);// consola[0].Start.ToString("dddd");
                                    schedule.Start = consola[0].Start.ToString("HH:mm");
                                    schedule.End = consola[0].End.ToString("HH:mm");

                                    Events events = new Events();
                                    events.Id = consola[0].IdRegistry;
                                    events.Start = consola[0].Start.AddHours(-horas);
                                    int lastRegistry = consola.Count();
                                    events.End = consola[lastRegistry - 1].End.AddHours(-horas);

                                    item.Schedule.Add(schedule);
                                    item.Events.Add(events);
                                }
                            }
                        }
                        else
                        {
                            RouteConsolas consolas = new RouteConsolas();
                            consolas.Id = consola[0].IdRoute;
                            consolas.Active = result.Active;

                            List<Schedule> listSchedule = new List<Schedule>();
                            Schedule schedule = new Schedule();
                            schedule.Id = consola[0].IdRegistry;
                            schedule.concurrent = abreviaDia(consola[0].Start); //consola[0].Start.ToString("dddd");
                            schedule.Start = consola[0].Start.ToString("HH:mm");
                            schedule.End = consola[0].End.ToString("HH:mm");
                            listSchedule.Add(schedule);
                            consolas.Schedule = listSchedule;                            

                            List<Events> listEvents = new List<Events>();
                            Events events = new Events();
                            events.Id = consola[0].IdRegistry;
                            events.Start = consola[0].Start.AddHours(-horas);
                            int lastRegistry = consola.Count();
                            events.End = consola[lastRegistry - 1].End.AddHours(-horas);
                            listEvents.Add(events);
                            consolas.Events = listEvents;
                            resultConsola.Routes.Add(consolas);
                        }
                        
                        mongoDBContext.spiderMongoDatabase.GetCollection<RouteConsola>("RouteConsola").ReplaceOneAsync(filterConsola, resultConsola);

                        //    Models.Mongo.Route.RouteDevice routeDevice = new Models.Mongo.Route.RouteDevice();
                        //    routeDevice.IdRegistry = consola.IdRegistry;
                        //    routeDevice.IdRoute = consola.IdRoute;
                        //    routeDevice.IdRegistry = consola.IdRegistry;
                        //    routeDevice.Start = consola.Start.AddHours(-horas);
                        //    routeDevice.End = consola.End.AddHours(-horas);
                        //    routeDevice.Device = consola.Device;
                        //    mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.RouteDevice>("RouteDevice").InsertOne(routeDevice);
                    }
                    else
                    {
                        resultConsola = new RouteConsola();
                        resultConsola.Routes = new List<RouteConsolas>();

                        resultConsola.Device = consola[0].Device;
                        RouteConsolas consolas = new RouteConsolas();
                        consolas.Id = consola[0].IdRoute;
                        consolas.Active = result.Active;

                        List<Schedule> listSchedule = new List<Schedule>();
                        Schedule schedule = new Schedule();
                        schedule.Id = consola[0].IdRegistry;
                        schedule.concurrent = abreviaDia(consola[0].Start); //consola[0].Start.ToString("dddd");
                        schedule.Start = consola[0].Start.ToString("HH:mm");
                        schedule.End = consola[0].End.ToString("HH:mm");
                        listSchedule.Add(schedule);
                        consolas.Schedule = listSchedule;

                        List<Events> listEvents = new List<Events>();
                        Events events = new Events();
                        events.Id = consola[0].IdRegistry;
                        events.Start = consola[0].Start.AddHours(-horas);
                        int lastRegistry = consola.Count();
                        events.End = consola[lastRegistry -1].End.AddHours(-horas);
                        listEvents.Add(events);
                        consolas.Events = listEvents;

                        resultConsola.Routes.Add(consolas);
                        mongoDBContext.spiderMongoDatabase.GetCollection<RouteConsola>("RouteConsola").InsertOne(resultConsola);

                        Models.Mongo.Route.RouteDevice routeDevice = new Models.Mongo.Route.RouteDevice();
                        //routeDevice.IdRegistry = consola.IdRegistry;
                        //routeDevice.IdRoute = consola.IdRoute;
                        //routeDevice.IdRegistry = consola.IdRegistry;
                        //routeDevice.Start = consola.Start.AddHours(-horas);
                        //routeDevice.End = consola.End.AddHours(-horas);
                        //routeDevice.Device = consola.Device;
                        //mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.RouteDevice>("RouteDevice").InsertOne(routeDevice);
                    }
                }   
                else
                {
                    response.success = false;
                    response.messages.Add("No existe la Ruta seleccionada");
                    return response;
                }
                response.success = true;                
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        public RouteResponse UpdateConsola(RouteData consola)
        {
            RouteResponse response = new RouteResponse();

            try
            {
                int horas = VerifyUser.VerifyUser.GetHours();

                var bsonArray = new BsonArray();
                bsonArray.Add(ObjectId.Parse(consola.IdRoute));

                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("_id", new BsonDocument("$in", bsonArray)));
                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Route.Route>("Route");
                var result = stored.Find(build).FirstOrDefault();

                if (result != null)
                {
                    var filterConsola = Builders<RouteConsola>.Filter.Eq(x => x.Device, consola.Device);
                    var storedConsola = mongoDBContext.spiderMongoDatabase.GetCollection<RouteConsola>("RouteConsola");
                    var resultConsola = storedConsola.Find(filterConsola).FirstOrDefault();

                    if (resultConsola != null)
                    {
                        int find = resultConsola.Routes.Where(x => x.Id == consola.IdRoute).Count();

                        if (find > 0)
                        {
                            foreach (var item in resultConsola.Routes)
                            {
                                if (item.Id.Equals(consola.IdRoute))
                                {

                                    foreach (var sche in item.Schedule)
                                    {
                                        if (sche.Id == consola.IdRegistry)
                                        {
                                            sche.concurrent = abreviaDia(consola.Start); //consola.Start.ToString("dddd");
                                            sche.Start = consola.Start.ToString("HH:mm");
                                            sche.End = consola.End.ToString("HH:mm");
                                        }
                                    }

                                    foreach (var eve in item.Events)
                                    {
                                        if (eve.Id == consola.IdRegistry)
                                        {
                                            eve.Start = consola.Start;
                                            eve.End = consola.End;
                                        }
                                    }
                                }
                            }
                        }                        

                        mongoDBContext.spiderMongoDatabase.GetCollection<RouteConsola>("RouteConsola").ReplaceOneAsync(filterConsola, resultConsola);
                    }
                }
                else
                {
                    response.success = false;
                    response.messages.Add("No existe la Ruta seleccionada");
                    return response;
                }
                response.success = true;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        public RouteResponse DeleteConsola(RouteData consola)
        {
            RouteResponse response = new RouteResponse();

            try
            {
                var filterConsola = Builders<RouteConsola>.Filter.Eq(x => x.Device, consola.Device);
                var storedConsola = mongoDBContext.spiderMongoDatabase.GetCollection<RouteConsola>("RouteConsola");
                var resultConsola = storedConsola.Find(filterConsola).FirstOrDefault();

                if (resultConsola != null)
                {
                    int find = resultConsola.Routes.Where(x => x.Id == consola.IdRoute).Count();

                    if (find > 0)
                    {
                        List<Schedule> listSchedule = new List<Schedule>();
                        List<Events> listEvents = new List<Events>();

                        foreach (var item in resultConsola.Routes)
                        {
                            if (item.Id.Equals(consola.IdRoute))
                            {                             
                                //resultConsola.Routes.Add(consolas);

                                foreach (var sche in item.Schedule)
                                {
                                    if (sche.Id != consola.IdRegistry)
                                    {
                                        Schedule schedule = new Schedule();
                                        schedule.Id = sche.Id;
                                        schedule.concurrent = sche.concurrent;
                                        schedule.Start = sche.Start;
                                        schedule.End = sche.End;
                                        listSchedule.Add(schedule);
                                    }
                                }

                                item.Schedule = listSchedule;

                                foreach (var eve in item.Events)
                                {
                                    if (eve.Id != consola.IdRegistry)
                                    {
                                        Events events = new Events();
                                        events.Id = eve.Id;
                                        events.Start = eve.Start;                                        
                                        events.End = eve.End;
                                        listEvents.Add(events);

                                    }
                                }

                                item.Events = listEvents;
                            }
                        }
                    }

                    mongoDBContext.spiderMongoDatabase.GetCollection<RouteConsola>("RouteConsola").ReplaceOneAsync(filterConsola, resultConsola);
                    response.success = true;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        #endregion

        #region SQL
        private int ValidaNombre(string node, string nombre)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_validation_route_name", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(nombre)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());
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

        private int Create(string node, string nombre, string mongo, string description)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_route", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(nombre)));
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(description)));
                    cmd.Parameters.Add(new SqlParameter("@mongo", Convert.ToString(mongo)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());
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

        private int Update(string node, string nombre, string mongo, string description, int status)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_route", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(nombre)));
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(description)));
                    cmd.Parameters.Add(new SqlParameter("@mongo", Convert.ToString(mongo)));
                    cmd.Parameters.Add(new SqlParameter("@status", Convert.ToInt32(status)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());
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

        private List<string> GetMongoId(string node)
        {
            List<string> list = new List<string>();
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
                            list.Add(Convert.ToString(reader["MongoId"]));
                        }
                        reader.Close();
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
            return list;
        }

        private int Delete(string id)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_delete_route", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mongo", Convert.ToString(id)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());
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

        #endregion
    }
}