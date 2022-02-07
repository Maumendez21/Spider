using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Inspection
{
    public class InspectionPrincipal
    {
        public int id { set; get; }
        public string node { set; get; }
        public string folio { set; get; }
        public string date { set; get; }
        public int idMechanic { set; get; }
        public int idType { set; get; }
        public string device { set; get; }
        public string mileage { set; get; }
        public int idResponsible { set; get; }

    }
}