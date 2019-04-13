using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.ReviewTameplateModels
{
    public class ReviewTameplateViewModel
    {
        public string ColumnName { get; set; }
        public List<string> Option { get; set; }
        public string Type { get; set; }
    }
}
