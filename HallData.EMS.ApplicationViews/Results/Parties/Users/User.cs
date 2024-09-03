using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using HallData.EMS.ApplicationViews.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HallData.EMS.ApplicationViews.Results
{
	/// <summary>
	/// User Key class
	/// </summary>
	public class UserKey : PersonKey, IUserKey 
	{
	}
	/// <summary>
	///User Base class with restriction by Title, Party and Organization type 
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TOrganization">Organization type</typeparam>
	public class User<TPartyType, TOrganization> : PersonGuid<TPartyType>, IUser<TPartyType, TOrganization>
		where TPartyType: PartyTypeKey
		where TOrganization: IOrganizationKey
	{
		/// <summary>
		/// Organization User Of
		/// </summary>
		/// <remarks>Organization user belongs to</remarks>
		[ChildView]
		[AddOperationParameter]
		[UpdateOperationParameter]
		public TOrganization UserOf { get; set; }
		/// <summary>
		/// User Name
		/// </summary>
		/// <remarks>Name of the user</remarks>
		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("USER_USERNAME_REQUIRED")]
		public string UserName { get; set; }
	}

	/// <summary>
	///User Base class with restriction by Title, Party, Status and Organization type 
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TOrganization">Organization</typeparam>
	/// <typeparam name="TStatusType">Status</typeparam>
	public class User<TPartyType, TOrganization, TStatusType> : User<TPartyType, TOrganization>, IUser<TPartyType, TOrganization, TStatusType>
		where TPartyType : PartyTypeKey
		where TOrganization : IOrganizationKey
		where TStatusType: StatusTypeKey
	{
		/// <summary>
		/// User Relationship Status
		/// </summary>
		/// <remarks>Status of the user relationship</remarks>
		[ChildView]
		[AddOperationParameter]
		public TStatusType UserRelationshipStatus { get; set; }
		/// <summary>
		/// Status type for the User 
		/// </summary>
		[ChildView]
		[AddOperationParameter]
		public TStatusType Status { get; set; }
	}

	/// <summary>
	/// User for Add operation 
	/// </summary>
	public class UserForAddBase : User<PartyTypeKey, OrganizationKey, StatusTypeKey>, IUserForAdd
	{
		/// <summary>
		/// Overridden to return User Party Type Key
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
		/// <summary>
		/// User password
		/// </summary>
		public string Password { get; set; }
		/// <summary>
		/// Password Hash
		/// </summary>
		/// <remarks>Is Not Serialized</remarks>
		[AddOperationParameter]
		[JsonIgnore]
		public string PasswordHash { get; set; }
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

	public class UserForAdd : UserForAddBase
	{
		/// <summary>
		/// Overridden to return User Party Guid
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
	/// User for Update operation 
	/// </summary>
	public class UserForUpdate : User<PartyTypeKey, OrganizationKey>, IUserForUpdate
	{
		/// <summary>
		/// Overridden to return User Party Type Key
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
	/// User Result 
	/// </summary>
	public class UserResult : User<PartyTypeResult, OrganizationResult, StatusTypeResult>, IUserResult 
	{
		public PrimaryPartyAddress PrimaryAddress { get; set; }
		public PrimaryPartyPhone PrimaryPhone { get; set; }
		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
