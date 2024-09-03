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
    public interface IReadOnlyDataViewResultRepository : IReadOnlyRepository<DataViewResultResult>
    {
        Task<QueryResults<DataViewResultResult>> GetChildren(int dataViewResultId, string viewName = null, Guid? userId = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewResultResult>> GetByDataView(int dataViewId, int? orderIndex = null, RecursionLevel recursion = RecursionLevel.None, string viewName = null, Guid? userId = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<DataViewResultResult>> GetByCollectionAttribute(int collectionInterfaceAttributeId, RecursionLevel recursion = RecursionLevel.None, string viewName = null, Guid? userId = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    public interface IDataViewResultRepository : IDeletableRepository<int, DataViewResultResult, DataViewResultForAdd, DataViewResultForUpdate>, IReadOnlyDataViewResultRepository
    {
    }
}
