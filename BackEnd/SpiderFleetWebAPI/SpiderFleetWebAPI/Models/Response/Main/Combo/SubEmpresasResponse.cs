using CredencialSpiderFleet.Models.Main.Combo;
using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.Main.Combo
{
    public class SubEmpresasResponse : BasicResponse
    {
        public List<SubEmpresa> listSubEmpresas { get; set; }

        public SubEmpresasResponse()
        {
            listSubEmpresas = new List<SubEmpresa>();
        }
    }
}