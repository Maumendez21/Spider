using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Catalog.Mechanics
{
    public class MechanicRequest
    {
        public string Node { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Specialty { get; set; }


    }
}