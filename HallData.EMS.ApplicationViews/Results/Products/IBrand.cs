using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;

namespace HallData.EMS.ApplicationViews
{
    public interface IBrandKey : IProductBaseKey { }
    
    public interface IBrand<TProductType> : IBrandKey, IProductBase<TProductType>
        where TProductType: ProductTypeKey
    {
        DateTime? StartDate { get; set; }
    }

    public interface IBrandWithStatus<TProductType, TStatusType> : IBrand<TProductType>, IProductBaseWithStatus<TProductType, TStatusType>
        where TProductType: ProductTypeKey
        where TStatusType : StatusTypeKey { }

    public interface IBrandWithOwner<TProductType, TOwner> : IBrand<TProductType>, IProductBaseWithOwner<TProductType, TOwner>
        where TProductType : ProductTypeKey
        where TOwner : IOrganizationKey { }

    public interface IBrand<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator> : IBrandWithOwner<TProductType, TOwner>,
        IBrandWithStatus<TProductType, TStatusType>, IProductBase<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator>
        where TProductType : ProductTypeKey
        where TStatusType: StatusTypeKey
        where TOwner: IOrganizationKey
        where TBusinessUnit: IBusinessUnitKey
        where TCreator : IUserKey { }

    public interface IBrandForAddBase : IBrandWithOwner<ProductTypeKey, OrganizationKey>, IProductBaseForAddBase { }
    public interface IBrandForAdd : IBrandForAddBase, IProductBaseForAdd { }
    
    public interface IBrandForUpdateBase : IBrand<ProductTypeKey>, IProductBaseForUpdateBase { }
    public interface IBrandForUpdate : IBrandForUpdateBase, IProductBaseForUpdate { }
    
	public interface IBrandResultBase : IBrand<ProductType, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>, IProductBaseResultBase { }
    public interface IBrandResult : IBrandResultBase, IProductBaseResult { }
}
