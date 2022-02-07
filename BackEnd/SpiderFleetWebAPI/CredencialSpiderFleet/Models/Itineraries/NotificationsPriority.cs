using CredencialSpiderFleet.Models.Main.LastPositionDevice;
using System;
using System.Collections.Generic;

namespace CredencialSpiderFleet.Models.Itineraries
{
    public class NotificationsPriority
    {
        public int Id { get; set; }
        public string Device { get; set; }
        public string Alarm { get; set; }
        public string Name { get; set; }
        public string Latitud { get; set; }
        public string Longitude { get; set; }
        public DateTime DateGenerated { get; set; }
        public string Description { get; set; }
        public int View { get; set; }
        public List<List<double>> Coordinates { get; set; }

        public NotificationsPriority()
        {
            Id = 0;
            Device = string.Empty;
            Alarm = string.Empty;
            Name = string.Empty;
            Latitud = string.Empty;
            Longitude = string.Empty;
            DateGenerated = DateTime.Now;
            Description = string.Empty;
            View = 0;
            Coordinates = new List<List<double>>();
        }
    }
}