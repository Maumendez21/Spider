
namespace SpiderFleetWebAPI.Models.Request.Inventory.Obd
{
    public class ObdRequest
    {
        public string IdDevice { get; set; }
        public string Label { get; set; }
        public int? IdCompany { get; set; }
        public int IdType { get; set; }
        public int? IdSim { get; set; }
        //public int IdStatus { get; set; }
    }
}