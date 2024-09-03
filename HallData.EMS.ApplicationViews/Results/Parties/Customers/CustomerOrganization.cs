using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews.Results
{
	/// <summary>
	/// Customer Organization Key class
	/// </summary>
	public class CustomerOrganizationKey : OrganizationKey<CustomerId>, ICustomerOrganizationKey
	{
		/// <summary>
		/// Gets and Sets Customer Of Party Guid
		/// </summary>
		/// <remarks>
		/// Guid of the Employer.
		/// </remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public Guid? CustomerOfPartyGuid { get; set; }
		/// <summary>
		/// Overridden to return Customer Id
		/// </summary>
		public override CustomerId Key
		{
			get
			{
				return new CustomerId(this.PartyGuid ?? Guid.Empty, this.CustomerOfPartyGuid);
			}
			set
			{
				this.PartyGuid = value.PartyGuid;
				this.CustomerOfPartyGuid = value.CustomerOfPartyGuid;
			}
		}
	}

	/// <summary>
	/// Customer Organization Base class with restriction by Party and Tier type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	public class CustomerOrganization<TPartyType, TTierType> : Organization<CustomerId, TPartyType, TTierType>, ICustomerOrganization<TPartyType, TTierType>
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
	{
		/// <summary>
		/// Gets and Sets Customer Of Party Guid
		/// </summary>
		/// <remarks>
		/// Guid of the Employer.
		/// </remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public Guid? CustomerOfPartyGuid { get; set; }
		/// <summary>
		/// Overridden to return Customer Id
		/// </summary>
		public override CustomerId Key
		{
			get
			{
				return new CustomerId(this.PartyGuid ?? Guid.Empty, this.CustomerOfPartyGuid);
			}
			set
			{
				this.PartyGuid = value.PartyGuid;
				this.CustomerOfPartyGuid = value.CustomerOfPartyGuid;
			}
		}
	}

	/// <summary>
	/// Customer Organization Base class with restriction by Party, Tier and Status type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public class CustomerOrganization<TPartyType, TTierType, TStatusType> : CustomerOrganization<TPartyType, TTierType>, ICustomerOrganization<TPartyType, TTierType, TStatusType>
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
		where TStatusType: StatusTypeKey
	{
		/// <summary>
		/// Gets and Sets Status type
		/// </summary>
		[AddOperationParameter]
		[ChildView]
		public TStatusType Status { get; set; }
		/// <summary>
		/// Gets and Sets Customer Relationship Status
		/// </summary>
		[AddOperationParameter]
		[ChildView]
		public TStatusType CustomerRelationshipStatus { get; set; }
	}

	/// <summary>
	/// Customer Organization Base class with restriction by Party, Tier, Status and Organization type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	/// <typeparam name="TOrganization">Organization</typeparam>
	public class CustomerOrganization<TPartyType, TTierType, TStatusType, TOrganization> : CustomerOrganization<TPartyType, TTierType, TStatusType>, ICustomerOrganization<TPartyType, TTierType, TStatusType, TOrganization>
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
		where TStatusType: StatusTypeKey
		where TOrganization: IOrganizationKey
	{
		/// <summary>
		/// Gets and Sets Customer Of Organization
		/// </summary>
		[ChildView]
		public TOrganization CustomerOf { get; set; }
	}

	/// <summary>
	/// Customer Organization for Add operation 
	/// </summary>
	public class CustomerOrganizationForAddBase : CustomerOrganization<PartyTypeKey, TierTypeKey, StatusTypeKey>, ICustomerOrganizationForAdd
	{

		/// <summary>
		/// Overridden to return a Customer Organization Party Type Key
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

	public class CustomerOrganizationForAdd : CustomerOrganizationForAddBase
	{
		/// <summary>
		/// Overridden to return a Customer Organization Party Guid
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
	/// Customer Organization for Update operation 
	/// </summary>
	public class CustomerOrganizationForUpdate : CustomerOrganization<PartyTypeKey, TierTypeKey>, ICustomerOrganizationForUpdate
	{
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
		/// <summary>
		/// Overridden to return a Customer Organization Party Type Key
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
	}

	/// <summary>
	/// Customer Organization Result
	/// </summary>
	public class CustomerOrganizationResult : CustomerOrganization<PartyTypeResult, TierType, StatusTypeResult, OrganizationResult>, ICustomerOrganizationResult
	{
		public PrimaryPartyAddress PrimaryAddress { get; set; }

		public PrimaryPartyPhone PrimaryPhone { get; set; }

		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
