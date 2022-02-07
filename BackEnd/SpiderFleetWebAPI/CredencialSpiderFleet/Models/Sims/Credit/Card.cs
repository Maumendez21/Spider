using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CredencialSpiderFleet.Models.Sims.Credit
{
    [XmlRoot(ElementName = "card")]
    public class Card
    {
        [XmlElement(ElementName = "balance")]
        public string Balance { get; set; }
        [XmlElement(ElementName = "amount")]
        public string Amount { get; set; }

        public Card()
        {
            Balance = string.Empty;
            Amount = string.Empty;
        }
    }
}