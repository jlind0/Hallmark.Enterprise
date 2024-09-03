using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HallData.Admin.Web.Controllers;

namespace HallData.Admin.Web.Controllers
{
	[RoutePrefix("applicationviews")]
	public class ApplicationViewsController : BaseController
	{
		// APPLICATION VIEWS
		// -----------------

		[HttpGet]
		[Route("", Name = "ApplicationViewsDisplay")]
		public ActionResult ApplicationViewDisplay()
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/ApplicationViews/ApplicationViewsDisplay.cshtml");
		}

		[HttpGet]
		[Route("{applicationViewId}", Name = "ApplicationViewDetailsDisplay")]
		public ActionResult ApplicationViewDetailsDisplay(int applicationViewId)
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/ApplicationViews/ApplicationViewDetailsDisplay.cshtml");
		}

		// ADD APPLICATION VIEW
		// -----------------

		[HttpGet]
		[Route("add", Name = "ApplicationViewAdd")]
		public ActionResult ApplicationAddView()
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/ApplicationViews/ApplicationViewAdd.cshtml");
		}

		// UPDATE APPLICATION VIEW
		// -----------------

		[HttpGet]
		[Route("{applicationViewId}/update", Name = "ApplicationViewUpdate")]
		public ActionResult ApplicationUpdateView(int applicationViewId)
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/ApplicationViews/ApplicationViewUpdate.cshtml");
		}
	}
}