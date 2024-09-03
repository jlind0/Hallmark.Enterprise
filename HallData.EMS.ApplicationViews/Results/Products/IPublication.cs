using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;

namespace HallData.EMS.ApplicationViews
{
    public interface IPublicationKey : IBrandedProductKey { }
    public interface IPublication<TProductType, TFrequency> : IPublicationKey, IBrandedProduct<TProductType, TFrequency>
        where TProductType : ProductTypeKey
        where TFrequency : FrequencyKey
    {
        bool? IsControlled { get; set; }
        bool? IsAudited { get; set; }
        string AuditBureau { get; set; }
    }
    public interface IPublication<TProductType, TFrequency, TBrand> : IPublication<TProductType, TFrequency>, IBrandedProduct<TProductType, TFrequency, TBrand>
        where TProductType : ProductTypeKey
        where TFrequency : FrequencyKey
        where TBrand : IBrandKey
    { }
    public interface IPublicationWithOwner<TProductType, TFrequency, TBrand, TOwner> : IPublication<TProductType, TFrequency, TBrand>,
        IBrandedProductWithOwner<TProductType, TFrequency, TBrand, TOwner>
        where TProductType : ProductTypeKey
        where TFrequency : FrequencyKey
        where TBrand : IBrandKey
        where TOwner : IOrganizationKey
    { }
    public interface IPublicationWithStatus<TProductType, TFrequency, TBrand, TStatusType> : IPublication<TProductType, TFrequency, TBrand>,
        IBrandedProductWithStatus<TProductType, TFrequency, TBrand, TStatusType>
        where TProductType : ProductTypeKey
        where TFrequency : FrequencyKey
        where TBrand : IBrandKey
        where TStatusType : StatusTypeKey
    { }
    public interface IPublication<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator> :
        IBrandedProduct<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator>,
        IPublicationWithOwner<TProductType, TFrequency, TBrand, TOwner>, IPublicationWithStatus<TProductType, TFrequency, TBrand, TStatusType>
        where TProductType : ProductTypeKey
        where TFrequency : FrequencyKey
        where TBrand : IBrandKey
        where TStatusType : StatusTypeKey
        where TOwner : IOrganizationKey
        where TBusinessUnit : IBusinessUnitKey
        where TCreator : IUserKey
    { }
    public interface IPublicationForAddBase : IPublicationWithOwner<ProductTypeKey, FrequencyKey, BrandKey, OrganizationKey>, IBrandedProductForAddBase { }
    public interface IPublicationForAdd : IPublicationForAddBase, IBrandedProductForAdd { }
    public interface IPublicationForUpdateBase : IPublication<ProductTypeKey, FrequencyKey>, IBrandedProductForUpdateBase { }
    public interface IPublicationForUpdate : IPublicationForUpdateBase, IBrandedProductForUpdate { }
    public interface IPublicationResultBase : IPublication<ProductType, Frequency, BrandResult, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>,
        IBrandedProductResultBase { }
    public interface IPublicationResult : IPublicationResultBase, IBrandedProductResult { }
}
