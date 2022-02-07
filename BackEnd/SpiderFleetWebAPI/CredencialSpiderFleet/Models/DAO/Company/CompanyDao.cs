using CredencialSpiderFleet.Models.Connection;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CredencialSpiderFleet.Models.DAO.Company
{
    public class CompanyDao
    {
        public CompanyDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        /// <summary>
        /// Creacion de Usurio
        /// </summary>
        public int Create(Request.Companies.CompanyRequest company)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_company", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new SqlParameter("@image", Convert.ToString(company.Image)));
                    cmd.Parameters.Add(new SqlParameter("@idsuscriptiontype", Convert.ToInt32(company.IdSuscriptionType)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(company.Name)));
                    cmd.Parameters.Add(new SqlParameter("@taxid", Convert.ToString(company.TaxId)));
                    cmd.Parameters.Add(new SqlParameter("@taxname", Convert.ToString(company.TaxName)));
                    cmd.Parameters.Add(new SqlParameter("@address", Convert.ToString(company.Address)));
                    cmd.Parameters.Add(new SqlParameter("@telephone", Convert.ToString(company.Telephone)));
                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(company.Email)));
                    cmd.Parameters.Add(new SqlParameter("@city", Convert.ToString(company.City)));
                    cmd.Parameters.Add(new SqlParameter("@country", Convert.ToString(company.Country)));
                    cmd.Parameters.Add(new SqlParameter("@porcentage", Convert.ToDecimal(company.Porcentage)));
                    cmd.Parameters.Add(new SqlParameter("@hierarchy", company.Hierarchy));

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
        public int Update(Request.Companies.CompanyRequest company)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_company", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(company.IdCompany)));
                    //cmd.Parameters.Add(new SqlParameter("@idimg", Convert.ToInt32(company.IdImg)));
                    //cmd.Parameters.Add(new SqlParameter("@idsuscriptiontype", Convert.ToInt32(company.IdSuscriptionType)));
                    //cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(company.Name)));
                    cmd.Parameters.Add(new SqlParameter("@taxid", Convert.ToString(company.TaxId)));
                    cmd.Parameters.Add(new SqlParameter("@taxname", Convert.ToString(company.TaxName)));
                    cmd.Parameters.Add(new SqlParameter("@address", Convert.ToString(company.Address)));
                    cmd.Parameters.Add(new SqlParameter("@telephone", Convert.ToString(company.Telephone)));
                    cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(company.Email)));
                    cmd.Parameters.Add(new SqlParameter("@city", Convert.ToString(company.City)));
                    cmd.Parameters.Add(new SqlParameter("@country", Convert.ToString(company.Country)));
                    //cmd.Parameters.Add(new SqlParameter("@porcentage", Convert.ToDecimal(company.Porcentage)));
                    //cmd.Parameters.Add(new SqlParameter("@hierarchy", company.Hierarchy));

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
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_company", cn);
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
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_id_company", cn);
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

        /// <summary>
        /// Eliminacion de Usuario por id, cambio de estatus a 0
        /// </summary>
        public int Delete(int id)
        {
            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("sp_ad_delete_user", cn);
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


        public DataSet ReadTaxId(string taxid)
        {
            DataSet dsConsulta = new DataSet();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_taxid_company", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@taxid", Convert.ToString(taxid)));
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