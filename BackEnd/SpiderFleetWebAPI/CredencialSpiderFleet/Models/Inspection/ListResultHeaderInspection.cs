using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Inspection
{
    public class ListResultHeaderInspection
    {
        public string header { set; get; }
        public List<CredencialSpiderFleet.Models.Inspection.InspectionResultheaderless> listinspectionresults { set; get; }
    }
}