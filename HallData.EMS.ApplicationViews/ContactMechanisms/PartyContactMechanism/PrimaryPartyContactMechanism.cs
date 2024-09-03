using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews
{
    public class PrimaryPartyAddress<TMechanismType, TStatusType> : PrimaryAddress<TMechanismType, TStatusType>, IPrimaryPartyAddress<TMechanismType, TStatusType>
        where TMechanismType: MechanismTypeKey
        where TStatusType: StatusTypeKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        public int? OrderIndex { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public string Attention { get; set; }
    }
    public class PrimaryPartyAddressForAddUpdate : PrimaryPartyAddress<MechanismTypeKey, StatusTypeKey>, IPrimaryPartyAddressForAddUpdate
    {
        [JsonIgnore]
        public override MechanismTypeKey MechanismType
        {
            get
            {
                return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Address };
            }
            set
            {
                
            }
        }
        [UpdateOperationParameter]
        public override StatusTypeKey Status
        {
            get
            {
                return base.Status;
            }
            set
            {
                base.Status = value;
            }
        }
    }
	public class PrimaryPartyAddress : PrimaryPartyAddress<MechanismTypeResult, StatusTypeResult>, IPrimaryPartyAddressResult
    {
        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ContactMechanismType ContactMechanismType { get; set; }
    }

    public class PrimaryPartyEmail<TMechanismType, TStatusType> : PrimaryEmail<TMechanismType, TStatusType>, IPrimaryPartyEmail<TMechanismType, TStatusType>
        where TMechanismType : MechanismTypeKey
        where TStatusType : StatusTypeKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        public int? OrderIndex { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public string Attention { get; set; }
    }
    public class PrimaryPartyEmailForAddUpdate : PrimaryPartyEmail<MechanismTypeKey, StatusTypeKey>, IPrimaryPartyEmailForAddUpdate
    {
        [JsonIgnore]
        public override MechanismTypeKey MechanismType
        {
            get
            {
                return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Email };
            }
            set
            {

            }
        }
        [UpdateOperationParameter]
        public override StatusTypeKey Status
        {
            get
            {
                return base.Status;
            }
            set
            {
                base.Status = value;
            }
        }
    }
	public class PrimaryPartyEmail : PrimaryPartyEmail<MechanismTypeResult, StatusTypeResult>, IPrimaryPartyEmailResult
    {

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ContactMechanismType ContactMechanismType { get; set; }
    }

    public class PrimaryPartyPhone<TMechanismType, TStatusType> : PrimaryPhone<TMechanismType, TStatusType>, IPrimaryPartyPhone<TMechanismType, TStatusType>
        where TMechanismType : MechanismTypeKey
        where TStatusType : StatusTypeKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        public int? OrderIndex { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public string Attention { get; set; }
    }
    public class PrimaryPartyPhoneForAddUpdate : PrimaryPartyPhone<MechanismTypeKey, StatusTypeKey>, IPrimaryPartyPhoneForAddUpdate
    {
        [JsonIgnore]
        public override MechanismTypeKey MechanismType
        {
            get
            {
                return new MechanismTypeKey() { MechanismTypeId = (int)Enums.MechanismTypes.Phone };
            }
            set
            {

            }
        }
        [UpdateOperationParameter]
        public override StatusTypeKey Status
        {
            get
            {
                return base.Status;
            }
            set
            {
                base.Status = value;
            }
        }
    }
	public class PrimaryPartyPhone : PrimaryPartyPhone<MechanismTypeResult, StatusTypeResult>, IPrimaryPartyPhoneResult
    {
        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ContactMechanismType ContactMechanismType { get; set; }
    }
}
