using System;
using System.Collections.Generic;
using HallData.Validation;
using HallData.ApplicationViews;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HallData.EMS.ApplicationViews.Results
{
	/// <summary>
	/// Party Key class
	/// </summary>
	public class PartyKey : PartyKey<Guid>
	{
		/// <summary>
		/// Overridden to return Party Guid
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
	/// Party Key abstract class
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	public abstract class PartyKey<TKey> : IPartyKey<TKey>
	{
		public PartyKey()
		{
			this.InstanceGuid = Guid.NewGuid();
		}
		/// <summary>
		/// Gets and Sets Key
		/// </summary>
		/// <remarks>Is Not Serialized</remarks>
		[JsonIgnore]
		public abstract TKey Key { get; set; }
		/// <summary>
		/// Party Guid
		/// </summary>
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? PartyGuid { get; set; }
		[JsonIgnore]
		public Guid InstanceGuid { get; private set; }
	}

	/// <summary>
	/// Party Base class with restriction by Party type 
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	/// <typeparam name="TPartyType">Party type</typeparam>
	public abstract class Party<TKey, TPartyType> : PartyKey<TKey>, IParty<TKey, TPartyType>, IValidatableObject
		where TPartyType : PartyTypeKey
	{
		/// <summary>
		/// Party type
		/// </summary>
		[ChildView]
		[AddOperationParameter]
		[GlobalizedRequired("PARTY_PARTYTYPE_REQUIRED")]
		public virtual TPartyType PartyType { get; set; }
		/// <summary>
		/// Virtual to return validation result 
		/// </summary>
		/// <param name="validationContext">Validation parameter</param>
		/// <returns>Validation Result</returns>
		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if(this.PartyType == null || this.PartyType.PartyTypeId == null)
				yield return ValidationResultFactory.Create(new ValidationResult("Party Type is required"), "PARTY_PARTYTYPE_REQUIRED");
		}
	}

	/// <summary>
	/// Party Base class with restriction by Party and Status type 
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public abstract class Party<TKey, TPartyType, TStatusType> : Party<TKey, TPartyType>, IParty<TKey, TPartyType, TStatusType>
		where TPartyType : PartyTypeKey
		where TStatusType: StatusTypeKey
	{
		/// <summary>
		/// Status type 
		/// </summary>
		[ChildView]
		[AddOperationParameter]
		public virtual TStatusType Status { get; set; }
	}

	/// <summary>
	/// Party Base Guid class with restriction by Party type 
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	public class PartyGuid<TPartyType> : Party<Guid, TPartyType>
		where TPartyType : PartyTypeKey
	{
		/// <summary>
		/// Overridden to return Party Guid
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
	/// Party for Add operation 
	/// </summary>
	public class PartyForAddBase: PartyGenericGuid<PartyTypeKey, TierTypeKey, StatusTypeKey>, IPartyGenericForAdd
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
	}

	public class PartyForAdd : PartyForAddBase
	{
		/// <summary>
		/// Overridden to return Party Guid
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
	/// Party for Update operation 
	/// </summary>
	public class PartyForUpdate : PartyGenericGuid<PartyTypeKey, TierTypeKey>, IPartyGenericForUpdate
	{
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
	/// Party Result 
	/// </summary>
	public class PartyResult : PartyGenericGuid<PartyTypeResult, TierType, StatusTypeResult>, IPartyGenericResult 
	{
		public PrimaryPartyAddress PrimaryAddress { get; set; }

		public PrimaryPartyPhone PrimaryPhone { get; set; }

		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
