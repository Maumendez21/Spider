using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Catalog.Mechanics;
using System.Data;

namespace SpiderFleetWebAPI.Utils.Catalog.Mechanics
{
    public class MechanicDao
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();
        private const int longitud = 60;

        public MechanicsResponse Create(CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics mechanics)
        {
            MechanicsResponse response = new MechanicsResponse();
            int respuesta = 0;

            try
            {
                if (string.IsNullOrEmpty(mechanics.FullName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el Nombre");
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            if (!string.IsNullOrEmpty(mechanics.FullName.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(mechanics.FullName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(mechanics.FullName.Trim(), longitud))
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
                    SqlCommand cmd = new SqlCommand("ad.sp_create_mechanic", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@fullname", Convert.ToString(mechanics.FullName)));
                    cmd.Parameters.Add(new SqlParameter("@phonenumber ", Convert.ToString(mechanics.Phone)));
                    cmd.Parameters.Add(new SqlParameter("@specialty ", Convert.ToString(mechanics.Specialty)));
                    cmd.Parameters.Add(new SqlParameter("@node ", Convert.ToString(mechanics.Node)));



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

        public MechanicsResponse Update(CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics mechanics)
        {
            MechanicsResponse response = new MechanicsResponse();
            int respuesta = 0;

            try
            {
                if (mechanics.IdMechanics == 0)
                {
                    response.success = false;
                    response.messages.Add("No tiene el parametro IdMechanic");
                    return response;
                }

                if (string.IsNullOrEmpty(mechanics.FullName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el nombre");
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            if (!string.IsNullOrEmpty(mechanics.FullName.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(mechanics.FullName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(mechanics.FullName.Trim(), longitud))
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
                    SqlCommand cmd = new SqlCommand("ad.sp_update_mechanic", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(mechanics.IdMechanics)));
                    cmd.Parameters.Add(new SqlParameter("@fullname", Convert.ToString(mechanics.FullName)));
                    cmd.Parameters.Add(new SqlParameter("@phonenumber ", Convert.ToString(mechanics.Phone)));
                    cmd.Parameters.Add(new SqlParameter("@specialty ", Convert.ToString(mechanics.Specialty)));


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

        public MechanicsListResponse Read(string node)
        {
            MechanicsListResponse response = new MechanicsListResponse();
            List<CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics> ListMechanics = new List<CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics>();
            CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics mechanic = new CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_mechanics", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mechanic = new CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics();
                            mechanic.IdMechanics = Convert.ToInt32(reader["id"]);
                            mechanic.FullName = Convert.ToString(reader["fullname"]);
                            mechanic.Phone = Convert.ToString(reader["phone"]);
                            mechanic.Specialty = Convert.ToString(reader["specialty"]);
                            mechanic.Node = Convert.ToString(reader["node"]);
                            ListMechanics.Add(mechanic);
                        }
                        reader.Close();
                        response.success = true;
                        response.ListMechanics = ListMechanics;
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

        public MechanicsRegistryResponse ReadId(int id)
        {
            MechanicsRegistryResponse response = new MechanicsRegistryResponse();
            CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics mechanics = new CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_mechanic_id", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mechanics = new CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics();
                            mechanics.IdMechanics = Convert.ToInt32(reader["id"]);
                            mechanics.FullName = Convert.ToString(reader["fullname"]);
                            mechanics.Phone = Convert.ToString(reader["phone"]);
                            mechanics.Specialty = Convert.ToString(reader["specialty"]);
                            mechanics.Node = Convert.ToString(reader["node"]);
                        }
                        reader.Close();
                        response.success = true;
                        response.mechanics = mechanics;
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

        public MechanicsDeleteResponse DeleteId(int id)
        {
            MechanicsDeleteResponse response = new MechanicsDeleteResponse();
            int respuesta = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_delete_mechanic", cn);
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