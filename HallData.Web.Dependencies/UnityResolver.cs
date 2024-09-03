using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;

namespace HallData.Web.Dependencies
{
    public class UnityResolver : IDependencyResolver
    {
        private IUnityContainer Container { get; set; }
        public UnityResolver(IUnityContainer container)
        {
            this.Container = container;
        }

        public IDependencyScope BeginScope()
        {
            var child = this.Container.CreateChildContainer();
            return new UnityResolver(child);
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return Container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return this.Container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new object[] { };
            }
        }

        public void Dispose()
        {
            this.Container.Dispose();
        }
    }
}
