using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Inspection
{
    public class InspectionList 
    {
        public string node { set; get; }
        public string folio { set; get; }
        public string date { set; get; }
        public int idmecanico { set; get; }
        public string Mecanico { set; get; }
        public int idType { set; get; }
        public string device { set; get; }
        public string namevehicle { set; get; }
        public string mileage { set; get; }
        public int idresponsable { set; get; }
        public string Responsable { set; get; }
        public List<CredencialSpiderFleet.Models.Inspection.ListResultHeaderInspection> results { set; get; }
    }
}