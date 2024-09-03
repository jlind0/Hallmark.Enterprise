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
	public class TemplateTypeKey : IHasKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual int? TemplateTypeId { get; set; }

		[JsonIgnore]
		public int Key
		{
			get
			{
				return this.TemplateTypeId ?? 0;
			}
			set
			{
				this.TemplateTypeId = value;
			}
		}
	}

	public class TemplateType : TemplateTypeKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("TEMPLATETYPE_NAME_REQUIRED")]
		public string Name { get; set; }
	}

	public class TemplateTypeForAdd : TemplateType
	{
		[JsonIgnore]
		public override int? TemplateTypeId
		{
			get
			{
				return base.TemplateTypeId;
			}
			set
			{
				base.TemplateTypeId = value;
			}
		}
	}

	public class TemplateTypeForUpdate : TemplateType
	{
		[GlobalizedRequired("TEMPLATETYPE_ID_REQUIRED")]
		public override int? TemplateTypeId
		{
			get
			{
				return base.TemplateTypeId;
			}
			set
			{
				base.TemplateTypeId = value;
			}
		}
	}

	public class TemplateTypeResult : TemplateType { }

    public enum TemplateTypes
    {
        Grid = 1,
        GridCell = 6,
        GridColumn = 3,
        GridFilter = 4,
        GridHeader = 2,
        GridPager = 5,
    }
}
