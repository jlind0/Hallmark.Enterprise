using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;

namespace HallData.EMS.Data
{
    public interface IReadOnlyPersonRepository<TKey, TPersonResult> : IReadOnlyPartyRepository<TKey, TPersonResult>
        where TPersonResult : IPersonResult<TKey> { }
    public interface IReadOnlyPersonRepository<TPersonResult> : IReadOnlyPersonRepository<Guid, TPersonResult>, IReadOnlyPartyRepository<TPersonResult>
        where TPersonResult : IPersonResult { }
    public interface IReadOnlyPersonRepository : IReadOnlyPersonRepository<PersonResult> { }
    public interface IPersonRepository<TKey, TPersonResult, TPersonForAdd, TPersonForUpdate> : IPartyRepository<TKey, TPersonResult, TPersonForAdd, TPersonForUpdate>, IReadOnlyPersonRepository<TKey, TPersonResult>
        where TPersonResult : IPersonResult<TKey>
        where TPersonForAdd : IPersonForAdd<TKey> 
        where TPersonForUpdate: IPersonForUpdate<TKey>
        { }
    public interface IPersonRepository<TPersonResult, TPersonForAdd, TPersonForUpdate> : IPersonRepository<Guid, TPersonResult, TPersonForAdd, TPersonForUpdate>, 
        IPartyRepository<TPersonResult, TPersonForAdd, TPersonForUpdate>, IReadOnlyPersonRepository<TPersonResult>
        where TPersonResult: IPersonResult
        where TPersonForAdd : IPersonForAdd
        where TPersonForUpdate: IPersonForUpdate{ }
    public interface IPersonRepository : IPersonRepository<PersonResult, PersonForAdd, PersonForUpdate>, IReadOnlyPersonRepository { }
}
