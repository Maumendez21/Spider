using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpiderFleetWebAPI.Models.Response.Inspection;
using CredencialSpiderFleet.Models.Inspection;
using System.Data.SqlClient;
using CredencialSpiderFleet.Models.Connection;
using System.Data;

namespace SpiderFleetWebAPI.Utils.Inspection
{
    public class InspectionDAO
    {
        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();


        public InspectionResponse Create(List<CredencialSpiderFleet.Models.Inspection.InspectionResults> Results, CredencialSpiderFleet.Models.Inspection.InspectionPrincipal principal)
        {
            InspectionResponse response = new InspectionResponse();
            try
            {
                
                string folio = "";
                foreach (var item in Results)
                {
                    item.Folio = folio;
                    response = Createresult(item);
                    folio = response.folio;
                }
                if (!string.IsNullOrEmpty(folio))
                {
                    principal.folio = folio;
                    Createprincipal(principal);
                }
                
                return response;
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }


        private InspectionResponse Createresult(CredencialSpiderFleet.Models.Inspection.InspectionResults inspection)
        {
            InspectionResponse response = new InspectionResponse();
            int respuesta = 0;
            string folio = "";
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_inspection_results", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@yes", Convert.ToInt32(inspection.yes)));
                    cmd.Parameters.Add(new SqlParameter("@no", Convert.ToInt32(inspection.no)));
                    cmd.Parameters.Add(new SqlParameter("@Good", Convert.ToInt32(inspection.Good)));
                    cmd.Parameters.Add(new SqlParameter("@Regular", Convert.ToInt32(inspection.Regular)));
                    cmd.Parameters.Add(new SqlParameter("@Bad", Convert.ToInt32(inspection.Bad)));
                    if (string.IsNullOrEmpty(inspection.Notes))
                    {
                        cmd.Parameters.Add(new SqlParameter("@Notes", ""));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@Notes", Convert.ToString(inspection.Notes)));
                    }
                    cmd.Parameters.Add(new SqlParameter("@idTemplate", Convert.ToInt32(inspection.idTemplate)));
                    cmd.Parameters.Add(new SqlParameter("@folio", Convert.ToString(inspection.Folio)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;
                    //SqlParameter sqlParameterf = new SqlParameter();
                    //sqlParameterf.ParameterName = "@folio";
                    //sqlParameterf.Size = 15;
                    //sqlParameterf.SqlDbType = SqlDbType.VarChar;
                    //sqlParameterf.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    //cmd.Parameters.Add(sqlParameterf);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());
                    //folio = Convert.ToString(sqlParameterf.Value.ToString());
                    if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Error al intenar dar de alta el registro");
                    }
                    else if (respuesta.ToString().Length == 10)
                    {
                        response.success = true;
                        response.folio = respuesta.ToString();
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                //return response;
            }
            finally
            {
                cn.Close();
            }
            return response;
        }


