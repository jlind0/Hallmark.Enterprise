using System;

namespace HallData.EMS.ApplicationViews.Results
{
	/// <summary>
	/// Interface for Business Unit Key
	/// </summary>
	public interface IBusinessUnitKey : IOrganizationKey<Guid> 
	{ }

	/// <summary>
	/// Interface for Business Unit with restriction by Tier and Party type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	public interface IBusinessUnit<TPartyType, TTierType> : IOrganization<Guid, TPartyType, TTierType>, IBusinessUnitKey
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
	{ }

	public interface IBusinessUnit<TPartyType, TTierType, TParent> : IBusinessUnit<TPartyType, TTierType>
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
		where TParent : IOrganizationKey
	{
		/// <summary>
		/// Organization Parent 
		/// </summary>
		/// <remarks>
		/// Organization this Business Unit belongs to
		/// </remarks>
		TParent Parent { get; set; }
	}

	/// <summary>
	/// Interface for Business Unit with restriction by Tier, Party, Status and Organization type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	/// <typeparam name="TOrganization">Organization type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public interface IBusinessUnit<TPartyType, TTierType, TParent, TStatusType> : IBusinessUnit<TPartyType, TTierType, TParent>, IOrganization<Guid, TPartyType, TTierType, TStatusType>
		where TPartyType: PartyTypeKey
		where TTierType: TierTypeKey
		where TStatusType : StatusTypeKey
		where TParent : IOrganizationKey
	{ }

	public interface IBusinessUnit<TPartyType, TTierType, TParent, TStatusType, TParentBusinessUnit, TRoot> : IBusinessUnit<TPartyType, TTierType, TParent, TStatusType>
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
		where TStatusType : StatusTypeKey
		where TParent : IOrganizationKey
		where TParentBusinessUnit: IBusinessUnitKey
		where TRoot: IOrganizationKey
	{
		TParentBusinessUnit ParentBusinessUnit { get; set; }
		TRoot Root { get; set; }
	}

	public interface IBusinessUnitForAddBase : IBusinessUnit<PartyTypeKey, TierTypeKey, OrganizationKey, StatusTypeKey>, IOrganizationForAddBase 
	{ }

	/// <summary>
	/// Interface for Business Unit for Add operation
	/// </summary>
	public interface IBusinessUnitForAdd : IBusinessUnitForAddBase, IOrganizationForAdd 
	{ }

	public interface IBusinessUnitForUpdateBase : IBusinessUnit<PartyTypeKey, TierTypeKey, OrganizationKey>, IOrganizationForUpdateBase 
	{ }

	/// <summary>
	/// Interface for Business Unit for Update operation
	/// </summary>
	public interface IBusinessUnitForUpdate : IBusinessUnitForUpdateBase, IOrganizationForUpdate 
	{ }

	public interface IBusinessUnitResultBase : IBusinessUnit<PartyTypeResult, TierType, OrganizationResult, StatusTypeResult, BusinessUnitResult, OrganizationResult>, IOrganizationResultBase 
	{ }

	/// <summary>
	/// Interface for Business Unit Result
	/// </summary>
	public interface IBusinessUnitResult : IBusinessUnitResultBase, IOrganizationResult 
    { }
}
