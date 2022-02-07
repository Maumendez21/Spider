using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.CardGraphics
{
    public class CardGraphics
    {
        public List<string> data { get; set; }
        public List<string> label { get; set; }

        public CardGraphics()
        {
            data = new List<string>();
            label = new List<string>();
        }
    }
}