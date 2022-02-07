using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Sims.Credit
{
    public class ListCredits
    {
        public List<Credits> credits { get; set; }

        public ListCredits()
        {
            credits = new List<Credits>();
        }
    }
}