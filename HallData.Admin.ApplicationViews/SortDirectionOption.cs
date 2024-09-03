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
	public class SortDirectionOptionKey : IHasKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual int? SortDirectionOptionId { get; set; }

		[JsonIgnore]
		public int Key
		{
			get
			{
				return this.SortDirectionOptionId ?? 0;
			}
			set
			{
				this.SortDirectionOptionId = value;
			}
		}
	}

	public class SortDirectionOption : SortDirectionOptionKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string Name { get; set; }
	}
}
