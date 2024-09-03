using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HallData.Admin.Web.Controllers;

namespace HallData.Admin.Web.Controllers
{
	[RoutePrefix("interfaces")]
	public class InterfacesController : BaseController
	{
		// INTERFACE VIEW
		// --------------

		[HttpGet]
		[Route("", Name = "InterfacesDisplay")]
		public ActionResult InterfacesDisplay()
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/Interfaces/InterfacesDisplay.cshtml");
		}

		[HttpGet]
		[Route("{interfaceId}", Name = "InterfaceDetailsDisplay")]
		public ActionResult InterfaceDetailsDisplay(int interfaceId)
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/Interfaces/InterfaceDetailsDisplay.cshtml");
		}

		// ADD INTERFACE VIEW
		// -----------------

		[HttpGet]
		[Route("add", Name = "InterfaceAdd")]
		public ActionResult InterfaceAdd()
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/Interfaces/InterfaceAdd.cshtml");
		}

		//[HttpGet]
		//[Route("addX", Name = "InterfaceAddX")]
		//public ActionResult InterfaceAddX()
		//{
		//	ViewBag.RouteValueDictionary = GetRouteData(Url);
		//	return View("~/Views/Interfaces/InterfaceAddX.cshtml");
		//}

		// UPDATE INTERFACE VIEW
		// -----------------

		[HttpGet]
		[Route("{interfaceId}/update", Name = "InterfaceUpdate")]
		public ActionResult InterfaceUpdate(int interfaceId)
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/Interfaces/InterfaceUpdate.cshtml");
		}
	}
}