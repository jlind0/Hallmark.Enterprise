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
    public class NewsletterKey : BrandedProductKey, INewsletterKey
    {

    }

    public class Newsletter<TProductType, TFrequency> : BrandedProduct<TProductType, TFrequency>, INewsletter<TProductType, TFrequency>
        where TProductType : ProductTypeKey
        where TFrequency : FrequencyKey
    {

    }

    public class Newsletter<TProductType, TFrequency, TBrand, TOwner> : Newsletter<TProductType, TFrequency>, INewsletterWithOwner<TProductType, TFrequency, TBrand, TOwner>
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

    public class Newsletter<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator> : Newsletter<TProductType, TFrequency, TBrand, TOwner>,
        INewsletter<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator>
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

    public class NewsletterForAddBase : Newsletter<ProductTypeKey, FrequencyKey, BrandKey, OrganizationKey>, INewsletterForAdd
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
                return new ProductTypeKey() { ProductTypeId = (int)Enums.ProductTypes.Newsletter };
            }
            set
            {
            }
        }
    }

    public class NewsletterForAddBrand : NewsletterForAddBase
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

    public class NewsletterForAdd : NewsletterForAddBase { }

    public class NewsletterForUpdate : Newsletter<ProductTypeKey, FrequencyKey>, INewsletterForUpdate
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
                return new ProductTypeKey() { ProductTypeId = (int)Enums.ProductTypes.Newsletter };
            }
            set
            {
            }
        }
    }
    public class NewsletterResult : Newsletter<ProductType, Frequency, BrandResult, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>, INewsletterResult
    {
        public PrimaryProductAddress PrimaryAddress { get; set; }
        public PrimaryProductPhone PrimaryPhone { get; set; }
        public PrimaryProductEmail PrimaryEmail { get; set; }
    }
}
