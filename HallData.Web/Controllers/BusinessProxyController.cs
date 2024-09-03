using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using System.Web.Http.Controllers;
using System.Web.Http;
using HallData.Business;
using Microsoft.Practices.Unity;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Reflection;
using Inflector;
using System.Net.Http;
using HallData.Utilities;

namespace HallData.Web.Controllers
{
    public interface IBusinessProxyController<out TBusinessImplementation> : IHttpController
        where TBusinessImplementation : IBusinessImplementation
    {
        TBusinessImplementation BusinessImplementation { get; }
        ModelStateDictionary ModelState { get; }
    }
    public class BusinessProxyController<TBusinessImplementation> : ApiController, IBusinessProxyController<TBusinessImplementation>
        where TBusinessImplementation : IBusinessImplementation
    {
        public BusinessProxyController(TBusinessImplementation business)
        {
            this.BusinessImplementation = business;
        }
        public TBusinessImplementation BusinessImplementation
        {
            get;
            private set;
        }
    }

    public static class BusinessProxyControllerFactory
    {
        public static void Register<TBusinessProxyController, TBusinessImplementation>(IUnityContainer container, RouteCollection routes, string name, string path = "api", params Expression<Func<TBusinessImplementation, dynamic>>[] ignoreAction)
            where TBusinessProxyController : IBusinessProxyController<TBusinessImplementation>
            where TBusinessImplementation : IBusinessImplementation
        {
            container.RegisterType<IHttpController, TBusinessProxyController>(name.ToLower());
            var controller = container.Resolve<TBusinessProxyController>();
            var methods = controller.BusinessImplementation.GetType().GetMethodsCached(BindingFlags.Public | BindingFlags.Instance);
            var interfaceMethods = controller.BusinessImplementation.GetType().GetInterfacesCached().SelectMany(i => i.GetMethodsCached(BindingFlags.Public | BindingFlags.Instance)).ToArray();
            var ignoreMethods = ignoreAction.Select(a =>
            {
                var methodCall = a.Body as MethodCallExpression;
                if (methodCall != null)
                    return new Tuple<MethodInfo, ServiceMethod>(methodCall.Method, methodCall.Method.GetCustomAttributeCached<ServiceMethod>(true));
                throw new ArgumentException("Ignore action must be a method call");
            });
            var serviceMethodRouts = methods.SelectMany(m => m.GetCustomAttributesCached<ServiceRoute>(true).Select(
                r => new Tuple<MethodInfo, ServiceRoute, ServiceMethod>(m, r, m.GetCustomAttributeCached<ServiceMethod>(true)))).Union(
                    interfaceMethods.SelectMany(i => i.GetCustomAttributesCached<ServiceRoute>(true).Select(r => new Tuple<MethodInfo, ServiceRoute, ServiceMethod>(i, r, i.GetCustomAttributeCached<ServiceMethod>(true)))), new RouteComparer()).Where(
                        m => !ignoreMethods.Any(im => im.Item1.Name == m.Item1.Name && im.Item2.MethodType == m.Item3.MethodType));
            foreach (var pritoryGroup in serviceMethodRouts.GroupBy(r => (int)r.Item2.Priority).OrderBy(g => g.Key))
            {
                var levelLookup = pritoryGroup.ToLookup(p => p.Item2.IdPathMapping.Length);
                RegisterRoutes(routes, name, path, levelLookup);
            }
        }
        private static void RegisterRoutes(RouteCollection routes, string name, string path, ILookup<int, Tuple<MethodInfo, ServiceRoute, ServiceMethod>> levelGroup, int? maxLevel = null, int level = 1)
        {
            if (maxLevel == null)
                maxLevel = levelGroup.Max(g => g.Key);
            foreach (var pathGroup in levelGroup.Where(l => l.Key >= level).SelectMany(g => g).GroupBy(g => g.Item2.IdPathMapping[level - 1]).OrderBy(g => g.Key))
            {
                if (level < maxLevel)
                {
                    var greaterThanGroup = pathGroup.Where(g => g.Item2.IdPathMapping.Length > level).ToLookup(g => g.Item2.IdPathMapping.Length);
                    RegisterRoutes(routes, name, path, greaterThanGroup, maxLevel, level + 1);
                }
                foreach (var route in pathGroup.Where(g => g.Item2.IdPathMapping.Length == level).OrderBy(g => g.Item2.IdPathMapping[level - 1]))
                {
                    routes.MapHttpRoute(string.Format("{0}{1}", name.ToLower(), route.Item2.Key),
                                string.Format("{0}/{1}/{2}", path, name, route.Item2.RoutePath),
                                new { controller = name.ToLower(), action = route.Item1.Name },
                                constraints: new { httpMethod = new HttpMethodConstraint(route.Item3.ToHttpMethod().Method, HttpMethod.Options.Method) });
                }
            }
        }
        public static void Register<TBusinessProxyController, TBusinessImplementation, TView>(IUnityContainer container, RouteCollection routes, string name = null, string path = "api", params Expression<Func<TBusinessImplementation, dynamic>>[] ignoreAction)
            where TBusinessProxyController : IBusinessProxyController<TBusinessImplementation>
            where TBusinessImplementation : IBusinessImplementation
        {
            if (name == null)
            {
                name = GetViewName<TBusinessImplementation>();
                if (name == null)
                    name = typeof(TView).Name.Pluralize();
            }
            Register<TBusinessProxyController, TBusinessImplementation>(container, routes, name, path, ignoreAction);
        }
        public static void RegisterDefault<TBusinessImplementation, TView>(IUnityContainer container, RouteCollection routes, string name = null, string path = "api", params Expression<Func<TBusinessImplementation, dynamic>>[] ignoreAction)
            where TBusinessImplementation : IBusinessImplementation
        {
            Register<BusinessProxyController<TBusinessImplementation>, TBusinessImplementation, TView>(container, routes, name, path, ignoreAction);
        }
        public static void RegisterDefault<TBusinessImplementation>(IUnityContainer container, RouteCollection routes, string name = null, string path = "api", params Expression<Func<TBusinessImplementation, dynamic>>[] ignoreAction)
            where TBusinessImplementation : IBusinessImplementation
        {
            if (name == null)
                name = GetViewName<TBusinessImplementation>();
            if (name == null)
                throw new ArgumentException("Name was not provided and the business implemention does not have a name on the ServiceAttribute");
            Register<BusinessProxyController<TBusinessImplementation>, TBusinessImplementation>(container, routes, name, path, ignoreAction);
        }
        private static string GetViewName<TBusinessImplemention>()
            where TBusinessImplemention : IBusinessImplementation
        {
            var serviceAttr = typeof(TBusinessImplemention).GetCustomAttributeCached<ServiceAttribute>(true);
            if (serviceAttr == null)
                return null;
            return serviceAttr.Name;
        }
        private class RouteComparer : IEqualityComparer<Tuple<MethodInfo, ServiceRoute, ServiceMethod>>
        {
            public bool Equals(Tuple<MethodInfo, ServiceRoute, ServiceMethod> x, Tuple<MethodInfo, ServiceRoute, ServiceMethod> y)
            {
                return x.Item2.Key == y.Item2.Key && x.Item3.MethodType == y.Item3.MethodType;
            }
            public int GetHashCode(Tuple<MethodInfo, ServiceRoute, ServiceMethod> obj)
            {
                return HashCodeProvider.BuildHashCode(obj.Item2.Key, obj.Item3.MethodType);
            }
        }
    }
}
