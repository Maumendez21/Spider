using CredencialSpiderFleet.Models.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Inventory.Sim
{
    public class SimDao
    {
        public SimDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        /// <summary>
        /// Creacion de Usurio
        /// </summary>
        public int Create(CredencialSpiderFleet.Models.Inventory.Sim.Sims sims)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_sim", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@sim", Convert.ToString(sims.Sim)));
                    cmd.Parameters.Add(new SqlParameter("@status", Convert.ToInt32(sims.Status)));
                    if (sims.LastUploadDate != null)
                    {
                        cmd.Parameters.Add(new SqlParameter("@lastUploadDate", sims.LastUploadDate));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@lastUploadDate", DBNull.Value));
                    }

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

        /// <summary>
        /// Actualizacion de Usuario
        /// </summary>
        public int Update(CredencialSpiderFleet.Models.Inventory.Sim.Sims sims)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_sim", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(sims.IdSim)));
                    cmd.Parameters.Add(new SqlParameter("@sim", Convert.ToString(sims.Sim)));
                    //cmd.Parameters.Add(new SqlParameter("@status", Convert.ToInt32(sims.Status)));
                    //cmd.Parameters.Add(new SqlParameter("@lastUploadDate", Convert.ToDateTime(sims.LastUploadDate)));

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

        /// <summary>
        /// Consulta de Usuarios con estatus 1
        /// </summary>
        public List<CredencialSpiderFleet.Models.Inventory.Sim.Sims> Read()
        {
            DataSet dsConsulta = new DataSet();
            List<CredencialSpiderFleet.Models.Inventory.Sim.Sims> listSims = new List<CredencialSpiderFleet.Models.Inventory.Sim.Sims>();
            CredencialSpiderFleet.Models.Inventory.Sim.Sims sims = new CredencialSpiderFleet.Models.Inventory.Sim.Sims();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_sim", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sims = new CredencialSpiderFleet.Models.Inventory.Sim.Sims();
                            sims.IdSim = Convert.ToInt32(reader["idSim"]);
                            sims.Sim = Convert.ToString(reader["sim"]);
                            sims.Status = Convert.ToInt32(reader["status"]);
                            sims.LastUploadDate = (reader["lastUploadDate"] == DBNull.Value) ? (DateTime?)null : ((DateTime)reader["lastUploadDate"]); // (DateTime?) reader["lastUploadDate"];
                            listSims.Add(sims);
                        }
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
            return listSims;
        }

        /// <summary>
        /// Consulta de Ususario por id
        /// </summary>
        public CredencialSpiderFleet.Models.Inventory.Sim.Sims ReadId(int id)
        {
            DataSet dsConsulta = new DataSet();
            CredencialSpiderFleet.Models.Inventory.Sim.Sims sims = new CredencialSpiderFleet.Models.Inventory.Sim.Sims();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_id_sim", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sims = new CredencialSpiderFleet.Models.Inventory.Sim.Sims();
                            sims.IdSim = Convert.ToInt32(reader["idSim"]);
                            sims.Sim = Convert.ToString(reader["sim"]);
                            sims.Status = Convert.ToInt32(reader["status"]);
                            sims.LastUploadDate = (reader["lastUploadDate"] == DBNull.Value) ? (DateTime?)null : ((DateTime)reader["lastUploadDate"]);
                        }
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
            return sims;
        }

        /// <summary>
        /// SoftDelete de Ususario por id
        /// </summary>
        public int DeleteId(int id)
        {
            int respuesta = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_delete_id_sim", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));

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

    }
}