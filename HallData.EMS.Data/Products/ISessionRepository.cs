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
    public interface IReadOnlySessionRepository : IReadOnlyProductRepository<SessionResult>
    {
		Task<QueryResult<SessionResult>> Get(CustomerId customerId, Guid eventId, Guid trackId, Guid sessionId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
		Task<QueryResult<JObject>> GetView(CustomerId customerId, Guid eventId, Guid trackId, Guid sessionId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }

    public interface ISessionRepository : IReadOnlySessionRepository, IProductRepository<SessionResult, SessionForAddBase, SessionForUpdate> { }
}
