using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.ApplicationViews;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Business
{
    public interface IReadOnlyBusinessUnitImplementation : IReadOnlyOrganizationImplementation<Guid, BusinessUnitResult>
    {
        [GetMethod]
        [ServiceRoute("GetByOrganization", "Organization/{organizationID}/")]
        [ServiceRoute("GetByOrganizationTyped", "Organization/{organizationID}/TypedView/{viewName}/")]
        [ServiceRoute("GetByOrganizationTypedDefault", "Organization/{organizationID}/TypedView/")]
        [Description("Get business units by organization")]
        Task<QueryResults<BusinessUnitResult>> GetByOrganization([Description("Target organization id")]Guid organizationID, string viewName = null,
            [Description("Filter for business units, must be url encoded JSON")][JsonEncode]FilterContext<BusinessUnitResult> filter = null,
            [Description("Sort for business units, must be url encoded JSON")][JsonEncode]SortContext<BusinessUnitResult> sort = null,
            [Description("Page for business units, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetByOrganizationView", "Organization/{organizationID}/View/{viewName}/")]
        [ServiceRoute("GetByOrganizationViewDefault", "Organization/{organizationID}/View/")]
        [Description("Get business units by organization")]
        Task<QueryResults<JObject>> GetByOrganizationView([Description("Target organization id")]Guid organizationID, string viewName = null,
            [Description("Filter for business units, must be url encoded JSON")][JsonEncode]FilterContext filter = null,
            [Description("Sort for business units, must be url encoded JSON")][JsonEncode]SortContext sort = null,
            [Description("Page for business units, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    [Service("businessunits")]
    public interface IBusinessUnitImplementation : IOrganizationImplementation<Guid, BusinessUnitResult, BusinessUnitForAdd, BusinessUnitForUpdate>, IReadOnlyBusinessUnitImplementation
    {

    }
}