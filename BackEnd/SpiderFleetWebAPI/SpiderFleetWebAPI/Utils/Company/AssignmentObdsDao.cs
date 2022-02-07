using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Company;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Company
{
    public class AssignmentObdsDao
    {
        /// <summary>
        /// 
        /// </summary>
        public AssignmentObdsDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        /// <summary>
        /// Consulta de Usuarios con estatus 1
        /// </summary>
        public AssignmentObdsResponse Read(string username)
        {
            AssignmentObdsResponse response = new AssignmentObdsResponse();
            List<CredencialSpiderFleet.Models.Company.AssignmentObds> listObds = new List<CredencialSpiderFleet.Models.Company.AssignmentObds>();
            CredencialSpiderFleet.Models.Company.AssignmentObds obds = new CredencialSpiderFleet.Models.Company.AssignmentObds();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_username_obd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obds = new CredencialSpiderFleet.Models.Company.AssignmentObds();
                            obds.IdDevice = Convert.ToString(reader["idDevice"]);
                            obds.Label = Convert.ToString(reader["label"]);
                            //obds.IdSubCompany = Convert.ToInt32(reader["idSubCompany"]);
                            obds.SubCompany = Convert.ToString(reader["SubCompany"]);
                            obds.Jerarquia = Convert.ToString(reader["hierarchy"]);
                            obds.IdTypeDevice = Convert.ToInt32(reader["idType"]);
                            obds.Description = Convert.ToString(reader["descripcion_type"]);

                            listObds.Add(obds);
                        }

                        reader.Close();
                        response.listObds = listObds;
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