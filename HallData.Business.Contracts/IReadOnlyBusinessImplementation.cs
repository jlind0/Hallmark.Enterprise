using System.Threading;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using Newtonsoft.Json.Linq;

namespace HallData.Business
{
	public interface IReadOnlyBusinessImplementation<in TKey, TView, TModel> : IBusinessImplementation
	{
		[GetMethod]
		[ServiceRoute("GetMany", "")]
		[ServiceRoute("GetManyTypedView", "TypedView/{viewName}/")]
		[ServiceRoute("GetManyTypedViewDefault", "TypedView/")]
		[Description("Gets strongly typed view for implementing service")]
		Task<QueryResults<TModel>> GetMany(string viewName = null, [Description("Filter for the view results, must be url encoded JSON")][JsonEncode]FilterContext<TView> filter = null,
			[Description("Sort for the view results, must be url encoded JSON")][JsonEncode]SortContext<TView> sort = null,
			[Description("Page for the view results, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		
		[GetMethod]
		[ServiceRoute("GetManyView", "View/{viewName}")]
		[ServiceRoute("GetManyViewDefault", "View/")]
		[Description("Gets a dynamically typed view for the implementing service")]
		Task<QueryResults<JObject>> GetManyView([Description("The name of the view")]string viewName = null,
			[Description("Filter for the view results, must be url encoded JSON")][JsonEncode]FilterContext filter = null,
			[Description("Sort for the view results, must be url encoded JSON")][JsonEncode]SortContext sort = null,
			[Description("Page for the view results, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
	   
		[GetMethod]
		[ServiceRoute("Get", "{id}/")]
		[ServiceRoute("GetTypedViewDefault", "{id}/TypedView/")]
		[Description("Gets a strongly typed view by id for implementing service")]
        Task<QueryResult<TModel>> Get([Description("The target id")]TKey id, CancellationToken token = default(CancellationToken));
		
		[GetMethod]
		[ServiceRoute("GetViewDefault", "{id}/View/")]
		[Description("Gets a dynamically typed view by id for implementing service")]
		Task<QueryResult<JObject>> GetView([Description("The target id")]TKey id, CancellationToken token = default(CancellationToken));
	}

	public interface IReadOnlyBusinessImplementation<in TKey, TView> : IReadOnlyBusinessImplementation<TKey, TView, TView> { }
	public interface IReadOnlyBusinessImplementation<TView> : IReadOnlyBusinessImplementation<int, TView> { }
}