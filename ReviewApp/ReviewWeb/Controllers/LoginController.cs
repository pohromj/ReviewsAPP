using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using ReviewWeb.Models.UserModels;
using Newtonsoft.Json;

using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ReviewWeb.Controllers
{
    public class LoginController : Controller
    {
        readonly string siteName;
        public LoginController(IConfiguration configuration)
        {
            this.siteName = configuration.GetValue<string>("Websetting:Url");
        }
        public IActionResult LoginPage()
        {
            if (HttpContext.Session.Keys.Count() > 0)
                return RedirectToAction("Index", "Home");
            else
                return View("Login");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            
            if (HttpContext.Session.Keys.Count() > 0)
                return RedirectToAction("Index", "Home");
            using (HttpClient client = new HttpClient())
            {
                
                string json = JsonConvert.SerializeObject(loginModel);
                HttpResponseMessage message = await client.PostAsync(siteName + "/api/Auth/login", new StringContent(json, Encoding.UTF8, "application/json"));
                if (message.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                {
                    string s = await message.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<SecurityToken>(s);
                    HttpContext.Response.Cookies.Append("Firstname", token.User.Firstname);
                    HttpContext.Response.Cookies.Append("Lastname", token.User.Lastname);
                    HttpContext.Session.SetString("SecurityToken", token.Token);
                    HttpContext.Session.SetString("SecurityTokenExpiration", token.Expiration);
                    Console.WriteLine(token.Token);
                    HttpContext.Session.SetString("Login", token.User.Email);
                }
                else
                {
                    ViewBag.LoginFailed = true;
                    return View("Login");
                }
            }
            return RedirectToAction("ShowProjects", "Project");
            
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("~/Views/Login/Login.cshtml");
        }
    }
}