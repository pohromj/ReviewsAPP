using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.ReviewModels
{
    public class ReviewSetup
    {
        public string Name { get; set; }
        public string Descrption { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int WorkProduct { get; set; }
        public int Tameplate { get; set; }
        public int Project { get; set; }
        public string Html { get; set; }
    }
}
