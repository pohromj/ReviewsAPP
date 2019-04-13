using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class ProjectType
    {
        public ProjectType()
        {
            Project = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Project> Project { get; set; }
    }
}
