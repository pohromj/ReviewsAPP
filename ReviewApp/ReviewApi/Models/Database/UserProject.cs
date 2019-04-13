using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class UserProject
    {
        public string UsersEmail { get; set; }
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
        public virtual Users UsersEmailNavigation { get; set; }
    }
}
