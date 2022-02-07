using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Diary
{
    public class DiaryUpdateRequest
    {
        public int IdStart { get; set; }
        public DateTime StartDate { get; set; }
        public int IdEnd { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
        public string Device { get; set; }
        public int Responsable { get; set; }
    }
}