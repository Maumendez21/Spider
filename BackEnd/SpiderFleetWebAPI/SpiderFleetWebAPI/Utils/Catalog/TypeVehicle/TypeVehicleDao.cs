using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Catalog.TypeVehicle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Catalog.TypeVehicle
{
    public class TypeVehicleDao
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();
        private const int longitud = 50;

        public TypeVehicleResponse Create(CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle typeVehicle)
        {
            TypeVehicleResponse response = new TypeVehicleResponse();
            int respuesta = 0;

            try
            {
                if (string.IsNullOrEmpty(typeVehicle.Description.Trim()))
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

            if (!string.IsNullOrEmpty(typeVehicle.Description.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(typeVehicle.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(typeVehicle.Description.Trim(), longitud))
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
                    SqlCommand cmd = new SqlCommand("ad.sp_create_vehicle_type", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(typeVehicle.Description)));
                    

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

        public TypeVehicleResponse Update(CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle typeVehicle)
        {
            TypeVehicleResponse response = new TypeVehicleResponse();
            int respuesta = 0;

            try
            {
                if (typeVehicle.IdTypeVehicle == 0)
                {
                    response.success = false;
                    response.messages.Add("No tiene el parametro idTipoVehiculo");
                    return response;
                }

                if (string.IsNullOrEmpty(typeVehicle.Description.Trim()))
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

            if (!string.IsNullOrEmpty(typeVehicle.Description.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(typeVehicle.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(typeVehicle.Description.Trim(), longitud))
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
                    SqlCommand cmd = new SqlCommand("ad.sp_update_vehicle_type", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(typeVehicle.IdTypeVehicle)));
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(typeVehicle.Description)));
                   

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

        public TypeVehicleListResponse Read()
        {
            TypeVehicleListResponse response = new TypeVehicleListResponse();
            List<CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle> ListTypeVehicle = new List<CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle>();
            CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle type = new CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_vehicle_type", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            type = new CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle();
                            type.IdTypeVehicle = Convert.ToInt32(reader["id"]);
                            type.Description = Convert.ToString(reader["description"]);
                            ListTypeVehicle.Add(type);
                        }
                        reader.Close();
                        response.success = true;
                        response.ListTypeVehicles = ListTypeVehicle;
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

        public TypeVehicleRegistryResponse ReadId(int id)
        {
            TypeVehicleRegistryResponse response = new TypeVehicleRegistryResponse();
            CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle TypeVehicle = new CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_vehicle_type_id", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TypeVehicle = new CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle();
                            TypeVehicle.IdTypeVehicle = Convert.ToInt32(reader["id"]);
                            TypeVehicle.Description = Convert.ToString(reader["description"]);
                            
                        }
                        reader.Close();
                        response.success = true;
                        response.TypeVehicle = TypeVehicle;
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


        public TypeVehicleDeleteResponse DeleteId(int id)
        {
            TypeVehicleDeleteResponse response = new TypeVehicleDeleteResponse();
            int respuesta = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_delete_vehicle_type", cn);
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