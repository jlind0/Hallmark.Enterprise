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
	public class RoleKey : IHasKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public int? RoleId { get; set; }

		[JsonIgnore]
		public int Key
		{
			get
			{
				return this.RoleId ?? 0;
			}
			set
			{
				this.RoleId = value;
			}
		}
	}

	public class RoleResult : RoleKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("ROLE_NAME_REQUIRED")]
		public string Name { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? IsSystem { get; set; }
	}
}
