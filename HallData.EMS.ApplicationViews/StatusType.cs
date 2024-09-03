using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews
{
	public class StatusTypeKey : IHasKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public int? StatusTypeId { get; set; }

		[JsonIgnore]
		public int Key
		{
			get
			{
				return this.StatusTypeId ?? 0;
			}
			set
			{
				this.StatusTypeId = value;
			}
		}
	}

	public class StatusTypeResult : StatusTypeKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("STATUSTYPE_NAME_REQUIRED")]
		public string Name { get; set; }
	}
}
