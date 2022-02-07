using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Request.Status
{
    public class StatusRequest
    {
        public int IdStatus { get; set; }
        public string Description { get; set; }
    }
}