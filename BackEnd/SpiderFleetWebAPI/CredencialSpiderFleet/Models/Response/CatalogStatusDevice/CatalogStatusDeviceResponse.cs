using CredencialSpiderFleet.Models.Abstract;
using CredencialSpiderFleet.Models.Request.CatalogStatusDevice;
using System.Collections.Generic;

namespace CredencialSpiderFleet.Models.Response.CatalogStatusDevice
{
    public class CatalogStatusDeviceResponse : AbstractResponse
    {
        public List<CatalogStatusDeviceRequest> listStatus { get; set; }

        public CatalogStatusDeviceResponse()
        {
            listStatus = new List<CatalogStatusDeviceRequest>();
        }
    }
}