using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SpiderFleetStripe.Configuration
{
    public class ConfigurationStripe
    {
        public string skKey { get; set; }
        public ConfigurationStripe()
        {
            skKey = ConfigurationManager.ConnectionStrings["skKey"].ConnectionString;
        }
    }
}