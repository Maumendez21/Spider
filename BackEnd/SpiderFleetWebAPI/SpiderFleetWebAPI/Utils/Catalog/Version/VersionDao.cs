using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Catalog.Version;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Catalog.Version
{
    public class VersionDao
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public VersionDataResponse CreateVersion(CredencialSpiderFleet.Models.Catalogs.Version.Version version)
        {
            VersionDataResponse response = new VersionDataResponse();
            int respuesta = 0;

            try
            {
                if (string.IsNullOrEmpty(version.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese la Versión");
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
                    SqlCommand cmd = new SqlCommand("ad.sp_create_version", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(version.Description)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    try
                    {
                        if (respuesta == 3)
                        {
                            response.success = false;
                            response.messages.Add("Error al intenar dar de alta el registro");
                            return response;
                        }
                        else
                        {
                            response.success = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.success = false;
                        response.messages.Add(ex.Message);
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

        public VersionListResponse Read()
        {
            VersionListResponse response = new VersionListResponse();
            List<CredencialSpiderFleet.Models.Catalogs.Version.Version> ListVersion = new List<CredencialSpiderFleet.Models.Catalogs.Version.Version>();
            CredencialSpiderFleet.Models.Catalogs.Version.Version version = new CredencialSpiderFleet.Models.Catalogs.Version.Version();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_versiones_vehiculos", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            version = new CredencialSpiderFleet.Models.Catalogs.Version.Version();
                            version.IdVersion = Convert.ToInt16(reader["IdVersion"]);
                            version.Description = Convert.ToString(reader["Description"]);
                            ListVersion.Add(version);
                        }
                        reader.Close();
                        response.success = true;
                        response.ListVersions = ListVersion;
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