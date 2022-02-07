using CredencialSpiderFleet.Models.Itineraries;
using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.Itineraries
{
    public class StrokeResponse : BasicResponse
    {
        public string VehicleName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartingPoint { get; set; }
        public string FinalPoint { get; set; }
        public List<Points> listPoints { get; set; }
        public List<Icons> listIcons { get; set; }
        public List<Time> listTime { get; set; }
        public List<WaitTime> listWaitTime { get; set; }
        public string FuelConsumption { get; set; }
        public string OdoConsumption { get; set; }
        public string ElapsedTime { get; set; }
        public int Braking { get; set; }
        public int Acceleration { get; set; }
        public int Speed { get; set; }
        public int RPM { get; set; }
        public string ResponsibleName { get; set; }
        public double TotalDistanciaDouble { get; set; }
        public int DeviceType { get; set; }
        public GraficaTiempoVelocidad Grafica { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public StrokeResponse()
        {
            VehicleName = string.Empty;
            StartDate = string.Empty;
            EndDate = string.Empty;
            StartingPoint = string.Empty;
            FinalPoint = string.Empty;
            listPoints = new List<Points>();
            listIcons = new List<Icons>();
            listTime = new List<Time>();
            listWaitTime = new List<WaitTime>();
            FuelConsumption = string.Empty;
            OdoConsumption = string.Empty;
            ElapsedTime = string.Empty;
            Braking = 0;
            Acceleration = 0;
            Speed = 0;
            RPM = 0;
            ResponsibleName = string.Empty;
            TotalDistanciaDouble = 0;
            DeviceType = 0;
            Grafica = new GraficaTiempoVelocidad();
            Latitude = string.Empty;
            Longitude = string.Empty;
        }
    }
}