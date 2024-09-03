using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.Repository;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Data
{
	public interface IReadOnlyPartyCategoryRepository : IReadOnlyRepository<PartyCategoryId, PartyCategoryResult>
	{
		Task<QueryResults<PartyCategoryResult>> GetByParty(Guid partyId, int? categoryTypeId = null, int? roleId = null, string viewName = null, Guid? userId = null,
			CancellationToken token = default(CancellationToken));

		Task<QueryResults<JObject>> GetByPartyView(Guid partyId, int? categoryTypeId = null, int? roleId = null, string viewName = null, Guid? userId = null,
			CancellationToken token = default(CancellationToken));
	}

	public interface IPartyCategoryRepository : IReadOnlyPartyCategoryRepository,
		IDeletableRepository<PartyCategoryId, PartyCategoryResult, PartyCategoryForAdd, PartyCategoryForUpdate> { }
}