using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Diary
{
    public class DiaryDeleteRequest
    {
        public int IdStart { get; set; }
        public int IdEnd { get; set; }
    }
}