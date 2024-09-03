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
	public class ApplicationViewColumnKey : IHasKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual int? ApplicationViewColumnId { get; set; }

		[JsonIgnore]
		public int Key
		{
			get
			{
				return this.ApplicationViewColumnId ?? 0;
			}
			set
			{
				this.ApplicationViewColumnId = value;
			}
		}
	}

	public class ApplicationViewColumn<TDefaultSpec> : ApplicationViewColumnKey
		where TDefaultSpec: ApplicationViewColumnSpec
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		public TDefaultSpec DefaultSpec { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? IsVisisbleByDefault { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? IsIncludedInResultByDefault { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? CanBeAddedToUI { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public int? ConfigureOrderIndex { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public int? DisplayOrderIndex { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public string RouteData { get; set; }
	}

	public class ApplicationViewColumn<TDefaultSpec, TApplicationView, TDataViewColumn> : ApplicationViewColumn<TDefaultSpec>, IValidatableObject
		where TApplicationView: ApplicationViewKey
		where TDefaultSpec: ApplicationViewColumnSpec
		where TDataViewColumn: DataViewColumnKey
	{
		[AddOperationParameter]
		[ChildView]
		public virtual TApplicationView ApplicationView { get; set; }

		[AddOperationParameter]
		[ChildView]
        [GlobalizedRequired("APPLICATIONVIEWCOLUMN_DATAVIEWCOLUMN_REQUIRED")]
		public TDataViewColumn DataViewColumn { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.DataViewColumn == null || this.DataViewColumn.DataViewColumnId == null)
                yield return ValidationResultFactory.Create(new ValidationResult("Data View Column Required"), "APPLICATIONVIEWCOLUMN_DATAVIEWCOLUMN_REQUIRED");
        }
    }

	public class ApplicationViewColumnForAddBase : ApplicationViewColumn<ApplicationViewColumnSpecForAddUpdate, ApplicationViewKey, DataViewColumnKey>
	{
		[JsonIgnore]
		public override int? ApplicationViewColumnId
		{
			get
			{
				return base.ApplicationViewColumnId;
			}
			set
			{
				base.ApplicationViewColumnId = value;
			}
		}
	}
    public class ApplicationViewColumnForAdd : ApplicationViewColumnForAddBase
    {
        [GlobalizedRequired("APPLICATIONVIEWCOLUMN_APPLICATIONVIEW_REQUIRED")]
        public override ApplicationViewKey ApplicationView
        {
            get
            {
                return base.ApplicationView;
            }
            set
            {
                base.ApplicationView = value;
            }
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var valid in base.Validate(validationContext))
                yield return valid;
            if(this.ApplicationView == null || this.ApplicationView.ApplicationViewId == null)
                yield return ValidationResultFactory.Create(new ValidationResult("Application View Required"), "APPLICATIONVIEWCOLUMN_APPLICATIONVIEW_REQUIRED");
        }
    }
	public class ApplicationViewColumnForMerge : ApplicationViewColumnForAddBase, IMergable
	{
		public MergeActions MergeAction { get; set; }
	}

	public class ApplicationViewColumnForUpdate : ApplicationViewColumn<ApplicationViewColumnSpecForAddUpdate>
	{
		[GlobalizedRequired("APPLICATIONVIEWCOLUMN_ID_REQUIRED")]
		public override int? ApplicationViewColumnId
		{
			get
			{
				return base.ApplicationViewColumnId;
			}
			set
			{
				base.ApplicationViewColumnId = value;
			}
		}
	}

	public class ApplicationViewColumnResult : ApplicationViewColumn<ApplicationViewColumnSpecResult, ApplicationViewResult, DataViewColumnResult> { }
}
