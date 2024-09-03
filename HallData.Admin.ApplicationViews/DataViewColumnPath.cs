using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;
using HallData.Utilities;
using System.ComponentModel.DataAnnotations;


namespace HallData.Admin.ApplicationViews
{
	public struct DataViewColumnPathId
	{
		public int DataViewColumnId { get; set; }
		public int PathOrderIndex { get; set; }

		public DataViewColumnPathId(int dataViewColumnId, int pathOrderIndex) : this()
		{
			this.DataViewColumnId = dataViewColumnId;
			this.PathOrderIndex = pathOrderIndex;
		}

		public override bool Equals(object obj)
		{
			DataViewColumnPathId? id = obj as DataViewColumnPathId?;
			if (id == null)
				return false;
			return id.Value.DataViewColumnId == this.DataViewColumnId && id.Value.PathOrderIndex == this.PathOrderIndex;
		}

		public override int GetHashCode()
		{
			return HashCodeProvider.BuildHashCode(this.DataViewColumnId, this.PathOrderIndex);
		}
	}

	public class DataViewColumnPathKey : IHasKey<DataViewColumnPathId>
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual int? DataViewColumnId { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		[GlobalizedRequired("ADMIN_DATAVIEWCOLUMNPATH_PATHORDERINDEX_REQUIRED")]
		public int? PathOrderIndex { get; set; }
		
		[JsonIgnore]
		public DataViewColumnPathId Key
		{
			get
			{
				return new DataViewColumnPathId(this.DataViewColumnId ?? 0, this.PathOrderIndex ?? 0);
			}
			set
			{
				this.DataViewColumnId = value.DataViewColumnId;
				this.PathOrderIndex = value.PathOrderIndex;
			}
		}
	}

	public class DataViewColumnPath<TInterfaceAttribute> : DataViewColumnPathKey, IValidatableObject
		where TInterfaceAttribute: InterfaceAttributeKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		[GlobalizedRequired("ADMIN_DATAVIEWCOLUMNPATH_INTERFACEATTRIBUTE_REQUIRED")]
		public InterfaceAttributeKey InterfaceAttribute { get; set; }
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (this.InterfaceAttribute == null || this.InterfaceAttribute.InterfaceAttributeId == null)
				yield return ValidationResultFactory.Create(new ValidationResult("Interface Attribute required"), "ADMIN_DATAVIEWCOLUMNPATH_INTERFACEATTRIBUTE_REQUIRED");
		}
	}

	public class DataViewColumnPathForAddUpdateBase : DataViewColumnPath<InterfaceAttributeKey>
	{
		
	}

	public class DataViewColumnPathForAddUpdate : DataViewColumnPathForAddUpdateBase
	{
		[GlobalizedRequired("ADMIN_DATAVIEWCOLUMNPATH_DATAVIEWCOLUMNID_REQUIRED")]
		public override int? DataViewColumnId
		{
			get
			{
				return base.DataViewColumnId;
			}
			set
			{
				base.DataViewColumnId = value;
			}
		}
	}

	public class DataViewColumnPathForMerge : DataViewColumnPathForAddUpdateBase, IMergable
	{
		public MergeActions MergeAction { get; set; }
	}

	public class DataViewColumnPathForAddUpdateDataViewColumn : DataViewColumnPathForAddUpdateBase
	{
		[JsonIgnore]
		public override int? DataViewColumnId
		{
			get
			{
				return base.DataViewColumnId;
			}
			set
			{
				base.DataViewColumnId = value;
			}
		}
	}

	public class DataViewColumnPathResult : DataViewColumnPath<InterfaceAttributeResult>
	{
	}
}
