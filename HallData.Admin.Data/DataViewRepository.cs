using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.ApplicationViews;
using HallData.Data;
using HallData.Repository;
using Newtonsoft.Json.Linq;

namespace HallData.Admin.Data
{
	public class DataViewRepository : DeletableRepository<int, DataViewResult, DataViewForAdd, DataViewForUpdate>,
		IDataViewRepository
	{
		public DataViewRepository(Database db)
			: base(db, "ui.usp_select_dataviews", "ui.usp_select_dataviews", "ui.usp_insert_dataviews", "ui.usp_update_dataviews", "ui.usp_delete_dataviews", null)
		{
		}

		public override Task<QueryResult<DataViewResult>> Get(int id, Guid? userId = null, CancellationToken token = new CancellationToken())
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
			this.PopulateGetCommand(id, cmd);
			return ReadQueryResult<DataViewResult>(cmd, userId, token);   
		}

		public override Task<QueryResult<JObject>> GetView(int id, Guid? userId = null, CancellationToken token = new CancellationToken())
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
			this.PopulateGetCommand(id, cmd);
			return ReadView(cmd, userId, token);
		}

		public Task RelateDataViews(int dataViewId, int relatedDataViewId, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand("ui.usp_relate_dataviews");
			cmd.AddParameter("dataviewid", dataViewId);
			cmd.AddParameter("relateddataviewid", relatedDataViewId);
			PopulateUserIdParameter(cmd, userId);
			return Execute(cmd, () => this.Database.ExecuteNonQueryAsync(cmd, token));
		}

		public Task UnRelateDataViews(int dataViewId, int relatedDataViewId, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand("ui.usp_unrelate_dataviews");
			cmd.AddParameter("dataviewid", dataViewId);
			cmd.AddParameter("relateddataviewid", relatedDataViewId);
			PopulateUserIdParameter(cmd, userId);
			return Execute(cmd, () => this.Database.ExecuteNonQueryAsync(cmd, token));
		}

		public Task<QueryResult<DataViewResult>> GetByName(string name, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
			cmd.AddParameter("name", name);
			return ReadQueryResult<DataViewResult>(cmd, userId, token);
		}

		public async Task<bool> AreDataViewsCommon(int dataViewId1, int dataViewId2, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand("ui.usp_arecommon_dataviews");
			cmd.AddParameter("dataviewid1", dataViewId1);
			cmd.AddParameter("dataviewid2", dataViewId2);
			PopulateUserIdParameter(cmd, userId);
			return (bool)await Execute(cmd, () => this.Database.ExecuteScalarAsync(cmd, token));
		}

		public override Task<ChangeStatusResult> ChangeStatus(int id, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		protected override void PopulateDeleteCommand(int id, DbCommand cmd)
		{
			cmd.AddParameter("dataviewid", id);
		}

		protected override int ReadKeyFromScalarReturnObject(object obj, DataViewForAdd view)
		{
			return (int)obj;
		}

		protected override void PopulateChangeStatusCommand(DbCommand cmd, int id)
		{
			throw new NotImplementedException();
		}

		protected override void PopulateGetCommand(int id, DbCommand cmd)
		{
			cmd.AddParameter("dataviewid", id);
		}
	}
}