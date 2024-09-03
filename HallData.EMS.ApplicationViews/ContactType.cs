using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews
{
    public class ContactTypeKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? ContactTypeId { get; set; }
        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.ContactTypeId ?? 0;
            }
            set
            {
                this.ContactTypeId = value;
            }
        }
    }
    public class ContactType : ContactTypeKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        public string Name { get; set; }
    }
}
