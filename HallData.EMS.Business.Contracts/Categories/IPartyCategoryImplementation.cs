using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Business;
using HallData.EMS.ApplicationViews.Results;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Business
{
	[Service("partycategories")]
	public interface IPartyCategoryImplementation : IReadOnlyPartyCategoryImplementation,
		IDeletableBusinessImplementationWithBase<
		PartyCategoryId, PartyCategoryResult, PartyCategoryForAdd, PartyCategoryForUpdate>
	{
		Task<ChangeStatusQueryResult<PartyCategoryResult>> ChangeStatusPartyCategory(Guid partyId, int categoryId, int roleId,
			string statusTypeName, CancellationToken token = default(CancellationToken));

		Task<ChangeStatusQueryResult<PartyCategoryResult>> ChangeStatusForcePartyCategory(Guid partyId, int categoryId, int roleId,
			string statusTypeName, CancellationToken token = default(CancellationToken));

		Task<ChangeStatusQueryResult<JObject>> ChangeStatusPartyCategoryView(Guid partyId, int categoryId, int roleId,
			string statusTypeName, CancellationToken token = default(CancellationToken));

		Task<ChangeStatusQueryResult<JObject>> ChangeStatusForcePartyCategoryView(Guid partyId, int categoryId, int roleId,
			string statusTypeName, CancellationToken token = default(CancellationToken));

		Task DeletePartyCategorySoft(Guid partyId, int categoryId, int roleId,
			CancellationToken token = default(CancellationToken));

		Task DeletePartyCategoryHard(Guid partyId, int categoryId, int roleId,
			CancellationToken token = default(CancellationToken));

	}

	public interface IReadOnlyPartyCategoryImplementation : IReadOnlyBusinessImplementation<PartyCategoryId, PartyCategoryResult>
	{
		[GetMethod]
		[ServiceRoute("GetPartyCategory", "{partyId}/")]
		[ServiceRoute("GetPartyCategoryWithCategory", "{partyId}/{categoryTypeId}/")]
		[ServiceRoute("GetPartyCategoryWithCategoryRole", "{partyId}/{categoryTypeId}/{roleId}/")]
		[ServiceRoute("GetPartyCategoryTypedDefault", "{partyId}/TypedView/")]
		[ServiceRoute("GetPartyCategoryWithCategoryTypedDefault", "{partyId}/{categoryTypeId}/TypedView/")]
		[ServiceRoute("GetPartyCategoryWithCategoryRoleTypedDefault", "{partyId}/{categoryTypeId}/{roleId}/TypedView/")]
		[Description("Gets a strongly typed list of party categories by party id and optional category type id/optional role id")]
		Task<QueryResults<PartyCategoryResult>> GetByParty(Guid partyId, int? categoryTypeId = null, int? roleId = null,
			string viewName = null, CancellationToken token = default(CancellationToken));

		[GetMethod]
		[ServiceRoute("GetPartyCategoryView", "{partyId}/View/{viewName}/")]
		[ServiceRoute("GetPartyCategoryWithCategoryView", "{partyId}/{categoryTypeId}/View/{viewName}/")]
		[ServiceRoute("GetPartyCategoryWithCategoryRoleView", "{partyId}/{categoryTypeId}/{roleId}/View/{viewName}/")]
		[ServiceRoute("GetPartyCategoryViewDefault", "{partyId}/View/")]
		[ServiceRoute("GetPartyCategoryWithCategoryViewDefault", "{partyId}/{categoryTypeId}/View/")]
		[ServiceRoute("GetPartyCategoryWithCategoryRoleViewDefault", "{partyId}/{categoryTypeId}/{roleId}/View/")]
		[Description("Gets a dynamically typed list of party categories by party id and optional category type id/optional role id")]
		Task<QueryResults<JObject>> GetByPartyView(Guid partyId, int? categoryTypeId = null, int? roleId = null,
			string viewName = null, CancellationToken token = default(CancellationToken));
	}
}
