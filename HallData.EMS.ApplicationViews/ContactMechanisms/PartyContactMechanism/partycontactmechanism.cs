using System;
using System.Collections.Generic;
using HallData.ApplicationViews;
using Newtonsoft.Json;
using HallData.Validation;
using System.ComponentModel.DataAnnotations;
using HallData.Utilities;
using HallData.EMS.ApplicationViews.Results;

namespace HallData.EMS.ApplicationViews
{
	public class PartyContactMechanismKey : IContactMechanismHolderKey<PartyContactMechanismId>
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? PartyGuid { get; set; }
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? ContactMechanismGuid { get; set; }
		[JsonIgnore]
		public PartyContactMechanismId Key
		{
			get
			{
				return new PartyContactMechanismId(this.ContactMechanismGuid, this.PartyGuid ?? Guid.Empty, this.ContactMechanismTypeName);
			}
			set
			{
				this.PartyGuid = value.PartyGuid;
				this.ContactMechanismGuid = value.ContactMechanismGuid;
			}
		}

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		[GlobalizedRequired("PARTYCONTACTMECHANISM_CONTACTMECHANISMTYPE_REQUIRED")]
		public string ContactMechanismTypeName { get; set; }
	}

	public class PartyContactMechanismDeleteKeyParty : PartyContactMechanismKey
	{
		[GlobalizedRequired("PARTYCONTACTMECHANISM_CONTACTMECHANISMGUID_REQUIRED")]
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

	public interface IPartyContactMechanism
	{
		int? OrderIndex { get; set; }
		string Attention { get; set; }
	}

	public class PartyContactMechanism : PartyContactMechanismKey, IPartyContactMechanism
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual int? OrderIndex { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual string Attention { get; set; }
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual bool? IsPrimary { get; set; }
	}

	public class PartyContactMechanism<TContactMechanism, TMechanismType> : PartyContactMechanism,
		IContactMechanismHolder<PartyContactMechanismId, TContactMechanism, TMechanismType>
		where TContactMechanism: IContactMechanism<TMechanismType>
		where TMechanismType: MechanismTypeKey
	{
		[ChildView]
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual TContactMechanism ContactMechanism { get; set; }
	}

	public class PartyContactMechanism<TContactMechanism, TMechanismType, TStatusType> :
		PartyContactMechanism<TContactMechanism, TMechanismType>, IContactMechanismHolder<PartyContactMechanismId, TContactMechanism, TMechanismType, TStatusType>
		where TContactMechanism: IContactMechanism<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType: StatusTypeKey
	{
		[AddOperationParameter]
		[ChildView]
		public virtual TStatusType Status { get; set; }
	}

	public class PartyContactMechanismForAddBase<TContactMechanism> : PartyContactMechanism<TContactMechanism, MechanismTypeKey, StatusTypeKey>, IContactMechanismHolderForAdd<PartyContactMechanismId, TContactMechanism>
		where TContactMechanism: IContactMechanismForAdd
	{
		 [GlobalizedRequired("PARTYCONTACTMECHANISM_CONTACTMECHANISM_REQUIRED")]
		public override TContactMechanism ContactMechanism
		{
			get
			{
				return base.ContactMechanism;
			}
			set
			{
				base.ContactMechanism = value;
			}
		}
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
	}

	public class PartyContactMechanismForAdd<TContactMechanism> : PartyContactMechanismForAddBase<TContactMechanism>
		where TContactMechanism: IContactMechanismForAdd
	{
		[GlobalizedRequired("PARTYCONTACTMECHANISM_PARTYGUID_REQUIRED")]
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

	public class PartyContactMechanismForAddRelationshipBase : PartyContactMechanism, IContactMechanismHolderForAddRelationship<PartyContactMechanismId>
	{
		[AddOperationParameter]
		[ChildView]
		public virtual StatusTypeKey Status { get; set; }
		
		[GlobalizedRequired("PARTYCONTACTMECHANISM_CONTACTMECHANISMGUID_REQUIRED")]
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
	}

	public class PartyContactMechanismForAddRelationship : PartyContactMechanismForAddRelationshipBase
	{
		[GlobalizedRequired("PARTYCONTACTMECHANISM_PARTYGUID_REQUIRED")]
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

	public class PartyContactMechanismForAddRelationshipParty : PartyContactMechanismForAddRelationshipBase
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

	public class PartyContactMechanismForUpdateBase<TContactMechanism> : PartyContactMechanism<TContactMechanism, MechanismTypeKey>, IContactMechanismHolderForUpdate<PartyContactMechanismId, TContactMechanism>
		where TContactMechanism : IContactMechanismForUpdate
	{
		[GlobalizedRequired("PARTYCONTACTMECHANISM_CONTACTMECHANISMGUID_REQUIRED")]
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
	}

	public class PartyContactMechanismForUpdateParty<TContactMechanism> : PartyContactMechanismForUpdateBase<TContactMechanism>
		where TContactMechanism : IContactMechanismForUpdate
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

	public class PartyContactMechanismForUpdate<TContactMechanism> : PartyContactMechanismForUpdateBase<TContactMechanism>
		where TContactMechanism: IContactMechanismForUpdate
	{
		[GlobalizedRequired("PARTYCONTACTMECHANISM_PARTYGUID_REQUIRED")]
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

	public class PartyContactMechanismForUpdateRelationshipBase : PartyContactMechanism, IContactMechanismHolderForUpdateRelationship<PartyContactMechanismId>
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public Guid? OriginalContactMechanismGuid { get; set; }

		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public string OriginalContactMechanismTypeName { get; set; }
	}

	public class PartyContactMechanismForUpdateRelationshipParty : PartyContactMechanismForUpdateRelationshipBase
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

	public class PartyContactMechanismForUpdateRelationship : PartyContactMechanismForUpdateRelationshipBase
	{
		[GlobalizedRequired("PARTYCONTACTMECHANISM_PARTYGUID_REQUIRED")]
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

	public class PartyContactMechanismForMerge<TContactMechanism> : PartyContactMechanismForAddBase<TContactMechanism>, IContactMechanismHolderForMerge<PartyContactMechanismId, TContactMechanism>
		where TContactMechanism: IContactMechanismForAdd
	{
		public MergeActions MergeAction { get; set; }
	}

	public class PartyContactMechanismForMergeRelationship : PartyContactMechanismForUpdateRelationshipBase, IContactMechanismHolderForMergeRelationship<PartyContactMechanismId>
	{

		public StatusTypeKey Status { get; set; }

		public MergeActions MergeAction { get; set; }
	}

	public interface IPartyContactMechanismResult : IPartyContactMechanism
	{
		DateTime? CreateDate { get; set; }
		DateTime? UpdateDate { get; set; }
	}

	[ContactMechanismHolderDefaultView("PartyContactMechanism{0}Result")]
	public class PartyContactMechanismResult<TContactMechanism> : PartyContactMechanism<TContactMechanism, MechanismTypeResult, StatusTypeResult>, IContactMechanismHolderResult<PartyContactMechanismId, TContactMechanism>, IPartyContactMechanismResult
		where TContactMechanism: IContactMechanismResult
	{
		public DateTime? CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }

		public ContactMechanismType ContactMechanismType { get; set; }
	}

	public struct PartyContactMechanismId : IContactMechanismHolderId<Guid>, IPartyId
	{
		public Guid ContactMechanismGuid { get; set; }
		public Guid PartyGuid { get; set; }
		public PartyContactMechanismId(Guid? contactMechanismId, Guid partyId, string contactMechanismTypeName)
			: this()
		{
			this.ContactMechanismGuid = contactMechanismId ?? Guid.Empty;
			this.PartyGuid = partyId;
			this.ContactMechanismTypeName = ContactMechanismTypeName ?? string.Empty;
		}

		public override bool Equals(object obj)
		{
			PartyContactMechanismId? mech = obj as PartyContactMechanismId?;
			if (mech == null)
				return false;
			return mech.Value.ContactMechanismGuid == this.ContactMechanismGuid && mech.Value.PartyGuid == this.PartyGuid && mech.Value.ContactMechanismTypeName == this.ContactMechanismTypeName;
		}

		public override int GetHashCode()
		{
			return HashCodeProvider.BuildHashCode(this.PartyGuid, this.ContactMechanismGuid, this.ContactMechanismTypeName);
		}

		public Guid Id
		{
			get
			{
				return this.PartyGuid;
			}
			set
			{
				this.PartyGuid = value;
			}
		}

		public string ContactMechanismTypeName { get; set; }
	}
}
