using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HallData.EMS.Web.Bootstrap;
using Microsoft.Practices.Unity;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.ComponentModel.DataAnnotations;

namespace HallData.EMS.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityContainer container = new UnityContainer();
            Bootstrapper.Register(container);
            Bootstrapper.RegisterControllers(container, RouteTable.Routes);
            Bootstrapper.Configure(container, GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
