using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MPDW
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Quest.MPDW.Controllers" }
            );

            // Prevent 404
            routes.MapRoute(
                name: "CatchAll",
                url: "{controller}/{action}/{id}/{*catchAll}",
                defaults: new { controller = "Error", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}