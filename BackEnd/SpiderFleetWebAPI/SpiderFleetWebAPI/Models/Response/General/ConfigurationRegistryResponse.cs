using CredencialSpiderFleet.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.General
{
    public class ConfigurationRegistryResponse: BasicResponse
    {
        public ConfigurationRegistry Registry { get; set; }

        public ConfigurationRegistryResponse()
        {
            Registry = new ConfigurationRegistry();
        }
    }
}