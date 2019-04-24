using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.ReviewModels
{
    public class ReviewProgress
    {
        public int ReviewId { get; set; }
        public string Html { get; set; }
        public List<HeaderData> HeaderDatas { get; set; }
    }
}
