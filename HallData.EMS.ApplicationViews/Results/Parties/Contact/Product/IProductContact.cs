using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Utilities;

namespace HallData.EMS.ApplicationViews.Results
{
    public struct ProductPartyId : IPartyId, IProductId
    {
        public Guid PartyGuid { get; set; }
        public Guid ProductGuid { get; set; }
        public ProductPartyId(Guid productId, Guid partyId) : this()
        {
            this.PartyGuid = partyId;
            this.ProductGuid = productId;
        }
        public override bool Equals(object obj)
        {
            ProductPartyId? id = obj as ProductPartyId?;
            if (id == null)
                return false;
            return this.PartyGuid == id.Value.PartyGuid && this.ProductGuid == id.Value.ProductGuid;
        }
        public override int GetHashCode()
        {
            return HashCodeProvider.BuildHashCode(this.ProductGuid, this.PartyGuid);
        }
    }
    public interface IProductContactId : IContactId<ProductPartyId>, IProductId { }
    public struct ProductContactId : IProductContactId
    {
        public string ContactRoleName { get; set; }
        public Guid PartyGuid { get; set; }
        public Guid ProductGuid { get; set; }
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
        public ProductContactId(Guid productId, Guid partyId, string roleId) : this()
        {
            this.ContactRoleName = roleId;
            this.ProductGuid = productId;
            this.PartyGuid = partyId;
        }
        public override bool Equals(object obj)
        {
            IProductContactId id = obj as IProductContactId;
            if (id == null)
                return false;
            return this.ProductGuid == id.ProductGuid && this.PartyGuid == id.PartyGuid && this.ContactRoleName == id.ContactRoleName;
        }
        public override int GetHashCode()
        {
            return HashCodeProvider.BuildHashCode(this.ProductGuid, this.PartyGuid, this.ContactRoleName);
        }
    }
    public interface IProductContactKey : IContactKey
    {
        Guid? ProductGuid { get; set; }
    }
    public interface IProductContactKey<TKey> : IContactKey<TKey>, IProductContactKey
        where TKey : IProductContactId { }
    public interface IProductContactBase<TKey, TContactType> : IProductContactKey<TKey>, IContact<TKey, TContactType>
        where TKey : IProductContactId
        where TContactType : ContactTypeKey { }
    public interface IProductContact<TContactType> : IProductContactBase<ProductContactId, TContactType>
        where TContactType : ContactTypeKey { }
    public interface IProductContactBase<TKey, TPartyType, TContactType> : IProductContactBase<TKey, TContactType>, IContact<TKey, TPartyType, TContactType>
        where TKey : IProductContactId
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey { }
    public interface IProductContact<TPartyType, TContactType> : IProductContact<TContactType>, IProductContactBase<ProductContactId, TPartyType, TContactType>
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey { }
    public interface IProductContactWithStatusBase<TKey, TContactType, TStatusType> : IProductContactBase<TKey, TContactType>, IContactWithStatus<TKey, TContactType, TStatusType>
        where TKey : IProductContactId
        where TContactType : ContactTypeKey
        where TStatusType : StatusTypeKey { }
    public interface IProductContactWithStatus<TContactType, TStatusType> : IProductContact<TContactType>, 
        IProductContactWithStatusBase<ProductContactId, TContactType, TStatusType>
        where TContactType : ContactTypeKey
        where TStatusType : StatusTypeKey
    { }
    public interface IProductContactBase<TKey, TPartyType, TContactType, TStatusType> : IProductContactBase<TKey, TPartyType, TContactType>,
        IProductContactWithStatusBase<TKey, TContactType, TStatusType>, IContact<TKey, TPartyType, TContactType, TStatusType>
        where TKey : IProductContactId
        where TPartyType : PartyTypeKey
        where TStatusType : StatusTypeKey
        where TContactType : ContactTypeKey { }
    public interface IProductContact<TPartyType, TContactType, TStatusType> : IProductContact<TPartyType, TContactType>, 
        IProductContactBase<ProductContactId, TPartyType, TContactType, TStatusType>
        where TPartyType : PartyTypeKey
        where TStatusType : StatusTypeKey
        where TContactType : ContactTypeKey
    { }
    public interface IProductContactBase<TKey, TPartyType, TContactType, TStatusType, TRole, TProduct> : IProductContactBase<TKey, TPartyType, TContactType, TStatusType>,
        IContact<TKey, TPartyType, TContactType, TStatusType, TRole>
        where TKey : IProductContactId
        where TPartyType : PartyTypeKey
        where TStatusType : StatusTypeKey
        where TContactType : ContactTypeKey
        where TRole : RoleKey 
        where TProduct: IProductBaseKey
    {
        TProduct Product { get; set; }
    }
    public interface IProductContact<TPartyType, TContactType, TStatusType, TRole, TProduct> : IProductContact<TPartyType, TContactType, TStatusType>,
        IProductContactBase<ProductContactId, TPartyType, TContactType, TStatusType, TRole, TProduct>
        where TPartyType : PartyTypeKey
        where TStatusType : StatusTypeKey
        where TContactType : ContactTypeKey
        where TRole : RoleKey 
        where TProduct: IProductBaseKey
    { }
    public interface IProductContactForAddRelationship<TKey> : IProductContactWithStatusBase<TKey, ContactTypeKey, StatusTypeKey>, IContactForAddRelationship<TKey>
        where TKey : IProductContactId
    { }
    public interface IProductContactForAddRelationship : IProductContactWithStatus<ContactTypeKey, StatusTypeKey>, IProductContactForAddRelationship<ProductContactId> { }
    public interface IProductContactForAddBase<TKey> : IProductContactBase<TKey, PartyTypeKey, ContactTypeKey, StatusTypeKey>, IContactForAddBase<TKey>
        where TKey : IProductContactId { }
    public interface IProductContactForAddBase : IProductContact<PartyTypeKey, ContactTypeKey, StatusTypeKey>, IProductContactForAddBase<ProductContactId> { }
    public interface IProductContactForAdd<TKey> : IProductContactForAddBase<TKey>, IContactForAdd<TKey>
        where TKey : IProductContactId { }
    public interface IProductContactForAdd : IProductContactForAddBase, IProductContactForAdd<ProductContactId> { }
    public interface IProductContactForUpdateBase<TKey> : IProductContactBase<TKey, PartyTypeKey, ContactTypeKey>, IContactForUpdateBase<TKey>
        where TKey : IProductContactId { }
    public interface IProductContactForUpdateBase : IProductContact<PartyTypeKey, ContactTypeKey>, IProductContactForUpdateBase<ProductContactId> { }
    public interface IProductContactForUpdateRelationship<TKey> : IProductContactBase<TKey, ContactTypeKey>, IContactForUpdateRelationship<TKey>
        where TKey : IProductContactId
    {
        Guid? OriginalProductGuid { get; set; }
    }
    public interface IProductContactForUpdateRelationship : IProductContact<ContactTypeKey>, IProductContactForUpdateRelationship<ProductContactId> { }
    public interface IProductContactForUpdate<TKey> : IProductContactForUpdateBase<TKey>, IContactForUpdate<TKey>
        where TKey : IProductContactId
    { }
    public interface IProductContactForUpdate : IProductContactForUpdateBase, IProductContactForUpdate<ProductContactId> { }
    public interface IProductContactResultBase<TKey, TProduct> : IProductContactBase<TKey, PartyTypeResult, ContactType, StatusTypeResult, RoleResult, TProduct>, IContactResultBase<TKey>
        where TKey : IProductContactId 
        where TProduct: IProductBaseKey
    { }
    public interface IProductContactResultBase<TKey> : IProductContactResultBase<TKey, ProductResult>
        where TKey : IProductContactId { }
    public interface IProductContactResultBase : IProductContact<PartyTypeResult, ContactType, StatusTypeResult, RoleResult, ProductResult>, IProductContactResultBase<ProductContactId> { }
    public interface IProductContactResult<TKey, TProduct> : IProductContactResultBase<TKey, TProduct>, IContactResult<TKey>
        where TKey : IProductContactId
        where TProduct: IProductBaseKey
    { }
    public interface IProductContactResult<TKey> : IProductContactResult<TKey, ProductResult>, IProductContactResultBase<TKey>
        where TKey : IProductContactId { }
    public interface IProductContactResult : IProductContactResultBase, IProductContactResult<ProductContactId> { }
}
