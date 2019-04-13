using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReviewApi.Models.Database;
using System.Security.Claims;
using ReviewApi.Models.Project;
using ReviewApi.Models.User;
using ReviewApi.Models.Artifact;

namespace ReviewApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Project")]
    public class ProjectController : Controller
    {
        ReviewsDatabaseContext context;
        public ProjectController(ReviewsDatabaseContext context)
        {
            this.context = context;
        }
        [Route("SaveProject")]
        [HttpPost]
        public IActionResult SaveProject([FromBody] ProjectModel project)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                string email = identity.FindFirst("UserEmail").Value;
                Project p = new Project() { Name = project.Name, Description = project.Description, UsersEmail = email, ProjectTypeId = project.ProjectTypeId};
                context.Project.Add(p);
                context.SaveChanges();

                UserProject r = new UserProject() { ProjectId = p.Id, UsersEmail = email };
                context.UserProject.Add(r);
                context.SaveChanges();
                return Ok();
            }
            return Unauthorized();

        }
        [HttpGet]
        [Route("GetAllProjects")]
        public IEnumerable<Project> GetAllProjectsByParticipant()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                string email = identity.FindFirst("UserEmail").Value;
                
                var projectIDs = context.UserProject.Where(o => o.UsersEmail == email).Select(i => i.ProjectId).ToArray();
                
                var projects = context.Project.Where(p => projectIDs.Contains(p.Id)).ToArray();
                return projects;
            }
            return null;
        }
        [HttpGet]
        [Route("GetAllProjectTypes")]
        public IEnumerable<ProjectTypeModel>GetAllProjectTypes()
        {
            List<ProjectTypeModel> types = new List<ProjectTypeModel>();
            var projectTypes = context.ProjectType.ToArray();
            foreach(var p in projectTypes)
            {
                types.Add(new ProjectTypeModel() { Id = p.Id, Name = p.Name });
            }
            return types;
        }
        [HttpGet("id")]
        [Route("ProjectDetail")]
        public ProjectDetailModel GetProjectDetail(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                //validate if user can access to specific projct
                string email = identity.FindFirst("UserEmail").Value;
                var hasUserProject = context.UserProject.Where(u => u.ProjectId == id && u.UsersEmail == email).FirstOrDefault();
                if (hasUserProject != null)
                    if (hasUserProject.UsersEmail == identity.FindFirst("UserEmail").Value)
                    {
                        ProjectDetailModel detailsModel = new ProjectDetailModel();
                        Project project = context.Project.Where(p => p.Id == id).FirstOrDefault();
                        ProjectModel model = new ProjectModel() { Id = project.Id, Description = project.Description, Name = project.Name, Owner = project.UsersEmail };
                        detailsModel.ProjectModel = model;
                        Workproduct[] workproducts = context.Workproduct.Where(p => p.ProjectId == id).ToArray();
                        List<WorkProductModel> wmodel = new List<WorkProductModel>();
                        foreach (Workproduct w in workproducts)
                        {
                            wmodel.Add(new WorkProductModel() { Name = w.Name, Description = w.Description, Id = w.Id, ProjectId = w.ProjectId });
                        }
                        detailsModel.WorkproductModel = wmodel.ToArray();
                        return detailsModel;
                    }
            }
            return null;
        }
        [HttpPost]
        [Route("SaveWorkProduct")]
        public ActionResult SaveWorkProduct([FromBody] Workproduct workproduct)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            workproduct.UsersEmail = identity.FindFirst("UserEmail").Value;
            Project p = context.Project.Where(o => o.Id == workproduct.ProjectId).FirstOrDefault();
            if (p == null)
                return NotFound(new { Message = "Project doesn't exist!" });
            if (workproduct.Name != "")
            {
                context.Workproduct.Add(workproduct);
                context.SaveChanges();
                return Ok();
            }

            return BadRequest(new { Message = "Name can't be empty" });
        }
        [HttpGet("id")]
        [Route("GetAllWorkProductsForProject")]
        public IEnumerable<WorkProductModel> GetAllWorkProductsForProject(int id)
        {
            List<WorkProductModel> workProducts = new List<WorkProductModel>();
            foreach (var w in context.Workproduct.Where(p => p.ProjectId == id))
            {
                workProducts.Add(new WorkProductModel() { Id = w.Id, Name = w.Name });
            }
            return workProducts;
        }
        [HttpGet("id")]
        [Route("GetAllParticipantsOnProject")]
        public IEnumerable<string> GetAllParticipantsOnProject(int id)
        {
            return context.UserProject.Where(p => p.ProjectId == id).Select(p => p.UsersEmail).ToArray();
        }

        [HttpPost]
        [Route("ChangeParticipants")]
        public ActionResult ChangeParticipantsOnProject([FromBody] Participant participants)
        {
            foreach (string s in participants.AddedUsers)
            {
                UserProject project = new UserProject() { ProjectId = participants.ProjectId, UsersEmail = s };
                context.UserProject.Add(project);
            }
            context.SaveChanges();
            List<UserProject> l = new List<UserProject>();
            foreach (string s in participants.RemovedUsers)
            {
                UserProject project = new UserProject() { ProjectId = participants.ProjectId, UsersEmail = s };
                l.Add(project);
            }
            context.UserProject.RemoveRange(l);
            context.SaveChanges();
            return Ok();
        }
        [HttpGet]
        [Route("GetWorkProductDetail")]
        public WorkProductModel GetWorkProductDetail(int id)
        {
            var workProduct = context.Workproduct.Where(w => w.Id == id).FirstOrDefault();
            WorkProductModel model = new WorkProductModel()
            {
                Id = workProduct.Id,
                Description = workProduct.Description,
                Name = workProduct.Name,
                ProjectId = workProduct.ProjectId
            };
            return model;

        }

    }
}