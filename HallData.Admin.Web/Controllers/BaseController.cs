using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace HallData.Admin.Web.Controllers
{
	public class BaseController : Controller
	{

		// route interceptor
		protected override RedirectToRouteResult RedirectToRoute(string routeName, System.Web.Routing.RouteValueDictionary routeValues)
		{
			return new AjaxAwareRedirectResult(Url, routeName, routeValues);
		}

		public class AjaxAwareRedirectResult : RedirectToRouteResult
		{
			public AjaxAwareRedirectResult(UrlHelper url, string routeName, System.Web.Routing.RouteValueDictionary routeValues)
				: base(routeName, routeValues)
			{
				this.url = url;
			}

			private readonly UrlHelper url;

			public override void ExecuteResult(ControllerContext context)
			{
				if (context.RequestContext.HttpContext.Request.IsAjaxRequest())
				{
					string destinationUrl = url.RouteUrl(base.RouteName, base.RouteValues);
					JsonResult result = new JsonResult()
					{
						Data = destinationUrl
					};
					result.ExecuteResult(context);
				}
				else
				{
					base.ExecuteResult(context);
				}
			}
		}

		// route value generator

		[HttpPost]
		[Route("routes/get", Name = "GetRoute")]
		public ActionResult GetRoute(List<Dictionary<string, string>> json)
		{

			RouteValueDictionary routeValueDictionary = new RouteValueDictionary();

			string routeName = json[0]["routeName"];
			foreach (Dictionary<string, string> dictionary in json)
			{
				foreach (KeyValuePair<string, string> keyValuePair in dictionary)
				{
					if (keyValuePair.Key != "routeName" && keyValuePair.Key != "controller" && keyValuePair.Key != "action")
					{
						routeValueDictionary.Add(keyValuePair.Key, keyValuePair.Value);
					}
				}
			}

			return RedirectToRoute(routeName, routeValueDictionary);
		}

		public List<KeyValuePair<string, object>> GetRouteData(UrlHelper Url)
		{
			List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();

			var key = Url.RequestContext.RouteData.Values.Keys.GetEnumerator();
			var value = Url.RequestContext.RouteData.Values.Values.GetEnumerator();

			for (int i = 0; i < Url.RequestContext.RouteData.Values.Keys.Count; i++)
			{
				if (key.MoveNext() && value.MoveNext())
				{
					list.Add(new KeyValuePair<string, object>(key.Current, value.Current));
				}
			}

			return list;
		}
	}
}

