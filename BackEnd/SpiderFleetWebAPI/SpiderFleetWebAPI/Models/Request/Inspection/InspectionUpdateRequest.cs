using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Inspection
{
    public class InspectionUpdateRequest 
    {
        public string Folio { set; get; }
        public string Date { set; get; }
        public string device { set; get; }
        public string mileage { set; get; }
        public List<CredencialSpiderFleet.Models.Inspection.InspectionResults> results { set; get; }
    }
}