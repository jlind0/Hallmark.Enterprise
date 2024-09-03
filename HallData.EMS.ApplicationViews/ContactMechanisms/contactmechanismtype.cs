using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews
{
    public class ContactMechanismTypeKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? ContactMechanismTypeId { get; set; }

        public int Key
        {
            get
            {
                return this.ContactMechanismTypeId ?? 0;
            }
            set
            {
                this.ContactMechanismTypeId = value;
            }
        }
    }
    public partial class ContactMechanismType : ContactMechanismTypeKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired("CONTACTMECHANISMTYPE_NAME_REQUIRED")]
        public string Name { get; set; }

    }
}
