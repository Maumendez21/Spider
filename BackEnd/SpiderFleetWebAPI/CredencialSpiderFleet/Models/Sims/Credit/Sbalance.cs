

using System.Xml.Serialization;

namespace CredencialSpiderFleet.Models.Sims.Credit
{
    [XmlRoot(ElementName = "sbalance")]
    public class Sbalance
    {
        [XmlElement(ElementName = "aserviceid")]
        public string Aserviceid { get; set; }
        [XmlElement(ElementName = "inum")]
        public string Inum { get; set; }
        [XmlElement(ElementName = "onum")]
        public string Onum { get; set; }
        [XmlElement(ElementName = "amount")]
        public string Amount { get; set; }
        [XmlElement(ElementName = "orderid")]
        public string Orderid { get; set; }
        [XmlElement(ElementName = "card")]
        public Card Card { get; set; }
        [XmlElement(ElementName = "client")]
        public Client Client { get; set; }

        public Sbalance()
        {
            Aserviceid = string.Empty;
            Inum = string.Empty;
            Onum = string.Empty;
            Amount = string.Empty;
            Orderid = string.Empty;
            Card = new Card();
            Client = new Client();
        }
    }
}