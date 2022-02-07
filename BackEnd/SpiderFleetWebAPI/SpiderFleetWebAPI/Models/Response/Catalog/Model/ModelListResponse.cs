using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.Model
{
    public class ModelListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Catalogs.Model.Model> ListModels { get; set; }

        public ModelListResponse()
        {
            ListModels = new List<CredencialSpiderFleet.Models.Catalogs.Model.Model>();
        }
    }
}