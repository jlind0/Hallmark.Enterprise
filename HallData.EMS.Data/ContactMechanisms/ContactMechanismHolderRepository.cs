using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Data;
using HallData.Repository;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Enums;
using System.Threading;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;
using System.Data.SqlClient;
using System.Data.Common;

namespace HallData.EMS.Data
{
    public abstract class ContactMechanismHolderRepository<TKey, TId, THolderBase> : Repository.Repository, IContactHolderRepository<TKey, TId, THolderBase>
        where THolderBase : IContactMechanismHolderKey<TKey>
        where TKey: IContactMechanismHolderId<TId>
    {
        protected string GetProcedure { get; private set; }
        protected string AddProcedure { get; private set; }
        protected string UpdateProcedure { get; private set; }
        protected string DeleteProcedure { get; private set; }
        protected string ChangeStatusProcedure { get; private set; }
        protected ContactMechanismHolderRepository(Database db, string getProcedure, string addProcedure, 
            string updateProcedure, string deleteProcedure, string changeStatusProcedure)
            :base(db)
        {
            this.GetProcedure = getProcedure;
            this.AddProcedure = addProcedure;
            this.UpdateProcedure = updateProcedure;
            this.DeleteProcedure = deleteProcedure;
            this.ChangeStatusProcedure = changeStatusProcedure;
        }

        public virtual async Task AddContactMechanism<TAddHolder, TContactMechanism>(TAddHolder view, Guid? userId = null, CancellationToken token = default(CancellationToken))
            where TAddHolder : IContactMechanismHolderForAdd<TKey, TContactMechanism>, THolderBase
            where TContactMechanism : IContactMechanismForAdd
        {
            var cmd = this.Database.CreateStoredProcCommand(this.AddProcedure);
            cmd.MapParameters(view, this.Database, ViewOperations.Add);
            PopulateUserIdParameter(cmd, userId);
            view.ContactMechanismGuid = (Guid)await this.Execute(cmd, () => this.Database.ExecuteScalarAsync(cmd, token));
            if (view.ContactMechanism != null)
                view.ContactMechanism.ContactMechanismGuid = view.ContactMechanismGuid;
        }

        public virtual Task UpdateContactMechanism<TUpdateHolder, TContactMechanism>(TUpdateHolder view, Guid? userId = null, CancellationToken token = default(CancellationToken))
            where TUpdateHolder : IContactMechanismHolderForUpdate<TKey, TContactMechanism>, THolderBase
            where TContactMechanism : IContactMechanismForUpdate
        {
            throw new NotImplementedException();
        }

        public virtual Task DeleteContactMechanism(TKey key, bool isHard = false, bool cascade = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public virtual Task<ChangeStatusResult> ChangeStatusContactMechanism(TKey key, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public virtual Task<QueryResults<TResultHolder>> GetContactMechanisms<TResultHolder, TContactMechanism>(MechanismTypes? mechanismType = null, string contactMechanismTypeName = null, string viewName = null, Guid? userId = null, FilterContext<TResultHolder> filter = null, SortContext<TResultHolder> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
            where TResultHolder : IContactMechanismHolderResult<TKey, TContactMechanism>, THolderBase
            where TContactMechanism : IContactMechanismResult
        {
            throw new NotImplementedException();
        }

        public virtual Task<QueryResults<JObject>> GetContactMechanismsView(MechanismTypes? mechanismType = null, string contactMechanismTypeName = null, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public virtual Task<QueryResults<TResultHolder>> GetContactMechanismsById<TResultHolder, TContactMechanism>(TId id, MechanismTypes? mechanismType = null, string contactMechanismTypeName = null, string viewName = null, Guid? userId = null, FilterContext<TResultHolder> filter = null, SortContext<TResultHolder> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
            where TResultHolder : IContactMechanismHolderResult<TKey, TContactMechanism>, THolderBase
            where TContactMechanism : IContactMechanismResult
        {
            throw new NotImplementedException();
        }

        public virtual Task<QueryResults<JObject>> GetContactMechanismsByIdView(TId id, MechanismTypes? mechanismType = null, string contactMechanismTypeName = null, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public virtual Task<QueryResult<TResultHolder>> GetContactMechanism<TResultHolder, TContactMechanism>(TKey key, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
            where TResultHolder : IContactMechanismHolderResult<TKey, TContactMechanism>, THolderBase
            where TContactMechanism : IContactMechanismResult
        {
            throw new NotImplementedException();
        }

        public virtual Task<QueryResult<JObject>> GetContactMechanismView(TKey key, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
        protected abstract void PopulateKeyParameter(DbCommand cmd, TKey key);
        protected abstract void PopulateIdParameter(DbCommand cmd, TId id);
        protected virtual void PopulateMechanismTypeParameter(DbCommand cmd, MechanismTypes? mechanismType = null)
        {
            if (mechanismType != null)
                cmd.AddParameter("mechanismtype", (int)mechanismType);
        }
    }
}
