using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HallData.ApplicationViews;
using HallData.Validation;

namespace HallData.EMS.ApplicationViews.Results
{

	/// <summary>
	/// Party Base Generic class with restriction by Tier and Party type 
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	public abstract class PartyGeneric<TKey, TPartyType, TTierType> : Party<TKey, TPartyType>,
		IPartyGeneric<TKey, TPartyType, TTierType>
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
	{
		/// <summary>
		/// Name of the organization.
		/// </summary>
		/// <remarks>
		/// Official name of the organization that will be displayed on customer's web sites or publications.
		/// </remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual string Name { get; set; }

		/// <summary>
		/// EIN - enterprise identification number.
		/// </summary>
		/// <remarks>
		/// EIN is required for billing purposes for Hallmark's Customers, that Hallmark is billing 
		/// </remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual string Ein { get; set; }

		/// <summary>
		/// Website
		/// </summary>
		/// <remarks>
		/// Website of the organization.
		/// </remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual string Website { get; set; }

		/// <summary>
		/// Code
		/// </summary>
		/// <remarks>
		/// The code will be used as a readable id. It will be used by accounting to do invoicing.
		/// </remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
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

		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual string ArName { get; set; }

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
			if (this.PartyType != null && this.PartyType.PartyTypeId != null)
			{
				var partyType = (Enums.PartyType)this.PartyType.PartyTypeId;
				if (partyType == Enums.PartyType.Person)
				{
					if (this.PartyGuid == null && this.ValidateFirstOrLast && string.IsNullOrWhiteSpace(this.FirstName) && string.IsNullOrWhiteSpace(this.LastName))
						yield return ValidationResultFactory.Create(new ValidationResult("First Name and Last name are not populated"), "PERSON_NAME_REQUIRED");
					if (this.Name != null)
						yield return ValidationResultFactory.Create(new ValidationResult("Name set for Person"), "PERSON_NAME_POPULATED");
					if (this.Ein != null)
						yield return ValidationResultFactory.Create(new ValidationResult("Ein set for Person"), "PERSON_EIN_POPULATED");
					if (this.Website != null)
						yield return ValidationResultFactory.Create(new ValidationResult("Website set for Person"), "PERSON_WEBSITE_POPULATED");
					if (this.Code != null)
						yield return ValidationResultFactory.Create(new ValidationResult("Code set for Person"), "PERSON_CODE_POPULATED");
					if (this.Tier != null && this.Tier.TierTypeId != null)
						yield return ValidationResultFactory.Create(new ValidationResult("Tier Type set for Person"), "PERSON_TIERTYPE_POPULATED");
					if (this.ArName != null)
						yield return ValidationResultFactory.Create(new ValidationResult("AR Name set for Organization"), "PERSON_ARNAME_POPULATED");
				}
				else if (partyType == Enums.PartyType.Organization)
				{
					if (this.FirstName != null)
						yield return ValidationResultFactory.Create(new ValidationResult("First Name set for Organization"), "ORGANIZATION_FIRSTNAME_POPULATED");
					if (this.LastName != null)
						yield return ValidationResultFactory.Create(new ValidationResult("Last Name set for Organization"), "ORGANIZATION_LASTNAME_POPULATED");
					if (this.Salutation != null)
						yield return ValidationResultFactory.Create(new ValidationResult("Salutation set for Organization"), "ORGANIZATION_SALUTATION_POPULATED");
					if (this.MiddleName != null)
						yield return ValidationResultFactory.Create(new ValidationResult("Middle Name set for Organization"), "ORGANIZATION_MIDDLENAME_POPULATED");
					if (this.Suffix != null)
						yield return ValidationResultFactory.Create(new ValidationResult("Suffix set for Organization"), "ORGANIZATION_SUFFIX_POPULATED");
					if (this.PartyGuid == null && string.IsNullOrWhiteSpace(this.Name))
						yield return ValidationResultFactory.Create(new ValidationResult("Name required for Organization"), "ORGANIZATION_NAME_REQUIRED");
				}
			}
		}
	}

	/// <summary>
	/// Party Base Generic Guid class with restriction by Tier and Party type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	public class PartyGenericGuid<TPartyType, TTierType> : PartyGeneric<Guid, TPartyType, TTierType>
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
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
	/// Party Base Generic class with restriction by Tier, Party and Status type
	/// </summary>
	/// <typeparam name="TKey">Key type</typeparam>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public abstract class PartyGeneric<TKey, TPartyType, TTierType, TStatusType> : PartyGeneric<TKey, TPartyType, TTierType>, IPartyGeneric<TKey, TPartyType, TTierType, TStatusType>
		where TStatusType : StatusTypeKey
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
	{
		/// <summary>
		/// Status type 
		/// </summary>
		[ChildView]
		[AddOperationParameter]
		public virtual TStatusType Status { get; set; }
	}

	/// <summary>
	/// Party Base Generic Guid class with restriction by Tier, Party and Status type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public class PartyGenericGuid<TPartyType, TTierType, TStatusType> : PartyGeneric<Guid, TPartyType, TTierType, TStatusType>
		where TStatusType : StatusTypeKey
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
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
}
