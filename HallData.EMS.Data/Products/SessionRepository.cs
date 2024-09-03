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
	public class SessionRepository : ProductRepository<SessionResult, SessionForAddBase, SessionForUpdate>, ISessionRepository
	{
		protected const string SelectSessionQuery = "select * from v_products where [productguid#] = @brandguid and [__userguid?] = @__userguid";

		public SessionRepository(Database db) : base(db) { }

		protected SessionRepository(Database db, string selectAllProcedure = SelectAllProductsProcedure, string selectProcedure = SelectSessionQuery, string insertProcedure = InsertProductProcedure,
			string updateProcedure = UpdateProductProcedure, string deleteProcedure = DeleteProductProcedure, string changeStatusProcedure = ChangeStatusProductProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }

		protected override ProductTypes? ProductType
		{
			get
			{
				return ProductTypes.Session;
			}
		}

		public Task<QueryResult<SessionResult>> Get(CustomerId customerId, Guid eventId, Guid trackId, Guid sessionId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			cmd.AddParameter("eventId", eventId);
			cmd.AddParameter("trackId", trackId);
			cmd.AddParameter("sessionId", sessionId);
			PopulateBrandId(cmd, brandId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadQueryResult<SessionResult>(cmd, userId, token);
		}

		public Task<QueryResult<JObject>> GetView(CustomerId customerId, Guid eventId, Guid trackId, Guid sessionId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			cmd.AddParameter("eventId", eventId);
			cmd.AddParameter("trackId", trackId);
			cmd.AddParameter("sessionId", sessionId);
			PopulateBrandId(cmd, brandId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadView(cmd, userId, token);
		}
	}
}
