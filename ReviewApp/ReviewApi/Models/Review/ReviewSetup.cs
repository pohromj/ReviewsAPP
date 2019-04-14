using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewApi.Models.Review
{
    public class ReviewSetup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int WorkProduct { get; set; }
        public int Tameplate { get; set; }
        public int Project { get; set; }
        public string Html { get; set; }
    }
}
