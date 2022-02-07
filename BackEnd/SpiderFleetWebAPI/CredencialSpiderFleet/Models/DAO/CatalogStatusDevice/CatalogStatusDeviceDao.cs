using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Request.CatalogStatusDevice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CredencialSpiderFleet.Models.DAO.CatalogStatusDevice
{
    public class CatalogStatusDeviceDao
    {
        public CatalogStatusDeviceDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        /// <summary>
        /// Creacion de Usurio
        /// </summary>
        public int Create(CatalogStatusDeviceRequest status)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_catalog_status_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(status.Name)));

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
        public int Update(CatalogStatusDeviceRequest status)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_catalog_status_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(status.IdStatus)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(status.Name)));

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
        public List<CatalogStatusDeviceRequest> Read()
        {
            List<CatalogStatusDeviceRequest> listStatus = new List<CatalogStatusDeviceRequest>();
            CatalogStatusDeviceRequest status = new CatalogStatusDeviceRequest();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_catalog_status_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            status = new CatalogStatusDeviceRequest();
                            status.IdStatus = Convert.ToInt32(reader["idStatus"]);
                            status.Name = Convert.ToString(reader["name"]);
                            listStatus.Add(status);
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
            return listStatus;
        }

        /// <summary>
        /// Consulta de Ususario por id
        /// </summary>
        public List<CatalogStatusDeviceRequest> ReadId(int id)
        {
            DataSet dsConsulta = new DataSet();
            List<CatalogStatusDeviceRequest> listStatus = new List<CatalogStatusDeviceRequest>();
            CatalogStatusDeviceRequest status = new CatalogStatusDeviceRequest();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_id_catalog_status_device", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            status = new CatalogStatusDeviceRequest();
                            status.IdStatus = Convert.ToInt32(reader["idStatus"]);
                            status.Name = Convert.ToString(reader["name"]);
                            listStatus.Add(status);
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
            return listStatus;
        }

    }
}