using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HallData.EMS.ApplicationViews
{
    public class IssueKey : ProductKey, IIssueKey
    {

    }
    public class Issue<TProductType> : Product<TProductType>, IIssue<TProductType>
        where TProductType : ProductTypeKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? IsAudited { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public DateTime? LabelDate { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public DateTime? FirstIssueCloseDate { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? IsGeneratedRevenue { get; set; }
    }
    public class Issue<TProductType, TPublication, TOwner> : Issue<TProductType>, IIssueWithOwner<TProductType, TOwner>, IIssueWithParent<TProductType, TPublication>
        where TProductType : ProductTypeKey
        where TPublication : IPublicationKey
        where TOwner : IOrganizationKey
    {
        [AddOperationParameter]
        [ChildView]
        public virtual TOwner Owner { get; set; }
        [ChildView]
        [AddOperationParameter]
        public virtual TPublication Publication { get; set; }
        [ChildView]
        [JsonIgnore]
        public virtual TPublication ParentProduct
        {
            get { return this.Publication; }
            set { this.Publication = value; }
        }
    }
    public class Issue<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator, TPublication> : Issue<TProductType, TPublication, TOwner>,
        IIssue<TProductType, TStatusType, TOwner, TBusinessUnit, TCreator>
        where TProductType : ProductTypeKey
        where TStatusType : StatusTypeKey
        where TOwner : IOrganizationKey
        where TBusinessUnit : IBusinessUnitKey
        where TCreator : IUserKey
        where TPublication : IPublicationKey
    {
        [ChildView]
        public TStatusType Status { get; set; }
        [ChildView]
        public TBusinessUnit BusinessUnit { get; set; }
        [ChildView]
        public TCreator Creator { get; set; }

        public DateTime? CreateDate { get; set; }
    }
    public class IssueForAddBase : Issue<ProductTypeKey, PublicationKey, OrganizationKey>, IIssueForAdd
    {
        [JsonIgnore]
        public override Guid? ProductGuid
        {
            get
            {
                return base.ProductGuid;
            }
            set
            {
                base.ProductGuid = value;
            }
        }
        [AddOperationParameter]
        [ChildView]
        public PrimaryProductAddressForAddUpdate PrimaryAddress { get; set; }
        [AddOperationParameter]
        [ChildView]
        public PrimaryProductPhoneForAddUpdate PrimaryPhone { get; set; }
        [AddOperationParameter]
        [ChildView]
        public PrimaryProductEmailForAddUpdate PrimaryEmail { get; set; }
    }
    public class IssueForAddPublication : IssueForAddBase
    {
        [JsonIgnore]
        public override PublicationKey Publication
        {
            get
            {
                return base.Publication;
            }
            set
            {
                base.Publication = value;
            }
        }
        [JsonIgnore]
        public override ProductTypeKey ProductType
        {
            get
            {
                return new ProductTypeKey() { ProductTypeId = (int)Enums.ProductTypes.Issue };
            }
            set
            {

            }
        }
    }
    public class IssueForAdd : IssueForAddBase
    {
        [GlobalizedRequired("ISSUE_PUBLICATION_REQUIRED")]
        public override PublicationKey Publication
        {
            get
            {
                return base.Publication;
            }
            set
            {
                base.Publication = value;
            }
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var valid in base.Validate(validationContext))
                yield return valid;
            if (this.Publication == null || this.Publication.ProductGuid == null)
                yield return ValidationResultFactory.Create(new ValidationResult("Publication is Required for Issue"), "ISSUE_PUBLICATION_REQUIRED");
        }
    }
    public class IssueForUpdate : Issue<ProductTypeKey>, IIssueForUpdate
    {
        [GlobalizedRequired("ISSUE_PRODUCTGUID_REQUIRED")]
        public override Guid? ProductGuid
        {
            get
            {
                return base.ProductGuid;
            }
            set
            {
                base.ProductGuid = value;
            }
        }
        [JsonIgnore]
        public override ProductTypeKey ProductType
        {
            get
            {
                return new ProductTypeKey() { ProductTypeId = (int)Enums.ProductTypes.Issue };
            }
            set
            {

            }
        }


        [UpdateOperationParameter]
        [ChildView]
        public PrimaryProductAddressForAddUpdate PrimaryAddress { get; set; }
        [UpdateOperationParameter]
        [ChildView]
        public PrimaryProductPhoneForAddUpdate PrimaryPhone { get; set; }
        [UpdateOperationParameter]
        [ChildView]
        public PrimaryProductEmailForAddUpdate PrimaryEmail { get; set; }
    }
    public class IssueResult : Issue<ProductType, StatusTypeResult, OrganizationResult, BusinessUnitResult, UserResult, PublicationResult>, IIssueResult
    {
        public PrimaryProductAddress PrimaryAddress { get; set; }

        public PrimaryProductPhone PrimaryPhone { get; set; }

        public PrimaryProductEmail PrimaryEmail { get; set; }
    }
}
