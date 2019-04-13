using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class UserReviewRole
    {
        public int ReviewRoleId { get; set; }
        public string UsersEmail { get; set; }

        public virtual ReviewRole ReviewRole { get; set; }
        public virtual Users UsersEmailNavigation { get; set; }
    }
}
