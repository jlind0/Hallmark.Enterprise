using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HallData.Web.Session
{
	public class HttpContextWrapper : IHttpContextWrapper
	{
		public string UserHostAddress
		{
			get { return HttpContext.Current.Request.UserHostAddress; }
		}

		public System.Collections.IDictionary Items
		{
			get { return HttpContext.Current.Items; }
		}
	}
}
