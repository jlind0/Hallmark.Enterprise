using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Repository;
using HallData.Security;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.Data;
using HallData.Business;
using System.Threading;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;
namespace HallData.EMS.Business
{
	public class ReadOnlyOrganizationImplementation<TRepository, TKey, TOrganizationResult> : ReadOnlyPartyImplementation<TRepository, TKey, TOrganizationResult>, IReadOnlyOrganizationImplementation<TKey, TOrganizationResult>
		where TRepository: IReadOnlyOrganizationRepository<TKey, TOrganizationResult>
		where TOrganizationResult: IOrganizationResult<TKey>
	{
		public ReadOnlyOrganizationImplementation(TRepository repository, ISecurityImplementation security) : 
			base(repository, security)
		{
		}
	}

	public class ReadOnlyOrganizationImplementation : ReadOnlyOrganizationImplementation<IReadOnlyOrganizationRepository, Guid, OrganizationResult>, IReadOnlyOrganizationImplementation
	{
		public ReadOnlyOrganizationImplementation(IReadOnlyOrganizationRepository repository, ISecurityImplementation security) :
			base(repository, security)
		{
		}
	}

	public class OrganizationImplementation<TRepository, TReadOnlyImplementation, TKey, TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate> :
		PartyImplementation<TRepository, TReadOnlyImplementation, TKey, TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate>,
		IOrganizationImplementation<TKey, TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate>
		where TOrganizationResult : IOrganizationResult<TKey>
		where TOrganizationForAdd: IOrganizationForAdd<TKey>
		where TOrganizationForUpdate: IOrganizationForUpdate<TKey>
		where TRepository: IOrganizationRepository<TKey, TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate>
		where TReadOnlyImplementation: IReadOnlyOrganizationImplementation<TKey, TOrganizationResult>
	{
		protected IReadOnlyBusinessUnitImplementation BusinessUnit { get; private set; }
		protected IReadOnlyEmployeeImplementation Employee { get; private set; }

		public OrganizationImplementation(TRepository repository, ISecurityImplementation security, IPartyContactImplementation partyContact, 
			TReadOnlyImplementation readOnly, IReadOnlyProductImplementation product, IReadOnlyBusinessUnitImplementation businessUnit, IReadOnlyEmployeeImplementation employee) : 
			base(repository, security, partyContact, readOnly, product)
		{
			this.BusinessUnit = businessUnit;
			this.Employee = employee;
		}

		public virtual Task<QueryResults<BusinessUnitResult>> GetBusinessUnits(Guid organizationID, string viewName = null, FilterContext<BusinessUnitResult> filter = null, SortContext<BusinessUnitResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.BusinessUnit.GetByOrganization(organizationID, viewName, filter, sort, page, token);
		}

		public virtual Task<QueryResults<JObject>> GetBusinessUnitsView(Guid organizationID, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.BusinessUnit.GetByOrganizationView(organizationID, viewName, filter, sort, page, token);
		}

		public virtual Task<QueryResults<EmployeeResult>> GetEmployees(Guid organizationID, string viewName = null, FilterContext<EmployeeResult> filter = null, SortContext<EmployeeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.Employee.GetByEmployer(organizationID, viewName, filter, sort, page, token);
		}

		public virtual Task<QueryResults<JObject>> GetEmployeesView(Guid organizationID, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.Employee.GetByEmployerView(organizationID, viewName, filter, sort, page, token);
		}

		public virtual Task<QueryResult<EmployeeResult>> GetEmployee(Guid organizationID, Guid employeeID, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			return this.Employee.GetEmployee(employeeID, organizationID, viewName, token);
		}

		public virtual Task<QueryResult<JObject>> GetEmployeeView(Guid organizationID, Guid employeeID, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			return this.Employee.GetEmployeeView(employeeID, organizationID, viewName, token);
		}
	}

	public class OrganizationImplementation<TRepository, TReadOnlyImplementation, TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate> :
		OrganizationImplementation<TRepository, TReadOnlyImplementation, Guid, TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate>
		where TOrganizationResult: IOrganizationResult
		where TOrganizationForAdd: IOrganizationForAdd
		where TOrganizationForUpdate: IOrganizationForUpdate
		where TRepository: IOrganizationRepository<TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate>
		where TReadOnlyImplementation: IReadOnlyOrganizationImplementation<Guid, TOrganizationResult>
	{
		public OrganizationImplementation(TRepository repository, ISecurityImplementation security, IPartyContactImplementation partyContact, 
			TReadOnlyImplementation readOnly, IReadOnlyProductImplementation product, IReadOnlyBusinessUnitImplementation businessUnit, IReadOnlyEmployeeImplementation employee) :
			base(repository, security, partyContact, readOnly, product, businessUnit, employee)
		{
		}
	}

	public class OrganizationImplementation : OrganizationImplementation<IOrganizationRepository, IReadOnlyOrganizationImplementation, OrganizationResult, OrganizationForAdd, OrganizationForUpdate>, IOrganizationImplementation
	{
		protected ICustomerImplementation Customer { get; private set; }
		public OrganizationImplementation(IOrganizationRepository repository, ISecurityImplementation security, IPartyContactImplementation partyContact,
			IReadOnlyOrganizationImplementation readOnly, IReadOnlyProductImplementation product, IReadOnlyBusinessUnitImplementation businessUnit, IReadOnlyEmployeeImplementation employee, ICustomerImplementation customer) :
			base(repository, security, partyContact, readOnly, product, businessUnit, employee)
		{
			this.Customer = customer;
		}

		public Task<QueryResults<CustomerResult>> GetCustomers(Guid organizationId, string viewName = null, FilterContext<CustomerResult> filter = null, SortContext<CustomerResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.Customer.GetCustomersOfCustomer(organizationId, viewName, filter, sort, page, token);
		}

		public Task<QueryResults<JObject>> GetCustomersView(Guid organizationId, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.Customer.GetCustomersOfCustomerView(organizationId, viewName, filter, sort, page, token);
		}
	}
}
