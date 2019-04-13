using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class Project
    {
        public Project()
        {
            UserProject = new HashSet<UserProject>();
            Workproduct = new HashSet<Workproduct>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProjectTypeId { get; set; }
        public string UsersEmail { get; set; }

        public virtual ProjectType ProjectType { get; set; }
        public virtual Users UsersEmailNavigation { get; set; }
        public virtual ICollection<UserProject> UserProject { get; set; }
        public virtual ICollection<Workproduct> Workproduct { get; set; }
    }
}
