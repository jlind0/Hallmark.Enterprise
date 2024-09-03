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
	public class PublicationRepository : BrandedProductRepository<PublicationResult, PublicationForAddBase, PublicationForUpdate>, IPublicationRepository
	{
		protected const string SelectPublicationQuery = "select * from v_products where [productguid#] = @brandguid and [__userguid?] = @__userguid";

		public PublicationRepository(Database db) : base(db) { }

		protected PublicationRepository(Database db, string selectAllProcedure = SelectAllProductsProcedure, string selectProcedure = SelectPublicationQuery, string insertProcedure = InsertProductProcedure,
			string updateProcedure = UpdateProductProcedure, string deleteProcedure = DeleteProductProcedure, string changeStatusProcedure = ChangeStatusProductProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }

		protected override ProductTypes? ProductType
		{
			get
			{
				return ProductTypes.Publication;
			}
		}

		public Task<QueryResults<IssueResult>> GetIssues(CustomerId customerId, Guid publicationId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext<IssueResult> filter = null, SortContext<IssueResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			cmd.AddParameter("publicationId", publicationId);
			PopulateBrandId(cmd, brandId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadQueryResults<IssueResult>(cmd, viewName, userId, filter, sort, page, token:token);
		}

		public Task<QueryResults<JObject>> GetIssuesView(CustomerId customerId, Guid publicationId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			cmd.AddParameter("publicationId", publicationId);
			PopulateBrandId(cmd, brandId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadViews(cmd, viewName, userId, filter, sort, page, token: token);
		}
	}
}
