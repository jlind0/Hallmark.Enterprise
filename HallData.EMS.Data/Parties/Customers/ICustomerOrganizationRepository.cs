using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;

namespace HallData.EMS.Data
{
    public interface IReadOnlyCustomerOrganizationRepository : IReadOnlyOrganizationRepository<CustomerId, CustomerOrganizationResult> { }
    public interface ICustomerOrganizationRepository : IOrganizationRepository<CustomerId, CustomerOrganizationResult, CustomerOrganizationForAdd, CustomerOrganizationForUpdate>, 
        IReadOnlyCustomerOrganizationRepository { }
}
