
namespace HallData.EMS.ApplicationViews.Results
{
	public interface IContactId : IPartyId
	{
		string ContactRoleName { get; set; }
	}
	public interface IContactId<TId> : IContactId
		where TId: IPartyId
	{
		TId Id { get; set; }
	}
	public interface IContactKey : IPersonKey
	{
		string ContactRoleName { get; set; }
	}
	public interface IContactKey<TKey> : IPersonKey<TKey>, IContactKey
		where TKey : IContactId { }

	public interface IContact<TKey, TContactType> : IContactKey<TKey>
		where TKey: IContactId
		where TContactType: ContactTypeKey
	{
		TContactType ContactType { get; set; }
		string JobTitle { get; set; }
	}

	public interface IContact<TKey, TPartyType, TContactType> : IContact<TKey, TContactType>, IPerson<TKey, TPartyType>
		where TKey : IContactId
		where TPartyType : PartyTypeKey
		where TContactType : ContactTypeKey
	{
		
	}

	public interface IContactWithStatus<TKey, TContactType, TStatusType> : IContact<TKey, TContactType>
		where TKey: IContactId
		where TContactType: ContactTypeKey
		where TStatusType: StatusTypeKey
	{
		TStatusType ContactRelationshipStatus { get; set; }
	}

	public interface IContact<TKey, TPartyType, TContactType, TStatusType> : IContact<TKey, TPartyType, TContactType>, IContactWithStatus<TKey, TContactType, TStatusType>, IPerson<TKey, TPartyType, TStatusType>
		where TKey: IContactId
		where TPartyType : PartyTypeKey
		where TStatusType: StatusTypeKey
		where TContactType: ContactTypeKey
	{
		
	}

	public interface IContact<TKey, TPartyType, TContactType, TStatusType, TRole> : IContact<TKey, TPartyType, TContactType, TStatusType>
		where TKey: IContactId
		where TPartyType : PartyTypeKey
		where TStatusType: StatusTypeKey
		where TContactType: ContactTypeKey
		where TRole: RoleKey
	{
		TRole ContactRole { get; set; }
	}

	public interface IContactForAddRelationship<TKey> : IContactWithStatus<TKey, ContactTypeKey, StatusTypeKey> 
		where TKey: IContactId
	{ }

	public interface IContactForAddBase<TKey> : IContact<TKey, PartyTypeKey, ContactTypeKey, StatusTypeKey>, IPersonForAddBase<TKey>
		where TKey : IContactId { }

	public interface IContactForAdd<TKey> :  IContactForAddBase<TKey>, IPersonForAdd<TKey>
		where TKey : IContactId { }

	public interface IContactForUpdateBase<TKey> : IContact<TKey, PartyTypeKey, ContactTypeKey>, IPersonForUpdateBase<TKey>
		where TKey : IContactId { }

	public interface IContactForUpdateRelationship<TKey> : IContact<TKey, ContactTypeKey> 
		where TKey: IContactId
	{
		string OriginalContactRoleName { get; set; }
	}

	public interface IContactForUpdate<TKey> : IContactForUpdateBase<TKey>, IPersonForUpdate<TKey> 
		where TKey: IContactId
	{ }

	public interface IContactResultBase<TKey> : IContact<TKey, PartyTypeResult, ContactType, StatusTypeResult, RoleResult>, IPersonResultBase<TKey>
		where TKey : IContactId { }
	public interface IContactResult<TKey> : IContactResultBase<TKey>, IPersonResult<TKey> 
		where TKey: IContactId
	{ }
}
