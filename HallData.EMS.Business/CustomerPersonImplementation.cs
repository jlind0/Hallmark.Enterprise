using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.Repository;
using HallData.EMS.Data;
using HallData.Business;
using Newtonsoft.Json.Linq;
using System.Threading;
using HallData.Security;
using HallData.EMS.ApplicationViews;
using HallData.ApplicationViews;

namespace HallData.EMS.Business
{
	public class ReadOnlyCustomerPersonImplementation : ReadOnlyPersonImplementation<IReadOnlyCustomerPersonRepository, CustomerId, CustomerPersonResult>, IReadOnlyCustomerPersonImplementation
	{
		public ReadOnlyCustomerPersonImplementation(IReadOnlyCustomerPersonRepository repository, ISecurityImplementation security, IReadOnlyEmployeeRepository employee) : base(repository, security, employee) { }

		public Task<QueryResult<CustomerPersonResult>> GetCustomer(Guid partyId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			return this.Get(new CustomerId(partyId, customerOfId), token);
		}

		public Task<QueryResult<JObject>> GetCustomerView(Guid partyId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			return this.GetView(new CustomerId(partyId, customerOfId), token);
		}
	}

	public class CustomerPersonImplementationBase : CustomerImplementation<IReadOnlyCustomerPersonImplementation, CustomerPersonResult>, ICustomerPersonImplementationBase
	{
		 public CustomerPersonImplementationBase(ICustomerRepository repository, ISecurityImplementation security, IPartyContactImplementation partyContact,
			IReadOnlyCustomerImplementation readOnly, IReadOnlyCustomerPersonImplementation baseImplemention, IReadOnlyProductImplementation product, IReadOnlyBrandImplementation brand)
			: base(repository, security, partyContact, readOnly, baseImplemention, product, brand) { }
	}

	public class CustomerPersonImplementation : PersonImplementation<ICustomerPersonRepository, IReadOnlyCustomerPersonImplementation, 
		CustomerId, CustomerPersonResult, CustomerPersonForAdd, CustomerPersonForUpdate>, ICustomerPersonImplementation
	{
		protected ICustomerPersonImplementationBase BaseImplementation { get; private set; }
        protected IReadOnlyBrandImplementation BrandImplementation { get; private set; }

		public CustomerPersonImplementation(ICustomerPersonRepository repository, ISecurityImplementation security, IPartyContactImplementation partyContact, IReadOnlyCustomerPersonImplementation readOnly,
            IProductImplementation product, ICustomerPersonImplementationBase baseImplementation, IReadOnlyBrandImplementation brandImplementation)
			 :base(repository, security, partyContact, readOnly, product)
		{
			this.BaseImplementation = baseImplementation;
            this.BrandImplementation = brandImplementation;

		}

		// customer

		public Task<QueryResult<CustomerPersonResult>> GetCustomer(Guid partyId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			return this.ReadOnly.GetCustomer(partyId, customerOfId, token);
		}

		public Task<QueryResult<JObject>> GetCustomerView(Guid partyId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			return this.ReadOnly.GetCustomerView(partyId, customerOfId, token);
		}

		// change status

		public Task<ChangeStatusQueryResult<CustomerPersonResult>> ChangeStatusTypeCustomerRelationship(Guid partyId, string statusTypeName, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			return this.BaseImplementation.ChangeStatusTypeCustomerRelationship(partyId, statusTypeName, customerOfId, token);
		}

		public Task<ChangeStatusQueryResult<CustomerPersonResult>> ChangeStatusTypeCustomerRelationshipForce(Guid partyId, string statusTypeName, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			return this.BaseImplementation.ChangeStatusTypeCustomerRelationshipForce(partyId, statusTypeName, customerOfId, token);
		}

		public Task<ChangeStatusQueryResult<CustomerPersonResult>> ChangeStatusCustomer(Guid partyId, string statusTypeName, Guid? customerOfId = null, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			return this.ChangeStatus(new CustomerId(partyId, customerOfId), statusTypeName, token);
		}

		public Task<ChangeStatusQueryResult<CustomerPersonResult>> ChangeStatusCustomerForce(Guid partyId, string statusTypeName, Guid? customerOfId = null, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			return this.ChangeStatusForce(new CustomerId(partyId, customerOfId), statusTypeName, token);
		}

		// delete

		public Task DeleteCustomerSoft(Guid partyId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			return this.DeleteSoft(new CustomerId(partyId, customerOfId), token);
		}

		public Task DeleteCustomerHard(Guid partyId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
		{
			return this.DeleteHard(new CustomerId(partyId, customerOfId), token);
		}

		// customers of customer

		public Task<QueryResults<CustomerResult>> GetCustomersOfCustomer(Guid partyId, string viewName = null, FilterContext<CustomerResult> filter = null, SortContext<CustomerResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.BaseImplementation.GetCustomersOfCustomer(partyId, viewName, filter, sort, page, token);
		}

		public Task<QueryResults<JObject>> GetCustomersOfCustomerView(Guid partyId, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.BaseImplementation.GetCustomersOfCustomerView(partyId, viewName, filter, sort, page, token);
		}

		// brands

		public Task<QueryResults<BrandResult>> GetBrands(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext<BrandResult> filter = null, SortContext<BrandResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
            return this.BrandImplementation.GetByCustomer(partyID, customerOfId, viewName, filter, sort, page, token);
        }

		public Task<QueryResults<JObject>> GetBrandsView(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
            return this.BrandImplementation.GetByCustomerView(partyID, customerOfId, viewName, filter, sort, page, token);
        }

		// brand

        //public Task<QueryResult<BrandResult>> GetBrand(Guid partyID, Guid brandId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<QueryResult<JObject>> GetBrandView(Guid partyID, Guid brandId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
        //{
        //    throw new NotImplementedException();
        //}

		// products

		public Task<QueryResults<ProductResult>> GetProducts(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext<ProductResult> filter = null, SortContext<ProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
            return this.Product.GetByCustomer(partyID, customerOfId, viewName, filter, sort, page, token);
        }

		public Task<QueryResults<JObject>> GetProductsView(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
            return this.Product.GetByCustomerView(partyID, customerOfId, viewName, filter, sort, page, token);
        }

		// prodcut

        //public Task<QueryResult<ProductResult>> GetProduct(Guid partyID, Guid productId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<QueryResult<JObject>> GetProductView(Guid partyID, Guid productId, Guid? customerOfId = null, CancellationToken token = default(CancellationToken))
        //{
        //    throw new NotImplementedException();
        //}

		// all products

		public Task<QueryResults<ProductResult>> GetAllProducts(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext<ProductResult> filter = null, SortContext<ProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
            return this.Product.GetAllByCustomer(partyID, customerOfId, viewName, filter, sort, page, token);
        }

		public Task<QueryResults<JObject>> GetAllProductsView(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
            return this.Product.GetAllByCustomerView(partyID, customerOfId, viewName, filter, sort, page, token);
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
}
