using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class Workproduct
    {
        public Workproduct()
        {
            Artifact = new HashSet<Artifact>();
            IbmArtifact = new HashSet<IbmArtifact>();
            Review = new HashSet<Review>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ArtifactsUrl { get; set; }
        public int ProjectId { get; set; }
        public string UsersEmail { get; set; }

        public virtual Project Project { get; set; }
        public virtual Users UsersEmailNavigation { get; set; }
        public virtual ICollection<Artifact> Artifact { get; set; }
        public virtual ICollection<IbmArtifact> IbmArtifact { get; set; }
        public virtual ICollection<Review> Review { get; set; }
    }
}
