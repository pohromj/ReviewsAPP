using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class ArtifactReview
    {
        public int ArtifactId { get; set; }
        public int ArtifactWorkproductId { get; set; }
        public int ReviewId { get; set; }

        public virtual Artifact Artifact { get; set; }
        public virtual Review Review { get; set; }
    }
}
