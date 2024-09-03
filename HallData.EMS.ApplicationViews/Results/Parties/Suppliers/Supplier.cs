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
	public class SupplierKeyBase : OrganizationKey<SupplierId>, ISupplierKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? SuppliedPartyGuid { get; set; }
		public override SupplierId Key
		{
			get
			{
				return new SupplierId(this.PartyGuid ?? Guid.Empty, this.SuppliedPartyGuid ?? Guid.Empty);
			}
			set
			{
				this.SuppliedPartyGuid = value.SuppliedPartyGuid;
				this.PartyGuid = value.PartyGuid;
			}
		}
		
	}

	public class SupplierKeyForSupplier : SupplierKeyBase
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
		[GlobalizedRequired("SUPPLIER_SUPPLIEDGUID_REQUIRED")]
		public override Guid? SuppliedPartyGuid
		{
			get
			{
				return base.SuppliedPartyGuid;
			}
			set
			{
				base.SuppliedPartyGuid = value;
			}
		}
	}

	public class SupplierKeyForSupplied : SupplierKeyBase
	{
		[GlobalizedRequired("SUPPLIER_PARTYGUID_REQUIRED")]
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
		public override Guid? SuppliedPartyGuid
		{
			get
			{
				return base.SuppliedPartyGuid;
			}
			set
			{
				base.SuppliedPartyGuid = value;
			}
		}
	}

	public class SupplierKey : SupplierKeyBase
	{
		[GlobalizedRequired("SUPPLIER_PARTYGUID_REQUIRED")]
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
		[GlobalizedRequired("SUPPLIER_SUPPLIEDGUID_REQUIRED")]
		public override Guid? SuppliedPartyGuid
		{
			get
			{
				return base.SuppliedPartyGuid;
			}
			set
			{
				base.SuppliedPartyGuid = value;
			}
		}
	}

	public class Supplier<TPartyType, TTierType> : Organization<SupplierId, TPartyType, TTierType>, ISupplier<TPartyType, TTierType>
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? SuppliedPartyGuid { get; set; }
		public override SupplierId Key
		{
			get
			{
				return new SupplierId(this.PartyGuid ?? Guid.Empty, this.SuppliedPartyGuid ?? Guid.Empty);
			}
			set
			{
				this.SuppliedPartyGuid = value.SuppliedPartyGuid;
				this.PartyGuid = value.PartyGuid;
			}
		}
	}

	public class Supplier<TPartyType, TTierType, TStatusType> : Supplier<TPartyType, TTierType>, ISupplier<TPartyType, TTierType, TStatusType>
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
		where TStatusType : StatusTypeKey
	{
		[ChildView]
		[AddOperationParameter]
		public TStatusType SuppliedRelationshipStatus { get; set; }
		[ChildView]
		[AddOperationParameter]
		public TStatusType Status { get; set; }
	}

	public class Supplier<TPartyType, TTierType, TStatusType, TProvided> : Supplier<TPartyType, TTierType, TStatusType>, ISupplier<TPartyType, TTierType, TStatusType, TProvided>
		where TPartyType : PartyTypeKey
		where TTierType : TierTypeKey
		where TStatusType : StatusTypeKey
		where TProvided : IOrganizationKey
	{
		[ChildView]
		public TProvided SuppliedOrganization { get; set; }
	}

	public class SupplierResult : Supplier<PartyTypeResult, TierType, StatusTypeResult, OrganizationResult>, ISupplierResult
	{
		public PrimaryPartyAddress PrimaryAddress { get; set; }

		public PrimaryPartyPhone PrimaryPhone { get; set; }

		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
