using System;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews.Results
{
	//TODO: JLIND - which fields will never be exposed to Add/Update
	//public abstract class PartyCategoryKey<TKey> : CategoryKey<TKey>, IPartyCategoryKey
	public abstract class PartyCategoryKey : IPartyCategoryKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual Guid? PartyGuid { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("PARTY_CATEGORY_ID_REQUIRED")]
		public virtual int? Id { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[GlobalizedRequired("ROLE_ID_REQUIRED")]
		public virtual int? RoleId { get; set; }

		[JsonIgnore]
		public virtual PartyCategoryId Key { get; set; }
	}

	public class PartyCategory : PartyCategoryKey, IPartyCategory
	{
		public PartyCategory()
		{
			//StartDate = DateTime.UtcNow;
		}

		[AddOperationParameter]
		[UpdateOperationParameter]
		public int? OrderIndex { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? IsDefault { get; set; }
	}

	public class PartyCategory<TStatusType> : PartyCategory, IPartyCategory<TStatusType>
		where TStatusType : StatusTypeKey
	{
		[ChildView]
		[AddOperationParameter]
		public virtual TStatusType Status { get; set; }
	}

	public class PartyCategory<TStatusType, TParty, TCategory, TRole> :
		PartyCategory<TStatusType>, IPartyCategory<TStatusType, TParty, TCategory, TRole>
		where TStatusType : StatusTypeKey
		where TParty : IPartyKey
		where TCategory : ICategoryKey
		where TRole : RoleKey
	{
		public virtual TRole Role { get; set; }
		public virtual TParty Party { get; set; }
		public virtual TCategory Category { get; set; }
	}

	// ADD

	public class PartyCategoryForAddBase : PartyCategory<StatusTypeKey>, IPartyCategoryForAdd 
	{ }

	public class PartyCategoryForAddParty : PartyCategoryForAddBase
	{
		[JsonIgnore]
		public override Guid? PartyGuid { get; set; }
	}

	public class PartyCategoryForAdd : PartyCategoryForAddBase
	{
		[GlobalizedRequired("PARTY_GUID_REQUIRED")]
		public override Guid? PartyGuid { get; set; }
	}

	// UPDATE

	public class PartyCategoryForUpdateBase : PartyCategory, IPartyCategoryForUpdate 
	{ }

	public class PartyCategoryForUpdateParty : PartyCategoryForUpdateBase
	{
		[JsonIgnore]
		public virtual Guid PartyGuid { get; set; }
	}

	public class PartyCategoryForUpdate : PartyCategoryForUpdateBase
	{
		[GlobalizedRequired("PARTY_GUID_REQUIRED")]
		public virtual Guid PartyGuid { get; set; }
	}

	// RESULT

	public class PartyCategoryResult : PartyCategory<StatusTypeResult, PartyResult, CategoryResult, RoleResult>,
		IPartyCategoryResult
	{
	   public DateTime? StartDate { get; set; }
	   public DateTime? EndDate { get; set; }
	}

}
