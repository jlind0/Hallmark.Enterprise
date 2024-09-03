
namespace HallData.EMS.ApplicationViews.Results
{
    public interface ISupplierEmployeeContactId : IEmployeeContactId, ISupplierEmployeeId { }
    public interface ISupplierEmployeeContactKey : ISupplierEmployeeKey, IEmployeeContactKey { }
    public interface ISupplierEmployeeContactKey<TKey> : ISupplierEmployeeKey<TKey>, IEmployeeContactKey<TKey>
        where TKey : ISupplierEmployeeContactId
    { }

    public interface ISupplierEmployeeContact<TKey, TContactType> : IEmployeeContact<TKey, TContactType>, ISupplierEmployeeContactKey<TKey>
        where TContactType : ContactTypeKey
        where TKey : ISupplierEmployeeContactId { }
    public interface ISupplierEmployeeContact<TKey, TPartyType, TContactType, TTitleType> : ISupplierEmployeeContact<TKey, TContactType>, ISupplierEmployeeBase<TKey, TPartyType, TTitleType>,
        IEmployeeContact<TKey, TPartyType, TContactType, TTitleType>
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey
        where TTitleType : TitleTypeKey
        where TKey : ISupplierEmployeeContactId
    { }
    public interface ISupplierEmployeeContactWithStatus<TKey, TContactType, TStatusType> : IEmployeeContactWithStatus<TKey, TContactType, TStatusType>,
        ISupplierEmployeeContact<TKey, TContactType>
        where TKey : ISupplierEmployeeContactId
        where TStatusType : StatusTypeKey
        where TContactType : ContactTypeKey { }
    public interface ISupplierEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType> : ISupplierEmployeeContact<TKey, TPartyType, TContactType, TTitleType>,
        ISupplierEmployeeBase<TKey, TPartyType, TTitleType, TStatusType>, IEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType>,
        ISupplierEmployeeContactWithStatus<TKey, TContactType, TStatusType>
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey
        where TTitleType : TitleTypeKey
        where TStatusType : StatusTypeKey
        where TKey : ISupplierEmployeeContactId
    { }
    public interface ISupplierEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType, TRole, TOrganization> : ISupplierEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType>,
        ISupplierEmployeeBase<TKey, TPartyType, TTitleType, TStatusType, TOrganization>,
        IEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType, TRole, TOrganization>
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey
        where TTitleType : TitleTypeKey
        where TStatusType : StatusTypeKey
        where TRole : RoleKey
        where TOrganization : ISupplierKey
        where TKey : ISupplierEmployeeContactId
    { }
    public interface ISupplierEmployeeContactForAddBase<TKey> : ISupplierEmployeeContact<TKey, PartyTypeKey, ContactTypeKey, TitleTypeKey, StatusTypeKey>,
        IEmployeeContactForAddBase<TKey>
        where TKey : ISupplierEmployeeContactId
    { }
    public interface ISupplierEmployeeContactForAdd<TKey> : ISupplierEmployeeContactForAddBase<TKey>, IEmployeeContactForAdd<TKey>
        where TKey : ISupplierEmployeeContactId
    { }
    public interface ISupplierEmployeeContactForAddRelationship<TKey> : ISupplierEmployeeContactWithStatus<TKey, ContactTypeKey, StatusTypeKey>,
        IEmployeeContactForAddRelationship<TKey>
        where TKey : ISupplierEmployeeContactId { }
    public interface ISupplierEmployeeContactForUpdateBase<TKey> : ISupplierEmployeeContact<TKey, PartyTypeKey, ContactTypeKey, TitleTypeKey>,
        IEmployeeContactForUpdateBase<TKey>
        where TKey : ISupplierEmployeeContactId
    { }
    public interface ISupplierEmployeeContactForUpdateRelationship<TKey> : ISupplierEmployeeContact<TKey, ContactTypeKey>,
        IEmployeeContactForUpdateRelationship<TKey>
        where TKey : ISupplierEmployeeContactId { }
    public interface ISupplierEmployeeContactForUpdate<TKey> : ISupplierEmployeeContactForUpdateBase<TKey>, IEmployeeContactForUpdate<TKey>
        where TKey : ISupplierEmployeeContactId
    { }
    public interface ISupplierEmployeeContactResultBase<TKey> : IEmployeeContactResultBase<TKey, SupplierResult>,
        ISupplierEmployeeContact<TKey, PartyTypeResult, ContactType, TitleTypeName, StatusTypeResult, RoleResult, SupplierResult>
        where TKey : ISupplierEmployeeContactId
    { }
    public interface ISupplierEmployeeContactResult<TKey> : ISupplierEmployeeContactResultBase<TKey>,
        IEmployeeContactResult<TKey, SupplierResult>
        where TKey : ISupplierEmployeeContactId
    { }
}
