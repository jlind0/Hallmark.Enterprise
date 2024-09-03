using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HallData.Repository;
using HallData.EMS.ApplicationViews.Results;
using HallData.Business;
using HallData.EMS.Data;
using Newtonsoft.Json.Linq;
using HallData.Security;
using HallData.ApplicationViews;

namespace HallData.EMS.Business
{
    public class ReadOnlyPersonImplementation<TRepository, TKey, TPersonResult> : ReadOnlyPartyImplementation<TRepository, TKey, TPersonResult>, IReadOnlyPersonImplementation<TKey, TPersonResult>
        where TRepository: IReadOnlyPersonRepository<TKey, TPersonResult>
        where TPersonResult: IPersonResult<TKey>
    {
        protected IReadOnlyEmployeeRepository Employee { get; private set; }
        public ReadOnlyPersonImplementation(TRepository repository, ISecurityImplementation security, IReadOnlyEmployeeRepository employee) : base(repository, security)
        {
            this.Employee = employee;
        }

        public virtual Task<QueryResults<EmployeeResult>> GetEmployers(Guid personID, string viewName = null, FilterContext<EmployeeResult> filter = null, SortContext<EmployeeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            //TODO: this.Employee.GetEmployers
            throw new NotImplementedException();
        }

        public virtual Task<QueryResults<JObject>> GetEmployersView(Guid personID, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            //TODO: this.Employee.GetEmployersView
            throw new NotImplementedException();
        }
    }
    public class ReadOnlyPersonImplementation : ReadOnlyPersonImplementation<IReadOnlyPersonRepository, Guid, PersonResult>, IReadOnlyPersonImplementation
    {
        public ReadOnlyPersonImplementation(IReadOnlyPersonRepository repository, ISecurityImplementation security, IReadOnlyEmployeeRepository employee) : base(repository, security, employee) { }
    }
    public class PersonImplementation<TRepository, TReadOnlyImplementation, TKey, TPersonResult, TPersonForAdd, TPersonForUpdate> :
        PartyImplementation<TRepository, TReadOnlyImplementation, TKey, TPersonResult, TPersonForAdd, TPersonForUpdate>,
        IPersonImplementation<TKey, TPersonResult, TPersonForAdd, TPersonForUpdate>
        where TRepository : IPersonRepository<TKey, TPersonResult, TPersonForAdd, TPersonForUpdate>
        where TReadOnlyImplementation: IReadOnlyPersonImplementation<TKey, TPersonResult>
        where TPersonResult: IPersonResult<TKey>
        where TPersonForAdd: IPersonForAdd<TKey>
        where TPersonForUpdate: IPersonForUpdate<TKey>
    {
        public PersonImplementation(TRepository repository, ISecurityImplementation security, IPartyContactImplementation partyContact, TReadOnlyImplementation readOnly, IProductImplementation product)
             :base(repository, security, partyContact, readOnly, product)
        {
        }


        public virtual Task<QueryResults<EmployeeResult>> GetEmployers(Guid personID, string viewName = null, FilterContext<EmployeeResult> filter = null, SortContext<EmployeeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetEmployers(personID, viewName, filter, sort, page, token);
        }

        public virtual Task<QueryResults<JObject>> GetEmployersView(Guid personID, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetEmployersView(personID, viewName, filter, sort, page, token);
        }
    }
    public class PersonImplementation : PersonImplementation<IPersonRepository, IReadOnlyPersonImplementation, Guid, PersonResult, PersonForAdd, PersonForUpdate>, IPersonImplementation
    {
        public PersonImplementation(IPersonRepository repository, ISecurityImplementation security, IPartyContactImplementation partyContact, IReadOnlyPersonImplementation readOnly, IProductImplementation product)
            : base(repository, security, partyContact, readOnly, product)
        {
        }
    }
}
