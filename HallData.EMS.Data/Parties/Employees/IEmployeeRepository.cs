using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.Repository;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Data
{
    public interface IReadOnlyEmployeeRepository : IReadOnlyPersonRepository<EmployeeId, EmployeeResult>
    {
        Task<QueryResults<EmployeeResult>> GetByEmployer(Guid employerID, Guid? userID = null, FilterContext<EmployeeResult> filter = null, SortContext<EmployeeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<JObject>> GetByEmployerView(Guid employerID, string viewName = null, Guid? userID = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<EmployeeResult>> GetEmployers(Guid partyID, Guid? userID = null, FilterContext<EmployeeResult> filter = null, SortContext<EmployeeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<JObject>> GetEmployersView(Guid partyID, Guid? userID = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    public interface IEmployeeRepository : IPersonRepository<EmployeeId, EmployeeResult, EmployeeForAdd, EmployeeForUpdate>, IReadOnlyEmployeeRepository
    {
        Task<ChangeStatusResult> ChangeStatusEmployeeRelationship(EmployeeId id, string statusTypeName, Guid? userID = null, CancellationToken token = default(CancellationToken));
    }
}