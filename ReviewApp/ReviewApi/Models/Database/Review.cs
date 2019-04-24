using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class Review
    {
        public Review()
        {
            Artifact = new HashSet<Artifact>();
            ArtifactReview = new HashSet<ArtifactReview>();
            HeaderRowData = new HashSet<HeaderRowData>();
            IbmArtifact = new HashSet<IbmArtifact>();
            IbmArtifactReview = new HashSet<IbmArtifactReview>();
            UserReviewRole = new HashSet<UserReviewRole>();
        }

        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CloseDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WorkproductId { get; set; }
        public int ReviewTameplateId { get; set; }
        public int WorkproductProjectId { get; set; }
        public string Html { get; set; }

        public virtual ReviewTameplate ReviewTameplate { get; set; }
        public virtual Workproduct Workproduct { get; set; }
        public virtual ICollection<Artifact> Artifact { get; set; }
        public virtual ICollection<ArtifactReview> ArtifactReview { get; set; }
        public virtual ICollection<HeaderRowData> HeaderRowData { get; set; }
        public virtual ICollection<IbmArtifact> IbmArtifact { get; set; }
        public virtual ICollection<IbmArtifactReview> IbmArtifactReview { get; set; }
        public virtual ICollection<UserReviewRole> UserReviewRole { get; set; }
    }
}
