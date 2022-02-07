using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Sub.SubComapny;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Sub.SubComapny
{
    public class SubCompanyAssignmentUsersDao
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        /// <summary>
        /// Metodo que Actualiza ell jerarquia para la asignacion del usuario hacia ls subempresa
        /// </summary>
        public ListErrorSubCompanyResponse Update(CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyAssignmentUsers usersList)
        {
            ListErrorSubCompanyResponse response = new ListErrorSubCompanyResponse();
            int respuesta = 0;
            Dictionary<string, string> listError = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(usersList.IdSubCompany))
                {
                    response.success = false;
                    response.messages.Add("Verifique el campo de la sub compañia se encuentra vacio");
                    return response;
                }

                if (usersList.AssignmentUsers.Count == 0)
                {
                    response.success = false;
                    response.messages.Add("No ingreso ningun Usuario");
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
                    int position = 1;
                    foreach (string user in usersList.AssignmentUsers)
                    {

                        if(string.IsNullOrEmpty(user))
                        {
                            listError.Add("El dato de la posion " + position + " se encuntra vacio", "Verificar los datos ingresados");
                            position++;
                            continue;
                        }

                        cn = sql.Connection();
                        SqlCommand cmd = new SqlCommand("ad.sp_update_subcompany_assignment", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@idsubcompany", Convert.ToString(usersList.IdSubCompany)));
                        cmd.Parameters.Add(new SqlParameter("@iduser", Convert.ToString(user)));

                        SqlParameter sqlParameter = new SqlParameter();
                        sqlParameter.ParameterName = "@cMensaje";
                        sqlParameter.SqlDbType = SqlDbType.Int;
                        sqlParameter.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(sqlParameter);
                        cmd.ExecuteNonQuery();

                        respuesta = Convert.ToInt32(sqlParameter.Value.ToString());
                        if (respuesta != 1)
                        {
                            if (respuesta == 2)
                            {
                                listError.Add(user + "", "No existe el usuario seleccionado");
                            }
                            else if (respuesta == 3)
                            {
                                listError.Add(user + "", "Error al intentar asignar el usuario");
                            }
                            else if (respuesta == 4)
                            {
                                listError.Add(usersList.IdSubCompany + "", "No existe la SubCompañia verifique");
                                break;
                            }
                        }

                        position++;
                    }

                    if (listError.Count == 0)
                    {
                        response.listError = listError;
                        response.success = true;
                    }
                    else if (listError.Count > 0)
                    {
                        response.success = false;
                        response.messages.Add("Verifique la lista de Errores");
                        response.listError = listError;
                        return response;
                    }
                }
                else
                {
                    response.success = false;
                    response.messages.Add("Problemas con la conexión vuelva a intentarlo");
                    response.listError = listError;
                    return response;
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