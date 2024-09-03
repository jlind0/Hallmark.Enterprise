using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Data;
using HallData.Repository;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.ApplicationViews.Enums;
using System.Data.SqlClient;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Data
{
	public class IssueRepository : ProductRepository<IssueResult, IssueForAddBase, IssueForUpdate>, IIssueRepository
	{
		protected const string SelectIssueQuery = "select * from v_products where [productguid#] = @productguid and [__userguid?] = @__userguid";

		public IssueRepository(Database db) : base(db) { }

		protected IssueRepository(Database db, string selectAllProcedure = SelectAllProductsProcedure, string selectProcedure = SelectIssueQuery, string insertProcedure = InsertProductProcedure,
			string updateProcedure = UpdateProductProcedure, string deleteProcedure = DeleteProductProcedure, string changeStatusProcedure = ChangeStatusProductProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }

		protected override ProductTypes? ProductType
		{
			get
			{
				return ProductTypes.Issue;
			}
		}

		public Task<QueryResult<IssueResult>> Get(CustomerId customerId, Guid publicationId, Guid issueId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			cmd.AddParameter("publicationId", publicationId);
			cmd.AddParameter("issueId", issueId);
			PopulateBrandId(cmd, brandId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadQueryResult<IssueResult>(cmd, userId, token);
		}

		public Task<QueryResult<JObject>> GetView(CustomerId customerId, Guid publicationId, Guid issueId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			cmd.AddParameter("publicationId", publicationId);
			cmd.AddParameter("issueId", issueId);
			PopulateBrandId(cmd, brandId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadView(cmd, userId, token);
		}
	}
}
