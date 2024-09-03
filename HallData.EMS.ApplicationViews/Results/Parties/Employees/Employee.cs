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
	public abstract class EmployeeKey<TKey> : PersonKey<TKey>, IEmployeeKey<TKey>
		where TKey: IEmployeeId
	{
		/// <summary>
		/// Employer Guid
		/// </summary>
		[UpdateOperationParameter]
		[AddOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? EmployerGuid { get; set; }
	}
	/// <summary>
	/// Employee Key class
	/// </summary>
	public class EmployeeKey : EmployeeKey<EmployeeId>
	{
		
		/// <summary>
		/// Overridden to return Employee Id
		/// </summary>
		/// <remarks>Is Not Serialized</remarks>
		public override EmployeeId Key
		{
			get
			{
				return new EmployeeId(this.EmployerGuid, this.PartyGuid ?? Guid.Empty);
			}
			set
			{
				this.PartyGuid = value.PartyGuid;
				this.EmployerGuid = value.EmployerGuid;
			}
		}
	}
	public class EmployeeKeyRelationshipForEmployer : EmployeeKey
	{
		[GlobalizedRequired("EMPLOYEE_PARTYGUID_REQUIRED")]
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
	public class EmployeeKeyRelationshipForEmployee : EmployeeKey
	{
		[GlobalizedRequired("EMPLOYEE_EMPLOYERGUID_REQUIRED")]
		public override Guid? EmployerGuid
		{
			get
			{
				return base.EmployerGuid;
			}
			set
			{
				base.EmployerGuid = value;
			}
		}
	}
	public abstract class EmployeeKeyRelationship : EmployeeKey
	{
		[GlobalizedRequired("EMPLOYEE_EMPLOYERGUID_REQUIRED")]
		public override Guid? EmployerGuid
		{
			get
			{
				return base.EmployerGuid;
			}
			set
			{
				base.EmployerGuid = value;
			}
		}
		[GlobalizedRequired("EMPLOYEE_PARTYGUID_REQUIRED")]
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
	/// Employee Base class with restriction by Title and Party type 
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTitleType">Title type</typeparam>
	public abstract class EmployeeWithKey<TKey, TPartyType, TTitleType> : Person<TKey, TPartyType>, IEmployeeBase<TKey, TPartyType, TTitleType>
		where TPartyType: PartyTypeKey
		where TTitleType: TitleTypeKey
		where TKey: IEmployeeId
	{
		
		/// <summary>
		/// Title type
		/// </summary>
		/// <remarks>Type of the title</remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		[GlobalizedRequired("EMPLOYEE_TITLE_REQUIRED")]
		public TTitleType Title { get; set; }
		/// <summary>
		/// Employer Guid
		/// </summary>
		/// <remarks>
		/// Guid of the Employee.
		/// </remarks>
		[UpdateOperationParameter]
		[AddOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? EmployerGuid { get; set; }
		/// <summary>
		/// Overridden to return validation result 
		/// </summary>
		/// <param name="validationContext">Validation parameter</param>
		/// <returns>Validation Result</returns>
		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			foreach (var result in base.Validate(validationContext))
				yield return result;
			if (this.Title == null || this.Title.TitleTypeId == null)
				yield return ValidationResultFactory.Create(new ValidationResult("Title is required"), "EMPLOYEE_TITLE_REQUIRED");
		}
	}
	public class Employee<TPartyType, TTitleType> : EmployeeWithKey<EmployeeId, TPartyType, TTitleType>, IEmployee<TPartyType, TTitleType>
		where TPartyType : PartyTypeKey
		where TTitleType: TitleTypeKey
	{
		/// <summary>
		/// Overridden to return Employee Id
		/// </summary>
		public override EmployeeId Key
		{
			get
			{
				return new EmployeeId(this.EmployerGuid, this.PartyGuid ?? Guid.Empty);
			}
			set
			{
				this.PartyGuid = value.PartyGuid;
				this.EmployerGuid = value.EmployerGuid;
			}
		}
	}
	/// <summary>
	/// Employee Base class with restriction by Title, Party and Status type 
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTitleType">Title type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public class Employee<TPartyType, TTitleType, TStatusType> : Employee<TPartyType, TTitleType>, IEmployee<TPartyType, TTitleType, TStatusType>
		where TPartyType : PartyTypeKey
		where TTitleType : TitleTypeKey
		where TStatusType: StatusTypeKey
	{
		/// <summary>
		/// Employee Relationship Status
		/// </summary>
		/// <remarks>Status of the Employee relationship</remarks>
		[AddOperationParameter]
		[ChildView]
		public TStatusType EmployeeRelationshipStatus { get; set; }
		/// <summary>
		///Employee status type
		/// </summary>
		[AddOperationParameter]
		[ChildView]
		public TStatusType Status { get; set; }
	}
	/// <summary>
	/// Employee Base class with restriction by Title, Party, Status and Organization type 
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTitleType">Title type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	/// <typeparam name="TOrganization">Organization type</typeparam>
	public class Employee<TPartyType, TTitleType, TStatusType, TOrganization> : Employee<TPartyType, TTitleType, TStatusType>, IEmployee<TPartyType, TTitleType, TStatusType, TOrganization>
		where TStatusType : StatusTypeKey
		where TPartyType : PartyTypeKey
		where TTitleType : TitleTypeKey
		where TOrganization: IOrganizationKey
	{
		/// <summary>
		/// Organization Employer
		/// </summary>
		[ChildView]
		public TOrganization Employer { get; set; }
	}
	public abstract class EmployeeForAddRelationshipBase<TKey> : EmployeeKey<TKey>, IEmployeeForAddRelationship<TKey>
		where TKey: IEmployeeId
	{
		[ChildView]
		[AddOperationParameter]
		public StatusTypeKey EmployeeRelationshipStatus { get; set; }
	}
	public class EmployeeForAddRelationshipBase : EmployeeKey, IEmployeeForAddRelationship
	{
		[ChildView]
		[AddOperationParameter]
		public StatusTypeKey EmployeeRelationshipStatus { get; set; }
	}
	public class EmployeeForAddRelationshipEmployee : EmployeeForAddRelationshipBase
	{
		[GlobalizedRequired("EMPLOYEE_EMPLOYERGUID_REQUIRED")]
		public override Guid? EmployerGuid
		{
			get
			{
				return base.EmployerGuid;
			}
			set
			{
				base.EmployerGuid = value;
			}
		}
	}
	public class EmployeeForAddRelationshipEmployer : EmployeeForAddRelationshipBase
	{
		[GlobalizedRequired("EMPLOYEE_PARTYGUID_REQUIRED")]
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
	public class EmployeeForAddRelationship : EmployeeForAddRelationshipBase
	{
		[GlobalizedRequired("EMPLOYEE_PARTYGUID_REQUIRED")]
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
		[GlobalizedRequired("EMPLOYEE_EMPLOYERGUID_REQUIRED")]
		public override Guid? EmployerGuid
		{
			get
			{
				return base.EmployerGuid;
			}
			set
			{
				base.EmployerGuid = value;
			}
		}
	}
	/// <summary>
	/// Employee for Add operation 
	/// </summary>
	public class EmployeeForAdd : Employee<PartyTypeKey, TitleTypeKey, StatusTypeKey>, IEmployeeForAdd
	{
		
		/// <summary>
		/// Overridden to return Employee Party Type Key
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
	public abstract class EmployeeForUpdateRelationshipBase<TKey> : EmployeeKey<TKey>, IEmployeeForUpdateRelationship<TKey>, IValidatableObject
		where TKey: IEmployeeId
	{
		public virtual Guid? OriginalEmployerGuid { get; set; }
		public virtual Guid? OriginalPartyGuid { get; set; }

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if ((this.OriginalEmployerGuid == null && this.OriginalPartyGuid == null) || (this.OriginalEmployerGuid != null && this.OriginalPartyGuid != null))
				yield return ValidationResultFactory.Create(new ValidationResult("Employee Update Relationship must have an original party guid or orgiginal employer guid"), "EMPLOYEE_RELATIONSHIP_UPDATE_INVALID");
		}
	}
	public class EmployeeForUpdateRelationshipBase : EmployeeForUpdateRelationshipBase<EmployeeId>, IEmployeeForUpdateRelationship
	{
		public override EmployeeId Key
		{
			get
			{
				return new EmployeeId(this.EmployerGuid, this.PartyGuid ?? Guid.Empty);
			}
			set
			{
				this.PartyGuid = value.PartyGuid;
				this.EmployerGuid = value.EmployerGuid;
			}
		}
	}
	public class EmployeeForUpdateRelationshipEmployee : EmployeeForUpdateRelationshipBase
	{
		[GlobalizedRequired("EMPLOYEE_EMPLOYERGUID_REQUIRED")]
		public override Guid? EmployerGuid
		{
			get
			{
				return base.EmployerGuid;
			}
			set
			{
				base.EmployerGuid = value;
			}
		}
		[GlobalizedRequired("EMPLOYEE_ORIGINALEMPLOYERGUID_REQUIRED")]
		public override Guid? OriginalEmployerGuid
		{
			get
			{
				return base.OriginalEmployerGuid;
			}
			set
			{
				base.OriginalEmployerGuid = value;
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
	public class EmployeeForUpdateRelationshipEmployer : EmployeeForUpdateRelationshipBase
	{
		[GlobalizedRequired("EMPLOYEE_PARTYGUID_REQUIRED")]
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
		public override Guid? EmployerGuid
		{
			get
			{
				return base.EmployerGuid;
			}
			set
			{
				base.EmployerGuid = value;
			}
		}
		[JsonIgnore]
		public override Guid? OriginalEmployerGuid
		{
			get
			{
				return base.OriginalEmployerGuid;
			}
			set
			{
				base.OriginalEmployerGuid = value;
			}
		}
		[GlobalizedRequired("EMPLOYEE_ORIGINALPARTYGUID_REQUIRED")]
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
	public class EmployeeForUpdateRelationship : EmployeeForUpdateRelationshipBase
	{
		[GlobalizedRequired("EMPLOYEE_PARTYGUID_REQUIRED")]
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
		[GlobalizedRequired("EMPLOYEE_ORIGINALEMPLOYERGUID_REQUIRED")]
		public override Guid? OriginalEmployerGuid
		{
			get
			{
				return base.OriginalEmployerGuid;
			}
			set
			{
				base.OriginalEmployerGuid = value;
			}
		}
	}

	/// <summary>
	/// Employee for Update operation 
	/// </summary>
	public class EmployeeForUpdate : Employee<PartyTypeKey, TitleTypeKey>, IEmployeeForUpdate
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
		/// Overridden to return Employee Party Type Key
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

	public class EmployeeForUpdateEmployer : EmployeeForUpdate
	{
		[JsonIgnore]
		public override Guid? EmployerGuid
		{
			get
			{
				return base.EmployerGuid;
			}
			set
			{
				base.EmployerGuid = value;
			}
		}
	}

	/// <summary>
	/// Employee Result 
	/// </summary>
	public class EmployeeResult : Employee<PartyTypeResult, TitleTypeName, StatusTypeResult, OrganizationResult>, IEmployeeResult
	{
		public PrimaryPartyAddress PrimaryAddress { get; set; }

		public PrimaryPartyPhone PrimaryPhone { get; set; }

		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
