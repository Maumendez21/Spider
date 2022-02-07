using CredencialSpiderFleet.Models.Configuration;
using CredencialSpiderFleet.Models.RevGeoCode;
using CredencialSpiderFleet.Models.Useful;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SpiderFleetWebAPI.Utils.RevGeoCode
{
    public class AddressCode
    {
        private VariableConfiguration configuration = new VariableConfiguration();
        public string RecoverAddress(string latitude, string longitude)
        {
            string address = string.Empty;
            try
            {
                //https://revgeocode.search.hereapi.com/v1/revgeocode?at=17.991233,-94.553480&apikey=NiCOtxSPTBwOf7MK6kuqO-Pjidrv57KgyIbQnjdA6U4
                var url = "https://revgeocode.search.hereapi.com/v1/revgeocode?at=" + latitude + "," + longitude + "&apikey=" + configuration.revgeocode;
                var result = new WebClient().DownloadString(url);

                Root test = JsonConvert.DeserializeObject<Root>(result);
                var direccion = UseFul.ToUTF8(test.items[0].address.label.ToString());

                return direccion;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}