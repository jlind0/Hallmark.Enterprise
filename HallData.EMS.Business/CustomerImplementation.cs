using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.Data;
using HallData.EMS.ApplicationViews.Results;
using HallData.Repository;
using System.Threading;
using HallData.Security;
using HallData.Business;
using Newtonsoft.Json.Linq;
using HallData.EMS.ApplicationViews;
using HallData.ApplicationViews;

namespace HallData.EMS.Business
{
	public class ReadOnlyCustomerImplementation : ReadOnlyPartyImplementation<IReadOnlyCustomerRepository, CustomerId, CustomerResult>, IReadOnlyCustomerImplementation
	{
		public ReadOnlyCustomerImplementation(IReadOnlyCustomerRepository repository, ISecurityImplementation security) :
				base(repository, security)
		{ }

		//TODO: should this be defined in ICustomerImplementation?
		public Task<QueryResult<CustomerResult>> GetCustomer(Guid partyId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			return this.Get(new CustomerId(partyId, customerOfId), token);
		}

		public Task<QueryResult<JObject>> GetCustomerView(Guid partyId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			return this.GetView(new CustomerId(partyId, customerOfId), token);
		}
	}

	public class CustomerImplementation<TReadOnlyImplementationBase, TCustomerImplementationResult> : 
		PartyImplementation<ICustomerRepository, IReadOnlyCustomerImplementation, CustomerId, CustomerResult, CustomerForAdd, CustomerForUpdate>, 
		ICustomerImplementation<CustomerResult, CustomerForAdd, CustomerForUpdate, TCustomerImplementationResult>
		where TCustomerImplementationResult : ICustomerResult
		where TReadOnlyImplementationBase: IReadOnlyCustomerImplementation<TCustomerImplementationResult>
	{
		protected TReadOnlyImplementationBase ReadOnlyBaseImplementation { get; private set; }
		protected IReadOnlyBrandImplementation BrandImplementation { get; private set; }

		public CustomerImplementation(ICustomerRepository customerRepository, ISecurityImplementation security, IPartyContactImplementation partyContact,
			IReadOnlyCustomerImplementation readOnly, TReadOnlyImplementationBase baseImplemention, IReadOnlyProductImplementation product, 
			IReadOnlyBrandImplementation brandImplementation) :
			base(customerRepository, security, partyContact, readOnly, product)
		{
			this.ReadOnlyBaseImplementation = baseImplemention;
			this.BrandImplementation = brandImplementation;
		}

		public virtual Task<ChangeStatusQueryResult<TCustomerImplementationResult>> ChangeStatusTypeCustomerRelationship(Guid partyId, string statusTypeName, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			var customerId = new CustomerId(partyId, customerOfId);
			return this.ChangeStatus(u => this.Repository.ChangeStatusCustomerOfRelationship(customerId, statusTypeName, false, u, token),
				() => this.ReadOnlyBaseImplementation.Get(customerId, token), token);
		}

		public virtual Task<ChangeStatusQueryResult<TCustomerImplementationResult>> ChangeStatusTypeCustomerRelationshipForce(Guid partyId, string statusTypeName, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			var customerId = new CustomerId(partyId, customerOfId);
			return this.ChangeStatus(u => this.Repository.ChangeStatusCustomerOfRelationship(customerId, statusTypeName, true, u, token),
				() => this.ReadOnlyBaseImplementation.Get(customerId, token), token);
		}

		public virtual Task<QueryResult<CustomerResult>> GetCustomer(Guid partyId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			return this.ReadOnly.GetCustomer(partyId, customerOfId, token);
		}

		public virtual Task<QueryResult<JObject>> GetCustomerView(Guid partyId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			return this.ReadOnly.GetCustomerView(partyId, customerOfId, token);
		}

		public virtual Task<ChangeStatusQueryResult<CustomerResult>> ChangeStatusCustomer(Guid partyId, string statusTypeName, Guid? customerOfId = null, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			return this.ChangeStatus(new CustomerId(partyId, customerOfId), statusTypeName, token);
		}

		public virtual Task<ChangeStatusQueryResult<CustomerResult>> ChangeStatusCustomerForce(Guid partyId, string statusTypeName, Guid? customerOfId = null, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			return this.ChangeStatusForce(new CustomerId(partyId, customerOfId), statusTypeName, token);
		}

		public virtual Task DeleteCustomerSoft(Guid partyId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			return this.DeleteSoft(new CustomerId(partyId, customerOfId), token);
		}

		public virtual Task DeleteCustomerHard(Guid partyId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			return this.DeleteHard(new CustomerId(partyId, customerOfId), token);
		}

		// customers

		public async Task<QueryResults<CustomerResult>> GetCustomersOfCustomer(Guid partyId, string viewName = null, FilterContext<CustomerResult> filter = null, SortContext<CustomerResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var userId = await ActivateAndGetSignedInUserGuid(token);
			CustomerId customerId = new CustomerId(partyId, null);
			return await this.Repository.GetCustomersOfCustomer(partyId, viewName, userId, filter, sort, page, token);
		}

		public async Task<QueryResults<JObject>> GetCustomersOfCustomerView(Guid partyId, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var userId = await ActivateAndGetSignedInUserGuid(token);
			CustomerId customerId = new CustomerId(partyId, null);
			return await this.Repository.GetCustomersOfCustomerView(partyId, viewName, userId, filter, sort, page, token);
		}

		// products

		public Task<QueryResults<ProductResult>> GetProducts(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext<ProductResult> filter = null, SortContext<ProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.Product.GetByCustomer(partyID, customerOfId, viewName, filter, sort, page, token);
		}

		public Task<QueryResults<JObject>> GetProductsView(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.Product.GetByCustomerView(partyID, customerOfId, viewName, filter, sort, page, token);
		}

		// all products

		public Task<QueryResults<ProductResult>> GetAllProducts(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext<ProductResult> filter = null, SortContext<ProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.Product.GetAllByCustomer(partyID, customerOfId, viewName, filter, sort, page, token);
		}

		public Task<QueryResults<JObject>> GetAllProductsView(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.Product.GetAllByCustomerView(partyID, customerOfId, viewName, filter, sort, page, token);
		}

		

		public Task<QueryResults<BrandResult>> GetBrands(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext<BrandResult> filter = null, SortContext<BrandResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.BrandImplementation.GetByCustomer(partyID, customerOfId, viewName, filter, sort, page, token);
		}

		public Task<QueryResults<JObject>> GetBrandsView(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.BrandImplementation.GetByCustomerView(partyID, customerOfId, viewName, filter, sort, page, token);
		}

		// business units

		public Task<QueryResults<ProductResult>> GetBusinessUnits(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext<ProductResult> filter = null, SortContext<ProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<JObject>> GetBusinessUnitsView(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}
	}

	public class CustomerImplementation : CustomerImplementation<IReadOnlyCustomerImplementation, CustomerResult>, ICustomerImplementation
	{
		public CustomerImplementation(ICustomerRepository repository, ISecurityImplementation security, IPartyContactImplementation partyContact, 
			IReadOnlyCustomerImplementation readOnly, IReadOnlyCustomerImplementation baseImplemention, IReadOnlyProductImplementation product, IReadOnlyBrandImplementation brand) :
			base(repository, security, partyContact, readOnly, baseImplemention, product, brand) { }
	}
}
