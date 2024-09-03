using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Utilities;

namespace HallData.EMS.ApplicationViews.Results
{
    public interface IProductSupplierEmployeeContactId : IProductEmployeeContactId, ISupplierEmployeeId { }
    public struct ProductSupplierEmployeeContactId : IProductSupplierEmployeeContactId
    {
        public string ContactRoleName { get; set; }
        public Guid PartyGuid { get; set; }
        public Guid ProductGuid { get; set; }
        public Guid? EmployerGuid { get; set; }
        public ProductPartyId Id
        {
            get
            {
                return new ProductPartyId(this.ProductGuid, this.PartyGuid);
            }
            set
            {
                this.PartyGuid = value.PartyGuid;
                this.ProductGuid = value.ProductGuid;
            }
        }
        public ProductSupplierEmployeeContactId(Guid productId, Guid partyId, string roleId, Guid? employerGuid = null)
            : this()
        {
            this.ContactRoleName = roleId;
            this.ProductGuid = productId;
            this.PartyGuid = partyId;
            this.EmployerGuid = employerGuid;
        }
        public override bool Equals(object obj)
        {
            IProductSupplierEmployeeContactId id = obj as IProductSupplierEmployeeContactId;
            if (id == null)
                return false;
            return this.ProductGuid == id.ProductGuid && this.PartyGuid == id.PartyGuid && this.ContactRoleName == id.ContactRoleName && this.EmployerGuid == id.EmployerGuid;
        }
        public override int GetHashCode()
        {
            return HashCodeProvider.BuildHashCode(this.ProductGuid, this.PartyGuid, this.ContactRoleName, this.EmployerGuid);
        }

    }
    public interface IProductSupplierEmployeeContactKey : IProductEmployeeContactKey<ProductSupplierEmployeeContactId>, ISupplierEmployeeKey<ProductSupplierEmployeeContactId> { }
    public interface IProductSupplierEmployeeContact<TContactType> : IProductEmployeeContactBase<ProductSupplierEmployeeContactId, TContactType>, IProductSupplierEmployeeContactKey
        where TContactType : ContactTypeKey { }
    public interface IProductSupplierEmployeeContact<TPartyType, TContactType, TTitleType> : IProductSupplierEmployeeContact<TContactType>, IProductEmployeeContactBase<ProductSupplierEmployeeContactId, TPartyType, TContactType, TTitleType>
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey
        where TTitleType : TitleTypeKey
    { }
    public interface IProductSupplierEmployeeContactWithStatus<TContactType, TStatusType> : IProductSupplierEmployeeContact<TContactType>,
        IProductEmployeeContactWithStatusBase<ProductSupplierEmployeeContactId, TContactType, TStatusType>
        where TContactType : ContactTypeKey
        where TStatusType : StatusTypeKey
    { }
    public interface IProductSupplierEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType> : IProductSupplierEmployeeContact<TPartyType, TContactType, TTitleType>,
        IProductEmployeeContactBase<ProductSupplierEmployeeContactId, TPartyType, TContactType, TTitleType, TStatusType>
        where TPartyType : PartyTypeKey
        where TStatusType : StatusTypeKey
        where TContactType : ContactTypeKey
        where TTitleType : TitleTypeKey
    { }
    public interface IProductSupplierEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType, TRole, TProduct, TEmployer> : IProductSupplierEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType>,
        IProductEmployeeContactBase<ProductSupplierEmployeeContactId, TPartyType, TContactType, TTitleType, TStatusType, TRole, TProduct, TEmployer>
        where TPartyType : PartyTypeKey
        where TStatusType : StatusTypeKey
        where TContactType : ContactTypeKey
        where TRole : RoleKey
        where TProduct : IProductBaseKey
        where TEmployer : ISupplierKey
        where TTitleType : TitleTypeKey
    { }
    public interface IProductSupplierEmployeeContactForAddRelationship : IProductSupplierEmployeeContactWithStatus<ContactTypeKey, StatusTypeKey>, IProductEmployeeContactForAddRelationship<ProductSupplierEmployeeContactId> { }
    public interface IProductSupplierEmployeeContactForAddBase : IProductSupplierEmployeeContact<PartyTypeKey, ContactTypeKey, TitleTypeKey, StatusTypeKey>, IProductEmployeeContactForAddBase<ProductSupplierEmployeeContactId> { }
    public interface IProductSupplierEmployeeContactForAdd : IProductSupplierEmployeeContactForAddBase, IProductEmployeeContactForAdd<ProductSupplierEmployeeContactId> { }
    public interface IProductSupplierEmployeeContactForUpdateBase : IProductSupplierEmployeeContact<PartyTypeKey, ContactTypeKey, TitleTypeKey>, IProductEmployeeContactForUpdateBase<ProductSupplierEmployeeContactId> { }
    public interface IProductSupplierEmployeeContactForUpdateRelationship : IProductSupplierEmployeeContact<ContactTypeKey>, IProductEmployeeContactForUpdateRelationship<ProductSupplierEmployeeContactId> { }
    public interface IProductSupplierEmployeeContactForUpdate : IProductSupplierEmployeeContactForUpdateBase, IProductEmployeeContactForUpdate<ProductSupplierEmployeeContactId> { }
    public interface IProductSupplierEmployeeContactResultBase : IProductSupplierEmployeeContact<PartyTypeResult, ContactType, TitleTypeName, StatusTypeResult, RoleResult, ProductResult, SupplierResult>, IProductEmployeeContactResultBase<ProductSupplierEmployeeContactId, SupplierResult> { }
    public interface IProductSupplierEmployeeContactResult : IProductSupplierEmployeeContactResultBase, IProductEmployeeContactResult<ProductSupplierEmployeeContactId, SupplierResult> { }
}
