using CredencialSpiderFleet.Models.Connection;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CredencialSpiderFleet.Models.DAO.SuscriptionsType
{
    public class SuscriptionTypeDao
    {
        public SuscriptionTypeDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        /// <summary>
        /// Creacion de Usurio
        /// </summary>
        public int Create(Request.SuscriptionsType.SuscriptionTypeRequest suscriptions)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_suscriptionstype", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(suscriptions.Description)));

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
        public int Update(Request.SuscriptionsType.SuscriptionTypeRequest suscriptions)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_suscriptionstype", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(suscriptions.IdSuscriptionType)));
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(suscriptions.Description)));

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
        public DataSet Read()
        {
            DataSet dsConsulta = new DataSet();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_suscriptionstype", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter sqlData = new SqlDataAdapter(cmd);
                    sqlData.Fill(dsConsulta);
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
            return dsConsulta;
        }

        /// <summary>
        /// Consulta de Ususario por id
        /// </summary>
        public DataSet ReadId(int id)
        {
            DataSet dsConsulta = new DataSet();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_id_suscriptionstype", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter sqlData = new SqlDataAdapter(cmd);
                    sqlData.Fill(dsConsulta);
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
            return dsConsulta;
        }

    }
}