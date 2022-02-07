using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using CredencialSpiderFleet.Models.Useful;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.GeoFence;
using SpiderFleetWebAPI.Models.Response.Main.GeoFence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Main.GeoFence
{
    public class GeoFenceDao
    {
        private MongoDBContext mongoDBContext = new MongoDBContext();
        private UseFul use = new UseFul();
        private const int longitudName = 50;

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public GeoFenceDao() { }

        public GeoFenceResponse Create(Models.Mongo.GeoFence.GeoFence geoFence) //, string hierarchy)
        {
            Models.Mongo.GeoFence.GeoFence geoFences = new Models.Mongo.GeoFence.GeoFence();
            GeoFenceResponse response = new GeoFenceResponse();

            if (string.IsNullOrEmpty(geoFence.Name))
            {
                response.success = false;
                response.messages.Add("El campo nombre no puede estar vacio favor de verificar");
                return response;
            }

            if (!string.IsNullOrEmpty(geoFence.Name.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(geoFence.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(geoFence.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Nombre excede de lo establecido rango maximo " + longitudName + " caracteres");
                    return response;
                }
            }

            if (string.IsNullOrEmpty(geoFence.Hierarchy))
            {
                response.success = false;
                response.messages.Add("El campo Jerarquia no puede estar vacio favor de verificar");
                return response;
            }

            try
            {

                int valida = ValidaNombre(geoFence.Hierarchy, geoFence.Name);

                if (valida == 2)
                {
                    response.success = false;
                    response.messages.Add("El nombre de la Geo Cerca ya se encuentra registrado, trate con un nombre diferente.");
                }
                else
                {
                    mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence").InsertOne(geoFence);
                    response.success = true;
                    Create(geoFence.Hierarchy, geoFence.Name, geoFence.Id, geoFence.Description);
                }

                //var build = Builders<Models.Mongo.GeoFence.GeoFence>.Filter.Regex(x => x.Hierarchy, ($"/{geoFence.Hierarchy}/i"))
                //& Builders<Models.Mongo.GeoFence.GeoFence>.Filter.Regex(x => x.Name, geoFence.Name);
                //var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence");
                //var result = stored.Find(build).ToList();

                //if(result.Count > 0)
                //{
                //    response.success = false;
                //    response.messages.Add("El nombre de la GeoCerca ya se encuentra registrado, trate con un nombre diferente.");
                //}
                //else
                //{
                //    mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence").InsertOne(geoFence);
                //    response.success = true;
                //}
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        public GeoFenceResponse Update(Models.Mongo.GeoFence.GeoFence geoFence)//, string hierarchy)
        {
            Models.Mongo.GeoFence.GeoFence geoFences = new Models.Mongo.GeoFence.GeoFence();
            GeoFenceResponse response = new GeoFenceResponse();

            if (string.IsNullOrEmpty(geoFence.Name))
            {
                response.success = false;
                response.messages.Add("El campo nombre no puede estar vacio favor de verificar");
                return response;
            }

            if (!string.IsNullOrEmpty(geoFence.Name.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(geoFence.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(geoFence.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Nombre excede de lo establecido rango maximo " + longitudName + " caracteres");
                    return response;
                }
            }

            if (string.IsNullOrEmpty(geoFence.Hierarchy))
            {
                response.success = false;
                response.messages.Add("El campo Jerarquia no puede estar vacio favor de verificar");
                return response;
            }


            if (!string.IsNullOrEmpty(geoFence.Id))
            {
                var build = Builders<Models.Mongo.GeoFence.GeoFence>.Filter.Eq(u => u.Id, geoFence.Id);
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence");
                var result = stored.Find(build).FirstOrDefault();

                if(result == null)
                {
                    response.success = false;
                    response.messages.Add("El registro que intenta actualizar no existe ,verifique la información");
                    return response;
                }  
            }

            try
            {

                var build = Builders<Models.Mongo.GeoFence.GeoFence>.Filter.Regex(x => x.Name, geoFence.Name);
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence");
                var result = stored.Find(build).FirstOrDefault();

                if (result != null)
                {
                    if (geoFence.Id.Equals(result.Id))
                    {
                        var update = Builders<Models.Mongo.GeoFence.GeoFence>.Update
                        .Set("name", geoFence.Name)
                        .Set("hierarchy", geoFence.Hierarchy)
                        .Set("active", geoFence.Active)
                        .Set("description", geoFence.Description)
                        .Set("polygon", geoFence.Polygon);

                        var filter = Builders<Models.Mongo.GeoFence.GeoFence>.Filter.Eq(u => u.Id, geoFence.Id);
                        mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence").UpdateOne(filter, update);
                        response.success = true;
                        Update(geoFence.Hierarchy, geoFence.Name, geoFence.Id, geoFence.Description, geoFence.Active == true ? 1 : 0);

                        #region Consolas

                        var buildGeoDevice = Builders<Models.Mongo.GeoFenceDevice.GeoFenceDevice>.Filter.And(
                            Builders<Models.Mongo.GeoFenceDevice.GeoFenceDevice>.Filter.Eq(x => x.IdGeoFence, geoFence.Id)
                        );
                        var storedGeoDevice = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFenceDevice.GeoFenceDevice>("GeoFenceDevice");
                        var resultEquals = storedGeoDevice.Find(buildGeoDevice).ToList();

                        if (resultEquals.Count > 0)
                        {
                            foreach (var data in resultEquals)
                            {
                                var filterConsola = Builders<GeoFenceConsola>.Filter.Eq(x => x.Device, data.Device);
                                var storedConsola = mongoDBContext.spiderMongoDatabase.GetCollection<GeoFenceConsola>("GeoFenceConsola");
                                var resultConsola = storedConsola.Find(filterConsola).FirstOrDefault();

                                if (resultConsola != null)
                                {
                                    if(resultConsola.Fences != null)
                                    {
                                        foreach (var z in resultConsola.Fences)
                                        {
                                            if (z.Id.Equals(geoFence.Id))
                                            {
                                                z.Name = geoFence.Name;
                                                z.Polygon = geoFence.Polygon;
                                                z.Active = geoFence.Active;
                                                z.Description = geoFence.Description;

                                                mongoDBContext.spiderMongoDatabase.GetCollection<GeoFenceConsola>("GeoFenceConsola").ReplaceOneAsync(filterConsola, resultConsola);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        GeoFenceConsolas geo = new GeoFenceConsolas();
                                        geo.Id = geoFence.Id;
                                        geo.Name = geoFence.Name;
                                        geo.Polygon = geoFence.Polygon;
                                        geo.Active = geoFence.Active;
                                        geo.Description = geoFence.Description;
                                        resultConsola.Fences = new List<GeoFenceConsolas>();
                                        resultConsola.Fences.Add(geo);
                                        mongoDBContext.spiderMongoDatabase.GetCollection<GeoFenceConsola>("GeoFenceConsola").ReplaceOneAsync(filterConsola, resultConsola);
                                    }                                   
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        response.success = false;
                        response.messages.Add("El nombre de la GeoCerca ya se encuentra registrado, trate con un nombre diferente.");
                        return response;
                    }

                }
                else
                {
                    var update = Builders<Models.Mongo.GeoFence.GeoFence>.Update
                    .Set("name", geoFence.Name)
                    .Set("hierarchy", geoFence.Hierarchy)
                    .Set("active", geoFence.Active)
                    .Set("description", geoFence.Description)
                    .Set("polygon", geoFence.Polygon);

                    var filter = Builders<Models.Mongo.GeoFence.GeoFence>.Filter.Eq(u => u.Id, geoFence.Id);
                    mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence").UpdateOne(filter, update);
                    response.success = true;
                    Update(geoFence.Hierarchy, geoFence.Name, geoFence.Id, geoFence.Description, geoFence.Active == true ? 1 : 0);

                    #region Consolas

                    var buildGeoDevice = Builders<Models.Mongo.GeoFenceDevice.GeoFenceDevice>.Filter.And(
                        Builders<Models.Mongo.GeoFenceDevice.GeoFenceDevice>.Filter.Eq(x => x.IdGeoFence, geoFence.Id)
                    );
                    var storedGeoDevice = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFenceDevice.GeoFenceDevice>("GeoFenceDevice");
                    var resultEquals = storedGeoDevice.Find(buildGeoDevice).ToList();

                    if (resultEquals.Count > 0)
                    {
                        foreach (var data in resultEquals)
                        {
                            var filterConsola = Builders<GeoFenceConsola>.Filter.Eq(x => x.Device, data.Device);
                            var storedConsola = mongoDBContext.spiderMongoDatabase.GetCollection<GeoFenceConsola>("GeoFenceConsola");
                            var resultConsola = storedConsola.Find(filterConsola).FirstOrDefault();

                            if(resultConsola != null)
                            {
                                foreach (var z in resultConsola.Fences)
                                {
                                    if(z.Id.Equals(geoFence.Id))
                                    {
                                        z.Name = geoFence.Name;
                                        z.Polygon = geoFence.Polygon;
                                        z.Active = geoFence.Active;
                                        z.Description = geoFence.Description;

                                        mongoDBContext.spiderMongoDatabase.GetCollection<GeoFenceConsola>("GeoFenceConsola").ReplaceOneAsync(filterConsola, resultConsola);
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                
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

        public GeoFenceListHierarchyResponse Read(string hierarchy)
        {
            List<GeoFences> listGeoFence = new List<GeoFences>();
            GeoFenceListHierarchyResponse response = new GeoFenceListHierarchyResponse();

            try
            {
                List<string> listIds = new List<string>();
                listIds = GetMongoId(hierarchy);

                var bsonArray = new BsonArray();
                foreach (var id in listIds)
                {
                    bsonArray.Add(ObjectId.Parse(id));
                }

                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("_id", new BsonDocument("$in", bsonArray)));
                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence");
                var result = stored.Find(build).ToList();

                //var build = Builders<Models.Mongo.GeoFence.GeoFence>.Filter.Regex(x => x.Hierarchy, ($"/{hierarchy}/i"));
                //var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence");
                //var resultRole = stored.Find(build).ToList();

                if (result == null)
                {
                    response.listGeoFence = listGeoFence;
                }
                else
                {
                    foreach (Models.Mongo.GeoFence.GeoFence data in result)
                    {
                        List<CoordenatesData> listCoordenates = new List<CoordenatesData>();
                        GeoFences geo = new GeoFences();
                        geo.Id = data.Id;
                        geo.Name = data.Name;
                        geo.Hierarchy = data.Hierarchy;
                        geo.Active = data.Active;
                        geo.Description = data.Description;

                        List<List<double>> Coordinates = data.Polygon.Coordinates[0];
                        foreach(List<double>  coor in Coordinates)
                        {
                            CoordenatesData coordenates = new CoordenatesData();
                            coordenates.lng = coor[0];
                            coordenates.lat = coor[1];

                            listCoordenates.Add(coordenates);
                        }

                        Polygons polygons = new Polygons();
                        polygons.Type = data.Polygon.Type;
                        polygons.Coordinates = listCoordenates;
                        geo.Polygon = polygons;                        
                        listGeoFence.Add(geo);

                    }

                    response.success = true;
                    response.listGeoFence = listGeoFence;
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

        public GeoFenceRegistryResponse Read(string id, string hierarchy)
        {
            GeoFences GeoFence = new GeoFences();
            GeoFenceRegistryResponse response = new GeoFenceRegistryResponse();
            try
            {
                var bsonArray = new BsonArray();
                bsonArray.Add(ObjectId.Parse(id));

                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("_id", new BsonDocument("$in", bsonArray)));
                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence");
                var result = stored.Find(build).ToList();

                //var build = Builders<Models.Mongo.GeoFence.GeoFence>.Filter.Eq(x => x.Id, id);
                //var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence");
                //var resultRole = stored.Find(build).ToList();

                if (result == null)
                {
                    response.GeoFence = GeoFence;
                }
                else
                {
                    foreach (Models.Mongo.GeoFence.GeoFence data in result)
                    {
                        List<CoordenatesData> listCoordenates = new List<CoordenatesData>();
                        GeoFence.Id = data.Id;
                        GeoFence.Name = data.Name;
                        GeoFence.Hierarchy = data.Hierarchy;
                        GeoFence.Active = data.Active;
                        GeoFence.Description = data.Description;

                        List<List<double>> Coordinates = data.Polygon.Coordinates[0];
                        foreach (List<double> coor in Coordinates)
                        {
                            CoordenatesData coordenates = new CoordenatesData();
                            coordenates.lng = coor[0];
                            coordenates.lat = coor[1];

                            listCoordenates.Add(coordenates);
                        }

                        Polygons polygons = new Polygons();
                        polygons.Type = data.Polygon.Type;
                        polygons.Coordinates = listCoordenates;
                       
                        GeoFence.Polygon = polygons;
                    }

                    response.success = true;
                    response.GeoFence = GeoFence;
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

        public GeoFenceDeleteResponse Delete(string hierarchy, string id)
        {
            GeoFenceDeleteResponse response = new GeoFenceDeleteResponse();
            try
            {

                var bsonArray = new BsonArray();
                bsonArray.Add(ObjectId.Parse(id));

                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("_id", new BsonDocument("$in", bsonArray)));
                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence");
                var result = stored.Find(build).ToList();

                //var build = Builders<Models.Mongo.GeoFence.GeoFence>.Filter.Eq(x => x.Id, id);
                //var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence");
                //var result = stored.Find(build).FirstOrDefault();

                if (result != null)
                {
                    try
                    {
                        var buildGeoDevice = Builders<Models.Mongo.GeoFenceDevice.GeoFenceDevice>.Filter.And(
                            Builders<Models.Mongo.GeoFenceDevice.GeoFenceDevice>.Filter.Eq(x => x.IdGeoFence, id));
                        var storedGeoDevice = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFenceDevice.GeoFenceDevice>("GeoFenceDevice");
                        var resultGeoDevice = storedGeoDevice.Find(buildGeoDevice).ToList();

                        //var deleteFilter = Builders<Models.Mongo.GeoFence.GeoFence>.Filter.Eq(x => x.Id, id);
                        //mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFence.GeoFence>("GeoFence").DeleteOne(deleteFilter);
                        Delete(id);

                        //var delete = Builders<Models.Mongo.GeoFenceDevice.GeoFenceDevice>.Filter.Eq(x => x.IdGeoFence, id);
                        //mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.GeoFenceDevice.GeoFenceDevice>("GeoFenceDevice").DeleteMany(delete);


                        if(resultGeoDevice.Count > 0)
                        {
                            if (resultGeoDevice.Count > 0)
                            {
                                foreach (var data in resultGeoDevice)
                                {
                                    var filterConsola = Builders<GeoFenceConsola>.Filter.Eq(x => x.Device, data.Device);
                                    var storedConsola = mongoDBContext.spiderMongoDatabase.GetCollection<GeoFenceConsola>("GeoFenceConsola");
                                    var resultConsola = storedConsola.Find(filterConsola).FirstOrDefault();

                                    if (resultConsola != null)
                                    {
                                        GeoFenceConsola consola = new GeoFenceConsola();
                                        consola.Id = resultConsola.Id;
                                        consola.Device = resultConsola.Device;
                                        List<GeoFenceConsolas> Fences = new List<GeoFenceConsolas>();

                                        foreach (var geo in resultConsola.Fences)
                                        {
                                            if (!geo.Id.Equals(data.IdGeoFence))
                                            {
                                                Fences.Add(geo);
                                            }
                                        }

                                        if (Fences.Count > 0)
                                        {
                                            consola.Fences = Fences;
                                        }
                                        else
                                        {
                                            consola.Fences = null;
                                        }

                                        var filter = Builders<GeoFenceConsola>.Filter.Eq(x => x.Device, data.Device);
                                        mongoDBContext.spiderMongoDatabase.GetCollection<GeoFenceConsola>("GeoFenceConsola").ReplaceOneAsync(filter, consola);
                                    }
                                }
                            }
                        }
                        response.success = true;
                    }
                    catch(Exception ex)
                    {
                        response.success = false;
                        response.messages.Add(ex.Message);
                        return response;
                    }
                }
                else
                {
                    response.success = false;
                    response.messages.Add("No se encontraron resultados con los datos ingresados favor de verificar");
                    return response;
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

        #region SQL
        private int ValidaNombre(string node, string nombre)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_validation_geo_fences_name", cn);
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
                    SqlCommand cmd = new SqlCommand("ad.sp_create_geo_fences", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(nombre)));
                    cmd.Parameters.Add(new SqlParameter("@description", string.IsNullOrEmpty(description)? "": Convert.ToString(description)));
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
                    SqlCommand cmd = new SqlCommand("ad.sp_update_geo_fences", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(nombre)));
                    cmd.Parameters.Add(new SqlParameter("@description", string.IsNullOrEmpty(description) ? "" : Convert.ToString(description)));
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

        public List<string> GetMongoId(string node)
        {
            List<string> list = new List<string>();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_geo_fencces_by_hierarchy", cn);
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
                    SqlCommand cmd = new SqlCommand("ad.sp_delete_geo_fences", cn);
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