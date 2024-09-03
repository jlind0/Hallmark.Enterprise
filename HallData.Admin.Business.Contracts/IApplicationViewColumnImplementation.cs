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
    public interface IReadOnlyApplicationViewColumnImplementation : IReadOnlyBusinessImplementation<ApplicationViewColumnResult>
    {
        Task<QueryResults<ApplicationViewColumnResult>> GetByApplicationView(int applicationViewId, string viewName = null, FilterContext<ApplicationViewColumnResult> filter = null, SortContext<ApplicationViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    [Service("applicationviews/columns")]
    public interface IApplicationViewColumnImplementation : IDeletableBusinessImplementationWithBase<int, ApplicationViewColumnResult, ApplicationViewColumnForAdd, ApplicationViewColumnForUpdate>, IReadOnlyApplicationViewColumnImplementation
    {
    }
    public interface IApplicationViewColumnPassThroughImplementation : IApplicationViewColumnImplementation { }
}
