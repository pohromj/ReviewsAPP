using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewApi.Models.Project
{
    public class ProjectDetailModel
    {
        public ProjectModel ProjectModel { get; set; }
        public WorkProductModel[] WorkproductModel { get; set; }
    }
}
