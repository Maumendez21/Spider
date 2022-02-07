using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Sub.SubUser
{
    public class SubUserRegistry
    {
        //public int IdCompany { get; set; }
        public string UserName { get; set; }
        public string Hierarchy { get; set; }
        public int IdRole { get; set; }
        public string DescripcionRol { get; set; }
        public int IdImage { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public decimal Porcentage { get; set; }
        public int IdStatus { get; set; }
        public string DescripcionStatus { get; set; }
        public string Grupo { get; set; }
        public string Node { get; set; }
    }
}