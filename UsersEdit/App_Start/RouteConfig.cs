using Infrastructure.RouteConstraints;
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
                name:"EditUserProfileRoute",
                url:"user-profile/edit-{id}",
                defaults: new {
                    controller = "Profile",
                    action = "Edit",
                },
                constraints: new { myConstraint = new PositiveIntegerOfLengthRouteConstraint("id")}
                );

            routes.MapRoute(
                name:"AllUsers",
                url: "all-user-profiles",
                defaults: new
                {
                    controller = "Profile",
                    action = "Index"
                }
                );

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
