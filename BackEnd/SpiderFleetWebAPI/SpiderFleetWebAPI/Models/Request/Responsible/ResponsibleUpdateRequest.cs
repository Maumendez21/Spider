using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Responsible
{
    public class ResponsibleUpdateRequest
    {
        public int Id { get; set; }
        public string Hierarchy { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Area { get; set; }
    }
}