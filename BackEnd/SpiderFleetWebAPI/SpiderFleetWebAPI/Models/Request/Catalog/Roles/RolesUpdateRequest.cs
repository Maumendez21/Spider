
namespace SpiderFleetWebAPI.Models.Request.Catalog.Roles
{
    public class RolesUpdateRequest: RolesRequest
    {
        public int IdRole { get; set; }
        public int Status { get; set; }
    }
}