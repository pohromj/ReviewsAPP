using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class Users
    {
        public Users()
        {
            Project = new HashSet<Project>();
            ReviewTameplate = new HashSet<ReviewTameplate>();
            UserProject = new HashSet<UserProject>();
            UserReview = new HashSet<UserReview>();
            UserReviewRole = new HashSet<UserReviewRole>();
            Workproduct = new HashSet<Workproduct>();
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Salt { get; set; }
        public int SystemRoleId { get; set; }

        public virtual SystemRole SystemRole { get; set; }
        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<ReviewTameplate> ReviewTameplate { get; set; }
        public virtual ICollection<UserProject> UserProject { get; set; }
        public virtual ICollection<UserReview> UserReview { get; set; }
        public virtual ICollection<UserReviewRole> UserReviewRole { get; set; }
        public virtual ICollection<Workproduct> Workproduct { get; set; }
    }
}
