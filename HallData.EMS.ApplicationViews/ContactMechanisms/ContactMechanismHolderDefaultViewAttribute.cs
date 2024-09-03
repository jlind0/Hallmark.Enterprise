using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;

namespace HallData.EMS.ApplicationViews
{
    public class ContactMechanismHolderDefaultViewAttribute : DefaultViewAttribute
    {
        private static Lazy<ConcurrentDictionary<Type, DefaultViewAttribute>> ContactMechanismsMappingLazy = new Lazy<ConcurrentDictionary<Type,DefaultViewAttribute>>(() => new ConcurrentDictionary<Type, DefaultViewAttribute>(), LazyThreadSafetyMode.PublicationOnly);
        private static DefaultViewAttribute GetDefaultViewAttribute<T>()
        {
            var type = typeof(T);
            DefaultViewAttribute defaultView;
            if(!ContactMechanismsMappingLazy.Value.TryGetValue(typeof(T), out defaultView))
            {
                defaultView = type.GetCustomAttribute<DefaultViewAttribute>(true);
                ContactMechanismsMappingLazy.Value.TryAdd(type, defaultView);
            }
            return defaultView;
        }
        public ContactMechanismHolderDefaultViewAttribute(string defaultViewToken, string defaultSingleToken = null, string defaultManyToken = null)
            :base(defaultViewToken, defaultSingleToken, defaultManyToken)
        {

        }
        public string GetDefaultViewName<TContactMechanism>()
            where TContactMechanism : IContactMechanismResult
        {
            var defaultView = GetDefaultViewAttribute<TContactMechanism>();
            string contactDefaultView = defaultView != null ? defaultView.DefaultView : "";
            return string.Format(this.DefaultView, contactDefaultView);
        }
        public string GetDefaultViewNameSingle<TContactMechanism>()
            where TContactMechanism : IContactMechanismResult
        {
            var defaultView = GetDefaultViewAttribute<TContactMechanism>();
            string contactDefaultView = defaultView != null ? defaultView.DefaultViewSingle : "";
            return string.Format(this.DefaultViewSingle, contactDefaultView);
        }
        public string GetDefaultViewNameMany<TContactMechanism>()
            where TContactMechanism : IContactMechanismResult
        {
            var defaultView = GetDefaultViewAttribute<TContactMechanism>();
            string contactDefaultView = defaultView != null ? defaultView.DefaultViewMany : "";
            return string.Format(this.DefaultViewMany, contactDefaultView);
        }
    }
}
