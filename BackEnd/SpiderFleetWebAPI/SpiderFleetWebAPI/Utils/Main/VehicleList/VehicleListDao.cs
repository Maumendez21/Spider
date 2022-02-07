using CredencialSpiderFleet.Models.Connection;
using MongoDB.Driver;
using SpiderFleetWebAPI.Models;
using SpiderFleetWebAPI.Models.Mongo;
using SpiderFleetWebAPI.Models.Response.Main.VehicleList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SpiderFleetWebAPI.Utils.Main.VehicleList
{
    public class VehicleListDao
    {
        public VehicleListDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private MongoDBContext mongoDBContext = new MongoDBContext();
        private const string LOGIN = "Login";
        private const string GPS = "GPS";
        private const string ALARM = "Alarm";

        public VehicleListResponse ReadVehicleList(string idempresa)
        {
            VehicleListResponse response = new VehicleListResponse();
            CredencialSpiderFleet.Models.Main.VehicleList.VehicleList vehiculos = new CredencialSpiderFleet.Models.Main.VehicleList.VehicleList();
            List<CredencialSpiderFleet.Models.Main.VehicleList.VehicleList> listVehiculos = new List<CredencialSpiderFleet.Models.Main.VehicleList.VehicleList>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_vehiculos_empresas", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@idempresa", Convert.ToString(idempresa)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime hoy = DateTime.Now;
                            string evento = string.Empty;
                            vehiculos = new CredencialSpiderFleet.Models.Main.VehicleList.VehicleList();
                            vehiculos.Device = Convert.ToString(reader["ID_Vehiculo"]);
                            vehiculos.Name = Convert.ToString(reader["Nombre"]);
                            vehiculos.CompanyId = Convert.ToString(reader["ID_Empresa"]);
                            vehiculos.Bearing = 0;// Convert.ToString(reader["ID_Empresa"]);
                            vehiculos.LastDate = Convert.ToDateTime(reader["date"]);
                            vehiculos.Latitude = Convert.ToString(reader["latitude"]);
                            vehiculos.Longitude = Convert.ToString(reader["longitude"]);
                            evento = Convert.ToString(reader["event"]);

                            //int days = diffTime.Days;//int hours = diffTime.Hours;//int seconds = diffTime.Seconds;
                            TimeSpan diffTime = hoy - vehiculos.LastDate;
                            int minutes = diffTime.Minutes;
                            

                            if (minutes < 5)
                            {                                
                                if (evento.Equals(GPS))
                                {
                                    vehiculos.Status = 1;
                                }
                                else
                                {
                                    vehiculos.Status = 0;
                                }
                            }
                            else if (minutes > 5)
                            {
                                if (minutes > 5 & minutes < 1440)
                                {
                                    vehiculos.Status = 2;
                                }
                                else if (minutes > 1440)
                                {
                                    vehiculos.Status = 3;
                                }                                    
                            }

                            listVehiculos.Add(vehiculos);
                        }
                        reader.Close();
                        response.listVehiculos = listVehiculos;
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