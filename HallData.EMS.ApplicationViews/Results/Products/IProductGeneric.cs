using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;

namespace HallData.EMS.ApplicationViews
{
    public interface IProductGenericKey : IBrandKey, IEventKey, ITrackKey, IPublicationKey, ISessionKey, IIssueKey { }
    public interface IProductGeneric<TProductType, TFrequency> : IProductGenericKey, IBrand<TProductType>, IEvent<TProductType, TFrequency>, IPublication<TProductType, TFrequency>,
        ITrack<TProductType>, ISession<TProductType>, IIssue<TProductType>, INewsletter<TProductType, TFrequency>
        where TProductType : ProductTypeKey
        where TFrequency: FrequencyKey
    { }
    public interface IProductGenericWithStatus<TProductType, TFrequency, TBrand, TStatusType> : IProductGeneric<TProductType, TFrequency>,
        IBrandWithStatus<TProductType, TStatusType>, IEventWithStatus<TProductType, TFrequency, TBrand, TStatusType>,
        IPublicationWithStatus<TProductType, TFrequency, TBrand, TStatusType>, ITrackWithStatus<TProductType, TStatusType>,
        ISessionWithStatus<TProductType, TStatusType>, IIssueWithStatus<TProductType, TStatusType>, INewsletterWithStatus<TProductType, TFrequency, TBrand, TStatusType>
        where TProductType : ProductTypeKey
        where TStatusType : StatusTypeKey
        where TFrequency : FrequencyKey
        where TBrand: IBrandKey
    { }
    public interface IProductGenericWithOwner<TProductType, TFrequency, TBrand, TOwner> : IProductGeneric<TProductType, TFrequency>,
        IBrandWithOwner<TProductType, TOwner>, IEventWithOwner<TProductType, TFrequency, TBrand, TOwner>, IPublicationWithOwner<TProductType, TFrequency, TBrand, TOwner>, ITrackWithOwner<TProductType, TOwner>,
        ISessionWithOwner<TProductType, TOwner>, IIssueWithOwner<TProductType, TOwner>, INewsletterWithOwner<TProductType, TFrequency, TBrand, TOwner>
        where TProductType: ProductTypeKey
        where TOwner : IOrganizationKey
        where TFrequency : FrequencyKey
        where TBrand: IBrandKey
    { }
    public interface IProductGeneric<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator> : IProductGenericWithOwner<TProductType, TFrequency, TBrand, TOwner>,
        IProductGenericWithStatus<TProductType, TFrequency, TBrand, TStatusType>, 
        IBrand<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator>, 
        IEvent<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator>,
        IPublication<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator>,
        ITrack<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator>,
        ISession<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator>,
        IIssue<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator>,
        INewsletter<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator>
        where TProductType : ProductTypeKey
        where TStatusType : StatusTypeKey
        where TOwner : IOrganizationKey
        where TBusinessUnit : IBusinessUnitKey
        where TCreator : IUserKey
        where TFrequency : FrequencyKey
        where TBrand: IBrandKey
    { }
    public interface IProductGenericForAdd : IProductGenericWithOwner<ProductTypeKey, FrequencyKey, BrandKey, OrganizationKey>, IBrandForAddBase, 
        IBrandedProductForAdd, IEventForAddBase, IProductWithParentProduct<ProductTypeKey, ProductGenericKey>, ITrackForAddBase, ISessionForAddBase, 
        IPublicationForAddBase, IIssueForAddBase, INewsletterForAddBase{ }
    public interface IProductGenericForUpdate : IProductGeneric<ProductTypeKey, FrequencyKey>, IBrandForUpdateBase, IBrandedProductForUpdate, IEventForUpdateBase, 
        ITrackForUpdateBase, ISessionForUpdateBase, IPublicationForUpdateBase, IIssueForUpdateBase, INewsletterForUpdateBase { }
    public interface IProductGenericResult : IProductGeneric<ProductType, Frequency, BrandResult, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>,
        IBrandResultBase, IBrandedProductResult, IEventResultBase, IProductWithParentProduct<ProductType, ProductResult>, 
        ITrackResultBase, ISessionResultBase, IPublicationResultBase, IIssueResultBase, INewsletterResultBase { }
}
