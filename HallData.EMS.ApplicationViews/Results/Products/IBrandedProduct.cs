using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;

namespace HallData.EMS.ApplicationViews
{
	public interface IBrandedProductKey : IProductKey { }
	public interface IBrandedProduct<TProductType, TFrequency> : IBrandedProductKey, IProduct<TProductType>
		where TProductType: ProductTypeKey
		where TFrequency: FrequencyKey

	{
		DateTime? StartDate { get; set; }
		DateTime? EndDate { get; set; }
		TFrequency Frequency { get; set; }
	}

	public interface IBrandedProduct<TProductType, TFrequency, TBrand> : IBrandedProduct<TProductType, TFrequency>
		where TProductType: ProductTypeKey
		where TBrand: IBrandKey
		where TFrequency: FrequencyKey
	{
		TBrand Brand { get; set; }
	}

	public interface IBrandedProductWithStatus<TProductType, TFrequency, TBrand, TStatusType> : 
		IBrandedProduct<TProductType, TFrequency, TBrand>, IProductWithStatus<TProductType, TStatusType>
		where TProductType: ProductTypeKey
		where TStatusType : StatusTypeKey
		where TFrequency : FrequencyKey
		where TBrand: IBrandKey
	{ }

	public interface IBrandedProductWithOwner<TProductType, TFrequency, TBrand, TOwner> :
		IBrandedProduct<TProductType, TFrequency, TBrand>, IProductWithOwner<TProductType, TOwner>
		where TProductType : ProductTypeKey
		where TOwner : IOrganizationKey
		where TFrequency : FrequencyKey
		where TBrand: IBrandKey
	{ }

	public interface IBrandedProduct<TProductType, TFrequency, TBrand, TStatusType, TOwner, TBusinessUnit, TCreator> : 
		IProduct<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator>, 
		IBrandedProductWithOwner<TProductType, TFrequency, TBrand, TOwner>, IBrandedProductWithStatus<TProductType, TFrequency, TBrand, TStatusType>
		where TProductType : ProductTypeKey
		where TFrequency : FrequencyKey
		where TBrand: IBrandKey
		where TStatusType: StatusTypeKey
		where TOwner: IOrganizationKey
		where TBusinessUnit: IBusinessUnitKey
		where TCreator: IUserKey
	{ }

	public interface IBrandedProductForAddBase : IBrandedProductWithOwner<ProductTypeKey, FrequencyKey, BrandKey, OrganizationKey>, IProductForAddBase { }
	public interface IBrandedProductForAdd : IBrandedProductForAddBase, IProductForAdd { }
   
	public interface IBrandedProductForUpdateBase : IProductForUpdateBase { }
	public interface IBrandedProductForUpdate : IBrandedProductForUpdateBase, IProductForUpdate { }
	
	public interface IBrandedProductResultBase : IBrandedProduct<ProductType, Frequency, BrandResult, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>, IProductResultBase { }
	public interface IBrandedProductResult : IBrandedProductResultBase, IProductResult { }

}
