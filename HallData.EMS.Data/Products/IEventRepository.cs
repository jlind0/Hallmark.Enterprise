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
    public interface IReadOnlyEventRepository : IReadOnlyBrandedProductRepository<EventResult>
    {
		Task<QueryResults<TrackResult>> GetTracks(CustomerId customerId, Guid eventId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext<TrackResult> filter = null, SortContext<TrackResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		Task<QueryResults<JObject>> GetTracksView(CustomerId customerId, Guid eventId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		Task<QueryResults<SessionResult>> GetSessions(CustomerId customerId, Guid eventId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext<SessionResult> filter = null, SortContext<SessionResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		Task<QueryResults<JObject>> GetSessionsView(CustomerId customerId, Guid eventId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }

    public interface IEventRepository : IReadOnlyEventRepository, IBrandedProductRepository<EventResult, EventForAddBase, EventForUpdate> { }
}
