using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Inspection
{
    public class HeaderandTemplateContenedor
    {
        public int idheader { set; get; }
        public string Encabezado { get; set; }
        public int idTemplate { get; set; }
        public string Objeto { get; set; }
    }
}