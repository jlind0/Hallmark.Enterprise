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
    public class TierTypeKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? TierTypeId { get; set; }

        [JsonIgnore]
        public int Key
        {
            get
            {
                return TierTypeId ?? 0;
            }
            set
            {
                this.TierTypeId = value;
            }
        }
    }
    public class TierType : TierTypeKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired("TIER_NAME_REQUIRED")]
        public string Name { get; set; }
    }
}
