//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Optimization;
//using System.Web.Routing;
using System.Web.Http;
using Microsoft.Practices.Unity;
//using HallData.Security;
//using HallData.EMS.Security.Tokenizer;
//using HallData.EMS.Business;
//using HallData.Business.Services.Proxy;
//using System.Configuration;
//using HallData.Web.Dependencies;

using System.Configuration;
using RedirectToRouteResult = System.Web.Http.Results.RedirectToRouteResult;

using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HallData.Admin.Web;
using Website.Security;

namespace HallData.Admin.Web
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
            UnityContainer container = new UnityContainer();
            Bootstrapper.Register(container);
            Bootstrapper.RegisterControllers(container, RouteTable.Routes);
            Bootstrapper.Configure(container, GlobalConfiguration.Configuration);

			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
	}
}
