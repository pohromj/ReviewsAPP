using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewApi.Models.Database;
using ReviewApi.Models.User;
using System.Security.Cryptography;

namespace ReviewApi.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        ReviewsDatabaseContext context;
        public UserController(ReviewsDatabaseContext context)
        {
            this.context = context;
        }
        [HttpPost]
        [Route("Registration")]
        public ActionResult Registration([FromBody] RegistrationModel registrationModel)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            string slt = Convert.ToBase64String(salt);
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(registrationModel.Password + slt);
            SHA256Managed sHA256 = new SHA256Managed();
            byte[] hash = sHA256.ComputeHash(passwordBytes);
            string hashedPassword = Convert.ToBase64String(hash);
            Users u = new Users();
            u.Email = registrationModel.Email;
            u.Firstname = registrationModel.Firstname;
            u.Lastname = registrationModel.Lastname;
            u.Password = hashedPassword;
            u.Salt = slt;
            u.SystemRoleId = context.SystemRole.FirstOrDefault().Id; //2; //zmenit potom na nacteni z databaze dane role
            context.Users.Add(u);
            context.SaveChanges();
            return Ok();
        }
        [HttpGet]
        [Route("GetAllUsers")]
        public IEnumerable<UserModel> GetAllUsers()
        {
            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            List<UserModel> usersModel = new List<UserModel>();
            /*if (identity != null)
            {
               
                string email = identity.FindFirst("UserEmail").Value;*/
            var users = context.Users; //context.Users.Where(u => u.Email != email).ToArray();
            foreach (Users u in users)
            {
                usersModel.Add(new UserModel() { Firstname = u.Firstname, Email = u.Email, Lastname = u.Lastname });
            }
            //}
            return usersModel;
        }
    }
}