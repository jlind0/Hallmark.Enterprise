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
    public class TitleTypeKey: IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? TitleTypeId { get; set; }
        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.TitleTypeId ?? 0;
            }
            set
            {
                this.TitleTypeId = value;
            }
        }
    }
    public class TitleTypeName : TitleTypeKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired("TITLETYPE_NAME_REQUIRED")]
        public string Name { get; set; }
    }
    public class TitleTypeBase<TTitleType> : TitleTypeName
        where TTitleType: TitleTypeKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        public int? OrderIndex { get; set; }
        [AddOperationParameter]
        [ChildView]
        public TTitleType Parent { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public int? Weight { get; set; }
    }
    public class TitleTypeBase : TitleTypeBase<TitleTypeKey> { }
    public class TitleTypeResult : TitleTypeBase<TitleTypeName> { }
}
