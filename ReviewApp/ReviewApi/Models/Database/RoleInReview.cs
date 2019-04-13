using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class RoleInReview
    {
        public int ReviewRoleId { get; set; }
        public int ReviewId { get; set; }

        public virtual Review Review { get; set; }
        public virtual ReviewRole ReviewRole { get; set; }
    }
}
