using CredencialSpiderFleet.Models.Main.TraceTrip;
using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.Main.TraceTrip
{
    public class TraceTripResponse : BasicResponse
    {
        public List<List<Point>> listPoints { get; set; }
        public List<Makers> listMarkers { get; set; }
        public List<TripsInformation> listTrips { get; set; }

        
        public TraceTripResponse()
        {
            listPoints = new List<List<Point>>();
            listMarkers = new List<Makers>();
            listTrips = new List<TripsInformation>();
        }
    }
}