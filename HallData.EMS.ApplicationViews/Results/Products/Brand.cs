using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using Newtonsoft.Json;
using HallData.Validation;
using HallData.EMS.ApplicationViews.Results;
using System.ComponentModel.DataAnnotations;
using HallData.EMS.ApplicationViews.Enums;

namespace HallData.EMS.ApplicationViews
{
	public class BrandKey : ProductBaseKey, IBrandKey
	{

	}

	public class Brand<TProductType> : ProductBase<TProductType>, IBrand<TProductType>
		where TProductType: ProductTypeKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		public virtual DateTime? StartDate { get; set; }
		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			foreach (var valid in base.Validate(validationContext))
				yield return valid;
			if (this.ProductGuid == null && this.StartDate == null)
				yield return ValidationResultFactory.Create(new ValidationResult("Start Date is required"), "BRAND_STARTDATE_REQUIRED");
		}
	}
	public class Brand<TProductType, TOwner> : Brand<TProductType>, IBrandWithOwner<TProductType, TOwner>
		where TProductType: ProductTypeKey
		where TOwner : IOrganizationKey
	{
		[AddOperationParameter]
		[ChildView]
		public virtual TOwner Owner { get; set; }
	}

	public class Brand<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator> : Brand<TProductType, TOwner>, IBrand<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator>
		where TProductType : ProductTypeKey
		where TStatusType : StatusTypeKey
		where TOwner : IOrganizationKey
		where TBusinessUnit : IBusinessUnitKey
		where TCreator : IUserKey
	{
		[ChildView]
		public virtual TStatusType Status { get; set; }
		[ChildView]
		public virtual TBusinessUnit BusinessUnit { get; set; }
		[ChildView]
		public virtual TCreator Creator { get; set; }
		[ChildView]
		public virtual DateTime? CreateDate { get; set; }
	}

	public class BrandForAddBase : Brand<ProductTypeKey, OrganizationKey>, IBrandForAdd
	{

		[AddOperationParameter]
		[ChildView]
		public PrimaryProductAddressForAddUpdate PrimaryAddress { get; set; }
		[AddOperationParameter]
		[ChildView]
		public PrimaryProductPhoneForAddUpdate PrimaryPhone { get; set; }
		[AddOperationParameter]
		[ChildView]
		public PrimaryProductEmailForAddUpdate PrimaryEmail { get; set; }

		[JsonIgnore]
		public override ProductTypeKey ProductType
		{
			get
			{
				return new ProductTypeKey() { ProductTypeId = (int)Enums.ProductTypes.Brand };
			}
			set
			{
			}
		}
	}

	public class BrandForAdd : BrandForAddBase
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

	public class BrandForUpdateBase : Brand<ProductTypeKey>, IBrandForUpdate
	{

		[UpdateOperationParameter]
		[ChildView]
		public PrimaryProductAddressForAddUpdate PrimaryAddress { get; set; }
		[UpdateOperationParameter]
		[ChildView]
		public PrimaryProductPhoneForAddUpdate PrimaryPhone { get; set; }
		[UpdateOperationParameter]
		[ChildView]
		public PrimaryProductEmailForAddUpdate PrimaryEmail { get; set; }

		[JsonIgnore]
		public override ProductTypeKey ProductType
		{
			get
			{
				return new ProductTypeKey() { ProductTypeId = (int)Enums.ProductTypes.Brand };
			}
			set
			{
			}
		}

	}

	public class BrandForUpdate : BrandForUpdateBase
	{
		[GlobalizedRequired("PRODUCT_GUID_REQUIRED")]
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

	public class BrandResult : Brand<ProductType, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>, IBrandResult
	{

		public DateTime? CreateDate { get; set; }

		public PrimaryProductAddress PrimaryAddress { get; set; }

		public PrimaryProductPhone PrimaryPhone { get; set; }

		public PrimaryProductEmail PrimaryEmail { get; set; }
	}
}
