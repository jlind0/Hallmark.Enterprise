using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace HallData.EMS.ApplicationViews.Results
{
	/// <summary>
	/// Customer Base Generic class with restriction by Party and Tier type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	public class CustomerGeneric<TPartyType, TTierType> : PartyGeneric<CustomerId, TPartyType, TTierType>, ICustomerGeneric<TPartyType, TTierType>
		where TPartyType: PartyTypeKey
		where TTierType: TierTypeKey
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
		/// <summary>
		/// Gets and Sets Job Title
		/// </summary>
		/// <remarks>
		/// Job Title of the customer person.
		/// </remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string JobTitle { get; set; }
		/// <summary>
		/// Gets and Sets Works for
		/// </summary>
		/// <remarks>
		/// Employer name
		/// </remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string WorksFor { get; set; }
		/// <summary>
		/// Gets validation status
		/// </summary>
		protected override bool ValidateFirstOrLast
		{
			get
			{
				return false;
			}
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
			if(this.PartyType != null && this.PartyType.PartyTypeId != null)
			{
				var pt = (Enums.PartyType)this.PartyType.PartyTypeId.Value;
				if(pt == Enums.PartyType.Organization)
				{
					if (this.JobTitle != null)
						yield return ValidationResultFactory.Create(new ValidationResult("Job Title populated for organization"), "CUSTOMERORGANIZATION_JOBTITLE_POPULATED");
					if(this.WorksFor != null)
						yield return ValidationResultFactory.Create(new ValidationResult("Works For populated for organization"), "CUSTOMERORGANIZATION_WORKSFOR_POPULATED");
				}
				else if(pt == Enums.PartyType.Person)
				{
					if (string.IsNullOrWhiteSpace(this.FirstName) && string.IsNullOrWhiteSpace(this.LastName) &&
						(string.IsNullOrWhiteSpace(this.JobTitle) || string.IsNullOrWhiteSpace(this.WorksFor)))
						yield return ValidationResultFactory.Create(new ValidationResult("First Name, Last Name or Job Title/Works For must be populated for customer person"), "CUSTOMERPERSON_NAME_REQUIRED");
				}
			}
		}
	}
	/// <summary>
	/// Customer Base Generic class with restriction by Party, Tier and Status type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public class CustomerGeneric<TPartyType, TTierType, TStatusType> : CustomerGeneric<TPartyType, TTierType>, ICustomerGeneric<TPartyType, TTierType, TStatusType>
		where TPartyType: PartyTypeKey
		where TTierType: TierTypeKey
		where TStatusType: StatusTypeKey
	{
		/// <summary>
		/// Gets and Sets Customer Relationship Status
		/// </summary>
		[AddOperationParameter]
		[ChildView]
		public TStatusType CustomerRelationshipStatus { get; set; }
		/// <summary>
		/// Gets and Sets Status type
		/// </summary>
		[AddOperationParameter]
		[ChildView]
		public TStatusType Status { get; set; }
	}
	/// <summary>
	/// Customer Base Generic class with restriction by Party, Tier, Status and Organization type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTierType">Tier type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	/// <typeparam name="TOrganization">Organization type</typeparam>
	public class CustomerGeneric<TPartyType, TTierType, TStatusType, TOrganization> : CustomerGeneric<TPartyType, TTierType, TStatusType>, ICustomerGeneric<TPartyType, TTierType, TStatusType, TOrganization>
		 where TStatusType: StatusTypeKey
		where TPartyType: PartyTypeKey
		where TTierType: TierTypeKey
		where TOrganization: IOrganizationKey
	{
		/// <summary>
		/// Gets and Sets Customer Of Organization
		/// </summary>
		[ChildView]
		public TOrganization CustomerOf { get; set; }
	}
	/// <summary>
	/// Customer for Add operation 
	/// </summary>
	public class CustomerForAddBase : CustomerGeneric<PartyTypeKey, TierTypeKey, StatusTypeKey>, ICustomerGenericForAdd
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
	public class CustomerForAdd : CustomerForAddBase
	{
		/// <summary>
		/// Overridden to return Customer Party Guid
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
	/// Customer for Update operation 
	/// </summary>
	public class CustomerForUpdate : CustomerGeneric<PartyTypeKey, TierTypeKey>, ICustomerGenericForUpdate
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
	/// Customer Result 
	/// </summary>
	public class CustomerResult : CustomerGeneric<PartyTypeResult, TierType, StatusTypeResult, OrganizationResult>, ICustomerResult
	{
		public PrimaryPartyAddress PrimaryAddress { get; set; }

		public PrimaryPartyPhone PrimaryPhone { get; set; }

		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
