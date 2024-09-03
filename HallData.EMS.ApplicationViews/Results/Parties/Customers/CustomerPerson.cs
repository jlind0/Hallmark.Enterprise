using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HallData.EMS.ApplicationViews.Results
{
	/// <summary>
	/// Customer Person Key class
	/// </summary>
	public class CustomerPersonKey : PersonKey<CustomerId>, ICustomerPersonKey
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
	/// Customer Person Base class with restriction by Party type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	public class CustomerPerson<TPartyType> : Person<CustomerId, TPartyType>, ICustomerPerson<TPartyType>
		where TPartyType: PartyTypeKey
	{
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
		/// Gets and Sets validation status
		/// </summary>
		protected override bool ValidateFirstOrLast
		{
			get
			{
				return false;
			}
		}

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
		/// Overridden to return validation result 
		/// </summary>
		/// <param name="validationContext">Validation parameter</param>
		/// <returns>Validation Result</returns>
		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			foreach (var result in base.Validate(validationContext))
				yield return result;
			if (string.IsNullOrWhiteSpace(this.FirstName) && string.IsNullOrWhiteSpace(this.LastName) &&
						(string.IsNullOrWhiteSpace(this.JobTitle) || string.IsNullOrWhiteSpace(this.WorksFor)))
				yield return ValidationResultFactory.Create(new ValidationResult("First Name, Last Name or Job Title/Works For must be populated for customer person"), "CUSTOMERPERSON_NAME_REQUIRED");
		}
	}

	/// <summary>
	/// Customer Person Base class with restriction by Party and Status type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public class CustomerPerson<TPartyType, TStatusType> : CustomerPerson<TPartyType>, ICustomerPerson<TPartyType, TStatusType>
		where TPartyType: PartyTypeKey
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
	/// Customer Person Base class with restriction by Party, Status and Organization type
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	/// <typeparam name="TOrganization">Organization type</typeparam>
	public class CustomerPerson<TPartyType, TStatusType, TOrganization> : CustomerPerson<TPartyType, TStatusType>, ICustomerPerson<TPartyType, TStatusType, TOrganization>
		where TPartyType: PartyTypeKey
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
	/// Customer Person for Add operation 
	/// </summary>
	public class CustomerPersonForAdd : CustomerPerson<PartyTypeKey, StatusTypeKey>, ICustomerPersonForAdd
	{
		/// <summary>
		/// Overridden to return a Customer Person Party Guid
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
		/// <summary>
		/// Overridden to return a Customer Person Party Type Key
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

	/// <summary>
	/// Customer Person for Update operation 
	/// </summary>
	public class CustomerPersonForUpdate : CustomerPerson<PartyTypeKey>, ICustomerPersonForUpdate
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
		/// Overridden to return a Customer Person Party Type Key
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
	/// Customer Person Result
	/// </summary>
	public class CustomerPersonResult : CustomerPerson<PartyTypeResult, StatusTypeResult, OrganizationResult>, ICustomerPersonResult
	{

		public PrimaryPartyAddress PrimaryAddress { get; set; }

		public PrimaryPartyPhone PrimaryPhone { get; set; }

		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
