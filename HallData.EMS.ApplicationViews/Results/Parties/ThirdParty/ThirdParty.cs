using System;
using System.Collections.Generic;
using HallData.ApplicationViews;
using HallData.Validation;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews.Results
{
	/// <summary>
	/// ThirdParty Key class
	/// </summary>
	public abstract class ThirdPartyKey<TKey> : OrganizationKey<TKey>, IThirdPartyKey<TKey>
		where TKey: IThirdPartyId
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? ThirdPartyOfGuid { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		[GlobalizedRequired("THIRDPARTY_ROLE_REQUIRED")]
		public virtual string ThirdPartyRoleName { get; set; }
	}
	public class ThirdPartyKey : ThirdPartyKey<ThirdPartyId>
	{
		/// <summary>
		/// Overridden to return ThirdParty Id
		/// </summary>
		/// <remarks>Is Not Serialized</remarks>
		public override ThirdPartyId Key
		{
			get
			{
				return new ThirdPartyId(this.PartyGuid ?? Guid.Empty, this.ThirdPartyOfGuid ?? Guid.Empty, this.ThirdPartyRoleName);
			}
			set
			{
				this.PartyGuid = value.PartyGuid;
				this.ThirdPartyOfGuid = value.ThirdPartyOfGuid;
				this.ThirdPartyRoleName = value.ThirdPartyRoleName;
			}
		}
	}
	public class ThirdPartyKeyRelationshipForOrganization : ThirdPartyKey
	{
		[GlobalizedRequired("THIRDPARTY_PARTYGUID_REQUIRED")]
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
	public class ThirdPartyKeyRelationshipForThirdParty : ThirdPartyKey
	{
		[GlobalizedRequired("THIRDPARTY_THIRDPARTYOF_REQUIRED")]
		public override Guid? ThirdPartyOfGuid
		{
			get
			{
				return base.ThirdPartyOfGuid;
			}
			set
			{
				base.ThirdPartyOfGuid = value;
			}
		}
	}
	public class ThirdPartyKeyRelationship : ThirdPartyKey
	{
		[GlobalizedRequired("THIRDPARTY_PARTYGUID_REQUIRED")]
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
		[GlobalizedRequired("THIRDPARTY_THIRDPARTYOF_REQUIRED")]
		public override Guid? ThirdPartyOfGuid
		{
			get
			{
				return base.ThirdPartyOfGuid;
			}
			set
			{
				base.ThirdPartyOfGuid = value;
			}
		}
	}
	/// <summary>
	/// ThirdParty Base class with restriction by Party type and Tier type
	/// </summary>
	/// <typeparam name="TKey">Key</typeparam>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	public abstract class ThirdPartyWithKey<TKey, TPartyType, TTierType> : Organization<TKey, TPartyType, TTierType>, IThirdPartyBase<TKey, TPartyType, TTierType>
		where TPartyType : PartyTypeKey
		where TTierType: TierTypeKey
		where TKey: IThirdPartyId
	{
		
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? ThirdPartyOfGuid { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		[GlobalizedRequired("THIRDPARTY_ROLE_REQUIRED")]
		public virtual string ThirdPartyRoleName { get; set; }
	}
	public class ThirdParty<TPartyType, TTierType> : ThirdPartyWithKey<ThirdPartyId, TPartyType, TTierType>, IThirdParty<TPartyType, TTierType>
		where TPartyType: PartyTypeKey
		where TTierType: TierTypeKey
	{
		/// <summary>
		/// Overridden to return ThirdParty Id
		/// </summary>
		/// <remarks>Is Not Serialized</remarks>
		public override ThirdPartyId Key
		{
			get
			{
				return new ThirdPartyId(this.PartyGuid ?? Guid.Empty, this.ThirdPartyOfGuid ?? Guid.Empty, this.ThirdPartyRoleName);
			}
			set
			{
				this.PartyGuid = value.PartyGuid;
				this.ThirdPartyOfGuid = value.ThirdPartyOfGuid;
				this.ThirdPartyRoleName = value.ThirdPartyRoleName;
			}
		}
	}
	/// <summary>
	/// ThirdParty Base class with restriction by Tier, Party and Status type 
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public class ThirdParty<TPartyType, TTierType, TStatusType> : ThirdParty<TPartyType, TTierType>, IThirdParty<TPartyType, TTierType, TStatusType>
		where TPartyType : PartyTypeKey
		where TStatusType : StatusTypeKey
		where TTierType: TierTypeKey
	{
		/// <summary>
		/// ThirdParty Relationship Status
		/// </summary>
		/// <remarks>Status of the ThirdParty relationship</remarks>
		[AddOperationParameter]
		[ChildView]
		public TStatusType ThirdPartyRelationshipStatus { get; set; }
		/// <summary>
		///ThirdParty status type
		/// </summary>
		[AddOperationParameter]
		[ChildView]
		public TStatusType Status { get; set; }
	}
	/// <summary>
	/// ThirdParty Base class with restriction by Title, Party, Status and Organization type 
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public class ThirdParty<TPartyType, TTierType, TStatusType, TThirdPartyOf, TRole> : ThirdParty<TPartyType, TTierType, TStatusType>, 
		IThirdParty<TPartyType, TTierType, TStatusType, TThirdPartyOf, TRole>
		where TStatusType : StatusTypeKey
		where TPartyType : PartyTypeKey
		where TThirdPartyOf : IOrganizationKey
		where TTierType: TierTypeKey
		where TRole: RoleKey
	{
		[ChildView]
		public TThirdPartyOf ThirdPartyOf { get; set; }

		[ChildView]
		public TRole ThirdPartyRole { get; set; }
	}
	public abstract class ThirdPartyForAddRelationship<TKey> : ThirdPartyKey<TKey>, IThirdPartyForAddRelationship<TKey>
		where TKey: IThirdPartyId
	{
		[AddOperationParameter]
		[ChildView]
		public StatusTypeKey ThirdPartyRelationshipStatus { get; set; }
	}
	public class ThirdPartyForAddRelationshipBase : ThirdPartyForAddRelationship<ThirdPartyId>, IThirdPartyForAddRelationship
	{
		public override ThirdPartyId Key
		{
			get
			{
				return new ThirdPartyId(this.PartyGuid ?? Guid.Empty, this.ThirdPartyOfGuid ?? Guid.Empty, this.ThirdPartyRoleName);
			}
			set
			{
				this.PartyGuid = value.PartyGuid;
				this.ThirdPartyOfGuid = value.ThirdPartyOfGuid;
				this.ThirdPartyRoleName = value.ThirdPartyRoleName;
			}
		}
	}
	public class ThirdPartyForAddRelationshipOrganization : ThirdPartyForAddRelationshipBase
	{
		[GlobalizedRequired("THIRDPARTY_PARTYGUID_REQUIRED")]
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
		[JsonIgnore]
		public override Guid? ThirdPartyOfGuid
		{
			get
			{
				return base.ThirdPartyOfGuid;
			}
			set
			{
				base.ThirdPartyOfGuid = value;
			}
		}
	}
	public class ThirdPartyForAddRelationshipThirdParty : ThirdPartyForAddRelationshipBase
	{
		[GlobalizedRequired("THIRDPARTY_THIRDPARTYOF_REQUIRED")]
		public override Guid? ThirdPartyOfGuid
		{
			get
			{
				return base.ThirdPartyOfGuid;
			}
			set
			{
				base.ThirdPartyOfGuid = value;
			}
		}
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
	public class ThirdPartyForAddRelationship : ThirdPartyForAddRelationshipBase
	{
		[GlobalizedRequired("THIRDPARTY_PARTYGUID_REQUIRED")]
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
		[GlobalizedRequired("THIRDPARTY_THIRDPARTYOF_REQUIRED")]
		public override Guid? ThirdPartyOfGuid
		{
			get
			{
				return base.ThirdPartyOfGuid;
			}
			set
			{
				base.ThirdPartyOfGuid = value;
			}
		}
	}
	/// <summary>
	/// ThirdParty for Add operation 
	/// </summary>
	public class ThirdPartyForAddBase : ThirdParty<PartyTypeKey, TierTypeKey, StatusTypeKey>, IThirdPartyForAdd
	{
		[ChildView]
		[AddOperationParameter]
		public PrimaryPartyAddressForAddUpdate PrimaryAddress { get; set; }
		[ChildView]
		[AddOperationParameter]
		public PrimaryPartyPhoneForAddUpdate PrimaryPhone { get; set; }
		[ChildView]
		[AddOperationParameter]
		public PrimaryPartyEmailForAddUpdate PrimaryEmail { get; set; }
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
		[JsonIgnore]
		public override PartyTypeKey PartyType
		{
			get
			{
				return new PartyTypeKey() { PartyTypeId = (int)Enums.PartyType.Organization };
			}
			set { }
		}
	}
	public class ThirdPartyForAddOrganization : ThirdPartyForAddBase
	{
		[JsonIgnore]
		public override Guid? ThirdPartyOfGuid
		{
			get
			{
				return base.ThirdPartyOfGuid;
			}
			set
			{
				base.ThirdPartyOfGuid = value;
			}
		}
	}
	public class ThirdPartyForAdd : ThirdPartyForAddBase
	{
		[GlobalizedRequired("THIRDPARTY_THIRDPARTYOF_REQUIRED")]
		public override Guid? ThirdPartyOfGuid
		{
			get
			{
				return base.ThirdPartyOfGuid;
			}
			set
			{
				base.ThirdPartyOfGuid = value;
			}
		}
	}

	public abstract class ThirdPartyForUpdateRelationship<TKey> : ThirdPartyKey<TKey>, IThirdPartyForUpdateRelationship<TKey>, IValidatableObject
		where TKey: IThirdPartyId
	{

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (this.OriginalPartyGuid == null && this.OriginalThirdPartyOfGuid == null && this.OriginalThirdPartyRoleName == null)
				yield return ValidationResultFactory.Create(new ValidationResult("Original Party Guid, Third Party Of Guid or Third Party Role Id is required for relationship update"), "THIRDPARTY_RELATIONSHIP_UPDATE_INVALID");
		}
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? OriginalPartyGuid { get; set; }
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? OriginalThirdPartyOfGuid { get; set; }
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual string OriginalThirdPartyRoleName { get; set; }
	}
	public class ThirdPartyForUpdateRelationshipBase : ThirdPartyForUpdateRelationship<ThirdPartyId>, IThirdPartyForUpdateRelationship
	{
		public override ThirdPartyId Key
		{
			get
			{
				return new ThirdPartyId(this.PartyGuid ?? Guid.Empty, this.ThirdPartyOfGuid ?? Guid.Empty, this.ThirdPartyRoleName);
			}
			set
			{
				this.PartyGuid = value.PartyGuid;
				this.ThirdPartyOfGuid = value.ThirdPartyOfGuid;
				this.ThirdPartyRoleName = value.ThirdPartyRoleName;
			}
		}
	}
	public class ThirdPartyForUpdateRelationshipOrganization : ThirdPartyForUpdateRelationshipBase
	{
		[GlobalizedRequired("THIRDPARTY_PARTYGUID_REQUIRED")]
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
		[JsonIgnore]
		public override Guid? OriginalThirdPartyOfGuid
		{
			get
			{
				return base.OriginalThirdPartyOfGuid;
			}
			set
			{
				base.OriginalThirdPartyOfGuid = value;
			}
		}
	}
	public class ThirdPartyForUpdateRelationshipThirdParty : ThirdPartyForUpdateRelationshipBase
	{
		[GlobalizedRequired("THIRDPARTY_THIRDPARTYOF_REQUIRED")]
		public override Guid? ThirdPartyOfGuid
		{
			get
			{
				return base.ThirdPartyOfGuid;
			}
			set
			{
				base.ThirdPartyOfGuid = value;
			}
		}
		[JsonIgnore]
		public override Guid? OriginalPartyGuid
		{
			get
			{
				return base.OriginalPartyGuid;
			}
			set
			{
				base.OriginalPartyGuid = value;
			}
		}
	}
	public class ThirdPartyForUpdateRelationship : ThirdPartyForUpdateRelationshipBase
	{
		[GlobalizedRequired("THIRDPARTY_PARTYGUID_REQUIRED")]
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
		[GlobalizedRequired("THIRDPARTY_THIRDPARTYOF_REQUIRED")]
		public override Guid? ThirdPartyOfGuid
		{
			get
			{
				return base.ThirdPartyOfGuid;
			}
			set
			{
				base.ThirdPartyOfGuid = value;
			}
		}
	}
	/// <summary>
	/// ThirdParty for Update operation 
	/// </summary>
	public class ThirdPartyForUpdateBase : ThirdParty<PartyTypeKey, TierTypeKey>, IThirdPartyForUpdate
	{
		[GlobalizedRequired("THIRDPARTY_PARTYGUID_REQUIRED")]
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
		[ChildView]
		[UpdateOperationParameter]
		public PrimaryPartyAddressForAddUpdate PrimaryAddress { get; set; }
		[ChildView]
		[UpdateOperationParameter]
		public PrimaryPartyPhoneForAddUpdate PrimaryPhone { get; set; }
		[ChildView]
		[UpdateOperationParameter]
		public PrimaryPartyEmailForAddUpdate PrimaryEmail { get; set; }
		[JsonIgnore]
		public override PartyTypeKey PartyType
		{
			get
			{
				return new PartyTypeKey() { PartyTypeId = (int)Enums.PartyType.Organization };
			}
			set { }
		}
	}
	public class ThirdPartyForUpdateOrganization : ThirdPartyForUpdateBase
	{
		[JsonIgnore]
		public override Guid? ThirdPartyOfGuid
		{
			get
			{
				return base.ThirdPartyOfGuid;
			}
			set
			{
				base.ThirdPartyOfGuid = value;
			}
		}
	}

	public class ThirdPartyForUpdate : ThirdPartyForUpdateBase
	{
		[GlobalizedRequired("THIRDPARTY_THIRDPARTYOF_REQUIRED")]
		public override Guid? ThirdPartyOfGuid
		{
			get
			{
				return base.ThirdPartyOfGuid;
			}
			set
			{
				base.ThirdPartyOfGuid = value;
			}
		}
	}

	/// <summary>
	/// ThirdParty Result 
	/// </summary>
	public class ThirdPartyResult : ThirdParty<PartyTypeResult, TierType, StatusTypeResult, OrganizationResult, RoleResult>, IThirdPartyResult
	{
		public PrimaryPartyAddress PrimaryAddress { get; set; }

		public PrimaryPartyPhone PrimaryPhone { get; set; }

		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
