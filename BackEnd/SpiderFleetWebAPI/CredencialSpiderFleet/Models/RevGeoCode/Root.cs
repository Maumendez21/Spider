using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.RevGeoCode
{
    public class Root
    {
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string title { get; set; }
        public string id { get; set; }
        public string resultType { get; set; }
        public string houseNumberType { get; set; }
        public Address address { get; set; }
        public Position position { get; set; }
        public List<Access> access { get; set; }
        public int distance { get; set; }
        public MapView mapView { get; set; }
    }

    public class Address
    {
        public string label { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string stateCode { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string street { get; set; }
        public string postalCode { get; set; }
        public string houseNumber { get; set; }
    }

    public class Position
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Access
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class MapView
    {
        public double west { get; set; }
        public double south { get; set; }
        public double east { get; set; }
        public double north { get; set; }
    }
}