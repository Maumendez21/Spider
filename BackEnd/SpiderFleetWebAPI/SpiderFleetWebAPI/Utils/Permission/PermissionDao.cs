using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Permission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Permission
{
    public class PermissionDao
    {
        public PermissionDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public PermissionResponse CreateorUpdate(CredencialSpiderFleet.Models.Permission.ListPermission permission)
        {
            PermissionResponse response = new PermissionResponse();
            int respuesta = 0; 
            try
            {
                
                if (sql.IsConnection)
                {
                    cn = sql.Connection();

                    for (int i = 0; i < permission.PermissionList.Count; i++)
                    {

                        SqlCommand cmd = new SqlCommand("ad.sp_create_or_update_permission", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@user", Convert.ToString(permission.PermissionList[i].IdUser)));
                        cmd.Parameters.Add(new SqlParameter("@module", Convert.ToString(permission.PermissionList[i].Modulo)));
                        cmd.Parameters.Add(new SqlParameter("@status", Convert.ToByte(permission.PermissionList[i].Active)));

                        SqlParameter sqlParameter = new SqlParameter();
                        sqlParameter.ParameterName = "@cMensaje";
                        sqlParameter.SqlDbType = SqlDbType.Int;
                        sqlParameter.Direction = ParameterDirection.Output;


                        cmd.Parameters.Add(sqlParameter);
                        cmd.ExecuteNonQuery();

                        respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    }

                    try
                    {
                        if (respuesta == 3)
                        {
                            response.success = false;
                            response.messages.Add("Error al asignar permisos.");
                            return response;
                        }
                        else if (respuesta == 1)
                        {
                            response.success = true;
                            return response;
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



        

        public ModulesResponse Read(string user)
        {
            ModulesResponse response = new ModulesResponse();
            List<CredencialSpiderFleet.Models.Permission.Modules> listModules = new List<CredencialSpiderFleet.Models.Permission.Modules>();
            CredencialSpiderFleet.Models.Permission.Modules modules = new CredencialSpiderFleet.Models.Permission.Modules();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_permission_activate", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@user", Convert.ToString(user)));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            modules = new CredencialSpiderFleet.Models.Permission.Modules();
                            modules.Id = Convert.ToString(reader["ID_Module"]);
                            modules.Descripcion = Convert.ToString(reader["Descripcion"]);
                            modules.estatus = Convert.ToBoolean(reader["estatus"]);
                            listModules.Add(modules);
                        }
                        reader.Close();
                        response.success = true;
                        response.modules = listModules;
                    }
                }
                else
                {
                    response.success = false;
                }


                return response;
                
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