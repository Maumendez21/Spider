using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Catalog.CommunicationMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Catalog.CommunicationMethods
{
    public class CommunicationMethodsDao
    {
        public CommunicationMethodsDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private CredencialSpiderFleet.Models.Catalogs.CommunicationMethods.CommunicationMethodsRegistry MapToValue(SqlDataReader reader)
        {
            return new CredencialSpiderFleet.Models.Catalogs.CommunicationMethods.CommunicationMethodsRegistry()
            {                
                Id = Convert.ToInt32(reader["id"].ToString()),
                Description = reader["description"].ToString()
            };
        }


        public CommunicationMethodsListResponse Read()
        {
            CommunicationMethodsListResponse response = new CommunicationMethodsListResponse();

            var ListComMethods = new List<CredencialSpiderFleet.Models.Catalogs.CommunicationMethods.CommunicationMethodsRegistry>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_consult_list_communication_methods", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListComMethods.Add(MapToValue(reader));
                            }

                            reader.Close();
                        }

                        if (ListComMethods.Count > 0)
                        {
                            response.ListComMethods = ListComMethods;
                            response.success = true;
                        }

                        return response;
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