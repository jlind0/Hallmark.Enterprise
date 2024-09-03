using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HallData.Admin.Web.Controllers;

namespace HallData.Admin.Web.Controllers
{
	[RoutePrefix("interfaces/{interfaceId}/attributes")]
	public class InterfaceAttributeController : BaseController
	{
		// INTERFACE ATTRIBUTES VIEW
		// --------------

		[HttpGet]
		[Route("{interfaceAttributeId}", Name = "InterfaceAttributeDetailsDisplay")]
		public ActionResult AttributeDetailsDisplay(int interfaceAttributeId)
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/InterfaceAttributes/InterfaceAttributeDetailsDisplay.cshtml");
		}

		// ADD INTERFACE ATTRIBUTE
		// -----------------

		[HttpGet]
		[Route("add", Name = "InterfaceAttributeAdd")]
		public ActionResult AttributeAdd(int interfaceId)
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/InterfaceAttributes/InterfaceAttributeAdd.cshtml");
		}

		// UPDATE INTERFACE ATTRIBUTE
		// -----------------

		[HttpGet]
		[Route("{interfaceAttributeId}/update", Name = "InterfaceAttributeUpdate")]
		public ActionResult AttributeUpdate(int interfaceAttributeId)
		{
			ViewBag.RouteValueDictionary = GetRouteData(Url);
			return View("~/Views/InterfaceAttributes/InterfaceAttributeUpdate.cshtml");
		}
	}
}