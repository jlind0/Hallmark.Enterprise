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
    public class ProductTypeKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? ProductTypeId { get; set; }
        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.ProductTypeId ?? 0;
            }
            set
            {
                this.ProductTypeId = value;
            }
        }
    }
    public partial class ProductType  : ProductTypeKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired("PRODUCTTYPE_NAME_REQUIRED")]
        public string Name { get; set; }
    }
}
