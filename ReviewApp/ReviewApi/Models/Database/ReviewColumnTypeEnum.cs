using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class ReviewColumnTypeEnum
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ReviewColumnId { get; set; }

        public virtual ReviewColumn ReviewColumn { get; set; }
    }
}
