using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UsersEdit
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Email");
            routes.IgnoreRoute("Email/{action}");

            routes.MapRoute(
                name: "Admin_elmah",
                url: "Admin/elmah/{type}",
                defaults: new { action = "Index", controller = "AdminElmah", type = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Profile", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
