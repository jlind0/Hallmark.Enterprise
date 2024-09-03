using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.ApplicationViews.Results;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Business
{
    [Service("parties")]
    public interface IPartyImplementation : IPartyImplementation<Guid, PartyResult, PartyForAdd, PartyForUpdate>, IReadOnlyPartyImplementation { }

    public interface IReadOnlyPartyImplementation<TKey, TPartyResult> : IReadOnlyBusinessImplementation<TKey, TPartyResult>
        where TPartyResult : IPartyResult<TKey> { }

    public interface IReadOnlyPartyImplementation : IReadOnlyPartyImplementation<Guid, PartyResult> { }

    public interface IPartyImplementation<TKey, TPartyResult, in TPartyForAdd, in TPartyForUpdate> : IReadOnlyPartyImplementation<TKey, TPartyResult>, 
        IDeletableBusinessImplementationWithBase<TKey, TPartyResult, TPartyForAdd, TPartyForUpdate>, IPartyContactImplementation
        where TPartyResult : IPartyResult<TKey>
        where TPartyForAdd : IPartyForAdd<TKey>
        where TPartyForUpdate: IPartyForUpdate<TKey>
    {
        [AddMethod]
        [ServiceRoute("AddCategory", "{partyID}/Categories/{categoryID}/Add/")]
        [Description("Adds a Category to a Party")]
        Task AddCategory([Description("Target party id")]Guid partyID, [Description("Target category id")]int categoryID, CancellationToken token = default(CancellationToken));
        [DeleteMethod]
        [ServiceRoute("RemoveCategory", "{partyID}/Categories/{categoryID}/Remove/")]
        [Description("Removes a Category from a Party")]
        Task RemoveCategory([Description("Target party id")]Guid partyID, [Description("Target category id")]int categoryID, CancellationToken token = default(CancellationToken));
    }
}