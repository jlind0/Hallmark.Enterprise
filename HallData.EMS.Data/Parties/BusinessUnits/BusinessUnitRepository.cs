using System;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.Data;
using System.Threading;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Data
{
	public class BusinessUnitRepository : OrganizationRepository<BusinessUnitResult, BusinessUnitForAdd, BusinessUnitForUpdate>, IBusinessUnitRepository
	{
        public const string SelectAllBusinessUnitProcedure = "usp_select_businessunits";
        public const string SelectBusinessUnitQuery = "select * from v_businessunits where [partyguid#] = @partyguid and [__userguid?] = @__userguid";
        public const string InsertBusinessUnitProcedure = "usp_insert_businessunits";
        public const string UpdateBusinessUnitProcedure = "usp_update_businessunits";
        public const string DeleteBusinessUnitProcedure = "usp_delete_businessunits";

        public BusinessUnitRepository(Database db) : base(db, SelectAllBusinessUnitProcedure, SelectBusinessUnitQuery, InsertBusinessUnitProcedure, UpdateBusinessUnitProcedure, DeleteBusinessUnitProcedure) { }


		public Task<QueryResults<BusinessUnitResult>> GetByOrganization(Guid organizationID, Guid? userID = null, FilterContext<BusinessUnitResult> filter = null, SortContext<BusinessUnitResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			//TODO: usp_select_businessunits @businessunitofpartyguid = @organizationID
			throw new NotImplementedException();
		}

		public Task<QueryResults<JObject>> GetByOrganizationView(Guid organizationID, string viewName = null, Guid? userID = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			//TODO: usp_select_businessunits @businessunitofpartyguid = @organizationID
			throw new NotImplementedException();
		}
	}
}
