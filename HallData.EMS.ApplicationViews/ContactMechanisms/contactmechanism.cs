using System;
using System.Collections.Generic;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HallData.EMS.ApplicationViews
{
	public class ContactMechanismKey : IContactMechanismKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? ContactMechanismGuid { get; set; }
		[JsonIgnore]
		public Guid Key
		{
			get
			{
				return this.ContactMechanismGuid ?? Guid.Empty;
			}
			set
			{
				this.ContactMechanismGuid = value;
			}
		}
	}

	public class ContactMechanism<TMechanismType> : ContactMechanismKey, IContactMechanism<TMechanismType>, IValidatableObject
		where TMechanismType: MechanismTypeKey
	{
		[ChildView]
		[AddOperationParameter]
		public virtual TMechanismType MechanismType { get; set; }

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (this.ContactMechanismGuid == null)
			{
				if (this.MechanismType == null || this.MechanismType.MechanismTypeId == null)
					yield return ValidationResultFactory.Create(new ValidationResult("The contact mechanism requires a mechanism type"), "CONTACTMECHANISM_MECHANISMTYPE_REQUIRED");
			}
		}
	}

	[DefaultView("Address")]
	[ContactMechanismType(Enums.MechanismTypes.Address, "ADDRESS_MECHANISMTYPE_INVALID")]
	public class Address<TMechanismType> : ContactMechanism<TMechanismType>, IAddress<TMechanismType>
		where TMechanismType: MechanismTypeKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string AddressLine1 { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string AddressLine2 { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string AddressLine3 { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string City { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string PostalCode { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string Country { get; set; }
		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			foreach (var valid in base.Validate(validationContext))
				yield return valid;
			if(this.ContactMechanismGuid == null && string.IsNullOrWhiteSpace(this.AddressLine1))
				yield return ValidationResultFactory.Create(new ValidationResult("The address line is required for add's without a contact mechanism guid"), "ADDRESS_ADDRESSLINE1_REQUIRED");
		}
	}

	public class PrimaryAddress<TMechanismType, TStatusType> : Address<TMechanismType, TStatusType>, IPrimaryAddress<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType: StatusTypeKey
	{
		[UpdateOperationParameter]
		public override TStatusType Status
		{
			get
			{
				return base.Status;
			}
			set
			{
				base.Status = value;
			}
		}

		[ChildView]
		[AddOperationParameter]
		[UpdateOperationParameter]
		public TStatusType PrimaryRelationshipStatus { get; set; }

		[AddOperationParameter]
		public override Guid? ContactMechanismGuid
		{
			get
			{
				return base.ContactMechanismGuid;
			}
			set
			{
				base.ContactMechanismGuid = value;
			}
		}

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		[GlobalizedRequired("CONTACT_CONTACTMECHANISMTYPE_REQUIRED")]
		public string ContactMechanismTypeName { get; set; }
	}
	
	public class Address<TMechanismType, TStatusType> : Address<TMechanismType>, IAddress<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType: StatusTypeKey
	{
		[AddOperationParameter]
		[ChildView]
		public virtual TStatusType Status { get; set; }
	}

	public class AddressForAdd : Address<MechanismTypeKey, StatusTypeKey>, IAddressForAdd
	{
		[JsonIgnore]
		public override MechanismTypeKey MechanismType
		{
			get
			{
				return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Address };
			}
			set
			{
			}
		}
	}
	public class AddressForUpdate : Address<MechanismTypeKey>, IAddressForUpdate
	{
		[JsonIgnore]
		public override MechanismTypeKey MechanismType
		{
			get
			{
				return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Address };
			}
			set
			{
			}
		}
	}
	public class PrimaryAddressForAddUpdate : PrimaryAddress<MechanismTypeKey, StatusTypeKey>, IPrimaryAddressForAddUpdate
	{
		[JsonIgnore]
		public override MechanismTypeKey MechanismType
		{
			get
			{
				return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Address };
			}
			set
			{
			}
		}
	}
	public class Address : Address<MechanismTypeResult, StatusTypeResult>, IAddressResult
	{

	}
	public class PrimaryAddress : PrimaryAddress<MechanismTypeResult, StatusTypeResult>, IPrimaryAddressResult
	{

		public ContactMechanismType ContactMechanismType { get; set; }
	}

	[DefaultView("Email")]
	[ContactMechanismType(Enums.MechanismTypes.Email, "EMAIL_MECHANISMTYPE_INVALID")]
	public class Email<TMechanismType> : ContactMechanism<TMechanismType>, IEmail<TMechanismType>
		where TMechanismType: MechanismTypeKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedDataType(DataType.EmailAddress, "EMAIL_ADDRESS_INVALID")]
		public string EmailAddress { get; set; }
		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			foreach (var valid in base.Validate(validationContext))
				yield return valid;
			if (this.ContactMechanismGuid == null && string.IsNullOrWhiteSpace(this.EmailAddress))
				yield return ValidationResultFactory.Create(new ValidationResult("Email Address required for add without contact mechanism guid"), "EMAIL_ADDRESS_REQUIRED");
		}
	}

	public class Email<TMechanismType, TStatusType> : Email<TMechanismType>, IEmail<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType: StatusTypeKey
	{
		[AddOperationParameter]
		[ChildView]
		public virtual TStatusType Status { get; set; }
	}

	public class PrimaryEmail<TMechanismType, TStatusType> : Email<TMechanismType, TStatusType>, IPrimaryEmail<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType: StatusTypeKey
	{
		[ChildView]
		[AddOperationParameter]
		[UpdateOperationParameter]
		public TStatusType PrimaryRelationshipStatus { get; set; }
		[UpdateOperationParameter]
		public override TStatusType Status
		{
			get
			{
				return base.Status;
			}
			set
			{
				base.Status = value;
			}
		}
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		[GlobalizedRequired("CONTACT_CONTACTMECHANISMTYPE_REQUIRED")]
		public string ContactMechanismTypeName { get; set; }
	}

	public class EmailForAdd : Email<MechanismTypeKey, StatusTypeKey>, IEmailForAdd
	{
		[JsonIgnore]
		public override Guid? ContactMechanismGuid
		{
			get
			{
				return base.ContactMechanismGuid;
			}
			set
			{
				base.ContactMechanismGuid = value;
			}
		}
		[JsonIgnore]
		public override MechanismTypeKey MechanismType
		{
			get
			{
				return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Email };
			}
			set
			{
			}
		}
	}
	public class EmailForUpdate : Email<MechanismTypeKey>, IEmailForUpdate
	{
		[JsonIgnore]
		public override MechanismTypeKey MechanismType
		{
			get
			{
				return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Email };
			}
			set
			{
			}
		}
	}
	public class PrimaryEmailForAddUpdate : PrimaryEmail<MechanismTypeKey, StatusTypeKey>, IPrimaryEmailForAddUpdate
	{
		[JsonIgnore]
		public override MechanismTypeKey MechanismType
		{
			get
			{
				return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Email };
			}
			set
			{
			}
		}
	}
	public class Email : Email<MechanismTypeResult, StatusTypeResult>, IEmailResult
	{

	}
	public class PrimaryEmail : PrimaryEmail<MechanismTypeResult, StatusTypeResult>, IPrimaryEmailResult 
	{

		public new ContactMechanismType ContactMechanismType { get; set; }
	}

	[DefaultView("Phone")]
	[ContactMechanismType(Enums.MechanismTypes.Phone, "PHONE_MECHANISMTYPE_INVALID")]
	public class Phone<TMechanismType> : ContactMechanism<TMechanismType>, IPhone<TMechanismType>
		where TMechanismType:MechanismTypeKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedDataType(DataType.PhoneNumber, "PHONE_NUMBER_INVALID")]
		public string PhoneNumber { get; set; }
		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			foreach (var valid in base.Validate(validationContext))
				yield return valid;
			if (this.ContactMechanismGuid == null && string.IsNullOrWhiteSpace(this.PhoneNumber))
				yield return ValidationResultFactory.Create(new ValidationResult("Phone Number required for add if contact mechanism guid not provided"), "PHONE_NUMBER_REQUIRED");
		}
	}

	public class Phone<TMechanismType, TStatusType> : Phone<TMechanismType>, IPhone<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType: StatusTypeKey
	{
		[AddOperationParameter]
		[ChildView]
		public virtual TStatusType Status { get; set; }
	}

	public class PrimaryPhone<TMechanismType, TStatusType> : Phone<TMechanismType, TStatusType>, IPrimaryPhone<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType: StatusTypeKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		public TStatusType PrimaryRelationshipStatus { get; set; }
		[UpdateOperationParameter]
		public override TStatusType Status
		{
			get
			{
				return base.Status;
			}
			set
			{
				base.Status = value;
			}
		}
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		[GlobalizedRequired("CONTACT_CONTACTMECHANISMTYPE_REQUIRED")]
		public string ContactMechanismTypeName { get; set; }
	}

	public class PhoneForAdd : Phone<MechanismTypeKey, StatusTypeKey>, IPhoneForAdd
	{
		[JsonIgnore]
		public override MechanismTypeKey MechanismType
		{
			get
			{
				return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Phone };
			}
			set
			{
			}
		}
	}
	public class PhoneForUpdate : Phone<MechanismTypeKey>, IPhoneForUpdate
	{
		[JsonIgnore]
		public override MechanismTypeKey MechanismType
		{
			get
			{
				return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Phone };
			}
			set
			{
			}
		}
	}
	public class PrimaryPhoneForAddUpdate : PrimaryPhone<MechanismTypeKey, StatusTypeKey>, IPrimaryPhoneForAddUpdate
	{
		[JsonIgnore]
		public override MechanismTypeKey MechanismType
		{
			get
			{
				return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Phone };
			}
			set
			{
			}
		}
	}
	public class Phone : Phone<MechanismTypeResult, StatusTypeResult>, IPhoneResult
	{

	}
	public class PrimaryPhone : PrimaryPhone<MechanismTypeResult, StatusTypeResult>, IPrimaryPhoneResult
	{

		public ContactMechanismType ContactMechanismType { get; set; }
	}
	[DefaultView("Generic")]
	public class ContactMechanismGeneric<TMechanismType> : ContactMechanism<TMechanismType>, IContactMechanismGeneric<TMechanismType>
		where TMechanismType: MechanismTypeKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string AddressLine1 { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string AddressLine2 { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string AddressLine3 { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string City { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string PostalCode { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string Country { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedDataType(DataType.PhoneNumber, "PHONE_NUMBER_INVALID")]
		public string PhoneNumber { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedDataType(DataType.EmailAddress, "EMAIL_ADDRESS_INVALID")]
		public string EmailAddress { get; set; }
		[AddOperationParameter]
		public override TMechanismType MechanismType
		{
			get
			{
				return base.MechanismType;
			}
			set
			{
				base.MechanismType = value;
			}
		}
		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			foreach (var val in base.Validate(validationContext))
				yield return val;
			if (this.ContactMechanismGuid == null)
			{
				if (this.MechanismType != null && this.MechanismType.MechanismTypeId != null)
				{
					Enums.MechanismTypes mt = (Enums.MechanismTypes)this.MechanismType.MechanismTypeId.Value;
					if (mt == Enums.MechanismTypes.Address)
					{
						if (string.IsNullOrWhiteSpace(this.AddressLine1))
							yield return ValidationResultFactory.Create(new ValidationResult("Address Line 1 required"), "ADDRESS_LINE1_REQUIRED");
					}
					else if (mt == Enums.MechanismTypes.Email)
					{
						if (string.IsNullOrWhiteSpace(this.EmailAddress))
							yield return ValidationResultFactory.Create(new ValidationResult("Email Address required"), "EMAIL_ADDRESS_REQUIRED");
					}
					else if (mt == Enums.MechanismTypes.Phone)
					{
						if (string.IsNullOrWhiteSpace(this.PhoneNumber))
							yield return ValidationResultFactory.Create(new ValidationResult("Phone Number required"), "PHONE_NUMBER_REQUIRED");
					}
				}
			}
		}
	}
	public class ContactMechanismGeneric<TMechanismType, TStatusType> : ContactMechanismGeneric<TMechanismType>, IContactMechanismGeneric<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType: StatusTypeKey
	{
		[AddOperationParameter]
		[ChildView]
		public TStatusType Status { get; set; }
	}
	public class ContactMechanismGenericForAdd : ContactMechanismGeneric<MechanismTypeKey, StatusTypeKey>, IContactMechanismGenericForAdd
	{
		
	}
	public class ContactMechanismGenericForUpdate : ContactMechanismGeneric<MechanismTypeKey>, IContactMechanismGenericForUpdate { }
	public class ContactMechanismGeneric : ContactMechanismGeneric<MechanismTypeResult, StatusTypeResult>, IContactMechanismGenericResult { }
}
