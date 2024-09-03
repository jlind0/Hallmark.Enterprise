using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.ApplicationViews;

namespace HallData.EMS.ApplicationViews
{
    public interface ISessionKey : IProductKey { }
    public interface ISession<TProductType> : ISessionKey, IProduct<TProductType>
        where TProductType : ProductTypeKey
    { }
    public interface ISession<TProductType, TTrack> : ISession<TProductType>
        where TProductType : ProductTypeKey
        where TTrack : ITrackKey
    {
        TTrack Track { get; set; }
    }
    public interface ISessionWithParent<TProductType, TTrack> : ISession<TProductType, TTrack>, IProductWithParentProduct<TProductType, TTrack>
        where TProductType : ProductTypeKey
        where TTrack : ITrackKey
    { }
    public interface ISessionWithOwner<TProductType, TOwner> : ISession<TProductType>, IProductWithOwner<TProductType, TOwner>
        where TProductType : ProductTypeKey
        where TOwner : IOrganizationKey
    { }
    public interface ISessionWithStatus<TProductType, TStatusType> : ISession<TProductType>, IProductWithStatus<TProductType, TStatusType>
        where TProductType : ProductTypeKey
        where TStatusType : StatusTypeKey
    { }
    public interface ISession<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator> : ISessionWithOwner<TProductType, TOwner>,
        ISessionWithStatus<TProductType, TStatusType>, IProduct<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator>
        where TProductType : ProductTypeKey
        where TStatusType : StatusTypeKey
        where TOwner : IOrganizationKey
        where TBusinessUnit : IBusinessUnitKey
        where TCreator : IUserKey
    { }
    public interface ISessionForAddBase : ISessionWithOwner<ProductTypeKey, OrganizationKey>, ISession<ProductTypeKey, TrackKey>,
        IProductWithLocationForAddUpdate, IProductForAddBase { }
    public interface ISessionForAdd : ISessionForAddBase, ISessionWithParent<ProductTypeKey, TrackKey> { }
    public interface ISessionForUpdateBase : ISession<ProductTypeKey>, IProductWithLocationForAddUpdate, IProductForUpdateBase { }
    public interface ISessionForUpdate : ISessionForUpdateBase { }
    public interface ISessionResultBase : ISession<ProductType, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>,
        IProductWithLocationResult, ISession<ProductType, TrackResult>, IProductResultBase { }
    public interface ISessionResult : ISessionResultBase, ISessionWithParent<ProductType, TrackResult> { }
}
