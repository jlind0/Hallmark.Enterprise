using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Session;
using HallData.EMS.Data.Session;
using HallData.Business;
using HallData.Security;
using HallData.Web;
using System.Web.Http;
using HallData.Web.Session;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using HallData.Data;
using System.Web.Routing;
using HallData.Web.Controllers;
using HallData.Web.Dependencies;
using HallData.Web.Filters;
using System.Web.Http.Dispatcher;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using HallData.Admin.Data;
using HallData.Admin.Business;
using System.Threading;
using HallData.Translation;
using HallData.Admin.ApplicationViews;
using HallData.Validation;
using Microsoft;
using System.Net;
using HallData.EMS.Data.UI;
using HallData.EMS.Business.UI;
using HallData.Swashbuckle;
using Swashbuckle.Application;
using System.IO;
using HallData.EMS.Security.Tokenizer;
using HallData.Session.Mocks;
using HallData.Translation.Mocks;
using HallData.EMS.Data.UI.Mocks;
using HallData.EMS.ApplicationViews.UI;
using HallData.Globalization;
using HallData.EMS.Data.Globalization;

namespace HallData.Admin.Web
{
    public static class Bootstrapper
    {
        public static IUnityContainer Container { get; private set; }
        public static void Register(IUnityContainer container, string databaseName = "hds")
        {
            Container = container;
            var db = DatabaseFactory.CreateDatabase(databaseName);

            container.RegisterInstance<Database>(db);
            container.RegisterInstance<ISecurityTokenizer, EmsTokenizer>();
            container.RegisterInstance<HallData.Session.ISessionRepository, HallData.EMS.Data.Session.SessionRepository>();
            container.RegisterInstance<IHttpContextWrapper, HttpContextWrapper>();
            container.RegisterInstance<ISession, RepositorySession>();

            container.RegisterInstance<IGlobalizationRepository, GlobalizationRepository>();
            container.RegisterInstance<IPersonalizationRepository, PersonalizationRepository>();

            container.RegisterInstance<ISecurityImplementation, SessionSecurityImplementation>();
            container.RegisterInstance<ITranslationService, MockTranslationService>();
            container.RegisterInstance<IPersonalizationImplementation, PersonalizationImplementation>();

            container.RegisterInstance<IReadOnlyInterfaceAttributeRepository, IInterfaceAttributeRepository, InterfaceAttributeRepository>();
            container.RegisterInstance<IReadOnlyInterfaceRepository, IInterfaceRepository, InterfaceRepository>();
            container.RegisterInstance<IReadOnlyDataViewColumnRepository, IDataViewColumnRepository, DataViewColumnRepository>();
            container.RegisterInstance<IReadOnlyDataViewResultRepository, IDataViewResultRepository, DataViewResultRepository>();
            container.RegisterInstance<IReadOnlyDataViewRepository, IDataViewRepository, DataViewRepository>();
            container.RegisterInstance<IReadOnlyApplicationViewColumnRepository, IApplicationViewColumnRepository, ApplicationViewColumnRepository>();
            container.RegisterInstance<IReadOnlyApplicationViewRepository, IApplicationViewRepository, ApplicationViewRepository>();
            container.RegisterInstance<IReadOnlyTemplateRepository, ITemplateRepository, TemplateRepository>();
            container.RegisterInstance<IReadOnlyFilterTypeRepository, IFilterTypeRepository, FilterTypeRepository>();
            container.RegisterInstance<IReadOnlyTemplateTypeRepository, TemplateTypeRepository>();
            
            container.RegisterInstance<IReadOnlyInterfaceAttributeImplementation, ReadOnlyInterfaceAttributeImplementation>();
            container.RegisterInstance<IReadOnlyInterfaceImplementation, ReadOnlyInterfaceImplementation>();
            container.RegisterInstance<IReadOnlyDataViewColumnImplementation, ReadOnlyDataViewColumnImplementation>();
            container.RegisterInstance<IReadOnlyDataViewResultImplementation, ReadOnlyDataViewResultImplementation>();
            container.RegisterInstance<IReadOnlyDataViewImplementation, ReadOnlyDataViewImplementation>();
            container.RegisterInstance<IReadOnlyTemplateImplementation, ReadOnlyTemplateImplementation>();
            container.RegisterInstance<IReadOnlyApplicationViewColumnImplementation, ReadOnlyApplicationViewColumnImplementation>();
            container.RegisterInstance<IReadOnlyApplicationViewImplementation, ReadOnlyApplicationViewImplementation>();

            container.RegisterInstance<IInterfaceAttributeImplementationBase, InterfaceAttributeImplementationBase>();
            container.RegisterInstance<IInterfaceImplementationBase, InterfaceImplementationBase>();
            container.RegisterInstance<ITemplateImplementation, TemplateImplementation>();

            container.RegisterInstance<IInterfaceAttributePassThroughImplementation, InterfaceAttributePassThroughImplementation>();
            container.RegisterInstance<IDataViewColumnPassThroughImplementation, DataViewColumnPassThroughImplementation>();
            container.RegisterInstance<IDataViewResultPassThroughImplementation, DataViewResultPassThroughImplementation>();
            container.RegisterInstance<IDataViewPassThroughImplementation, DataViewPassThroughImplementation>();
            container.RegisterInstance<IApplicationViewColumnPassThroughImplementation, ApplicationViewColumnPassThroughImplementation>();
            container.RegisterInstance<IApplicationViewPassThroughImplementation, ApplicationViewPassThroughImplementation>();

            container.RegisterInstance<IInterfaceAttributeImplementation, InterfaceAttributeImplementation>();
            container.RegisterInstance<IInterfaceImplementation, InterfaceImplementation>();
            container.RegisterInstance<IDataViewColumnImplementation, DataViewColumnImplementation>();
            container.RegisterInstance<IDataViewResultImplementation, DataViewResultImplementation>();
            container.RegisterInstance<IDataViewImplementation, DataViewImplementation>();
            container.RegisterInstance<IApplicationViewColumnImplementation, ApplicationViewColumnImplementation>();
            container.RegisterInstance<IApplicationViewImplementation, ApplicationViewImplementation>();
            
			ValidationResultFactory.Initialize((r,e) => r);
            
        }
        public static void RegisterControllers(IUnityContainer container, RouteCollection routes)
        {
            BusinessProxyControllerFactory.RegisterDefault<ISecurityImplementation>(container, routes, "sessions", "api",
                e => e.SignIn(default(string), default(string), default(string), default(CancellationToken)));
            BusinessProxyControllerFactory.RegisterDefault<IPersonalizationImplementation>(container, routes);
            BusinessProxyControllerFactory.RegisterDefault<IInterfaceAttributeImplementation>(container, routes, null, "api", e => e.ChangeStatus(default(int), default(string), default(CancellationToken)),
                e => e.ChangeStatusForce(default(int), default(string), default(CancellationToken)), e => e.ChangeStatusForceView(default(int), default(string), default(CancellationToken)),
                e => e.ChangeStatusView(default(int), default(string), default(CancellationToken)));
            BusinessProxyControllerFactory.RegisterDefault<IInterfaceImplementation>(container, routes, null, "api", e => e.ChangeStatus(default(int), default(string), default(CancellationToken)),
                e => e.ChangeStatusForce(default(int), default(string), default(CancellationToken)), e => e.ChangeStatusForceView(default(int), default(string), default(CancellationToken)),
                e => e.ChangeStatusView(default(int), default(string), default(CancellationToken)));
            BusinessProxyControllerFactory.RegisterDefault<IDataViewColumnImplementation>(container, routes, null, "api", e => e.ChangeStatus(default(int), default(string), default(CancellationToken)),
                e => e.ChangeStatusForce(default(int), default(string), default(CancellationToken)), e => e.ChangeStatusForceView(default(int), default(string), default(CancellationToken)),
                e => e.ChangeStatusView(default(int), default(string), default(CancellationToken)));
            BusinessProxyControllerFactory.RegisterDefault<IDataViewResultImplementation>(container, routes, null, "api", e => e.ChangeStatus(default(int), default(string), default(CancellationToken)),
               e => e.ChangeStatusForce(default(int), default(string), default(CancellationToken)), e => e.ChangeStatusForceView(default(int), default(string), default(CancellationToken)),
               e => e.ChangeStatusView(default(int), default(string), default(CancellationToken)));
            BusinessProxyControllerFactory.RegisterDefault<IDataViewImplementation>(container, routes, null, "api", e => e.ChangeStatus(default(int), default(string), default(CancellationToken)),
               e => e.ChangeStatusForce(default(int), default(string), default(CancellationToken)), e => e.ChangeStatusForceView(default(int), default(string), default(CancellationToken)),
               e => e.ChangeStatusView(default(int), default(string), default(CancellationToken)));
            BusinessProxyControllerFactory.RegisterDefault<IApplicationViewColumnImplementation>(container, routes, null, "api", e => e.ChangeStatus(default(int), default(string), default(CancellationToken)),
               e => e.ChangeStatusForce(default(int), default(string), default(CancellationToken)), e => e.ChangeStatusForceView(default(int), default(string), default(CancellationToken)),
               e => e.ChangeStatusView(default(int), default(string), default(CancellationToken)));
            BusinessProxyControllerFactory.RegisterDefault<IApplicationViewImplementation>(container, routes, null, "api", e => e.ChangeStatus(default(int), default(string), default(CancellationToken)),
               e => e.ChangeStatusForce(default(int), default(string), default(CancellationToken)), e => e.ChangeStatusForceView(default(int), default(string), default(CancellationToken)),
               e => e.ChangeStatusView(default(int), default(string), default(CancellationToken)));
        }

