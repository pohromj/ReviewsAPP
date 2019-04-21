using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using ReviewWeb.Models.ProjectModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReviewWeb.Models.Artifact;

namespace ReviewWeb.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult ShowProject(int id)
        {
            ProjectViewModel project = new ProjectViewModel();
            ViewBag.Project = project;
            return View("Projects");
        }
        public async Task<IActionResult> ShowProjects()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage message = await client.GetAsync("http://localhost:55188/api/Project/GetAllProjects");
                if (message.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    RedirectToAction("LoginPage", "Login");
                else
                {
                    string s = await message.Content.ReadAsStringAsync();
                    var projects = JsonConvert.DeserializeObject<List<ProjectViewModel>>(s);
                    ViewBag.Projects = projects;
                }
            }
            return View("Project");
        }
        public async Task<IActionResult> CreateProject()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage message = await client.GetAsync("http://localhost:55188/api/Project/GetAllProjectTypes");
                var projectTypes = JsonConvert.DeserializeObject<List<ProjectTypeViewModel>>(await message.Content.ReadAsStringAsync());
                List<SelectListItem> projectTypesSelect = new List<SelectListItem>();
                foreach(var pt in projectTypes)
                {
                    projectTypesSelect.Add(new SelectListItem() { Text = pt.Name, Value = pt.Id.ToString() });
                }
                ViewBag.projectTypes = projectTypesSelect;

                return View("CreateProject");
            }
                
        }

        [HttpPost]
        public async Task<IActionResult> SaveProject(CreateProjectViewModel model)
        {
            using (HttpClient client = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(model);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage message = await client.PostAsync("http://localhost:55188/api/Project/SaveProject", new StringContent(json, Encoding.UTF8, "application/json"));
            }
            return RedirectToAction("ShowProjects", "Project");
        }
        [HttpGet]
        public async Task<IActionResult> ProjectDetail(int projectId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage message = await client.GetAsync("http://localhost:55188/api/Project/ProjectDetail?id=" + projectId);
                var project = JsonConvert.DeserializeObject<ProjectDetailViewModel>(await message.Content.ReadAsStringAsync());
                ViewBag.Project = project;
                //HttpResponseMessage msg = await client.GetAsync("http://localhost:55188/api/Artifact/GetAllArtifactForProject?id=" + projectId);
                //HttpResponseMessage msg = await client.GetAsync("http://localhost:55188/api/Artifact/GetArtifactsPerPage?projectId=" + projectId + "&&page=" + 0);
/*
                var artifacts = JsonConvert.DeserializeObject<List<ArtifactViewModel>>(await msg.Content.ReadAsStringAsync());
                HttpResponseMessage msg2 = await client.GetAsync("http://localhost:55188/api/Artifact/NumberOfArtifactsInProject?projectId=" + projectId);
                int numberOfArtifact = JsonConvert.DeserializeObject<int>(await msg2.Content.ReadAsStringAsync());
                if (artifacts != null)
                {*/
               /*     ViewBag.NumberOfPage = numberOfArtifact / 15;
                    ViewBag.Artifacts = artifacts;
                    ViewBag.ActivePage = 0;
                    /*
                    var propertyList = artifacts[0].ArtifactProperties.Keys.ToList();
                    propertyList.Sort();
                    ViewBag.propertyList = propertyList;*/
              //  }*/


            }
            return View("ProjectDetail");
        }
        [HttpPost]
        public async Task<IActionResult> SaveWorkProduct(WorkProductViewModel workProduct)
        {
            using (HttpClient client = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(workProduct);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage message = await client.PostAsync("http://localhost:55188/api/Project/SaveWorkProduct", new StringContent(json, Encoding.UTF8, "application/json"));

            }
            return RedirectToAction("ProjectDetail", "Project", new { projectId = workProduct.ProjectId });
        }
        [HttpGet]
        public async Task<IActionResult>GetWorkProductDetail(int id)
        {
            using(HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage message = await client.GetAsync("http://localhost:55188/api/Project/GetWorkProductDetail?id=" + id);
                var workProductDetail = JsonConvert.DeserializeObject<WorkProductViewModel>(await message.Content.ReadAsStringAsync());
                ViewBag.workProductDetail = workProductDetail;
                HttpResponseMessage msg = await client.GetAsync("http://localhost:55188/api/Artifact/GetArtifactsPerPage?workProductId=" + id + "&&page=" + 0);
                var artifacts = JsonConvert.DeserializeObject<List<JazzArtifact>>(await msg.Content.ReadAsStringAsync());
                HttpResponseMessage msg2 = await client.GetAsync("http://localhost:55188/api/Artifact/NumberOfArtifactsInWorkProduct?workProductId=" + id);
                int numberOfArtifact = JsonConvert.DeserializeObject<int>(await msg2.Content.ReadAsStringAsync());
                if (artifacts != null)
                {
                    if(numberOfArtifact % 15 > 0)
                        ViewBag.NumberOfPage = numberOfArtifact / 15 + 1;
                    else
                        ViewBag.NumberOfPage = numberOfArtifact / 15;
                    ViewBag.Artifacts = artifacts;
                    ViewBag.ActivePage = 0;
                }
                return View("WorkProductDetail");
            }
            
        }
    }
}