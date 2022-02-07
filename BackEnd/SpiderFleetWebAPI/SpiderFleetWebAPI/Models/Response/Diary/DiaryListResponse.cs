using CredencialSpiderFleet.Models.Diary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Diary
{
    public class DiaryListResponse: BasicResponse
    {
        public List<DiaryResgitrys> ListEvents { get; set; }

        public DiaryListResponse()
        {
            ListEvents = new List<DiaryResgitrys>();
        }
    }
}