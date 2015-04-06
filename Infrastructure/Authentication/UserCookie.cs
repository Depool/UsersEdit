using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infrastructure.Authentication
{
    public class UserCookie
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Role { get; set; }
        public bool RememberMe { get; set; }

        public string Email { get; set; }
    }
}