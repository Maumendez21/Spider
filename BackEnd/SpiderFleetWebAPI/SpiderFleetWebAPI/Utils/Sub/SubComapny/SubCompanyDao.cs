using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Sub.SubComapny;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Sub.SubComapny
{
    public class SubCompanyDao
    {
        public SubCompanyDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        /// <summary>
        /// Creacion de Sub Empresas
        /// </summary>
        public SubCompanyResponse Create(CredencialSpiderFleet.Models.Sub.SubComapny.SubCompany subCompany)
        {
            SubCompanyResponse response = new SubCompanyResponse();
            int respuesta = 0;
            
            try
            {
                if (string.IsNullOrEmpty(subCompany.NameSubCompany.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Nombre de la SubCompañia");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_subcompany", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(subCompany.UserName)));
                    cmd.Parameters.Add(new SqlParameter("@idfather", Convert.ToString(subCompany.IdFather)));
                    cmd.Parameters.Add(new SqlParameter("@nameSubCompany", Convert.ToString(subCompany.NameSubCompany)));

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
                        return response;
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
                return response;
            }
            finally
            {
                cn.Close();
            }
            return response;
        }

        /// <summary>
        /// Actualizacion de Usuario
        /// </summary>
        public SubCompanyResponse Update(CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyUpdate subCompany)
        {
            SubCompanyResponse response = new SubCompanyResponse();
            int respuesta = 0;

            try
            {
                if (string.IsNullOrEmpty(subCompany.IdSubCompany))
                {
                    response.success = false;
                    response.messages.Add("No tiene el Id Sub Compañia");
                    return response;
                }

                if (string.IsNullOrEmpty(subCompany.NameSubCompany.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Nombre de la Sub Compañia");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_subcompany", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(subCompany.IdSubCompany)));
                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(subCompany.NameSubCompany)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    if (respuesta == 1)
                    {
                        response.success = true;
                    }
                    else if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("No se encuentra el registro");
                        return response;
                    }
                    else if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Error al tratar de actualizar el registro");
                        return response;
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

        /// <summary>
        /// Consulta de Usuarios con estatus 1
        /// </summary>
        public SubCompanyListResponse Read(string username)
        {
            List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData> listSubCompany = new List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData>();
            CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData subCompany = new CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData();
            SubCompanyListResponse response = new SubCompanyListResponse();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_subcompany", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subCompany = new CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData();
                            // subCompany.IdSubCompany = Convert.ToInt32(reader["idSubCompany"]);
                            subCompany.Name = Convert.ToString(reader["Nombre"]);
                            subCompany.Hierarchy = Convert.ToString(reader["hierarchy"]);
                            listSubCompany.Add(subCompany);
                        }
                        reader.Close();
                        response.success = true;
                        response.listSubCompany = listSubCompany;
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

        /// <summary>
        /// Consulta de Ususario por id
        /// </summary>
        public SubCompanyRegistryResponse ReadId(string id)
        {
            CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData subCompany = new CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData();
            SubCompanyRegistryResponse response = new SubCompanyRegistryResponse();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_id_subcompany", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subCompany = new CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData();
                            //subCompany.IdSubCompany = Convert.ToInt32(reader["idSubCompany"]);
                            subCompany.Name = Convert.ToString(reader["Nombre"]);
                            subCompany.Hierarchy = Convert.ToString(reader["hierarchy"]);
                        }
                        reader.Close();
                        response.success = false;
                        response.subCompany = subCompany;
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

        /// <summary>
        /// Metodo que regresa los registros de acceso de jerarquias atravez del username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public SubCompanyListResponse ReadHierarchy(string username)
        {
            List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData> listSubCompany = new List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData>();
            CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData subCompany = new CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData();
            SubCompanyListResponse response = new SubCompanyListResponse();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_username_hierarchy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subCompany = new CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData();
                            //subCompany.IdSubCompany = Convert.ToInt32(reader["idSubCompany"]);
                            subCompany.Name = Convert.ToString(reader["name"]);
                            subCompany.Hierarchy = Convert.ToString(reader["hierarchy"]);
                            listSubCompany.Add(subCompany);
                        }

                        response.success = true;
                        response.listSubCompany = listSubCompany;
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