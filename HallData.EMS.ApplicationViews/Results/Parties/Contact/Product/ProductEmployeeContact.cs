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
	public abstract class ProductEmployeeContactKey<TKey> : EmployeeContactKey<TKey>, IProductEmployeeContactKey<TKey>
		where TKey: IProductEmployeeContactId
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? ProductGuid { get; set; }
	}
	public class ProductEmployeeContactKeyBase : ProductEmployeeContactKey<ProductEmployeeContactId>
	{

		public override ProductEmployeeContactId Key
		{
			get
			{
				return new ProductEmployeeContactId(this.ProductGuid ?? Guid.Empty, this.PartyGuid ?? Guid.Empty, this.ContactRoleName, this.EmployerGuid);
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
	public class ProductEmployeeContactKeyForProduct : ProductEmployeeContactKeyBase
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
	public class ProductEmployeeContactKeyForParty : ProductEmployeeContactKeyBase
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
	public class ProductEmployeeContactKey : ProductEmployeeContactKeyBase
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
	public class ProductEmployeeContactKeyForEmployer : ProductEmployeeContactKey
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
	public abstract class ProductEmployeeContactBase<TKey, TPartyType, TContactType, TTitleType> : EmployeeContact<TKey, TPartyType, TContactType, TTitleType>, 
		IProductEmployeeContactBase<TKey, TPartyType, TContactType, TTitleType>
		where TKey : IProductEmployeeContactId
		where TPartyType : PartyTypeKey
		where TContactType : ContactTypeKey 
		where TTitleType: TitleTypeKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual Guid? ProductGuid { get; set; }
	}
	public class ProductEmployeeContact<TPartyType, TContactType, TTitleType> : ProductEmployeeContactBase<ProductEmployeeContactId, TPartyType, TContactType, TTitleType>,
		IProductEmployeeContact<TPartyType, TContactType, TTitleType>
		where TPartyType : PartyTypeKey
		where TContactType : ContactTypeKey
		where TTitleType : TitleTypeKey
	{
		public override ProductEmployeeContactId Key
		{
			get
			{
				return new ProductEmployeeContactId(this.ProductGuid ?? Guid.Empty, this.PartyGuid ?? Guid.Empty, this.ContactRoleName, this.EmployerGuid);
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
	public class ProductEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType> : ProductEmployeeContact<TPartyType, TContactType, TTitleType>, 
		IProductEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType>
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
	public class ProductEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType, TRole, TProduct, TEmployer> : 
		ProductEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType>,
		IProductEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType, TRole, TProduct, TEmployer>
		where TPartyType : PartyTypeKey
		where TStatusType : StatusTypeKey
		where TContactType : ContactTypeKey
		where TRole : RoleKey
		where TProduct : IProductBaseKey
		where TEmployer : IOrganizationKey
		where TTitleType : TitleTypeKey
	{
		[ChildView]
		public virtual TRole ContactRole { get; set; }
		[ChildView]
		public virtual TEmployer Employer { get; set; }
		[ChildView]
		public virtual TProduct Product { get; set; }
	}
	public class ProductEmployeeContactForAddRelationshipBase : ProductEmployeeContactKeyBase, IProductEmployeeContactForAddRelationship
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
	public class ProductEmployeeContactForAddRelationshipProduct : ProductEmployeeContactForAddRelationshipBase
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
	public class ProductEmployeeContactForAddRelationshipParty : ProductEmployeeContactForAddRelationshipBase
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
	public class ProductEmployeeContactForAddRelationship : ProductEmployeeContactForAddRelationshipBase
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
	public class ProductEmployeeContactForAddBase : ProductEmployeeContact<PartyTypeKey, ContactTypeKey, TitleTypeKey, StatusTypeKey>, IProductEmployeeContactForAdd
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
	public class ProductEmployeeContactForAddProduct : ProductEmployeeContactForAddBase
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
	public class ProductEmployeeContactForAdd : ProductEmployeeContactForAddBase
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
	public class ProductEmployeeContactForUpdateRelationshipBase : ProductEmployeeContactKeyBase, IProductEmployeeContactForUpdateRelationship
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
	public class ProductEmployeeContactForUpdateRelationshipProduct : ProductEmployeeContactForUpdateRelationshipBase
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
	public class ProductEmployeeContactForUpdateRelationshipEmployee : ProductEmployeeContactForUpdateRelationshipBase
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
	public class ProductEmployeeContactForUpdateRelationship : ProductEmployeeContactForUpdateRelationshipBase
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
	public class ProductEmployeeContactForUpdateRelationshipEmployer : ProductEmployeeContactForUpdateRelationship
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
	public class ProductEmployeeContactForUpdateBase : ProductEmployeeContact<PartyTypeKey, ContactTypeKey, TitleTypeKey>, IProductEmployeeContactForUpdate
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
	public class ProductEmployeeContactForUpdateProduct : ProductEmployeeContactForUpdateBase
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
	public class ProductEmployeeContactForUpdateEmployee : ProductEmployeeContactForUpdateBase
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
	public class ProductEmployeeContactForUpdate : ProductEmployeeContactForUpdateBase
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
	public class ProductEmployeeContactForUpdateEmployer : ProductEmployeeContactForUpdate
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
	public class ProductEmployeeContactResult : ProductEmployeeContact<PartyTypeResult, ContactType, TitleTypeName, StatusTypeResult, RoleResult, ProductResult, OrganizationResult>,
		IProductEmployeeContactResult
	{
		public PrimaryPartyAddress PrimaryAddress { get; set; }

		public PrimaryPartyPhone PrimaryPhone { get; set; }

		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
