using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Operator
{
    public class OperatorRegistry
    {
        public int Id { get; set; }
        public string Device { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Location { get; set; }
        public int IdImg { get; set; }
        public string Image { get; set; }
    }
}