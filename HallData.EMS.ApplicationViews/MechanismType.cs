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
    public class MechanismTypeKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? MechanismTypeId { get; set; }
        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.MechanismTypeId ?? 0;
            }
            set
            {
                this.MechanismTypeId = value;
            }
        }
    }

	public partial class MechanismTypeResult : MechanismTypeKey
    {
        [UpdateOperationParameter]
        [AddOperationParameter]
        [GlobalizedRequired("MECHANISMTYPE_REQUIRED")]
        public string Name { get; set; }
    }
}
