using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.DashBoard
{
    public class GraficasDLT
    {
        public Graficas graficaDistancia { get; set; }
        public Graficas graficaLitros { get; set; }
        public Graficas graficaTiempo { get; set; }
        public Graficas graficaRendimiento { get; set; }
        public GraficasDLT()
        {
            graficaDistancia = new Graficas();
            graficaLitros = new Graficas();
            graficaTiempo = new Graficas();
            graficaRendimiento = new Graficas();
        }
    }
}