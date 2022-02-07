using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Cleaning
{
    public class Cleaning
    {
        public string Device { get; set; }
        public int DayRange { get; set; }
        public int PreviousDay { get; set; }
    }
}