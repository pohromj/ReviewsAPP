using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.ProjectModels
{
    public class ProjectReview
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WorkProductName { get; set; }
        public int WorkProductId { get; set; }
        public bool? Complete { get; set; }
    }
}
