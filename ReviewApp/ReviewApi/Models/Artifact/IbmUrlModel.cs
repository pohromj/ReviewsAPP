using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewApi.Models.Artifact
{
    public class IbmUrlModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public int ProjectId { get; set; }
        public int WorkProductId { get; set; }
    }
}
