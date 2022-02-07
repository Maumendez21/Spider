using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Request.User
{
    /// <summary>
    /// ...
    /// </summary>
    public class UserRequest
    {
        public int IdUser { get; set; }
        public int IdCompany { get; set; }
        public int IdRole { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Telephone { get; set; }
        public string Hierarchy { get; set; }
        public int IdStatus { get; set; }
        public decimal Porcentage { get; set; }
    }
}