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
    public interface IReadOnlyInterfaceAttributeImplementation : IReadOnlyBusinessImplementation<InterfaceAttributeResult>
    {
        Task<QueryResults<InterfaceAttributeResult>> GetByInterface(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<InterfaceAttributeResult>> GetByInterfaceWithDownwardRecursion(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<InterfaceAttributeResult>> GetByInterfaceWithUpwardRecursion(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<InterfaceAttributeResult>> GetByInterfaceWithBiDirectionalRecursion(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    public interface IInterfaceAttributeImplementationBase : IBusinessImplementation
    {
        Task<bool> CanInterfaceAttributesCompose(IEnumerable<InterfaceAttributeResult> attributes, List<IEnumerable<int>> processedInterfaceRelations = null, CancellationToken token = default(CancellationToken));
    }
    [Service("interfaces/attributes")]
    public interface IInterfaceAttributeImplementation : IDeletableBusinessImplementationWithBase<int, InterfaceAttributeResult, InterfaceAttributeForAdd, InterfaceAttributeForUpdate>, IReadOnlyInterfaceAttributeImplementation, IInterfaceAttributeImplementationBase
    {
        [GetMethod]
        [ServiceRoute("GetDataViewResultsByCollection", "{interfaceAttributeId}/results/collections/")]
        [ServiceRoute("GetDataViewResultsByCollectionTypedViewDefault", "{interfaceAttributeId}/results/collections/TypedView/")]
        [ServiceRoute("GetDataViewResultsByCollectionTypedView", "{interfaceAttributeId}/results/collections/TypedView/{viewName}")]
        [ServiceRoute("GetColumnsRecurseNone", "{interfaceAttributeId}/results/collections/recurse/none/")]
        [ServiceRoute("GetColumnsRecurseNoneTypedViewDefault", "{interfaceAttributeId}/results/collections/recurse/none/TypedView/")]
        [ServiceRoute("GetColumnsRecurseNoneTypedView", "{interfaceAttributeId}/results/collections/recurse/none/TypedView/{viewName}")]
        Task<QueryResults<DataViewResultResult>> GetDataViewResultsByCollection(int interfaceAttributeId, string viewName = null, [JsonEncode]FilterContext<DataViewResultResult> filter = null, [JsonEncode]SortContext<DataViewResultResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetDataViewResultsByCollectionRecurseDown", "{interfaceAttributeId}/results/collections/recurse/down/")]
        [ServiceRoute("GetDataViewResultsByCollectionRecurseDownTypedViewDefault", "{interfaceAttributeId}/results/collections/recurse/down/TypedView/")]
        [ServiceRoute("GetDataViewResultsByCollectionRecurseDownTypedView", "{interfaceAttributeId}/results/collections/recurse/down/TypedView/{viewName}")]
        Task<QueryResults<DataViewResultResult>> GetDataViewResultsByCollectionWithDownwardRecursion(int interfaceAttributeId, string viewName = null, [JsonEncode]FilterContext<DataViewResultResult> filter = null, [JsonEncode]SortContext<DataViewResultResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetDataViewResultsByCollectionRecurseUp", "{interfaceAttributeId}/results/collections/recurse/up/")]
        [ServiceRoute("GetDataViewResultsByCollectionRecurseUpTypedViewDefault", "{interfaceAttributeId}/results/collections/recurse/up/TypedView/")]
        [ServiceRoute("GetDataViewResultsByCollectionRecurseUpTypedView", "{interfaceAttributeId}/results/collections/recurse/up/TypedView/{viewName}")]
        Task<QueryResults<DataViewResultResult>> GetDataViewResultsByCollectionWithUpwardRecursion(int interfaceAttributeId, string viewName = null, [JsonEncode]FilterContext<DataViewResultResult> filter = null, [JsonEncode]SortContext<DataViewResultResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetDataViewResultsByCollectionRecurseBi", "{interfaceAttributeId}/results/collections/recurse/")]
        [ServiceRoute("GetDataViewResultsByCollectionRecurseBiTypedViewDefault", "{interfaceAttributeId}/results/collections/recurse/TypedView/")]
        [ServiceRoute("GetDataViewResultsByCollectionRecurseBiTypedView", "{interfaceAttributeId}/results/collections/recurse/TypedView/{viewName}")]
        Task<QueryResults<DataViewResultResult>> GetDataViewResultsByCollectionWithBiDirectionalRecursion(int interfaceAttributeId, string viewName = null, [JsonEncode]FilterContext<DataViewResultResult> filter = null, [JsonEncode]SortContext<DataViewResultResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetColumns", "{interfaceAttributeId}/columns/")]
        [ServiceRoute("GetColumnsTypedView", "{interfaceAttributeId}/columns/TypedView/{viewName}/")]
        [ServiceRoute("GetColumnsTypedViewDefault", "{interfaceAttributeId}/columns/TypedView/")]
        Task<QueryResults<DataViewColumnResult>> GetColumns(int interfaceAttributeId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    public interface IInterfaceAttributePassThroughImplementation : IInterfaceAttributeImplementation { }
}
