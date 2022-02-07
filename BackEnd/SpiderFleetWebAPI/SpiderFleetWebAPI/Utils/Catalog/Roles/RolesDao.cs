using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Catalog.Roles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Catalog.Roles
{
    public class RolesDao
    {
        public RolesDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();
        private const int longitud = 50;

        /// <summary>
        /// Creacion de Usurio
        /// </summary>
        public RolesResponse Create(CredencialSpiderFleet.Models.Catalogs.Roles.Roles  roles)
        {
            RolesResponse response = new RolesResponse();
            int respuesta = 0;

            try
            {
                if (string.IsNullOrEmpty(roles.Description.Trim()))
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

            if (!string.IsNullOrEmpty(roles.Description.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(roles.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(roles.Description.Trim(), longitud))
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
                    SqlCommand cmd = new SqlCommand("ad.sp_create_role", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(roles.Description)));
                    cmd.Parameters.Add(new SqlParameter("@status", Convert.ToInt32(roles.Status)));

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
        public RolesResponse Update(CredencialSpiderFleet.Models.Catalogs.Roles.Roles roles)
        {
            RolesResponse response = new RolesResponse();
            int respuesta = 0;

            try
            {
                if (roles.IdRole == 0)
                {
                    response.success = false;
                    response.messages.Add("No tiene el parametro idRol");
                    return response;
                }

                if (string.IsNullOrEmpty(roles.Description.Trim()))
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

            if (!string.IsNullOrEmpty(roles.Description.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(roles.Description.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(roles.Description.Trim(), longitud))
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
                    SqlCommand cmd = new SqlCommand("ad.sp_update_role", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(roles.IdRole)));
                    cmd.Parameters.Add(new SqlParameter("@description", Convert.ToString(roles.Description)));
                    //cmd.Parameters.Add(new SqlParameter("@status", Convert.ToInt32(roles.Status)));

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
        public RolesListResponse Read(int status)
        {
            RolesListResponse response = new RolesListResponse();
            List<CredencialSpiderFleet.Models.Catalogs.Roles.Roles> listRoles = new List<CredencialSpiderFleet.Models.Catalogs.Roles.Roles>();
            CredencialSpiderFleet.Models.Catalogs.Roles.Roles rol = new CredencialSpiderFleet.Models.Catalogs.Roles.Roles();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_role", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@status", Convert.ToInt32(status)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rol = new CredencialSpiderFleet.Models.Catalogs.Roles.Roles();
                            rol.IdRole = Convert.ToInt32(reader["idRole"]);
                            rol.Description = Convert.ToString(reader["description"]);
                            rol.Status = Convert.ToInt32(reader["status"]);
                            listRoles.Add(rol);
                        }
                        reader.Close();
                        response.success = true;
                        response.listRoles = listRoles;
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
        public RolesRegistryResponse ReadId(int id)
        {
            RolesRegistryResponse response = new RolesRegistryResponse();
            CredencialSpiderFleet.Models.Catalogs.Roles.Roles rol = new CredencialSpiderFleet.Models.Catalogs.Roles.Roles();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_id_role", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToInt32(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rol = new CredencialSpiderFleet.Models.Catalogs.Roles.Roles();
                            rol.IdRole = Convert.ToInt32(reader["idRole"]);
                            rol.Description = Convert.ToString(reader["description"]);
                            rol.Status = Convert.ToInt32(reader["status"]);
                        }
                        reader.Close();
                        response.success = true;
                        response.roles = rol;
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
        /// SoftDelete de Ususario por id
        /// </summary>
        public RolesDeleteResponse DeleteId(int id)
        {
            RolesDeleteResponse response = new RolesDeleteResponse();
            int respuesta = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_delete_id_role", cn);
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
                        response.messages.Add("Error al intenar eliminar el registro");
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