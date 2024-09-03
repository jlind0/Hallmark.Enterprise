using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Enums;

namespace HallData.EMS.ApplicationViews
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContactMechanismTypeAttribute : Attribute
    {
        public MechanismTypes MechanismType { get; private set; }
        public string ErrorCode { get; private set; }
        public ContactMechanismTypeAttribute(MechanismTypes mechanismType, string errorCode = "CONTACTMECHANISM_MECHANISMTYPE_INVALID")
        {
            this.MechanismType = mechanismType;
            this.ErrorCode = errorCode;
        }
    }
}
