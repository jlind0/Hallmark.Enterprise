using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using HallData.Utilities;

namespace HallData.EMS.ApplicationViews.Results
{
    public interface ISupplierKey : IOrganizationKey<SupplierId>
    {
        Guid? SuppliedPartyGuid { get; set; }
    }
    public interface ISupplier<TPartyType, TTierType> : ISupplierKey, IOrganization<SupplierId, TPartyType, TTierType>
        where TPartyType: PartyTypeKey
        where TTierType: TierTypeKey
    {
        
    }
    public interface ISupplierStatus<TStatusType> : ISupplierKey
        where TStatusType: StatusTypeKey
    {
        TStatusType SuppliedRelationshipStatus { get; set; }
    }
    public interface ISupplier<TPartyType, TTierType, TStatusType> : ISupplier<TPartyType, TTierType>, ISupplierStatus<TStatusType>, IOrganization<SupplierId, TPartyType, TTierType, TStatusType>
        where TPartyType : PartyTypeKey
        where TTierType : TierTypeKey
        where TStatusType : StatusTypeKey { }
    public interface ISupplierOrganization<TOrganization> : ISupplierKey
        where TOrganization: IOrganizationKey
    {
        TOrganization SuppliedOrganization { get; set; }
    }
    public interface ISupplier<TPartyType, TTierType, TStatusType, TOrganization> : ISupplier<TPartyType, TTierType, TStatusType>, ISupplierOrganization<TOrganization>
        where TPartyType : PartyTypeKey
        where TTierType : TierTypeKey
        where TStatusType : StatusTypeKey
        where TOrganization : IOrganizationKey { }
    public interface ISupplierResultBase : ISupplier<PartyTypeResult, TierType, StatusTypeResult, OrganizationResult>, IOrganizationResultBase<SupplierId> { }
    public interface ISupplierResult : ISupplierResultBase, IOrganizationResult<SupplierId> { }
    public interface ISupplierId : IPartyId
    {
        Guid SuppliedPartyGuid { get; set; }
    }
    public struct SupplierId : ISupplierId
    {
        public Guid PartyGuid { get; set; }
        public Guid SuppliedPartyGuid { get; set; }
        public SupplierId(Guid party, Guid supplied) : this()
        {
            this.PartyGuid = party;
            this.SuppliedPartyGuid = supplied;
        }
        public override bool Equals(object obj)
        {
            ISupplierId id = obj as ISupplierId;
            if (id == null)
                return false;
            return this.SuppliedPartyGuid == id.SuppliedPartyGuid && this.PartyGuid == id.PartyGuid;
        }
        public override int GetHashCode()
        {
            return HashCodeProvider.BuildHashCode(this.PartyGuid, this.SuppliedPartyGuid);
        }
    }
}
