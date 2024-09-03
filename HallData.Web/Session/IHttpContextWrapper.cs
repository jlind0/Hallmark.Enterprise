using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Web.Session
{
	public interface IHttpContextWrapper
	{
		string UserHostAddress { get; }
		IDictionary Items { get; }
	}
}
