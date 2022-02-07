using CredencialSpiderFleet.Models.Abstract;
using CredencialSpiderFleet.Models.Request.Sim;
using System.Collections.Generic;

namespace CredencialSpiderFleet.Models.Response.Sim
{
    public class SimResponse : AbstractResponse
    {
        public List<SimRequest> listSims { get; set; }
        public SimResponse() 
        {
            listSims = new List<SimRequest>();
        }
    }
}
