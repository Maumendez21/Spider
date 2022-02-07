using CredencialSpiderFleet.Models.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Inventory.Obd
{
    public class ObdDao
    {
        //public ObdDao() { }

        //private SqlConnection cn = new SqlConnection();
        //private SqlHelper sql = new SqlHelper();

        ///// <summary>
        ///// Creacion de Usurio
        ///// </summary>
        //public int Create(CredencialSpiderFleet.Models.Inventory.Obd.Obd obd)
        //{
        //    int respuesta = 0;
        //    try
        //    {
        //        if (sql.IsConnection)
        //        {
        //            cn = sql.Connection();
        //            SqlCommand cmd = new SqlCommand("ad.sp_create_obd", cn);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@idDevice", Convert.ToString(obd.IdDevice)));
        //            cmd.Parameters.Add(new SqlParameter("@label", Convert.ToString(obd.Label)));
        //            //cmd.Parameters.Add(new SqlParameter("@idCompany", Convert.ToInt32(obd.IdCompany)));
        //            if (obd.IdCompany != null) { cmd.Parameters.Add(new SqlParameter("@idCompany", Convert.ToInt32(obd.IdCompany))); }
        //            else { cmd.Parameters.Add(new SqlParameter("@idCompany", DBNull.Value)); }

        //            cmd.Parameters.Add(new SqlParameter("@idType", Convert.ToInt32(obd.IdType)));
        //            if (obd.IdSim != null) { cmd.Parameters.Add(new SqlParameter("@idSim", Convert.ToInt32(obd.IdSim))); }
        //            else { cmd.Parameters.Add(new SqlParameter("@idSim", DBNull.Value)); }

        //            SqlParameter sqlParameter = new SqlParameter();
        //            sqlParameter.ParameterName = "@cMensaje";
        //            sqlParameter.SqlDbType = SqlDbType.Int;
        //            sqlParameter.Direction = ParameterDirection.Output;

        //            cmd.Parameters.Add(sqlParameter);
        //            cmd.ExecuteNonQuery();

        //            respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

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
        //    return respuesta;
        //}

        ///// <summary>
        ///// Actualizacion de Usuario
        ///// </summary>
        //public int Update(CredencialSpiderFleet.Models.Inventory.Obd.Obd obd)
        //{
        //    int respuesta = 0;
        //    try
        //    {
        //        if (sql.IsConnection)
        //        {
        //            cn = sql.Connection();
        //            SqlCommand cmd = new SqlCommand("ad.sp_update_obd", cn);
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(obd.IdDevice)));
        //            cmd.Parameters.Add(new SqlParameter("@label", Convert.ToString(obd.Label)));
        //            //cmd.Parameters.Add(new SqlParameter("@idCompany", Convert.ToInt32(obd.IdCompany)));
        //            if (obd.IdCompany != null) { cmd.Parameters.Add(new SqlParameter("@idCompany", Convert.ToInt32(obd.IdCompany))); }
        //            else { cmd.Parameters.Add(new SqlParameter("@idCompany", DBNull.Value)); }
        //            cmd.Parameters.Add(new SqlParameter("@idType", Convert.ToInt32(obd.IdType)));                    
        //            if (obd.IdSim !=  null) { cmd.Parameters.Add(new SqlParameter("@idSim", Convert.ToInt32(obd.IdSim))); }
        //            else { cmd.Parameters.Add(new SqlParameter("@idSim", DBNull.Value)); }

        //            SqlParameter sqlParameter = new SqlParameter();
        //            sqlParameter.ParameterName = "@cMensaje";
        //            sqlParameter.SqlDbType = SqlDbType.Int;
        //            sqlParameter.Direction = ParameterDirection.Output;

        //            cmd.Parameters.Add(sqlParameter);
        //            cmd.ExecuteNonQuery();

        //            respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

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
        //    return respuesta;
        //}

        ///// <summary>
        ///// Consulta de Usuarios con estatus 1
        ///// </summary>
        //public List<CredencialSpiderFleet.Models.Inventory.Obd.Obd> Read()
        //{
        //    DataSet dsConsulta = new DataSet();
        //    List<CredencialSpiderFleet.Models.Inventory.Obd.Obd> listObds = new List<CredencialSpiderFleet.Models.Inventory.Obd.Obd>();
        //    CredencialSpiderFleet.Models.Inventory.Obd.Obd obd = new CredencialSpiderFleet.Models.Inventory.Obd.Obd();
        //    try
        //    {
        //        if (sql.IsConnection)
        //        {
        //            cn = sql.Connection();
        //            SqlCommand cmd = new SqlCommand("ad.sp_read_obd", cn);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    obd = new CredencialSpiderFleet.Models.Inventory.Obd.Obd();
        //                    obd.IdDevice = Convert.ToString(reader["idDevice"]);
        //                    obd.Label = Convert.ToString(reader["label"]);
        //                    obd.IdType = Convert.ToInt32(reader["idType"]);
        //                    obd.IdSim = (reader["idSim"] == DBNull.Value) ? (int?)null : ((int)reader["idSim"]);
        //                    obd.SIM = (reader["sim"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["sim"]));
        //                    obd.IdCompany = (reader["idCompany"] == DBNull.Value) ? (int?)null : ((int)reader["idCompany"]); //Convert.ToInt32(reader["idCompany"]);
        //                    obd.Name = (reader["name"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["name"]); //Convert.ToString(reader["name"]);
        //                    obd.TaxID = (reader["taxId"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["taxId"]); // Convert.ToString(reader["taxId"]);
        //                    listObds.Add(obd);
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
        //    return listObds;
        //}

        ///// <summary>
        ///// Consulta de Ususario por id
        ///// </summary>
        //public CredencialSpiderFleet.Models.Inventory.Obd.Obd ReadId(string id)
        //{
        //    CredencialSpiderFleet.Models.Inventory.Obd.Obd obd = new CredencialSpiderFleet.Models.Inventory.Obd.Obd();

        //    try
        //    {
        //        if (sql.IsConnection)
        //        {
        //            cn = sql.Connection();
        //            SqlCommand cmd = new SqlCommand("ad.sp_read_id_obd", cn);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(id)));
        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    obd = new CredencialSpiderFleet.Models.Inventory.Obd.Obd();
        //                    obd.IdDevice = Convert.ToString(reader["idDevice"]);
        //                    obd.Label = Convert.ToString(reader["label"]);
        //                    obd.IdType = Convert.ToInt32(reader["idType"]);
        //                    obd.IdSim = Convert.ToInt32(reader["idSim"]);
        //                    obd.SIM = Convert.ToString(reader["sim"]);
        //                    obd.IdCompany = (reader["idCompany"] == DBNull.Value) ? (int?)null : ((int)reader["idCompany"]); //Convert.ToInt32(reader["idCompany"]);
        //                    obd.Name = (reader["name"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["name"]); //Convert.ToString(reader["name"]);
        //                    obd.TaxID = (reader["taxId"] == DBNull.Value) ? string.Empty : Convert.ToString(reader["taxId"]); // Convert.ToString(reader["taxId"]);
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
        //    return obd;
        //}

        ///// <summary>
        ///// Metodo que obtiene los dispositivos por el id de la sub compañia
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public List<CredencialSpiderFleet.Models.Inventory.Obd.ObdHierarchy> ReadIdSubCompanyHierarchy(int id, string name)
        //{
        //    List<CredencialSpiderFleet.Models.Inventory.Obd.ObdHierarchy> listDevice = new List<CredencialSpiderFleet.Models.Inventory.Obd.ObdHierarchy>();
        //    CredencialSpiderFleet.Models.Inventory.Obd.ObdHierarchy obd = new CredencialSpiderFleet.Models.Inventory.Obd.ObdHierarchy();

        //    try
        //    {
        //        if (sql.IsConnection)
        //        {
        //            cn = sql.Connection();
        //            SqlCommand cmd = new SqlCommand("ad.sp_consult_device_hierarchy_list", cn);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@idsubcompany", Convert.ToString(id)));
        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    obd = new CredencialSpiderFleet.Models.Inventory.Obd.ObdHierarchy();
        //                    obd.Device = Convert.ToString(reader["idDevice"]);
        //                    obd.IdSubCompany = Convert.ToInt32(reader["idSubCompany"]);
        //                    obd.NameSubCompany = name;

        //                    listDevice.Add(obd);
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
        //    return listDevice;
        //}

    }
}