        private InspectionResponseprincipal Createprincipal(CredencialSpiderFleet.Models.Inspection.InspectionPrincipal inspectionprincipal)
        {
            InspectionResponseprincipal response = new InspectionResponseprincipal();
            int respuesta = 0;

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_principal_inspection", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(inspectionprincipal.node)));
                    cmd.Parameters.Add(new SqlParameter("@Folio", Convert.ToString(inspectionprincipal.folio)));
                    cmd.Parameters.Add(new SqlParameter("@date", Convert.ToString(inspectionprincipal.date)));
                    cmd.Parameters.Add(new SqlParameter("@idMechanic", Convert.ToInt32(inspectionprincipal.idMechanic)));
                    cmd.Parameters.Add(new SqlParameter("@idType", Convert.ToInt32(inspectionprincipal.idType)));
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(inspectionprincipal.device)));
                    cmd.Parameters.Add(new SqlParameter("@mileage", Convert.ToString(inspectionprincipal.mileage)));
                    cmd.Parameters.Add(new SqlParameter("@idResponsible", Convert.ToInt32(inspectionprincipal.idResponsible)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Error al intenar dar de alta el registro");
                        return response;
                    }
                    else if (respuesta == 1)
                    {
                        response.success = true;
                    }
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

        public InspectionResponseup UpdateInspection(List<CredencialSpiderFleet.Models.Inspection.InspectionResults> inspectionresults, CredencialSpiderFleet.Models.Inspection.InspectionPrincipal inspectionprincipal)
        {
            InspectionResponseup response = new InspectionResponseup();

            try
            {
                response = UpdatePrincipal(inspectionprincipal);
                if(response.success)
                {
                    if (inspectionresults.Count > 0) 
                    { 
                        foreach(var item in inspectionresults)
                        {
                            response = UpdateResults(item);
                        }
                    }
                    {

                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }

        public InspectionResponseup UpdateResults(CredencialSpiderFleet.Models.Inspection.InspectionResults inspectionresults)
        {
            InspectionResponseup response = new InspectionResponseup();
            int respuesta = 0;
            int longitud = 60;
            try
            {
                if (inspectionresults.Folio is null)
                {
                    response.success = false;
                    response.messages.Add("No tiene el parametro Folio");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            if (!string.IsNullOrEmpty(inspectionresults.Notes.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(inspectionresults.Notes.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(inspectionresults.Notes.Trim(), longitud))
                {
                    response.success = false;
                    response.messages.Add("La longitud excede de lo establecido rango maximo " + longitud + " caracteres");
                    return response;
                }
            }

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_Inspection", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@folio", Convert.ToString(inspectionresults.Folio)));
                    cmd.Parameters.Add(new SqlParameter("@yes", Convert.ToInt32(inspectionresults.yes)));
                    cmd.Parameters.Add(new SqlParameter("@no", Convert.ToInt32(inspectionresults.no)));
                    cmd.Parameters.Add(new SqlParameter("@Good", Convert.ToInt32(inspectionresults.Good)));
                    cmd.Parameters.Add(new SqlParameter("@Regular", Convert.ToInt32(inspectionresults.Regular)));
                    cmd.Parameters.Add(new SqlParameter("@Bad", Convert.ToInt32(inspectionresults.Bad)));
                    cmd.Parameters.Add(new SqlParameter("@Notes", Convert.ToString(inspectionresults.Notes)));
                    cmd.Parameters.Add(new SqlParameter("@idTemplate", Convert.ToInt32(inspectionresults.idTemplate)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    if (respuesta == 1)
                    {
                        response.success = true;
                    }
                    else if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("No se encuentra el registro");
                        return response;
                    }
                    else if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Error al tratar de actualizar el registro");
                        return response;
                    }
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
        public InspectionResponseup UpdatePrincipal(CredencialSpiderFleet.Models.Inspection.InspectionPrincipal inspectionprincipal)
        {
            InspectionResponseup response = new InspectionResponseup();
            int respuesta = 0;
            try
            {
                if (inspectionprincipal.folio is null)
                {
                    response.success = false;
                    response.messages.Add("No tiene el parametro Folio");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_principal_inspection", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Folio", Convert.ToString(inspectionprincipal.folio)));
                    cmd.Parameters.Add(new SqlParameter("@date", Convert.ToString(inspectionprincipal.date)));
                    cmd.Parameters.Add(new SqlParameter("@device", Convert.ToString(inspectionprincipal.device)));
                    cmd.Parameters.Add(new SqlParameter("@mileage", Convert.ToString(inspectionprincipal.mileage)));


                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.Int;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(sqlParameter.Value.ToString());

                    if (respuesta == 1)
                    {
                        response.success = true;
                    }
                    else if (respuesta == 2)
                    {
                        response.success = false;
                        response.messages.Add("No se encuentra el registro");
                        return response;
                    }
                    else if (respuesta == 3)
                    {
                        response.success = false;
                        response.messages.Add("Error al tratar de actualizar el registro");
                        return response;
                    }
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

        public InspectionResponseList ReadGeneral(string node)
        { 
            
            InspectionResponseList response = new InspectionResponseList();

            try 
            {
                response = ReadPrincipal(node);
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

        }

        public InspectionResponseList ReadFolio(string node, string folio)
        {
            InspectionResponseList response = new InspectionResponseList();

            try
            {
                response = ReadPrincipalFolio(node,folio);
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }

        private List<InspectionResultList> Readresults(string folio)
        {
            
            List<InspectionResultList> resultslist = new List<InspectionResultList>();
            CredencialSpiderFleet.Models.Inspection.InspectionResultList results = new CredencialSpiderFleet.Models.Inspection.InspectionResultList();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_Inspection_Results", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@folio", Convert.ToString(folio)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            results = new CredencialSpiderFleet.Models.Inspection.InspectionResultList();
                            results.encabezado = Convert.ToString(reader["Encabezado"]);
                            results.idTemplate = Convert.ToInt32(reader["idtemplate"]);
                            results.objeto = Convert.ToString(reader["objeto"]);
                            results.folio = Convert.ToString(reader["Folio"]);
                            results.yes = Convert.ToInt32(reader["yes"]);
                            results.no = Convert.ToInt32(reader["no"]);
                            results.Good = Convert.ToInt32(reader["Good"]);
                            results.Regular = Convert.ToInt32(reader["Regular"]);
                            results.Bad = Convert.ToInt32(reader["Bad"]);
                            results.Notes = Convert.ToString(reader["Notes"]);
                            resultslist.Add(results);
                        }
                        reader.Close();
                        return resultslist;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return resultslist;
        }

        public InspectionResultListResponse readresultsplantilla(string folio)
        {
            InspectionResultListResponse response = new InspectionResultListResponse();
            //List<CredencialSpiderFleet.Models.Inspection.Header> ListIsnpectionresults = new List<CredencialSpiderFleet.Models.Inspection.Header>();
            //CredencialSpiderFleet.Models.Inspection.Header header = new CredencialSpiderFleet.Models.Inspection.Header();

            List<CredencialSpiderFleet.Models.Inspection.ListResultHeaderInspection> Encabezadoslist = new List<CredencialSpiderFleet.Models.Inspection.ListResultHeaderInspection>();
            CredencialSpiderFleet.Models.Inspection.ListResultHeaderInspection encabezados = new CredencialSpiderFleet.Models.Inspection.ListResultHeaderInspection();

            List<InspectionResultList> resultslist = new List<InspectionResultList>();

            List<CredencialSpiderFleet.Models.Inspection.InspectionResultheaderless> resultslistheaderless = new List<InspectionResultheaderless>();
            CredencialSpiderFleet.Models.Inspection.InspectionResultheaderless results = new CredencialSpiderFleet.Models.Inspection.InspectionResultheaderless();

            try
            {
                resultslist = Readresults(folio);
                string headeraux = string.Empty;
                int count = 0;
                foreach (var item in resultslist)
                {
                    if (string.IsNullOrEmpty(headeraux))
                    {
                        encabezados = new ListResultHeaderInspection();
                        headeraux = item.encabezado;
                        encabezados.header = headeraux;
                        encabezados.listinspectionresults = new List<InspectionResultheaderless>();
                        results = new InspectionResultheaderless();
                        results.idtemplate = item.idTemplate;
                        results.objeto = item.objeto;
                        results.folio = item.folio;
                        results.yes = item.yes;
                        results.no = item.no;
                        results.Good = item.Good;
                        results.Regular = item.Regular;
                        results.Bad = item.Bad;
                        results.Notes = item.Notes;
                        resultslistheaderless.Add(results);
                    }
                    else
                    {
                        if (headeraux.Equals(item.encabezado))
                        {
                            results = new InspectionResultheaderless();
                            results.idtemplate = item.idTemplate;
                            results.objeto = item.objeto;
                            results.folio = item.folio;
                            results.yes = item.yes;
                            results.no = item.no;
                            results.Good = item.Good;
                            results.Regular = item.Regular;
                            results.Bad = item.Bad;
                            results.Notes = item.Notes;
                            resultslistheaderless.Add(results);
                        }
                        else
                        {
                            encabezados.listinspectionresults = resultslistheaderless;
                            Encabezadoslist.Add(encabezados);
                            resultslistheaderless = new List<InspectionResultheaderless>();
                            encabezados = new ListResultHeaderInspection();
                            headeraux = item.encabezado;
                            encabezados.header = headeraux;
                            results = new InspectionResultheaderless();
                            results.idtemplate = item.idTemplate;
                            results.objeto = item.objeto;
                            results.folio = item.folio;
                            results.yes = item.yes;
                            results.no = item.no;
                            results.Good = item.Good;
                            results.Regular = item.Regular;
                            results.Bad = item.Bad;
                            results.Notes = item.Notes;
                            resultslistheaderless.Add(results);
                        }
                    }
                    count++;
                }
                encabezados.listinspectionresults = resultslistheaderless;
                Encabezadoslist.Add(encabezados);
                response.results = Encabezadoslist;
                response.success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }

        public InspectionResponseList ReadPrincipal(string node)
        {
            InspectionResponseList response = new InspectionResponseList();
            List<CredencialSpiderFleet.Models.Inspection.InspectionList> InspectionList = new List<CredencialSpiderFleet.Models.Inspection.InspectionList>();
            CredencialSpiderFleet.Models.Inspection.InspectionList inspectionlistob = new CredencialSpiderFleet.Models.Inspection.InspectionList();
            InspectionResultListResponse responseResults = new InspectionResultListResponse();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_principal_inspection_hierachy", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inspectionlistob = new CredencialSpiderFleet.Models.Inspection.InspectionList();
                            inspectionlistob.node = Convert.ToString(reader["Nodo"]);
                            inspectionlistob.folio = Convert.ToString(reader["Folio"]);
                            inspectionlistob.date = Convert.ToString(reader["Date"]);
                            inspectionlistob.idmecanico = Convert.ToInt32(reader["idmecanico"]);
                            inspectionlistob.Mecanico = Convert.ToString(reader["Mecanico"]);
                            inspectionlistob.idType = Convert.ToInt32(reader["idType"]);
                            inspectionlistob.device = Convert.ToString(reader["device"]);
                            inspectionlistob.namevehicle = Convert.ToString(reader["namevehicle"]);
                            inspectionlistob.mileage = Convert.ToString(reader["mileage"]);
                            inspectionlistob.idresponsable = Convert.ToInt32(reader["idresponsable"]);
                            inspectionlistob.Responsable = Convert.ToString(reader["Responsable"]);
                            InspectionList.Add(inspectionlistob);
                        }
                        reader.Close();
                        foreach(var item in InspectionList)
                        {
                            responseResults = readresultsplantilla(item.folio);
                            item.results = responseResults.results;
                        }
                        response.success = true;
                        
                        response.InspectionList = InspectionList;
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

        public InspectionResponseList ReadPrincipalFolio(string node,string folio)
        {
            InspectionResponseList response = new InspectionResponseList();
            List<CredencialSpiderFleet.Models.Inspection.InspectionList> InspectionList = new List<CredencialSpiderFleet.Models.Inspection.InspectionList>();
            CredencialSpiderFleet.Models.Inspection.InspectionList inspectionlistob = new CredencialSpiderFleet.Models.Inspection.InspectionList();
            InspectionResultListResponse responseResults = new InspectionResultListResponse();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_principal_inspection", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    cmd.Parameters.Add(new SqlParameter("@folio", Convert.ToString(folio)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inspectionlistob = new CredencialSpiderFleet.Models.Inspection.InspectionList();
                            inspectionlistob.node = Convert.ToString(reader["Nodo"]);
                            inspectionlistob.folio = Convert.ToString(reader["Folio"]);
                            inspectionlistob.date = Convert.ToString(reader["Date"]);
                            inspectionlistob.idmecanico = Convert.ToInt32(reader["idmecanico"]);
                            inspectionlistob.Mecanico = Convert.ToString(reader["Mecanico"]);
                            inspectionlistob.idType = Convert.ToInt32(reader["idType"]);
                            inspectionlistob.device = Convert.ToString(reader["device"]);
                            inspectionlistob.namevehicle = Convert.ToString(reader["namevehicle"]);
                            inspectionlistob.mileage = Convert.ToString(reader["mileage"]);
                            inspectionlistob.idresponsable = Convert.ToInt32(reader["idresponsable"]);
                            inspectionlistob.Responsable = Convert.ToString(reader["Responsable"]);
                            InspectionList.Add(inspectionlistob);
                        }
                        reader.Close();
                        foreach (var item in InspectionList)
                        {
                            responseResults = readresultsplantilla(item.folio);
                            item.results = responseResults.results;
                        }
                        response.success = true;

                        response.InspectionList = InspectionList;
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

        private List<CredencialSpiderFleet.Models.Inspection.HeaderandTemplateContenedor> ReadEncabezados()
        {
            //headersandTemplatesResponse response = new headersandTemplatesResponse();

            //List<CredencialSpiderFleet.Models.Inspection.Headerandtemplate> PlantillaHeader = new List<CredencialSpiderFleet.Models.Inspection.Headerandtemplate>();
            //CredencialSpiderFleet.Models.Inspection.Headerandtemplate headertemplate = new CredencialSpiderFleet.Models.Inspection.Headerandtemplate();

            //List<CredencialSpiderFleet.Models.Inspection.Templatesplantilla> Templates = new List<CredencialSpiderFleet.Models.Inspection.Templatesplantilla>();
            //CredencialSpiderFleet.Models.Inspection.Templatesplantilla templateplantilla = new CredencialSpiderFleet.Models.Inspection.Templatesplantilla();

            List<CredencialSpiderFleet.Models.Inspection.HeaderandTemplateContenedor> contenedorlist = new List<CredencialSpiderFleet.Models.Inspection.HeaderandTemplateContenedor>();
            CredencialSpiderFleet.Models.Inspection.HeaderandTemplateContenedor contenedor = new HeaderandTemplateContenedor();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_template_header", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contenedor = new HeaderandTemplateContenedor();
                            contenedor.idheader = Convert.ToInt32(reader["idheader"]);
                            contenedor.Encabezado = Convert.ToString(reader["Encabezado"]);
                            contenedor.idTemplate = Convert.ToInt32(reader["idTemplate"]);
                            contenedor.Objeto = Convert.ToString(reader["Objeto"]);
                            contenedorlist.Add(contenedor);
                        }
                        reader.Close();
                        return contenedorlist;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                
            }
            return contenedorlist;
        }

        public headersandTemplatesResponse Readplantilla()
        {
            headersandTemplatesResponse response = new headersandTemplatesResponse();
            List<CredencialSpiderFleet.Models.Inspection.HeaderandTemplateContenedor> contenedorlist = new List<CredencialSpiderFleet.Models.Inspection.HeaderandTemplateContenedor>();

            List<CredencialSpiderFleet.Models.Inspection.Headerandtemplate> PlantillaHeader = new List<CredencialSpiderFleet.Models.Inspection.Headerandtemplate>();
            CredencialSpiderFleet.Models.Inspection.Headerandtemplate headertemplate = new CredencialSpiderFleet.Models.Inspection.Headerandtemplate();

            List<CredencialSpiderFleet.Models.Inspection.Templatesplantilla> Templates = new List<CredencialSpiderFleet.Models.Inspection.Templatesplantilla>();
            CredencialSpiderFleet.Models.Inspection.Templatesplantilla templateplantilla = new CredencialSpiderFleet.Models.Inspection.Templatesplantilla();
            try
            {
                contenedorlist = ReadEncabezados();
                string header = string.Empty;
                int count = 0;
                foreach (var item in contenedorlist)
                {
                    if (string.IsNullOrEmpty(header))
                    {
                        header = item.Encabezado;
                        headertemplate.Encabezado = header;
                        templateplantilla = new CredencialSpiderFleet.Models.Inspection.Templatesplantilla();
                        templateplantilla.idTemplate = item.idTemplate;
                        templateplantilla.template = item.Objeto;
                        templateplantilla.yes = 0;
                        templateplantilla.no = 0;
                        templateplantilla.Good = 0;
                        templateplantilla.Regular = 0;
                        templateplantilla.Bad = 0;
                        templateplantilla.Notes = "";
                        Templates.Add(templateplantilla);
                    }
                    else
                    {
                        if (header.Equals(item.Encabezado))
                        {
                            templateplantilla = new CredencialSpiderFleet.Models.Inspection.Templatesplantilla();
                            templateplantilla.idTemplate = item.idTemplate;
                            templateplantilla.template = item.Objeto;
                            templateplantilla.yes = 0;
                            templateplantilla.no = 0;
                            templateplantilla.Good = 0;
                            templateplantilla.Regular = 0;
                            templateplantilla.Bad = 0;
                            templateplantilla.Notes = "";
                            Templates.Add(templateplantilla);
                        }
                        else
                        {
                            headertemplate.Templates = Templates;
                            PlantillaHeader.Add(headertemplate);
                            Templates = new List<Templatesplantilla>();
                            headertemplate = new CredencialSpiderFleet.Models.Inspection.Headerandtemplate();
                            header = item.Encabezado;
                            headertemplate.Encabezado = header;
                            templateplantilla = new CredencialSpiderFleet.Models.Inspection.Templatesplantilla();
                            templateplantilla.idTemplate = item.idTemplate;
                            templateplantilla.template = item.Objeto;
                            templateplantilla.yes = 0;
                            templateplantilla.no = 0;
                            templateplantilla.Good = 0;
                            templateplantilla.Regular = 0;
                            templateplantilla.Bad = 0;
                            templateplantilla.Notes = "";
                            Templates.Add(templateplantilla);
                        }
                    }
                    count++;
                }
                headertemplate.Templates = Templates;
                PlantillaHeader.Add(headertemplate);
                response.PlantillaHeader = PlantillaHeader;
                response.success = true;
                return response;
            }
            catch(Exception ex)
            {
                response.success = false;
                return response;
            }
        }
    }
}