using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.ApplicationViews.Enums;
using HallData.Repository;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Data
{
    public interface IReadOnlyTrackRepository : IReadOnlyProductRepository<TrackResult>
    {
		Task<QueryResults<SessionResult>> GetSessions(CustomerId customerId, Guid eventId, Guid trackId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext<SessionResult> filter = null, SortContext<SessionResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		Task<QueryResults<JObject>> GetSessionsView(CustomerId customerId, Guid eventId, Guid trackId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		Task<QueryResult<TrackResult>> Get(CustomerId customerId, Guid eventId, Guid trackId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
		Task<QueryResult<JObject>> GetView(CustomerId customerId, Guid eventId, Guid trackId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }
    public interface ITrackRepository : IReadOnlyTrackRepository, IProductRepository<TrackResult, TrackForAddBase, TrackForUpdate> { }
}
