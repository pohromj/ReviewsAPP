using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.ProjectModels
{
    public class CreateProjectViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProjectTypeId { get; set; }
    }
}
