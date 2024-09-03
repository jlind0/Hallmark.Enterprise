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
    
    public class FrequencyKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? FrequencyId { get; set; }
        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.FrequencyId ?? 0;
            }
            set
            {
                this.FrequencyId = value;
            }
        }
    }

    public class Frequency : FrequencyKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired("FREQUENCY_NAME_REQUIRED")]
        public string Name { get; set; }
    }

}
