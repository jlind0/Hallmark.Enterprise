using System;
using HallData.ApplicationViews;
using HallData.Utilities;

namespace HallData.EMS.ApplicationViews.Results
{

	public interface IPartyCategoryId
	{
		Guid PartyGuid { get; set; }
		int Id { get; set; }
		int RoleId { get; set; }
	}

	public struct PartyCategoryId : IPartyCategoryId
	{
		public PartyCategoryId(Guid partyGuid, int categoryId, int roleId) : this()
		{
			this.PartyGuid = partyGuid;
			this.Id = categoryId;
			this.RoleId = roleId;
		}

		public override int GetHashCode()
		{
			return HashCodeProvider.BuildHashCode(this.PartyGuid, this.Id, this.RoleId);
		}

		public override bool Equals(object obj)
		{
			IPartyCategoryId partyCategoryId = obj as IPartyCategoryId;

			if (partyCategoryId == null)
			{
				return false;
			}

			return partyCategoryId.PartyGuid == this.PartyGuid && partyCategoryId.Id == this.Id && partyCategoryId.RoleId == this.RoleId;
		}

		public Guid PartyGuid { get; set; }
		public int Id { get; set; }
		public int RoleId { get; set; }
	}

	public interface IPartyCategoryKey : IHasKey<PartyCategoryId>
	{
		Guid? PartyGuid { get; set; }
		int? Id { get; set; }
		int? RoleId { get; set; }
	}

	public interface IPartyCategory : IPartyCategoryKey
	{
		int? OrderIndex { get; set; }
		bool? IsDefault { get; set; }
	}

	public interface IPartyCategory<TStatusType> : IPartyCategory
		where TStatusType : StatusTypeKey
	{
		TStatusType Status { get; set; }
	}

	public interface IPartyCategory<TStatusType, TParty, TCategory, TRole> : IPartyCategory<TStatusType>
		where TStatusType : StatusTypeKey
		where TParty : IPartyKey
		where TCategory : ICategoryKey
		where TRole : RoleKey
	{
		TParty Party { get; set; }
		TCategory Category { get; set; }
		TRole Role { get; set; }
	}

	// ADD

	public interface IPartyCategoryForAdd : IPartyCategory<StatusTypeKey> { }

	// UPDATE
 
	public interface IPartyCategoryForUpdate : IPartyCategory {}

	// RESULT

	public interface IPartyCategoryResult : IPartyCategory<StatusTypeResult, PartyResult, CategoryResult, RoleResult>
	{
		DateTime? StartDate { get; set; }
		DateTime? EndDate { get; set; }
	}
}
