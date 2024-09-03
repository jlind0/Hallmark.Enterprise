using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using System.Threading;
using HallData.Repository;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Data
{
    public interface ICustomerRepository : IPartyRepository<CustomerId, CustomerResult, CustomerForAdd, CustomerForUpdate>, IReadOnlyCustomerRepository
    {
        Task<ChangeStatusResult> ChangeStatusCustomerOfRelationship(CustomerId id, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }
    public interface IReadOnlyCustomerRepository : IReadOnlyPartyRepository<CustomerId, CustomerResult> 
    {
        Task<QueryResults<CustomerResult>> GetCustomersOfCustomer(Guid partyId, string viewName = null, Guid? userId = null, FilterContext<CustomerResult> filter = null, SortContext<CustomerResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<JObject>> GetCustomersOfCustomerView(Guid partyId, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
}
