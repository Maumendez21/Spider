using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Mobility.PointInterestAnalysis;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.Mobility.PointInterestAnalysis;
using SpiderFleetWebAPI.Models.Response.PointsInterest;
using SpiderFleetWebAPI.Utils.PointInterest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Mobility.PointInterestAnalysis
{
    public class PointInterestAnalysisDao
    {
        //PointInterestAnalysis

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public PointInterestAnalysisDao() { }

        public PointInterestAnalysisResponse Analysis(string poinInterest, DateTime start, DateTime end, string device)
        {
            PointInterestAnalysisResponse response = new PointInterestAnalysisResponse();
            try
            {
                var listPoint = GetPointInteres(poinInterest, start, end, device);
                var listAnalysis = new List<PointInterestAnalysisRegistry>();

                if(listPoint.Count > 0)
                {
                    DateTime fechaAnterior = DateTime.Today;
                    int count = 0;
                    bool band = false;
                    DateTime now = DateTime.Now;
                    now = UseFul.DateFormatMX(now);

                    foreach (var item in listPoint)
                    {
                        PointInterestAnalysisRegistry analysis = new PointInterestAnalysisRegistry();

                        if (count == 0)
                        {
                            if (item.Status & item.Diff > 0)
                            {
                                DateTime date = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, 0, 0, 0);
                                DateTime today = DateTime.Today;

                                int compare = UseFul.compare(today, date);

                                if (compare == 0)
                                {
                                    double seconds = UseFul.diferenciaSeconds(now, item.Date);
                                    analysis.Time = UseFul.CalcularTime(Convert.ToInt32(seconds));
                                }
                                else
                                {
                                    analysis.Time = UseFul.CalcularTime(item.Diff);
                                }

                                analysis.Latitude = item.Latitude;
                                analysis.Longitude = item.Longitude;

                                band = true;
                            }
                        }
                        else
                        {
                            if(!item.Status)
                            {
                                int length = listAnalysis.Count;
                                if (band)
                                {
                                    fechaAnterior = listAnalysis[length - 1].Start;

                                    double seconds = UseFul.diferenciaSeconds(item.Date, fechaAnterior);
                                    listAnalysis[length - 1].Time = UseFul.CalcularTime(Convert.ToInt32(seconds));
                                    listAnalysis[length - 1].Latitude = item.Latitude;
                                    listAnalysis[length - 1].Longitude = item.Longitude;

                                    band = false;
                                }
                            }
                            else
                            {
                                if(item.Diff > 0)
                                {
                                    DateTime date = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, 0, 0, 0);
                                    DateTime today = DateTime.Today;

                                    int compare = UseFul.compare(today, date);

                                    if (compare == 0)
                                    {
                                        double seconds = UseFul.diferenciaSeconds(now, item.Date);
                                        analysis.Time = UseFul.CalcularTime(Convert.ToInt32(seconds));
                                    }
                                    else
                                    {
                                        analysis.Time = UseFul.CalcularTime(item.Diff);
                                    }

                                    analysis.Latitude = item.Latitude;
                                    analysis.Longitude = item.Longitude;

                                    band = true;
                                }                                
                            }
                        }

                        fechaAnterior = item.Date;
                        analysis.Start = item.Date;
                        analysis.VehicleName = item.VehicleName;
                        analysis.Device = item.Device;
                        analysis.Date = UseFul.DateFormatdddd_ddMMMMyyyy(item.Date);

                        count++;

                        if (item.Status & item.Diff > 0)
                        {
                            listAnalysis.Add(analysis);
                        }                        
                    }

                    response.success = true;
                    response.ListAnalysis = listAnalysis;

                    if(listAnalysis.Count > 0)
                    {
                        PointInterestRegistryResponse point = new PointInterestRegistryResponse();
                        point = (new PointInterestDao()).ReadId(poinInterest);
                        response.PointInterest = point.PointInterest;
                    }                    
                }
                else
                {
                    response.success = false;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
            finally
            {
                cn.Close();
            }
            return response;
        }


        private List<CredencialSpiderFleet.Models.Mobility.PointInterestAnalysis.PointInterestAnalysis> GetPointInteres(string poinInterest, DateTime start, DateTime end, string device)
        {
            var listPoint = new List<CredencialSpiderFleet.Models.Mobility.PointInterestAnalysis.PointInterestAnalysis>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    using (SqlCommand cmd = new SqlCommand("ad.sp_consult_point_interes_analysis", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mongo", poinInterest));
                        cmd.Parameters.Add(new SqlParameter("@device", device));
                        cmd.Parameters.Add(new SqlParameter("@start", start));
                        cmd.Parameters.Add(new SqlParameter("@end", end));

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CredencialSpiderFleet.Models.Mobility.PointInterestAnalysis.PointInterestAnalysis point = new CredencialSpiderFleet.Models.Mobility.PointInterestAnalysis.PointInterestAnalysis();

                                point.Date = Convert.ToDateTime(reader["fecha"].ToString());
                                point.Device = reader["device"].ToString();
                                point.VehicleName = reader["Nombre"].ToString();

                                string[] coordinates = reader["coordinates"].ToString().Split(',');
                                point.Latitude = coordinates[0].ToString();
                                point.Longitude = coordinates[1].ToString();

                                point.Status = Convert.ToBoolean(reader["estatus"]);
                                point.Diff = Convert.ToInt32(reader["diff"]);


                                //DateTime now = DateTime.Now;
                                //DateTime date = new DateTime(point.Date.Year, point.Date.Month, point.Date.Day, 0, 0, 0); 
                                //DateTime today = DateTime.Today;


                                //int compare = UseFul.compare(today, date);

                                //if(compare == 0)
                                //{

                                //    now = UseFul.DateFormatMX(now);

                                //    double seconds = UseFul.diferenciaSeconds(now, point.Date);
                                //    point.Time = UseFul.CalcularTime(Convert.ToInt32(seconds));
                                //}
                                //else
                                //{
                                //    point.Time = UseFul.CalcularTime(Convert.ToInt32(reader["diff"].ToString()));
                                //}
                                listPoint.Add(point);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }
            return listPoint;
        }

    }
}