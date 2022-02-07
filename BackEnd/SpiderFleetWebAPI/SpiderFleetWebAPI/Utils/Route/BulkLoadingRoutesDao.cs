using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using CredencialSpiderFleet.Models.RouteXML;
using SpiderFleetWebAPI.Models.Response.Route;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SpiderFleetWebAPI.Utils.Route
{
    public class BulkLoadingRoutesDao
    {

        public BulkLoadingRoutesResponse ReaderExcel(StreamReader reader, string name, string hierarchy)
        {
            BulkLoadingRoutesResponse response = new BulkLoadingRoutesResponse();
            try
            {
                StringBuilder xmlInputData = new StringBuilder();

                string Line;

                while ((Line = reader.ReadLine()) != null)
                {
                    Line = Line.Replace(",0 ", "").Replace("0-","-").Trim();
                    xmlInputData.Append(Line);
                }

                Kml dataKml = Deserialize<Kml>(xmlInputData.ToString());

                string[] coordinates = null;

                foreach (var item in dataKml.Document.Folder.Placemark)
                {
                    coordinates = item.LineString.Coordinates.Replace("0-", "-").Split(',');
                    if(coordinates.Length > 0)
                    {
                        break;
                    }
                }

                if(coordinates != null)
                {
                    List<List<double>> listCoordinates = new List<List<double>>();
                    List<double> listData = new List<double>();
                    int band = 0;

                    for (int i = 0; i < coordinates.Length; i++)
                    {
                        //string data = coordinates[i].Replace("0            ", "").Trim();
                        string data = coordinates[i].Trim();

                        if (band < 2)
                        {
                            listData.Add(Convert.ToDouble(data));
                            band++;
                        }

                        if (band == 2)
                        {

                            listCoordinates.Add(listData);
                            listData = new List<double>();
                            band = 0;
                        }
                    }

                    Models.Mongo.Route.Route route = new Models.Mongo.Route.Route();
                    route.Name = name;
                    route.Description = name;
                    route.Active = true;
                    route.Polygon = new Polygon();
                    Polygon polygon = new Polygon();
                    List<List<List<double>>> listCoordinatesRoute = new List<List<List<double>>>();
                    listCoordinatesRoute.Add(listCoordinates);
                    polygon.Coordinates = listCoordinatesRoute;
                    polygon.Type = "LineString";
                    route.Polygon = polygon;
                    RouteResponse responseRoute = new RouteResponse();
                    responseRoute = (new RouteDao()).Create(route, hierarchy);

                    if(responseRoute.success)
                    {
                        response.success = true;
                    }
                    else
                    {
                        response.success = false;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {

                return response;
            }
        }


        private void readXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("docxml.xml");
            foreach (XmlNode n1 in doc.DocumentElement.ChildNodes)
            {

                if (n1.HasChildNodes)
                {
                    foreach (XmlNode n2 in n1.ChildNodes)
                    {
                        string mat = n2.Attributes[""].Value;

                        foreach (XmlNode n3 in n2.ChildNodes)
                        {
                            string data = n3.Name + "  " + n3.InnerText;
                        }

                    }

                }

            }
        }

        public T Deserialize<T>(string input) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        public string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }
    }
}