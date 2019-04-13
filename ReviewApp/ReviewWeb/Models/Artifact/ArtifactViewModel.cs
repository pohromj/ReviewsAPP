using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.Artifact
{
    public class ArtifactViewModel
    {
        public int Id { get; set; }
        public Dictionary<string, string> ArtifactProperties { get; set; }
    }
}
