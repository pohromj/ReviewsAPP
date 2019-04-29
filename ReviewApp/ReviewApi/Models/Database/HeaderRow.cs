using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class HeaderRow
    {
        public HeaderRow()
        {
            HeaderRowData = new HashSet<HeaderRowData>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Function { get; set; }
        public string Parameter { get; set; }
        public int ReviewTameplateId { get; set; }
        public int? ReviewColumnId { get; set; }
        public bool? Deleted { get; set; }

        public virtual ReviewColumn ReviewColumn { get; set; }
        public virtual ReviewTameplate ReviewTameplate { get; set; }
        public virtual ICollection<HeaderRowData> HeaderRowData { get; set; }
    }
}
