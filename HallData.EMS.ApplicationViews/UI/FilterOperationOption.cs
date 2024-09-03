using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews.UI
{
    public class FilterOperationOptionKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int FilterOperationOptionId { get; set; }
        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.FilterOperationOptionId;
            }
            set
            {
                this.FilterOperationOptionId = value;
            }
        }
    }
    public class FilterOperationOptionBase : FilterOperationOptionKey
    {
        [UpdateOperationParameter]
        [AddOperationParameter]
        public int? OrderIndex { get; set; }
    }
    public class FilterOperationOption : FilterOperationOptionBase
    {
        public string Name { get; set; }
    }
}
