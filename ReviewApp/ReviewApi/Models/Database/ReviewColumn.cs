using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class ReviewColumn
    {
        public ReviewColumn()
        {
            HeaderRow = new HashSet<HeaderRow>();
            ReviewColumnTypeEnum = new HashSet<ReviewColumnTypeEnum>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int ReviewTameplateId { get; set; }

        public virtual ReviewTameplate ReviewTameplate { get; set; }
        public virtual ICollection<HeaderRow> HeaderRow { get; set; }
        public virtual ICollection<ReviewColumnTypeEnum> ReviewColumnTypeEnum { get; set; }
    }
}
