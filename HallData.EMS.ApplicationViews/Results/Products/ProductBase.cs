using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HallData.ApplicationViews;
using Newtonsoft.Json;
using HallData.Validation;
namespace HallData.EMS.ApplicationViews
{
    
    public class ProductBaseKey : IProductBaseKey
    {
        public ProductBaseKey()
        {
            this.InstanceGuid = Guid.NewGuid();
        }
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public virtual Guid? ProductGuid { get; set; }
        [JsonIgnore]
        public Guid Key
        {
            get
            {
                return this.ProductGuid ?? Guid.Empty;
            }
            set
            {
                this.ProductGuid = value;
            }
        }

        public Guid InstanceGuid { get; private set; }
    }

    public class ProductBase<TProductType> : ProductBaseKey, IProductBase<TProductType>, IValidatableObject
        where TProductType: ProductTypeKey
    {
        [ChildView]
        [AddOperationParameter]
        [UpdateOperationParameter]
        public virtual TProductType ProductType { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public virtual string Name { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public virtual string Code { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public virtual string Caption { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(this.ProductGuid == null)
            {
                if (this.ProductType == null || this.ProductType.ProductTypeId == null)
                    yield return ValidationResultFactory.Create(new ValidationResult("The Product Must have a Product Type if Product Guid is not provided"), "PRODUCT_PRODUCTTYPE_REQUIRED");
                if (string.IsNullOrWhiteSpace(this.Name))
                    yield return ValidationResultFactory.Create(new ValidationResult(""), "PRODUCT_NAME_REQUIRED");
                if (string.IsNullOrWhiteSpace(this.Code))
                    yield return ValidationResultFactory.Create(new ValidationResult(""), "PRODUCT_CODE_REQUIRED");
                if (string.IsNullOrWhiteSpace(this.Caption))
                    yield return ValidationResultFactory.Create(new ValidationResult(""), "PRODUCT_CAPTION_REQUIRED");
            }
        }
    }
    public class ProductBase<TProductType, TStatusType> : ProductBase<TProductType>, IProductBaseWithStatus<TProductType, TStatusType>
        where TProductType: ProductTypeKey
        where TStatusType: StatusTypeKey
    {
        [AddOperationParameter]
        [ChildView]
        public virtual TStatusType Status { get; set; }
    }
    [Obsolete]
    public class ProductBase : ProductBase<ProductTypeKey, StatusTypeKey> { }
    
}
