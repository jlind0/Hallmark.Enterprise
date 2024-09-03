using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.ApplicationViews;
using HallData.Business;
using System.Threading;

namespace HallData.Admin.Business
{
    public interface IReadOnlyDataViewImplementation : IReadOnlyBusinessImplementation<DataViewResult>
    {
        [GetMethod]
        [ServiceRoute("GetByName", "byname/{name}/")]
        Task<QueryResult<DataViewResult>> GetByName(string name, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetByName", "{name}/exists/")]
        Task<bool> DoesNameExist(string name, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("AreDataViewsCommon", "{dataViewId1}/relate/{dataViewId2}/")]
        Task<bool> AreDataViewsCommon(int dataViewId1, int dataViewId2, CancellationToken token = default(CancellationToken));
        Task<bool> AreDataViewsCommon(IEnumerable<int> dataViewIds, CancellationToken token = default(CancellationToken));
    }
    [Service("dataviews")]
    public interface IDataViewImplementation : IDeletableBusinessImplementationWithBase<int, DataViewResult, DataViewForAdd, DataViewForUpdate>, IReadOnlyDataViewImplementation 
    {
        [UpdateMethod]
        [ServiceRoute("RelateDataViews", "{dataViewId1}/relate/{relatedDataViewId}/")]
        Task<QueryResult<DataViewResult>> RelateDataViews(int dataViewId, int relatedDataViewId, CancellationToken token = default(CancellationToken));
        [DeleteMethod]
        [ServiceRoute("UnRelateDataViews", "{dataViewId1}/relate/{relatedDataViewId}/")]
        Task<QueryResult<DataViewResult>> UnRelateDataViews(int dataViewId, int relatedDataViewId, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetDataViewResults", "{dataViewId}/results/")]
        [ServiceRoute("GetDataViewResultsTypedViewDefault", "{dataViewId}/results/TypedView/")]
        [ServiceRoute("GetDataViewResultsTypedView", "{dataViewId}/results/TypedView/{viewName}")]
        [ServiceRoute("GetDataViewResultsByOrder", "{dataViewId}/results/{orderIndex}/")]
        [ServiceRoute("GetDataViewResultsByOrderTypedViewDefault", "{dataViewId}/results/{orderIndex}/TypedView/")]
        [ServiceRoute("GetDataViewResultsByOrderTypedView", "{dataViewId}/results/{orderIndex}/TypedView/{viewName}")]
        [ServiceRoute("GetDataViewResultsRecurseNone", "{dataViewId}/results/recurse/none/")]
        [ServiceRoute("GetDataViewResultsRecurseNoneTypedViewDefault", "{dataViewId}/recurse/none/results/TypedView/")]
        [ServiceRoute("GetDataViewResultsRecurseNoneTypedView", "{dataViewId}/results/recurse/none/TypedView/{viewName}")]
        [ServiceRoute("GetDataViewResultsRecurseNoneByOrder", "{dataViewId}/results/recurse/none/{orderIndex}/")]
        [ServiceRoute("GetDataViewResultsRecurseNoneByOrderTypedViewDefault", "{dataViewId}/results/recurse/none/{orderIndex}/TypedView/")]
        [ServiceRoute("GetDataViewResultsRecurseNoneByOrderTypedView", "{dataViewId}/results/recurse/none/{orderIndex}/TypedView/{viewName}")]
        Task<QueryResults<DataViewResultResult>> GetDataViewResults(int dataViewId, int? orderIndex = null, string viewName = null, [JsonEncode]FilterContext<DataViewResultResult> filter = null, [JsonEncode]SortContext<DataViewResultResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetDataViewResultsRecurseDown", "{dataViewId}/results/recurse/down/")]
        [ServiceRoute("GetDataViewResultsRecurseDownTypedViewDefault", "{dataViewId}/results/recurse/down/TypedView/")]
        [ServiceRoute("GetDataViewResultsRecurseDownTypedView", "{dataViewId}/results/recurse/down/TypedView/{viewName}")]
        [ServiceRoute("GetDataViewResultsRecurseDownByOrder", "{dataViewId}/results/recurse/down/{orderIndex}/")]
        [ServiceRoute("GetDataViewResultsRecurseDownByOrderTypedViewDefault", "{dataViewId}/results/recurse/down/{orderIndex}/TypedView/")]
        [ServiceRoute("GetDataViewResultsRecurseDownByOrderTypedView", "{dataViewId}/results/recurse/down/{orderIndex}/TypedView/{viewName}")]
        Task<QueryResults<DataViewResultResult>> GetDataViewResultsWithDownwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, [JsonEncode]FilterContext<DataViewResultResult> filter = null, [JsonEncode]SortContext<DataViewResultResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetDataViewResultsRecurseUp", "{dataViewId}/results/recurse/up/")]
        [ServiceRoute("GetDataViewResultsRecurseUpTypedViewDefault", "{dataViewId}/results/recurse/up/TypedView/")]
        [ServiceRoute("GetDataViewResultsRecurseUpTypedView", "{dataViewId}/results/recurse/up/TypedView/{viewName}")]
        [ServiceRoute("GetDataViewResultsRecurseUpByOrder", "{dataViewId}/results/recurse/up/{orderIndex}/")]
        [ServiceRoute("GetDataViewResultsRecurseUpByOrderTypedViewDefault", "{dataViewId}/results/recurse/up/{orderIndex}/TypedView/")]
        [ServiceRoute("GetDataViewResultsRecurseUpByOrderTypedView", "{dataViewId}/results/recurse/up/{orderIndex}/TypedView/{viewName}")]
        Task<QueryResults<DataViewResultResult>> GetDataViewResultsWithUpwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, [JsonEncode]FilterContext<DataViewResultResult> filter = null, [JsonEncode]SortContext<DataViewResultResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetDataViewResultsRecurseBi", "{dataViewId}/results/recurse/")]
        [ServiceRoute("GetDataViewResultsRecurseBiTypedViewDefault", "{dataViewId}/results/recurse/TypedView/")]
        [ServiceRoute("GetDataViewResultsRecurseBiTypedView", "{dataViewId}/results/recurse/TypedView/{viewName}")]
        [ServiceRoute("GetDataViewResultsRecurseBiByOrder", "{dataViewId}/results/recurse/{orderIndex}/")]
        [ServiceRoute("GetDataViewResultsRecurseBiByOrderTypedViewDefault", "{dataViewId}/results/recurse/{orderIndex}/TypedView/")]
        [ServiceRoute("GetDataViewResultsRecurseBiByOrderTypedView", "{dataViewId}/results/recurse/{orderIndex}/TypedView/{viewName}")]
        Task<QueryResults<DataViewResultResult>> GetDataViewResultsWithBiDirectionalRecursion(int dataViewId, int? orderIndex = null, string viewName = null, [JsonEncode]FilterContext<DataViewResultResult> filter = null, [JsonEncode]SortContext<DataViewResultResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetColumns", "{dataViewId}/columns/")]
        [ServiceRoute("GetColumnsTypedViewDefault", "{dataViewId}/columns/TypedView/")]
        [ServiceRoute("GetColumnsTypedView", "{dataViewId}/columns/TypedView/{viewName}")]
        [ServiceRoute("GetColumnsByOrder", "{dataViewId}/{orderIndex}/columns/")]
        [ServiceRoute("GetColumnsByOrderTypedViewDefault", "{dataViewId}/{orderIndex}/columns/TypedView/")]
        [ServiceRoute("GetColumnsByOrderTypedView", "{dataViewId}/{orderIndex}/columns/TypedView/{viewName}")]
        [ServiceRoute("GetColumnsRecurseNone", "{dataViewId}/columns/recurse/none/")]
        [ServiceRoute("GetColumnsRecurseNoneTypedViewDefault", "{dataViewId}/columnsrecurse/none/TypedView/")]
        [ServiceRoute("GetColumnsRecurseNoneTypedView", "{dataViewId}/columns/recurse/none/TypedView/{viewName}")]
        [ServiceRoute("GetColumnsRecurseNoneByOrder", "{dataViewId}/{orderIndex}/columns/recurse/none/")]
        [ServiceRoute("GetColumnsRecurseNoneByOrderTypedViewDefault", "{dataViewId}/{orderIndex}/columns/recurse/none/TypedView/")]
        [ServiceRoute("GetColumnsRecurseNoneByOrderTypedView", "{dataViewId}/{orderIndex}/columns/recurse/none/TypedView/{viewName}")]
        Task<QueryResults<DataViewColumnResult>> GetColumns(int dataViewId, int? orderIndex = null, string viewName = null, [JsonEncode]FilterContext<DataViewColumnResult> filter = null, [JsonEncode]SortContext<DataViewColumnResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetColumnsRecurseDown", "{dataViewId}/columns/recurse/down/")]
        [ServiceRoute("GetColumnsRecurseDownTypedViewDefault", "{dataViewId}/columns/recurse/down/TypedView/")]
        [ServiceRoute("GetColumnsRecurseDownTypedView", "{dataViewId}/columns/recurse/down/TypedView/{viewName}")]
        [ServiceRoute("GetColumnsRecurseDownByOrder", "{dataViewId}/{orderIndex}/columns/recurse/down/")]
        [ServiceRoute("GetColumnsRecurseDownByOrderTypedViewDefault", "{dataViewId}/{orderIndex}/columns/recurse/down/TypedView/")]
        [ServiceRoute("GetColumnsRecurseDownByOrderTypedView", "{dataViewId}/{orderIndex}/columns/recurse/down/TypedView/{viewName}")]
        Task<QueryResults<DataViewColumnResult>> GetColumnsWithDownwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, [JsonEncode]FilterContext<DataViewColumnResult> filter = null, [JsonEncode]SortContext<DataViewColumnResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetColumnsRecurseUp", "{dataViewId}/columns/recurse/up/")]
        [ServiceRoute("GetColumnsRecurseUpTypedViewDefault", "{dataViewId}/columns/recurse/up/TypedView/")]
        [ServiceRoute("GetColumnsRecurseUpTypedView", "{dataViewId}/columns/recurse/up/TypedView/{viewName}")]
        [ServiceRoute("GetColumnsRecurseUpByOrder", "{dataViewId}/{orderIndex}/columns/recurse/up/")]
        [ServiceRoute("GetColumnsRecurseUpByOrderTypedViewDefault", "{dataViewId}/{orderIndex}/columns/recurse/up/TypedView/")]
        [ServiceRoute("GetColumnsRecurseUpByOrderTypedView", "{dataViewId}/{orderIndex}/columns/recurse/up/TypedView/{viewName}")]
        Task<QueryResults<DataViewColumnResult>> GetColumnsWithUpwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, [JsonEncode]FilterContext<DataViewColumnResult> filter = null, [JsonEncode]SortContext<DataViewColumnResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetColumnsRecurseBi", "{dataViewId}/columns/recurse/")]
        [ServiceRoute("GetColumnsRecurseBiTypedViewDefault", "{dataViewId}/columns/recurse/TypedView/")]
        [ServiceRoute("GetColumnsRecurseBiTypedView", "{dataViewId}/columns/recurse/TypedView/{viewName}")]
        [ServiceRoute("GetColumnsRecurseBiByOrder", "{dataViewId}/{orderIndex}/columns/recurse/")]
        [ServiceRoute("GetColumnsRecurseBiByOrderTypedViewDefault", "{dataViewId}/{orderIndex}/columns/recurse/TypedView/")]
        [ServiceRoute("GetColumnsRecurseBiByOrderTypedView", "{dataViewId}/{orderIndex}/columns/recurse/TypedView/{viewName}")]
        Task<QueryResults<DataViewColumnResult>> GetColumnsWithBiDirectionalRecursion(int dataViewId, int? orderIndex = null, string viewName = null, [JsonEncode]FilterContext<DataViewColumnResult> filter = null, [JsonEncode]SortContext<DataViewColumnResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetApplicationViews", "{dataViewId}/applicationviews/")]
        [ServiceRoute("GetApplicationViewsTypedView", "{dataViewId}/applicationviews/TypedView/{viewName}/")]
        [ServiceRoute("GetApplicationViewsTypedViewDefault", "{dataViewId}/applicationviews/TypedView/")]
        Task<QueryResults<ApplicationViewResult>> GetApplicationViews(int dataViewId, string viewName = null, [JsonEncode]FilterContext<ApplicationViewResult> filter = null, [JsonEncode]SortContext<ApplicationViewResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("CanDataViewsRelate", "{dataViewId}/Relate/{relatedDataViewId}/")]
        Task<bool> CanDataViewsRelate(int dataViewId, int relatedDataViewId, CancellationToken token = default(CancellationToken));
        Task<bool> CanDataViewsRelate(IEnumerable<int> dataViewIds, CancellationToken token = default(CancellationToken));
    }
    public interface IDataViewPassThroughImplementation : IDataViewImplementation { } 
}
