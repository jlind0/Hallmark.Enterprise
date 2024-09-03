using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Enums;
using HallData.EMS.ApplicationViews.Results;
using HallData.Repository;
using HallData.ApplicationViews;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Data
{
    public interface IPartyRepository : IPartyRepository<PartyResult, PartyForAdd, PartyForUpdate>, IReadOnlyPartyRepository { }

    public interface IPartyRepository<TPartyResult, TPartyForAdd, TPartyForUpdate> : IPartyRepository<Guid, TPartyResult, TPartyForAdd, TPartyForUpdate>, IReadOnlyPartyRepository<TPartyResult>
        where TPartyResult: IPartyResult
        where TPartyForAdd : IPartyForAdd
        where TPartyForUpdate: IPartyForUpdate
        { }

    public interface IPartyRepository<TKey, TPartyResult, TPartyForAdd, TPartyForUpdate> : IDeletableRepository<TKey, TPartyResult, TPartyForAdd, TPartyForUpdate>, IReadOnlyPartyRepository<TKey, TPartyResult>
        where TPartyResult: IPartyResult<TKey>
        where TPartyForAdd: IPartyForAdd<TKey>
        where TPartyForUpdate: IPartyForUpdate<TKey>
    {
        Task AddCategory(Guid partyID, int categoryID, Guid userID, CancellationToken token = default(CancellationToken));
        Task RemoveCategory(Guid partyID, int categoryID, Guid userID, CancellationToken token = default(CancellationToken));
    }

    public interface IReadOnlyPartyRepository<TKey, TPartyResult> : IReadOnlyRepository<TKey, TPartyResult>
        where TPartyResult : IPartyResult<TKey>
    {
        
    }

    public interface IReadOnlyPartyRepository<TPartyResult> : IReadOnlyPartyRepository<Guid, TPartyResult>
        where TPartyResult : IPartyResult { }

    public interface IReadOnlyPartyRepository : IReadOnlyPartyRepository<PartyResult> { }

    public interface IReadOnlyPartyContactMechanismRepoistory : IReadOnlyContactHolderRepository<PartyContactMechanismId, Guid, PartyContactMechanismKey> { }

    public interface IPartyContactMechanismRepository : IReadOnlyPartyContactMechanismRepoistory, IContactHolderRepository<PartyContactMechanismId, Guid, PartyContactMechanismKey> { }
}