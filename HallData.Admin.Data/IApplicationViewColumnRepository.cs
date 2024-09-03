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
    public interface IReadOnlyApplicationViewColumnRepository : IReadOnlyRepository<ApplicationViewColumnResult>
    {
        Task<QueryResults<ApplicationViewColumnResult>> GetByApplicationView(int applicationViewId, string viewName = null, Guid? userId = null, FilterContext<ApplicationViewColumnResult> filter = null, SortContext<ApplicationViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    public interface IApplicationViewColumnRepository : IDeletableRepository<int, ApplicationViewColumnResult, ApplicationViewColumnForAdd, ApplicationViewColumnForUpdate>, IReadOnlyApplicationViewColumnRepository
    {
        
    }
}
