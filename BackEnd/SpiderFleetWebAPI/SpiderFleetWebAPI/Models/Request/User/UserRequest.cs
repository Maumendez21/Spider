
namespace SpiderFleetWebAPI.Models.Request.User
{
    public class UserRequest
    {
        //public int IdCompany { get; set; }
        //public int IdRole { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Telephone { get; set; }
        //public int IdStatus { get; set; }
        public string CompanyName { get; set; }
        public string TaxId { get; set; }
        public string TaxName { get; set; }
        public int Type { get; set; }
    }
}