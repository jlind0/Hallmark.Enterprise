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
	public class InterfaceAttributeKey : IHasKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual int? InterfaceAttributeId { get; set; }

		[JsonIgnore]
		public int Key
		{
			get
			{
				return this.InterfaceAttributeId ?? 0;
			}
			set
			{
				this.InterfaceAttributeId = value;
			}
		}
	}

	public class InterfaceAttribute<TType> : InterfaceAttributeKey, IValidatableObject
		where TType: InterfaceKey
	{
		[ChildView]
		[AddOperationParameter]
		[UpdateOperationParameter]
		public TType Type { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("ADMIN_INTERFACEATTRIBUTE_NAME_REQUIRED")]
		public string Name { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("ADMIN_INTERFACEATTRIBUTE_DISPLAYNAME_REQUIRED")]
		public string DisplayName { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool IsKey { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool IsCollection { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (this.Type != null && this.Type.InterfaceId == null)
				yield return ValidationResultFactory.Create(new ValidationResult("Attribute must have a type id if type object is supplied"), "ADMIN_INTERFACEATTRIBUTE_TYPE_INVALID");
			if (this.IsCollection && this.Type == null)
				yield return ValidationResultFactory.Create(new ValidationResult("Attribute must be strongly typed if collection"), "ADMIN_INTERFACEATTRIBUTE_TYPE_REQUIRED");
			if (this.IsKey && (this.IsCollection || this.Type != null))
				yield return ValidationResultFactory.Create(new ValidationResult("Attribute cannot be strongly typed or a collection if a key"), "ADMIN_INTERFACEATTRIBUTE_KEY_INVALID");
		}
	}

	public class InterfaceAttribute<TType, TInterface> :InterfaceAttribute<TType>
		where TType: InterfaceKey
		where TInterface: InterfaceKey
	{
		[ChildView]
		[AddOperationParameter]
		public TInterface Interface { get; set; }
	}

	public class InterfaceAttributeForAdd : InterfaceAttribute<InterfaceKey, InterfaceKey>
	{
		[JsonIgnore]
		public override int? InterfaceAttributeId
		{
			get
			{
				return base.InterfaceAttributeId;
			}
			set
			{
				base.InterfaceAttributeId = value;
			}
		}
	}

	public class InterfaceAttributeForMerge : InterfaceAttributeForAdd, IMergable
	{
		public MergeActions MergeAction { get; set; }
	}

	public class InterfaceAttributeForUpdateBase : InterfaceAttribute<InterfaceKey>
	{

	}

	public class InterfaceAttributeForUpdate : InterfaceAttributeForUpdateBase
	{
		[GlobalizedRequired("ADMIN_INTERFACEATTRIBUTE_INTERFACEATTRIBUTEID_REQUIRED")]
		public override int? InterfaceAttributeId
		{
			get
			{
				return base.InterfaceAttributeId;
			}
			set
			{
				base.InterfaceAttributeId = value;
			}
		}
	}

	public class InterfaceAttributeResult : InterfaceAttribute<InterfaceResult, InterfaceResult>
	{

	}
}
