using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Configuration
{
    public class Enumeracion { }

    public enum msisdns
    {
        tsimid,
        aserviceid,
        inum,
        onum,
        prepayed,
        blocked,
        balance,
        curr
    }

    public enum statussims
    {
        disable,
        available,
        assigned
    }
}