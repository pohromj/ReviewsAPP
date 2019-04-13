using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewApi.Models.Artifact
{
    public class JazzArtifact
    {
        public int Id { get; set; }
        public int IbmId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
