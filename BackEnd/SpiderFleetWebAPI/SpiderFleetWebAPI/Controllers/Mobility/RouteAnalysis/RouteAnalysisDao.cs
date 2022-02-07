using CredencialSpiderFleet.Models.Itineraries;
using SpiderFleetWebAPI.Models.Response.Route;
using SpiderFleetWebAPI.Utils.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Controllers.Mobility.RouteAnalysis
{
    public class RouteAnalysisDao
    {





        public double Porcentaje(string route, string device, List<Points> listPoints)
        {
            double analisisFinal = 0.0;
            try
            {
                int analisis = 0;
                foreach (var item in listPoints)
                {
                    analisis +=CheckRoute(device, Convert.ToDouble(item.lat), Convert.ToDouble(item.lng));
                }

                RouteRegistryResponse response = new RouteRegistryResponse();
                response = (new RouteDao()).ReadId(route);

                int routePoints = response.routes.ListPoints.Count();

                analisisFinal = Math.Round( Convert.ToDouble((analisis * 100) / routePoints), 2);




            }
            catch(Exception ex)
            {

            }
            return 0.0;
        }


        private int CheckRoute(string device, double latitud, double longitud)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return 1;
        }


    }
}