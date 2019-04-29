using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class ReviewRole
    {
        public ReviewRole()
        {
            UserReviewRole = new HashSet<UserReviewRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ReviewTameplateId { get; set; }
        public bool? Deleted { get; set; }

        public virtual ReviewTameplate ReviewTameplate { get; set; }
        public virtual ICollection<UserReviewRole> UserReviewRole { get; set; }
    }
}
