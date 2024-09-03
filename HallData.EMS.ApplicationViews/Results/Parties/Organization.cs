using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using HallData.EMS.ApplicationViews.Enums;

namespace HallData.EMS.ApplicationViews.Results
{
   /// <summary>
   /// Organization Key abstract class
   /// </summary>
   /// <typeparam name="TKey">Key type</typeparam>
	public abstract class OrganizationKey<TKey> : PartyKey<TKey>, IOrganizationKey<TKey> { }
	/// <summary>
	/// Organization Key class
	/// </summary>
	public class OrganizationKey : OrganizationKey<Guid>
	{
		/// <summary>
		/// Overridden to return an Organization Party Guid
		/// </summary>
		public override Guid Key
		{
			get
			{
				return this.PartyGuid ?? Guid.Empty;
			}
			set
			{
				this.PartyGuid = value;
			}
		}
	}
	/// <summary>
	/// Organization base abstract class with restriction by Tier and Party type 
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	public abstract class Organization<TKey, TPartyType, TTierType> : Party<TKey, TPartyType>, IOrganization<TKey, TPartyType, TTierType>
		where TTierType : TierTypeKey
		where TPartyType: PartyTypeKey
	{
		/// <summary>
		/// Name of the organization.
		/// </summary>
		/// <remarks>
		/// Official name of the organization that will be displayed on customer's web sites or publications.
		/// </remarks>
		[UpdateOperationParameter]
		[AddOperationParameter]
		public virtual string Name { get; set; }
		/// <summary>
		/// EIN - enterprise identification number.
		/// </summary>
		/// <remarks>
		/// EIN is required for billing purposes for Hallmark's Customers, that Hallmark is billing 
		/// </remarks>
		[UpdateOperationParameter]
		[AddOperationParameter]
		public virtual string Ein { get; set; }
		/// <summary>
		/// Website
		/// </summary>
		/// <remarks>
		/// Website of the organization.
		/// </remarks>
		[UpdateOperationParameter]
		[AddOperationParameter]
		[GlobalizedDataType(DataType.Url, "ORGANIZATION_WEBSITE_URL_VALID")]
		public virtual string Website { get; set; }
		/// <summary>
		/// Code
		/// </summary>
		/// <remarks>
		/// The code will be used as a readable id. It will be used by accounting to do invoicing.
		/// </remarks>
		[UpdateOperationParameter]
		[AddOperationParameter]
		public virtual string Code { get; set; }
		/// <summary>
		/// Tier represents the total number of fulfillment records.
		/// </summary>
		/// <remarks>
		/// The purpose of tier is to assign organization to proper support tier at the time of setup. The tier allows Hallmark to identify organization by size. 
		/// </remarks>
		[ChildView]
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual TTierType Tier { get; set; }
	   
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual string ArName { get; set; }
		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			foreach (var valid in base.Validate(validationContext))
				yield return valid;
			if (this.PartyGuid == null && string.IsNullOrWhiteSpace(this.Name))
				yield return ValidationResultFactory.Create(new ValidationResult("Organization Name Required if Party Guid not provided"), "ORGANIZATION_NAME_REQUIRED");
		}
	}

	/// <summary>
	/// Organization base abstract class with restriction by Tier, Party and Status type
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public abstract class Organization<TKey, TPartyType, TTierType, TStatusType> : Organization<TKey, TPartyType, TTierType>, IOrganization<TKey, TPartyType, TTierType, TStatusType>
		where TTierType : TierTypeKey
		where TPartyType : PartyTypeKey
		where TStatusType: StatusTypeKey
	{
		/// <summary>
		/// Status type for the organization
		/// </summary>
		/// <remarks>
		/// Organization status must be set to "Enabled" when it is ready to be used and set to "Disabled" when deactivating it.
		/// </remarks>
		[ChildView]
		[AddOperationParameter]
		public virtual TStatusType Status { get; set; }
	}

	/// <summary>
	/// Organization base Guid class with restriction by Tier and Party type 
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	public class OrganizationGuid<TPartyType, TTierType> : Organization<Guid, TPartyType, TTierType>
		where TTierType : TierTypeKey
		where TPartyType : PartyTypeKey
	{
		/// <summary>
		/// Overridden to return an Organization Party Guid
		/// </summary>
		public override Guid Key
		{
			get
			{
				return this.PartyGuid ?? Guid.Empty;
			}
			set
			{
				this.PartyGuid = value;
			}
		}
	}

	/// <summary>
	/// Organization base Guid class with restriction by Tier, Party and Status type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public class OrganizationGuid<TPartyType, TTierType, TStatusType> : Organization<Guid, TPartyType, TTierType, TStatusType>
		where TStatusType : StatusTypeKey
		where TTierType : TierTypeKey
		where TPartyType : PartyTypeKey
	{
		/// <summary>
		/// Overridden to return an Organization Party Guid
		/// </summary>
		public override Guid Key
		{
			get
			{
				return this.PartyGuid ?? Guid.Empty;
			}
			set
			{
				this.PartyGuid = value;
			}
		}
	}

	/// <summary>
	/// Organization for Add operation 
	/// </summary>
	public class OrganizationForAddBase : OrganizationGuid<PartyTypeKey, TierTypeKey, StatusTypeKey>, IOrganizationForAdd
	{

		/// <summary>
		/// Overridden to return an Organization Party Type Key
		/// </summary>
		/// <remarks>Is Not Serialized</remarks>
		[JsonIgnore]
		public override PartyTypeKey PartyType
		{
			get
			{
				return new PartyTypeKey() { PartyTypeId = (int)HallData.EMS.ApplicationViews.Enums.PartyType.Organization };
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
	public class OrganizationForAdd : OrganizationForAddBase
	{
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
	/// Organization for Update operation 
	/// </summary>
	public class OrganizationForUpdate : OrganizationGuid<PartyTypeKey, TierTypeKey>, IOrganizationForUpdate
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
		/// Overridden to return an Organization Party Type Key
		/// </summary>
		/// <remarks>Is Not Serialized</remarks>
		[JsonIgnore]
		public override PartyTypeKey PartyType
		{
			get
			{
				return new PartyTypeKey() { PartyTypeId = (int)HallData.EMS.ApplicationViews.Enums.PartyType.Organization };
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
	/// Organization Result 
	/// </summary>
	public class OrganizationResult : OrganizationGuid<PartyTypeResult, TierType, StatusTypeResult>, IOrganizationResult 
	{
		public PrimaryPartyAddress PrimaryAddress { get; set; }

		public PrimaryPartyPhone PrimaryPhone { get; set; }

		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
