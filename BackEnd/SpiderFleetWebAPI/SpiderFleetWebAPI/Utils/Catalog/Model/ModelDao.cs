using CredencialSpiderFleet.Models.Connection;
using SpiderFleetWebAPI.Models.Response.Catalog.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Catalog.Model
{
    public class ModelDao
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public ModelListResponse Read()
        {
            ModelListResponse response = new ModelListResponse();
            List<CredencialSpiderFleet.Models.Catalogs.Model.Model> ListModels = new List<CredencialSpiderFleet.Models.Catalogs.Model.Model>();
            CredencialSpiderFleet.Models.Catalogs.Model.Model model = new CredencialSpiderFleet.Models.Catalogs.Model.Model();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_modelos_vehiculos", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model = new CredencialSpiderFleet.Models.Catalogs.Model.Model();
                            model.IdModel = Convert.ToInt16(reader["IdModel"]);
                            model.Description = Convert.ToString(reader["Description"]);
                            ListModels.Add(model);
                        }
                        reader.Close();
                        response.success = true;
                        response.ListModels = ListModels;
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