using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.ApplicationViews;

namespace HallData.EMS.ApplicationViews
{
	public interface IProductKey : IProductBaseKey { }

	public interface IProduct<TProductType> : IProductKey, IProductBase<TProductType>
		where TProductType : ProductTypeKey { }

	public interface IProductWithStatus<TProductType, TStatusType> : IProduct<TProductType>, IProductBaseWithStatus<TProductType, TStatusType>
		where TProductType: ProductTypeKey
		where TStatusType : StatusTypeKey { }

	public interface IProductWithOwner<TProductType, TOwner> : IProduct<TProductType>, IProductBaseWithOwner<TProductType, TOwner> 
		where TOwner: IOrganizationKey
		where TProductType: ProductTypeKey
	{ }

	public interface IProductWithParentProduct<TProductType, TParent> : IProduct<TProductType>
		where TProductType: ProductTypeKey
		where TParent: IProductKey
	{
		TParent ParentProduct { get; set; }
	}

	public interface IProductWithLocation<TProductType, TMechanismType, TLocation, TContactEmail, TContactPhone> : IProduct<TProductType>
		where TProductType: ProductTypeKey
		where TLocation: IPrimaryProductAddress<TMechanismType>
		where TContactEmail: IPrimaryProductEmail<TMechanismType>
		where TContactPhone: IPrimaryProductPhone<TMechanismType>
		where TMechanismType: MechanismTypeKey
	{
		TLocation AttendenceLocation { get; set; }
		TContactEmail ContactEmail { get; set; }
		TContactPhone ContactPhone { get; set; }
	}

	public interface IProductWithLocationForAddUpdate : IProductWithLocation<ProductTypeKey, MechanismTypeKey, PrimaryProductAddressForAddUpdate, PrimaryProductEmailForAddUpdate, PrimaryProductPhoneForAddUpdate> { }
	
	public interface IProductWithLocationResult : IProductWithLocation<ProductType, MechanismTypeResult, PrimaryProductAddress, PrimaryProductEmail, PrimaryProductPhone> { }
 	
	public interface IProduct<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator> : IProductWithOwner<TProductType, TOwner>, 
		IProductWithStatus<TProductType, TStatusType>, IProductBase<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator>
		where TProductType : ProductTypeKey
		where TStatusType : StatusTypeKey
		where TOwner : IOrganizationKey
		where TBusinessUnit : IBusinessUnitKey
		where TCreator : IUserKey
	{ }

	public interface IProductForAddBase : IProductWithOwner<ProductTypeKey, OrganizationKey>, IProductBaseForAddBase { }
	
	public interface IProductForAdd : IProductForAddBase, IProductBaseForAdd { }
	
	public interface IProductForUpdateBase : IProduct<ProductTypeKey>, IProductBaseForUpdateBase { }
   
	public interface IProductForUpdate : IProductForUpdateBase, IProductBaseForUpdate { }
	
	public interface IProductResultBase : IProduct<ProductType, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>, IProductBaseResultBase { }
	
	public interface IProductResult : IProductResultBase, IProductBaseResult { }
}
