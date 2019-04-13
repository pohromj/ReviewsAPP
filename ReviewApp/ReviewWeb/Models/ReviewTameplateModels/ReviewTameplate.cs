using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.ReviewTameplateModels
{
    public class ReviewTameplate
    {
        public string Name { get; set; }
        public string Descritpion { get; set; }
        public List<ReviewTameplateViewModel> Model { get; set; }
        public List<ReviewHeaderModel> Header { get; set; }
        public List<string> Role { get; set; }
    }
}
