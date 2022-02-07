using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Main.Combo;
using SpiderFleetWebAPI.Models.Response.Main.Combo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Main.Combo
{
    public class ComboDao
    {
        public ComboDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        public EmpresaResponse ReadEmpresas(string username)
        {
            EmpresaResponse response = new EmpresaResponse();
            Empresa empresas = new Empresa();
            List<Empresa> listEmpresas = new List<Empresa>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_subcompany", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            empresas = new Empresa();
                            empresas.Level = Convert.ToString(reader["level"]);
                            empresas.Id = Convert.ToString(reader["ID_Empresa"]);
                            empresas.Nombre = Convert.ToString(reader["Nombre"]);
                            listEmpresas.Add(empresas);
                        }
                    }
                    response.listEmpresas = listEmpresas;
                    response.success = true;
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

        public SubEmpresasResponse ReadSubEmpresas(string hierarchy)
        {
            SubEmpresasResponse response = new SubEmpresasResponse();
            SubEmpresa subEmpresas = new SubEmpresa();
            List<SubEmpresa> listSubEmpresas = new List<SubEmpresa>();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_device_subcompany", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@hierarchy", Convert.ToString(hierarchy)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subEmpresas = new SubEmpresa();
                            subEmpresas.Management = Convert.ToString(reader["Gerencia"]);
                            subEmpresas.Hierarchy = Convert.ToString(reader["Hierarchy"]);                            
                            //subEmpresas.Device = Convert.ToString(reader["device"]);
                            //subEmpresas.Nombre = Convert.ToString(reader["Nombre"]);
                            //subEmpresas.ID_Spider = Convert.ToString(reader["ID_Spider"]);
                            listSubEmpresas.Add(subEmpresas);
                        }
                        reader.Close();
                        response.listSubEmpresas = listSubEmpresas;
                        response.success = true;
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
    
    }
}