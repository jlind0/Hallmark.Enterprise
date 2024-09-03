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
	public class DataViewColumnRepository : DeletableRepository<int, DataViewColumnResult, DataViewColumnForAdd, DataViewColumnForUpdate>,
		IDataViewColumnRepository
	{
		public DataViewColumnRepository(Database db)
			: base(db, "ui.usp_select_dataviewcolumns", "ui.usp_select_dataviewcolumns", "ui.usp_insert_dataviewcolumns", "ui.usp_update_dataviewcolumns", "ui.usp_delete_dataviewcolumns", null)
		{
		}

		protected virtual void PopulatePathParameter(DbCommand cmd, DataViewColumnPathForAddUpdate path)
		{
			if (path != null)
			{
				cmd.AddParameter("dataviewcolumnid", path);
			}
		}

		public override Task<QueryResult<DataViewColumnResult>> Get(int id, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
			this.PopulateGetCommand(id, cmd);
			return ReadQueryResult<DataViewColumnResult>(cmd, userId, token); 
		}

		public Task<QueryResults<DataViewColumnResult>> GetChildren(int dataViewResultId, string viewName = null, Guid? userId = null, FilterContext<DataViewColumnResult> filter = null,
			SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetAllStoredProcName);
			cmd.AddParameter("dataviewresultid", dataViewResultId);
            cmd.AddParameter("fetchchildren", true);
			return ReadQueryResults<DataViewColumnResult>(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public Task AddPath(DataViewColumnPathForAddUpdate path, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand("ui.usp_addpath_dataviewcolumns");
			PopulatePathParameter(cmd, path);
			PopulateUserIdParameter(cmd, userId);
			return Execute(cmd, () => this.Database.ExecuteScalarAsync(cmd, token));
		}

		public Task UpdatePath(DataViewColumnPathForAddUpdate path, Guid? userId = null,
			CancellationToken token = new CancellationToken())
		{
			var cmd = this.Database.CreateStoredProcCommand("ui.usp_updatepath_dataviewcolumns");
			PopulatePathParameter(cmd, path);
			PopulateUserIdParameter(cmd, userId);
			return Execute(cmd, () => this.Database.ExecuteScalarAsync(cmd, token));
		}

		public Task RemovePath(int dataViewColumnId, int? orderIndex = null, Guid? userId = null,
			CancellationToken token = new CancellationToken())
		{
			var cmd = this.Database.CreateStoredProcCommand("ui.usp_removepath_dataviewcolumns");
			cmd.AddParameter("dataviewcolumnid", dataViewColumnId);
			PopulateUserIdParameter(cmd, userId);
			return  Execute(cmd, () => this.Database.ExecuteScalarAsync(cmd, token));
		}

		protected override void PopulateGetCommand(int id, DbCommand cmd)
		{
			cmd.AddParameter("dataviewcolumnid", id);
		}

		protected override int ReadKeyFromScalarReturnObject(object obj, DataViewColumnForAdd view)
		{
			return (int)obj;
		}

		protected override void PopulateChangeStatusCommand(DbCommand cmd, int id)
		{
			throw new NotImplementedException();
		}

		protected override void PopulateDeleteCommand(int id, DbCommand cmd)
		{
			cmd.AddParameter("dataviewcolumnid", id);
		}

		public Task<QueryResults<DataViewColumnResult>> GetByDataView(int dataViewId, int? orderIndex = null, RecursionLevel recursion = RecursionLevel.None, string viewName = null,
			Guid? userId = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null,
			CancellationToken token = new CancellationToken())
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetAllStoredProcName);
			cmd.AddParameter("dataviewid", dataViewId);
			cmd.AddParameter("recursionlevel", (int)recursion);
			return this.ReadQueryResults<DataViewColumnResult>(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public Task<QueryResults<DataViewColumnResult>> GetByDataViewResult(int dataViewResultId, RecursionLevel recursion = RecursionLevel.None, string viewName = null,
			Guid? userId = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null,
			CancellationToken token = new CancellationToken())
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetAllStoredProcName);
			cmd.AddParameter("dataviewresultid", dataViewResultId);
			cmd.AddParameter("recursionlevel", (int)recursion);
			return this.ReadQueryResults<DataViewColumnResult>(cmd, viewName, userId, filter, sort, page, token: token);
		}

        public override Task<QueryResult<JObject>> GetView(int id, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
            this.PopulateGetCommand(id, cmd);
            return ReadView(cmd, userId, token); 
        }
        public override Task<ChangeStatusResult> ChangeStatus(int id, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
        public Task<QueryResults<DataViewColumnResult>> GetByParent(int parentResultColumnId, string viewName = null, Guid? userId = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand(this.GetAllStoredProcName);
            cmd.AddParameter("parentResultColumnId", parentResultColumnId);
            return this.ReadQueryResults<DataViewColumnResult>(cmd, viewName, userId, filter, sort, page, token: token);
        }


        public Task<QueryResults<DataViewColumnResult>> GetByInterfaceAttribute(int interfaceAttributeId, string viewName = null, Guid? userId = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand(this.GetAllStoredProcName);
            cmd.AddParameter("interfaceAttributeId", interfaceAttributeId);
            return this.ReadQueryResults<DataViewColumnResult>(cmd, viewName, userId, filter, sort, page, token: token);
        }
    }
}
