using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewApi.Models.Database;
using ReviewApi.Models.User;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;

namespace ReviewApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        ReviewsDatabaseContext context;

        public AuthController(ReviewsDatabaseContext context)
        {
            this.context = context;
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            var user = ValidateAndCreateUser(loginModel.Login, loginModel.Password);//context.Users.Where(u => u.Email == loginModel.Login && u.Password == loginModel.Password).FirstOrDefault();

            if (user != null)
            {
                var claims = new[]
                {
                    new Claim("UserEmail",user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecreteKeyForMyAlgorithm"));

                var token = new JwtSecurityToken(
                    issuer: "http://oec.com",
                    audience: "http://oec.com",
                    expires: DateTime.UtcNow.AddHours(24),
                    claims: claims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)

                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user
                });
            }
            return Unauthorized();
        }
        private Users ValidateAndCreateUser(string login, string password)
        {
            var user = context.Users.Where(u => u.Email == login).FirstOrDefault();
            if (user != null)
            {
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password + user.Salt);
                SHA256Managed sHA256 = new SHA256Managed();
                byte[] hash = sHA256.ComputeHash(passwordBytes);
                string hashedPassword = Convert.ToBase64String(hash);
                if (hashedPassword == user.Password)
                    return user;
            }
            return null;
        }
    }
}