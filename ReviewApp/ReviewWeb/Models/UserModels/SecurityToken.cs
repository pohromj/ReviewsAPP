using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewWeb.Models.UserModels
{
    public class SecurityToken
    {
        public string Token { get; set; }
        public string Expiration { get; set; }

        public UserDetail User { get; set; }
    }
}
