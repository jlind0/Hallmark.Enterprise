using HallData.EMS.ApplicationViews.Results;

namespace HallData.EMS.ApplicationViews
{
    public interface ITrackKey : IProductKey { }
    public interface ITrack<TProductType> : ITrackKey, IProduct<TProductType>
        where TProductType: ProductTypeKey
    {}
    public interface ITrack<TProductType, TEvent> : ITrack<TProductType>
        where TProductType: ProductTypeKey
        where TEvent: IEventKey
    {
        TEvent Event { get; set; }
    }
    public interface ITrackWithParent<TProductType, TEvent> : ITrack<TProductType, TEvent>, IProductWithParentProduct<TProductType, TEvent>
        where TProductType : ProductTypeKey
        where TEvent : IEventKey
    { }
    public interface ITrackWithOwner<TProductType, TOwner> : ITrack<TProductType>, IProductWithOwner<TProductType, TOwner>
        where TProductType: ProductTypeKey
        where TOwner: IOrganizationKey
    { }
    public interface ITrackWithStatus<TProductType, TStatusType> : ITrack<TProductType>, IProductWithStatus<TProductType, TStatusType>
        where TProductType: ProductTypeKey
        where TStatusType: StatusTypeKey
    { }
    public interface ITrack<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator> : ITrackWithOwner<TProductType, TOwner>, 
        ITrackWithStatus<TProductType, TStatusType>, IProduct<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator>
        where TProductType : ProductTypeKey
        where TStatusType : StatusTypeKey
        where TOwner : IOrganizationKey
        where TBusinessUnit : IBusinessUnitKey
        where TCreator : IUserKey
    { }
    public interface ITrackForAddBase : ITrackWithOwner<ProductTypeKey, OrganizationKey>, ITrack<ProductTypeKey, EventKey>,
        IProductWithLocationForAddUpdate, IProductForAddBase { }
    public interface ITrackForAdd : ITrackForAddBase, ITrackWithParent<ProductTypeKey, EventKey> { }
    public interface ITrackForUpdateBase : ITrack<ProductTypeKey>, IProductWithLocationForAddUpdate, IProductForUpdateBase { }
    public interface ITrackForUpdate : ITrackForUpdateBase { }
    public interface ITrackResultBase : ITrack<ProductType, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>,
        IProductWithLocationResult, ITrack<ProductType, EventResult>, IProductResultBase { }
    public interface ITrackResult : ITrackResultBase, ITrackWithParent<ProductType, EventResult> { }
}
