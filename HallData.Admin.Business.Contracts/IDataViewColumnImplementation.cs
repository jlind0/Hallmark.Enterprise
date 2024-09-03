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
    public interface IReadOnlyDataViewColumnImplementation : IReadOnlyBusinessImplementation<DataViewColumnResult>
    {
        Task<QueryResults<DataViewColumnResult>> GetByDataView(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewColumnResult>> GetByDataViewResult(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewColumnResult>> GetByDataViewWithUpwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewColumnResult>> GetByDataViewResultWithUpwardRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewColumnResult>> GetByDataViewWithDownwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewColumnResult>> GetByDataViewResultWithDownwardRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewColumnResult>> GetByDataViewWithBiDirectionalRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewColumnResult>> GetByDataViewResultWithBiDirectionalRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetChildren", "{dataViewColumnId}/children/")]
        [ServiceRoute("GetChildrenTypedView", "{dataViewColumnId}/children/TypedView/{viewName}/")]
        [ServiceRoute("GetChildrenTypedViewDefault", "{dataViewColumnId}/children/TypedView/")]
        Task<QueryResults<DataViewColumnResult>> GetChildren(int dataViewColumnId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewColumnResult>> GetByInterfaceAttribute(int interfaceAttributeId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    [Service("dataviews/results/columns")]
    public interface IDataViewColumnImplementation : IDeletableBusinessImplementationWithBase<int, DataViewColumnResult, DataViewColumnForAdd, DataViewColumnForUpdate>, IReadOnlyDataViewColumnImplementation
    {
        [AddMethod]
        [ServiceRoute("AddPath", "paths/")]
        Task<QueryResult<DataViewColumnResult>> AddPath(DataViewColumnPathForAddUpdate path, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("UpdatePath", "paths/")]
        Task<QueryResult<DataViewColumnResult>> UpdatePath(DataViewColumnPathForAddUpdate path, CancellationToken token = default(CancellationToken));
        [DeleteMethod]
        [ServiceRoute("RemovePath", "{dataViewColumnId}/paths/")]
        [ServiceRoute("RemovePathOrder", "{dataViewColumnId}/paths/{orderIndex}")]
        Task<QueryResult<DataViewColumnResult>> RemovePath(int dataViewColumnId, int? orderIndex = null, CancellationToken token = default(CancellationToken));
        Task<bool> DoColumnsCompose(IEnumerable<DataViewColumnResult> columns, CancellationToken token = default(CancellationToken));
        Task<bool> AreColumnPathsValid(IEnumerable<DataViewColumnPathResult> paths, int interfaceAttributeId, CancellationToken token = default(CancellationToken));
        
    }
    public interface IDataViewColumnPassThroughImplementation : IDataViewColumnImplementation { }
}
