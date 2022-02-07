using CredencialSpiderFleet.Models.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Catalog.InfoSubCompany
{
    public class InfoSubCompanyDao
    {

        public InfoSubCompanyDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyListByUser> Read(string username)
        {
            List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyListByUser> listSubCompany = new List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyListByUser>();
            CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyListByUser subCompany = new CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyListByUser();


            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_subcompany_list", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subCompany = new CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyListByUser();
                            subCompany.IdSubCompany = Convert.ToString(reader["ID_Node"]);
                            subCompany.SubCompany = Convert.ToString(reader["Nombre"]);
                            subCompany.hierarchy = Convert.ToString(reader["Jerarquia"]);
                            listSubCompany.Add(subCompany);
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
            return listSubCompany;
        }


        //public List<CredencialSpiderFleet.Models.Inventory.Obd.ObdInfoUser> ReadId(int idSubaCompany)
        //{
        //    List<CredencialSpiderFleet.Models.Inventory.Obd.ObdInfoUser> listSubCompany = new List<CredencialSpiderFleet.Models.Inventory.Obd.ObdInfoUser>();
        //    CredencialSpiderFleet.Models.Inventory.Obd.ObdInfoUser subCompany = new CredencialSpiderFleet.Models.Inventory.Obd.ObdInfoUser();

        //    try
        //    {
        //        if (sql.IsConnection)
        //        {
        //            cn = sql.Connection();
        //            SqlCommand cmd = new SqlCommand("ad.sp_consult_device_assignament_subcompany_list", cn);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@idsubcompany", Convert.ToInt32(idSubaCompany)));
        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    subCompany = new CredencialSpiderFleet.Models.Inventory.Obd.ObdInfoUser();
        //                    subCompany.IdDevice = Convert.ToString(reader["idDevice"]);
        //                    subCompany.Name = Convert.ToString(reader["Name"]);
        //                    subCompany.LicensePlate = Convert.ToString(reader["LicensePlate"]);
        //                    subCompany.IdImagen = Convert.ToInt32(reader["IdImg"]);
        //                    subCompany.hierarchy = Convert.ToString(reader["Jerarquia"]);

        //                    subCompany.Date= Convert.ToDateTime(reader["date"]);
        //                    subCompany.Longitude = Convert.ToString(reader["longitude"]);
        //                    subCompany.Latitude = Convert.ToString(reader["latitude"]);
        //                    subCompany.Event = Convert.ToString(reader["event"]);
        //                    subCompany.Status = Convert.ToInt32(reader["status"]);

        //                    listSubCompany.Add(subCompany);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        cn.Close();
        //    }
        //    return listSubCompany;
        //}
   
    }
}