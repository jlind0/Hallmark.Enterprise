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
    public interface IReadOnlyDataViewRepository : IReadOnlyRepository<DataViewResult>
    {
        Task<QueryResult<DataViewResult>> GetByName(string name, Guid? userId = null, CancellationToken token = default(CancellationToken));
        Task<bool> AreDataViewsCommon(int dataViewId1, int dataViewId2, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }
    public interface IDataViewRepository : IDeletableRepository<int, DataViewResult, DataViewForAdd, DataViewForUpdate>, IReadOnlyDataViewRepository
    {
        Task RelateDataViews(int dataViewId, int relatedDataViewId, Guid? userId = null, CancellationToken token = default(CancellationToken));
        Task UnRelateDataViews(int dataViewId, int relatedDataViewId, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }
}
