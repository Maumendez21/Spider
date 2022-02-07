using CredencialSpiderFleet.Models.DashBoard;
using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.DashBoard
{
    public class DashBoardDLTGraficaBarraResponse: BasicResponse
    {
        public GraficasDLT graficas { get; set; }
        public string TotalDistancia { get; set; }
        public string TotalLitros { get; set; }
        public string TotalTiempo { get; set; }
        public string TotalRendimiento { get; set; }
        public List<Ranking> ListRankingBest { get; set; }
        public List<Ranking> ListRankingLower { get; set; }

        public DashBoardDLTGraficaBarraResponse()
        {
            graficas = new GraficasDLT();
            TotalDistancia = string.Empty;
            TotalLitros = string.Empty;
            TotalTiempo = string.Empty;
            TotalRendimiento = string.Empty;
            ListRankingBest = new List<Ranking>();
            ListRankingLower = new List<Ranking>();
        }

    }
}