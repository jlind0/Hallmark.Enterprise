using System;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using Newtonsoft.Json;
using HallData.Validation;

namespace HallData.EMS.ApplicationViews
{
    public class PublicationKey : BrandedProductKey, IPublicationKey { }

    public class Publication<TProductType, TFrequency> : BrandedProduct<TProductType, TFrequency>, IPublication<TProductType, TFrequency>
        where TProductType : ProductTypeKey
        where TFrequency : FrequencyKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? IsControlled { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? IsAudited { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public string AuditBureau { get; set; }
    }

    public class Publication<TProductType, TFrequency, TBrand, TOwner> : Publication<TProductType, TFrequency>, IPublicationWithOwner<TProductType, TFrequency, TBrand, TOwner>
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

    public class Publication<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator> : Publication<TProductType, TFrequency, TBrand, TOwner>,
        IPublication<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator>
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

    public class PublicationForAddBase : Publication<ProductTypeKey, FrequencyKey, BrandKey, OrganizationKey>, IPublicationForAdd
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
                return new ProductTypeKey() { ProductTypeId = (int)Enums.ProductTypes.Publication };
            }
            set
            {
            }
        }
    }

    public class PublicationForAddBrand : PublicationForAddBase
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

    public class PublicationForAdd : PublicationForAddBase { }

    public class PublicationForUpdate : Publication<ProductTypeKey, FrequencyKey>, IPublicationForUpdate
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
                return new ProductTypeKey() { ProductTypeId = (int)Enums.ProductTypes.Publication };
            }
            set
            {
            }
        }
    }
    public class PublicationResult : Publication<ProductType, Frequency, BrandResult, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>, IPublicationResult
    {
        public PrimaryProductAddress PrimaryAddress { get; set; }
        public PrimaryProductPhone PrimaryPhone { get; set; }
        public PrimaryProductEmail PrimaryEmail { get; set; }
    }
}
