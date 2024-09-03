using System;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using System.Collections.Generic;

namespace HallData.EMS.ApplicationViews
{
	public interface IProductId
	{
		Guid ProductGuid { get; set; }
	}

	public interface IProductBaseKey : IHasKey<Guid>, IHasInstanceGuid
	{
		Guid? ProductGuid { get; set; }
	}

	public interface IProductBase<TProductType> : IProductBaseKey
		where TProductType: ProductTypeKey
	{
		TProductType ProductType { get; set; }
		string Name { get; set; }
		string Code { get; set; }
		string Caption { get; set; }
	}

	public interface IProductBaseWithStatus<TProductType, TStatusType> : IProductBase<TProductType>
		where TProductType: ProductTypeKey
		where TStatusType: StatusTypeKey
	{
		TStatusType Status { get; set; }
	}

	public interface IProductBaseWithOwner<TProductType, TOwner> : IProductBase<TProductType>
		where TProductType : ProductTypeKey
		where TOwner: IOrganizationKey
	{
		TOwner Owner { get; set; }
	}

	public interface IProductBase<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator> : IProductBaseWithOwner<TProductType, TOwner>, IProductBaseWithStatus<TProductType, TStatusType>
		where TProductType : ProductTypeKey
		where TStatusType: StatusTypeKey
		where TOwner: IOrganizationKey
		where TBusinessUnit: IBusinessUnitKey
		where TCreator : IUserKey
	{
		TBusinessUnit BusinessUnit { get; set; }
		TCreator Creator { get; set; }
		DateTime? CreateDate { get; set; }
	}

	public interface IProductWithContactMechanisms<TMechanismType, TPrimaryAddress, TPrimaryEmail, TPrimaryPhone> : IProductBaseKey
		where TMechanismType : MechanismTypeKey
		where TPrimaryAddress : IPrimaryProductAddress<TMechanismType>
		where TPrimaryEmail : IPrimaryProductEmail<TMechanismType>
		where TPrimaryPhone : IPrimaryProductPhone<TMechanismType>
	{
		TPrimaryAddress PrimaryAddress { get; set; }
		TPrimaryPhone PrimaryPhone { get; set; }
		TPrimaryEmail PrimaryEmail { get; set; }
	}

	public interface IProductWithContactMechanismsForAdd : IProductWithContactMechanisms<MechanismTypeKey,
		PrimaryProductAddressForAddUpdate, PrimaryProductEmailForAddUpdate, PrimaryProductPhoneForAddUpdate> { }
	
	public interface IProductWithContactMechanismsForUpdate : IProductWithContactMechanisms<MechanismTypeKey,
		PrimaryProductAddressForAddUpdate, PrimaryProductEmailForAddUpdate, PrimaryProductPhoneForAddUpdate> { }
	
	public interface IProductWithContactMechanismsForResult: 
		IProductWithContactMechanisms<MechanismTypeResult, PrimaryProductAddress, PrimaryProductEmail, PrimaryProductPhone> { }

	public interface IProductBaseForAddBase : IProductBaseWithOwner<ProductTypeKey, OrganizationKey> { }

	public interface IProductBaseForAdd : IProductBaseForAddBase, IProductWithContactMechanismsForAdd { }

	public interface IProductBaseForUpdateBase : IProductBase<ProductTypeKey> { }

	public interface IProductBaseForUpdate : IProductBaseForUpdateBase, IProductWithContactMechanismsForUpdate { }

	public interface IProductBaseResultBase : IProductBase<ProductType, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult> { }

	public interface IProductBaseResult : IProductBaseResultBase, IProductWithContactMechanismsForResult { }
	
}