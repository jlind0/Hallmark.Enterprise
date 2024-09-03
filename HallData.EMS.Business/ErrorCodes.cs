using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.EMS.Business
{
    public static class ErrorCodes
    {
        public static class ContactMechanism
        {
            public const string NotEmail = "CONTACTMECHANISM_NOT_EMAIL";
            public const string NotAddress = "CONTACTMECHANISM_NOT_ADDRESS";
            public const string NotPhone = "CONTACTMECHANISM_NOT_PHONE";
        }
    }
}
