using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HallData.Admin.ApplicationViews
{
	public class PageOption
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public int PageSize { get; set; }
	}
}
