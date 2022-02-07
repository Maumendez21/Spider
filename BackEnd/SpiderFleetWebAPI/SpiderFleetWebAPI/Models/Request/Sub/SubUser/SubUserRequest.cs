
namespace SpiderFleetWebAPI.Models.Request.Sub.SubUser
{
    public class SubUserRequest
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Telephone { get; set; }
        public string Grupo { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }
    }
}