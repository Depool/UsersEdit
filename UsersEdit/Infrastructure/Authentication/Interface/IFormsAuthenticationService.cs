using ApplicationRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersEdit.Infrastructure.Authentication.Interface
{
    public interface IFormsAuthenticationService
    {
        void SignIn(User user, bool createPersistentCookie);

        void SignOut();
    }
}