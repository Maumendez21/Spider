using CredencialSpiderFleet.Models.Main.Combo;
using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.Main.Combo
{
    public class EmpresaResponse : BasicResponse
    {
        public List<Empresa> listEmpresas { get; set; }

        public EmpresaResponse()
        {
            listEmpresas = new List<Empresa>();
        }
    }
}