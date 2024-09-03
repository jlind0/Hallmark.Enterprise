using System;
using HallData.Utilities;

namespace HallData.EMS.ApplicationViews.Results
{
    public interface IThirdPartyId : IPartyId
    {
        Guid ThirdPartyOfGuid { get; set; }
        string ThirdPartyRoleName { get; set; }
    }
    public interface IThirdPartyKey : IOrganizationKey
    {
        Guid? ThirdPartyOfGuid { get; set; }
        string ThirdPartyRoleName { get; set; }
    }
    public interface IThirdPartyKey<TKey> : IThirdPartyKey, IOrganizationKey<TKey>
        where TKey : IThirdPartyId { }
    public interface IThirdPartyBase<TKey, TPartyType, TTierType> : IThirdPartyKey<TKey>, IOrganization<TKey, TPartyType, TTierType>
        where TPartyType : PartyTypeKey
        where TKey: IThirdPartyId
        where TTierType: TierTypeKey
    {
    }
    public interface IThirdParty<TPartyType, TTierType> : IThirdPartyBase<ThirdPartyId, TPartyType, TTierType>
        where TPartyType : PartyTypeKey
        where TTierType: TierTypeKey
    { }
    public interface IThirdPartyBaseWithStatus<TKey, TStatusType> : IThirdPartyKey<TKey>
        where TKey: IThirdPartyId
        where TStatusType: StatusTypeKey
    {
        TStatusType ThirdPartyRelationshipStatus { get; set; }
    }
    public interface IThirdPartyWithStatus<TStatusType> : IThirdPartyBaseWithStatus<ThirdPartyId, TStatusType> 
        where TStatusType: StatusTypeKey
    { }
    public interface IThirdPartyBase<TKey, TPartyType, TTierType, TStatusType> : IThirdPartyBase<TKey, TPartyType, TTierType>, IThirdPartyBaseWithStatus<TKey, TStatusType>, 
        IOrganization<TKey, TPartyType, TTierType, TStatusType>
        where TPartyType: PartyTypeKey
        where TStatusType: StatusTypeKey
        where TKey: IThirdPartyId
        where TTierType: TierTypeKey
    {
        
    }
    public interface IThirdParty<TPartyType, TTierType, TStatusType> : IThirdPartyBase<ThirdPartyId, TPartyType, TTierType, TStatusType>, 
        IThirdPartyWithStatus<TStatusType>, 
        IThirdParty<TPartyType, TTierType>
        where TPartyType: PartyTypeKey
        where TStatusType: StatusTypeKey
        where TTierType: TierTypeKey
    {

    }
    public interface IThirdPartyBaseWithRole<TKey, TStatusType, TThirdPartyOf, TRole> : IThirdPartyBaseWithStatus<TKey, TStatusType>
        where TStatusType: StatusTypeKey
        where TThirdPartyOf: IOrganizationKey
        where TRole: RoleKey
        where TKey: IThirdPartyId
    {
        TThirdPartyOf ThirdPartyOf { get; set; }
        TRole ThirdPartyRole { get; set; }
    }
    public interface IThirdPartyWithRole<TStatusType, TThirdPartyOf, TRole> : IThirdPartyBaseWithRole<ThirdPartyId, TStatusType, TThirdPartyOf, TRole>, IThirdPartyWithStatus<TStatusType>
        where TStatusType: StatusTypeKey
        where TThirdPartyOf: IOrganizationKey
        where TRole : RoleKey { }
    public interface IThirdPartyBase<TKey, TPartyType, TTierType, TStatusType, TThirdPartyOf, TRole> : IThirdPartyBase<TKey, TPartyType, TTierType, TStatusType>, 
        IThirdPartyBaseWithRole<TKey, TStatusType, TThirdPartyOf, TRole>
        where TPartyType: PartyTypeKey
        where TStatusType: StatusTypeKey
        where TThirdPartyOf: IOrganizationKey
        where TRole: RoleKey
        where TKey: IThirdPartyId
        where TTierType: TierTypeKey
    {
        
    }
    public interface IThirdParty<TPartyType, TTierType, TStatusType, TThirdPartyOf, TRole> : IThirdPartyBase<ThirdPartyId, TPartyType, TTierType, TStatusType, TThirdPartyOf, TRole>, 
        IThirdParty<TPartyType, TTierType, TStatusType>, IThirdPartyWithRole<TStatusType, TThirdPartyOf, TRole>
        where TPartyType: PartyTypeKey
        where TStatusType: StatusTypeKey
        where TThirdPartyOf: IOrganizationKey
        where TTierType: TierTypeKey
        where TRole : RoleKey { }
    public interface IThirdPartyForAddRelationship<TKey> : IThirdPartyBaseWithStatus<TKey, StatusTypeKey>
        where TKey : IThirdPartyId { }
    public interface IThirdPartyForAddRelationship : IThirdPartyForAddRelationship<ThirdPartyId>, IThirdPartyWithStatus<StatusTypeKey> { }
    public interface IThirdPartyForAddRelationshipBase : IThirdPartyForAddRelationship<ThirdPartyId>, IThirdPartyWithStatus<StatusTypeKey> { }
    public interface IThirdPartyForAddBase<TKey> : IThirdPartyBase<TKey, PartyTypeKey, TierTypeKey, StatusTypeKey>, IOrganizationForAddBase<TKey>
        where TKey: IThirdPartyId
    { }
    public interface IThirdPartyForAddBase : IThirdPartyForAddRelationshipBase, IThirdPartyForAddBase<ThirdPartyId>, IThirdParty<PartyTypeKey, TierTypeKey, StatusTypeKey> { }
    public interface IThirdPartyForAdd<TKey> :  IThirdPartyForAddBase<TKey>, IOrganizationForAdd<TKey>
        where TKey: IThirdPartyId
    { }
    public interface IThirdPartyForAdd : IThirdPartyForAdd<ThirdPartyId>, IThirdPartyForAddBase { }
    public interface IThirdPartyForUpdateRelationship<TKey>: IThirdPartyKey<TKey> 
        where TKey: IThirdPartyId
    {
        Guid? OriginalPartyGuid { get; set; }
        Guid? OriginalThirdPartyOfGuid { get; set; }
        string OriginalThirdPartyRoleName { get; set; }
    }
    public interface IThirdPartyForUpdateRelationship : IThirdPartyForUpdateRelationship<ThirdPartyId> { }
    public interface IThirdPartyForUpdateBase<TKey> : IThirdPartyBase<TKey, PartyTypeKey, TierTypeKey>, IOrganizationForUpdateBase<TKey> 
        where TKey: IThirdPartyId{ }
    public interface IThirdPartyForUpdateBase : IThirdPartyForUpdateBase<ThirdPartyId>, IThirdParty<PartyTypeKey, TierTypeKey> { }
    public interface IThirdPartyForUpdate<TKey> : IThirdPartyForUpdateBase<TKey>, IOrganizationForUpdate<TKey>
        where TKey: IThirdPartyId
    { }
    public interface IThirdPartyForUpdate : IThirdPartyForUpdate<ThirdPartyId>, IThirdPartyForUpdateBase { }
    public interface IThirdPartyResultBase<TKey> : IThirdPartyBase<TKey, PartyTypeResult, TierType, StatusTypeResult, OrganizationResult, RoleResult>, IPartyResultBase<TKey> 
        where TKey: IThirdPartyId{ }
    public interface IThirdPartyResultBase : IThirdPartyResultBase<ThirdPartyId>, IThirdParty<PartyTypeResult, TierType, StatusTypeResult, OrganizationResult, RoleResult> { }
    public interface IThirdPartyResult<TKey> : IThirdPartyResultBase<TKey>, IOrganizationResult<TKey> 
        where TKey: IThirdPartyId
    { }
    public interface IThirdPartyResult : IThirdPartyResult<ThirdPartyId>, IThirdPartyResultBase { }
    public struct ThirdPartyId : IThirdPartyId
    {
        public Guid PartyGuid { get; set; }
        public Guid ThirdPartyOfGuid { get; set; }
        public string ThirdPartyRoleName { get; set; }
        public ThirdPartyId(Guid partyGuid, Guid thirdPartyOfGuid, string roleId) : this()
        {
            this.PartyGuid = partyGuid;
            this.ThirdPartyOfGuid = thirdPartyOfGuid;
            this.ThirdPartyRoleName = roleId;
        }
        public override bool Equals(object obj)
        {
            IThirdPartyId id = obj as IThirdPartyId;
            if (id == null)
                return false;
            return id.PartyGuid == this.PartyGuid && id.ThirdPartyOfGuid == this.ThirdPartyOfGuid && this.ThirdPartyRoleName == id.ThirdPartyRoleName;
        }
        public override int GetHashCode()
        {
            return HashCodeProvider.BuildHashCode(this.PartyGuid, this.ThirdPartyOfGuid, this.ThirdPartyRoleName);
        }
    }
}
