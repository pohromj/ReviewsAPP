using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.ReviewModels
{
    public class FullReviewSetup
    {
        public ReviewSetup Setup { get; set; }
        public List<Participant> Participant { get; set; }
        public List<int> Artifact { get; set; }

    }
}
