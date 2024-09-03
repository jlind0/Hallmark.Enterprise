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
    public class PartyTypeKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? PartyTypeId { get; set; }

        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.PartyTypeId ?? 0;
            }
            set
            {
                this.PartyTypeId = value;
            }
        }
    }
    public class PartyTypeResult : PartyTypeKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired]
        public string Name { get; set; }
    }
}
