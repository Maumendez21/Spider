using CredencialSpiderFleet.Models.Abstract;
using CredencialSpiderFleet.Models.Request.TypeDevices;
using System.Collections.Generic;

namespace CredencialSpiderFleet.Models.Response.TypeDevices
{
    public class TypeDevicesResponse : AbstractResponse
    {
        public List<TypeDevicesRequest> listType { get; set; }
       
        public TypeDevicesResponse()
        {
            listType = new List<TypeDevicesRequest>();
        }
    }
}