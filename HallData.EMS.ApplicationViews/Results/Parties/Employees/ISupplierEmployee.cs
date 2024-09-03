using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Utilities;

namespace HallData.EMS.ApplicationViews.Results
{
    public interface ISupplierEmployeeId : IEmployeeId { }
    public struct SupplierEmployeeId : ISupplierEmployeeId
    {


        public Guid? EmployerGuid { get; set; }

        public Guid PartyGuid { get; set; }
        public SupplierEmployeeId(Guid partyGuid, Guid? employerGuid = null)
            : this()
        {
            this.PartyGuid = partyGuid;
            this.EmployerGuid = employerGuid;
        }
        public override bool Equals(object obj)
        {
            ISupplierEmployeeId id = obj as ISupplierEmployeeId;
            if (id == null)
                return false;
            return id.PartyGuid == this.PartyGuid && id.EmployerGuid == this.EmployerGuid;
        }
        public override int GetHashCode()
        {
            return HashCodeProvider.BuildHashCode(this.PartyGuid, this.EmployerGuid);
        }


    }
    public interface ISupplierEmployeeKey : IEmployeeKey { }
    public interface ISupplierEmployeeKey<TKey> : IEmployeeKey<TKey>, ISupplierEmployeeKey
        where TKey : ISupplierEmployeeId { }
    public interface ISupplierEmployeeBase<TKey, TPartyType, TTitleType> : ISupplierEmployeeKey<TKey>, IEmployeeBase<TKey, TPartyType, TTitleType>
        where TKey : ISupplierEmployeeId
        where TPartyType : PartyTypeKey
        where TTitleType : TitleTypeKey { }
    public interface ISupplierEmployee<TPartyType, TTitleType> : ISupplierEmployeeBase<SupplierEmployeeId, TPartyType, TTitleType>
        where TPartyType : PartyTypeKey
        where TTitleType : TitleTypeKey { }
    public interface ISupplierEmployeeBaseWithStatus<TKey, TStatusType> : ISupplierEmployeeKey<TKey>, IEmployeeBaseWithStatus<TKey, TStatusType>
        where TKey : ISupplierEmployeeId
        where TStatusType : StatusTypeKey { }
    public interface ISupplierEmployeeWithStatus<TStatusType> : ISupplierEmployeeBaseWithStatus<SupplierEmployeeId, TStatusType>
        where TStatusType : StatusTypeKey { }
    public interface ISupplierEmployeeBase<TKey, TPartyType, TTitleType, TStatusType> : ISupplierEmployeeBase<TKey, TPartyType, TTitleType>, ISupplierEmployeeBaseWithStatus<TKey, TStatusType>, IEmployeeBase<TKey, TPartyType, TTitleType, TStatusType>
        where TKey : ISupplierEmployeeId
        where TPartyType : PartyTypeKey
        where TTitleType : TitleTypeKey
        where TStatusType : StatusTypeKey { }
    public interface ISupplierEmployee<TPartyType, TTitleType, TStatusType> : ISupplierEmployeeBase<SupplierEmployeeId, TPartyType, TTitleType, TStatusType>,
        ISupplierEmployee<TPartyType, TTitleType>, ISupplierEmployeeWithStatus<TStatusType>
        where TPartyType : PartyTypeKey
        where TTitleType : TitleTypeKey
        where TStatusType : StatusTypeKey { }
    public interface ISupplierEmployeeBase<TKey, TPartyType, TTitleType, TStatusType, TEmployer> : ISupplierEmployeeBase<TKey, TPartyType, TTitleType, TStatusType>,
        IEmployeeBase<TKey, TPartyType, TTitleType, TStatusType, TEmployer>
        where TPartyType : PartyTypeKey
        where TTitleType : TitleTypeKey
        where TStatusType : StatusTypeKey
        where TEmployer : ISupplierKey
        where TKey : ISupplierEmployeeId { }
    public interface ISupplierEmployee<TPartyType, TTitleType, TStatusType, TEmployer> : ISupplierEmployeeBase<SupplierEmployeeId, TPartyType, TTitleType, TStatusType, TEmployer>,
        ISupplierEmployee<TPartyType, TTitleType, TStatusType>
        where TPartyType : PartyTypeKey
        where TTitleType : TitleTypeKey
        where TStatusType : StatusTypeKey
        where TEmployer : ISupplierKey { }
    public interface ISupplierEmployeeResultBase<TKey> : ISupplierEmployeeBase<TKey, PartyTypeResult, TitleTypeName, StatusTypeResult, SupplierResult>, IEmployeeResultBase<TKey, SupplierResult>
        where TKey : ISupplierEmployeeId { }
    public interface ISupplierEmployeeResultBase : ISupplierEmployeeResultBase<SupplierEmployeeId>, ISupplierEmployee<PartyTypeResult, TitleTypeName, StatusTypeResult, SupplierResult> { }
    public interface ISupplierEmployeeResult<TKey> : ISupplierEmployeeResultBase<TKey>, IEmployeeResult<TKey, SupplierResult>
        where TKey : ISupplierEmployeeId
    { }
    public interface ISupplierEmployeeResult : ISupplierEmployeeResult<SupplierEmployeeId>, ISupplierEmployeeResultBase { }

}
