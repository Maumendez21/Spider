
namespace SpiderFleetWebAPI.Models.Request.User
{
    public class UserUpdateRequest 
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
    }
}