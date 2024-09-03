using System;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews.Results
{
	/// <summary>
	/// Business Unit Key class
	/// </summary>
	public class BusinessUnitKey : OrganizationKey, IBusinessUnitKey { }
	/// <summary>
	/// Business Unit Key class with restriction by Tier and Party type 
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	public class BusinessUnit<TPartyType, TTierType> : OrganizationGuid<TPartyType, TTierType>, IBusinessUnit<TPartyType, TTierType>
		where TPartyType: PartyTypeKey
		where TTierType: TierTypeKey
	{

	}
	public class BusinessUnit<TPartyType, TTierType, TParent> : BusinessUnit<TPartyType, TTierType>, IBusinessUnit<TPartyType, TTierType, TParent>
		where TPartyType: PartyTypeKey
		where TTierType: TierTypeKey
		where TParent: IOrganizationKey
	{
		/// <summary>
		/// Organization Parent 
		/// </summary>
		/// <remarks>
		/// Organization this Business Unit belongs to
		/// </remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		public virtual TParent Parent { get; set; }
	}
	/// <summary>
	/// Business Unit Base class with restriction by Tier, Party, Status and Organization type 
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	/// <typeparam name="TOrganization">Organization type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public class BusinessUnit<TPartyType, TTierType, TParent, TStatusType> : BusinessUnit<TPartyType, TTierType, TParent>, IBusinessUnit<TPartyType, TTierType, TParent, TStatusType>
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
		where TParent : IOrganizationKey
		where TStatusType: StatusTypeKey
	{
		/// <summary>
		/// Status type for the Business Unit 
		/// </summary>
		/// <remarks>
		/// A Business Unit must be set to "Created" status when is is first saved, set to "Enabled" when it is ready to be used and set to "Disabled" when deactivating it.
		/// </remarks>
		[ChildView]
		[AddOperationParameter]
		public virtual TStatusType Status { get; set; }
	}
	public class BusinessUnit<TPartyType, TTierType, TParent, TStatusType, TParentBusinessUnit, TRoot> : BusinessUnit<TPartyType, TTierType, TParent, TStatusType>, 
		IBusinessUnit<TPartyType, TTierType, TParent, TStatusType, TParentBusinessUnit, TRoot>
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
		where TStatusType : StatusTypeKey
		where TParent : IOrganizationKey
		where TParentBusinessUnit : IBusinessUnitKey
		where TRoot : IOrganizationKey
	{
		[ChildView]
		public virtual TParentBusinessUnit ParentBusinessUnit { get; set; }
		[ChildView]
		public virtual TRoot Root { get; set; }
	}
	/// <summary>
	/// Business Unit for Add operation 
	/// </summary>
	public class BusinessUnitForAddBase : BusinessUnit<PartyTypeKey, TierTypeKey, OrganizationKey, StatusTypeKey>, IBusinessUnitForAdd
	{
		/// <summary>
		/// Overridden to return Business Unit Party Type Key
		/// </summary>
		/// <remarks>Is Not Serialized</remarks>
		[JsonIgnore]
		public override PartyTypeKey PartyType
		{
			get
			{
				return new PartyTypeKey() { PartyTypeId = (int)Enums.PartyType.Organization };
			}
			set
			{
			}
		}
		[ChildView]
		[AddOperationParameter]
		public PrimaryPartyAddressForAddUpdate PrimaryAddress { get; set; }
		[ChildView]
		[AddOperationParameter]
		public PrimaryPartyPhoneForAddUpdate PrimaryPhone { get; set; }
		[ChildView]
		[AddOperationParameter]
		public PrimaryPartyEmailForAddUpdate PrimaryEmail { get; set; }
	}
	public class BusinessUnitForAdd : BusinessUnitForAddBase
	{
		/// <summary>
		/// Overridden to return Business Unit Party Guid
		/// </summary>
		/// <remarks>Is Not Serialized</remarks>
		[JsonIgnore]
		public override Guid? PartyGuid
		{
			get
			{
				return base.PartyGuid;
			}
			set
			{
				base.PartyGuid = value;
			}
		}
	}
	/// <summary>
	/// Business Unit for Update operation 
	/// </summary>
	public class BusinessUnitForUpdate : BusinessUnit<PartyTypeKey, TierTypeKey, OrganizationKey>, IBusinessUnitForUpdate
	{
		/// <summary>
		/// Overridden to return Business Unit Party Type Key
		/// </summary>
		/// <remarks>Is Not Serialized</remarks>
		[JsonIgnore]
		public override PartyTypeKey PartyType
		{
			get
			{
				return new PartyTypeKey() { PartyTypeId = (int)Enums.PartyType.Organization };
			}
			set
			{
			}
		}
		[ChildView]
		[UpdateOperationParameter]
		public PrimaryPartyAddressForAddUpdate PrimaryAddress { get; set; }
		[ChildView]
		[UpdateOperationParameter]
		public PrimaryPartyPhoneForAddUpdate PrimaryPhone { get; set; }
		[ChildView]
		[UpdateOperationParameter]
		public PrimaryPartyEmailForAddUpdate PrimaryEmail { get; set; }
		[GlobalizedRequired("PARTY_GUID_REQUIRED_FORUPDATE")]
		public override Guid? PartyGuid
		{
			get
			{
				return base.PartyGuid;
			}
			set
			{
				base.PartyGuid = value;
			}
		}
	}

	/// <summary>
	/// Business Unit Result 
	/// </summary>
	public class BusinessUnitResult : BusinessUnit<PartyTypeResult, TierType, OrganizationResult, StatusTypeResult, BusinessUnitResult, OrganizationResult>, IBusinessUnitResult
	{
		public PrimaryPartyAddress PrimaryAddress { get; set; }

		public PrimaryPartyPhone PrimaryPhone { get; set; }

		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
