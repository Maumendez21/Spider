using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Catalog.SuscriptionsType;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Catalog.SuscriptionsType
{
    public class SuscriptionsTypeDao
    {
        public SuscriptionsTypeDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();
        private const int longitudDescription = 50;

        /// <summary>
        /// Creacion de Usurio
        /// </summary>
        public SuscriptionsTypeResponse Create(CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType suscriptions)
        {
            SuscriptionsTypeResponse response = new SuscriptionsTypeResponse();
            int respuesta = 0;

            try
            {

                if (string.IsNullOrEmpty(suscriptions.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese la Descripción");
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            if (!string.IsNullOrEmpty(suscriptions.Description.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(suscriptions.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(suscriptions.Description.Trim(), longitudDescription))
                {
                    response.success = false;
                    response.messages.Add("La longitud excede de lo establecido rango maximo " + longitudDescription + " caracteres");
                    return response;
                }
            }

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
        public SuscriptionsTypeResponse Update(CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType suscriptions)
        {
            SuscriptionsTypeResponse response = new SuscriptionsTypeResponse();
            int respuesta = 0;

            try
            {
                if (suscriptions.IdSuscriptionType == 0)
                {
                    response.success = false;
                    response.messages.Add("No tiene el parametro idSuscriptionType");
                    return response;
                }

                if (string.IsNullOrEmpty(suscriptions.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese la Descripción");
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            if (!string.IsNullOrEmpty(suscriptions.Description.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(suscriptions.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(suscriptions.Description.Trim(), longitudDescription))
                {
                    response.success = false;
                    response.messages.Add("La longitud excede de lo establecido rango maximo " + longitudDescription + " caracteres");
                    return response;
                }
            }

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
        public SuscriptionsTypeListResponse Read()
        {
            SuscriptionsTypeListResponse response = new SuscriptionsTypeListResponse();
            List<CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType> listSuscriptionType = new List<CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType>();
            CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType type = new CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_suscriptionstype", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            type = new CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType();
                            type.IdSuscriptionType = Convert.ToInt32(reader["idSuscriptionType"]);
                            type.Description = Convert.ToString(reader["description"]);
                            listSuscriptionType.Add(type);
                        }
                        reader.Close();
                        response.success = true;
                        response.listType = listSuscriptionType;
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
        public SuscriptionTypeRegistryResponse ReadId(int id)
        {
            SuscriptionTypeRegistryResponse response = new SuscriptionTypeRegistryResponse();
            CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType suscriptionType = new CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_id_suscriptionstype", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suscriptionType = new CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType();
                            suscriptionType.IdSuscriptionType = Convert.ToInt32(reader["idSuscriptionType"]);
                            suscriptionType.Description = Convert.ToString(reader["description"]);
                        }
                        reader.Close();
                        response.success = true;
                        response.suscriptionType = suscriptionType;
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