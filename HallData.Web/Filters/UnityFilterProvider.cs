using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Reflection;
using Microsoft.Practices.Unity;
using HallData.Web.Controllers;
using HallData.Business;

namespace HallData.Web.Filters
{
    public class UnityFilterProvider : ActionDescriptorFilterProvider, IFilterProvider, IDisposable
    {
        private IUnityContainer Container { get; set; }
        public UnityFilterProvider(IUnityContainer container)
        {
            this.Container = container;
        }
        IEnumerable<FilterInfo> IFilterProvider.GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(configuration, actionDescriptor);
            foreach (var filter in filters)
            {
                this.Container.BuildUp(filter.Instance.GetType(), filter.Instance);
            }
            try
            {
                List<FilterInfo> filterCollection = new List<FilterInfo>(filters);
                var controller = Container.Resolve<IHttpController>(actionDescriptor.ControllerDescriptor.ControllerName.ToLower()) as IBusinessProxyController<IBusinessImplementation>;
                var authorizations = controller.BusinessImplementation.GetType().GetCustomAttributes<Authorize>(true).Union(controller.BusinessImplementation.GetType().GetInterfaces().SelectMany(i => i.GetType().GetCustomAttributes<Authorize>(true)));
                foreach (var auth in authorizations)
                {
                    filterCollection.Add(new FilterInfo(new AuthorizeAttribute() { Roles = auth.Roles }, FilterScope.Controller));
                }
                ServiceMethodHttpActionDescriptor actionDesc = actionDescriptor as ServiceMethodHttpActionDescriptor;
                if (actionDesc != null)
                {
                    var interfaceMethods = controller.BusinessImplementation.GetType().GetInterfaces().SelectMany(i =>
                            i.GetMethods(BindingFlags.Public | BindingFlags.Instance)).ToArray();
                    var matchingMethods = interfaceMethods.Where(m => m.GetCustomAttribute<ServiceMethod>() != null).Where(m => m.GetCustomAttribute<ServiceMethod>(true).MethodType == actionDesc.ServiceMethod.MethodType && m.Name == actionDesc.BusinessMethod.Name).ToArray();
                    var authAttributes = matchingMethods.SelectMany(m => m.GetCustomAttributes<Authorize>(true)).ToArray();
                    var actionAuth = actionDesc.BusinessMethod.GetCustomAttributes<Authorize>(true).Union(authAttributes);
                    foreach (var auth in actionAuth)
                    {
                        filterCollection.Add(new FilterInfo(new AuthorizeAttribute() { Roles = auth.Roles }, FilterScope.Action));
                    }
                }
                return filterCollection;
            }
            catch (ResolutionFailedException) { }
            return filters;
        }

        public void Dispose()
        {
            this.Container.Dispose();
        }
    }
}
