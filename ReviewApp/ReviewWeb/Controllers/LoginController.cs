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

namespace ReviewWeb.Controllers
{
    public class LoginController : Controller
    {
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
            //CookieOptions cookieOptions = new CookieOptions();
            //cookieOptions.
            if (HttpContext.Session.Keys.Count() > 0)
                return RedirectToAction("Index", "Home");
            using (HttpClient client = new HttpClient())
            {
                //var json = Json(loginModel);
                string json = JsonConvert.SerializeObject(loginModel);
                HttpResponseMessage message = await client.PostAsync("http://localhost:55188/api/Auth/login", new StringContent(json, Encoding.UTF8, "application/json"));
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
            //return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("~/Views/Login/Login.cshtml");//RedirectToAction("Login", "LoginPage");
        }
    }
}