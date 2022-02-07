using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Configuration
{
    public class ConfigurationRequest
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}