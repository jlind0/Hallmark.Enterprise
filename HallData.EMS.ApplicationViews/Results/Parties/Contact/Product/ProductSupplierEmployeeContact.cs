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
	public class ProductSupplierEmployeeContactKeyBase : ProductEmployeeContactKey<ProductSupplierEmployeeContactId>
	{

		public override ProductSupplierEmployeeContactId Key
		{
			get
			{
				return new ProductSupplierEmployeeContactId(this.ProductGuid ?? Guid.Empty, this.PartyGuid ?? Guid.Empty, this.ContactRoleName, this.EmployerGuid);
			}
			set
			{
				this.ProductGuid = value.PartyGuid;
				this.PartyGuid = value.PartyGuid;
				this.ContactRoleName = value.ContactRoleName;
				this.EmployerGuid = value.EmployerGuid;
			}
		}
	}
	public class ProductSupplierEmployeeContactKeyForProduct : ProductSupplierEmployeeContactKeyBase
	{
		[JsonIgnore]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
		[GlobalizedRequired("CONTACT_PARTY_REQUIRED")]
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
	public class ProductSupplierEmployeeContactKeyForParty : ProductSupplierEmployeeContactKeyBase
	{
		[GlobalizedRequired("PRODUCTCONTACT_PRODUCT_REQUIRED")]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
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
	public class ProductSupplierEmployeeContactKey : ProductSupplierEmployeeContactKeyBase
	{
		[GlobalizedRequired("CONTACT_PARTY_REQUIRED")]
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
		[GlobalizedRequired("PRODUCTCONTACT_PRODUCT_REQUIRED")]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
	}
	public class ProductSupplierEmployeeContactKeyForEmployer : ProductSupplierEmployeeContactKey
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
	public class ProductSupplierEmployeeContact<TPartyType, TContactType, TTitleType> : ProductEmployeeContactBase<ProductSupplierEmployeeContactId, TPartyType, TContactType, TTitleType>,
		IProductSupplierEmployeeContact<TPartyType, TContactType, TTitleType>
		where TPartyType : PartyTypeKey
		where TContactType : ContactTypeKey
		where TTitleType : TitleTypeKey
	{
		public override ProductSupplierEmployeeContactId Key
		{
			get
			{
				return new ProductSupplierEmployeeContactId(this.ProductGuid ?? Guid.Empty, this.PartyGuid ?? Guid.Empty, this.ContactRoleName, this.EmployerGuid);
			}
			set
			{
				this.ProductGuid = value.PartyGuid;
				this.PartyGuid = value.PartyGuid;
				this.ContactRoleName = value.ContactRoleName;
				this.EmployerGuid = value.EmployerGuid;
			}
		}
	}
	public class ProductSupplierEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType> : ProductSupplierEmployeeContact<TPartyType, TContactType, TTitleType>,
		IProductSupplierEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType>
		where TPartyType : PartyTypeKey
		where TStatusType : StatusTypeKey
		where TContactType : ContactTypeKey
		where TTitleType : TitleTypeKey
	{
		[AddOperationParameter]
		[ChildView]
		public virtual TStatusType ContactRelationshipStatus { get; set; }
		[AddOperationParameter]
		[ChildView]
		public virtual TStatusType Status { get; set; }
		[AddOperationParameter]
		[ChildView]
		public virtual TStatusType EmployeeRelationshipStatus { get; set; }
	}
	public class ProductSupplierEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType, TRole, TProduct, TEmployer> :
		ProductSupplierEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType>,
		IProductSupplierEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType, TRole, TProduct, TEmployer>
		where TPartyType : PartyTypeKey
		where TStatusType : StatusTypeKey
		where TContactType : ContactTypeKey
		where TRole : RoleKey
		where TProduct : IProductBaseKey
		where TEmployer : ISupplierKey
		where TTitleType : TitleTypeKey
	{
		[ChildView]
		public virtual TRole ContactRole { get; set; }
		[ChildView]
		public virtual TEmployer Employer { get; set; }
		[ChildView]
		public virtual TProduct Product { get; set; }
	}
	public class ProductSupplierEmployeeContactForAddRelationshipBase : ProductSupplierEmployeeContactKey, IProductSupplierEmployeeContactForAddRelationship
	{
		[AddOperationParameter]
		[ChildView]
		[GlobalizedRequired("CONTACT_CONTACTTYPE_REQUIRED")]
		public ContactTypeKey ContactType { get; set; }
		[AddOperationParameter]
		public string JobTitle { get; set; }
		[AddOperationParameter]
		[ChildView]
		public StatusTypeKey ContactRelationshipStatus { get; set; }
	}
	public class ProductSupplierEmployeeContactForAddRelationshipProduct : ProductSupplierEmployeeContactForAddRelationshipBase
	{
		[JsonIgnore]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
		[GlobalizedRequired("CONTACT_PARTY_REQUIRED")]
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
	public class ProductSupplierEmployeeContactForAddRelationshipParty : ProductSupplierEmployeeContactForAddRelationshipBase
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
		[GlobalizedRequired("PRODUCTCONTACT_PRODUCT_REQUIRED")]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
	}
	public class ProductSupplierEmployeeContactForAddRelationship : ProductSupplierEmployeeContactForAddRelationshipBase
	{
		[GlobalizedRequired("PRODUCTCONTACT_PRODUCT_REQUIRED")]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
		[GlobalizedRequired("CONTACT_PARTY_REQUIRED")]
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
	public class ProductSupplierEmployeeContactForAddBase : ProductSupplierEmployeeContact<PartyTypeKey, ContactTypeKey, TitleTypeKey, StatusTypeKey>, IProductSupplierEmployeeContactForAdd
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
	public class ProductSupplierEmployeeContactForAddProduct : ProductSupplierEmployeeContactForAddBase
	{
		[JsonIgnore]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
	}
	public class ProductSupplierEmployeeContactForAdd : ProductSupplierEmployeeContactForAddBase
	{
		[GlobalizedRequired("PRODUCTCONTACT_PRODUCT_REQUIRED")]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
	}
	public class ProductSupplierEmployeeContactForUpdateRelationshipBase : ProductSupplierEmployeeContactKeyBase, IProductSupplierEmployeeContactForUpdateRelationship
	{
		[UpdateOperationParameter]
		[ChildView]
		public virtual ContactTypeKey ContactType { get; set; }
		[UpdateOperationParameter]
		public virtual string JobTitle { get; set; }
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? OriginalProductGuid { get; set; }
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual string OriginalContactRoleName { get; set; }
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? OriginalEmployerGuid { get; set; }
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? OriginalPartyGuid { get; set; }
	}
	public class ProductSupplierEmployeeContactForUpdateRelationshipProduct : ProductSupplierEmployeeContactForUpdateRelationshipBase
	{
		[GlobalizedRequired("CONTACT_PARTY_REQUIRED")]
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
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
		[JsonIgnore]
		public override Guid? OriginalProductGuid
		{
			get
			{
				return base.OriginalProductGuid;
			}
			set
			{
				base.OriginalProductGuid = value;
			}
		}
	}
	public class ProductSupplierEmployeeContactForUpdateRelationshipEmployee : ProductSupplierEmployeeContactForUpdateRelationshipBase
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
		[JsonIgnore]
		public override Guid? OriginalPartyGuid
		{
			get
			{
				return base.OriginalProductGuid;
			}
			set
			{
				base.OriginalProductGuid = value;
			}
		}
		[GlobalizedRequired("PRODUCTCONTACT_PRODUCT_REQUIRED")]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
	}
	public class ProductSupplierEmployeeContactForUpdateRelationship : ProductSupplierEmployeeContactForUpdateRelationshipBase
	{
		[GlobalizedRequired("PRODUCTCONTACT_PRODUCT_REQUIRED")]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
		[GlobalizedRequired("CONTACT_PARTY_REQUIRED")]
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
	public class ProductSupplierEmployeeContactForUpdateRelationshipEmployer : ProductSupplierEmployeeContactForUpdateRelationship
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
	}
	public class ProductSupplierEmployeeContactForUpdateBase : ProductSupplierEmployeeContact<PartyTypeKey, ContactTypeKey, TitleTypeKey>, IProductSupplierEmployeeContactForUpdate
	{
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
	public class ProductSupplierEmployeeContactForUpdateProduct : ProductSupplierEmployeeContactForUpdateBase
	{
		[JsonIgnore]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
		[GlobalizedRequired("CONTACT_PARTY_REQUIRED")]
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
	public class ProductSupplierEmployeeContactForUpdateEmployee : ProductSupplierEmployeeContactForUpdateBase
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
		[GlobalizedRequired("PRODUCTCONTACT_PRODUCT_REQUIRED")]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
	}
	public class ProductSupplierEmployeeContactForUpdate : ProductSupplierEmployeeContactForUpdateBase
	{
		[GlobalizedRequired("PRODUCTCONTACT_PRODUCT_REQUIRED")]
		public override Guid? ProductGuid
		{
			get
			{
				return base.ProductGuid;
			}
			set
			{
				base.ProductGuid = value;
			}
		}
		[GlobalizedRequired("CONTACT_PARTY_REQUIRED")]
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
	public class ProductSupplierEmployeeContactForUpdateEmployer : ProductSupplierEmployeeContactForUpdate
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
	public class ProductSupplierEmployeeContactResult : ProductSupplierEmployeeContact<PartyTypeResult, ContactType, TitleTypeName, StatusTypeResult, RoleResult, ProductResult, SupplierResult>,
		IProductSupplierEmployeeContactResult
	{
		public PrimaryPartyAddress PrimaryAddress { get; set; }

		public PrimaryPartyPhone PrimaryPhone { get; set; }

		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
