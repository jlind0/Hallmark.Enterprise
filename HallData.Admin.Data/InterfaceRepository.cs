using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Data;
using HallData.Repository;
using HallData.Admin.ApplicationViews;
using HallData.ApplicationViews;
using System.Threading;
using System.Data.Common;
using Newtonsoft.Json.Linq;

namespace HallData.Admin.Data
{
    public class InterfaceRepository : DeletableRepository<int, InterfaceResult, InterfaceForAdd, InterfaceForUpdate>, IInterfaceRepository
    {
        public InterfaceRepository(Database db) : base(db, "ui.usp_select_interfaces", "ui.usp_select_interfaces", "ui.usp_insert_interfaces", "ui.usp_update_interfaces",
            "ui.usp_delete_interfaces", null) { }

        public Task RelateInterfaces(int interfaceId, int relatedInterfaceId, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand("ui.usp_relate_interfaces");
            cmd.AddParameter("interfaceid", interfaceId);
            cmd.AddParameter("relatedinterfaceid", relatedInterfaceId);
            PopulateUserIdParameter(cmd, userId);
            return Execute(cmd, () => this.Database.ExecuteNonQueryAsync(cmd, token));
        }

        public Task UnRelateInterfaces(int interfaceId, int relatedInterfaceId, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand("ui.usp_unrelate_interfaces");
            cmd.AddParameter("interfaceid", interfaceId);
            cmd.AddParameter("relatedinterfaceid", relatedInterfaceId);
            PopulateUserIdParameter(cmd, userId);
            return Execute(cmd, () => this.Database.ExecuteNonQueryAsync(cmd, token));
        }

        public override Task<QueryResult<InterfaceResult>> Get(int id, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
            this.PopulateGetCommand(id, cmd);
            return ReadQueryResult<InterfaceResult>(cmd, userId, token);   
        }

        public override Task<QueryResult<JObject>> GetView(int id, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
            this.PopulateGetCommand(id, cmd);
            return ReadView(cmd, userId, token);
        }

        public Task<QueryResult<InterfaceResult>> GetByName(string name, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand(this.GetSqlQuery);
            cmd.AddParameter("name", name);
            return ReadQueryResult<InterfaceResult>(cmd, userId, token);
        }

        public async Task<bool> AreInterfacesCommon(int interfaceId1, int interfaceId2, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand("ui.usp_arecommon_interfaces");
            cmd.AddParameter("interfaceid1", interfaceId1);
            cmd.AddParameter("interfaceid2", interfaceId2);
            PopulateUserIdParameter(cmd, userId);
            return (bool)await Execute(cmd, () => this.Database.ExecuteScalarAsync(cmd, token));
        }
        public override Task<ChangeStatusResult> ChangeStatus(int id, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
        protected override void PopulateDeleteCommand(int id, DbCommand cmd)
        {
            cmd.AddParameter("interfaceid", id);
        }

        protected override int ReadKeyFromScalarReturnObject(object obj, InterfaceForAdd view)
        {
            return (int)obj;
        }

        protected override void PopulateChangeStatusCommand(DbCommand cmd, int id)
        {
            throw new NotImplementedException();
        }

        protected override void PopulateGetCommand(int id, DbCommand cmd)
        {
            cmd.AddParameter("interfaceid", id);
        }
    }
}
