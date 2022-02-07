using CredencialSpiderFleet.Models.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils.Inventory.Obd
{
    public class CompanyAssignmentObdsDao
    {
        public CompanyAssignmentObdsDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();
        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();
        private const int longitudDevice = 15;

        /// <summary>
        /// Actualizacion de Usuario
        /// </summary>
        public Dictionary<string, string> Update(CredencialSpiderFleet.Models.Inventory.Obd.CompanyAssignmentObds obdList)
        {
            int respuesta = 0;
            Dictionary<string, string> listError = new Dictionary<string, string>();
            try
            {
                if (sql.IsConnection)
                {
                    foreach (string obd in obdList.AssignmentObds)
                    {
                        useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                        if (string.IsNullOrEmpty(obd))
                        {
                            listError.Add("Hay un registro NULL", "No hay dispositivo"); continue;
                        }
                        else if (!useful.IsValidLength(obd.Trim(), longitudDevice))
                        {
                            if (!string.IsNullOrEmpty(obd))
                            {
                                listError.Add(obd, "La longitud excede de lo establecido rango maximo " + longitudDevice + " caracteres");
                            }                           
                            continue;
                        }

                        cn = sql.Connection();
                        SqlCommand cmd = new SqlCommand("ad.sp_update_obd_company_assignment", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@idCompany", Convert.ToString(obdList.IdCompany)));
                    
                        if (!string.IsNullOrEmpty(obd)) { cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(obd))); }
                        //else { listError.Add("Hay un registro NULL", "No hay dispositivo"); continue; }

                        SqlParameter sqlParameter = new SqlParameter();
                        sqlParameter.ParameterName = "@cMensaje";
                        sqlParameter.SqlDbType = SqlDbType.Int;
                        sqlParameter.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(sqlParameter);
                        cmd.ExecuteNonQuery();

                        respuesta = Convert.ToInt32(sqlParameter.Value.ToString());
                        if(respuesta != 1)
                        {
                            if (respuesta == 2)
                            {
                                listError.Add(obd, "No existe el dispositivo seleccionado");
                            }
                            else if (respuesta == 3)
                            {
                                listError.Add(obd, "Error al intentar asignar el dispositivo");
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
            return listError;
        }

        /// <summary>
        /// Consulta de Usuarios con estatus 1
        /// </summary>
        public List<string> Read()
        {
            //List<CredencialSpiderFleet.Models.Inventory.Obd.CompanyAssignmentObds> listObds = new List<CredencialSpiderFleet.Models.Inventory.Obd.CompanyAssignmentObds>();
            //CredencialSpiderFleet.Models.Inventory.Obd.CompanyAssignmentObds obd = new CredencialSpiderFleet.Models.Inventory.Obd.CompanyAssignmentObds();
            List<string> listObds = new List<string>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_obd_company_no_assignment", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string device = Convert.ToString(reader["idDevice"]);
                            listObds.Add(device);
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
            return listObds;
        }

        /// <summary>
        /// Consulta de por id de la Compañia para obtener los asignados
        /// </summary>
        public List<string> ReadId(int id)
        {
            //CredencialSpiderFleet.Models.Inventory.Obd.Obd obd = new CredencialSpiderFleet.Models.Inventory.Obd.Obd();
            List<string> listObds = new List<string>();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_read_obd_id_company", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string device = Convert.ToString(reader["idDevice"]);
                            listObds.Add(device);
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
            return listObds;
        }
    }
}