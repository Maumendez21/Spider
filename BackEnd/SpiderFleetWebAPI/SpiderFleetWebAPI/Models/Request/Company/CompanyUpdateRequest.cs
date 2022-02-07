
namespace SpiderFleetWebAPI.Models.Request.Company
{
    public class CompanyUpdateRequest 
    {
        //public string Image { get; set; }
        //public int? IdSuscriptionType { get; set; }
        public string Name { get; set; }
        public string TaxId { get; set; }
        public string TaxName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}