using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class HeaderRowData
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int ReviewId { get; set; }
        public int HeaderRowId { get; set; }

        public virtual HeaderRow HeaderRow { get; set; }
        public virtual Review Review { get; set; }
    }
}
