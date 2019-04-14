using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class Artifact
    {
        public Artifact()
        {
            ArtifactDetail = new HashSet<ArtifactDetail>();
            ArtifactReview = new HashSet<ArtifactReview>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int WorkproductId { get; set; }
        public int WorkproductProjectId { get; set; }
        public int? ReviewId { get; set; }
        public int WorkproductProjectId1 { get; set; }

        public virtual Review Review { get; set; }
        public virtual Workproduct Workproduct { get; set; }
        public virtual ICollection<ArtifactDetail> ArtifactDetail { get; set; }
        public virtual ICollection<ArtifactReview> ArtifactReview { get; set; }
    }
}
