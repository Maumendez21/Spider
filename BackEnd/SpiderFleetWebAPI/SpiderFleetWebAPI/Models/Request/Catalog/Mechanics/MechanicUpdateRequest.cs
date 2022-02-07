using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Catalog.Mechanics
{
    public class MechanicUpdateRequest : MechanicRequest
    {
        public int IdMechanic { set; get; }
    }
}