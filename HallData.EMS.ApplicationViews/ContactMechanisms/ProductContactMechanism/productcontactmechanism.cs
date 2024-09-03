using System;
using HallData.EMS.ApplicationViews.Enums;
using HallData.ApplicationViews;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using HallData.Validation;
using System.Collections.Generic;
using HallData.Utilities;

namespace HallData.EMS.ApplicationViews
{
    public class ProductContactMechanismKey : IProductContactMechanismKey, IContactMechanismHolderKey<ProductContactMechanismId>
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public virtual Guid? ProductGuid { get; set; }
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public virtual Guid? ContactMechanismGuid { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        [GlobalizedRequired("PRODUCTCONTACTMECHANISM_CONTACTMECHANISMTYPE_REQUIRED")]
        public virtual string ContactMechanismTypeName { get; set; }
        [JsonIgnore]
        public ProductContactMechanismId Key
        {
            get
            {
                return new ProductContactMechanismId(this.ProductGuid ?? Guid.Empty, this.ContactMechanismGuid, this.ContactMechanismTypeName);
            }
            set
            {
                this.ProductGuid = value.ProductGuid;
                this.ContactMechanismGuid = value.ContactMechanismGuid;
            }
        }


        
    }
    public class ProductContactMechanismForDeleteKeyBase : ProductContactMechanismKey
    {
        [GlobalizedRequired("PRODUCTCONTACTMECHANISM_CONTACTMECHANISMGUID_REQUIRED")]
        public override Guid? ContactMechanismGuid
        {
            get
            {
                return base.ContactMechanismGuid;
            }
            set
            {
                base.ContactMechanismGuid = value;
            }
        }
    }
    public class ProductContactMechanismForDeleteKeyProduct : ProductContactMechanismForDeleteKeyBase
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
    }
    public class ProductContactMechanismForDeleteKey : ProductContactMechanismForDeleteKeyBase
    {
        [GlobalizedRequired("PRODUCTCONTACTMECHANISM_PRODUCTGUID_REQUIRED")]
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
    }
    public interface IProductContactMechanismKey
    {
    }
    public class ProductContactMechanism<TContactMechanism, TMechanismType> : ProductContactMechanismKey,
        IContactMechanismHolder<ProductContactMechanismId, TContactMechanism, TMechanismType>, IValidatableObject
        where TContactMechanism : IContactMechanism<TMechanismType>
        where TMechanismType : MechanismTypeKey
    {
        [ChildView]
        [AddOperationParameter]
        [UpdateOperationParameter]
        public virtual TContactMechanism ContactMechanism { get; set; }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.ContactMechanismGuid == null && this.ContactMechanism == null)
                yield return ValidationResultFactory.Create(new ValidationResult("Contact Mechanism Required if Contact Mechanism Guid not provided"), "PRODUCTCONTACTMECHANISM_CONTACTMECHANISM_REQUIRED");
        }
    }
    public class ProductContactMechanism<TContactMechanism, TMechanismType, TStatusType> : 
        ProductContactMechanism<TContactMechanism, TMechanismType>, IContactMechanismHolder<ProductContactMechanismId, TContactMechanism, TMechanismType, TStatusType>
        where TContactMechanism : IContactMechanism<TMechanismType, TStatusType>
        where TMechanismType : MechanismTypeKey
        where TStatusType : StatusTypeKey
    {
        [AddOperationParameter]
        [ChildView]
        public TStatusType Status { get; set; }
    }
    public class ProductContactMechanismForAddBase<TContactMechanism> : ProductContactMechanism<TContactMechanism, MechanismTypeKey, StatusTypeKey>, 
        IContactMechanismHolderForAdd<ProductContactMechanismId, TContactMechanism>
        where TContactMechanism : IContactMechanismForAdd
    {
        [GlobalizedRequired("PRODUCTCONTACTMECHANISM_CONTACTMECHANISM_REQUIRED")]
        public override TContactMechanism ContactMechanism
        {
            get
            {
                return base.ContactMechanism;
            }
            set
            {
                base.ContactMechanism = value;
            }
        }
        [JsonIgnore]
        public override Guid? ContactMechanismGuid
        {
            get
            {
                return base.ContactMechanismGuid;
            }
            set
            {
                base.ContactMechanismGuid = value;
            }
        }
    }
    public class ProductContactMechanismForAddProduct<TContactMechanism> : ProductContactMechanismForAddBase<TContactMechanism>
        where TContactMechanism : IContactMechanismForAdd
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
    }
    public class ProductContactMechanismForAdd<TContactMechanism> : ProductContactMechanismForAddBase<TContactMechanism>
        where TContactMechanism: IContactMechanismForAdd
    {
        [GlobalizedRequired("PRODUCTCONTACTMECHANISM_PRODUCTGUID_REQUIRED")]
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
    }
    public class ProductContactMechanismForAddRelationshipBase : ProductContactMechanismKey, IContactMechanismHolderForAddRelationship<ProductContactMechanismId>
    {
        [AddOperationParameter]
        [ChildView]
        public StatusTypeKey Status { get; set; }
        [JsonIgnore]
        public override Guid? ContactMechanismGuid
        {
            get
            {
                return base.ContactMechanismGuid;
            }
            set
            {
                base.ContactMechanismGuid = value;
            }
        }
    }
    public class ProductContactMechanismForAddRelationshipProduct : ProductContactMechanismForAddRelationshipBase
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
    }
    public class ProductContactMechanismForAddRelationship : ProductContactMechanismForAddRelationshipBase
    {
        [GlobalizedRequired("PRODUCTCONTACTMECHANISM_PRODUCTGUID_REQUIRED")]
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
    }
    public class ProductContactMechanismForUpdateBase<TContactMechanism> : ProductContactMechanism<TContactMechanism, MechanismTypeKey>,
        IContactMechanismHolderForUpdate<ProductContactMechanismId, TContactMechanism>
        where TContactMechanism : IContactMechanismForUpdate
    {
        [GlobalizedRequired("PRODUCTCONTACTMECHANISM_CONTACTMECHANISMGUID_REQUIRED")]
        public override Guid? ContactMechanismGuid
        {
            get
            {
                return base.ContactMechanismGuid;
            }
            set
            {
                base.ContactMechanismGuid = value;
            }
        }
    }
    public class ProductContactMechanismForUpdateProduct<TContactMechanism> : ProductContactMechanismForUpdateBase<TContactMechanism>
        where TContactMechanism : IContactMechanismForUpdate
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
    }
    public class ProductContactMechanismForUpdate<TContactMechanism> : ProductContactMechanismForUpdateBase<TContactMechanism>
        where TContactMechanism: IContactMechanismForUpdate
    {
        [GlobalizedRequired("PRODUCTCONTACTMECHANISM_PRODUCTGUID_REQUIRED")]
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
    }
    public class ProductContactMechanismForUpdateRelationshipBase : ProductContactMechanismKey, IContactMechanismHolderForUpdateRelationship<ProductContactMechanismId>
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public Guid? OriginalContactMechanismGuid { get; set; }
        [GlobalizedRequired("PRODUCTCONTACTMECHANISM_CONTACTMECHANISMGUID_REQUIRED")]
        public override Guid? ContactMechanismGuid
        {
            get
            {
                return base.ContactMechanismGuid;
            }
            set
            {
                base.ContactMechanismGuid = value;
            }
        }

        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public string OriginalContactMechanismTypeName { get; set; }
    }
    public class ProductContactMechanismForUpdateRelationshipProduct : ProductContactMechanismForUpdateRelationshipBase
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
    }
    public class ProductContactMechanismForUpdateRelationship : ProductContactMechanismForUpdateRelationshipBase
    {
        [GlobalizedRequired("PRODUCTCONTACTMECHANISM_PRODUCTGUID_REQUIRED")]
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
    }
    public interface IProductContactMechanismResult : IProductContactMechanismKey
    {
        DateTime? CreateDate { get; set; }
        DateTime? UpdateDate { get; set; }
    }
    [ContactMechanismHolderDefaultView("ProductContactMechanism{0}Result")]
	public class ProductContactMechanismResult<TContactMechanism> : ProductContactMechanism<TContactMechanism, MechanismTypeResult, StatusTypeResult>,
        IContactMechanismHolderResult<ProductContactMechanismId, TContactMechanism>, IProductContactMechanismResult
        where TContactMechanism: IContactMechanismResult
    {
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public ContactMechanismType ContactMechanismType { get; set; }
    }
    public class ProductContactMechanismMerge<TContactMechanism> : ProductContactMechanismForAddBase<TContactMechanism>, IContactMechanismHolderForMerge<ProductContactMechanismId, TContactMechanism>
        where TContactMechanism: IContactMechanismForAdd
    {
        public MergeActions MergeAction { get; set; }
    }
    public class ProductContactMergeRelationship : ProductContactMechanismForUpdateRelationshipBase, IContactMechanismHolderForMergeRelationship<ProductContactMechanismId>
    {
        [AddOperationParameter]
        [ChildView]
        public StatusTypeKey Status { get; set; }

        public MergeActions MergeAction { get; set; }
    }
    public struct ProductContactMechanismId : IContactMechanismHolderId<Guid>, IProductId
    {
        public ProductContactMechanismId(Guid productId, Guid? contactMechanismId, string contactMechanismTypeName)
            : this()
        {
            this.ProductGuid = productId;
            this.ContactMechanismGuid = contactMechanismId ?? Guid.Empty;
            this.ContactMechanismTypeName = contactMechanismTypeName ?? "";
        }
        public Guid ProductGuid { get; set; }
        public Guid ContactMechanismGuid { get; set; }
        public override bool Equals(object obj)
        {
            ProductContactMechanismId? mech = obj as ProductContactMechanismId?;
            if (mech == null)
                return false;
            return mech.Value.ContactMechanismGuid == this.ContactMechanismGuid && mech.Value.ProductGuid == this.ProductGuid && this.ContactMechanismTypeName == mech.Value.ContactMechanismTypeName;
        }
        public override int GetHashCode()
        {
            return HashCodeProvider.BuildHashCode(this.ProductGuid, this.ContactMechanismGuid, this.ContactMechanismTypeName);
        }

        public Guid Id
        {
            get
            {
                return this.ProductGuid;
            }
            set
            {
                this.ProductGuid = value;
            }
        }
        public string ContactMechanismTypeName { get; set; }
    }
}
