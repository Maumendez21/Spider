using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CredencialSpiderFleet.Models.Sims.XMLSim
{
    [XmlRoot(ElementName = "account")]
    public class Account
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "active")]
        public string Active { get; set; }
        [XmlElement(ElementName = "expire")]
        public string Expire { get; set; }
        [XmlElement(ElementName = "balance")]
        public string Balance { get; set; }
        [XmlElement(ElementName = "currency")]
        public string Currency { get; set; }
        [XmlElement(ElementName = "orderid")]
        public string Orderid { get; set; }
    }
}