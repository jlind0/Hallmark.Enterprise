using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.ApplicationViews;
using HallData.EMS.Data;
using HallData.Security;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews.Enums;
using HallData.EMS.ApplicationViews.Results;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Business
{
	public class ReadOnlyProductBaseImplementation<TRepository, TProductResult> : ReadOnlyBusinessRepositoryProxy<TRepository, Guid, TProductResult>,
		IReadOnlyProductBaseImplementation<TProductResult>
		where TProductResult : IProductBaseResultBase
		where TRepository : IReadOnlyProductBaseRepository<TProductResult>
	{

		public ReadOnlyProductBaseImplementation(TRepository repository, ISecurityImplementation security) : base(repository, security)
		{
			
		}

		public async Task<QueryResults<TProductResult>> GetByCustomer(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext<TProductResult> filter = null, SortContext<TProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var userId = await this.ActivateAndGetSignedInUserGuid(token);
			CustomerId customerId = new CustomerId(partyID, customerOfId);
			return await this.Repository.GetByCustomer(customerId, userId: userId, viewName: viewName, filter: filter, sort: sort, page: page, token: token);
		}

		public async Task<QueryResults<JObject>> GetByCustomerView(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var userId = await this.ActivateAndGetSignedInUserGuid(token);
			CustomerId customerId = new CustomerId(partyID, customerOfId);
			return await this.Repository.GetByCustomerView(customerId, userId: userId, viewName: viewName, filter: filter, sort: sort, page: page, token: token);
		}

		public async Task<QueryResults<TProductResult>> GetAllByCustomer(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext<TProductResult> filter = null, SortContext<TProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var userId = await this.ActivateAndGetSignedInUserGuid(token);
			CustomerId customerId = new CustomerId(partyID, customerOfId);
			return await this.Repository.GetAllByCustomer(customerId, userId: userId, viewName: viewName, filter: filter, sort: sort, page: page, token: token);
		}

		public async Task<QueryResults<JObject>> GetAllByCustomerView(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var userId = await this.ActivateAndGetSignedInUserGuid(token);
			CustomerId customerId = new CustomerId(partyID, customerOfId);
			return await this.Repository.GetAllByCustomerView(customerId, userId: userId, viewName: viewName, filter: filter, sort: sort, page: page, token: token);
		}

		public async Task<QueryResults<TProductResult>> GetByBusinessUnit(Guid businessUnitId, string viewName = null, FilterContext<TProductResult> filter = null, SortContext<TProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var userId = await this.ActivateAndGetSignedInUserGuid(token);
			return await this.Repository.GetByBusinessUnit(businessUnitId, viewName: viewName, userId: userId, filter: filter, sort: sort, page: page, token: token);
		}

		public async Task<QueryResults<JObject>> GetByBusinessUnitView(Guid businessUnitId, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var userId = await this.ActivateAndGetSignedInUserGuid(token);
			return await this.Repository.GetByBusinessUnitView(businessUnitId, viewName: viewName, userId: userId, filter: filter, sort: sort, page: page, token: token);
		}
	}

	public class ProductBaseImplementation<TRepository, TReadOnlyImplementation, TProductResult, TProductForAdd, TProductForUpdate> : 
		DeletableBusinessRepositoryProxyWithBase<TRepository, Guid, TProductResult, TProductForAdd, TProductForUpdate>, 
		IProductBaseImplementation<TProductResult, TProductForAdd, TProductForUpdate> 
		where TProductResult : IProductBaseResultBase
		where TReadOnlyImplementation : IReadOnlyProductBaseImplementation<TProductResult>
		where TProductForAdd : IProductBaseForAddBase
		where TProductForUpdate : IProductBaseForUpdateBase
		where TRepository : IProductBaseRepository<TProductResult, TProductForAdd, TProductForUpdate>
	{
		protected TReadOnlyImplementation ReadOnly { get; private set; }

		public ProductBaseImplementation(TRepository repository, ISecurityImplementation security,
			TReadOnlyImplementation readOnly)
			: base(repository, security)
		{
			this.ReadOnly = readOnly;
		}

		public Task<QueryResults<TProductResult>> GetByCustomer(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext<TProductResult> filter = null, SortContext<TProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.ReadOnly.GetByCustomer(partyID, customerOfId, viewName, filter, sort, page, token);
		}

		public Task<QueryResults<JObject>> GetByCustomerView(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.ReadOnly.GetByCustomerView(partyID, customerOfId, viewName, filter, sort, page, token);
		}

		public Task<QueryResults<TProductResult>> GetAllByCustomer(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext<TProductResult> filter = null, SortContext<TProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.ReadOnly.GetAllByCustomer(partyID, customerOfId, viewName, filter, sort, page, token);
		}

		public Task<QueryResults<JObject>> GetAllByCustomerView(Guid partyID, Guid? customerOfId = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.ReadOnly.GetAllByCustomerView(partyID, customerOfId, viewName, filter, sort, page, token);
		}

		public Task<QueryResults<TProductResult>> GetByBusinessUnit(Guid businessUnitId, string viewName = null, FilterContext<TProductResult> filter = null, SortContext<TProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.ReadOnly.GetByBusinessUnit(businessUnitId, viewName, filter, sort, page, token);
		}

		public Task<QueryResults<JObject>> GetByBusinessUnitView(Guid businessUnitId, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.ReadOnly.GetByBusinessUnitView(businessUnitId, viewName, filter, sort, page, token);
		}
	}
}
