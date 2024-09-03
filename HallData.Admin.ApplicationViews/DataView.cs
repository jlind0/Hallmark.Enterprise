using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;

namespace HallData.Admin.ApplicationViews
{
	public class DataViewKey : IHasKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual int? DataViewId { get; set; }

		[JsonIgnore]
		public int Key
		{
			get
			{
				return this.DataViewId ?? 0;
			}
			set
			{
				this.DataViewId = value;
			}
		}
	}

	public class DataViewForMerge : DataViewKey, IRelationshipMergeable
	{
		public RelationshipMergeActions MergeAction { get; set; }

		[GlobalizedRequired("ADMIN_DATAVIEW_DATAVIEWID_REQUIRED")]
		public override int? DataViewId
		{
			get
			{
				return base.DataViewId;
			}
			set
			{
				base.DataViewId = value;
			}
		}
	}

	public class DataView<TRelatedDataView, TRelatedDataViewCollection, TDataViewResult, TDataViewResultCollection> : DataViewKey
		where TRelatedDataView: DataViewKey
		where TRelatedDataViewCollection : IEnumerable<TRelatedDataView>
		where TDataViewResult: DataViewResultKey
		where TDataViewResultCollection: IEnumerable<TDataViewResult>
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("ADMIN_DATAVIEW_NAME_REQUIRED")]
		public string Name { get; set; }
		public TRelatedDataViewCollection RelatedDataViews { get; set; }
		public TDataViewResultCollection DataViewResults { get; set; }
	}

	public class DataViewForAdd : DataView<DataViewKey, IList<DataViewKey>, DataViewResultForAddBase, IList<DataViewResultForAddBase>>
	{
		public DataViewForAdd()
		{
			this.RelatedDataViews = new List<DataViewKey>();
			this.DataViewResults = new List<DataViewResultForAddBase>();
		}

		[JsonIgnore]
		public override int? DataViewId
		{
			get
			{
				return base.DataViewId;
			}
			set
			{
				base.DataViewId = value;
			}
		}
	}

	public class DataViewForUpdate : DataView<DataViewForMerge, RelationshipMerge<DataViewKey, DataViewForMerge>, DataViewResultForMerge, 
		Merge<DataViewResultForAddBase, DataViewResultForUpdate, DataViewResultKey, DataViewResultForMerge>>
	{
		public DataViewForUpdate()
		{
			this.RelatedDataViews = new RelationshipMerge<DataViewKey,DataViewForMerge>();
			this.DataViewResults = new Merge<DataViewResultForAddBase, DataViewResultForUpdate, DataViewResultKey, DataViewResultForMerge>();
		}

		[GlobalizedRequired("ADMIN_DATAVIEW_DATAVIEWID_REQUIRED")]
		public override int? DataViewId
		{
			get
			{
				return base.DataViewId;
			}
			set
			{
				base.DataViewId = value;
			}
		}
	}

	[DefaultView("DataViewResults")]
	public class DataViewResult : DataView<DataViewResult, IList<DataViewResult>, DataViewResultResult, IList<DataViewResultResult>>
	{
		public DataViewResult()
		{
			this.RelatedDataViews = new List<DataViewResult>();
			this.DataViewResults = new List<DataViewResultResult>();
		}
	}
}
