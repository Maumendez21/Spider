using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using CredencialSpiderFleet.Models.PointInterest;
using CredencialSpiderFleet.Models.Useful;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.PointInterest;
using SpiderFleetWebAPI.Models.Response.PointsInterest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.PointInterest
{
    public class PointInterestDao
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private MongoDBContext mongoDBContext = new MongoDBContext();
        private UseFul use = new UseFul();
        private const int longitudName = 50;

        public PointInterestDao() { }

        #region SQL Consultas

        private string IsExistsName(string node, string name)
        {
            string count = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_name_points_interest_by_hierarchy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(name)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count = Convert.ToString(reader["item"]);
                        }
                        reader.Close();
                    }
                }
                else
                {
                    count = string.Empty;
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                cn.Close();
            }
            return count;
        }

        private bool IsExistsId(string mongo)
        {
            bool result = false;
            int count = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_point_interest_by_id", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mongo", Convert.ToString(mongo)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count = Convert.ToInt32(reader["result"]);
                        }
                        reader.Close();
                    }
                }

                result = count == 0 ? false : true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cn.Close();
            }
            return result;
        }

        private int Create(PointInterestCreate point)
        {
            int respuesta = 0;
            try
            {

                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_points_interest", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(point.Node)));
                    cmd.Parameters.Add(new SqlParameter("@mongo", Convert.ToString(point.Id)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(point.Name)));
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(point.Description)));

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
            }
            finally
            {
                cn.Close();
            }
            return respuesta;
        }

        public List<string> ListPointsByHierarchy(string node)
        {
            List<string> listPoints = new List<string>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_point_interest_by_hierarchy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listPoints.Add(Convert.ToString(reader["MongoId"]));
                        }
                        reader.Close();
                    }
                }
                else
                {
                    listPoints = new List<string>();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cn.Close();
            }
            return listPoints;
        }

        public PointInterestListRegistryResponse ListPointsByHierarchyService(string node)
        {
            PointInterestListRegistryResponse response = new PointInterestListRegistryResponse();
            List<PointInterestRegistry> listPoints = new List<PointInterestRegistry>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_point_interest_by_hierarchy_service", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PointInterestRegistry registry = new PointInterestRegistry();
                            registry.MongoId = reader["MongoId"].ToString();
                            registry.Description = reader["Name"].ToString();

                            listPoints.Add(registry);
                        }
                        reader.Close();
                        response.ListPoints = listPoints;
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

        private bool DeleteSql(string mongo)
        {
            bool result = false;
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_delete_point_interest_id", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(mongo)));
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

            }
            finally
            {
                cn.Close();
            }
            return result;
        }

        #endregion

        #region Mongo CRUD

        public PointsInterestResponse Create(PointsInterest points, string node)
        {
            PointsInterestResponse response = new PointsInterestResponse();

            if (string.IsNullOrEmpty(points.Name))
            {
                response.success = false;
                response.messages.Add("El campo nombre no puede estar vacio favor de verificar");
                return response;
            }

            if (!string.IsNullOrEmpty(points.Name.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(points.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(points.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Nombre excede de lo establecido rango maximo " + longitudName + " caracteres");
                    return response;
                }
            }

            try
            {
                string count = IsExistsName(node, points.Name);
                if (string.IsNullOrEmpty(count))
                {
                    mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterest>("PointsInterest").InsertOne(points);
                    response.success = true;

                    PointInterestCreate point = new PointInterestCreate();
                    point.Id = points.Id;
                    point.Name = points.Name;
                    point.Description = points.Description;
                    point.Node = node;

                    Create(point);
                    
                }
                else
                {
                    response.success = false;
                    response.messages.Add("El nombre de la GeoCerca ya se encuentra registrado, trate con un nombre diferente.");
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

        public void Create(PointsInterest points, string node, out string id, PointsInterestResponse response)
        {
            id = string.Empty;

            if (string.IsNullOrEmpty(points.Name))
            {
                response.success = false;
                response.messages.Add("El campo nombre no puede estar vacio favor de verificar");
            }

            if (!string.IsNullOrEmpty(points.Name.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(points.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                }

                if (!use.IsValidLength(points.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Nombre excede de lo establecido rango maximo " + longitudName + " caracteres");
                }
            }

            try
            {
                string count = IsExistsName(node, points.Name);
                if (string.IsNullOrEmpty(count))
                {
                    mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterest>("PointsInterest").InsertOne(points);
                    response.success = true;

                    PointInterestCreate point = new PointInterestCreate();
                    point.Id = points.Id;
                    point.Name = points.Name;
                    point.Description = points.Description;
                    point.Node = node;

                    Create(point);

                    id = points.Id;
                }
                else
                {
                    response.success = false;
                    response.messages.Add("El nombre de la GeoCerca ya se encuentra registrado, trate con un nombre diferente.");
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
            }

        }


        public PointsInterestResponse Update(PointsInterest point, string node)
        {
            PointsInterest geoFences = new PointsInterest();
            PointsInterestResponse response = new PointsInterestResponse();

            if (string.IsNullOrEmpty(point.Name))
            {
                response.success = false;
                response.messages.Add("El campo nombre no puede estar vacio favor de verificar");
                return response;
            }

            if (!string.IsNullOrEmpty(point.Name.Trim()))
            {
                use = new UseFul();

                if (use.hasSpecialChar(point.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!use.IsValidLength(point.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Nombre excede de lo establecido rango maximo " + longitudName + " caracteres");
                    return response;
                }
            }

            if (!IsExistsId(point.Id))
            {
                response.success = false;
                response.messages.Add("El registro que intenta actualizar no existe ,verifique la información");
                return response;
            }

            try
            {
                var build = Builders<PointsInterest>.Filter.Eq(x => x.Name, point.Name);
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterest>("PointsInterest");
                var result = stored.Find(build).FirstOrDefault();

                if (result != null)
                {
                    if (point.Id.Equals(result.Id))
                    {

                        //Actualizacion de Tabla Principal
                        var update = Builders<PointsInterest>.Update
                        .Set("name", point.Name)
                        //.Set("hierarchy", node)
                        .Set("active", point.Active)
                        .Set("description", point.Description)                        
                        .Set("interestPoints", point.InterestPoint);

                        var filter = Builders<PointsInterest>.Filter.Eq(u => u.Id, point.Id);
                        mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterest>("PointsInterest").UpdateOne(filter, update);

                        response.success = true;

                        #region Consolas

                        var buildGeoDevice = Builders<PointsInterestDevice>.Filter.And(
                            Builders<PointsInterestDevice>.Filter.Eq(x => x.IdPoint, point.Id)
                        );
                        var storedGeoDevice = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestDevice>("PointsInterestDevice");
                        var resultEquals = storedGeoDevice.Find(buildGeoDevice).ToList();

                        if (resultEquals.Count > 0)
                        {
                            foreach (var data in resultEquals)
                            {
                                var filterConsola = Builders<PointsInterestConsola>.Filter.Eq(x => x.Device, data.Device);
                                var storedConsola = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestConsola>("PointsInterestConsola");
                                var resultConsola = storedConsola.Find(filterConsola).FirstOrDefault();

                                if (resultConsola != null)
                                {
                                    if (resultConsola.PointsInterest != null)
                                    {
                                        foreach (var z in resultConsola.PointsInterest)
                                        {
                                            if (z.Id.Equals(point.Id))
                                            {
                                                z.Name = point.Name;
                                                z.InterestPoint = point.InterestPoint;
                                                z.Active = point.Active;
                                                z.Description = point.Description;

                                                mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestConsola>("PointsInterestConsola").ReplaceOneAsync(filterConsola, resultConsola);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        PointInterestConsola geo = new PointInterestConsola();
                                        geo.Id = point.Id;
                                        geo.Name = point.Name;
                                        geo.InterestPoint = point.InterestPoint;
                                        geo.Active = point.Active;
                                        geo.Description = point.Description;
                                        resultConsola.PointsInterest = new List<PointInterestConsola>();
                                        resultConsola.PointsInterest.Add(geo);
                                        mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestConsola>("PointsInterestConsola").ReplaceOneAsync(filterConsola, resultConsola);
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        response.success = false;
                        response.messages.Add("El nombre del Punto de Interes  ya se encuentra registrado, trate con un nombre diferente.");
                        return response;
                    }

                }
                else
                {
                    var update = Builders<PointsInterest>.Update
                    .Set("name", point.Name)
                    //.Set("hierarchy", point.Hierarchy)
                    .Set("active", point.Active)
                    .Set("description", point.Description)
                    .Set("interestPoints", point.InterestPoint);

                    var filter = Builders<PointsInterest>.Filter.Eq(u => u.Id, point.Id);
                    mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterest>("PointsInterest").UpdateOne(filter, update);
                    response.success = true;

                    #region Consolas

                    var buildGeoDevice = Builders<PointsInterestDevice>.Filter.And(
                        Builders<PointsInterestDevice>.Filter.Eq(x => x.IdPoint, point.Id)
                    );
                    var storedGeoDevice = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestDevice>("PointsInterestDevice");
                    var resultEquals = storedGeoDevice.Find(buildGeoDevice).ToList();

                    if (resultEquals.Count > 0)
                    {
                        foreach (var data in resultEquals)
                        {
                            var filterConsola = Builders<PointsInterestConsola>.Filter.Eq(x => x.Device, data.Device);
                            var storedConsola = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestConsola>("PointsInterestConsola");
                            var resultConsola = storedConsola.Find(filterConsola).FirstOrDefault();

                            if (resultConsola != null)
                            {
                                foreach (var z in resultConsola.PointsInterest)
                                {
                                    if (z.Id.Equals(point.Id))
                                    {
                                        z.Name = point.Name;
                                        z.InterestPoint = point.InterestPoint;
                                        z.Active = point.Active;
                                        z.Description = point.Description;

                                        mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestConsola>("PointsInterestConsola").ReplaceOneAsync(filterConsola, resultConsola);
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

        public PointInerestListResponse Read(string hierarchy)
        {
            List<PointInterestData> listPointInterest = new List<PointInterestData>();
            PointInerestListResponse response = new PointInerestListResponse();

            try
            {
                List<string> ListPoint = new List<string>();
                ListPoint = ListPointsByHierarchy(hierarchy);

                var bsonArray = new BsonArray();
                foreach (var id in ListPoint)
                {
                    bsonArray.Add(ObjectId.Parse(id));
                }

                BsonDocument bsonDocument = new BsonDocument(new BsonDocument("_id", new BsonDocument("$in", bsonArray)));
                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterest>("PointsInterest");
                var result = stored.Find(build).ToList();

                if (result == null)
                {

                    response.ListPointInterest = listPointInterest;
                }
                else
                {
                    foreach (PointsInterest data in result)
                    {
                        List<CoordenatesData> listCoordenates = new List<CoordenatesData>();
                        PointInterestData point = new PointInterestData();
                        point.Id = data.Id;
                        point.Name = data.Name;
                        point.Hierarchy = data.Hierarchy;
                        point.Active = data.Active;
                        point.Description = data.Description;
                        point.Latitude = data.InterestPoint.Coordinate[1].ToString();
                        point.Longitude = data.InterestPoint.Coordinate[0].ToString();
                        point.Radius = data.InterestPoint.Radius;

                        listPointInterest.Add(point);

                    }

                    response.success = true;
                    response.ListPointInterest = listPointInterest;
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

        public PointInterestRegistryResponse ReadId(string id)
        {
            PointInterestData point = new PointInterestData();
            PointInterestRegistryResponse response = new PointInterestRegistryResponse();
            try
            {

                var build = Builders<PointsInterest>.Filter.Eq(x => x.Id, id);
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterest>("PointsInterest");
                var result = stored.Find(build).ToList();

                if (result == null)
                {
                    response.PointInterest = point;
                }
                else
                {
                    foreach (PointsInterest data in result)
                    {
                        List<CoordenatesData> listCoordenates = new List<CoordenatesData>();
                        point.Id = data.Id;
                        point.Name = data.Name;
                        point.Hierarchy = data.Hierarchy;
                        point.Active = data.Active;
                        point.Description = data.Description;
                        point.Latitude = data.InterestPoint.Coordinate[1].ToString();
                        point.Longitude = data.InterestPoint.Coordinate[0].ToString();
                        point.Radius = data.InterestPoint.Radius;
                    }

                    response.success = true;
                    response.PointInterest = point;
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

        public PointInterestDeleteResponse Delete(string id)
        {
            PointInterestDeleteResponse response = new PointInterestDeleteResponse();
            try
            {
                var build = Builders<PointsInterest>.Filter.Eq(x => x.Id, id);
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterest>("PointsInterest");
                var result = stored.Find(build).FirstOrDefault();

                if (result != null)
                {
                    try
                    {
                        var buildGeoDevice = Builders<PointsInterestDevice>.Filter.And(
                            Builders<PointsInterestDevice>.Filter.Eq(x => x.IdPoint, id));
                        var storedGeoDevice = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestDevice>("PointsInterestDevice");
                        var resultGeoDevice = storedGeoDevice.Find(buildGeoDevice).ToList();

                        var deleteFilter = Builders<PointsInterestDevice>.Filter.Eq(x => x.Id, id);
                        mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestDevice>("PointsInterest").DeleteOne(deleteFilter);
                        var delete = Builders<PointsInterestDevice>.Filter.Eq(x => x.IdPoint, id);
                        mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestDevice>("PointsInterestDevice").DeleteMany(delete);

                        if (resultGeoDevice.Count > 0)
                        {
                            if (resultGeoDevice.Count > 0)
                            {
                                foreach (var data in resultGeoDevice)
                                {
                                    var filterConsola = Builders<PointsInterestConsola>.Filter.Eq(x => x.Device, data.Device);
                                    var storedConsola = mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestConsola>("PointsInterestConsola");
                                    var resultConsola = storedConsola.Find(filterConsola).FirstOrDefault();

                                    if (resultConsola != null)
                                    {
                                        PointsInterestConsola consola = new PointsInterestConsola();
                                        consola.Id = resultConsola.Id;
                                        consola.Device = resultConsola.Device;
                                        List<PointInterestConsola> PointInterest = new List<PointInterestConsola>();

                                        foreach (var geo in resultConsola.PointsInterest)
                                        {
                                            if (!geo.Id.Equals(data.IdPoint))
                                            {
                                                PointInterest.Add(geo);
                                            }
                                        }

                                        if (PointInterest.Count > 0)
                                        {
                                            consola.PointsInterest = PointInterest;
                                        }
                                        else
                                        {
                                            consola.PointsInterest = new List<PointInterestConsola>();
                                        }

                                        var filter = Builders<PointsInterestConsola>.Filter.Eq(x => x.Device, data.Device);
                                        mongoDBContext.spiderMongoDatabase.GetCollection<PointsInterestConsola>("PointsInterestConsola").ReplaceOneAsync(filter, consola);
                                    }
                                }
                            }
                        }
                        response.success = true;

                        DeleteSql(id);
                    }
                    catch (Exception ex)
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

        #endregion
    }
}