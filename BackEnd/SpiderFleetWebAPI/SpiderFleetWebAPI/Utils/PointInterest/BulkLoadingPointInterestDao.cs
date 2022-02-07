using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using SpiderFleetWebAPI.Models.Mongo.PointInterest;
using SpiderFleetWebAPI.Models.Response.PointsInterest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace SpiderFleetWebAPI.Utils.PointInterest
{
    public class BulkLoadingPointInterestDao
    {
        public BulkLoadingPointIntrerestResponse ReaderExcel(string hierarchy, DataTable dtPuntosInteres)
        {
            BulkLoadingPointIntrerestResponse response = new BulkLoadingPointIntrerestResponse();
            try
            {
                DataExcel(hierarchy, dtPuntosInteres, response);

                if(response.messages.Count > 0)
                {
                    response.success = false;
                    return response;
                }

                response.success = true;
                return response;
            }
            catch (Exception ex)
            {

                return response;
            }
        }        

        private void DataExcel(string node, DataTable dt, BulkLoadingPointIntrerestResponse principal)
        {
            PointsInterestResponse response = new PointsInterestResponse();
            try
            {                

                for (int i = 1; i < dt.Rows.Count; i++)
                {

                    PointsInterest points = new PointsInterest();
                    string id = string.Empty;

                    points.Name = Convert.ToString(dt.Rows[i][0]);
                    points.Hierarchy = node;
                    points.Active = true;
                    points.Description = Convert.ToString(dt.Rows[i][1]);

                    InterestPoint coordinates = new InterestPoint();
                    coordinates.Radius = 100.0;
                    coordinates.Type = "Point";

                    string lat = dt.Rows[i][2].ToString();
                    string lng = dt.Rows[i][3].ToString();

                    coordinates.Coordinate = new List<double>();                    
                    coordinates.Coordinate.Add(Convert.ToDouble(lng));
                    coordinates.Coordinate.Add(Convert.ToDouble(lat));

                    points.InterestPoint = new InterestPoint();
                    points.InterestPoint = coordinates;

                    response = (new PointInterestDao()).Create(points, node);                    

                    if(response.messages.Count > 0)
                    {
                        foreach (var item in response.messages)
                        {
                            principal.messages.Add(item);
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}