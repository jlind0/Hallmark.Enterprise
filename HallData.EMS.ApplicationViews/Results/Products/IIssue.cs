using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.ApplicationViews;

namespace HallData.EMS.ApplicationViews
{
    public interface IIssueKey : IProductKey { }
    public interface IIssue<TProductType> : IIssueKey, IProduct<TProductType>
        where TProductType : ProductTypeKey
    {
        bool? IsAudited { get; set; }
        DateTime? LabelDate { get; set; }
        DateTime? FirstIssueCloseDate { get; set; }
        bool? IsGeneratedRevenue { get; set; }
    }

    public interface IIssue<TProductType, TPublication> : IIssue<TProductType>
        where TProductType : ProductTypeKey
        where TPublication : IPublicationKey
    {
        TPublication Publication { get; set; }
    }

    public interface IIssueWithParent<TProductType, TPublication> : IIssue<TProductType, TPublication>, IProductWithParentProduct<TProductType, TPublication>
        where TProductType : ProductTypeKey
        where TPublication : IPublicationKey
    { }

    public interface IIssueWithOwner<TProductType, TOwner> : IIssue<TProductType>, IProductWithOwner<TProductType, TOwner>
        where TProductType : ProductTypeKey
        where TOwner : IOrganizationKey
    { }

    public interface IIssueWithStatus<TProductType, TStatusType> : IIssue<TProductType>, IProductWithStatus<TProductType, TStatusType>
        where TProductType : ProductTypeKey
        where TStatusType : StatusTypeKey
    { }

    public interface IIssue<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator> : IIssueWithOwner<TProductType, TOwner>,
        IIssueWithStatus<TProductType, TStatusType>, IProduct<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator>
        where TProductType : ProductTypeKey
        where TStatusType : StatusTypeKey
        where TOwner : IOrganizationKey
        where TBusinessUnit : IBusinessUnitKey
        where TCreator : IUserKey
    { }

    public interface IIssueForAddBase : IIssueWithOwner<ProductTypeKey, OrganizationKey>, IIssue<ProductTypeKey, PublicationKey>, IProductForAddBase { }
    public interface IIssueForAdd : IIssueForAddBase, IProductForAdd, IIssueWithParent<ProductTypeKey, PublicationKey> { }
    
	public interface IIssueForUpdateBase : IIssue<ProductTypeKey>, IProductForUpdateBase { }
    public interface IIssueForUpdate : IIssueForUpdateBase, IProductForUpdate { }

    public interface IIssueResultBase : IIssue<ProductType, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult>, IIssue<ProductType, PublicationResult>, IProductResultBase { }
    public interface IIssueResult : IIssueResultBase, IIssueWithParent<ProductType, PublicationResult>, IProductResult { }
}
