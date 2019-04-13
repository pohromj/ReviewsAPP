using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class UserReview
    {
        public string UsersEmail { get; set; }
        public int ReviewId { get; set; }

        public virtual Review Review { get; set; }
        public virtual Users UsersEmailNavigation { get; set; }
    }
}
