using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewApi.Models.Project
{
    public class ProjectModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Project name has to be set!")]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Owner { get; set; }
        public int ProjectTypeId { get; set; }
    }
}
