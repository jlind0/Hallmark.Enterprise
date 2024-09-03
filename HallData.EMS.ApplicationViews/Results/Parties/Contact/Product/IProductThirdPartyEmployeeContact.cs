using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Utilities;

namespace HallData.EMS.ApplicationViews.Results
{
    public interface IProductThirdPartyEmployeeContactId : IProductEmployeeContactId, IThirdPartyEmployeeId { }
    public struct ProductThirdPartyEmployeeContactId : IProductThirdPartyEmployeeContactId
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
        public ProductThirdPartyEmployeeContactId(Guid productId, Guid partyId, string roleId, Guid? employerGuid = null)
            : this()
        {
            this.ContactRoleName = roleId;
            this.ProductGuid = productId;
            this.PartyGuid = partyId;
            this.EmployerGuid = employerGuid;
        }
        public override bool Equals(object obj)
        {
            IProductThirdPartyEmployeeContactId id = obj as IProductThirdPartyEmployeeContactId;
            if (id == null)
                return false;
            return this.ProductGuid == id.ProductGuid && this.PartyGuid == id.PartyGuid && this.ContactRoleName == id.ContactRoleName && this.EmployerGuid == id.EmployerGuid;
        }
        public override int GetHashCode()
        {
            return HashCodeProvider.BuildHashCode(this.ProductGuid, this.PartyGuid, this.ContactRoleName, this.EmployerGuid);
        }

    }
    public interface IProductThirdPartyEmployeeContactKey : IProductEmployeeContactKey<ProductThirdPartyEmployeeContactId>, IThirdPartyEmployeeKey<ProductThirdPartyEmployeeContactId> { }
    public interface IProductThirdPartyEmployeeContact<TContactType> : IProductEmployeeContactBase<ProductThirdPartyEmployeeContactId, TContactType>, IProductThirdPartyEmployeeContactKey
        where TContactType : ContactTypeKey { }
    public interface IProductThirdPartyEmployeeContact<TPartyType, TContactType, TTitleType> : IProductThirdPartyEmployeeContact<TContactType>, IProductEmployeeContactBase<ProductThirdPartyEmployeeContactId, TPartyType, TContactType, TTitleType>
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey
        where TTitleType : TitleTypeKey
    { }
    public interface IProductThirdPartyEmployeeContactWithStatus<TContactType, TStatusType> : IProductThirdPartyEmployeeContact<TContactType>,
        IProductEmployeeContactWithStatusBase<ProductThirdPartyEmployeeContactId, TContactType, TStatusType>
        where TContactType : ContactTypeKey
        where TStatusType : StatusTypeKey
    { }
    public interface IProductThirdPartyEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType> : IProductThirdPartyEmployeeContact<TPartyType, TContactType, TTitleType>,
        IProductEmployeeContactBase<ProductThirdPartyEmployeeContactId, TPartyType, TContactType, TTitleType, TStatusType>
        where TPartyType : PartyTypeKey
        where TStatusType : StatusTypeKey
        where TContactType : ContactTypeKey
        where TTitleType : TitleTypeKey
    { }
    public interface IProductThirdPartyEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType, TRole, TProduct, TEmployer> : IProductThirdPartyEmployeeContact<TPartyType, TContactType, TTitleType, TStatusType>,
        IProductEmployeeContactBase<ProductThirdPartyEmployeeContactId, TPartyType, TContactType, TTitleType, TStatusType, TRole, TProduct, TEmployer>
        where TPartyType : PartyTypeKey
        where TStatusType : StatusTypeKey
        where TContactType : ContactTypeKey
        where TRole : RoleKey
        where TProduct : IProductBaseKey
        where TEmployer : IThirdPartyKey
        where TTitleType : TitleTypeKey
    { }
    public interface IProductThirdPartyEmployeeContactForAddRelationship : IProductThirdPartyEmployeeContactWithStatus<ContactTypeKey, StatusTypeKey>, IProductEmployeeContactForAddRelationship<ProductThirdPartyEmployeeContactId> { }
    public interface IProductThirdPartyEmployeeContactForAddBase : IProductThirdPartyEmployeeContact<PartyTypeKey, ContactTypeKey, TitleTypeKey, StatusTypeKey>, IProductEmployeeContactForAddBase<ProductThirdPartyEmployeeContactId> { }
    public interface IProductThirdPartyEmployeeContactForAdd : IProductThirdPartyEmployeeContactForAddBase, IProductEmployeeContactForAdd<ProductThirdPartyEmployeeContactId> { }
    public interface IProductThirdPartyEmployeeContactForUpdateBase : IProductThirdPartyEmployeeContact<PartyTypeKey, ContactTypeKey, TitleTypeKey>, IProductEmployeeContactForUpdateBase<ProductThirdPartyEmployeeContactId> { }
    public interface IProductThirdPartyEmployeeContactForUpdateRelationship : IProductThirdPartyEmployeeContact<ContactTypeKey>, IProductEmployeeContactForUpdateRelationship<ProductThirdPartyEmployeeContactId> { }
    public interface IProductThirdPartyEmployeeContactForUpdate : IProductThirdPartyEmployeeContactForUpdateBase, IProductEmployeeContactForUpdate<ProductThirdPartyEmployeeContactId> { }
    public interface IProductThirdPartyEmployeeContactResultBase : IProductThirdPartyEmployeeContact<PartyTypeResult, ContactType, TitleTypeName, StatusTypeResult, RoleResult, ProductResult, ThirdPartyResult>, IProductEmployeeContactResultBase<ProductThirdPartyEmployeeContactId, ThirdPartyResult> { }
    public interface IProductThirdPartyEmployeeContactResult : IProductThirdPartyEmployeeContactResultBase, IProductEmployeeContactResult<ProductThirdPartyEmployeeContactId, ThirdPartyResult> { }
}
