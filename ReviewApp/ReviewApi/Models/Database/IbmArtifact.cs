using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class IbmArtifact
    {
        public IbmArtifact()
        {
            IbmArtifactReview = new HashSet<IbmArtifactReview>();
        }

        public int Id { get; set; }
        public int IbmId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int WorkproductId { get; set; }
        public int? ReviewId { get; set; }
        public int WorkproductProjectId { get; set; }

        public virtual Review Review { get; set; }
        public virtual Workproduct Workproduct { get; set; }
        public virtual ICollection<IbmArtifactReview> IbmArtifactReview { get; set; }
    }
}
