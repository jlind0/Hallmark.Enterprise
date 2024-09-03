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
    public interface IReadOnlyApplicationViewRepository : IReadOnlyRepository<ApplicationViewResult>
    {
        Task<QueryResult<ApplicationViewResult>> GetByName(string name, Guid? userId = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<ApplicationViewResult>> GetByDataView(int dataViewId, string viewName = null, Guid? userId = null, FilterContext<ApplicationViewResult> filter = null, SortContext<ApplicationViewResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    public interface IApplicationViewRepository : IDeletableRepository<int, ApplicationViewResult, ApplicationViewForAdd, ApplicationViewForUpdate>, IReadOnlyApplicationViewRepository
    {
        Task<int> Copy(int sourceApplicationViewId, string targetName, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }
}
