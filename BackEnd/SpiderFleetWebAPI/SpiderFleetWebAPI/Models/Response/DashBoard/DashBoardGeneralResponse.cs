using CredencialSpiderFleet.Models.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.DashBoard
{
    public class DashBoardGeneralResponse : BasicResponse
    {
        public string TotalDistancia { get; set; }
        public string TotalTiempo { get; set; }
        public string TotalActivas { get; set; }
        public Graficas Graficas { get; set; }

        public DashBoardGeneralResponse ()
        {
            TotalDistancia = string.Empty;
            TotalTiempo = string.Empty;
            TotalActivas = string.Empty;
            Graficas = new Graficas();
        }
    }
}