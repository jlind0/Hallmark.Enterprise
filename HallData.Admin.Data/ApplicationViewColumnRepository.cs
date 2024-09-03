using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.ApplicationViews;
using HallData.Data;
using HallData.Repository;
using Newtonsoft.Json.Linq;

namespace HallData.Admin.Data
{
	public class ApplicationViewColumnRepository : DeletableRepository<int, ApplicationViewColumnResult, ApplicationViewColumnForAdd, ApplicationViewColumnForUpdate>, 
		IApplicationViewColumnRepository
	{
		public ApplicationViewColumnRepository(Database db)
			: base(db, "ui.usp_select_applicationviewcolumns", "ui.usp_select_applicationviewcolumns", "ui.usp_insert_applicationviewcolumns", "ui.usp_update_applicationviewcolumns",
			"ui.usp_delete_applicationviewcolumns", null)
		{

		}

		protected override void PopulateDeleteCommand(int id, DbCommand cmd)
		{
			cmd.AddParameter("applicationviewcolumnid", id);
		}

		protected override int ReadKeyFromScalarReturnObject(object obj, ApplicationViewColumnForAdd view)
		{
			return (int) obj;
		}

		protected override void PopulateChangeStatusCommand(DbCommand cmd, int id)
		{
			throw new NotImplementedException();
		}

		protected override void PopulateGetCommand(int id, DbCommand cmd)
		{
			cmd.AddParameter("applicationviewcolumnid", id);
		}

		public override Task<QueryResult<ApplicationViewColumnResult>> Get(int id, Guid? userId = null, CancellationToken token = new CancellationToken())
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
			this.PopulateGetCommand(id, cmd);
			return ReadQueryResult<ApplicationViewColumnResult>(cmd, userId, token);
		}

		public override Task<QueryResult<JObject>> GetView(int id, Guid? userId = null, CancellationToken token = new CancellationToken())
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
			this.PopulateGetCommand(id, cmd);
			return ReadView(cmd, userId, token);
		}

		public Task<QueryResults<ApplicationViewColumnResult>> GetByApplicationView(int applicationViewId, string viewName = null, 
			Guid? userId = null, FilterContext<ApplicationViewColumnResult> filter = null, 
			SortContext<ApplicationViewColumnResult> sort = null, PageDescriptor page = null, 
			CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetAllStoredProcName);
			cmd.AddParameter("applicationViewId", applicationViewId);
			return this.ReadQueryResults<ApplicationViewColumnResult>(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public override Task<ChangeStatusResult> ChangeStatus(int id, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}
	}
}
