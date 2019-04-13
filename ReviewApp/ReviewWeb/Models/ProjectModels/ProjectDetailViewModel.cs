using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.ProjectModels
{
    public class ProjectDetailViewModel
    {
        public ProjectViewModel ProjectModel { get; set; }
        public WorkProductViewModel[] WorkproductModel { get; set; }
    }
}
