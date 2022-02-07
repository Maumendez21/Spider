using CredencialSpiderFleet.Models.Responsible;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Responsible
{
    public class ResponsibleVehicleResponse :  BasicResponse
    {
        public ResponsibleVehicle responsible;

        public ResponsibleVehicleResponse()
        {
            responsible = new ResponsibleVehicle();
        }
    }
}