using ApplicationRepository.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using UsersEdit.Infrastructure.Authentication.Interface;

namespace UsersEdit.Infrastructure.Authentication
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        private readonly HttpContextBase httpContext;

        public FormsAuthenticationService(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public void SignIn(User user, bool createPersistentCookie)
        {
            if (user == null)
                throw new ArgumentNullException("Signing user must be non-null");

            UserCookie cookie = new UserCookie
            {
                Login = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDay,
                Role = user.Role.Name,
                RememberMe = createPersistentCookie
            };

            string userData = JsonConvert.SerializeObject(cookie);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, cookie.Login, DateTime.Now,
                                                                             DateTime.Now.Add(FormsAuthentication.Timeout),
                                                                             createPersistentCookie, userData);

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };

            httpContext.Response.Cookies.Add(httpCookie);
        }
    }
}