using CredencialSpiderFleet.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.General
{
    public class ConfigurationListResponse: BasicResponse
    {
        public List<ConfigurationRegistry> ListResgistry { get; set; }

        public ConfigurationListResponse()
        {
            ListResgistry = new List<ConfigurationRegistry>();
        }
    }
}