using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class UserReviewRole
    {
        public string UsersEmail { get; set; }
        public int ReviewId { get; set; }
        public int ReviewRoleId { get; set; }

        public virtual Review Review { get; set; }
        public virtual ReviewRole ReviewRole { get; set; }
        public virtual Users UsersEmailNavigation { get; set; }
    }
}
