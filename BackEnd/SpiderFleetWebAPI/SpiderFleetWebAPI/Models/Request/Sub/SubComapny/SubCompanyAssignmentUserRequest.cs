using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Request.Sub.SubComapny
{
    public class SubCompanyAssignmentUserRequest
    {
        public string IdSubCompany { get; set; }
        public List<string> AssignmentUsers { get; set; }
    }
}