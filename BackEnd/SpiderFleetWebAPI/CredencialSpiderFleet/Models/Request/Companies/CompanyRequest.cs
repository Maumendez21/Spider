using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Request.Companies
{
    public class CompanyRequest
    {
        public int IdCompany { get; set; }
        public string Image { get; set; }
        public int IdSuscriptionType { get; set; }
        public string Name { get; set; }
        public string TaxId { get; set; }
        public string TaxName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal Porcentage { get; set; }
        public string Hierarchy { get; set; }
    }
}