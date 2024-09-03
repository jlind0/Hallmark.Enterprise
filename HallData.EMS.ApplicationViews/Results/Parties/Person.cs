using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HallData.ApplicationViews;
using HallData.Validation;
using HallData.EMS.ApplicationViews.Enums;
using System.ComponentModel.DataAnnotations;

namespace HallData.EMS.ApplicationViews.Results
{
	/// <summary>
	/// Person Key abstract class
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	public abstract class PersonKey<TKey> : PartyKey<TKey>, IPersonKey<TKey> { }
	/// <summary>
	/// Person Key class
	/// </summary>
	public class PersonKey : PersonKey<Guid>
	{
		/// <summary>
		/// Overridden to return a Person Party Guid
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
	/// Person base abstract class with restriction by Party type 
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	/// <typeparam name="TPartyType">Party type</typeparam>
	public abstract class Person<TKey, TPartyType> : Party<TKey, TPartyType>, IPerson<TKey, TPartyType>
		where TPartyType: PartyTypeKey
	{
		/// <summary>
		/// First Name
		/// </summary>
		/// <remarks>
		/// A person first name
		/// </remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual string FirstName { get; set; }
		/// <summary>
		/// Last Name
		/// </summary>
		/// <remarks>
		/// A person last name
		/// </remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual string LastName { get; set; }
		/// <summary>
		/// Salutation
		/// </summary>
		/// <remarks>
		/// Example of Salutation: "Mr", "Mrs", "Miss"
		/// </remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual string Salutation { get; set; }
		/// <summary>
		/// Middle Name
		/// </summary>
		/// <remarks>
		/// A person middle name
		/// </remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual string MiddleName { get; set; }
		/// <summary>
		/// Suffix
		/// </summary>
		/// <remarks>
		/// Examples of Suffix: "Jr", "3rd", "III"
		/// </remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual string Suffix { get; set; }
		/// <summary>
		/// Gets validation status
		/// </summary>
		protected virtual bool ValidateFirstOrLast
		{
			get { return true; }
		}
		/// <summary>
		/// Overridden to return validation result 
		/// </summary>
		/// <param name="validationContext">Validation parameter</param>
		/// <returns>Validation Result</returns>
		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			foreach (var result in base.Validate(validationContext))
				yield return result;
			if (this.PartyGuid == null)
			{
				if (this.ValidateFirstOrLast && string.IsNullOrWhiteSpace(this.FirstName) && string.IsNullOrWhiteSpace(this.LastName))
					yield return ValidationResultFactory.Create(new ValidationResult("The person must have a first or last name"), "PERSON_NAME_REQUIRED");
			}
		}
	}

	/// <summary>
	/// Person Base Guid abstract class with restriction by Party and Status type
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public abstract class Person<TKey, TPartyType, TStatusType> : Person<TKey, TPartyType>, IPerson<TKey, TPartyType, TStatusType>
		where TStatusType : StatusTypeKey
		where TPartyType : PartyTypeKey
	{
		/// <summary>
		/// Gets and Sets status type
		/// </summary>
		[ChildView]
		[AddOperationParameter]
		public TStatusType Status { get; set; }
	}

	/// <summary>
	/// Person Base Guid class with restriction by Party type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	public class PersonGuid<TPartyType> : Person<Guid, TPartyType>
		where TPartyType: PartyTypeKey
	{
		/// <summary>
		/// Overridden to return a Person Party Guid
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
	/// Person Base Guid class with restriction by Party and Status type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public class PersonGuid<TPartyType, TStatusType> : Person<Guid, TPartyType, TStatusType>
		where TStatusType : StatusTypeKey
		where TPartyType : PartyTypeKey
	{
		/// <summary>
		/// Overridden to return a Person Party Guid
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
	/// Person for Add operation 
	/// </summary>
	public class PersonForAddBase : PersonGuid<PartyTypeKey, StatusTypeKey>, IPersonForAdd
	{

		/// <summary>
		/// Overridden to return a Person Party Type Key
		/// </summary>
		/// <remarks>Is Not Serialized</remarks>
		[JsonIgnore]
		public override PartyTypeKey PartyType
		{
			get
			{
				return new PartyTypeKey() { PartyTypeId = (int)Enums.PartyType.Person };
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

	public class PersonForAdd : PersonForAddBase
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
	/// Person for Update operation 
	/// </summary>
	public class PersonForUpdate : PersonGuid<PartyTypeKey>, IPersonForUpdate
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
		/// Overridden to return a Person Party Type Key
		/// </summary>
		/// <remarks>Is Not Serialized</remarks>
		[JsonIgnore]
		public override PartyTypeKey PartyType
		{
			get
			{
				return new PartyTypeKey() { PartyTypeId = (int)Enums.PartyType.Person };
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
	/// Person Result 
	/// </summary>
	public class PersonResult : PersonGuid<PartyTypeResult, StatusTypeResult>, IPersonResult
	{
		public PrimaryPartyAddress PrimaryAddress { get; set; }

		public PrimaryPartyPhone PrimaryPhone { get; set; }

		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
