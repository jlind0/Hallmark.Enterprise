using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.EMS.ApplicationViews.Results
{
    /// <summary>
    /// Interface for Customer Organization Key 
    /// </summary>
    public interface ICustomerOrganizationKey : IOrganizationKey<CustomerId>, ICustomerKey { }
    /// <summary>
    /// Interface for Customer Organization with restriction by Party and Tier type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TTierType">Tier type</typeparam>
    public interface ICustomerOrganization<TPartyType, TTierType> : IOrganization<CustomerId, TPartyType, TTierType>, ICustomer<TPartyType>, ICustomerOrganizationKey
        where TTierType : TierTypeKey
        where TPartyType : PartyTypeKey
    {

    }
    public interface ICustomerOrganizationWithStatus<TStatusType> : ICustomerWithStatus<TStatusType>, ICustomerOrganizationKey
        where TStatusType: StatusTypeKey
    {

    }
    /// <summary>
    /// Interface for Customer Organization with restriction by Party, Tier and Status type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TTierType">Tier type</typeparam>
    /// <typeparam name="TStatusType">Status type</typeparam>
    public interface ICustomerOrganization<TPartyType, TTierType, TStatusType> : IOrganization<CustomerId, TPartyType, TTierType, TStatusType>,
        ICustomer<TPartyType, TStatusType>, ICustomerOrganization<TPartyType, TTierType>, ICustomerOrganizationWithStatus<TStatusType>
        where TStatusType: StatusTypeKey
        where TTierType: TierTypeKey
        where TPartyType: PartyTypeKey
    {

    }
    /// <summary>
    /// Interface for Customer Organization with restriction by Party, Tier, Status and Organization type
    /// </summary>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TTierType">Tier type</typeparam>
    /// <typeparam name="TStatusType">Status type</typeparam>
    /// <typeparam name="TOrganization">Organization type</typeparam>
    public interface ICustomerOrganization<TPartyType, TTierType, TStatusType, TOrganization> : ICustomerOrganization<TPartyType, TTierType, TStatusType>,
        ICustomer<TPartyType, TStatusType, TOrganization>
        where TStatusType: StatusTypeKey
        where TTierType: TierTypeKey
        where TPartyType: PartyTypeKey
        where TOrganization : IOrganizationKey { }
    public interface ICustomerOrganizationForAddRelationship : ICustomerOrganizationWithStatus<StatusTypeKey>, ICustomerForAddRelationship { }
    public interface ICustomerOrganizationForAddBase : ICustomerOrganization<PartyTypeKey, TierTypeKey, StatusTypeKey>, ICustomerForAddBase, 
        IOrganizationForAddBase<CustomerId> { }
    /// <summary>
    /// Interface for Customer Organization for Add operation 
    /// </summary>
    public interface ICustomerOrganizationForAdd : ICustomerOrganizationForAddBase, ICustomerForAdd, IOrganizationForAdd<CustomerId>, ICustomerForAddRelationship { }

    public interface ICustomerOrganizationForUpdateBase : ICustomerOrganization<PartyTypeKey, TierTypeKey>, ICustomerForUpdateBase, 
        IOrganizationForUpdateBase<CustomerId> { }
    public interface ICustomerOrganizationForUpdateRelationship : ICustomerOrganizationKey, ICustomerForUpdateRelationship { }
    /// <summary>
    /// Interface for Customer Organization for Update operation 
    /// </summary>
    public interface ICustomerOrganizationForUpdate : ICustomerOrganizationForUpdateBase, ICustomerForUpdate, IOrganizationForUpdate<CustomerId> { }
    public interface ICustomerOrganizationResultBase : ICustomerOrganization<PartyTypeResult, TierType, StatusTypeResult, OrganizationResult>, ICustomerResultBase, IOrganizationResultBase<CustomerId> { }
    /// <summary>
    /// Interface for Customer Organization Result
    /// </summary>
    public interface ICustomerOrganizationResult : ICustomerOrganizationResultBase, ICustomerResult, IOrganizationResult<CustomerId> { }
}
