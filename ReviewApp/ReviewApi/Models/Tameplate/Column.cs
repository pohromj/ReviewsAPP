using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewApi.Models.Tameplate
{
    public class Column
    {
        public int? Id { get; set; }
        public string Type { get; set; }
        public string ColumnName { get; set; }
        public List<string> Option { get; set; }
    }
}
