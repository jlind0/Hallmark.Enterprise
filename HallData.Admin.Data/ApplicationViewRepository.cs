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
	public class ApplicationViewRepository : DeletableRepository<int, ApplicationViewResult, ApplicationViewForAdd, ApplicationViewForUpdate>,
		IApplicationViewRepository
	{
		public ApplicationViewRepository(Database db)
			: base(db, "ui.usp_select_applicationviews", "ui.usp_select_applicationviews", "ui.usp_insert_applicationviews", "ui.usp_update_applicationviews", "ui.usp_delete_applicationviews", null)
		{
		}

		public override Task<QueryResult<ApplicationViewResult>> Get(int id, Guid? userId = null, CancellationToken token = new CancellationToken())
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
			this.PopulateGetCommand(id, cmd);
			return ReadQueryResult<ApplicationViewResult>(cmd, userId, token);
		}

		public override Task<QueryResult<JObject>> GetView(int id, Guid? userId = null, CancellationToken token = new CancellationToken())
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
			this.PopulateGetCommand(id, cmd);
			return ReadView(cmd, userId, token);
		}

		public async Task<int> Copy(int sourceApplicationViewId, string targetName, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand("ui.usp_copy_applicationviews");
			cmd.AddParameter("sourceapplicationviewid", sourceApplicationViewId);
			cmd.AddParameter("targetname", targetName);
			PopulateUserIdParameter(cmd, userId);
			return (int) await Execute(cmd, () => this.Database.ExecuteScalarAsync(cmd, token));
		}

		public Task<QueryResult<ApplicationViewResult>> GetByName(string name, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
			cmd.AddParameter("name", name);
			return ReadQueryResult<ApplicationViewResult>(cmd, userId, token);
		}

		public Task<QueryResults<ApplicationViewResult>> GetByDataView(int dataViewId, string viewName = null, Guid? userId = null, FilterContext<ApplicationViewResult> filter = null, SortContext<ApplicationViewResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
			cmd.AddParameter("dataviewid", dataViewId);
			return ReadQueryResults<ApplicationViewResult>(cmd,viewName,userId,filter,sort,page,token: token);
		}

		protected override void PopulateDeleteCommand(int id, DbCommand cmd)
		{
			cmd.AddParameter("applicationviewid", id);
		}

		protected override int ReadKeyFromScalarReturnObject(object obj, ApplicationViewForAdd view)
		{
			return (int)obj;
		}

		protected override void PopulateChangeStatusCommand(DbCommand cmd, int id)
		{
			throw new NotImplementedException();
		}

		protected override void PopulateGetCommand(int id, DbCommand cmd)
		{
			cmd.AddParameter("applicationviewid", id);
		}
	}
}
