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
using ReviewWeb.Models.Artifact;
using Microsoft.Extensions.Configuration;

namespace ReviewWeb.Controllers
{
    [Route("Review")]
    public class ReviewController : Controller
    {
        readonly string siteName;
        public ReviewController(IConfiguration configuration)
        {
            this.siteName = configuration.GetValue<string>("Websetting:Url");
        }
        [HttpGet("projectID")]
        public async Task<IActionResult> ReviewCreator(int projectID, int? workproductId)
        {
            List<SelectListItem> listItemsTameplates = new List<SelectListItem>();
            List<SelectListItem> listItemsWorkProducts = new List<SelectListItem>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage msg = await client.GetAsync(siteName + "/api/Tameplate/GetAllTameplates");
                var tameplates = JsonConvert.DeserializeObject<List<ReviewTameplateForDropDown>>(await msg.Content.ReadAsStringAsync());
                listItemsTameplates.Add(new SelectListItem() { Text = "", Value = "" });
                foreach (var p in tameplates)
                {
                    listItemsTameplates.Add(new SelectListItem() { Text = p.Name, Value = p.ID.ToString() });
                }
                ViewBag.AllTameplates = listItemsTameplates;
                msg = await client.GetAsync(siteName+"/api/Project/GetAllWorkProductsForProject?id=" + projectID);
                var workProducts = JsonConvert.DeserializeObject<List<WorkProductViewModel>>(await msg.Content.ReadAsStringAsync());
                foreach (var k in workProducts)
                {
                    if(k.Id == workproductId)
                    listItemsWorkProducts.Add(new SelectListItem() { Text = k.Name, Value = k.Id.ToString(), Selected = true });
                    else
                        listItemsWorkProducts.Add(new SelectListItem() { Text = k.Name, Value = k.Id.ToString()});
                }
                ViewBag.WorkProducts = listItemsWorkProducts;
                HttpResponseMessage message = await client.GetAsync(siteName + "/api/User/GetAllUsers");

                var users = JsonConvert.DeserializeObject<List<UserDetail>>(await message.Content.ReadAsStringAsync());
                if (users.Count > 0 && users != null)
                {
                    HttpResponseMessage msgP = await client.GetAsync(siteName + "/api/Project/GetAllParticipantsOnProject?id=" + projectID);
                    var projectParticipants = JsonConvert.DeserializeObject<List<string>>(await msgP.Content.ReadAsStringAsync());
                    List<UserDetail> participants = new List<UserDetail>(users);
                    participants.RemoveAll(p => !projectParticipants.Contains(p.Email));
                    users.RemoveAll(u => projectParticipants.Contains(u.Email));
                    HttpResponseMessage msg2 = await client.GetAsync(siteName + "/api/Project/ProjectDetail?id=" + projectID);
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
        public async Task<int>CreateReview([FromBody] FullReviewSetup setup)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                string json = JsonConvert.SerializeObject(setup);
                HttpResponseMessage message = await client.PostAsync(siteName + "/api/Review/CreateReview", new StringContent(json, Encoding.UTF8, "application/json"));
                int reviewId = JsonConvert.DeserializeObject<int>(await message.Content.ReadAsStringAsync());
                //return RedirectToAction("GetReview", "Review", new { id = reviewId });
                return reviewId;
            }
        }
        [HttpGet]
        [Route("GetReview")]
        public async Task<IActionResult>GetReview(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage msg2 = await client.GetAsync(siteName + "/api/Review/GetReview?id=" + id);
                FullReview review = JsonConvert.DeserializeObject<FullReview>(await msg2.Content.ReadAsStringAsync());
                ViewBag.ReviewId = id;
                ViewBag.ReviewSetup = review;
                return View("~/Views/Review/ReviewForm.cshtml");
            }
        }
        [HttpGet]
        [Route("GetMyReviews")]
        public async Task<IActionResult>GetMyReviews()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage msg2 = await client.GetAsync(siteName + "/api/Review/GetMyReviews");
                List<ReviewSetup> review = JsonConvert.DeserializeObject<List<ReviewSetup>>(await msg2.Content.ReadAsStringAsync());
                ViewBag.ReviewSetup = review;
                return View("~/Views/Review/MyReviews.cshtml");
            }
        }
        [HttpPost("SaveReviewProgress")]
        public async Task<IActionResult>SaveReviewProgress([FromBody] ReviewProgress progress)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                string json = JsonConvert.SerializeObject(progress);
                HttpResponseMessage message = await client.PostAsync(siteName + "/api/Review/SaveReviewProgress", new StringContent(json, Encoding.UTF8, "application/json"));
                
                return Ok();
            }
        }

        [HttpGet]
        [Route("GetReviewsForArtifact")]
        public async Task<IActionResult> GetReviewsForArtifact(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage msg2 = await client.GetAsync(siteName + "/api/Review/GetReviewsForArtifact?id=" + id);
                var artifact = JsonConvert.DeserializeObject<ReviewsForArtifact>(await msg2.Content.ReadAsStringAsync());
                ViewBag.Artifact = artifact;
                return View("~/Views/Review/ReviewsForArtifact.cshtml");
            }
        }
        [HttpPost("CloseReview")]
        public async Task<IActionResult> CloseReview([FromBody] ReviewProgress progress)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                string json = JsonConvert.SerializeObject(progress);
                HttpResponseMessage message = await client.PostAsync(siteName+"/api/Review/CloseReview", new StringContent(json, Encoding.UTF8, "application/json"));
                return Ok();
            }
        }
        [Route("DeleteReview")]
       public async Task<IActionResult>DeleteReview(int id)
       {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                
                HttpResponseMessage message = await client.DeleteAsync(siteName + "/api/Review/DeleteReview?id=" + id);
                return RedirectToAction("ShowProjects", "Project");
            }
        }
    }
}