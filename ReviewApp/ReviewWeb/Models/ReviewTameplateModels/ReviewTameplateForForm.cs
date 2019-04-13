using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.ReviewTameplateModels
{
    public class ReviewTameplateForForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descritpion { get; set; }
        public List<Role> Roles { get; set; }
        public List<Header> Header { get; set; }
        public List<Column> Columns { get; set; }
    }
}