        public static void Configure(IUnityContainer container, HttpConfiguration config)
        {
            config.DependencyResolver = new UnityResolver(container);
            config.Services.Replace(typeof(IHttpControllerSelector), new UnityControllerSelector(GlobalConfiguration.Configuration, container));
            config.Services.Replace(typeof(IHttpActionSelector), new DynamicActionSelector(container, container.Resolve<ITranslationService>(), config));
            var providers = config.Services.GetFilterProviders();
            var defaultProvider = providers.Single(i => i is ActionDescriptorFilterProvider);
            config.Services.Remove(typeof(IFilterProvider), defaultProvider);
            config.Services.Add(typeof(IFilterProvider), new UnityFilterProvider(container));
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            config.EnableCors();
            GlobalConfiguration.Configuration.EnableSwagger("api/docs/{apiVersion}", c =>
            {
                c.SingleApiVersion("v1", "Hallmark Data Systems EMS API");
                c.BasicAuth("Basic HTTP Authentication");
                c.ResolveConflictingActions(rca => rca.First());
                c.OperationFilter<DescriptionAttributeOperationFilter>();
                c.DocumentFilter<SetOptionalParametersDocumentFilter>();
                c.UseFullTypeNameInSchemaIds();
                c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "HallData.ApplicationViews.XML"));
            }).EnableSwaggerUi("api/documentation/{*assetPath}");
        }
    }
    internal static class UnityExtensions
    {
        public static void RegisterInstance<TInterface, TImplementation>(this IUnityContainer container)
            where TImplementation : TInterface
        {
            container.RegisterInstance(typeof(TInterface), container.Resolve(typeof(TImplementation)));
        }
        public static void RegisterInstance<TInterfaceBase, TInterface, TImplementation>(this IUnityContainer container)
            where TInterface : TInterfaceBase
            where TImplementation : TInterface
        {
            var instance = container.Resolve<TImplementation>();
            container.RegisterInstance(typeof(TInterfaceBase), instance);
            container.RegisterInstance(typeof(TInterface), instance);
        }
    }
}