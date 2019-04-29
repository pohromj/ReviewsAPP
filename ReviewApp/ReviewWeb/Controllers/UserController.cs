using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using ReviewWeb.Models.UserModels;
using Microsoft.AspNetCore.Http;
using ReviewWeb.Models.ProjectModels;

namespace ReviewWeb.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        public IActionResult RegistrationPage()
        {
            return View("~/Views/Login/Registration.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ValidateData(model))
                return View("Registration");

            using (HttpClient client = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(new
                {
                    Email = model.Email,
                    Password = model.Password,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname
                });
                HttpResponseMessage message = await client.PostAsync("http://localhost:55188/api/User/Registration", new StringContent(json, Encoding.UTF8, "application/json"));
                //LoginViewModel login = new LoginViewModel();
                //login.Login = model.Email;
                //login.Password = model.Password;
                //LoginController controller = new LoginController();

                //var c = HttpContext.RequestServices.GetService(typeof(LoginController));
                //return await lgn.Login(login);
                ViewBag.LoginEmail = model.Email;
                return View("~/Views/Login/Login.cshtml");
            }
        }
        private bool ValidateData(RegisterViewModel model)
        {
            bool isValid = true;
            if (model.Password != model.PasswordConfirm)
            {
                ViewBag.PasswordMatch = false;
                isValid = false;
            }
            if (!IsValidEmail(model.Email))
            {
                ViewBag.InvalidEmail = true;
                isValid = false;
            }
            if (model.Firstname == "")
            {
                ViewBag.FirstnameEmpty = true;
                isValid = false;
            }
            if (model.Lastname == "")
            {
                ViewBag.LastnameEmpty = true;
                isValid = false;
            }
            return isValid;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        [HttpGet("projectId")]
        public async Task<IActionResult> GetAllUsersForProject(int projectId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage message = await client.GetAsync("http://localhost:55188/api/User/GetAllUsers");

                var users = JsonConvert.DeserializeObject<List<UserDetail>>(await message.Content.ReadAsStringAsync());
                if (users.Count > 0 && users != null)
                {
                    HttpResponseMessage msg = await client.GetAsync("http://localhost:55188/api/Project/GetAllParticipantsOnProject?id=" + projectId);
                    var projectParticipants = JsonConvert.DeserializeObject<List<string>>(await msg.Content.ReadAsStringAsync());
                    List<UserDetail> participants = new List<UserDetail>(users);
                    participants.RemoveAll(p => !projectParticipants.Contains(p.Email));
                    users.RemoveAll(u => projectParticipants.Contains(u.Email));
                    HttpResponseMessage msg2 = await client.GetAsync("http://localhost:55188/api/Project/ProjectDetail?id=" + projectId);
                    var project = JsonConvert.DeserializeObject<ProjectDetailViewModel>(await msg2.Content.ReadAsStringAsync());
                    ViewBag.Project = project;
                    ViewBag.Users = users;
                    ViewBag.Participants = participants;
                    //HttpContext.Session.GetString("Email");
                    if (HttpContext.Session.GetString("Login") == project.ProjectModel.Owner)
                        ViewBag.IsOwner = true;
                    else
                        ViewBag.IsOwner = false;
                    return View("~/Views/Project/Participants.cshtml");
                }

            }
            return View("Error");
        }
        [HttpPost("ChangeParticipants")]
        //[Route("ChangeParticipants")]
        public async Task<IActionResult> ChangeParticipants([FromBody] Participant participants)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                string json = JsonConvert.SerializeObject(participants);
                HttpResponseMessage message = await client.PostAsync("http://localhost:55188/api/Project/ChangeParticipants", new StringContent(json, Encoding.UTF8, "application/json"));
            }
            return Ok();//RedirectToAction("ProjectDetail", "Project", new { projectId = participants.ProjectId });
        }
    }
}