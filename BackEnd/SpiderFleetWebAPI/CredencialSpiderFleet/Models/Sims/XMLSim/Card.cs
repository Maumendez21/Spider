using System.Xml.Serialization;

namespace CredencialSpiderFleet.Models.Sims.XMLSim
{
    [XmlRoot(ElementName = "card")]
    public class Card
    {
        [XmlElement(ElementName = "tsimid")]
        public string Tsimid { get; set; }
        [XmlElement(ElementName = "aserviceid")]
        public string Aserviceid { get; set; }
        [XmlElement(ElementName = "inum")]
        public string Inum { get; set; }
        [XmlElement(ElementName = "onum")]
        public string Onum { get; set; }
        [XmlElement(ElementName = "prepayed")]
        public string Prepayed { get; set; }
        [XmlElement(ElementName = "blocked")]
        public string Blocked { get; set; }
        [XmlElement(ElementName = "balance")]
        public string Balance { get; set; }
        [XmlElement(ElementName = "curr")]
        public string Curr { get; set; }
    }
}