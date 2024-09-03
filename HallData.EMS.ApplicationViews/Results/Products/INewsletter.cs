using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;

namespace HallData.EMS.ApplicationViews
{
	public interface INewsletterKey : IBrandedProductKey { }
	public interface INewsletter<TProductType, TFrequency> : INewsletterKey, IBrandedProduct<TProductType, TFrequency>
		where TProductType : ProductTypeKey
		where TFrequency : FrequencyKey
	{ }

	public interface INewsletter<TProductType, TFrequency, TBrand> : INewsletter<TProductType, TFrequency>, IBrandedProduct<TProductType, TFrequency, TBrand>
		where TProductType : ProductTypeKey
		where TFrequency : FrequencyKey
		where TBrand : IBrandKey
	{ }

	public interface INewsletterWithOwner<TProductType, TFrequency, TBrand, TOwner> : INewsletter<TProductType, TFrequency, TBrand>,
		IBrandedProductWithOwner<TProductType, TFrequency, TBrand, TOwner>
		where TProductType : ProductTypeKey
		where TFrequency : FrequencyKey
		where TBrand : IBrandKey
		where TOwner : IOrganizationKey
	{ }

	public interface INewsletterWithStatus<TProductType, TFrequency, TBrand, TStatusType> : INewsletter<TProductType, TFrequency, TBrand>,
		IBrandedProductWithStatus<TProductType, TFrequency, TBrand, TStatusType>
		where TProductType : ProductTypeKey
		where TFrequency : FrequencyKey
		where TBrand : IBrandKey
		where TStatusType : StatusTypeKey
	{ }

	public interface INewsletter<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator> :
		IBrandedProduct<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator>,
		INewsletterWithOwner<TProductType, TFrequency, TBrand, TOwner>, INewsletterWithStatus<TProductType, TFrequency, TBrand, TStatusType>
		where TProductType : ProductTypeKey
		where TFrequency : FrequencyKey
		where TBrand : IBrandKey
		where TStatusType : StatusTypeKey
		where TOwner : IOrganizationKey
		where TBusinessUnit : IBusinessUnitKey
		where TCreator : IUserKey
	{ }

	public interface INewsletterForAddBase : INewsletterWithOwner<ProductTypeKey, FrequencyKey, BrandKey, OrganizationKey>, IBrandedProductForAddBase { }
	public interface INewsletterForAdd : INewsletterForAddBase, IBrandedProductForAdd { }
	
	public interface INewsletterForUpdateBase : INewsletter<ProductTypeKey, FrequencyKey>, IBrandedProductForUpdateBase { }
	public interface INewsletterForUpdate : INewsletterForUpdateBase, IBrandedProductForUpdate { }

	public interface INewsletterResultBase : INewsletter<ProductType, Frequency, BrandResult, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>,
		IBrandedProductResultBase { }
	public interface INewsletterResult : INewsletterResultBase, IBrandedProductResult { }
}
