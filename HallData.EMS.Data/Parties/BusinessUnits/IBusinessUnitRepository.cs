using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.Repository;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Data
{
    public interface IReadOnlyBusinessUnitRepository : IReadOnlyOrganizationRepository<BusinessUnitResult>
    {
        Task<QueryResults<BusinessUnitResult>> GetByOrganization(Guid organizationID, Guid? userID = null, FilterContext<BusinessUnitResult> filter = null, SortContext<BusinessUnitResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<JObject>> GetByOrganizationView(Guid organizationID, string viewName = null, Guid? userID = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    public interface IBusinessUnitRepository : IOrganizationRepository<BusinessUnitResult, BusinessUnitForAdd, BusinessUnitForUpdate>, IReadOnlyBusinessUnitRepository
    {
        
    }
}