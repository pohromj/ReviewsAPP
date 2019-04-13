using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.ReviewModels
{
    public class ReviewViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CloseDate { get; set; }
        public int WorkProduct { get; set; }
        public int ProjectId { get; set; }
        public int Tameplate { get; set; }
    }
}
