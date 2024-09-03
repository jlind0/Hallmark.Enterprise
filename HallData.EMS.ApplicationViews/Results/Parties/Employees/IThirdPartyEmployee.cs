using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Utilities;

namespace HallData.EMS.ApplicationViews.Results
{
    public interface IThirdPartyEmployeeId : IEmployeeId { }
    public struct ThirdPartyEmployeeId : IThirdPartyEmployeeId
    {
        public Guid? EmployerGuid { get; set; }

        public Guid PartyGuid { get; set; }
        public ThirdPartyEmployeeId(Guid partyGuid, Guid? employerGuid = null) : this()
        {
            this.PartyGuid = partyGuid;
            this.EmployerGuid = employerGuid;
        }
        public override bool Equals(object obj)
        {
            IThirdPartyEmployeeId id = obj as IThirdPartyEmployeeId;
            if (id == null)
                return false;
            return id.PartyGuid == this.PartyGuid && id.EmployerGuid == this.EmployerGuid;
        }
        public override int GetHashCode()
        {
            return HashCodeProvider.BuildHashCode(this.PartyGuid, this.EmployerGuid);
        }



        
    }
    public interface IThirdPartyEmployeeKey : IEmployeeKey { }
    public interface IThirdPartyEmployeeKey<TKey> : IEmployeeKey<TKey>, IThirdPartyEmployeeKey
        where TKey : IThirdPartyEmployeeId { }
    public interface IThirdPartyEmployeeBase<TKey, TPartyType, TTitleType> : IThirdPartyEmployeeKey<TKey>, IEmployeeBase<TKey, TPartyType, TTitleType>
        where TKey : IThirdPartyEmployeeId
        where TPartyType : PartyTypeKey
        where TTitleType : TitleTypeKey { }
    public interface IThirdPartyEmployee<TPartyType, TTitleType> : IThirdPartyEmployeeBase<ThirdPartyEmployeeId, TPartyType, TTitleType>
        where TPartyType: PartyTypeKey
        where TTitleType : TitleTypeKey { }
    public interface IThirdPartyEmployeeBaseWithStatus<TKey, TStatusType> : IThirdPartyEmployeeKey<TKey>, IEmployeeBaseWithStatus<TKey, TStatusType>
        where TKey : IThirdPartyEmployeeId
        where TStatusType : StatusTypeKey { }
    public interface IThirdPartyEmployeeWithStatus<TStatusType> : IThirdPartyEmployeeBaseWithStatus<ThirdPartyEmployeeId, TStatusType>
        where TStatusType : StatusTypeKey { }
    public interface IThirdPartyEmployeeBase<TKey, TPartyType, TTitleType, TStatusType> : IThirdPartyEmployeeBase<TKey, TPartyType, TTitleType>, IThirdPartyEmployeeBaseWithStatus<TKey, TStatusType>, IEmployeeBase<TKey, TPartyType, TTitleType, TStatusType>
        where TKey: IThirdPartyEmployeeId
        where TPartyType: PartyTypeKey
        where TTitleType: TitleTypeKey
        where TStatusType : StatusTypeKey { }
    public interface IThirdPartyEmployee<TPartyType, TTitleType, TStatusType> : IThirdPartyEmployeeBase<ThirdPartyEmployeeId, TPartyType, TTitleType, TStatusType>, 
        IThirdPartyEmployee<TPartyType, TTitleType>, IThirdPartyEmployeeWithStatus<TStatusType>
        where TPartyType: PartyTypeKey
        where TTitleType: TitleTypeKey
        where TStatusType : StatusTypeKey { }
    public interface IThirdPartyEmployeeBase<TKey, TPartyType, TTitleType, TStatusType, TEmployer> : IThirdPartyEmployeeBase<TKey, TPartyType, TTitleType, TStatusType>,
        IEmployeeBase<TKey, TPartyType, TTitleType, TStatusType, TEmployer>
        where TPartyType : PartyTypeKey
        where TTitleType : TitleTypeKey
        where TStatusType : StatusTypeKey
        where TEmployer : IThirdPartyKey
        where TKey : IThirdPartyEmployeeId { }
     public interface IThirdPartyEmployee<TPartyType, TTitleType, TStatusType, TEmployer> : IThirdPartyEmployeeBase<ThirdPartyEmployeeId, TPartyType, TTitleType, TStatusType, TEmployer>,
         IThirdPartyEmployee<TPartyType, TTitleType, TStatusType>
        where TPartyType: PartyTypeKey
        where TTitleType: TitleTypeKey
        where TStatusType : StatusTypeKey
        where TEmployer: IThirdPartyKey
     {}
    public interface IThirdPartyEmployeeResultBase<TKey> : IThirdPartyEmployeeBase<TKey, PartyTypeResult, TitleTypeName, StatusTypeResult, ThirdPartyResult>, IEmployeeResultBase<TKey, ThirdPartyResult>
        where TKey : IThirdPartyEmployeeId { }
    public interface IThirdPartyEmployeeResultBase : IThirdPartyEmployeeResultBase<ThirdPartyEmployeeId>, IThirdPartyEmployee<PartyTypeResult, TitleTypeName, StatusTypeResult, ThirdPartyResult> { }
    public interface IThirdPartyEmployeeResult<TKey> : IThirdPartyEmployeeResultBase<TKey>, IEmployeeResult<TKey, ThirdPartyResult> 
        where TKey: IThirdPartyEmployeeId
    { }
    public interface IThirdPartyEmployeeResult : IThirdPartyEmployeeResult<ThirdPartyEmployeeId>, IThirdPartyEmployeeResultBase { }

}
