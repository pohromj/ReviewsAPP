using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.ReviewModels
{
    public class ReviewProgress
    {
        public string Html { get; set; }
        List<HeaderData> HeaderDatas { get; set; }
    }
}
