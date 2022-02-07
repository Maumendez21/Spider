using System;
using System.Data.SqlClient;
using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Catalog.TypeService;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SpiderFleetWebAPI.Utils.Catalog.TypeService
{
    public class TypeServiceDao
    {

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();
        private const int longitud = 50;

        public TypeServiceResponse Create(CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService typeService)
        {
            TypeServiceResponse response = new TypeServiceResponse();
            int respuesta = 0;

            try
            {
                if (string.IsNullOrEmpty(typeService.Description.Trim()))
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

            if (!string.IsNullOrEmpty(typeService.Description.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(typeService.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(typeService.Description.Trim(), longitud))
                {
                    response.success = false;
                    response.messages.Add("La longitud excede de lo establecido rango maximo " + longitud + " caracteres");
                    return response;
                }
            }

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_type_services", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(typeService.Description)));
                    cmd.Parameters.Add(new SqlParameter("@estimatedtime", Convert.ToString(typeService.EstimatedTime)));
                    cmd.Parameters.Add(new SqlParameter("@estimatedcost", Convert.ToDouble(typeService.EstimatedCost)));


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

        public TypeServiceResponse Update(CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService typeService)
        {
            TypeServiceResponse response = new TypeServiceResponse();
            int respuesta = 0;

            try
            {
                if (typeService.IdTypeService == 0)
                {
                    response.success = false;
                    response.messages.Add("No tiene el parametro idTipoVehiculo");
                    return response;
                }

                if (string.IsNullOrEmpty(typeService.Description.Trim()))
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

            if (!string.IsNullOrEmpty(typeService.Description.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(typeService.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(typeService.Description.Trim(), longitud))
                {
                    response.success = false;
                    response.messages.Add("La longitud excede de lo establecido rango maximo " + longitud + " caracteres");
                    return response;
                }
            }

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_type_services", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(typeService.IdTypeService)));
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(typeService.Description)));
                    cmd.Parameters.Add(new SqlParameter("@estimatedtime", Convert.ToString(typeService.EstimatedTime)));
                    cmd.Parameters.Add(new SqlParameter("@estimatedcost", Convert.ToDouble(typeService.EstimatedCost)));


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

        public TypeServiceListResponse Read()
        {
            TypeServiceListResponse response = new TypeServiceListResponse();
            List<CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService> ListTypeService = new List<CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService>();
            CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService type = new CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_type_services", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            type = new CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService();
                            type.IdTypeService = Convert.ToInt32(reader["id"]);
                            type.Description = Convert.ToString(reader["description"]);
                            type.EstimatedTime = Convert.ToString(reader["estimatedTime"]);
                            type.EstimatedCost = Convert.ToDouble(reader["estimatedCost"]);
                            ListTypeService.Add(type);
                        }
                        reader.Close();
                        response.success = true;
                        response.ListTypeServices = ListTypeService;
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


        public TypeServiceRegistryResponse ReadId(int id)
        {
            TypeServiceRegistryResponse response = new TypeServiceRegistryResponse();
            CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService typeService = new CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_type_services_id", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            typeService = new CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService();
                            typeService.IdTypeService = Convert.ToInt32(reader["id"]);
                            typeService.Description = Convert.ToString(reader["description"]);
                            typeService.EstimatedTime = Convert.ToString(reader["estimatedTime"]);
                            typeService.EstimatedCost = Convert.ToDouble(reader["estimatedCost"]);


                        }
                        reader.Close();
                        response.success = true;
                        response.typeService = typeService;
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

        public TypeServiceDeleteResponse DeleteId(int id)
        {
            TypeServiceDeleteResponse response = new TypeServiceDeleteResponse();
            int respuesta = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_delete_type_services", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));

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
                        response.messages.Add("El registro que desea eliminar no existe");
                        return response;
                    }
                    else if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Error al intentar eliminar el registro");
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


    }
}