using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.General
{
    public class DeviceResponse : BasicResponse
    {
        public List<string> ListDevice { get; set; }
        public string Grupo { get; set; }
        public DeviceResponse() 
        {
            ListDevice = new List<string>();
        }
    
    }
}