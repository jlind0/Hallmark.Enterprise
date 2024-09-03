using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HallData.Admin.Web.Controllers;

namespace HallData.Admin.Web.Controllers
{
	[RoutePrefix("dataviews/{dataViewId}/results")]
	public class DataViewResultsController : BaseController
	{
		// DATA VIEW RESULTS
		// ----------

		[HttpGet]
		[Route("{dataViewResultId}", Name = "DataViewResultDetailsDisplay")]
		public ActionResult DataViewResultDetailsDisplay(int dataViewResultId)
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/DataViewResults/DataViewResultDetailsDisplay.cshtml");
		}

		// ADD DATA VIEW RESULTS
		// -----------------

		[HttpGet]
		[Route("add", Name = "DataViewResultAdd")]
		public ActionResult DataViewResultAdd()
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/DataViewResults/DataViewResultAdd.cshtml");
		}

		// UPDATE DATA VIEW RESULTS
		// -----------------

		[HttpGet]
		[Route("{dataViewResultId}/update", Name = "DataViewResultUpdate")]
		public ActionResult DataViewResultUpdate()
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/DataViewResults/DataViewResultUpdate.cshtml");
		}
	}
}