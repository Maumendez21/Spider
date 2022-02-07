using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Admin
{
    public class CompanyAdminDao
    {

        public CompanyAdminDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public CompanyAdminResponse UpdateChangeCompanyDevice(string empresa, string device)
        {
            CompanyAdminResponse response = new CompanyAdminResponse();
            int respuesta = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_change_device_company", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@company", Convert.ToString(empresa)));
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(device)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("Error al intenar actulizar el registro");
                        return response;
                    }
                    else
                    {
                        response.success = true;
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