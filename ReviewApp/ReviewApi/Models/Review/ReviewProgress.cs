using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewApi.Models.Review
{
    public class ReviewProgress
    {
        public string Html { get; set; }
        List<HeaderData> HeaderDatas { get; set; }
    }
}
