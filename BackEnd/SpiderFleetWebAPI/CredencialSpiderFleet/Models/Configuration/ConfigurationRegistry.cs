using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Configuration
{
    public class ConfigurationRegistry
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}