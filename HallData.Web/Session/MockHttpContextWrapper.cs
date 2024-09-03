using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Web.Session
{
	public class MockHttpContextWrapper : IHttpContextWrapper
	{
		private readonly Dictionary<object, object> items = new Dictionary<object, object>();

		public string UserHostAddress
		{
			get { return "192.168.1.1"; }
		}

		public System.Collections.IDictionary Items
		{
			get { return items; }
		}
	}
}
