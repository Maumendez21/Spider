using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.EngineStop;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo.LogEngineStop;
using SpiderFleetWebAPI.Models.Response.EngineStop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.EngineStop
{
    public class EngineStopDao
    {
        private MongoDBContext mongoDBContext = new MongoDBContext();
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public EngineStopDao() { }

        public EngineStopNumberPagesResponse GetNumberPages(string node, string search)
        {
            EngineStopNumberPagesResponse response = new EngineStopNumberPagesResponse();
            int respuesta = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_total_paginas_vehicle_engine_stop", cn))
                    {
                        search = string.IsNullOrEmpty(search) ? "" : search;

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@node", node));
                        cmd.Parameters.Add(new SqlParameter("@search", search));

                        SqlParameter sqlParameter = new SqlParameter();
                        sqlParameter.ParameterName = "@totalPaginas";
                        sqlParameter.SqlDbType = SqlDbType.Decimal;
                        sqlParameter.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(sqlParameter);
                        cmd.ExecuteNonQuery();

                        respuesta = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(sqlParameter.Value.ToString())).ToString());

                        if (respuesta > 0)
                        {
                            response.success = true;
                            response.NumberPages = respuesta;
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

        public EngineStopResponse GetDeviceEngineStop(string node, string search, int page, int rowsPage)
        {
            var listDevices = new List<CredencialSpiderFleet.Models.EngineStop.EngineStop>();
            EngineStopResponse response = new EngineStopResponse();
            
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_consult_list_vehicle_engine_stop", cn))
                    {
                        search = string.IsNullOrEmpty(search) ? "" : search;

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@node", node));
                        cmd.Parameters.Add(new SqlParameter("@search", search));
                        cmd.Parameters.Add(new SqlParameter("@PageNumber", page));
                        cmd.Parameters.Add(new SqlParameter("@RowsOfPage", rowsPage));

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listDevices.Add(MapToValue(reader));
                            }


                            if (listDevices.Count > 0)
                            {
                                response.success = true;
                                response.ListEngineStops = listDevices;
                            }
                            else
                            {
                                response.success = false;
                            }
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

        public EngineStopRegistryResponse GetEngineStop(string device)
        {
            var infoDevice = new CredencialSpiderFleet.Models.EngineStop.EngineStop();
            EngineStopRegistryResponse response = new EngineStopRegistryResponse();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_consult_status_vehicle_engine_stop", cn))
                    {                        

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@device", device));
                        

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                infoDevice = MapToValue(reader);
                            }

                            response.EngineStop = infoDevice;
                            response.success = true;
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

        private CredencialSpiderFleet.Models.EngineStop.EngineStop MapToValue(SqlDataReader reader)
        {
            return new CredencialSpiderFleet.Models.EngineStop.EngineStop()
            {
                Device = reader["ID_Vehiculo"].ToString(),
                Name = reader["Nombre"].ToString(),
                Status = Convert.ToInt32(reader["Status"].ToString())
            };
        }

        public SendEngineStopResponse ExecuteEngineStop(string node, string user, string device, int status)
        {
            SendEngineStopResponse response = new SendEngineStopResponse();
            int respuesta = 0;

            try
            {

                LogEngineStop log = new LogEngineStop();
                log.Date = (DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");
                log.Node = node;
                log.User = user;
                log.Device = device;
                
                if(status == 0)
                {
                    Create(device, user, response);
                }
                else if(status == 2)
                {
                    Update(device, user, 3, response);
                }

                if(response.messages.Count == 0)
                {
                    Log(log, response);
                    Create(device, response);
                    response.success = true;
                }
                else
                {
                    return response;
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

        private void Create(string device, string user, SendEngineStopResponse response)
        {
            
            int respuesta = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_engine_stop_events", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", device));
                    cmd.Parameters.Add(new SqlParameter("@user", user));

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
                        response.messages.Add("Error al intenar dar de alta el registro");
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
            }
            finally
            {
                cn.Close();
            }
        }

        private void Update(string device, string user, int status, SendEngineStopResponse response)
        {

            int respuesta = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_engine_stop_events", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", device));
                    cmd.Parameters.Add(new SqlParameter("@user", user));
                    cmd.Parameters.Add(new SqlParameter("@status", status));

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
                        response.messages.Add("Error al intenar dar de alta el registro");
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
            }
            finally
            {
                cn.Close();
            }
        }

        private void Create(string device, SendEngineStopResponse response)
        {
            int respuesta = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_engine_stop", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@device", device));
                    cmd.Parameters.Add(new SqlParameter("@date", DateTime.Now));

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
                        response.messages.Add("Error al intenar dar de alta el evento");
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
            }
            finally
            {
                cn.Close();
            }
        }

        #region Mongo
        private void Log(LogEngineStop log, SendEngineStopResponse response)
        {
            try
            {
                mongoDBContext.spiderMongoDatabase.GetCollection<LogEngineStop>("LogEngineStop").InsertOne(log);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
            }
        }

        #endregion

    }
}

