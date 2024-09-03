using System;

namespace HallData.EMS.ApplicationViews.Results
{
    /// <summary>
    /// Interface for Party Generic with restriction by Party and Tier type
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TTierType">Tier type</typeparam>
    public interface IPartyGeneric<TKey, TPartyType, TTierType> : IParty<TKey, TPartyType>,
        IOrganization<TKey, TPartyType, TTierType>, IPerson<TKey, TPartyType>
        where TPartyType : PartyTypeKey
        where TTierType : TierTypeKey
    { }

    /// <summary>
    /// Interface for Party Generic with restriction by Party, Tier and Status type
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TPartyType">Party type</typeparam>
    /// <typeparam name="TTierType">Tier type</typeparam>
    /// <typeparam name="TStatusType">Status type</typeparam>
    public interface IPartyGeneric<TKey, TPartyType, TTierType, TStatusType> : IParty<TKey, TPartyType, TStatusType>,
        IOrganization<TKey, TPartyType, TTierType, TStatusType>,
        IPerson<TKey, TPartyType, TStatusType>
        where TPartyType : PartyTypeKey
        where TTierType : TierTypeKey
        where TStatusType : StatusTypeKey
    { }

    public interface IPartyGenericForAddBase<TKey> : IPartyGeneric<TKey, PartyTypeKey, TierTypeKey, StatusTypeKey>, IPersonForAddBase<TKey>, IOrganizationForAddBase<TKey> 
    { }

    public interface IPartyGenericForAddBase : IPartyGenericForAddBase<Guid>, IOrganizationForAddBase, IPersonForAddBase 
    { }

    /// <summary>
    /// Interface for Party Generic for Add operation with Key type
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    public interface IPartyGenericForAdd<TKey> : IPersonForAdd<TKey>, IOrganizationForAdd<TKey>, IPartyGenericForAddBase<TKey> 
    { }

    /// <summary>
    /// Interface for Party Generic for Add operation
    /// </summary>
    public interface IPartyGenericForAdd : IPartyGenericForAdd<Guid>, IPersonForAdd, IOrganizationForAdd, IPartyGenericForAddBase 
    { }

    public interface IPartyGenericForUpdateBase<TKey> : IPersonForUpdateBase<TKey>, IOrganizationForUpdateBase<TKey>, IPartyGeneric<TKey, PartyTypeKey, TierTypeKey> 
    { }

    public interface IPartyGenericForUpdateBase : IPartyGenericForUpdateBase<Guid>, IPersonForUpdateBase, IOrganizationForUpdateBase 
    { }

    /// <summary>
    /// Interface for Party Generic for Update operation with Key type
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    public interface IPartyGenericForUpdate<TKey> : IOrganizationForUpdate<TKey>, IPersonForUpdate<TKey>, IPartyGenericForUpdateBase<TKey> 
    { }

    public interface IPartyGenericForUpdate : IPartyGenericForUpdate<Guid>, IPersonForUpdate, IOrganizationForUpdate, IPartyGenericForUpdateBase 
    { }




    public interface IPartyGenericResultBase<TKey> : IPartyResultBase<TKey>, IPartyGeneric<TKey, PartyTypeResult, TierType, StatusTypeResult> 
    { }

    public interface IPartyGenericResultBase : IPartyGenericResultBase<Guid>, IPartyResultBase 
    { }

    /// <summary>
    /// Interface for Party Generic Result with Key type 
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    public interface IPartyGenericResult<TKey> : IPartyResult<TKey>, IPartyGenericResultBase<TKey> 
    { }

    /// <summary>
    /// Interface for Party Generic Result  
    /// </summary>
    public interface IPartyGenericResult : IPartyGenericResultBase, IPartyResult 
    { }
}
