using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HallData.Admin.Web.Controllers
{
	//[RouteArea("Admin", AreaPrefix = "Admin")]
	[RoutePrefix("")]
	public class HomeController : Controller
	{
		[Route("", Name = "DisplayAdminDashboard")]
		public ActionResult Index()
		{
			return View("~/Views/Home/Index.cshtml");
		}
	}
}