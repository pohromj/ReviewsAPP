using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewApi.Models.User
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Login is required!")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password is reqired!")]
        public string Password { get; set; }
    }
}
