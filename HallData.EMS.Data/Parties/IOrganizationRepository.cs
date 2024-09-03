using HallData.EMS.ApplicationViews.Results;
using System;
namespace HallData.EMS.Data
{
    public interface IOrganizationRepository : IOrganizationRepository<OrganizationResult, OrganizationForAdd, OrganizationForUpdate>, IReadOnlyOrganizationRepository { }
    public interface IOrganizationRepository<TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate> : 
        IOrganizationRepository<Guid, TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate>, IPartyRepository<TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate>, IReadOnlyOrganizationRepository<TOrganizationResult>
        where TOrganizationResult: IOrganizationResult
        where TOrganizationForAdd : IOrganizationForAdd 
        where TOrganizationForUpdate: IOrganizationForUpdate{ }
    public interface IOrganizationRepository<TKey, TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate> : IPartyRepository<TKey, TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate>, IReadOnlyOrganizationRepository<TKey, TOrganizationResult>
        where TOrganizationResult: IOrganizationResult<TKey>
        where TOrganizationForAdd : IOrganizationForAdd<TKey> 
        where TOrganizationForUpdate: IOrganizationForUpdate<TKey>{ }
    public interface IReadOnlyOrganizationRepository<TKey, TOrganizationResult> : IReadOnlyPartyRepository<TKey, TOrganizationResult>
        where TOrganizationResult : IOrganizationResult<TKey> { }
    public interface IReadOnlyOrganizationRepository<TOrganizationResult> : IReadOnlyOrganizationRepository<Guid, TOrganizationResult>, IReadOnlyPartyRepository<TOrganizationResult>
        where TOrganizationResult : IOrganizationResult { }
    public interface IReadOnlyOrganizationRepository : IReadOnlyOrganizationRepository<OrganizationResult> { }
}