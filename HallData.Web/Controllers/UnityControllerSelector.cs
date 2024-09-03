using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Practices.Unity;
using System.Web.Http.Dispatcher;
using System.Web.Http.Controllers;

namespace HallData.Web.Controllers
{
    public class UnityControllerSelector : DefaultHttpControllerSelector, IDisposable
    {
        private IUnityContainer Container { get; set; }
        private HttpConfiguration Configuration { get; set; }
        public UnityControllerSelector(HttpConfiguration configuration, IUnityContainer container)
            : base(configuration)
        {
            this.Container = container;
            this.Configuration = configuration;
        }
        public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            var mapping = base.GetControllerMapping();
            foreach(var controller in this.Container.Registrations.Where(c => c.RegisteredType == typeof(IHttpController)))
            {
                mapping[controller.Name] = new HttpControllerDescriptor()
                {
                    Configuration = this.Configuration,
                    ControllerName = controller.Name,
                    ControllerType = controller.MappedToType
                };
            }
            return mapping;
        }
        public override HttpControllerDescriptor SelectController(System.Net.Http.HttpRequestMessage request)
        {
            var controllerName = base.GetControllerName(request);
            try
            {
                var controller = Container.Resolve<IHttpController>(controllerName.ToLower());
                return new HttpControllerDescriptor(this.Configuration, controllerName, controller.GetType());
            }
            catch (ResolutionFailedException) { }
            return base.SelectController(request);
        }

        public void Dispose()
        {
            this.Container.Dispose();
        }
    }
}
