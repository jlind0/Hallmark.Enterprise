using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.Repository;
using System.Threading;
using HallData.ApplicationViews;

namespace HallData.Admin.Data
{
    public interface IReadOnlyDataViewColumnRepository : IReadOnlyRepository<DataViewColumnResult>
    {
        Task<QueryResults<DataViewColumnResult>> GetByDataView(int dataViewId, int? orderIndex = null, RecursionLevel recursion = RecursionLevel.None, string viewName = null, Guid? userId = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewColumnResult>> GetByDataViewResult(int dataViewResultId, RecursionLevel recursion = RecursionLevel.None, string viewName = null, Guid? userId = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewColumnResult>> GetByParent(int parentResultColumnId, string viewName = null, Guid? userId = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewColumnResult>> GetByInterfaceAttribute(int interfaceAttributeId, string viewName = null, Guid? userId = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    public interface IDataViewColumnRepository : IDeletableRepository<int, DataViewColumnResult, DataViewColumnForAdd, DataViewColumnForUpdate>, IReadOnlyDataViewColumnRepository
    {
        Task AddPath(DataViewColumnPathForAddUpdate path, Guid? userId = null, CancellationToken token = default(CancellationToken));
        Task UpdatePath(DataViewColumnPathForAddUpdate path, Guid? userId = null, CancellationToken token = default(CancellationToken));
        Task RemovePath(int dataViewColumnId, int? orderIndex = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }
}
