using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.LandingPage
{
    public class LandingPageRequest
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Comments { get; set; }

        public string Company { get; set; }
    }
}