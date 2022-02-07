using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Catalog.SamplingTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Catalog.SamplingTime
{
    public class SamplingTimeDao
    {
        public SamplingTimeDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private CredencialSpiderFleet.Models.Catalogs.SamplingTime.SamplingTimeRegistry MapToValue(SqlDataReader reader)
        {
            return new CredencialSpiderFleet.Models.Catalogs.SamplingTime.SamplingTimeRegistry()
            {
                Id = Convert.ToInt32(reader["id"].ToString()),
                Description = reader["description"].ToString()
            };
        }


        public SamplingTimeListResponse Read()
        {
            SamplingTimeListResponse response = new SamplingTimeListResponse();

            var ListSamTime = new List<CredencialSpiderFleet.Models.Catalogs.SamplingTime.SamplingTimeRegistry>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_consult_list_sampling_Time", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListSamTime.Add(MapToValue(reader));
                            }

                            reader.Close();
                        }

                        if (ListSamTime.Count > 0)
                        {
                            response.ListSamTime = ListSamTime;
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