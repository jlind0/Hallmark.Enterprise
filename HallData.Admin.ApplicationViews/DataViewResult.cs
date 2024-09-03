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
	public class DataViewResultKey : IHasKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual int? DataViewResultId { get; set; }
		public int Key
		{
			get
			{
				return this.DataViewResultId ?? 0;
			}
			set
			{
				this.DataViewResultId = value;
			}
		}
	}

	public class DataViewResult<TParent, TInterface, TCollectionAttribute, TDataViewColumn, TDataViewColumnCollection> : DataViewResultKey, IValidatableObject
		where TInterface: InterfaceKey
		where TCollectionAttribute: InterfaceAttributeKey
		where TParent: DataViewResultKey
		where TDataViewColumn : DataViewColumnKey
		where TDataViewColumnCollection : IEnumerable<TDataViewColumn>
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("ADMIN_DATAVIEWRESULT_RESULTINDEX_REQUIRED")]
		public int? ResultIndex { get; set; }

		[ChildView]
		[AddOperationParameter]
		[UpdateOperationParameter]
		public TParent Parent { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("ADMIN_DATAVIEWRESULT_INTERFACE_REQUIRED")]
		[ChildView]
		public TInterface Interface { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		public TCollectionAttribute CollectionInterfaceAttribute { get; set; }
		public TDataViewColumnCollection DataViewColumns { get; set; }

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (this.Interface == null || this.Interface.InterfaceId == null)
				yield return ValidationResultFactory.Create(new ValidationResult("Interface must be populated on Data View Result"), "ADMIN_DATAVIEWRESULT_INTERFACE_REQUIRED");
			if (this.ResultIndex < 0)
				yield return ValidationResultFactory.Create(new ValidationResult("Result Index must be grater or equal to zero"), "ADMIN_DATAVIEWRESULT_RESULTINDEX_INVALID");
			if (this.ResultIndex > 0 && (this.CollectionInterfaceAttribute == null || this.CollectionInterfaceAttribute.InterfaceAttributeId == null))
				yield return ValidationResultFactory.Create(new ValidationResult("Collection Interface Attribute must be populated on result index's greater than zero"), "ADMIN_DATAVIEWRESULT_COLLECTIONINTERFACEATTRIBUTE_REQUIRED");
		}
	}

	public class DataViewResult<TParent, TInterface, TCollectionAttribute, TDataViewColumn, TDataViewColumnCollection, TDataView> : DataViewResult<TParent, TInterface, TCollectionAttribute, TDataViewColumn, TDataViewColumnCollection>
		where TInterface: InterfaceKey
		where TCollectionAttribute: InterfaceAttributeKey
		where TParent: DataViewResultKey
		where TDataView: DataViewKey
		where TDataViewColumn : DataViewColumnKey
		where TDataViewColumnCollection : IEnumerable<TDataViewColumn>
	{
		[ChildView]
		[AddOperationParameter]
		public virtual TDataView DataView { get; set; }

		
	}

	public class DataViewResultForAddBase : DataViewResult<DataViewResultKey, InterfaceKey, InterfaceAttributeKey, DataViewColumnForAddBase, IList<DataViewColumnForAddBase>, DataViewKey>
	{
		public DataViewResultForAddBase()
		{
			this.DataViewColumns = new List<DataViewColumnForAddBase>();
		}

		[JsonIgnore]
		public override int? DataViewResultId
		{
			get
			{
				return base.DataViewResultId;
			}
			set
			{
				base.DataViewResultId = value;
			}
		}
	}
    public class DataViewResultForAdd : DataViewResultForAddBase
    {
        [GlobalizedRequired("ADMIN_DATAVIEWRESULT_DATAVIEW_REQUIRED")]
        public override DataViewKey DataView
        {
            get
            {
                return base.DataView;
            }
            set
            {
                base.DataView = value;
            }
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var valid in base.Validate(validationContext))
                yield return valid;
            if (this.DataView == null || this.DataView.DataViewId == null)
                yield return ValidationResultFactory.Create(new ValidationResult("Data View is required"), "ADMIN_DATAVIEWRESULT_DATAVIEW_REQUIRED");
        }
    }
	public class DataViewResultForMerge : DataViewResultForAddBase, IMergable
	{
		public MergeActions MergeAction { get; set; }
	}

	public class DataViewResultForUpdate : DataViewResult<DataViewResultKey, InterfaceKey, InterfaceAttributeKey, DataViewColumnForMerge, 
		Merge<DataViewColumnForAddBase, DataViewColumnForUpdate, DataViewColumnKey, DataViewColumnForMerge>>
	{
        public DataViewResultForUpdate()
		{
			this.DataViewColumns = new Merge<DataViewColumnForAddBase, DataViewColumnForUpdate, DataViewColumnKey, DataViewColumnForMerge>();
		}

		[GlobalizedRequired("ADMIN_DATAVIEWRESULT_DATAVIEWRESULTID_REQUIRED")]
		public override int? DataViewResultId
		{
			get
			{
				return base.DataViewResultId;
			}
			set
			{
				base.DataViewResultId = value;
			}
		}
	}
	[DefaultView("DataViewResultResults")]
	public class DataViewResultResult : DataViewResult<DataViewResultResult, InterfaceResult, InterfaceAttributeResult, DataViewColumnResult, IList<DataViewColumnResult>, DataViewResult>
	{
		public DataViewResultResult()
		{
			this.DataViewColumns = new List<DataViewColumnResult>();
		}
	}
}
