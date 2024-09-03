using System;
using HallData.ApplicationViews;
using HallData.Validation;

namespace HallData.EMS.ApplicationViews.Results
{
    public abstract class EmployeeContactKey<TKey> : EmployeeKey<TKey>, IEmployeeContactKey<TKey>
        where TKey: IEmployeeContactId
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        [GlobalizedRequired("CONTACT_ROLE_REQUIRED")]
        public virtual string ContactRoleName { get; set; }
        [GlobalizedRequired("EMPLOYEECONTACT_EMPLOYER_REQUIRED")]
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

    public abstract class EmployeeContact<TKey, TPartyType, TContactType, TTitleType> : EmployeeWithKey<TKey, TPartyType, TTitleType>, IEmployeeContact<TKey, TPartyType, TContactType, TTitleType>
        where TPartyType: PartyTypeKey
        where TContactType: ContactTypeKey
        where TTitleType : TitleTypeKey
        where TKey: IEmployeeContactId
    {
        [ChildView]
        [AddOperationParameter]
        [UpdateOperationParameter]
        public virtual TContactType ContactType { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public virtual string JobTitle { get; set; }

        [AddOperationParameter]
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        [GlobalizedRequired("CONTACT_ROLE_REQUIRED")]
        public virtual string ContactRoleName { get; set; }
        [GlobalizedRequired("EMPLOYEECONTACT_EMPLOYER_REQUIRED")]
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

    public abstract class EmployeeContactForAddRelationship<TKey> : EmployeeContactKey<TKey>, IEmployeeContactForAddRelationship<TKey>
        where TKey: IEmployeeContactId
    {
        [ChildView]
        [AddOperationParameter]
        public virtual StatusTypeKey ContactRelationshipStatus { get; set; }
        [ChildView]
        [AddOperationParameter]
        [GlobalizedRequired("CONTACT_CONTACTTYPE_REQUIRED")]
        public virtual ContactTypeKey ContactType { get; set; }
        [AddOperationParameter]
        public virtual string JobTitle { get; set; }
    }

    public abstract class EmployeeContactForUpdateRelationship<TKey> : EmployeeContactKey<TKey>, IEmployeeContactForUpdateRelationship<TKey>
        where TKey: IEmployeeContactId
    {
        [ChildView]
        [UpdateOperationParameter]
        public virtual ContactTypeKey ContactType { get; set; }
        [UpdateOperationParameter]
        public virtual string JobTitle { get; set; }

        public string OriginalContactRoleName { get; set; }

        public Guid? OriginalEmployerGuid { get; set; }

        public Guid? OriginalPartyGuid { get; set; }
    }
}
