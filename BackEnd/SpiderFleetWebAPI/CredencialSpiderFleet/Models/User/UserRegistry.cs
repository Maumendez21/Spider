
namespace CredencialSpiderFleet.Models.User
{
    public class UserRegistry
    {
        public string IdUser { get; set; }
        public string DescripcionRole { get; set; }
        public int IdRole { get; set; }
        public int IdImage { get; set; }
        public string Image { get; set; }
        public int IdStatus { get; set; }
        public string DescriptionStatus { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }        
        public string Telephone { get; set; }
        public string Hierarchy { get; set; }        
        public decimal Porcentage { get; set; }
    }
}