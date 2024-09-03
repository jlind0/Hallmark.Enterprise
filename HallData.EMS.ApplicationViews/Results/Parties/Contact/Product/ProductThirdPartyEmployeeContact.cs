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
	public class ProductThirdPartyEmployeeContactKeyBase : ProductEmployeeContactKey<ProductThirdPartyEmployeeContactId>
	{

		public override ProductThirdPartyEmployeeContactId Key
		{
			get
			{
				return new ProductThirdPartyEmployeeContactId(this.ProductGuid ?? Guid.Empty, this.PartyGuid ?? Guid.Empty, this.ContactRoleName, this.EmployerGuid);
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

	public class ProductThirdPartyEmployeeContactKeyForProduct : ProductThirdPartyEmployeeContactKeyBase
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

	public class ProductThirdPartyEmployeeContactKeyForParty : ProductThirdPartyEmployeeContactKeyBase
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

	public class ProductThirdPartyEmployeeContactKey : ProductThirdPartyEmployeeContactKeyBase
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

	public class ProductThirdPartyEmployeeContactKeyForEmployer : ProductThirdPartyEmployeeContactKey
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

	public class ProductThirdPartyEmployeeContact<TPartyType, TContactType, TTitleType> : ProductEmployeeContactBase<ProductThirdPartyEmployeeContactId, TPartyType, TContactType, TTitleType>,
		IProductThirdPartyEmployeeContact<TPartyType, TContactType, TTitleType>
		where TPartyType : PartyTypeKey
		where TContactType : ContactTypeKey
		where TTitleType : TitleTypeKey
	{
		public override ProductThirdPartyEmployeeContactId Key
		{
			get
			{
				return new ProductThirdPartyEmployeeContactId(this.ProductGuid ?? Guid.Empty, this.PartyGuid ?? Guid.Empty, this.ContactRoleName, this.EmployerGuid);
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

	public class ProductThirdPartyEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType> : ProductThirdPartyEmployeeContact<TPartyType, TContactType, TTitleType>,
		IProductThirdPartyEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType>
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

	public class ProductThirdPartyEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType, TRole, TProduct, TEmployer> :
		ProductThirdPartyEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType>,
		IProductThirdPartyEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType, TRole, TProduct, TEmployer>
		where TPartyType : PartyTypeKey
		where TStatusType : StatusTypeKey
		where TContactType : ContactTypeKey
		where TRole : RoleKey
		where TProduct : IProductBaseKey
		where TEmployer : IThirdPartyKey
		where TTitleType : TitleTypeKey
	{
		[ChildView]
		public virtual TRole ContactRole { get; set; }
		[ChildView]
		public virtual TEmployer Employer { get; set; }
		[ChildView]
		public virtual TProduct Product { get; set; }
	}

	public class ProductThirdPartyEmployeeContactForAddRelationshipBase : ProductThirdPartyEmployeeContactKey, IProductThirdPartyEmployeeContactForAddRelationship
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

	public class ProductThirdPartyEmployeeContactForAddRelationshipProduct : ProductThirdPartyEmployeeContactForAddRelationshipBase
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

	public class ProductThirdPartyEmployeeContactForAddRelationshipParty : ProductThirdPartyEmployeeContactForAddRelationshipBase
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

	public class ProductThirdPartyEmployeeContactForAddRelationship : ProductThirdPartyEmployeeContactForAddRelationshipBase
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

	public class ProductThirdPartyEmployeeContactForAddBase : ProductThirdPartyEmployeeContact<PartyTypeKey, ContactTypeKey, TitleTypeKey, StatusTypeKey>, IProductThirdPartyEmployeeContactForAdd
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

	public class ProductThirdPartyEmployeeContactForAddProduct : ProductThirdPartyEmployeeContactForAddBase
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

	public class ProductThirdPartyEmployeeContactForAdd : ProductThirdPartyEmployeeContactForAddBase
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

	public class ProductThirdPartyEmployeeContactForUpdateRelationshipBase : ProductThirdPartyEmployeeContactKeyBase, IProductThirdPartyEmployeeContactForUpdateRelationship
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

	public class ProductThirdPartyEmployeeContactForUpdateRelationshipProduct : ProductThirdPartyEmployeeContactForUpdateRelationshipBase
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

	public class ProductThirdPartyEmployeeContactForUpdateRelationshipEmployee : ProductThirdPartyEmployeeContactForUpdateRelationshipBase
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

	public class ProductThirdPartyEmployeeContactForUpdateRelationship : ProductThirdPartyEmployeeContactForUpdateRelationshipBase
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

	public class ProductThirdPartyEmployeeContactForUpdateRelationshipEmployer : ProductThirdPartyEmployeeContactForUpdateRelationship
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

	public class ProductThirdPartyEmployeeContactForUpdateBase : ProductThirdPartyEmployeeContact<PartyTypeKey, ContactTypeKey, TitleTypeKey>, IProductThirdPartyEmployeeContactForUpdate
	{
		public Merge<PartyContactMechanismForAddRelationshipParty, PartyContactMechanismForUpdateRelationshipParty, PartyContactMechanismDeleteKeyParty, PartyContactMechanismForMergeRelationship> ContactMechanismRelationships { get; set; }
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

	public class ProductThirdPartyEmployeeContactForUpdateProduct : ProductThirdPartyEmployeeContactForUpdateBase
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

	public class ProductThirdPartyEmployeeContactForUpdateEmployee : ProductThirdPartyEmployeeContactForUpdateBase
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

	public class ProductThirdPartyEmployeeContactForUpdate : ProductThirdPartyEmployeeContactForUpdateBase
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

	public class ProductThirdPartyEmployeeContactForUpdateEmployer : ProductThirdPartyEmployeeContactForUpdate
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

	public class ProductThirdPartyEmployeeContactResult : ProductThirdPartyEmployeeContact<PartyTypeResult, ContactType, TitleTypeName, StatusTypeResult, RoleResult, ProductResult, ThirdPartyResult>,
		IProductThirdPartyEmployeeContactResult
	{
		public PrimaryPartyAddress PrimaryAddress { get; set; }

		public PrimaryPartyPhone PrimaryPhone { get; set; }

		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
