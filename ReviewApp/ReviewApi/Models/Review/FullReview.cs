using ReviewApi.Models.Tameplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewApi.Models.Review
{
    public class FullReview
    {
        public Setup Setup { get; set; }
        public List<ParticipantToHeader> Participant { get; set; }
        public List<Header> HeadersRow { get; set; }
    }
}
