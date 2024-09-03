using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;

namespace HallData.EMS.ApplicationViews
{
	public interface IEventKey : IBrandedProductKey { }
	public interface IEvent<TProductType, TFrequency> : IEventKey, IBrandedProduct<TProductType, TFrequency>
		where TProductType : ProductTypeKey
		where TFrequency : FrequencyKey 
	{ }

	public interface IEvent<TProductType, TFrequency, TBrand> : IEvent<TProductType, TFrequency>, IBrandedProduct<TProductType, TFrequency, TBrand>
		where TProductType : ProductTypeKey
		where TFrequency : FrequencyKey
		where TBrand : IBrandKey 
	{ }

	public interface IEventWithOwner<TProductType, TFrequency, TBrand, TOwner> : IEvent<TProductType, TFrequency, TBrand>,
		IBrandedProductWithOwner<TProductType, TFrequency, TBrand, TOwner>
		where TProductType : ProductTypeKey
		where TFrequency : FrequencyKey
		where TBrand : IBrandKey
		where TOwner: IOrganizationKey
	{ }

	public interface IEventWithStatus<TProductType, TFrequency, TBrand, TStatusType> : IEvent<TProductType, TFrequency, TBrand>,
		IBrandedProductWithStatus<TProductType, TFrequency, TBrand, TStatusType>
		where TProductType : ProductTypeKey
		where TFrequency : FrequencyKey
		where TBrand : IBrandKey
		where TStatusType : StatusTypeKey
	{ }

	public interface IEvent<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator> :
		IBrandedProduct<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator>,
		IEventWithOwner<TProductType, TFrequency, TBrand, TOwner>, IEventWithStatus<TProductType, TFrequency, TBrand, TStatusType>
		where TProductType : ProductTypeKey
		where TFrequency : FrequencyKey
		where TBrand: IBrandKey
		where TStatusType: StatusTypeKey
		where TOwner: IOrganizationKey
		where TBusinessUnit: IBusinessUnitKey
		where TCreator: IUserKey
	{ }

	public interface IEventForAddBase : IEventWithOwner<ProductTypeKey, FrequencyKey, BrandKey, OrganizationKey>, IBrandedProductForAddBase { }
	public interface IEventForAdd : IEventForAddBase, IBrandedProductForAdd { }
	
	public interface IEventForUpdateBase : IEvent<ProductTypeKey, FrequencyKey>, IBrandedProductForUpdateBase { }
	public interface IEventForUpdate : IEventForUpdateBase, IBrandedProductForUpdate { }

	public interface IEventResultBase : IEvent<ProductType, Frequency, BrandResult, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>,
		IBrandedProductResultBase { }
	public interface IEventResult : IEventResultBase, IBrandedProductResult { }
}
