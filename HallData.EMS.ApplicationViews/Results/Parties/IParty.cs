using System;
using HallData.ApplicationViews;

namespace HallData.EMS.ApplicationViews.Results
{
	/// <summary>
	/// Interface for Party Id type 
	/// </summary>
	public interface IPartyId
	{
		Guid PartyGuid { get; set; }
	}

	/// <summary>
	/// Interface for Party Key with Key type
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	public interface IPartyKey<TKey> : IHasKey<TKey>, IPartyKey 
	{ }

	/// <summary>
	/// Interface for Party Key
	/// </summary>
	public interface IPartyKey : IHasInstanceGuid
	{
		/// <summary>
		/// Party Guid
		/// </summary>
		Guid? PartyGuid { get; set; }
	}

	/// <summary>
	/// Interface for Party with restriction by Party type
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	/// <typeparam name="TPartyType">Party type</typeparam>
	public interface IParty<TKey, TPartyType> : IPartyKey<TKey>
		where TPartyType : PartyTypeKey
	{
		/// <summary>
		/// Party type
		/// </summary>
		TPartyType PartyType { get; set; }
	}

	/// <summary>
	/// Interface for Party with restriction by Party and Status type
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public interface IParty<TKey, TPartyType, TStatusType> : IParty<TKey, TPartyType>
		where TStatusType : StatusTypeKey
		where TPartyType : PartyTypeKey
	{
		/// <summary>
		/// Party status type
		/// </summary>
		TStatusType Status { get; set; }
	}

	public interface IPartyWithPrimaryContactMechanisms<TMechanismType, TPrimaryAddress, TPrimaryEmail, TPrimaryPhone> : IPartyKey
		where TMechanismType : MechanismTypeKey
		where TPrimaryAddress : IPrimaryPartyAddress<TMechanismType>
		where TPrimaryEmail : IPrimaryPartyEmail<TMechanismType>
		where TPrimaryPhone : IPrimaryPartyPhone<TMechanismType>
	{
		TPrimaryAddress PrimaryAddress { get; set; }
		TPrimaryPhone PrimaryPhone { get; set; }
		TPrimaryEmail PrimaryEmail { get; set; }
	}

	public interface IPartyWithPrimaryContactMechanismsForAdd :
		IPartyWithPrimaryContactMechanisms<MechanismTypeKey, PrimaryPartyAddressForAddUpdate, PrimaryPartyEmailForAddUpdate, PrimaryPartyPhoneForAddUpdate>
	{ }

	public interface IPartyWithPrimaryContactMechanismsForUpdate :
		IPartyWithPrimaryContactMechanisms<MechanismTypeKey, PrimaryPartyAddressForAddUpdate, PrimaryPartyEmailForAddUpdate, PrimaryPartyPhoneForAddUpdate>
	{ }

	public interface IPartyWithPrimaryContactMechanismsResult :
		IPartyWithPrimaryContactMechanisms<MechanismTypeResult, PrimaryPartyAddress, PrimaryPartyEmail, PrimaryPartyPhone>
	{ }

	public interface IPartyForAddBase<TKey> : IParty<TKey, PartyTypeKey, StatusTypeKey> 
	{ }

    public interface IPartyForAddBase : IPartyForAdd<Guid>
    { }

	/// <summary>
	/// Interface for Party for Add operation with Key type
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	public interface IPartyForAdd<TKey> : IPartyForAddBase<TKey>, IPartyWithPrimaryContactMechanismsForAdd 
	{ }

	/// <summary>
	/// Interface for Party for Add operation 
	/// </summary>
	public interface IPartyForAdd : IPartyForAdd<Guid>, IPartyForAddBase 
	{ }

	public interface IPartyForUpdateBase<TKey> : IParty<TKey, PartyTypeKey> 
	{ }

	public interface IPartyForUpdateBase : IPartyForUpdateBase<Guid> 
	{ }

	/// <summary>
	/// Interface for Party for Update operation with Key type
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	public interface IPartyForUpdate<TKey> : IPartyForUpdateBase<TKey>, IPartyWithPrimaryContactMechanismsForUpdate
	{}

	/// <summary>
	/// Interface for Party for Update operation
	/// </summary>
	public interface IPartyForUpdate : IPartyForUpdate<Guid>, IPartyForUpdateBase 
	{ }

	public interface IPartyResultBase<TKey> : IParty<TKey, PartyTypeResult, StatusTypeResult> 
	{ }

	/// <summary>
	/// Interface for Party Result with Key type 
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	public interface IPartyResult<TKey> : IPartyResultBase<TKey>, IPartyWithPrimaryContactMechanismsResult
	{ }

	public interface IPartyResultBase : IPartyResultBase<Guid> 
	{ }

	/// <summary>
	/// Interface for Party Result  
	/// </summary>
	public interface IPartyResult : IPartyResult<Guid>, IPartyResultBase
	{ }
}