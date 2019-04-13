using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class ArtifactDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DetailValue { get; set; }
        public int ArtifactId { get; set; }
        public int ArtifactWorkproductId { get; set; }
        public int ArtifactWorkproductProjectId { get; set; }

        public virtual Artifact Artifact { get; set; }
    }
}
