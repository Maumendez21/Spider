using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Responsible
{
    public class ResponsibleRegistry
    {
        public int Id { get; set; }
        //public string Hierarchy { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Area { get; set; }
        //public int Status { get; set; }
    }
}