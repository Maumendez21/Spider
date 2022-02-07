using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Catalog.TradeMark;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Catalog.TradeMark
{
    public class TradeMarkDao
    {

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public TradeMarkListResponse Read()
        {
            TradeMarkListResponse response = new TradeMarkListResponse();
            List<CredencialSpiderFleet.Models.Catalogs.TradeMark.TradeMark> ListMarks = new List<CredencialSpiderFleet.Models.Catalogs.TradeMark.TradeMark>();
            CredencialSpiderFleet.Models.Catalogs.TradeMark.TradeMark mark = new CredencialSpiderFleet.Models.Catalogs.TradeMark.TradeMark();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_marcas_vehiculos", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mark = new CredencialSpiderFleet.Models.Catalogs.TradeMark.TradeMark();
                            mark.IdMark = Convert.ToInt16(reader["IdMark"]);
                            mark.Description = Convert.ToString(reader["Description"]);
                            ListMarks.Add(mark);
                        }
                        reader.Close();
                        response.success = true;
                        response.ListMarks = ListMarks;
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