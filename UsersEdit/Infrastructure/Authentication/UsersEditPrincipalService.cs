using ApplicationRepository.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using UsersEdit.Infrastructure.Authentication.Interface;

namespace UsersEdit.Infrastructure.Authentication
{
    public class UsersEditPrincipalService : IPrincipalService
    {
        private readonly HttpContextBase context;
        private IUserRepository users;

        public UsersEditPrincipalService(IUserRepository users, HttpContextBase context)
        {
            this.users = users;
            this.context = context;
        }

        public IPrincipal GetCurrent()
        {
            string cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName].Value;
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie);

            UserPrincipal user = new UserPrincipal(new UserIdentity(ticket));

            return checkUser(user) ? new UserPrincipal(new UserIdentity(ticket)) :
                                     new UserPrincipal(new UserIdentity(null));
        }

        private bool checkUser(UserPrincipal user)
        {
            UserIdentity identity = user.Information;
            return users.FindFirst(usr => usr.Login == identity.Login &&
                                          usr.FirstName == identity.FirstName &&
                                          usr.LastName == identity.LastName) != null;
        }
    }
}