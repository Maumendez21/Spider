using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.ApiGoogle
{
    public class GoogleGeoCodeResponse
    {
        public Plus_code plus_code { get; set; }
        public List<ResultsItem> results { get; set; }
        public string status { get; set; }
    }
}