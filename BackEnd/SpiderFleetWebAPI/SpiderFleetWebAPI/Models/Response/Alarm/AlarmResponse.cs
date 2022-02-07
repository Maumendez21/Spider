using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Alarm
{
    public class AlarmResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Alarm.Alarm> ListAlarms { get; set; }

        public AlarmResponse ()
        {
            ListAlarms = new List<CredencialSpiderFleet.Models.Alarm.Alarm>();
        }
    }
}