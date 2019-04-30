using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class ReviewTameplate
    {
        public ReviewTameplate()
        {
            HeaderRow = new HashSet<HeaderRow>();
            Review = new HashSet<Review>();
            ReviewColumn = new HashSet<ReviewColumn>();
            ReviewRole = new HashSet<ReviewRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UsersEmail { get; set; }
        public bool? Deleted { get; set; }

        public virtual Users UsersEmailNavigation { get; set; }
        public virtual ICollection<HeaderRow> HeaderRow { get; set; }
        public virtual ICollection<Review> Review { get; set; }
        public virtual ICollection<ReviewColumn> ReviewColumn { get; set; }
        public virtual ICollection<ReviewRole> ReviewRole { get; set; }
    }
}
