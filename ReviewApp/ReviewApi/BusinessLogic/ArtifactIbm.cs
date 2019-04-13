using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewApi.BusinessLogic
{
    public class ArtifactIbm
    {
        public int Id { get; set; }
        public Dictionary<string, string> ArtifactProperties { get; set; }
        public ArtifactIbm()
        {
            this.ArtifactProperties = new Dictionary<string, string>();
        }

        public void AddProperty(string key, string value)
        {
            this.ArtifactProperties.Add(key, value);
        }
    }
}
