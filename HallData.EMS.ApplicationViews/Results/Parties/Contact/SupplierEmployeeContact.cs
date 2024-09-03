using System;
using HallData.Validation;

namespace HallData.EMS.ApplicationViews.Results
{
    public abstract class SupplierEmployeeContactKey<TKey> : EmployeeContactKey<TKey>, ISupplierEmployeeContactKey<TKey>
        where TKey: ISupplierEmployeeContactId
    {
        [GlobalizedRequired("SUPPLIEREMPLOYEE_EMPLOYEEOF_REQUIRED")]
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
    public abstract class SupplierEmployeeContact<TKey, TPartyType, TContactType, TTitleType> : EmployeeContact<TKey, TPartyType, TContactType, TTitleType>, ISupplierEmployeeContact<TKey, TPartyType, TContactType, TTitleType>
        where TPartyType : PartyTypeKey
        where TContactType : ContactTypeKey
        where TTitleType : TitleTypeKey
        where TKey : ISupplierEmployeeContactId 
    {
        [GlobalizedRequired("SUPPLIEREMPLOYEE_EMPLOYEEOF_REQUIRED")]
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

    public abstract class SupplierEmployeeContactForAddRelationship<TKey> : EmployeeContactForAddRelationship<TKey>, ISupplierEmployeeContactForAddRelationship<TKey>
        where TKey : ISupplierEmployeeContactId 
    {
        [GlobalizedRequired("SUPPLIEREMPLOYEE_EMPLOYEEOF_REQUIRED")]
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

    public abstract class SupplierEmployeeContactForUpdateRelationship<TKey> : EmployeeContactForUpdateRelationship<TKey>, ISupplierEmployeeContactForUpdateRelationship<TKey>
        where TKey : ISupplierEmployeeContactId
    {
        [GlobalizedRequired("SUPPLIEREMPLOYEE_EMPLOYEEOF_REQUIRED")]
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
