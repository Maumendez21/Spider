using System;
using System.Configuration;

namespace CredencialSpiderFleet.Models.Configuration
{
    public class VariableConfiguration
    {
        public string user { get; set; }
        public string password { get; set; }
        public string currency { get; set; }
        public string url { get; set; }
        public string email { get; set; }
        public int totalTrips { get; set; }
        public string snap { get; set; }
        public string revgeocode { get; set; }

        public string emailContacto { get; set; }
        /// <summary>
        /// ...
        /// </summary>
        public VariableConfiguration()
        {
            user = ConfigurationManager.ConnectionStrings["userKey"].ConnectionString;
            password = ConfigurationManager.ConnectionStrings["passKey"].ConnectionString;
            currency = ConfigurationManager.ConnectionStrings["currencyKey"].ConnectionString;
            url = ConfigurationManager.ConnectionStrings["urlSim"].ConnectionString;
            email = ConfigurationManager.ConnectionStrings["email"].ConnectionString;
            totalTrips = Convert.ToInt32(ConfigurationManager.ConnectionStrings["totalTrips"].ConnectionString);
            snap = ConfigurationManager.ConnectionStrings["snap"].ConnectionString;
            revgeocode = ConfigurationManager.ConnectionStrings["revgeocode"].ConnectionString;
            emailContacto = ConfigurationManager.ConnectionStrings["emailcontacto"].ConnectionString;
        }
    }
}