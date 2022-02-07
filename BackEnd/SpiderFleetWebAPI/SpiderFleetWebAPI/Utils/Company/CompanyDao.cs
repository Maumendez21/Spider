using CredencialSpiderFleet.Models.Connection;
using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.Company;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SpiderFleetWebAPI.Utils.Company
{
    /// <summary>
    /// 
    /// </summary>
    public class CompanyDao
    {
        public CompanyDao() { }

        private SqlConnection cn = new SqlConnection();
        private SqlHelper sql = new SqlHelper();

        private UseFul useful = new UseFul();
        private const int longitudName = 50;
        private const int longitudTaxId = 15;
        private const int longitudTaxName = 15;
        private const int longitudAddress = 200;
        private const int longitudTelephone = 15;
        private const int longitudEmail = 50;
        private const int longitudCity = 50;
        private const int longitudCountry = 50;

       /// <summary>
       /// Metodo que Crea la Empresa
       /// </summary>
       /// <param name="company"></param>
       /// <returns></returns>
        public string Create(CredencialSpiderFleet.Models.Company.Company company)
        {
            string respuesta = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_create_empresa_company", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (company.IdSuscriptionType != null) { cmd.Parameters.Add(new SqlParameter("@idsuscriptiontype", Convert.ToInt32(company.IdSuscriptionType))); }
                    else { cmd.Parameters.Add(new SqlParameter("@idsuscriptiontype", DBNull.Value)); }

                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(company.Name)));
                    
                    if (!string.IsNullOrEmpty(company.TaxId)) { cmd.Parameters.Add(new SqlParameter("@taxid", Convert.ToString(company.TaxId))); }
                    else { cmd.Parameters.Add(new SqlParameter("@taxid", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(company.TaxName)) { cmd.Parameters.Add(new SqlParameter("@taxname", Convert.ToString(company.TaxName))); }
                    else { cmd.Parameters.Add(new SqlParameter("@taxname", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(company.Address)) { cmd.Parameters.Add(new SqlParameter("@address", Convert.ToString(company.Address))); }
                    else { cmd.Parameters.Add(new SqlParameter("@address", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(company.Telephone)) { cmd.Parameters.Add(new SqlParameter("@telephone", Convert.ToString(company.Telephone))); }
                    else { cmd.Parameters.Add(new SqlParameter("@telephone", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(company.Email)) { cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(company.Email))); }
                    else { cmd.Parameters.Add(new SqlParameter("@email", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(company.City)) { cmd.Parameters.Add(new SqlParameter("@city", Convert.ToString(company.City))); }
                    else { cmd.Parameters.Add(new SqlParameter("@city", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(company.Country)) { cmd.Parameters.Add(new SqlParameter("@country", Convert.ToString(company.Country))); }
                    else { cmd.Parameters.Add(new SqlParameter("@country", DBNull.Value)); }

                    cmd.Parameters.Add(new SqlParameter("@porcentage", Convert.ToDecimal(company.Porcentage)));

                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@cMensaje";
                    sqlParameter.SqlDbType = SqlDbType.VarChar;
                    sqlParameter.Size = 60;
                    sqlParameter.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(sqlParameter);
                    cmd.ExecuteNonQuery();

                    respuesta = sqlParameter.Value.ToString();

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
            return respuesta;
        }

       /// <summary>
       /// Metodo que Actualiza los datos de la Empresa
       /// </summary>
       /// <param name="company"></param>
       /// <returns></returns>
       public CompanyResponse Update(CredencialSpiderFleet.Models.Company.Company company)
        {
            CompanyResponse response = new CompanyResponse();

            try
            {
                if (string.IsNullOrEmpty(company.IdCompany))
                {
                    response.success = false;
                    response.messages.Add("No tiene el parametro username");
                    return response;
                }
                
                if (string.IsNullOrEmpty(company.TaxId.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese el RFC");
                    return response;
                }

                if (string.IsNullOrEmpty(company.TaxName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("Ingrese la Razon Social");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            //Validaciones de longitud y Caracteres especiales
            try
            {
                response = IsValid(company);
                if (!response.success) 
                {
                    return response; 
                }
                response.success = false;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            try
            {
                string taxId = (new CompanyDao()).ReadTaxId(company.TaxId);

                if (!string.IsNullOrEmpty(taxId))
                {
                    if (!company.Hierarchy.Equals(taxId))
                    {
                        response.success = false;
                        response.messages.Add("Ya existe el RFC, ingrese otro por favor");
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

            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_empresa_company", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(company.IdCompany)));
                    
                    //if (company.IdSuscriptionType != null) { cmd.Parameters.Add(new SqlParameter("@idsuscriptiontype", Convert.ToInt32(company.IdSuscriptionType))); }
                    //else { cmd.Parameters.Add(new SqlParameter("@idsuscriptiontype", DBNull.Value)); }

                    cmd.Parameters.Add(new SqlParameter("@name", Convert.ToString(company.Name)));
                    cmd.Parameters.Add(new SqlParameter("@taxid", Convert.ToString(company.TaxId)));
                    cmd.Parameters.Add(new SqlParameter("@taxname", Convert.ToString(company.TaxName)));

                    if (!string.IsNullOrEmpty(company.Address)) { cmd.Parameters.Add(new SqlParameter("@address", Convert.ToString(company.Address))); }
                    else { cmd.Parameters.Add(new SqlParameter("@address", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(company.Telephone)) { cmd.Parameters.Add(new SqlParameter("@telephone", Convert.ToString(company.Telephone))); }
                    else { cmd.Parameters.Add(new SqlParameter("@telephone", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(company.Email)) { cmd.Parameters.Add(new SqlParameter("@email", Convert.ToString(company.Email))); }
                    else { cmd.Parameters.Add(new SqlParameter("@email", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(company.City)) { cmd.Parameters.Add(new SqlParameter("@city", Convert.ToString(company.City))); }
                    else { cmd.Parameters.Add(new SqlParameter("@city", DBNull.Value)); }

                    if (!string.IsNullOrEmpty(company.Country)) { cmd.Parameters.Add(new SqlParameter("@country", Convert.ToString(company.Country))); }
                    else { cmd.Parameters.Add(new SqlParameter("@country", DBNull.Value)); }

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
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }
            return response;
        }

        /// <summary>
        /// Medodo que lee los datos de la empresa 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public CompanyListResponse Read(string username)
        {
            CompanyListResponse response = new CompanyListResponse();
            List<CredencialSpiderFleet.Models.Company.Company> listCompany = new List<CredencialSpiderFleet.Models.Company.Company>();
            CredencialSpiderFleet.Models.Company.Company company = new CredencialSpiderFleet.Models.Company.Company();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_company", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            company = new CredencialSpiderFleet.Models.Company.Company();
                            company.IdImagen = Convert.ToInt32(reader["idImg"]);
                            company.Image = Convert.ToString(reader["image"]);
                            company.IdSuscriptionType = (reader["idSuscriptionType"] == DBNull.Value) ? (int?)null : ((int)reader["idSuscriptionType"]);
                            company.Description = Convert.ToString(reader["description"]);
                            company.Name = Convert.ToString(reader["Nombre"]);
                            company.TaxId = Convert.ToString(reader["taxId"]);
                            company.TaxName = Convert.ToString(reader["taxName"]);
                            company.Address = (reader["address"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["address"]));
                            company.Telephone = (reader["telephone"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["telephone"]));
                            company.Email = (reader["email"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["email"]));
                            company.City = (reader["city"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["city"]));
                            company.Country = (reader["country"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["country"]));
                            company.Hierarchy = Convert.ToString(reader["hierarchy"]);
                            company.IdCompany = Convert.ToString(reader["hierarchy"]);
                            company.Porcentage = 0;//(reader["country"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["country"]));

                            listCompany.Add(company);
                        }
                        reader.Close();
                        response.success = true;
                        response.listCompany = listCompany;
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

        /// <summary>
        /// Metodo que devuelve un registro por id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyRegistryResponse ReadId(string id)
        {
            CompanyRegistryResponse response = new CompanyRegistryResponse();
            CredencialSpiderFleet.Models.Company.Company company = new CredencialSpiderFleet.Models.Company.Company();

            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_id_company", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id", Convert.ToString(id)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            company = new CredencialSpiderFleet.Models.Company.Company();
                            company.IdImagen = Convert.ToInt32(reader["idImg"]);
                            company.Image = Convert.ToString(reader["image"]);
                            company.IdSuscriptionType = (reader["idSuscriptionType"] == DBNull.Value) ? (int?)null : ((int)reader["idSuscriptionType"]);
                            company.Description = Convert.ToString(reader["description"]);
                            company.Name = Convert.ToString(reader["Nombre"]);
                            company.TaxId = Convert.ToString(reader["taxId"]);
                            company.TaxName = Convert.ToString(reader["taxName"]);
                            company.Address = (reader["address"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["address"]));
                            company.Telephone = (reader["telephone"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["telephone"]));
                            company.Email = (reader["email"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["email"]));
                            company.City = (reader["city"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["city"]));
                            company.Country = (reader["country"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["country"]));
                            company.Hierarchy = Convert.ToString(reader["hierarchy"]);

                        }
                        reader.Close();
                        response.company = company;
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

        /// <summary>
        /// Metodo que devuelve la jerarquia por medio del RFC
        /// </summary>
        /// <param name="taxid"></param>
        /// <returns></returns>
        private string ReadTaxId(string taxid)
        {
            DataSet dsConsulta = new DataSet();
            string hierarchy = string.Empty;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_taxid_company", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@taxid", Convert.ToString(taxid)));
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            hierarchy = Convert.ToString(reader["ID_user"]);
                        }
                        reader.Close();
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
            return hierarchy;
        }


        /// <summary>
        /// Medodo que lee las empresas de nivel uno
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public CompanyListLevelOneResponse ReadLevelOne()
        {
            CompanyListLevelOneResponse response = new CompanyListLevelOneResponse();
            List<CredencialSpiderFleet.Models.Company.CompanyList> listCompany = new List<CredencialSpiderFleet.Models.Company.CompanyList>();
            CredencialSpiderFleet.Models.Company.CompanyList company = new CredencialSpiderFleet.Models.Company.CompanyList();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_company_one_nivel", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            company = new CredencialSpiderFleet.Models.Company.CompanyList();
                            company.Name = Convert.ToString(reader["Nombre"]);
                            company.Hierarchy = Convert.ToString(reader["hierarchy"]);

                            listCompany.Add(company);
                        }
                        reader.Close();
                        response.success = true;
                        response.listCompany = listCompany;
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


        /// <summary>
        /// Medodo que lee los datos de la empresa 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public CompanyListResponse ReadCompanySubCompany(string username)
        {
            CompanyListResponse response = new CompanyListResponse();
            List<CredencialSpiderFleet.Models.Company.Company> listCompany = new List<CredencialSpiderFleet.Models.Company.Company>();
            CredencialSpiderFleet.Models.Company.Company company = new CredencialSpiderFleet.Models.Company.Company();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_list_company_subcompany", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@username", Convert.ToString(username)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            company = new CredencialSpiderFleet.Models.Company.Company();
                            company.IdImagen = Convert.ToInt32(reader["idImg"]);
                            company.Image = Convert.ToString(reader["image"]);
                            company.IdSuscriptionType = (reader["idSuscriptionType"] == DBNull.Value) ? (int?)null : ((int)reader["idSuscriptionType"]);
                            company.Description = Convert.ToString(reader["description"]);
                            company.Name = Convert.ToString(reader["Nombre"]);
                            company.TaxId = Convert.ToString(reader["taxId"]);
                            company.TaxName = Convert.ToString(reader["taxName"]);
                            company.Address = (reader["address"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["address"]));
                            company.Telephone = (reader["telephone"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["telephone"]));
                            company.Email = (reader["email"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["email"]));
                            company.City = (reader["city"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["city"]));
                            company.Country = (reader["country"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["country"]));
                            company.Hierarchy = Convert.ToString(reader["hierarchy"]);
                            company.IdCompany = Convert.ToString(reader["hierarchy"]);
                            company.Porcentage = 0;//(reader["country"] == DBNull.Value) ? string.Empty : (Convert.ToString(reader["country"]));

                            listCompany.Add(company);
                        }
                        reader.Close();
                        response.success = true;
                        response.listCompany = listCompany;
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


        /// <summary>
        /// Metodo que valida las longitudes de los campos y Validacion de caratcteres extraños
        /// </summary>
        /// <param name="companyRequest"></param>
        /// <returns></returns>
        private CompanyResponse IsValid(CredencialSpiderFleet.Models.Company.Company company)
        {
            CompanyResponse response = new CompanyResponse();

            //Campo Nombre
            if (!string.IsNullOrEmpty(company.Name.Trim()))
            {
                useful = new UseFul();

                if (useful.hasSpecialChar(company.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(company.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Nombre excede de lo establecido rango maximo " + longitudName + " caracteres");
                    return response;
                }
            }

            //Campo RFC
            if (!string.IsNullOrEmpty(company.TaxId.Trim()))
            {
                useful = new UseFul();

                if (useful.hasSpecialChar(company.TaxId.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(company.TaxId.Trim(), longitudTaxId))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo RFC excede de lo establecido rango maximo " + longitudTaxId + " caracteres");
                    return response;
                }
            }

            //Campo Razon Social
            if (!string.IsNullOrEmpty(company.TaxName.Trim()))
            {
                useful = new UseFul();

                if (useful.hasSpecialChar(company.TaxName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(company.TaxName.Trim(), longitudTaxName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Razon Social excede de lo establecido rango maximo " + longitudTaxName + " caracteres");
                    return response;
                }
            }

            //Campo Direccion
            if (!string.IsNullOrEmpty(company.Address.Trim()))
            {
                useful = new UseFul();

                if (useful.hasSpecialChar(company.Address.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(company.Address.Trim(), longitudAddress))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Direccion excede de lo establecido rango maximo " + longitudAddress + " caracteres");
                    return response;
                }
            }

            //Campo Telefono
            if (!string.IsNullOrEmpty(company.Telephone.Trim()))
            {
                useful = new UseFul();

                if (useful.hasSpecialChar(company.Telephone.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(company.Telephone.Trim(), longitudTelephone))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Telefono excede de lo establecido rango maximo " + longitudTelephone + " caracteres");
                    return response;
                }
            }

            //Campo Email
            if (!string.IsNullOrEmpty(company.Email.Trim()))
            {
                useful = new UseFul();

                if (!useful.IsValidLength(company.Email.Trim(), longitudEmail))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Email excede de lo establecido rango maximo " + longitudEmail + " caracteres");
                    return response;
                }
            }

            //Campo Ciudad
            if (!string.IsNullOrEmpty(company.City.Trim()))
            {
                useful = new UseFul();

                if (useful.hasSpecialChar(company.City.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(company.City.Trim(), longitudCity))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Ciudad excede de lo establecido rango maximo " + longitudCity + " caracteres");
                    return response;
                }
            }

            //Campo Pais
            if (!string.IsNullOrEmpty(company.Country.Trim()))
            {
                useful = new UseFul();

                if (useful.hasSpecialChar(company.Country.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(company.Country.Trim(), longitudCountry))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Pais excede de lo establecido rango maximo " + longitudCountry + " caracteres");
                    return response;
                }
            }
            response.success = true;
            return response;
        }

        public AdministrationCompanyResponse UpdateStatus(CredencialSpiderFleet.Models.Company.AdministrationCompany company)
        {
            AdministrationCompanyResponse response = new AdministrationCompanyResponse();

            try
            {
                if (string.IsNullOrEmpty(company.Node))
                {
                    response.success = false;
                    response.messages.Add("No tiene el parametro de la Empresa");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            int respuesta = 0;
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_update_status_by_company", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(company.Node)));
                    cmd.Parameters.Add(new SqlParameter("@activo", Convert.ToString(company.Active)));

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
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }
            return response;
        }
        
        public AdministrationCompanyRegistryResponse ReadCompanyNode(string node)
        {
            AdministrationCompanyRegistryResponse response = new AdministrationCompanyRegistryResponse();
            CredencialSpiderFleet.Models.Company.AdministrationCompanyRegistry company = new CredencialSpiderFleet.Models.Company.AdministrationCompanyRegistry();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_status_company_by_node", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@node", Convert.ToString(node)));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            company = new CredencialSpiderFleet.Models.Company.AdministrationCompanyRegistry();
                            
                            company.Node = Convert.ToString(reader["Node"]);                            
                            company.Name = Convert.ToString(reader["Nombre"]);
                            company.Status = Convert.ToInt32(reader["status"]);
                        }
                        reader.Close();
                        response.success = true;
                        response.registry = company;
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

        public AdministrationCompanyListResponse ReadListCompanyNode()
        {
            AdministrationCompanyListResponse response = new AdministrationCompanyListResponse();
            List<CredencialSpiderFleet.Models.Company.AdministrationCompanyRegistry> listCompany = new List<CredencialSpiderFleet.Models.Company.AdministrationCompanyRegistry>();
            CredencialSpiderFleet.Models.Company.AdministrationCompanyRegistry company = new CredencialSpiderFleet.Models.Company.AdministrationCompanyRegistry();
            try
            {
                if (sql.IsConnection)
                {
                    cn = sql.Connection();
                    SqlCommand cmd = new SqlCommand("ad.sp_consult_status_company", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            company = new CredencialSpiderFleet.Models.Company.AdministrationCompanyRegistry();

                            company.Node = Convert.ToString(reader["Node"]);
                            company.Name = Convert.ToString(reader["Nombre"]);
                            company.Status = Convert.ToInt32(reader["status"]);

                            listCompany.Add(company);
                        }
                        reader.Close();
                        response.success = true;
                        response.ListCompany = listCompany;
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