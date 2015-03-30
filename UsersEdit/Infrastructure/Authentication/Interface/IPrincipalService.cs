using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace UsersEdit.Infrastructure.Authentication.Interface
{
    public interface IPrincipalService
    {
        IPrincipal GetCurrent();
    }
}