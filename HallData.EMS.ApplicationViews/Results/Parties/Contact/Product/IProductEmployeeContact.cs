using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Utilities;

namespace HallData.EMS.ApplicationViews.Results
{
    public interface IProductEmployeeContactId : IProductContactId, IEmployeeContactId { }
    public struct ProductEmployeeContactId : IProductEmployeeContactId
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
        public ProductEmployeeContactId(Guid productId, Guid partyId, string roleId, Guid? employerGuid = null)
            : this()
        {
            this.ContactRoleName = roleId;
            this.ProductGuid = productId;
            this.PartyGuid = partyId;
            this.EmployerGuid = employerGuid;
        }
        public override bool Equals(object obj)
        {
            IProductEmployeeContactId id = obj as IProductEmployeeContactId;
            if (id == null)
                return false;
            return this.ProductGuid == id.ProductGuid && this.PartyGuid == id.PartyGuid && this.ContactRoleName == id.ContactRoleName && this.EmployerGuid == id.EmployerGuid;
        }
        public override int GetHashCode()
        {
            return HashCodeProvider.BuildHashCode(this.ProductGuid, this.PartyGuid, this.ContactRoleName, this.EmployerGuid);
        }

    }
    public interface IProductEmployeeContactKey : IProductContactKey, IEmployeeContactKey { }
    public interface IProductEmployeeContactKey<TKey> : IProductContactKey<TKey>, IProductEmployeeContactKey, IEmployeeContactKey<TKey>
        where TKey : IProductEmployeeContactId { }
    public interface IProductEmployeeContactBase<TKey, TContactType> : IProductEmployeeContactKey<TKey>, IProductContactBase<TKey, TContactType>, IEmployeeContact<TKey, TContactType>
        where TKey : IProductEmployeeContactId
        where TContactType : ContactTypeKey { }
    public interface IProductEmployeeContact<TContactType> : IProductEmployeeContactBase<ProductEmployeeContactId, TContactType>
        where TContactType : ContactTypeKey { }
    public interface IProductEmployeeContactBase<TKey, TPartyType, TContactType, TTitleType> : IProductEmployeeContactBase<TKey, TContactType>, 
        IProductContactBase<TKey, TPartyType, TContactType>, IEmployeeContact<TKey, TPartyType, TContactType, TTitleType>
        where TKey : IProductEmployeeContactId
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey 
        where TTitleType: TitleTypeKey
    { }
    public interface IProductEmployeeContact<TPartyType, TContactType, TTitleType> : IProductEmployeeContact<TContactType>, IProductEmployeeContactBase<ProductEmployeeContactId, TPartyType, TContactType, TTitleType>
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey 
        where TTitleType: TitleTypeKey
    { }
    public interface IProductEmployeeContactWithStatusBase<TKey, TContactType, TStatusType> : IProductEmployeeContactBase<TKey, TContactType>, 
        IProductContactWithStatusBase<TKey, TContactType, TStatusType>, IEmployeeContactWithStatus<TKey, TContactType, TStatusType>
        where TKey : IProductEmployeeContactId
        where TContactType : ContactTypeKey
        where TStatusType : StatusTypeKey { }
    public interface IProductEmployeeContactWithStatus<TContactType, TStatusType> : IProductEmployeeContact<TContactType>,
        IProductEmployeeContactWithStatusBase<ProductEmployeeContactId, TContactType, TStatusType>
        where TContactType : ContactTypeKey
        where TStatusType : StatusTypeKey
    { }
    public interface IProductEmployeeContactBase<TKey, TPartyType, TContactType, TTitleType, TStatusType> : IProductEmployeeContactBase<TKey, TPartyType, TContactType, TTitleType>,
        IProductEmployeeContactWithStatusBase<TKey, TContactType, TStatusType>, IProductContactBase<TKey, TPartyType, TContactType, TStatusType>,
        IEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType>
        where TKey : IProductEmployeeContactId
        where TPartyType : PartyTypeKey
        where TStatusType : StatusTypeKey
        where TContactType : ContactTypeKey 
        where TTitleType: TitleTypeKey
    { }
    public interface IProductEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType> : IProductEmployeeContact<TPartyType, TContactType, TTitleType>, 
        IProductEmployeeContactBase<ProductEmployeeContactId, TPartyType, TContactType, TTitleType, TStatusType>
        where TPartyType : PartyTypeKey
        where TStatusType : StatusTypeKey
        where TContactType : ContactTypeKey
        where TTitleType : TitleTypeKey
    { }
    public interface IProductEmployeeContactBase<TKey, TPartyType, TContactType, TTitleType, TStatusType, TRole, TProduct, TEmployer> : IProductEmployeeContactBase<TKey, TPartyType, TContactType, TTitleType, TStatusType>,
        IProductContactBase<TKey, TPartyType, TContactType, TStatusType, TRole, TProduct>, IEmployeeContact<TKey, TPartyType, TContactType, TTitleType, TStatusType, TRole, TEmployer>
        where TKey : IProductEmployeeContactId
        where TPartyType : PartyTypeKey
        where TStatusType : StatusTypeKey
        where TContactType : ContactTypeKey
        where TRole : RoleKey
        where TProduct : IProductBaseKey
        where TEmployer: IOrganizationKey
        where TTitleType: TitleTypeKey
    { }
    public interface IProductEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType, TRole, TProduct, TEmployer> : IProductEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType>,
        IProductEmployeeContactBase<ProductEmployeeContactId, TPartyType, TContactType, TTitleType, TStatusType, TRole, TProduct, TEmployer>
        where TPartyType : PartyTypeKey
        where TStatusType : StatusTypeKey
        where TContactType : ContactTypeKey
        where TRole : RoleKey
        where TProduct : IProductBaseKey
        where TEmployer : IOrganizationKey
        where TTitleType : TitleTypeKey
    { }
    public interface IProductEmployeeContactForAddRelationship<TKey> : IProductEmployeeContactWithStatusBase<TKey, ContactTypeKey, StatusTypeKey>, 
        IProductContactForAddRelationship<TKey>, IEmployeeContactForAddRelationship<TKey>
        where TKey : IProductEmployeeContactId
    { }
    public interface IProductEmployeeContactForAddRelationship : IProductEmployeeContactWithStatus<ContactTypeKey, StatusTypeKey>, IProductEmployeeContactForAddRelationship<ProductEmployeeContactId> { }
    public interface IProductEmployeeContactForAddBase<TKey> : IProductEmployeeContactBase<TKey, PartyTypeKey, ContactTypeKey, TitleTypeKey, StatusTypeKey>,
        IProductContactForAddBase<TKey>, IEmployeeContactForAddBase<TKey>
        where TKey : IProductEmployeeContactId { }
    public interface IProductEmployeeContactForAddBase : IProductEmployeeContact<PartyTypeKey, ContactTypeKey, TitleTypeKey, StatusTypeKey>, IProductEmployeeContactForAddBase<ProductEmployeeContactId> { }
    public interface IProductEmployeeContactForAdd<TKey> : IProductEmployeeContactForAddBase<TKey>, IProductContactForAdd<TKey>, IEmployeeContactForAdd<TKey>
        where TKey : IProductEmployeeContactId { }
    public interface IProductEmployeeContactForAdd : IProductEmployeeContactForAddBase, IProductEmployeeContactForAdd<ProductEmployeeContactId> { }
    public interface IProductEmployeeContactForUpdateBase<TKey> : IProductEmployeeContactBase<TKey, PartyTypeKey, ContactTypeKey, TitleTypeKey>, IProductContactForUpdateBase<TKey>, IEmployeeContactForUpdateBase<TKey>
        where TKey : IProductEmployeeContactId { }
    public interface IProductEmployeeContactForUpdateBase : IProductEmployeeContact<PartyTypeKey, ContactTypeKey, TitleTypeKey>, IProductEmployeeContactForUpdateBase<ProductEmployeeContactId> { }
    public interface IProductEmployeeContactForUpdateRelationship<TKey> : IProductEmployeeContactBase<TKey, ContactTypeKey>, IProductContactForUpdateRelationship<TKey>, IEmployeeContactForUpdateRelationship<TKey>
        where TKey : IProductEmployeeContactId { }
    public interface IProductEmployeeContactForUpdateRelationship : IProductEmployeeContact<ContactTypeKey>, IProductEmployeeContactForUpdateRelationship<ProductEmployeeContactId> { }
    public interface IProductEmployeeContactForUpdate<TKey> : IProductEmployeeContactForUpdateBase<TKey>, IProductContactForUpdate<TKey>, IEmployeeContactForUpdate<TKey>
        where TKey : IProductEmployeeContactId
    { }
    public interface IProductEmployeeContactForUpdate : IProductEmployeeContactForUpdateBase, IProductEmployeeContactForUpdate<ProductEmployeeContactId> { }
    public interface IProductEmployeeContactResultBase<TKey, TProduct, TEmployer> : 
        IProductEmployeeContactBase<TKey, PartyTypeResult, ContactType, TitleTypeName, StatusTypeResult, RoleResult, TProduct, TEmployer>, 
        IProductContactResultBase<TKey, TProduct>, IEmployeeContactResultBase<TKey, TEmployer>
        where TKey : IProductEmployeeContactId
        where TProduct : IProductBaseKey
        where TEmployer: IOrganizationKey
    { }
    public interface IProductEmployeeContactResultBase<TKey, TEmployer> : IProductEmployeeContactResultBase<TKey, ProductResult, TEmployer>, IProductContactResultBase<TKey>
        where TEmployer: IOrganizationKey
        where TKey : IProductEmployeeContactId { }
    public interface IProductEmployeeContactResultBase<TKey> : IProductEmployeeContactResultBase<TKey, OrganizationResult>, IEmployeeContactResultBase<TKey> 
        where TKey: IProductEmployeeContactId
    { }
    public interface IProductEmployeeContactResultBase : IProductEmployeeContact<PartyTypeResult, ContactType, TitleTypeName, StatusTypeResult, RoleResult, ProductResult, OrganizationResult>, IProductEmployeeContactResultBase<ProductEmployeeContactId> { }
    public interface IProductEmployeeContactResult<TKey, TProduct, TEmployer> : IProductEmployeeContactResultBase<TKey, TProduct, TEmployer>, IProductContactResult<TKey, TProduct>, IEmployeeContactResult<TKey, TEmployer>
        where TKey : IProductEmployeeContactId
        where TProduct : IProductBaseKey
        where TEmployer: IOrganizationKey
    { }
    public interface IProductEmployeeContactResult<TKey, TEmployer> : IProductEmployeeContactResult<TKey, ProductResult, TEmployer>, IProductEmployeeContactResultBase<TKey, TEmployer>, IProductContactResult<TKey>
        where TKey : IProductEmployeeContactId 
        where TEmployer: IOrganizationKey
    { }
    public interface IProductEmployeeContactResult<TKey> : IProductEmployeeContactResult<TKey, OrganizationResult>, IEmployeeContactResult<TKey>
        where TKey : IProductEmployeeContactId { }
    public interface IProductEmployeeContactResult : IProductEmployeeContactResultBase, IProductEmployeeContactResult<ProductEmployeeContactId> { }
}
