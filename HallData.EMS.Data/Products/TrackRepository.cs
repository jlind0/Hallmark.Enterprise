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
    public class TrackRepository : ProductRepository<TrackResult, TrackForAddBase, TrackForUpdate>, ITrackRepository
    {

		protected const string SelectTrackQuery = "select * from v_products where [productguid#] = @brandguid and [__userguid?] = @__userguid";

	    public TrackRepository(Database db) : base(db) { }

		protected TrackRepository(Database db, string selectAllProcedure = SelectAllProductsProcedure, string selectProcedure = SelectTrackQuery, string insertProcedure = InsertProductProcedure,
			string updateProcedure = UpdateProductProcedure, string deleteProcedure = DeleteProductProcedure, string changeStatusProcedure = ChangeStatusProductProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }

		public Task<QueryResults<SessionResult>> GetSessions(CustomerId customerId, Guid eventId, Guid trackId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext<SessionResult> filter = null, SortContext<SessionResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			cmd.AddParameter("eventId", eventId);
			cmd.AddParameter("trackId", trackId);
			PopulateBrandId(cmd, brandId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadQueryResults<SessionResult>(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public Task<QueryResults<JObject>> GetSessionsView(CustomerId customerId, Guid eventId, Guid trackId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			cmd.AddParameter("eventId", eventId);
			cmd.AddParameter("trackId", trackId);
			PopulateBrandId(cmd, brandId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadViews(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public Task<QueryResult<TrackResult>> Get(CustomerId customerId, Guid eventId, Guid trackId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			cmd.AddParameter("eventId", eventId);
			cmd.AddParameter("trackId", trackId);
			PopulateBrandId(cmd, brandId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadQueryResult<TrackResult>(cmd, userId, token);
		}

		public Task<QueryResult<JObject>> GetView(CustomerId customerId, Guid eventId, Guid trackId, Guid? brandId = null, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerId(cmd, customerId);
			cmd.AddParameter("eventId", eventId);
			cmd.AddParameter("trackId", trackId);
			PopulateBrandId(cmd, brandId);
			PopulateBusinessUnitId(cmd, businessUnitId);
			return ReadView(cmd, userId, token);
		}
	}
}
