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
    public class InterfaceAttributeRepository : DeletableRepository<int, InterfaceAttributeResult, InterfaceAttributeForAdd, InterfaceAttributeForUpdate>, IInterfaceAttributeRepository
    {
        public InterfaceAttributeRepository(Database db) : base(db, "ui.usp_select_interfaceattributes", 
            @"select * from 
                ui.v_interfaceattributes where 
                [interfaceattributeid#] = @interfaceattributeid and [__userguid?] = @__userguid", "ui.usp_insert_interfaceattributes",
            "ui.usp_update_interfaceattributes", "ui.usp_delete_interfaceattributes", null) { }

        public override Task<ChangeStatusResult> ChangeStatus(int id, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
        public Task<QueryResults<InterfaceAttributeResult>> GetByInterface(int interfaceId, RecursionLevel recursion = RecursionLevel.None, string viewName = null, Guid? userId = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand(this.GetAllStoredProcName);
            cmd.AddParameter("interfaceid", interfaceId);
            cmd.AddParameter("recursionlevel", (int)recursion);
            return this.ReadQueryResults<InterfaceAttributeResult>(cmd, viewName, userId, filter, sort, page, token: token);
        }

        protected override void PopulateDeleteCommand(int id, DbCommand cmd)
        {
            cmd.AddParameter("interfaceattributeid", id);
        }

        protected override int ReadKeyFromScalarReturnObject(object obj, InterfaceAttributeForAdd view)
        {
            return (int)obj;
        }

        protected override void PopulateChangeStatusCommand(DbCommand cmd, int id)
        {
            throw new NotImplementedException();
        }

        protected override void PopulateGetCommand(int id, DbCommand cmd)
        {
            cmd.AddParameter("interfaceattributeid", id);
        }
    }
}
