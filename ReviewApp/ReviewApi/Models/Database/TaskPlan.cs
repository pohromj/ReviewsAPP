using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class TaskPlan
    {
        public int Id { get; set; }
        public int IbmId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
