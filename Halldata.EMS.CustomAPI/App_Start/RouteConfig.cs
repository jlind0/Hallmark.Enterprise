using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Halldata.EMS.CustomAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "ApazineDefault", 
                url: "Apazine/{controller}/{action}", 
                defaults: new { controller = "Registrations", action = "Index" },
                namespaces: new string[] { "Halldata.EMS.CustomAPI.apazine.Controllers" }
            );
        }
    }
}
