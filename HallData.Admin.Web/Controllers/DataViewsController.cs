using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HallData.Admin.Web.Controllers;

namespace HallData.Admin.Web.Controllers
{
	[RoutePrefix("dataviews")]
	public class DataViewsController : BaseController
	{
		// DATA VIEWS
		// ----------

		[HttpGet]
		[Route("", Name = "DataViewsDisplay")]
		public ActionResult DataViewsDisplay()
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/DataViews/DataViewsDisplay.cshtml");
		}

		[HttpGet]
		[Route("{dataViewId}", Name = "DataViewDetailsDisplay")]
		public ActionResult DataViewDetailsDisplay(int dataViewId)
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/DataViews/DataViewDetailsDisplay.cshtml");
		}

		// ADD DATA VIEW VIEW
		// -----------------

		[HttpGet]
		[Route("add", Name = "DataViewAdd")]
		public ActionResult DataViewAdd()
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/DataViews/DataViewAdd.cshtml");
		}

		// UPDATE DATA VIEW
		// -----------------

		[HttpGet]
		[Route("{dataViewId}/update", Name = "DataViewUpdate")]
		public ActionResult DataViewUpdate(int dataViewId)
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/DataViews/DataViewUpdate.cshtml");
		}
	}
}