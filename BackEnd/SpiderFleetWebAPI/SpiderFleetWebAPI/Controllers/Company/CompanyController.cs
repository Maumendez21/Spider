using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Request.Company;
using SpiderFleetWebAPI.Models.Response.Company;
using SpiderFleetWebAPI.Utils.Company;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Data;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Company
{
    public class CompanyController : ApiController
    {
        //private UseFul use = new UseFul();
        private const string Tag = "Mantenimiento de Compañias";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/companies";
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
        /// Alta de Compañia
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de la Compañia
        /// #### Ejemplo de entrada
        /// ```
        /// {
        /// "idsuscriptiontype" : 1,
        /// "name" : "APASCO",
        /// "taxid" : "yfmfki85758",
        /// "taxname" : "Razon",
        /// "address" : "Calle",
        /// "telephone" : "4444",
        /// "email" : "apasco@gmail.com",
        /// "city" : "Venecia",
        /// "country" : "Italia"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "idsuscriptiontype" : "int"
        /// "name" : "string",
        /// "taxid" : "string",
        /// "taxname" : "string",
        /// "address" : "string",
        /// "telephone" : "string",
        /// "email" : "string",
        /// "city" : "string",
        /// "country" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="companyRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Company estructura similar a la tabla Companies</returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CompanyResponse))]
        public CompanyResponse Create([FromBody] CompanyRequest companyRequest)
        {
            CompanyResponse response = new CompanyResponse();

            try
            {

                if (!(companyRequest is CompanyRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {

                    //if (companyRequest.IdSuscriptionType == 0)
                    //{
                    //    response.success = false;
                    //    response.messages.Add("Seleccione un Rol");
                    //    return response;
                    //}

                    if (string.IsNullOrEmpty(companyRequest.Name.Trim()))
                    {
                        response.success = false;
                        response.messages.Add("Ingrese el Nombre de la Compañia");
                        return response;
                    }

                    if (string.IsNullOrEmpty(companyRequest.TaxId.Trim()))
                    {
                        response.success = false;
                        response.messages.Add("Ingrese el RFC");
                        return response;
                    }

                    if (string.IsNullOrEmpty(companyRequest.TaxName.Trim()))
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
                    response = IsValid(companyRequest);
                    if (!response.success) { return response; }
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                //string hierarchy = string.Empty;
                //DataRow row;
                //try
                //{
                //    hierarchy = (new CompanyDao()).ReadTaxId(companyRequest.TaxId);

                //    if (string.IsNullOrEmpty(hierarchy))
                //    {
                //        row = ds.Tables[0].Rows[0];

                //        if (companyRequest.TaxId.Equals(row["taxId"].ToString().Trim()))
                //        {
                //            response.success = false;
                //            response.messages.Add("Ya existe el RFC, ingrese otro por favor");
                //            return response;
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    response.success = false;
                //    response.messages.Add(ex.Message);
                //    return response;
                //}

                CredencialSpiderFleet.Models.Company.Company company = new CredencialSpiderFleet.Models.Company.Company();
                company.IdSuscriptionType = 0;// (int?)companyRequest.IdSuscriptionType;
                company.Name = companyRequest.Name;
                company.TaxId = companyRequest.TaxId;
                company.TaxName = companyRequest.TaxName;
                company.Address = companyRequest.Address;
                company.Telephone = companyRequest.Telephone;
                company.Email = companyRequest.Email;
                company.City = companyRequest.City;
                company.Country = companyRequest.Country;
                company.Hierarchy = "/1000000/";

                try
                {
                    string respuesta = (new CompanyDao()).Create(company);
                    if (respuesta.Contains("ERROR"))
                    {
                        response.success = false;
                        response.messages.Add("Error al intenar dar de alta el registro");
                        return response;
                    }
                    else if (respuesta.Contains("1"))
                    {
                        response.success = true;
                    }
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
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

        /// <summary>
        /// Actualizacion de Compañia
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de la Compañia
        /// #### Ejemplo de entrada
        /// ```
        /// {
        /// "idCompany" : 3,
        /// "idsuscriptiontype" : 1,
        /// "name" : "APASCO",
        /// "taxid" : "yfmfki85758",
        /// "taxname" : "Razon",
        /// "address" : "Calle",
        /// "telephone" : "4444",
        /// "email" : "apasco@gmail.com",
        /// "city" : "Puebla",
        /// "country" : "Mexico"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "idCompany" :¨"int",
        /// "idsuscriptiontype" : "int",
        /// "name" : "string",
        /// "taxid" : "string",
        /// "taxname" : "string",
        /// "address" : "string",
        /// "telephone" : "string",
        /// "email" : "string",
        /// "city" : "string",
        /// "country" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="companyRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Company estructura similar a la tabla Companies</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CompanyResponse))]
        public CompanyResponse Update([FromBody] CompanyUpdateRequest companyRequest)
        {
            CompanyResponse response = new CompanyResponse();
            try
            {
                if (!(companyRequest is CompanyUpdateRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                string username = string.Empty;
                try
                {
                    username = (new VerifyUser()).verifyTokenUser(User);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                CredencialSpiderFleet.Models.Company.Company company = new CredencialSpiderFleet.Models.Company.Company();
                company.IdCompany = username;
                //company.IdSuscriptionType = (int?)companyRequest.IdSuscriptionType;
                company.Name = companyRequest.Name;
                company.TaxId = companyRequest.TaxId;
                company.TaxName = companyRequest.TaxName;
                company.Address = companyRequest.Address;
                company.Telephone = companyRequest.Telephone;
                company.Email = companyRequest.Email;
                company.City = companyRequest.City;
                company.Country = companyRequest.Country;
                //company.Hierarchy = hierarchy;

                try
                {
                    response = (new CompanyDao()).Update(company);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
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

        /// <summary>
        /// Obtiene todos los registros de Compañia
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla Compañia
        /// <returns>Es una lista de Compañia estructura similar a la tabla Compañia</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CompanyResponse))]
        public CompanyListResponse GetList()
        {
            CompanyListResponse response = new CompanyListResponse();

            string username = string.Empty;
            try
            {
                username = (new VerifyUser()).verifyTokenUser(User);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            try
            {
                try
                {
                    response = (new CompanyDao()).Read(username);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
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

        /// <summary>
        /// Obtiene un registro por Id de la tabla Compañia
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// <param name="id">Id del registro a consultar</param>
        /// <returns>Es un objeto de Compañia estructura similar a la tabla Compañia</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CompanyRegistryResponse))]
        public CompanyRegistryResponse GetRegistryId([FromUri(Name = "id")] string id)
        {
            CompanyRegistryResponse response = new CompanyRegistryResponse();
            try
            {
                try
                {
                    response = (new CompanyDao()).ReadId(id);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
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

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list/company")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CompanyResponse))]
        public CompanyListLevelOneResponse GetListLevelOne()
        {
            CompanyListLevelOneResponse response = new CompanyListLevelOneResponse();

            string username = string.Empty;
            try
            {
                username = (new VerifyUser()).verifyTokenUser(User);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            try
            {
                try
                {
                    response = (new CompanyDao()).ReadLevelOne();
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
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


        ///// <summary>
        ///// Metodo que valida las longitudes de los campos y Validacion de caratcteres extraños
        ///// </summary>
        ///// <param name="companyRequest"></param>
        ///// <returns></returns>
        private CompanyResponse IsValid(CompanyRequest companyRequest)
        {
            CompanyResponse response = new CompanyResponse();

            if (!string.IsNullOrEmpty(companyRequest.Name.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(companyRequest.Name.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(companyRequest.Name.Trim(), longitudName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Nombre excede de lo establecido rango maximo " + longitudName + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(companyRequest.TaxId.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(companyRequest.TaxId.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(companyRequest.TaxId.Trim(), longitudTaxId))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo RFC excede de lo establecido rango maximo " + longitudTaxId + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(companyRequest.TaxName.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(companyRequest.TaxName.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(companyRequest.TaxName.Trim(), longitudTaxName))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Razon Social excede de lo establecido rango maximo " + longitudTaxName + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(companyRequest.Address.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(companyRequest.Address.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(companyRequest.Address.Trim(), longitudAddress))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Direccion excede de lo establecido rango maximo " + longitudAddress + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(companyRequest.Telephone.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(companyRequest.Telephone.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(companyRequest.Telephone.Trim(), longitudTelephone))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Telefono excede de lo establecido rango maximo " + longitudTelephone + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(companyRequest.Email.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (!useful.IsValidLength(companyRequest.Email.Trim(), longitudEmail))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Email excede de lo establecido rango maximo " + longitudEmail + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(companyRequest.City.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(companyRequest.City.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(companyRequest.City.Trim(), longitudCity))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Ciudad excede de lo establecido rango maximo " + longitudCity + " caracteres");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(companyRequest.Country.Trim()))
            {
                useful = new CredencialSpiderFleet.Models.Useful.UseFul();

                if (useful.hasSpecialChar(companyRequest.Country.Trim()))
                {
                    response.success = false;
                    response.messages.Add("La cadena contiene caracteres especiales");
                    return response;
                }

                if (!useful.IsValidLength(companyRequest.Country.Trim(), longitudCountry))
                {
                    response.success = false;
                    response.messages.Add("La longitud del campo Pais excede de lo establecido rango maximo " + longitudCountry + " caracteres");
                    return response;
                }
            }
            return response;
        }
    
    }
}
