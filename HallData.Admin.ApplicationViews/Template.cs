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
	public class TemplateKey : IHasKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual int? TemplateId { get; set; }

		[JsonIgnore]
		public int Key
		{
			get
			{
				return this.TemplateId ?? 0;
			}
			set
			{
				this.TemplateId = value;
			}
		}
	}

	public class Template : TemplateKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("TEMPLATE_NAME_REQUIRED")]
		public string Name { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("TEMPLATE_CODE_REQUIRED")]
		public string Code { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool IsDefault { get; set; }
	}

	public class Template<TTemplateType> : Template, IValidatableObject
		where TTemplateType: TemplateTypeKey
	{
		[ChildView]
		[AddOperationParameter]
		[GlobalizedRequired("TEMPLATE_TEMPLATETYPE_REQUIRED")]
		public TTemplateType TemplateType { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (this.TemplateType == null || this.TemplateType.TemplateTypeId == null)
				yield return ValidationResultFactory.Create(new ValidationResult("Template Type Required"), "TEMPLATE_TEMPLATETYPE_REQUIRED");
		}
	}

	public class TemplateForAdd : Template<TemplateTypeKey>
	{
		[JsonIgnore]
		public override int? TemplateId
		{
			get
			{
				return base.TemplateId;
			}
			set
			{
				base.TemplateId = value;
			}
		}
	}

	public class TemplateForUpdate : Template
	{
		[GlobalizedRequired("TEMPLATE_TEMPLATEID_REQUIRED")]
		public override int? TemplateId
		{
			get
			{
				return base.TemplateId;
			}
			set
			{
				base.TemplateId = value;
			}
		}
	}

	public class TemplateResult : Template<TemplateTypeResult> { }
}
