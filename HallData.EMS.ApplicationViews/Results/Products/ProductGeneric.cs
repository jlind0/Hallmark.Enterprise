using System;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using Newtonsoft.Json;
using HallData.Validation;
using HallData.Utilities;

namespace HallData.EMS.ApplicationViews
{
    public class ProductGenericKeyBase : ProductBaseKey, IProductGenericKey
    {

    }
    public class ProductGenericKey : ProductGenericKeyBase
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
    public class ProductGeneric<TProductType, TFrequency> : ProductBase<TProductType>, IProductGeneric<TProductType, TFrequency>
        where TProductType : ProductTypeKey
        where TFrequency : FrequencyKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        public virtual DateTime? StartDate { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public virtual DateTime? EndDate { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        [ChildView]
        public virtual TFrequency Frequency { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? IsControlled { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? IsAudited { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public string AuditBureau { get; set; }

        [AddOperationParameter]
        [UpdateOperationParameter]
        public DateTime? LabelDate { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public DateTime? FirstIssueCloseDate { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? IsGeneratedRevenue { get; set; }
    }
    public class ProductGeneric<TProductType, TFrequency, TBrand, TOwner> : ProductGeneric<TProductType, TFrequency>, IProductGenericWithOwner<TProductType, TFrequency, TBrand, TOwner>
        where TProductType : ProductTypeKey
        where TOwner : IOrganizationKey
        where TFrequency : FrequencyKey
        where TBrand : IBrandKey
    {
        [AddOperationParameter]
        [ChildView]
        public TOwner Owner { get; set; }
        [AddOperationParameter]
        [ChildView]
        public TBrand Brand { get; set; }
    }
    public class ProductGeneric<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator> : ProductGeneric<TProductType, TFrequency, TBrand, TOwner>,
        IProductGeneric<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator>
        where TProductType : ProductTypeKey
        where TStatusType : StatusTypeKey
        where TOwner : IOrganizationKey
        where TBusinessUnit : IBusinessUnitKey
        where TCreator : IUserKey
        where TFrequency : FrequencyKey
        where TBrand : IBrandKey
    {
        [ChildView]
        public TStatusType Status { get; set; }
        [ChildView]
        public TBusinessUnit BusinessUnit { get; set; }
        [ChildView]
        public TCreator Creator { get; set; }
        public DateTime? CreateDate { get; set; }
    }
    public class ProductForAddBase : ProductGeneric<ProductTypeKey, FrequencyKey, BrandKey, OrganizationKey>, IProductGenericForAdd
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
        [ChildView]
        [JsonIgnore]
        public ProductGenericKey ParentProduct
        {
            get
            {
                if (this.ProductType != null)
                {
                    if (this.ProductType.ProductTypeId == (int)Enums.ProductTypes.Track && this.Event != null)
                        return this.Event.CreateRelatedInstance<ProductGenericKey>();
                    if (this.ProductType.ProductTypeId == (int)Enums.ProductTypes.Session && this.Track != null)
                        return this.Track.CreateRelatedInstance<ProductGenericKey>();
                    if (this.ProductType.ProductTypeId == (int)Enums.ProductTypes.Issue && this.Publication != null)
                        return this.Publication.CreateRelatedInstance<ProductGenericKey>();
                }
                return null;
            }
            set
            {
                if (value != null && this.ProductType != null)
                {
                    if (this.ProductType.ProductTypeId == (int)Enums.ProductTypes.Track)
                    {
                        this.Event = value.CreateRelatedInstance<EventKey>();
                        this.Track = null;
                        this.Publication = null;
                    }
                    else if(this.ProductType.ProductTypeId == (int)Enums.ProductTypes.Session)
                    {
                        this.Track = value.CreateRelatedInstance<TrackKey>();
                        this.Event = null;
                        this.Publication = null;
                    }
                    else if (this.ProductType.ProductTypeId == (int)Enums.ProductTypes.Issue)
                    {
                        this.Publication = value.CreateRelatedInstance<PublicationKey>();
                        this.Event = null;
                        this.Track = null;
                    }
                }
                this.Track = null;    
                this.Event = null;
                this.Publication = null;
            }
        }

        [AddOperationParameter]
        [ChildView]
        public PrimaryProductAddressForAddUpdate AttendenceLocation { get; set; }
        [AddOperationParameter]
        [ChildView]
        public PrimaryProductEmailForAddUpdate ContactEmail { get; set; }
        [AddOperationParameter]
        [ChildView]
        public PrimaryProductPhoneForAddUpdate ContactPhone { get; set; }
        [ChildView]
        [AddOperationParameter]
        public EventKey Event { get; set; }
        [ChildView]
        [AddOperationParameter]
        public TrackKey Track { get; set; }
        [ChildView]
        [AddOperationParameter]
        public PublicationKey Publication { get; set; }
    }
    public class ProductForAdd : ProductForAddBase
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
    public class ProductForUpdateBase : ProductGeneric<ProductTypeKey, FrequencyKey>, IProductGenericForUpdate
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
        [UpdateOperationParameter]
        [ChildView]
        public PrimaryProductAddressForAddUpdate AttendenceLocation { get; set; }
        [UpdateOperationParameter]
        [ChildView]
        public PrimaryProductEmailForAddUpdate ContactEmail { get; set; }
        [UpdateOperationParameter]
        [ChildView]
        public PrimaryProductPhoneForAddUpdate ContactPhone { get; set; }
    }
    public class ProductForUpdate : ProductForUpdateBase
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
    public class ProductResult : ProductGeneric<ProductType, Frequency, BrandResult, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>, IProductGenericResult
    {
        public DateTime? CreateDate { get; set; }

        public PrimaryProductAddress PrimaryAddress { get; set; }

        public PrimaryProductPhone PrimaryPhone { get; set; }

        public PrimaryProductEmail PrimaryEmail { get; set; }

        public ProductResult ParentProduct { get; set; }

        public PrimaryProductAddress AttendenceLocation { get; set; }

        public PrimaryProductEmail ContactEmail { get; set; }

        public PrimaryProductPhone ContactPhone { get; set; }

        public EventResult Event { get; set; }

        public TrackResult Track { get; set; }

        public PublicationResult Publication { get; set; }
    }
}
