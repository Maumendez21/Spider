using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Catalog.TypeService
{
    public class TypeServiceUpdateRequest : TypeServiceRequest
    {
        public int IdTypeService { get; set; }
    }
}