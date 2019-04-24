﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using ReviewWeb.Models.ReviewTameplateModels;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace ReviewWeb.Controllers
{
    [Route("Template")]
    public class TameplateController : Controller
    {

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
    }
}