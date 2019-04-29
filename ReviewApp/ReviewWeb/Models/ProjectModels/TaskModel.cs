using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.ProjectModels
{
    public class TaskModel
    {
        public int Id { get; set; }
        public int IbmId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
    }
}
