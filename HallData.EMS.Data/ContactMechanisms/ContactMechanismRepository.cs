using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Repository;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Enums;
using HallData.Data;
using System.Threading;
using System.Data.SqlClient;
using HallData.ApplicationViews;
using System.Data.Common;

namespace HallData.EMS.Data
{
    public class ContactMechanismRepository : DeletableRepository<Guid, ContactMechanismGeneric, ContactMechanismGenericForAdd, ContactMechanismGenericForUpdate>, IContactMechanismRepository
    {
        public ContactMechanismRepository(Database db) : base(db, "usp_select_contactmechanisms", "usp_select_contactmechanisms", "usp_insert_contactmechanisms", "usp_update_contactmechanisms", "usp_delete_contactmechanisms", "usp_changestatus_contactmechanisms") { }


        protected override void PopulateDeleteCommand(Guid id, DbCommand cmd)
        {
            cmd.AddParameter("contactmechanismguid", id);
        }

        protected override Guid ReadKeyFromScalarReturnObject(object obj, ContactMechanismGenericForAdd view)
        {
            return (Guid)obj;
        }

        protected override void PopulateChangeStatusCommand(DbCommand cmd, Guid id)
        {
            cmd.AddParameter("contactmechanismguid", id);
        }

        protected override void PopulateGetCommand(Guid id, DbCommand cmd)
        {
            cmd.AddParameter("contactmechanismguid", id);
        }

        public Task<QueryResults<ContactMechanismGeneric>> Get(MechanismTypes mechanismType, string viewName = null, Guid? userId = null, FilterContext<ContactMechanismGeneric> filter = null, SortContext<ContactMechanismGeneric> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand(this.GetAllStoredProcName);
            cmd.AddParameter("mechanismtypeid", (int)mechanismType);
            return this.ReadQueryResults<ContactMechanismGeneric>(cmd, viewName, userId, filter, sort, page, token: token);
        }


        public Task<QueryResults<Newtonsoft.Json.Linq.JObject>> GetView(MechanismTypes mechanismType, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand(this.GetAllStoredProcName);
            cmd.AddParameter("mechanismtypeid", (int)mechanismType);
            return this.ReadViews(cmd, viewName, userId, filter, sort, page, token: token);
        }
    }
}
