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
	public class FilterTypeKey : IHasKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual int? FilterTypeId { get; set; }

		[JsonIgnore]
		public int Key
		{
			get
			{
				return this.FilterTypeId ?? 0;
			}
			set
			{
				this.FilterTypeId = value;
			}
		}
	}

	public class FilterType<TTemplate, TFilterOption, TFilterOptionCollection> : FilterTypeKey, IValidatableObject
		where TTemplate: TemplateKey
		where TFilterOption: FilterOperationOptionKey
		where TFilterOptionCollection: IEnumerable<TFilterOption>
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("FILTERTYPE_NAME_REQUIRED")]
		public string Name { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("FILTERTYPE_DATATYPE_REQUIRED")]
		public string DataType { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool IsDefault { get; set; }

		[ChildView]
		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("FILTERTYPE_TEMPLATE_REQUIRED")]
		public TTemplate Template { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildViewCollection(parmType: CollectionParameterType.Xml)]
		public TFilterOptionCollection OperationOptions { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (this.Template == null || this.Template.TemplateId == null)
				yield return ValidationResultFactory.Create(new ValidationResult("Template Required"), "FILTERTYPE_TEMPLATE_REQUIRED");
		}
	}

	public class FilterTypeForAdd : FilterType<TemplateKey, FilterOperationOptionKey, List<FilterOperationOptionKey>>
	{
		[JsonIgnore]
		public override int? FilterTypeId
		{
			get
			{
				return base.FilterTypeId;
			}
			set
			{
				base.FilterTypeId = value;
			}
		}
	}

	public class FilterTypeForUpdate : FilterType<TemplateKey, FilterOperationOptionKey, List<FilterOperationOptionKey>>
	{
		[GlobalizedRequired("FILTERTYPE_ID_REQUIRED")]
		public override int? FilterTypeId
		{
			get
			{
				return base.FilterTypeId;
			}
			set
			{
				base.FilterTypeId = value;
			}
		}
	}

	public class FilterTypeResult : FilterType<TemplateResult, FilterOperationOptionResult, List<FilterOperationOptionResult>> { }
}
