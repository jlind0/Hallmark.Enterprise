using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;

namespace HallData.EMS.ApplicationViews.UI
{
    public class EntityKey : IHasKey
    {
        [ChildKeyOperationParameter]
        [UpdateOperationParameter]
        public int? EntityId { get; set; }

        public int Key
        {
            get
            {
                return this.EntityId ?? 0;
            }
            set
            {
                this.EntityId = value;
            }
        }
    }
    public class Entity<TEntity, TEntityColumn> : EntityKey
        where TEntity: EntityKey
        where TEntityColumn: EntityColumnKey
    {
        public Entity()
        {
            this.EntityColumns = new HashSet<TEntityColumn>();
        }
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired]
        public string Name { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired]
        public string DisplayName { get; set; }
        [ChildView]
        public TEntity ParentEntity { get; set; }
        [ChildViewCollection("ui.entitycolumntabletype")]
        public ICollection<TEntityColumn> EntityColumns { get; set; }
    }
    public class EntityBase : Entity<EntityKey, EntityColumnBase> { }
    public class EntityResult : Entity<EntityResult, EntityColumnResult> { }
}
