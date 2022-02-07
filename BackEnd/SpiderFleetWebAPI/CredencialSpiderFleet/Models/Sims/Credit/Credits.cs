using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Sims.Credit
{
    public class Credits
    {
        public string Msisdn { get; set; }
        public string Amount { get; set; }

        public Credits()
        {
            Msisdn = string.Empty;
            Amount = string.Empty;
        }
    }
}