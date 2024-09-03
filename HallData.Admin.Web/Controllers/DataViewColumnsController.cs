using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HallData.Admin.Web.Controllers;

namespace HallData.Admin.Web.Controllers
{
	[RoutePrefix("dataviews/{dataViewId}/columns")]
	public class DataViewColumnsController : BaseController
	{
		// DATA VIEW RESULTS
		// ----------

		[HttpGet]
		[Route("{dataViewColumnId}", Name = "DataViewColumnDetailsDisplay")]
		public ActionResult DataViewColumnsDetailsDisplay(int dataViewColumnId)
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/DataViewColumns/DataViewColumnDetailsDisplay.cshtml");
		}

		// ADD DATA VIEW RESULTS
		// -----------------

		[HttpGet]
		[Route("add", Name = "DataViewColumnAdd")]
		public ActionResult DataViewColumnsAdd()
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/DataViewColumns/DataViewColumnAdd.cshtml");
		}

		// UPDATE DATA VIEW RESULTS
		// -----------------

		[HttpGet]
		[Route("{dataViewResultId}/update", Name = "DataViewColumnUpdate")]
		public ActionResult DataViewResultUpdate(int dataViewColumnId)
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/DataViewColumns/DataViewColumnUpdate.cshtml");
		}
	}
}