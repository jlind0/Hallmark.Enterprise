using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace HallData.Business
{
	public interface IBusinessImplementation
	{
		Guid? CurrentSessionId { get; set; }
		Task<bool> IsCurrentSessionActive(CancellationToken token = default(CancellationToken));
		bool IsCurrentSessionActiveSync();
	}
	public interface IBusinessImplementation<in TKey, TView, TModel, in TModelForAdd, in TModelForUpdate> : IReadOnlyBusinessImplementation<TKey, TView, TModel>
		where TView: IHasKey<TKey>
		where TModel : IHasKey<TKey>
		where TModelForAdd : IHasKey<TKey>
		where TModelForUpdate: IHasKey<TKey>
	{
		[AddMethod]
		[ServiceRoute("Add", "")]
		[ServiceRoute("AddTypedDefault", "TypedView/")]
		[Description("Adds a view using the implementing service")]
		Task<QueryResult<TModel>> Add([Description("View to be added")][Content]TModelForAdd view, CancellationToken token = default(CancellationToken));
	   
		[AddMethod]
		[ServiceRoute("AddViewDefault", "View/")]
		[Description("Adds a view using the implementing service")]
		Task<QueryResult<JObject>> AddView([Description("View to be added")][Content]TModelForAdd view, CancellationToken token = default(CancellationToken));
		
		[UpdateMethod]
		[ServiceRoute("Update", "")]
		[ServiceRoute("UpdateTypedDefault", "TypedView/")]
		[Description("Upades a view using the implementing service")]
		Task<QueryResult<TModel>> Update([Description("View to be updated")][Content]TModelForUpdate view, CancellationToken token = default(CancellationToken));
		
		[UpdateMethod]
		[ServiceRoute("UpdateViewDefault", "View/")]
		[Description("Upades a view using the implementing service")]
		Task<QueryResult<JObject>> UpdateView([Description("View to be updated")][Content]TModelForUpdate view, CancellationToken token = default(CancellationToken));
		
		[UpdateMethod]
		[ServiceRoute("ChangeStatus", "{id}/Status/{statusTypeName}/")]
		[ServiceRoute("ChangeStatusTypedViewDefault", "{id}/Status/{statusTypeName}/TypedView/")]
		[Description("Changes the status, if no warning, of a view using the implementing service")]
		Task<ChangeStatusQueryResult<TModel>> ChangeStatus([Description("The target id")]TKey id, string statusTypeName, CancellationToken token = default(CancellationToken));
		
		[UpdateMethod]
		[ServiceRoute("ChangeStatusViewDefault", "{id}/Status/{statusTypeName}/View/")]
		[Description("Changes the status, if no warning, of a view using the implementing service")]
		Task<ChangeStatusQueryResult<JObject>> ChangeStatusView([Description("The target id")]TKey id, string statusTypeName, CancellationToken token = default(CancellationToken));
		
		[UpdateMethod]
		[ServiceRoute("ChangeStatusForce", "{id}/Status/{statusTypeName}/Force/")]
		[ServiceRoute("ChangeStatusForceTypedViewDefault", "{id}/Status/{statusTypeName}/Force/TypedView/")]
		[Description("Changes the status, ignoring warnings, of a view using the implementing service")]
		Task<ChangeStatusQueryResult<TModel>> ChangeStatusForce([Description("The target id")]TKey id, string statusTypeName, CancellationToken token = default(CancellationToken));
		
		[UpdateMethod]
		[ServiceRoute("ChangeStatusForceViewDefault", "{id}/Status/{statusTypeName}/View/")]
		[Description("Changes the status, ignoring warnings, of a view using the implementing service")]
		Task<ChangeStatusQueryResult<JObject>> ChangeStatusForceView([Description("The target id")]TKey id, string statusTypeName, CancellationToken token = default(CancellationToken));
	}

	public interface IBusinessImplementation<in TKey, TView, TModel, in TModelForAdd, in TModelForUpdate, in TModelForAddRelationship, in TModelForUpdateRelationship> :
		IBusinessImplementation<TKey, TView, TModel, TModelForAdd, TModelForUpdate>
		where TView: IHasKey<TKey>
		where TModel : IHasKey<TKey>
		where TModelForAdd : IHasKey<TKey>
		where TModelForUpdate: IHasKey<TKey>
		where TModelForAddRelationship: IHasKey<TKey>
		where TModelForUpdateRelationship: IHasKey<TKey>
	{
		[AddMethod]
		[ServiceRoute("AddRelationship", "Relationship/")]
		[ServiceRoute("AddRelationshipTypedDefault", "Relationship/TypedView/")]
		[Description("Adds a view using the implementing service")]
		Task<QueryResult<TModel>> AddRelationship([Description("View to be added")][Content]TModelForAdd view, CancellationToken token = default(CancellationToken));
		
		[AddMethod]
		[ServiceRoute("AddRelationshipViewDefault", "Relationship/View/")]
		[Description("Adds a view using the implementing service")]
		Task<QueryResult<JObject>> AddRelationshipView([Description("View to be added")][Content]TModelForAdd view, CancellationToken token = default(CancellationToken));
		
		[UpdateMethod]
		[ServiceRoute("UpdateRelationship", "Relationship/")]
		[ServiceRoute("UpdateRelationshipTypedDefault", "Relationship/TypedView/")]
		[Description("Upades a view using the implementing service")]
		Task<QueryResult<TModel>> UpdateRelationship([Description("View to be updated")][Content]TModelForUpdate view, CancellationToken token = default(CancellationToken));
		
		[UpdateMethod]
		[ServiceRoute("UpdateRelationshipViewDefault", "Relationship/View/")]
		[Description("Upades a view using the implementing service")]
		Task<QueryResult<JObject>> UpdateRelationshipView([Description("View to be updated")][Content]TModelForUpdate view, CancellationToken token = default(CancellationToken));
	}

	public interface IBusinessImplementation<in TKey, TView, TModel, in TModelBase> : IBusinessImplementation<TKey, TView, TModel, TModelBase, TModelBase>
		where TView: IHasKey<TKey>
		where TModel: IHasKey<TKey>
		where TModelBase : IHasKey<TKey> { }

	public interface IBusinessImplementation<in TKey, TView, TModel> : IBusinessImplementation<TKey, TView, TModel, TModel>
		where TView: IHasKey<TKey>
		where TModel: IHasKey<TKey>
	{ }

	public interface IBusinessImplementation<in TKey, TView> : IBusinessImplementation<TKey, TView, TView>, IReadOnlyBusinessImplementation<TKey, TView>, IBusinessImplementationWithBase<TKey, TView, TView>
		where TView : IHasKey<TKey> { }

	public interface IBusinessImplementation<TView> : IBusinessImplementation<int, TView>, IReadOnlyBusinessImplementation<TView>
		where TView : IHasKey<int> { }
}
