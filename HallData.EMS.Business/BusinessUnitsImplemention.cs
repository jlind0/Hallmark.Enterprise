using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.Models;
using HallData.Repository;
using HallData.Business;
using HallData.EMS.Data;
using HallData.Security;
using System.Threading;
using System.Security.Authentication;
using HallData.Exceptions;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Business
{
    public class ReadOnlyBusinessUnitImplementation : ReadOnlyOrganizationImplementation<IReadOnlyBusinessUnitRepository, Guid, BusinessUnitResult>, IReadOnlyBusinessUnitImplementation
    {
        public ReadOnlyBusinessUnitImplementation(IReadOnlyBusinessUnitRepository repository, ISecurityImplementation security) :
            base(repository, security) { }

        public virtual Task<QueryResults<BusinessUnitResult>> GetByOrganization(Guid organizationID, string viewName = null, FilterContext<BusinessUnitResult> filter = null, SortContext<BusinessUnitResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            //TODO: this.Repository.GetByOrganization
            throw new NotImplementedException();
        }

        public virtual Task<QueryResults<JObject>> GetByOrganizationView(Guid organizationID, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            //TODO: this.Repository.GetByOrganizationView
            throw new NotImplementedException();
        }
    }
    public class BusinessUnitImplementation : OrganizationImplementation<IBusinessUnitRepository, IReadOnlyBusinessUnitImplementation, BusinessUnitResult, BusinessUnitForAdd, BusinessUnitForUpdate>, 
        IBusinessUnitImplementation
    {
        public BusinessUnitImplementation(IBusinessUnitRepository repository, ISecurityImplementation security, IPartyContactImplementation partyContact,
            IReadOnlyBusinessUnitImplementation readOnly, IReadOnlyProductImplementation product, IReadOnlyBusinessUnitImplementation businessUnit, IReadOnlyEmployeeImplementation employee) :
            base(repository, security, partyContact, readOnly, product, businessUnit, employee) { }

        public virtual Task<QueryResults<BusinessUnitResult>> GetByOrganization(Guid organizationID, string viewName = null, FilterContext<BusinessUnitResult> filter = null, SortContext<BusinessUnitResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByOrganization(organizationID, viewName, filter, sort, page, token);
        }

        public virtual Task<QueryResults<JObject>> GetByOrganizationView(Guid organizationID, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByOrganizationView(organizationID, viewName, filter, sort, page, token);
        }
    }
}
