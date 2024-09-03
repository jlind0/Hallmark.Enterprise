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
    public interface IReadOnlyPublicationRepository : IReadOnlyBrandedProductRepository<PublicationResult>
    {
		Task<QueryResults<IssueResult>> GetIssues(CustomerId customerId, Guid publicationId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext<IssueResult> filter = null, SortContext<IssueResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		Task<QueryResults<JObject>> GetIssuesView(CustomerId customerId, Guid publicationId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    public interface IPublicationRepository : IReadOnlyPublicationRepository, IBrandedProductRepository<PublicationResult, PublicationForAddBase, PublicationForUpdate> { }
}
