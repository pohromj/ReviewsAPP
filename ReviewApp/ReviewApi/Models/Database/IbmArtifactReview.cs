using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class IbmArtifactReview
    {
        public int IbmArtifactId { get; set; }
        public int IbmArtifactIbmId { get; set; }
        public int ReviewId { get; set; }

        public virtual IbmArtifact IbmArtifactI { get; set; }
        public virtual Review Review { get; set; }
    }
}
