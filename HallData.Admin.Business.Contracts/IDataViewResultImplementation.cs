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
    public interface IReadOnlyDataViewResultImplementation : IReadOnlyBusinessImplementation<DataViewResultResult>
    {
        [GetMethod]
        [ServiceRoute("GetChildren", "{dataViewResultId}/children/")]
        [ServiceRoute("GetChildrenTypedView", "{dataViewResultId}/children/TypedView/{viewName}/")]
        [ServiceRoute("GetChildrenTypedViewDefault", "{dataViewResultId}/children/TypedView/")]
        Task<QueryResults<DataViewResultResult>> GetChildren(int dataViewResultId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewResultResult>> GetByDataView(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewResultResult>> GetByDataViewWithUpwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewResultResult>> GetByDataViewWithDownwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewResultResult>> GetByDataViewWithBiDirectionalRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewResultResult>> GetByCollectionAttribute(int collectionInterfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewResultResult>> GetByCollectionAttributeWithUpwardRecursion(int collectionInterfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewResultResult>> GetByCollectionAttributeWithDownwardRecursion(int collectionInterfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewResultResult>> GetByCollectionAttributeWithBiDirectionalRecursion(int collectionInterfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    [Service("dataviews/results")]
    public interface IDataViewResultImplementation : IDeletableBusinessImplementationWithBase<int, DataViewResultResult, DataViewResultForAdd, DataViewResultForUpdate>, IReadOnlyDataViewResultImplementation
    {
        [GetMethod]
        [ServiceRoute("GetColumns", "{dataViewResultId}/columns/")]
        [ServiceRoute("GetColumnsTypedViewDefault", "{dataViewResultId}/columns/TypedView/")]
        [ServiceRoute("GetColumnsTypedView", "{dataViewResultId}/columns/TypedView/{viewName}")]
        [ServiceRoute("GetColumnsRecurseNone", "{dataViewResultId}/columns/recurse/none/")]
        [ServiceRoute("GetColumnsRecurseNoneTypedViewDefault", "{dataViewResultId}/columns/recurse/none/TypedView/")]
        [ServiceRoute("GetColumnsRecurseNoneTypedView", "{dataViewResultId}/columns/recurse/none/TypedView/{viewName}")]
        Task<QueryResults<DataViewColumnResult>> GetColumns(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetColumnsRecurseUp", "{dataViewResultId}/columns/recurse/up/")]
        [ServiceRoute("GetColumnsRecurseUpTypedViewDefault", "{dataViewResultId}/columns/recurse/up/TypedView/")]
        [ServiceRoute("GetColumnsRecurseUpTypedView", "{dataViewResultId}/columns/recurse/up/TypedView/{viewName}")]
        Task<QueryResults<DataViewColumnResult>> GetColumnsWithUpwardRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetColumnsRecurseDown", "{dataViewResultId}/columns/recurse/down/")]
        [ServiceRoute("GetColumnsRecurseDownTypedViewDefault", "{dataViewResultId}/columns/recurse/down/TypedView/")]
        [ServiceRoute("GetColumnsRecurseDownTypedView", "{dataViewResultId}/columns/recurse/down/TypedView/{viewName}")]
        Task<QueryResults<DataViewColumnResult>> GetColumnsWithDownwardRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetColumnsRecurseBi", "{dataViewResultId}/columns/recurse/")]
        [ServiceRoute("GetColumnsRecurseBiTypedViewDefault", "{dataViewResultId}/columns/recurse/TypedView/")]
        [ServiceRoute("GetColumnsRecurseBiTypedView", "{dataViewResultId}/columns/recurse/TypedView/{viewName}")]
        Task<QueryResults<DataViewColumnResult>> GetColumnsWithBiDirectionalRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<bool> DoDataViewResultsCompose(IEnumerable<DataViewResultResult> results, CancellationToken token = default(CancellationToken));
    }
    public interface IDataViewResultPassThroughImplementation : IDataViewResultImplementation { }
}
