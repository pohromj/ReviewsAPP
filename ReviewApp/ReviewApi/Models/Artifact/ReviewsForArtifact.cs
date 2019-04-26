using ReviewApi.Models.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewApi.Models.Artifact
{
    public class ReviewsForArtifact
    {
        public JazzArtifact Artifact { get; set; }
        public List<ReviewSetup> Reviews { get; set; }
    }
}
