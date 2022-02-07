using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Diary
{
    public class Diary
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
        public string Device { get; set; }
        public int Responsable { get; set; }
        public string Frecuency { get; set; }
        public string Node { get; set; }
    }
}