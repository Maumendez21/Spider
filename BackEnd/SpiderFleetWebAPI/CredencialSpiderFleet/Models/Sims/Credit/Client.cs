using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CredencialSpiderFleet.Models.Sims.Credit
{
    [XmlRoot(ElementName = "client")]
    public class Client
    {
        [XmlElement(ElementName = "balance")]
        public string Balance { get; set; }
        [XmlElement(ElementName = "amount")]
        public string Amount { get; set; }
        
        public Client()
        {
            Balance = string.Empty;
            Amount = string.Empty;
        }
    }
}