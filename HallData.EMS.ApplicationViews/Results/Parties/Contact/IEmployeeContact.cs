
namespace HallData.EMS.ApplicationViews.Results
{
    public interface IEmployeeContactId : IEmployeeId, IContactId { }
    public interface IEmployeeContactKey : IEmployeeKey, IContactKey { }
    public interface IEmployeeContactKey<TKey> : IEmployeeKey<TKey>, IContactKey<TKey>, IEmployeeContactKey
        where TKey: IEmployeeContactId
    { }
    
    public interface IEmployeeContact<TKey, TContactType> : IContact<TKey, TContactType>, IEmployeeContactKey<TKey>
        where TContactType: ContactTypeKey
        where TKey : IEmployeeContactId { }
    public interface IEmployeeContact<TKey, TPartyType, TContactType, TTitleType> : IEmployeeContact<TKey, TContactType>, IEmployeeBase<TKey, TPartyType, TTitleType>,
        IContact<TKey, TPartyType, TContactType> 
        where TPartyType: PartyTypeKey
        where TContactType: ContactTypeKey
        where TTitleType : TitleTypeKey
        where TKey: IEmployeeContactId
    { }
    public interface IEmployeeContactWithStatus<TKey, TContactType, TStatusType> : IContactWithStatus<TKey, TContactType, TStatusType>, 
        IEmployeeContact<TKey, TContactType>
        where TKey: IEmployeeContactId
        where TStatusType: StatusTypeKey
        where TContactType : ContactTypeKey { }
    public interface IEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType> : IEmployeeContact<TKey, TPartyType, TContactType, TTitleType>,
        IEmployeeBase<TKey, TPartyType, TTitleType, TStatusType>, IContact<TKey, TPartyType, TContactType, TStatusType>, IEmployeeContactWithStatus<TKey, TContactType, TStatusType>
        where TPartyType: PartyTypeKey
        where TContactType: ContactTypeKey
        where TTitleType : TitleTypeKey
        where TStatusType : StatusTypeKey 
        where TKey: IEmployeeContactId
    { }
    public interface IEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType, TRole, TOrganization> : IEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType>,
        IEmployeeBase<TKey, TPartyType, TTitleType, TStatusType, TOrganization>, IContact<TKey, TPartyType, TContactType, TStatusType, TRole>
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey
        where TTitleType : TitleTypeKey
        where TStatusType : StatusTypeKey 
        where TRole: RoleKey
        where TOrganization : IOrganizationKey
        where TKey : IEmployeeContactId
    { }
    public interface IEmployeeContactForAddBase<TKey> : IEmployeeContact<TKey, PartyTypeKey, ContactTypeKey, TitleTypeKey, StatusTypeKey>, 
        IEmployeeForAddBase<TKey>, IContactForAddBase<TKey> 
        where TKey: IEmployeeContactId
    { }
    public interface IEmployeeContactForAdd<TKey> : IEmployeeContactForAddBase<TKey>, IEmployeeForAdd<TKey>, IContactForAdd<TKey> 
        where TKey: IEmployeeContactId
    { }
    public interface IEmployeeContactForAddRelationship<TKey> : IEmployeeContactWithStatus<TKey, ContactTypeKey, StatusTypeKey>, IContactForAddRelationship<TKey>
        where TKey : IEmployeeContactId { }
    public interface IEmployeeContactForUpdateBase<TKey> : IEmployeeContact<TKey, PartyTypeKey, ContactTypeKey, TitleTypeKey>,
        IEmployeeForUpdateBase<TKey>, IContactForUpdateBase<TKey> 
        where TKey : IEmployeeContactId
    { }
    public interface IEmployeeContactForUpdateRelationship<TKey> : IEmployeeContact<TKey, ContactTypeKey>, IContactForUpdateRelationship<TKey>, IEmployeeForUpdateRelationship<TKey>
        where TKey : IEmployeeContactId { }
    public interface IEmployeeContactForUpdate<TKey> : IEmployeeContactForUpdateBase<TKey>, IEmployeeForUpdate<TKey>, IContactForUpdate<TKey>
        where TKey : IEmployeeContactId
    { }
    public interface IEmployeeContactResultBase<TKey, TOrganization> : IEmployeeResultBase<TKey, TOrganization>, IContactResultBase<TKey>,
        IEmployeeContact<TKey, PartyTypeResult, ContactType, TitleTypeName, StatusTypeResult, RoleResult, TOrganization>
        where TKey: IEmployeeContactId
        where TOrganization: IOrganizationKey
    { }
    public interface IEmployeeContactResultBase<TKey> : IEmployeeContactResultBase<TKey, OrganizationResult>, IEmployeeResultBase<TKey> 
        where TKey: IEmployeeContactId
    { }
    public interface IEmployeeContactResult<TKey, TOrganization> : IEmployeeContactResultBase<TKey, TOrganization>, 
        IEmployeeResult<TKey, TOrganization>, IContactResult<TKey>
        where TKey : IEmployeeContactId
        where TOrganization : IOrganizationKey
    { }
    public interface IEmployeeContactResult<TKey> : IEmployeeContactResult<TKey, OrganizationResult>, IEmployeeContactResultBase<TKey>, IEmployeeResult<TKey>
        where TKey : IEmployeeContactId 
    { }

}
