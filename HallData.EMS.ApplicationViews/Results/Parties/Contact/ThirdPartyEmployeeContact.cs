using System;
using HallData.Validation;

namespace HallData.EMS.ApplicationViews.Results
{
    public abstract class ThirdPartyEmployeeContactKey<TKey> : EmployeeContactKey<TKey>, IThirdPartyEmployeeContactKey<TKey>
        where TKey : IThirdPartyEmployeeContactId
    {
        [GlobalizedRequired("THIRDPARTYEMPLOYEE_EMPLOYEEOF_REQUIRED")]
        public override Guid? EmployerGuid
        {
            get
            {
                return base.EmployerGuid;
            }
            set
            {
                base.EmployerGuid = value;
            }
        }
    }
    public abstract class ThirdPartyEmployeeContact<TKey, TPartyType, TContactType, TTitleType> : EmployeeContact<TKey, TPartyType, TContactType, TTitleType>, IThirdPartyEmployeeContact<TKey, TPartyType, TContactType, TTitleType>
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey
        where TTitleType : TitleTypeKey
        where TKey : IThirdPartyEmployeeContactId
    {
        [GlobalizedRequired("THIRDPARTYEMPLOYEE_EMPLOYEEOF_REQUIRED")]
        public override Guid? EmployerGuid
        {
            get
            {
                return base.EmployerGuid;
            }
            set
            {
                base.EmployerGuid = value;
            }
        }
    }

    public abstract class ThirdPartyEmployeeContactForAddRelationship<TKey> : EmployeeContactForAddRelationship<TKey>, IThirdPartyEmployeeContactForAddRelationship<TKey>
        where TKey : IThirdPartyEmployeeContactId
    {
        [GlobalizedRequired("THIRDPARTYEMPLOYEE_EMPLOYEEOF_REQUIRED")]
        public override Guid? EmployerGuid
        {
            get
            {
                return base.EmployerGuid;
            }
            set
            {
                base.EmployerGuid = value;
            }
        }
    }

    public abstract class ThirdPartyEmployeeContactForUpdateRelationship<TKey> : EmployeeContactForUpdateRelationship<TKey>, IThirdPartyEmployeeContactForUpdateRelationship<TKey>
        where TKey : IThirdPartyEmployeeContactId
    {
        [GlobalizedRequired("THIRDPARTYEMPLOYEE_EMPLOYEEOF_REQUIRED")]
        public override Guid? EmployerGuid
        {
            get
            {
                return base.EmployerGuid;
            }
            set
            {
                base.EmployerGuid = value;
            }
        }
    }
}
