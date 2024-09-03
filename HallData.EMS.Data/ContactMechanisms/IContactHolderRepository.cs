using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Enums;
using HallData.ApplicationViews;
using HallData.Repository;
using System.Threading;
using Newtonsoft.Json.Linq;
namespace HallData.EMS.Data
{
    public interface IReadOnlyContactHolderRepository<TKey, TId, THolderBase> : IRepository
        where THolderBase : IContactMechanismHolderKey<TKey>
        where TKey: IContactMechanismHolderId<TId>
    {
        Task<QueryResults<TResultHolder>> GetContactMechanisms<TResultHolder, TContactMechanism>(MechanismTypes? mechanismType = null, string contactMechanismTypeName = null, string viewName = null, Guid? userId = null, FilterContext<TResultHolder> filter = null,
            SortContext<TResultHolder> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
            where TContactMechanism : IContactMechanismResult
            where TResultHolder : IContactMechanismHolderResult<TKey, TContactMechanism>, THolderBase;
        Task<QueryResults<JObject>> GetContactMechanismsView(MechanismTypes? mechanismType = null, string contactMechanismTypeName = null, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResults<TResultHolder>> GetContactMechanismsById<TResultHolder, TContactMechanism>(TId id, MechanismTypes? mechanismType = null, string contactMechanismTypeName = null, string viewName = null, Guid? userId = null, FilterContext<TResultHolder> filter = null,
            SortContext<TResultHolder> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
            where TContactMechanism : IContactMechanismResult
            where TResultHolder : IContactMechanismHolderResult<TKey, TContactMechanism>, THolderBase;
        Task<QueryResults<JObject>> GetContactMechanismsByIdView(TId id, MechanismTypes? mechanismType = null, string contactMechanismTypeName = null, string viewName = null, Guid? userId = null, FilterContext filter = null,
            SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResult<TResultHolder>> GetContactMechanism<TResultHolder, TContactMechanism>(TKey key, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
            where TContactMechanism : IContactMechanismResult
            where TResultHolder : IContactMechanismHolderResult<TKey, TContactMechanism>, THolderBase;
        Task<QueryResult<JObject>> GetContactMechanismView(TKey key, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }
    public interface IContactHolderRepository<TKey, TId, THolderBase> : IReadOnlyContactHolderRepository<TKey, TId, THolderBase>
        where THolderBase : IContactMechanismHolderKey<TKey>
        where TKey : IContactMechanismHolderId<TId>
    {
        Task AddContactMechanism<TAddHolder, TContactMechanism>(TAddHolder view, Guid? userId = null, CancellationToken token = default(CancellationToken))
            where TAddHolder : IContactMechanismHolderForAdd<TKey, TContactMechanism>, THolderBase
            where TContactMechanism : IContactMechanismForAdd;
        Task UpdateContactMechanism<TUpdateHolder, TContactMechanism>(TUpdateHolder view, Guid? userId = null, CancellationToken token = default(CancellationToken))
            where TUpdateHolder : IContactMechanismHolderForUpdate<TKey, TContactMechanism>, THolderBase
            where TContactMechanism : IContactMechanismForUpdate;
        Task DeleteContactMechanism(TKey key, bool isHard = false, bool cascade = false, Guid? userId = null, CancellationToken token = default(CancellationToken));
        Task<ChangeStatusResult> ChangeStatusContactMechanism(TKey key, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }
}
