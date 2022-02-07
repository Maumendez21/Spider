using CredencialSpiderFleet.Models.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.DashBoard
{
    public class DashBoardActivityDayResponse :  BasicResponse
    {
        public List<DashBoardActivityDay> ListDay { get; set; }
        public int Actives { get; set; }
        public int Inactives { get; set; }
        public int Total { get; set; }


        public DashBoardActivityDayResponse()
        {
            ListDay = new List<DashBoardActivityDay>();
        }
    }
}