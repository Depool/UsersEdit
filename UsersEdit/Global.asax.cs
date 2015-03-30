using ApplicationRepository.Concrete.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using UsersEdit.Infrastructure.Authentication;
using UsersEdit.Infrastructure.Authentication.Interface;

namespace UsersEdit
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            if (FormsAuthentication.CookiesSupported)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    IPrincipalService principalService = new UsersEditPrincipalService(new UserRepository(), new HttpContextWrapper(Context));
                    var user = principalService.GetCurrent();
                    Context.User = principalService.GetCurrent();
                }
            }
        }
    }
}
