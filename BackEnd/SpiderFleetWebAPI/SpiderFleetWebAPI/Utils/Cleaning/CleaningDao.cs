using CredencialSpiderFleet.Models.Connection;
using MongoDB.Bson;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.GPS;
using SpiderFleetWebAPI.Models.Response.Cleaning;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Cleaning
{
    public class CleaningDao
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private MongoDBContext mongoDBContext = new MongoDBContext();

        #region SQL
        public CleaningResponse CleaningSqlGPS()
        {
            CleaningResponse response = new CleaningResponse();
            try
            {
                var listCompany = GetListCleaning();

                foreach (var item in listCompany)
                {
                    CleaningGps(item.Device, (item.DayRange + item.PreviousDay));
                }

                response.success = true;
                return response;
            }
            catch(Exception ex)
            {
                response.messages.Add(ex.Message);
                return response;
            }            
        }

        public CleaningResponse CleaningSqlAlarmas()
        {
            CleaningResponse response = new CleaningResponse();
            try
            {
                var listCompany = GetListCleaning();

                foreach (var item in listCompany)
                {
                    CleaningAlarmas(item.Device, (item.DayRange + item.PreviousDay));
                }

                response.success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.messages.Add(ex.Message);
                return response;
            }
        }

        private CredencialSpiderFleet.Models.Cleaning.Cleaning MapToValue(SqlDataReader reader)
        {
            return new CredencialSpiderFleet.Models.Cleaning.Cleaning()
            {
                Device = reader["device"].ToString(),
                DayRange = Convert.ToInt32(reader["day_range"].ToString()),
                PreviousDay = Convert.ToInt32(reader["previous_day"].ToString()),
            };
        }

        private List<CredencialSpiderFleet.Models.Cleaning.Cleaning> GetListCleaning()
        {
            var list = new List<CredencialSpiderFleet.Models.Cleaning.Cleaning>();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_consult_cleaning", cn))
                    {
                        cn = sql.Connection();
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(MapToValue(reader));
                            }
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CleaningGps(string device, int dias)
        {
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_cleaning_gps", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@dias", dias));
                    cmd.Parameters.Add(new SqlParameter("@device", device));
                    cmd.ExecuteNonQuery();
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
        }

        private void CleaningAlarmas(string device, int dias)
        {
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_cleaning_alarmas", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@dias", dias));
                    cmd.Parameters.Add(new SqlParameter("@device", device));
                    cmd.ExecuteNonQuery();
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
        }

        #endregion

        #region Mongo
        public CleaningResponse CleaningMongoGPS()
        {
            CleaningResponse response = new CleaningResponse();
            try
            {
                var listCompany = GetListCleaning();

                foreach (var item in listCompany)
                {
                    DateTime now = DateTime.Now;
                    DateTime start = DateTime.Now;
                    DateTime end = DateTime.Now;
                    int dias = (item.DayRange + item.PreviousDay);

                    start = start.AddDays(-dias);
                    end = end.AddDays(-dias);
                    start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
                    end = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);

                    MongoGps(item.Device ,start ,end );
                }
                response.success = true;
                return response;
            } 
            catch(Exception ex)
            {
                response.messages.Add(ex.Message);
                return response;
            }
        }

        public CleaningResponse CleaningMongoAlarmas()
        {
            CleaningResponse response = new CleaningResponse();
            try
            {
                var listCompany = GetListCleaning();

                foreach (var item in listCompany)
                {
                    DateTime now = DateTime.Now;
                    DateTime start = DateTime.Now;
                    DateTime end = DateTime.Now;
                    int dias = (item.DayRange + item.PreviousDay);

                    start = start.AddDays(-dias);
                    end = end.AddDays(-dias);
                    start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
                    end = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);

                    MongoAlarms(item.Device, start, end);
                }
                response.success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.messages.Add(ex.Message);
                return response;
            }
        }

        private void MongoGps(string device, DateTime startdate, DateTime enddate)
        {
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

                if(result.Count > 0)
                {
                    stored.DeleteMany(build);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void MongoAlarms(string device, DateTime startdate, DateTime enddate)
        {
            try
            {
                string start = startdate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                string end = enddate.ToString("yyyy-MM-ddTHH:mm:ssZ");

                BsonDocument bsonDocument = new BsonDocument();
                bsonDocument.Add("device", device);
                bsonDocument.Add("date", new BsonDocument("$gte", Convert.ToDateTime(start)).Add("$lte", Convert.ToDateTime(end)));

                var build = bsonDocument;
                var stored = mongoDBContext.spiderMongoDatabase.GetCollection<Models.Mongo.Alarms.Alarms>("Alarms");
                var result = stored.Find(build).Sort("{date:1}").ToList();

                if (result.Count > 0)
                {
                    stored.DeleteMany(build);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}