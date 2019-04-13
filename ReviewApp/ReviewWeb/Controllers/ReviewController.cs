using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using ReviewWeb.Models.ReviewModels;
using Newtonsoft.Json;
using ReviewWeb.Models.ProjectModels;
using ReviewWeb.Models.UserModels;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace ReviewWeb.Controllers
{
    [Route("Review")]
    public class ReviewController : Controller
    {
        [HttpGet("projectID")]
        public async Task<IActionResult> ReviewCreator(int projectID)
        {
            List<SelectListItem> listItemsTameplates = new List<SelectListItem>();
            List<SelectListItem> listItemsWorkProducts = new List<SelectListItem>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage msg = await client.GetAsync("http://localhost:55188/api/Tameplate/GetAllTameplates");
                var tameplates = JsonConvert.DeserializeObject<List<ReviewTameplateForDropDown>>(await msg.Content.ReadAsStringAsync());
                listItemsTameplates.Add(new SelectListItem() { Text = "", Value = "" });
                foreach (var p in tameplates)
                {
                    listItemsTameplates.Add(new SelectListItem() { Text = p.Name, Value = p.ID.ToString() });
                }
                ViewBag.AllTameplates = listItemsTameplates;
                msg = await client.GetAsync("http://localhost:55188/api/Project/GetAllWorkProductsForProject?id=" + projectID);
                var workProducts = JsonConvert.DeserializeObject<List<WorkProductViewModel>>(await msg.Content.ReadAsStringAsync());
                foreach (var k in workProducts)
                {
                    listItemsWorkProducts.Add(new SelectListItem() { Text = k.Name, Value = k.Id.ToString() });
                }
                ViewBag.WorkProducts = listItemsWorkProducts;
                HttpResponseMessage message = await client.GetAsync("http://localhost:55188/api/User/GetAllUsers");

                var users = JsonConvert.DeserializeObject<List<UserDetail>>(await message.Content.ReadAsStringAsync());
                if (users.Count > 0 && users != null)
                {
                    HttpResponseMessage msgP = await client.GetAsync("http://localhost:55188/api/Project/GetAllParticipantsOnProject?id=" + projectID);
                    var projectParticipants = JsonConvert.DeserializeObject<List<string>>(await msgP.Content.ReadAsStringAsync());
                    List<UserDetail> participants = new List<UserDetail>(users);
                    participants.RemoveAll(p => !projectParticipants.Contains(p.Email));
                    users.RemoveAll(u => projectParticipants.Contains(u.Email));
                    HttpResponseMessage msg2 = await client.GetAsync("http://localhost:55188/api/Project/ProjectDetail?id=" + projectID);
                    var project = JsonConvert.DeserializeObject<ProjectDetailViewModel>(await msg2.Content.ReadAsStringAsync());
                    ViewBag.Project = project;
                    ViewBag.Users = users;
                    ViewBag.Participants = participants;
                    //HttpContext.Session.GetString("Email");
                    if (HttpContext.Session.GetString("Login") == project.ProjectModel.Owner)
                        ViewBag.IsOwner = true;
                    else
                        ViewBag.IsOwner = false;
                }
            }
            ViewBag.projectID = projectID;
            return View("~/Views/Review/ReviewCreator.cshtml");
        }
        [HttpPost("CreateReview")]
        public async Task<IActionResult>CreateReview([FromBody] FullReviewSetup setup)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                string json = JsonConvert.SerializeObject(setup);
                HttpResponseMessage message = await client.PostAsync("http://localhost:55188/api/Review/CreateReview", new StringContent(json, Encoding.UTF8, "application/json"));
                return Ok();
            }
        }
    }
}