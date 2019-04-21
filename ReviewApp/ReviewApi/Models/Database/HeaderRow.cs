using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class HeaderRow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Function { get; set; }
        public string Parameter { get; set; }
        public int ReviewTameplateId { get; set; }
        public int? ReviewColumnId { get; set; }

        public virtual ReviewColumn ReviewColumn { get; set; }
        public virtual ReviewTameplate ReviewTameplate { get; set; }
    }
}
