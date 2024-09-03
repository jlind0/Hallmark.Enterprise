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
	public class ApplicationViewKey : IHasKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual int? ApplicationViewId { get; set; }

		[JsonIgnore]
		public int Key
		{
			get
			{
				return this.ApplicationViewId ?? 0;
			}
			set
			{
				this.ApplicationViewId = value;
			}
		}
	}

	public class ApplicationView<TDefaultSpec, TColumn, TColumnCollection> : ApplicationViewKey
		where TDefaultSpec: ApplicationViewSpec
		where TColumn: ApplicationViewColumnKey
		where TColumnCollection: IEnumerable<TColumn>
	{
		[ChildView]
		[AddOperationParameter]
		[UpdateOperationParameter]
		public TDefaultSpec DefaultSpec { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("APPLICATIONVIEW_NAME_REQUIRED")]
		public string Name { get; set; }
		public TColumnCollection Columns { get; set; }
	}

	public class ApplicationView<TDefaultSpec, TColumn, TColumnCollection, TDataView> : ApplicationView<TDefaultSpec, TColumn, TColumnCollection>
		where TDefaultSpec : ApplicationViewSpec
		where TColumn : ApplicationViewColumnKey
		where TColumnCollection : IEnumerable<TColumn>
		where TDataView: DataViewKey
	{
        [ChildView]
        [AddOperationParameter]
        public virtual TDataView DataView { get; set; }
	}

	public class ApplicationViewForAddBase : ApplicationView<ApplicationViewSpecForAddUpdate, ApplicationViewColumnForAddBase, IList<ApplicationViewColumnForAddBase>, DataViewKey>
	{
		[JsonIgnore]
		public override int? ApplicationViewId
		{
			get
			{
				return base.ApplicationViewId;
			}
			set
			{
				base.ApplicationViewId = value;
			}
		}
	}
    public class ApplicationViewForAdd : ApplicationViewForAddBase, IValidatableObject
    {
        [GlobalizedRequired("APPLICATIONVIEW_DATAVIEW_REQUIRED")]
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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.DataView == null || this.DataView.DataViewId == null)
                yield return ValidationResultFactory.Create(new ValidationResult("Data View Required"), "APPLICATIONVIEW_DATAVIEW_REQUIRED");
        }
    }
	public class ApplicationViewForUpdate : ApplicationView<ApplicationViewSpecForAddUpdate, ApplicationViewColumnForMerge,
        Merge<ApplicationViewColumnForAdd, ApplicationViewColumnForUpdate, ApplicationViewColumnKey, ApplicationViewColumnForMerge>>
	{
		[GlobalizedRequired("APPLICATIONVIEW_ID_REQUIRED")]
		public override int? ApplicationViewId
		{
			get
			{
				return base.ApplicationViewId;
			}
			set
			{
				base.ApplicationViewId = value;
			}
		}
	}

	public class ApplicationViewResult : ApplicationView<ApplicationViewSpecResult, ApplicationViewColumnResult, IList<ApplicationViewColumnResult>, DataViewResult> { }
}
