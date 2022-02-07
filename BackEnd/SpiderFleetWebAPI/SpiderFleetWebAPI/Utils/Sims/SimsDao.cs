using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Sims;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Sims
{
    public class SimsDao
    {
        public SimsDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        /// <summary>
        /// Metodo que regresa los sims disponibles 
        /// </summary>
        /// <returns></returns>
        public SimsResponse ReadAvailable()
        {
            SimsResponse response = new SimsResponse();

            try
            {
                response = ReadStatus(1);
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

        public SimsResponse ReadAssigned()
        {
            SimsResponse response = new SimsResponse();

            try
            {
                response = ReadStatus(2);
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

        /// <summary>
        /// Metodo que devuelve sim dependiendo del estatus solicitado
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public SimsResponse ReadStatus(int status)
        {
            SimsResponse response = new SimsResponse();
            List<CredencialSpiderFleet.Models.Sims.Sims> listSims = new List<CredencialSpiderFleet.Models.Sims.Sims>();
            CredencialSpiderFleet.Models.Sims.Sims sims = new CredencialSpiderFleet.Models.Sims.Sims();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_sim_status", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@status", Convert.ToInt32(status)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sims = new CredencialSpiderFleet.Models.Sims.Sims();
                            sims.IdSim = Convert.ToInt32(reader["idSim"]);
                            sims.Sim = Convert.ToString(reader["sim"]);

                            listSims.Add(sims);
                        }
                        reader.Close();
                        response.listSims = listSims;
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