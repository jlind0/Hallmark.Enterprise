using System;
using System.Collections.Generic;
using HallData.EMS.ApplicationViews.Results;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HallData.EMS.ApplicationViews
{
    public class TrackKey : ProductKey, ITrackKey
    {

    }
    public class Track<TProductType> : Product<TProductType>, ITrack<TProductType>
        where TProductType: ProductTypeKey
    {

    }
    public class Track<TProductType, TEvent, TOwner> : Track<TProductType>, ITrackWithOwner<TProductType, TOwner>, ITrackWithParent<TProductType, TEvent>
        where TProductType : ProductTypeKey
        where TEvent : IEventKey
        where TOwner : IOrganizationKey
    {
        [AddOperationParameter]
        [ChildView]
        public virtual TOwner Owner { get; set; }
        [ChildView]
        [AddOperationParameter]
        public virtual TEvent Event { get; set; }
        [ChildView]
        [JsonIgnore]
        public virtual TEvent ParentProduct
        {
            get { return this.Event; }
            set { this.Event = value; }
        }
    }
    public class Track<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator, TEvent> : Track<TProductType, TEvent, TOwner>, 
        ITrack<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator>
        where TProductType : ProductTypeKey
        where TStatusType : StatusTypeKey
        where TOwner : IOrganizationKey
        where TBusinessUnit : IBusinessUnitKey
        where TCreator : IUserKey
        where TEvent : IEventKey
    {
        [ChildView]
        public TStatusType Status { get; set; }
        [ChildView]
        public TBusinessUnit BusinessUnit { get; set; }
        [ChildView]
        public TCreator Creator { get; set; }

        public DateTime? CreateDate { get; set; }
    }
    public class TrackForAddBase : Track<ProductTypeKey, EventKey, OrganizationKey>, ITrackForAdd
    {
        [AddOperationParameter]
        [ChildView]
        public PrimaryProductAddressForAddUpdate AttendenceLocation { get; set; }
        [AddOperationParameter]
        [ChildView]
        public PrimaryProductEmailForAddUpdate ContactEmail { get; set; }
        [AddOperationParameter]
        [ChildView]
        public PrimaryProductPhoneForAddUpdate ContactPhone { get; set; }

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
    public class TrackForAddEvent : TrackForAddBase
    {
        [JsonIgnore]
        public override EventKey Event
        {
            get
            {
                return base.Event;
            }
            set
            {
                base.Event = value;
            }
        }
        [JsonIgnore]
        public override ProductTypeKey ProductType
        {
            get
            {
                return new ProductTypeKey() { ProductTypeId = (int)Enums.ProductTypes.Track };
            }
            set
            {
                
            }
        }
    }
    public class TrackForAdd : TrackForAddBase
    {
        [GlobalizedRequired("TRACK_EVENT_REQUIRED")]
        public override EventKey Event
        {
            get
            {
                return base.Event;
            }
            set
            {
                base.Event = value;
            }
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var valid in base.Validate(validationContext))
                yield return valid;
            if (this.Event == null || this.Event.ProductGuid == null)
                yield return ValidationResultFactory.Create(new ValidationResult("Event is Required for Track"), "TRACK_EVENT_REQUIRED");
        }
    }
    public class TrackForUpdate : Track<ProductTypeKey>, ITrackForUpdate
    {
        [GlobalizedRequired("TRACK_PRODUCTGUID_REQUIRED")]
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
                return new ProductTypeKey() { ProductTypeId = (int)Enums.ProductTypes.Track };
            }
            set
            {

            }
        }
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
    public class TrackResult : Track<ProductType, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult, EventResult>, ITrackResult
    {
        public PrimaryProductAddress AttendenceLocation { get; set; }

        public PrimaryProductEmail ContactEmail { get; set; }

        public PrimaryProductPhone ContactPhone { get; set; }
    }
}
