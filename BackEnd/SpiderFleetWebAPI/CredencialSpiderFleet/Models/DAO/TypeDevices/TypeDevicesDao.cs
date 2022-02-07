using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Request.TypeDevices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CredencialSpiderFleet.Models.DAO.TypeDevices
{
    public class TypeDevicesDao
    {
        public TypeDevicesDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        /// <summary>
        /// Creacion de Usurio
        /// </summary>
        public int Create(TypeDevicesRequest sims)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_type_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(sims.Name)));
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(sims.Description)));                   

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
        public int Update(TypeDevicesRequest sims)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_type_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(sims.idTypeDevice)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(sims.Name)));
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(sims.Description)));

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
        public List<TypeDevicesRequest> Read()
        {
            DataSet dsConsulta = new DataSet();
            List<TypeDevicesRequest> listSims = new List<TypeDevicesRequest>();
            TypeDevicesRequest sims = new TypeDevicesRequest();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_type_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sims = new TypeDevicesRequest();
                            sims.idTypeDevice = Convert.ToInt32(reader["idTypeDevice"]);
                            sims.Name = Convert.ToString(reader["name"]);
                            sims.Description = Convert.ToString(reader["description"]);
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
        public List<TypeDevicesRequest> ReadId(int id)
        {
            DataSet dsConsulta = new DataSet();
            List<TypeDevicesRequest> listSims = new List<TypeDevicesRequest>();
            TypeDevicesRequest sims = new TypeDevicesRequest();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_id_type_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sims = new TypeDevicesRequest();
                            sims.idTypeDevice = Convert.ToInt32(reader["idTypeDevice"]);
                            sims.Name = Convert.ToString(reader["name"]);
                            sims.Description = Convert.ToString(reader["description"]);
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

    }
}