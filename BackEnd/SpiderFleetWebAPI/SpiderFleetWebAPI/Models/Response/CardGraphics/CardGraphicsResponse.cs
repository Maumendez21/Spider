using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.CardGraphics
{
    public class CardGraphicsResponse: BasicResponse
    {
        public CredencialSpiderFleet.Models.CardGraphics.CardGraphics Odo { get; set; }
        public CredencialSpiderFleet.Models.CardGraphics.CardGraphics Fuel { get; set; }
        public CredencialSpiderFleet.Models.CardGraphics.CardGraphics Time { get; set; }


        public CardGraphicsResponse()
        {
            Odo = new CredencialSpiderFleet.Models.CardGraphics.CardGraphics();
            Fuel = new CredencialSpiderFleet.Models.CardGraphics.CardGraphics();
            Time = new CredencialSpiderFleet.Models.CardGraphics.CardGraphics();
        }
    }
}