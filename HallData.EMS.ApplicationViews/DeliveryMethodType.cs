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
    public class DeliveryMethodTypeKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? DeliveryMethodTypeId { get; set; }
        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.DeliveryMethodTypeId ?? 0;
            }
            set
            {
                this.DeliveryMethodTypeId = value;
            }
        }
    }
    public class DeliveryMethodTypeKeyForRelationshipMerge : DeliveryMethodTypeKey, IRelationshipMergeable
    {
        public RelationshipMergeActions MergeAction { get; set; }
    }
    public partial class DeliveryMethodType : DeliveryMethodTypeKey
    { 
        [UpdateOperationParameter]
        [AddOperationParameter]
        [GlobalizedRequired("DELIVERYMETHODTYPE_NAME_REQUIRED")]
        public string Name { get; set; }

    }
}
