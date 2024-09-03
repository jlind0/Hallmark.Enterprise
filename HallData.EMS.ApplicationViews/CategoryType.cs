using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews
{
    public class CategoryTypeKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? CategoryTypeId { get; set; }
        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.CategoryTypeId ?? 0;
            }
            set
            {
                this.CategoryTypeId = value;
            }
        }
    }
    public class CategoryTypeResult : CategoryTypeKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        public string Name { get; set; }
    }
}
