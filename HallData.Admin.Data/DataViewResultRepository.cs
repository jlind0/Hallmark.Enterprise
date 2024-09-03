using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.ApplicationViews;
using HallData.Data;
using HallData.Repository;
using Newtonsoft.Json.Linq;

namespace HallData.Admin.Data
{
	public class DataViewResultRepository : DeletableRepository<int, DataViewResultResult, DataViewResultForAdd, DataViewResultForUpdate>, IDataViewResultRepository
	{

		public DataViewResultRepository(Database db) 
			: base(db, "ui.usp_select_dataviewresults", "ui.usp_select_dataviewresults", "ui.usp_insert_dataviewresults", "ui.usp_update_dataviewresults", "ui.usp_delete_dataviewresults", null)
		{
			
		}

		protected override void PopulateDeleteCommand(int id, System.Data.Common.DbCommand cmd)
		{
			cmd.AddParameter("dataviewresultid", id);
		}

		protected override int ReadKeyFromScalarReturnObject(object obj, DataViewResultForAdd view)
		{
			return (int) obj;
		}

		protected override void PopulateChangeStatusCommand(System.Data.Common.DbCommand cmd, int id)
		{
			throw new NotImplementedException();
		}

		protected override void PopulateGetCommand(int id, System.Data.Common.DbCommand cmd)
		{
			cmd.AddParameter("dataviewresultid", id);
		}

		public override Task<QueryResult<DataViewResultResult>> Get(int id, Guid? userId = null, CancellationToken token = new CancellationToken())
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
			this.PopulateGetCommand(id, cmd);
			return ReadQueryResult<DataViewResultResult>(cmd, userId, token);   
		}

		public override Task<QueryResult<JObject>> GetView(int id, Guid? userId = null,
			CancellationToken token = new CancellationToken())
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
			this.PopulateGetCommand(id, cmd);
			return ReadView(cmd, userId, token);
		}

		public Task<QueryResults<DataViewResultResult>> GetChildren(int dataViewResultId, string viewName = null, Guid? userId = null,
			FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null,
			PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetAllStoredProcName);
			cmd.AddParameter("dataviewresultid", dataViewResultId);
			cmd.AddParameter("fetchchildren", true);
			return this.ReadQueryResults<DataViewResultResult>(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public Task<QueryResults<DataViewResultResult>> GetByDataView(int dataViewId, int? orderIndex = null, RecursionLevel recursion = RecursionLevel.None,
			string viewName = null, Guid? userId = null, FilterContext<DataViewResultResult> filter = null,
			SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetAllStoredProcName);
			cmd.AddParameter("dataviewid", dataViewId);
            if (orderIndex != null)
                cmd.AddParameter("orderIndex", orderIndex.Value);
			cmd.AddParameter("recursionlevel", (int)recursion);
			return this.ReadQueryResults<DataViewResultResult>(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public override Task<ChangeStatusResult> ChangeStatus(int id, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}


        public Task<QueryResults<DataViewResultResult>> GetByCollectionAttribute(int collectionInterfaceAttributeId, RecursionLevel recursion = RecursionLevel.None, string viewName = null, Guid? userId = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand(this.GetAllStoredProcName);
            cmd.AddParameter("collectionInterfaceAttributeId", collectionInterfaceAttributeId);
            cmd.AddParameter("recursionlevel", (int)recursion);
            return this.ReadQueryResults<DataViewResultResult>(cmd, viewName, userId, filter, sort, page, token: token);
        }
    }
}
