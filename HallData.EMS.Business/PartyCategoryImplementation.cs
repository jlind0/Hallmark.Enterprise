using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Business;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.Data;
using HallData.Security;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Business
{

	public class ReadOnlyPartyCategoryImplementation : ReadOnlyBusinessRepositoryProxy<
		IReadOnlyPartyCategoryRepository, PartyCategoryId, PartyCategoryResult>,
		IReadOnlyPartyCategoryImplementation
	{
		public ReadOnlyPartyCategoryImplementation(IReadOnlyPartyCategoryRepository repository, 
			ISecurityImplementation security) :
		base(repository, security)
		{

		}

		// get single

		public Task<QueryResult<PartyCategoryResult>> GetCustomer(Guid partyId, int categoryId, int roleId, CancellationToken token = default(CancellationToken))
		{
			return this.Get(new PartyCategoryId(partyId, categoryId, roleId), token);
		}

		public Task<QueryResult<JObject>> GetCustomerView(Guid partyId, int categoryId, int roleId, CancellationToken token = default(CancellationToken))
		{
			return this.GetView(new PartyCategoryId(partyId, categoryId, roleId), token);
		}

		// get many

		public async Task<QueryResults<PartyCategoryResult>> GetByParty(Guid partyId, int? categoryTypeId = null, int? roleId = null, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			var userId = await ActivateAndGetSignedInUserGuid(token);
			return await this.Repository.GetByParty(partyId, categoryTypeId, roleId, viewName, userId, token);
		}

		public async Task<QueryResults<JObject>> GetByPartyView(Guid partyId, int? categoryTypeId = null, int? roleId = null, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			var userId = await ActivateAndGetSignedInUserGuid(token);
			return await this.Repository.GetByPartyView(partyId, categoryTypeId, roleId, viewName, userId, token);
		}

	}

	public class PartyCategoryImplementation :
		DeletableBusinessRepositoryProxyWithBase<IPartyCategoryRepository, 
		PartyCategoryId, PartyCategoryResult, PartyCategoryForAdd, PartyCategoryForUpdate>,
		IPartyCategoryImplementation
	{

		protected IReadOnlyPartyCategoryImplementation ReadOnly { get; private set; }

		public PartyCategoryImplementation(IPartyCategoryRepository repository,
			ISecurityImplementation security, IReadOnlyPartyCategoryImplementation readOnly) :
			base(repository, security)
		{
			this.ReadOnly = readOnly;
		}

		public Task<ChangeStatusQueryResult<PartyCategoryResult>> ChangeStatusPartyCategory(Guid partyId, int categoryId, int roleId, string statusTypeName, CancellationToken token = default(CancellationToken))
		{
			PartyCategoryId partyCategoryId = new PartyCategoryId(partyId, categoryId, roleId);
			return this.ChangeStatus(partyCategoryId, statusTypeName, token);
		}

		public Task<ChangeStatusQueryResult<PartyCategoryResult>> ChangeStatusForcePartyCategory(Guid partyId, int categoryId, int roleId, string statusTypeName, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusPartyCategoryView(Guid partyId, int categoryId, int roleId, string statusTypeName, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusForcePartyCategoryView(Guid partyId, int categoryId, int roleId, string statusTypeName, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task DeletePartyCategorySoft(Guid partyId, int categoryId, int roleId, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task DeletePartyCategoryHard(Guid partyId, int categoryId, int roleId, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<PartyCategoryResult>> GetByParty(Guid partyId, int? categoryTypeId = null, int? roleId = null, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			return this.ReadOnly.GetByParty(partyId, categoryTypeId, roleId, viewName, token);
		}

		public Task<QueryResults<JObject>> GetByPartyView(Guid partyId, int? categoryTypeId = null, int? roleId = null, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			return this.ReadOnly.GetByPartyView(partyId, categoryTypeId, roleId, viewName, token);
		}
	}
}
