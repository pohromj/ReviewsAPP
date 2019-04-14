
using ReviewWeb.Models.ReviewTameplateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.ReviewModels
{
    public class FullReview
    {
        public Setup Setup { get; set; }
        public List<ParticipantToHeader> Participant { get; set; }
        public List<Header> HeadersRow { get; set; }
    }
}
