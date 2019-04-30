using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using ReviewWeb.Models.ReviewTameplateModels;
using Microsoft.AspNetCore.Http;
using System.Text;
using ReviewWeb.Models.ReviewModels;

namespace ReviewWeb.Controllers
{
    [Route("Template")]
    public class TameplateController : Controller
    {
        [Route("TameplateCreator")]
        public IActionResult TameplateCreator()
        {
            return View("TemplateCreator");
        }
        [HttpPost("SaveTemplate")]
        public async Task<IActionResult> SaveReviewTemplate([FromBody]ReviewTameplate model)
        {
            using (HttpClient client = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(model);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage message = await client.PostAsync("http://localhost:55188/api/Tameplate/CreateTameplate", new StringContent(json, Encoding.UTF8, "application/json"));
            }

            return RedirectToAction("Index", "Home");
        }
        [HttpGet("GetTemplate")]
        public async Task<ReviewTameplateForForm> GetTemplate(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage message = await client.GetAsync("http://localhost:55188/api/Tameplate/GetTameplate?id=" + id);
                var tameplate = JsonConvert.DeserializeObject<ReviewTameplateForForm>(await message.Content.ReadAsStringAsync());
                return tameplate;
            }
        }
        [Route("ShowAllTemplates")]
        public async Task<IActionResult> ShowAllTemplates()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                HttpResponseMessage msg = await client.GetAsync("http://localhost:55188/api/Tameplate/GetAllTameplates");
                List<ReviewTameplateForDropDown> templates = JsonConvert.DeserializeObject<List<ReviewTameplateForDropDown>>(await msg.Content.ReadAsStringAsync());
                ViewBag.Templates = templates;
                return View("~/Views/Tameplate/Templates.cshtml");
            }
        }
        [HttpGet("id")]
        [Route("GetTemplateDetail")]
        public IActionResult GetTemplateDetail(int id)
        {
            ViewBag.TemplateId = id;
            return View("~/Views/Tameplate/TemplateDetail.cshtml");
        }
        [HttpPut("UpdateTemplate")]
        //[Route("UpdateTemplate")]
        public async Task<IActionResult> UpdateTemplate([FromBody]ReviewTameplateForForm model)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                string json = JsonConvert.SerializeObject(model);
                HttpResponseMessage msg = await client.PutAsync("http://localhost:55188/api/Tameplate/UpdateTemplate", new StringContent(json, Encoding.UTF8, "application/json"));
                return Ok();
            }
        }
        [Route("DeleteTemplate")]
        public async Task<IActionResult>DeleteTemplate(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("SecurityToken"));
                
                HttpResponseMessage msg = await client.DeleteAsync("http://localhost:55188/api/Tameplate/DeleteTemplate?id=" + id);
                return RedirectToAction("ShowAllTemplates", "Template");
            }
        }
    }
}