using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace HallData.Models
{
    public enum ModelBindingDirection
    {
        OneWayToView,
        OnewWayToModel,
        TwoWays
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public class MapToViewPropertyAttribute : Attribute
    {
        public string PropertyPath { get; private set; }
        public ModelBindingDirection BindingDirection { get; private set; }
        public MapToViewPropertyAttribute(string propertyPath, ModelBindingDirection bindingDirection = ModelBindingDirection.TwoWays)
        {
            this.PropertyPath = propertyPath;
            this.BindingDirection = bindingDirection;
        }
    }
    public class MapChildToModelPropertyAttribute : MapToViewPropertyAttribute
    {
        public MapChildToModelPropertyAttribute(string propertyPath, ModelBindingDirection bindingDirection = ModelBindingDirection.TwoWays)
            : base(propertyPath, bindingDirection)
        {
        }
    }
    public class MapChildCollectionToModelPropertyAttribute : MapToViewPropertyAttribute
    {
        public MapChildCollectionToModelPropertyAttribute(string propertyPath, ModelBindingDirection bindingDirection = ModelBindingDirection.TwoWays) : base(propertyPath, bindingDirection) { }
    }
    public class MapChildMergeToModelPropertyAttribute : MapToViewPropertyAttribute
    {
        public MapChildMergeToModelPropertyAttribute(string propertyPath, ModelBindingDirection bindingDirection = ModelBindingDirection.OneWayToView) : base(propertyPath, bindingDirection) { }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class MapToViewAttribute : Attribute
    {
        public Type ViewType { get; set; }
        public MapToViewAttribute(Type viewType)
        {
            this.ViewType = viewType;
        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public class ResultIgnoreAttribute : Attribute { }
}
