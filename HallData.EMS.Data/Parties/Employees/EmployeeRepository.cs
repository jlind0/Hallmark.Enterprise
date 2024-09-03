using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data.SqlClient;
using HallData.Data;
using HallData.Repository;
using HallData.EMS.ApplicationViews.Results;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;
using System.Data.Common;

namespace HallData.EMS.Data
{
    public class EmployeeRepository : PersonRepository<EmployeeId, EmployeeResult, EmployeeForAdd, EmployeeForUpdate>, IEmployeeRepository
    {
        public EmployeeRepository(Database db) : base(db, "usp_select_employees", "usp_select_employees", "usp_insert_employees", "usp_update_employees", "usp_delete_employees", "usp_changestatus_employees") { }
        public static void PopulateEmployeeIdParameters(EmployeeId id, DbCommand cmd)
        {
            if(id.EmployerGuid != null)
                cmd.AddParameter("employerguid", id.EmployerGuid);
            cmd.AddParameter("partyguid", id.PartyGuid);
        }
        protected override void PopulateDeleteCommand(EmployeeId id, DbCommand cmd)
        {
            PopulateEmployeeIdParameters(id, cmd);
        }

        protected override EmployeeId ReadKeyFromScalarReturnObject(object obj, EmployeeForAdd view)
        {
            Guid partyGuid = (Guid)obj;
            return new EmployeeId(view.EmployerGuid, partyGuid);
        }

        protected override void PopulateGetCommand(EmployeeId id, DbCommand cmd)
        {
            PopulateEmployeeIdParameters(id, cmd);
        }

        protected override void PopulateChangeStatusCommand(DbCommand cmd, EmployeeId id)
        {
            PopulateEmployeeIdParameters(id, cmd);
        }

        public Task<QueryResults<EmployeeResult>> GetByEmployer(Guid employerID, Guid? userID = null, FilterContext<EmployeeResult> filter = null, SortContext<EmployeeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            //TODO: usp_select_employees @employerguid = employerID
            throw new NotImplementedException();
        }

        public Task<QueryResults<JObject>> GetByEmployerView(Guid employerID, string viewName = null, Guid? userID = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            //TODO: usp_select_employees @employerguid = employerID
            throw new NotImplementedException();
        }

        public Task<ChangeStatusResult> ChangeStatusEmployeeRelationship(EmployeeId id, string statusTypeName, Guid? userID = null, CancellationToken token = default(CancellationToken))
        {
            //TODO: usp_changestatus_employees_relationship
            throw new NotImplementedException();
        }


        public Task<QueryResults<EmployeeResult>> GetEmployers(Guid partyID, Guid? userID = null, FilterContext<EmployeeResult> filter = null, SortContext<EmployeeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            //TODO: usp_select_employees @partyguid = partyID, @selectall = true
            throw new NotImplementedException();
        }

        public Task<QueryResults<JObject>> GetEmployersView(Guid partyID, Guid? userID = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            //TODO: usp_select_employees @partyguid = partyID, @selectall = true
            throw new NotImplementedException();
        }
    }
}
