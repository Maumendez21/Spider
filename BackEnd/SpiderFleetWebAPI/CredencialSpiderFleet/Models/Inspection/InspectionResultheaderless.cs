using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Inspection
{
    public class InspectionResultheaderless
    {
        public int idtemplate  { set; get; }
        public string objeto { set; get; }
        public string folio { set; get; }
        public int yes { set; get; }
        public int no { set; get; }
        public int Good { set; get; }
        public int Regular { set; get; }
        public int Bad { set; get; }
        public string Notes { set; get; }
    }
}