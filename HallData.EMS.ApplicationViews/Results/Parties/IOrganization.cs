using System;
namespace HallData.EMS.ApplicationViews.Results
{
	/// <summary>
	/// Interface for Organization Key with Key type 
	/// </summary>
	/// <typeparam name="TKey">Key Type</typeparam>
	public interface IOrganizationKey<TKey> : IOrganizationKey, IPartyKey<TKey>
	{ }

	/// <summary>
	/// Interface for Organization Key 
	/// </summary>
	public interface IOrganizationKey : IPartyKey
	{ }

	/// <summary>
	/// Interface for Organization with restriction by Tier and Party type
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	public interface IOrganization<TKey, TPartyType, TTierType> : IParty<TKey, TPartyType>, IOrganizationKey<TKey>
		where TTierType : TierTypeKey
		where TPartyType : PartyTypeKey
	{
		/// <summary>
		/// Name of the organization.
		/// </summary>
		/// <remarks>
		/// Official name of the organization that will be displayed on customer's web sites or publications.
		/// </remarks>
		string Name { get; set; }

		/// <summary>
		/// EIN - enterprise identification number.
		/// </summary>
		/// <remarks>
		/// EIN is required for billing purposes for Hallmark's Customers, that Hallmark is billing 
		/// </remarks>
		string Ein { get; set; }

		/// <summary>
		/// Website
		/// </summary>
		/// <remarks>
		/// Website of the organization.
		/// </remarks>
		string Website { get; set; }

		/// <summary>
		/// Code
		/// </summary>
		/// <remarks>
		/// The code will be used as a readable id. It will be used by accounting to do invoicing.
		/// </remarks>
		string Code { get; set; }

		string ArName { get; set; }

		/// <summary>
		/// Tier represents the total number of fulfillment records.
		/// </summary>
		/// <remarks>
		/// The purpose of tier is to assign organization to proper support tier at the time of setup. The tier allows Hallmark to identify organization by size. 
		/// </remarks>
		TTierType Tier { get; set; }
	}

	/// <summary>
	/// Interface for Organization with restriction by Tier, Party and Status type
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public interface IOrganization<TKey, TPartyType, TTierType, TStatusType> : IOrganization<TKey, TPartyType, TTierType>, IParty<TKey, TPartyType, TStatusType>
		where TTierType : TierTypeKey
		where TPartyType : PartyTypeKey
		where TStatusType : StatusTypeKey
	{ }

	public interface IOrganizationForAddBase<TKey> : IOrganization<TKey, PartyTypeKey, TierTypeKey, StatusTypeKey>, IPartyForAddBase<TKey>
	{ }

	public interface IOrganizationForAddBase : IOrganizationForAddBase<Guid>, IPartyForAddBase
	{ }

	/// <summary>
	/// Interface for Organization for Add operation with Key type
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	public interface IOrganizationForAdd<TKey> : IOrganizationForAddBase<TKey>, IPartyForAdd<TKey>
	{ }

	/// <summary>
	/// Interface for Organization for Add operation 
	/// </summary>
	public interface IOrganizationForAdd : IOrganizationForAdd<Guid>, IPartyForAdd, IOrganizationForAddBase
	{ }

	public interface IOrganizationForUpdateBase<TKey> : IOrganization<TKey, PartyTypeKey, TierTypeKey>, IPartyForUpdateBase<TKey>
	{ }

	public interface IOrganizationForUpdateBase : IOrganizationForUpdateBase<Guid>, IPartyForUpdateBase
	{ }

	/// <summary>
	/// Interface for Organization for Update operation with Key type
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	public interface IOrganizationForUpdate<TKey> : IOrganizationForUpdateBase<TKey>, IPartyForUpdate<TKey>
	{ }

	/// <summary>
	/// Interface for Organization for Update operation 
	/// </summary>
	public interface IOrganizationForUpdate : IOrganizationForUpdate<Guid>, IPartyForUpdate, IOrganizationForUpdateBase
	{ }

	public interface IOrganizationResultBase<TKey> : IOrganization<TKey, PartyTypeResult, TierType, StatusTypeResult>, IPartyResultBase<TKey> 
	{ }

	public interface IOrganizationResultBase : IOrganizationResultBase<Guid>, IPartyResultBase 
	{ }

	/// <summary>
	/// Interface for Organization Result with Key type 
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	public interface IOrganizationResult<TKey> : IOrganizationResultBase<TKey>, IPartyResult<TKey> 
	{ }

	/// <summary>
	/// Interface for Organization Result  
	/// </summary>
	public interface IOrganizationResult : IOrganizationResult<Guid>, IPartyResult, IOrganizationResultBase 
	{ }
}