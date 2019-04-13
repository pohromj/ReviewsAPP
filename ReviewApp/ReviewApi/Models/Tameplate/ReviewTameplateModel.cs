using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewApi.Models.Tameplate
{
    public class ReviewTameplateModel
    {
        public string Name { get; set; }
        public string Descritpion { get; set; }

        public List<ReviewTameplateViewModel> Model { get; set; }
        public List<ReviewHeader> Header { get; set; }
        public List<string> Role { get; set; }
    }
}
