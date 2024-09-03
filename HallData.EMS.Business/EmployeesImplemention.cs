using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.Business;
using HallData.Exceptions;
using HallData.EMS.Data;
using HallData.EMS.Models;
using HallData.Repository;
using System.Threading;
using HallData.Security;
using HallData.EMS.ApplicationViews;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Business
{
    public class ReadOnlyEmployeeImplementation : ReadOnlyPersonImplementation<IReadOnlyEmployeeRepository, EmployeeId, EmployeeResult>, IReadOnlyEmployeeImplementation
    {
        public ReadOnlyEmployeeImplementation(IReadOnlyEmployeeRepository repository, ISecurityImplementation security) : base(repository, security, repository) { }

        public virtual Task<QueryResult<EmployeeResult>> GetEmployee(Guid personID, Guid? employerID = null, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            return this.Get(new EmployeeId(employerID, personID), token);
        }

        public virtual Task<QueryResult<JObject>> GetEmployeeView(Guid personID, Guid? employerID = null, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            return this.GetView(new EmployeeId(employerID, personID), token);
        }

        public virtual Task<QueryResults<EmployeeResult>> GetByEmployer(Guid employerID, string viewName = null, FilterContext<EmployeeResult> filter = null, SortContext<EmployeeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            //TODO: this.Repository.GetByEmployer
            throw new NotImplementedException();
        }

        public virtual Task<QueryResults<JObject>> GetByEmployerView(Guid employerID, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            //TODO: this.Repository.GetByEmployerView
            throw new NotImplementedException();
        }
    }
    public class EmployeeImplementation : PersonImplementation<IEmployeeRepository, IReadOnlyEmployeeImplementation, EmployeeId, EmployeeResult, EmployeeForAdd, EmployeeForUpdate>, IEmployeeImplementation
    {
        public EmployeeImplementation(IEmployeeRepository repository, ISecurityImplementation security, IPartyContactImplementation partyContact, IReadOnlyEmployeeImplementation readOnly, IProductImplementation product)
            : base(repository, security, partyContact, readOnly, product) { }

        public virtual Task DeleteEmployeeSoft(Guid personID, Guid? employerID = null, CancellationToken token = default(CancellationToken))
        {
            return this.DeleteSoft(new EmployeeId(employerID, personID), token);
        }

        public virtual Task DeleteEmployeeHard(Guid personID, Guid? employerID = null, CancellationToken token = default(CancellationToken))
        {
            return this.DeleteHard(new EmployeeId(employerID, personID), token);
        }

        public virtual Task<ChangeStatusQueryResult<EmployeeResult>> ChangeStatusEmployee(Guid personID, string statusTypeName, Guid? employerID = null, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            return this.ChangeStatus(new EmployeeId(employerID, personID), statusTypeName, token);
        }

        public virtual Task<ChangeStatusQueryResult<EmployeeResult>> ChangeStatusEmployeeForce(Guid personID, string statusTypeName, Guid? employerID = null, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            return this.ChangeStatusForce(new EmployeeId(employerID, personID), statusTypeName, token);
        }

        public virtual Task<ChangeStatusQueryResult<EmployeeResult>> ChangeStatusEmployeeRelationship(Guid personID, string statusTypeName, Guid? employerID = null, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            //TODO: this.ChangeStatus<EmployeeResult>(this.Repository.ChangeStatusEmployeeRelationship force:false, this.GetEmployee)
            throw new NotImplementedException();
        }

        public virtual Task<ChangeStatusQueryResult<EmployeeResult>> ChangeStatusEmployeeRelationshipForce(Guid personID, string statusTypeName, Guid? employerID = null, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            //TODO: this.ChangeStatus<EmployeeResult>(this.Repository.ChangeStatusEmployeeRelationship force:true, this.GetEmployee)
            throw new NotImplementedException();
        }

        public virtual Task<QueryResult<EmployeeResult>> GetEmployee(Guid personID, Guid? employerID = null, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetEmployee(personID, employerID, viewName, token);
        }

        public virtual Task<QueryResult<JObject>> GetEmployeeView(Guid personID, Guid? employerID = null, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetEmployeeView(personID, employerID, viewName, token);
        }

        public virtual Task<QueryResults<EmployeeResult>> GetByEmployer(Guid employerID, string viewName = null, FilterContext<EmployeeResult> filter = null, SortContext<EmployeeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByEmployer(employerID, viewName, filter, sort, page, token);
        }

        public virtual Task<QueryResults<JObject>> GetByEmployerView(Guid employerID, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByEmployerView(employerID, viewName, filter, sort, page, token);
        }
    }
}
