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
    public interface IReadOnlyApplicationViewImplementation : IReadOnlyBusinessImplementation<ApplicationViewResult>
    {
        [GetMethod]
        [ServiceRoute("DoesNameExist", "{name}/Exists/")]
        Task<bool> DoesNameExist(string name, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetByName", "byname/{name}/")]
        Task<QueryResult<ApplicationViewResult>> GetByName(string name, CancellationToken token = default(CancellationToken));
        Task<QueryResults<ApplicationViewResult>> GetByDataView(int dataViewId, string viewName = null, FilterContext<ApplicationViewResult> filter = null, SortContext<ApplicationViewResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    [Service("applicationviews")]
    public interface IApplicationViewImplementation  : IDeletableBusinessImplementationWithBase<int, ApplicationViewResult, ApplicationViewForAdd, ApplicationViewForUpdate>, IReadOnlyApplicationViewImplementation
    {
        [AddMethod]
        [ServiceRoute("Copy", "{sourceApplicationViewId}/copy/{targetName}/")]
        Task<QueryResult<ApplicationViewResult>> Copy(int sourceApplicationViewId, string targetName, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetColumns", "{applicationViewId}/columns/")]
        [ServiceRoute("GetColumnsTypedView", "{applicationViewId}/columns/TypedView/{viewName}/")]
        [ServiceRoute("GetColumnsTypedViewDefault", "{applicationViewId}/columns/TypedView/")]
        Task<QueryResults<ApplicationViewColumnResult>> GetColumns(int applicationViewId, string viewName = null, [JsonEncode]FilterContext<ApplicationViewColumnResult> filter = null, [JsonEncode]SortContext<ApplicationViewColumnResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    public interface IApplicationViewPassThroughImplementation : IApplicationViewImplementation { }
}
