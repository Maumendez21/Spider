using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Inspection
{
    public class InspectionRequest
    {
        public List<CredencialSpiderFleet.Models.Inspection.InspectionResults> listinspectionresult { set; get; }
        public string node { set; get; }
        public string Folio { set; get; }
        public string Date { set; get; }
        public int idMechanic { set; get; }
        public int idType { set; get; }
        public string device { set; get; }
        public string mileage { set; get; }
        public int idResponsible { set; get; }
    }
}