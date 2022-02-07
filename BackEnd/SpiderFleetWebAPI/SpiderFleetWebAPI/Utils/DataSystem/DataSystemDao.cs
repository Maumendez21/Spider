using CredencialSpiderFleet.Models.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.DataSystem
{
    public class DataSystemDao
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public string GetDataSystem(string key)
        {
            var response = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_consult_system_key", cn))
                    {
                        cn = sql.Connection();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@key", key));


                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response = MapToValue(reader);
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private string MapToValue(SqlDataReader reader)
        {
            string parameter = reader["Parameter"].ToString();
            return parameter;
        }
    }
}
