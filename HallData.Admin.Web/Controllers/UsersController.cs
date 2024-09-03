using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HallData.Admin.Web.Controllers;

namespace HallData.Admin.Web.Controllers
{
	[RoutePrefix("")]
	public class UsersController : BaseController
	{
		// DISPLAY ADMIN USER DETAILS
		// --------------------------

		[HttpGet]
		[Route("users/{partyGuid}", Name = "DisplayUserDetails")]
		public ActionResult DisplayUserDetails()
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/Users/UserDetailsDisplay.cshtml");
		}

		// ADD ADMIN USER
		// --------------

		[HttpGet]
		[Route("users/add", Name = "AddAdminUser")]
		public ActionResult AddAdminUser()
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/Users/UserAdd.cshtml");
		}

		// UPDATE ADMIN USER
		// -----------------

		[HttpGet]
		[Route("users/{partyGuid}/update", Name = "UpdateAdminUser")]
		public ActionResult UpdateAdminUser()
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/Users/UserUpdate.cshtml");
		}
	}
}