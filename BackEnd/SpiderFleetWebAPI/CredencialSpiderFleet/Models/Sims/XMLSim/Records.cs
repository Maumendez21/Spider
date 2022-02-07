using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CredencialSpiderFleet.Models.Sims.XMLSim
{
    [XmlRoot(ElementName = "records")]
    public class Records
    {
        [XmlElement(ElementName = "card")]
        public Card Card { get; set; }
    }
}