using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using HallData.ApplicationViews;
using HallData.Utilities;

namespace HallData.Models
{
    public abstract class ModelBase<TView>
    {
        public ModelBase() { }
        public ModelBase(TView view)
        {
            PopulateModel(this, view);
        }
        public virtual TView ToView()
        {
            var view = Activator.CreateInstance<TView>();
            if (BuildView(view, this))
                return view;
            return default(TView);
        }
        private static bool PopulateModel(object model, object view)
        {
            bool created = false;
            foreach (var property in model.GetType().GetPropertiesCached(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var mapAttr = property.GetCustomAttributeCached<MapToViewPropertyAttribute>(true);
                if (mapAttr != null && mapAttr.BindingDirection == ModelBindingDirection.OnewWayToModel || mapAttr.BindingDirection == ModelBindingDirection.TwoWays)
                {
                    var val = GetPropertyValueForPath(mapAttr.PropertyPath, view);
                    if (val != null)
                    {
                        if (mapAttr is MapChildToModelPropertyAttribute)
                        {
                            var subModel = CreateSubModel(property.PropertyType, val);
                            if(subModel != null)
                            {
                                property.SetValue(model, subModel);
                                created = true;
                            }
                        }
                        else if(mapAttr is MapChildCollectionToModelPropertyAttribute)
                        {
                            var modelCol = property.GetValue(model) as IList;
                            if (modelCol != null)
                            {
                                var values = val as IEnumerable<object>;
                                var modelColItemType = property.PropertyType.GetCollectionValueType();
                                if (values != null)
                                {
                                    foreach(var viewValue in values)
                                    {
                                        var subModel = CreateSubModel(modelColItemType, viewValue);
                                        if(subModel != null)
                                        {
                                            modelCol.Add(subModel);
                                            created = true;
                                        }
                                    }

                                }
                            }
                        }
                        else if(mapAttr is MapChildMergeToModelPropertyAttribute)
                        {
                            var modelMerge = property.GetValue(model) as IMerge<IMergable>;
                            if(modelMerge != null)
                            {
                                var viewMerge = val as IMerge<IMergable>;
                                if(viewMerge != null)
                                {
                                    foreach(var add in viewMerge.AddCollection)
                                    {
                                        var subModel = CreateSubModel(modelMerge.AddType, add);
                                        if(subModel != null)
                                        {
                                            modelMerge.AddCollection.Add(subModel);
                                            created = true;
                                        }
                                    }
                                    foreach(var update in viewMerge.UpdateCollection)
                                    {
                                        var subModel = CreateSubModel(modelMerge.UpdateType, update);
                                        if(subModel != null)
                                        {
                                            modelMerge.UpdateCollection.Add(subModel);
                                            created = true;
                                        }
                                    }
                                    foreach(var delete in viewMerge.HardDeleteCollection)
                                    {
                                        var subModel = CreateSubModel(modelMerge.DeleteType, delete);
                                        if(subModel != null)
                                        {
                                            modelMerge.HardDeleteCollection.Add(subModel);
                                            created = true;
                                        }
                                    }
                                    foreach (var delete in viewMerge.SoftDeleteCollection)
                                    {
                                        var subModel = CreateSubModel(modelMerge.DeleteType, delete);
                                        if (subModel != null)
                                        {
                                            modelMerge.SoftDeleteCollection.Add(subModel);
                                            created = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            property.SetValue(model, val);
                            created = true;
                        }
                    }
                }

            }
            return created;
        }
        private static object CreateSubModel(Type modelType, object viewValue)
        {
            object subModel = null;
            var viewMap = modelType.GetCustomAttribute<MapToViewAttribute>(true);
            if (viewMap != null && viewValue.GetType() == viewMap.ViewType && modelType.GetConstructors().Any(c => c.GetParameters().All(p => p.ParameterType == viewMap.ViewType || p.ParameterType.IsSubclassOf(viewMap.ViewType))))
                subModel = Activator.CreateInstance(modelType, viewValue);
            else
            {
                subModel = Activator.CreateInstance(modelType);
                if (!PopulateModel(subModel, viewValue))
                    subModel = null;
            }
            return subModel;
        }
        private static bool BuildView(object view, object model)
        {
            bool created = false;
            var modelType = model.GetType();
            foreach (var property in modelType.GetPropertiesCached())
            {
                var mapAttr = property.GetCustomAttributeCached<MapToViewPropertyAttribute>(true);
                if (mapAttr != null && mapAttr.BindingDirection == ModelBindingDirection.OneWayToView || mapAttr.BindingDirection == ModelBindingDirection.TwoWays)
                {
                    var mapViewAttr = property.PropertyType.GetCustomAttributeCached<MapToViewAttribute>(true);
                    if (mapViewAttr != null)
                    {
                        bool childCreated = false;
                        var childModel = property.GetValue(model);
                        if (childModel != null)
                        {
                            var childView = Activator.CreateInstance(mapViewAttr.ViewType);
                            childCreated = BuildView(childView, childModel);
                            if (childCreated)
                                childCreated = SetValueOnPath(mapAttr.PropertyPath.Split('.'), view, childView);
                        }
                        created = childCreated || created;
                    }
                    else if (property.GetCustomAttributeCached<MapChildCollectionToModelPropertyAttribute>(true) != null)
                    {
                        var modelCol = property.GetValue(model) as IEnumerable<object>;
                        if (modelCol != null)
                        {
                            var viewCollection = GetPropertyValueForPath(mapAttr.PropertyPath, view, true) as IList;
                            if (viewCollection != null)
                            {
                                var viewCollectionItemType = viewCollection.GetType().GetCollectionValueType();
                                foreach (var colItem in modelCol)
                                {
                                    var childView = Activator.CreateInstance(viewCollectionItemType);
                                    if (BuildView(childView, colItem))
                                    {
                                        viewCollection.Add(childView);
                                        created = true;
                                    }
                                }
                            }
                        }
                    }
                    else if (property.GetCustomAttributeCached<MapChildMergeToModelPropertyAttribute>(true) != null)
                    {
                        var modelMerge = property.GetValue(model) as IMerge<IMergable>;
                        if (modelMerge != null)
                        {
                            var viewMerge = GetPropertyValueForPath(mapAttr.PropertyPath, view, true) as IMerge<IMergable>;
                            if (viewMerge != null)
                            {
                                foreach (var add in modelMerge.AddCollection)
                                {
                                    var childView = Activator.CreateInstance(viewMerge.AddType);
                                    if (BuildView(add, childView))
                                    {
                                        viewMerge.AddCollection.Add(childView);
                                        created = true;
                                    }
                                }
                                foreach (var update in modelMerge.UpdateCollection)
                                {
                                    var childView = Activator.CreateInstance(viewMerge.UpdateType);
                                    if (BuildView(update, childView))
                                    {
                                        viewMerge.UpdateCollection.Add(update);
                                        created = true;
                                    }
                                }
                                foreach (var delete in modelMerge.HardDeleteCollection)
                                {
                                    var childView = Activator.CreateInstance(viewMerge.DeleteType);
                                    if (BuildView(delete, childView))
                                    {
                                        viewMerge.HardDeleteCollection.Add(delete);
                                        created = true;
                                    }
                                }
                                foreach (var delete in modelMerge.SoftDeleteCollection)
                                {
                                    var childView = Activator.CreateInstance(viewMerge.DeleteType);
                                    if (BuildView(delete, childView))
                                    {
                                        viewMerge.SoftDeleteCollection.Add(delete);
                                        created = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var obj = property.GetValue(model);
                        if (obj != null)
                            created = SetValueOnPath(mapAttr.PropertyPath.Split('.'), view, obj) || created;
                    }
                }
            }
            return created;
        }
        private static bool SetValueOnPath(string[] path, object view, object value, int level = 1)
        {
            if(view != null && path.Length >= level)
            {
                var property = view.GetType().GetPropertyCached(path[level - 1]);
                if(property != null)
                {
                    if (path.Length > level)
                    {
                        var obj = property.GetValue(view);
                        if (obj == null)
                            obj = Activator.CreateInstance(property.PropertyType);
                        if (SetValueOnPath(path, obj, value, level + 1))
                        {
                            property.SetValue(view, obj);
                            return true;
                        }
                    }
                    else
                    {
                        property.SetValue(view, value);
                        return true;
                    }
                }
            }
            return false;
        }
        private static object GetPropertyValueForPath(string path, object obj, bool createChildren = false)
        {
            string[] paths = path.Split('.');
            return GetPropertyValueForPath(paths, obj, createChildren);

        }
        private static object GetPropertyValueForPath(string[] path, object obj, bool createChildren = false, int level = 1)
        {
            if (obj != null && path.Length >= level)
            {
                var property = obj.GetType().GetPropertyCached(path[level - 1]);
                if (property != null)
                {
                    var val = property.GetValue(obj);
                    if (val == null && createChildren && path.Length > level)
                    {
                        val = Activator.CreateInstance(property.PropertyType);
                        property.SetValue(obj, val);
                    }
                    if (val != null && path.Length > level)
                        return GetPropertyValueForPath(path, val, createChildren, level + 1);
                    return val;
                }
            }
            return null;
        }
    }
}
