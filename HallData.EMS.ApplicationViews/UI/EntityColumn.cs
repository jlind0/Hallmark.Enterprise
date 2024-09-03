using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;

namespace HallData.EMS.ApplicationViews.UI
{
    public class EntityColumnKey : IHasKey
    {
        [ChildKeyOperationParameter]
        [UpdateOperationParameter]
        public int? EntityColumnId { get; set; }

        public int Key
        {
            get
            {
                return this.EntityColumnId ?? 0;
            }
            set
            {
                this.EntityColumnId = value;
            }
        }
    }
    public class EntityColumn<TEntity, TEntityChild> : EntityColumnKey
        where TEntity: EntityKey
        where TEntityChild: EntityKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired]
        public string Name { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired]
        public string DisplayName { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool IsKey { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool IsCollection { get; set; }
        [ChildView]
        public TEntity Entity { get; set; }
        [ChildView]
        public TEntityChild ChildEntity { get; set; }
    }
    public class EntityColumnBase : EntityColumn<EntityKey, EntityKey> { }
    public class EntityColumnResult : EntityColumn<EntityKey, EntityResult> { }
}
