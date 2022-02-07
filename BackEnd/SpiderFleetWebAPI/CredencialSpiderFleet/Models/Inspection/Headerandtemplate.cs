using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Inspection
{
    public class Headerandtemplate
    {
        public string Encabezado { set; get; }
        public List<CredencialSpiderFleet.Models.Inspection.Templatesplantilla> Templates { set; get; }
    }
}