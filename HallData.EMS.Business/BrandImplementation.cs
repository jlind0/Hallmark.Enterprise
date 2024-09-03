using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.Data;
using HallData.Security;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Business
{
	public class ReadOnlyBrandImplementation : ReadOnlyProductBaseImplementation<IReadOnlyBrandRepository, BrandResult>, IReadOnlyBrandImplementation
	{

		public ReadOnlyBrandImplementation(IReadOnlyBrandRepository repository, ISecurityImplementation security,
			IContactMechanismRepository contactMechanismRepository)
			: base(repository, security) { }

		public async Task<QueryResults<BrandResult>> GetBrands(Guid partyID, Guid? customerOfId = null, Guid? businessUnitId = null, string viewName = null, FilterContext<BrandResult> filter = null, SortContext<BrandResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var userId = await ActivateAndGetSignedInUserGuid(token);
			CustomerId customerId = new CustomerId(partyID, customerOfId);
			return await this.Repository.GetBrands(customerId, businessUnitId: businessUnitId, viewName: viewName, filter: filter, sort: sort, page: page, token: token);
		}

		public async Task<QueryResults<JObject>> GetBrandsView(Guid partyID, Guid? customerOfId = null, Guid? businessUnitId = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var userId = await ActivateAndGetSignedInUserGuid(token);
			CustomerId customerId = new CustomerId(partyID, customerOfId);
			return await this.Repository.GetBrandsView(customerId, businessUnitId: businessUnitId, viewName: viewName, filter: filter, sort: sort, page: page, token: token);
		}

		public async Task<QueryResult<BrandResult>> GetBrand(Guid partyID, Guid brandId, Guid? customerOfId = null, Guid? businessUnitId = null, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			var userId = await ActivateAndGetSignedInUserGuid(token);
			CustomerId customerId = new CustomerId(partyID, customerOfId);
			return await this.Repository.GetBrand(customerId, brandId, businessUnitId: businessUnitId, viewName: viewName, token: token);
		}

		public async Task<QueryResult<JObject>> GetBrandView(Guid partyID, Guid brandId, Guid? customerOfId = null, Guid? businessUnitId = null, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			var userId = await ActivateAndGetSignedInUserGuid(token);
			CustomerId customerId = new CustomerId(partyID, customerOfId);
			return await this.Repository.GetBrandView(customerId, brandId, businessUnitId: businessUnitId, viewName: viewName, token: token);
		}
	}

	public class BrandImplementation :
		ProductBaseImplementation<IBrandRepository, IReadOnlyBrandImplementation, BrandResult, BrandForAdd, BrandForUpdate>,
		IBrandImplementation
	{
		protected IBrandRepository Repository { get; private set; }

		public BrandImplementation(IBrandRepository repository, ISecurityImplementation security,
			IReadOnlyBrandImplementation readOnly) :
				base(repository, security, readOnly)
		{
			this.Repository = repository;
		}

		public Task<QueryResults<BrandResult>> GetBrands(Guid partyID, Guid? customerOfId = null, Guid? businessUnitId = null, string viewName = null, FilterContext<BrandResult> filter = null, SortContext<BrandResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.ReadOnly.GetByCustomer(partyID, customerOfId, viewName, filter, sort, page, token);
		}

		public Task<QueryResults<JObject>> GetBrandsView(Guid partyID, Guid? customerOfId = null, Guid? businessUnitId = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return this.ReadOnly.GetByCustomerView(partyID, customerOfId, viewName, filter, sort, page, token);
		}
	}
}
