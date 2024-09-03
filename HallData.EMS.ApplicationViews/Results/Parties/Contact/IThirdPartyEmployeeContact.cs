
namespace HallData.EMS.ApplicationViews.Results
{
    public interface IThirdPartyEmployeeContactId : IEmployeeContactId, IThirdPartyEmployeeId { }
    public interface IThirdPartyEmployeeContactKey : IThirdPartyEmployeeKey, IEmployeeContactKey { }
    public interface IThirdPartyEmployeeContactKey<TKey> : IThirdPartyEmployeeKey<TKey>, IEmployeeContactKey<TKey>
        where TKey : IThirdPartyEmployeeContactId
    { }

    public interface IThirdPartyEmployeeContact<TKey, TContactType> : IEmployeeContact<TKey, TContactType>, IThirdPartyEmployeeContactKey<TKey>
        where TContactType : ContactTypeKey
        where TKey : IThirdPartyEmployeeContactId { }
    public interface IThirdPartyEmployeeContact<TKey, TPartyType, TContactType, TTitleType> : IThirdPartyEmployeeContact<TKey, TContactType>, IThirdPartyEmployeeBase<TKey, TPartyType, TTitleType>,
        IEmployeeContact<TKey, TPartyType, TContactType, TTitleType>
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey
        where TTitleType : TitleTypeKey
        where TKey : IThirdPartyEmployeeContactId
    { }
    public interface IThirdPartyEmployeeContactWithStatus<TKey, TContactType, TStatusType> : IEmployeeContactWithStatus<TKey, TContactType, TStatusType>,
        IThirdPartyEmployeeContact<TKey, TContactType>
        where TKey : IThirdPartyEmployeeContactId
        where TStatusType : StatusTypeKey
        where TContactType : ContactTypeKey { }
    public interface IThirdPartyEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType> : IThirdPartyEmployeeContact<TKey, TPartyType, TContactType, TTitleType>,
        IThirdPartyEmployeeBase<TKey, TPartyType, TTitleType, TStatusType>, IEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType>, 
        IThirdPartyEmployeeContactWithStatus<TKey, TContactType, TStatusType>
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey
        where TTitleType : TitleTypeKey
        where TStatusType : StatusTypeKey
        where TKey : IThirdPartyEmployeeContactId
    { }
    public interface IThirdPartyEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType, TRole, TOrganization> : IThirdPartyEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType>,
        IThirdPartyEmployeeBase<TKey, TPartyType, TTitleType, TStatusType, TOrganization>,
        IEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType, TRole, TOrganization>
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey
        where TTitleType : TitleTypeKey
        where TStatusType : StatusTypeKey
        where TRole : RoleKey
        where TOrganization : IThirdPartyKey
        where TKey : IThirdPartyEmployeeContactId
    { }
    public interface IThirdPartyEmployeeContactForAddBase<TKey> : IThirdPartyEmployeeContact<TKey, PartyTypeKey, ContactTypeKey, TitleTypeKey, StatusTypeKey>,
        IEmployeeContactForAddBase<TKey>
        where TKey : IThirdPartyEmployeeContactId
    { }
    public interface IThirdPartyEmployeeContactForAdd<TKey> : IThirdPartyEmployeeContactForAddBase<TKey>, IEmployeeContactForAdd<TKey>
        where TKey : IThirdPartyEmployeeContactId
    { }
    public interface IThirdPartyEmployeeContactForAddRelationship<TKey> : IThirdPartyEmployeeContactWithStatus<TKey, ContactTypeKey, StatusTypeKey>,
        IEmployeeContactForAddRelationship<TKey>
        where TKey : IThirdPartyEmployeeContactId { }
    public interface IThirdPartyEmployeeContactForUpdateBase<TKey> : IThirdPartyEmployeeContact<TKey, PartyTypeKey, ContactTypeKey, TitleTypeKey>,
        IEmployeeContactForUpdateBase<TKey>
        where TKey : IThirdPartyEmployeeContactId
    { }
    public interface IThirdPartyEmployeeContactForUpdateRelationship<TKey> : IThirdPartyEmployeeContact<TKey, ContactTypeKey>, 
        IEmployeeContactForUpdateRelationship<TKey>
        where TKey : IThirdPartyEmployeeContactId { }
    public interface IThirdPartyEmployeeContactForUpdate<TKey> : IThirdPartyEmployeeContactForUpdateBase<TKey>, IEmployeeContactForUpdate<TKey>
        where TKey : IThirdPartyEmployeeContactId
    { }
    public interface IThirdPartyEmployeeContactResultBase<TKey> : IEmployeeContactResultBase<TKey, ThirdPartyResult>, 
        IThirdPartyEmployeeContact<TKey, PartyTypeResult, ContactType, TitleTypeName, StatusTypeResult, RoleResult, ThirdPartyResult>
        where TKey : IThirdPartyEmployeeContactId
    { }
    public interface IThirdPartyEmployeeContactResult<TKey> : IThirdPartyEmployeeContactResultBase<TKey>,
        IEmployeeContactResult<TKey, ThirdPartyResult>
        where TKey : IThirdPartyEmployeeContactId
    { }
}
