using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using Newtonsoft.Json;
using HallData.Validation;
using System.ComponentModel.DataAnnotations;

namespace HallData.EMS.ApplicationViews
{
	public class EventKey : BrandedProductKey, IEventKey{}

	public class Event<TProductType, TFrequency> : BrandedProduct<TProductType, TFrequency>, IEvent<TProductType, TFrequency>
		where TProductType: ProductTypeKey
		where TFrequency: FrequencyKey
	{

	}

	public class Event<TProductType, TFrequency, TBrand, TOwner> : Event<TProductType, TFrequency>, IEventWithOwner<TProductType, TFrequency, TBrand, TOwner>
		where TProductType : ProductTypeKey
		where TFrequency : FrequencyKey
		where TBrand : IBrandKey
		where TOwner : IOrganizationKey
	{
		[AddOperationParameter]
		[ChildView]
		public virtual TBrand Brand { get; set; }
		[AddOperationParameter]
		[ChildView]
		public virtual TOwner Owner { get; set; }
	}

	public class Event<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator> : Event<TProductType, TFrequency, TBrand, TOwner>,
		IEvent<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator>
		where TProductType : ProductTypeKey
		where TFrequency : FrequencyKey
		where TBrand : IBrandKey
		where TStatusType : StatusTypeKey
		where TOwner : IOrganizationKey
		where TBusinessUnit : IBusinessUnitKey
		where TCreator : IUserKey
	{
		[ChildView]
		public TStatusType Status { get; set; }
		[ChildView]
		public TBusinessUnit BusinessUnit { get; set; }
		[ChildView]
		public TCreator Creator { get; set; }

		public DateTime? CreateDate { get; set; }
	}

	public class EventForAddBase : Event<ProductTypeKey, FrequencyKey, BrandKey, OrganizationKey>, IEventForAdd
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

		[GlobalizedRequired("PRODUCT_STARTDATE_REQUIRED")]
		public override DateTime? StartDate
		{
			get
			{
				return base.StartDate;
			}
			set
			{
				base.StartDate = value;
			}
		}

		[JsonIgnore]
		public override ProductTypeKey ProductType
		{
			get
			{
				return new ProductTypeKey() { ProductTypeId = (int)Enums.ProductTypes.Event };
			}
			set
			{
			}
		}
	}

	public class EventForAddBrand : EventForAddBase
	{
		[JsonIgnore]
		public override BrandKey Brand
		{
			get
			{
				return base.Brand;
			}
			set
			{
				base.Brand = value;
			}
		}
	}

	public class EventForAdd : EventForAddBase
	{
	}

	public class EventForUpdate : Event<ProductTypeKey, FrequencyKey>, IEventForUpdate
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

		[GlobalizedRequired("PRODUCT_PRODUCTGUID_REQUIRED")]
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
		public override ProductTypeKey ProductType
		{
			get
			{
				return new ProductTypeKey() { ProductTypeId = (int)Enums.ProductTypes.Event };
			}
			set
			{
			}
		}
	}
	public class EventResult : Event<ProductType, Frequency, BrandResult, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>, IEventResult
	{
		public PrimaryProductAddress PrimaryAddress { get; set; }
		public PrimaryProductPhone PrimaryPhone { get; set; }
		public PrimaryProductEmail PrimaryEmail { get; set; }
	}
}
