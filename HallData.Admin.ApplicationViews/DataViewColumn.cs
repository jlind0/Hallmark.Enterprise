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
	public class DataViewColumnKey : IHasKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual int? DataViewColumnId { get; set; }

		[JsonIgnore]
		public int Key
		{
			get
			{
				return this.DataViewColumnId ?? 0;
			}
			set
			{
				this.DataViewColumnId = value;
			}
		}
	}

	public class DataViewColumn<TInterfaceAttribute, TDataViewColumnPath, TDataViewColumnPathCollection, TParentColumn> : DataViewColumnKey, IValidatableObject
		where TInterfaceAttribute: InterfaceAttributeKey
		where TDataViewColumnPath: DataViewColumnPathKey
		where TDataViewColumnPathCollection : IEnumerable<TDataViewColumnPath>
        where TParentColumn: DataViewColumnKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string ResultName { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? IsRequired { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("ADMIN_DATAVIEWCOLUMN_NAME_REQUIRED")]
		public string Name { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		[GlobalizedRequired("ADMIN_DATAVIEWCOLUMN_INTERFACEATTRIBUTE_REQUIRED")]
		public TInterfaceAttribute InterfaceAttribute { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? IsVirtual { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public string Alias { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool IsCalculated { get; set; }
        [ChildView]
        [AddOperationParameter]
        [UpdateOperationParameter]
        public TParentColumn ParentResultColumn { get; set; }
		public TDataViewColumnPathCollection DataViewColumnPaths { get; set; }

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (this.InterfaceAttribute == null || this.InterfaceAttribute.InterfaceAttributeId == null)
				yield return ValidationResultFactory.Create(new ValidationResult("Interface Attribute Required"), "ADMIN_DATAVIEWCOLUMN_INTERFACEATTRIBUTE_REQUIRED");
            if (!this.IsCalculated && string.IsNullOrEmpty(this.ResultName))
                yield return ValidationResultFactory.Create(new ValidationResult("Result Name Required for Non-Calculated"), "ADMIN_DATAVIEWCOLUMN_RESULTNAME_REQUIRED");
		}
	}

	public class DataViewColumn<TInterfaceAttribute, TDataViewColumnPath, TDataViewColumnPathCollection, TParentColumn, TDataViewResult> : DataViewColumn<TInterfaceAttribute, TDataViewColumnPath, TDataViewColumnPathCollection, TParentColumn>
		where TInterfaceAttribute : InterfaceAttributeKey
		where TDataViewColumnPath : DataViewColumnPathKey
		where TDataViewColumnPathCollection : IEnumerable<TDataViewColumnPath>
		where TDataViewResult : DataViewResultKey
        where TParentColumn: DataViewColumnKey
	{
		[AddOperationParameter]
		[ChildView]
		public virtual TDataViewResult DataViewResult { get; set; }
        
	}
    
	public class DataViewColumnForAddBase : DataViewColumn<InterfaceAttributeKey, DataViewColumnPathForAddUpdateDataViewColumn, IList<DataViewColumnPathForAddUpdateDataViewColumn>, DataViewColumnKey, DataViewResultKey>
	{
		public DataViewColumnForAddBase()
		{
			this.DataViewColumnPaths = new List<DataViewColumnPathForAddUpdateDataViewColumn>();
		}
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
    public class DataViewColumnForAdd : DataViewColumnForAddBase
    {
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var valid in base.Validate(validationContext))
                yield return valid;
            if (this.DataViewResult == null || this.DataViewResult.DataViewResultId == null)
                yield return ValidationResultFactory.Create(new ValidationResult("Data View Reuqired"), "ADMIN_DATAVIEWCOLUMN_DATAVIEWRESULT_REQUIRED");
        }
        [GlobalizedRequired("ADMIN_DATAVIEWCOLUMN_DATAVIEWRESULT_REQUIRED")]
        public override DataViewResultKey DataViewResult
        {
            get
            {
                return base.DataViewResult;
            }
            set
            {
                base.DataViewResult = value;
            }
        }
    }
	public class DataViewColumnForMerge : DataViewColumnForAddBase, IMergable
	{
		public MergeActions MergeAction { get; set; }
	}

	public class DataViewColumnForUpdate : DataViewColumn<InterfaceAttributeKey, DataViewColumnPathForMerge, 
		Merge<DataViewColumnPathForAddUpdateDataViewColumn, DataViewColumnPathForAddUpdateDataViewColumn, DataViewColumnPathKey, DataViewColumnPathForMerge>, DataViewColumnKey>
	{
		public DataViewColumnForUpdate()
		{
			this.DataViewColumnPaths = new Merge<DataViewColumnPathForAddUpdateDataViewColumn, DataViewColumnPathForAddUpdateDataViewColumn, DataViewColumnPathKey, DataViewColumnPathForMerge>();
		}
	}

	[DefaultView("DataViewColumnResults")]
	public class DataViewColumnResult : DataViewColumn<InterfaceAttributeResult, DataViewColumnPathResult, IList<DataViewColumnPathResult>, DataViewColumnResult, DataViewResultResult>
	{
		public DataViewColumnResult()
		{
			this.DataViewColumnPaths = new List<DataViewColumnPathResult>();
		}
	}
}
