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
	public class FilterOperationOptionKey : IHasKey<short>
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual short? FilterOperationOptionId { get; set; }

		[JsonIgnore]
		public short Key
		{
			get
			{
				return this.FilterOperationOptionId ?? 0;
			}
			set
			{
				this.FilterOperationOptionId = value;
			}
		}
	}

	public class FilterOperationOption : FilterOperationOptionKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("FILTEROPERATIONOPTION_NAME_REQUIRED")]
		public string Name { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public int OrderIndex { get; set; }
	}

	public class FilterOperationOptionForAdd : FilterOperationOption
	{
		[JsonIgnore]
		public override short? FilterOperationOptionId
		{
			get
			{
				return base.FilterOperationOptionId;
			}
			set
			{
				base.FilterOperationOptionId = value;
			}
		}
	}

	public class FilterOperationOptionForUpdate : FilterOperationOption
	{
		[GlobalizedRequired("FILTEROPERATIONOPTION_ID_REQUIRED")]
		public override short? FilterOperationOptionId
		{
			get
			{
				return base.FilterOperationOptionId;
			}
			set
			{
				base.FilterOperationOptionId = value;
			}
		}
	}

	public class FilterOperationOptionResult : FilterOperationOption { }
}
