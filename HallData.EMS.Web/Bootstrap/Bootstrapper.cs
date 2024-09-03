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
using HallData.EMS.Data;
using HallData.EMS.Business;
using HallData.EMS.Models;
using System.Threading;
using HallData.Translation;
using HallData.EMS.ApplicationViews.Results;
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

namespace HallData.EMS.Web.Bootstrap
{
    public static class Bootstrapper
    {
        public static IUnityContainer Container { get; private set; }
        public static void Register(IUnityContainer container, string databaseName = "hds")
        {
            Container = container;
            var db = DatabaseFactory.CreateDatabase(databaseName);
            TranslatorContainer translation = new TranslatorContainer(new Uri("https://api.datamarket.azure.com/Bing/MicrosoftTranslator/"));
            var auth = @"++OwByuXo0l9/m8gqu5xkC7OMiimHs8AU05eHP+9s7Y";
            translation.Credentials = new NetworkCredential(auth, auth);
            container.RegisterInstance<TranslatorContainer>(translation);
            //MockSessionRepository mockSession = new MockSessionRepository(new SecurityUser[] { 
            //    new SecurityUser() 
            //    { 
            //        UserGuid = new Guid("CF16B8C5-9A85-4EC4-A176-D2D76308750E"), 
            //        UserName = "jlind", 
            //        FirstName = "Jason", 
            //        LastName = "Lind",
            //        OrganizationGuid = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7") 
            //    },
            //    new SecurityUser() 
            //    { 
            //        UserGuid = new Guid("FC834D59-E7BE-4BEF-807C-C51938018021"), 
            //        UserName = "dweber", 
            //        FirstName = "David", 
            //        LastName = "Weber",
            //        OrganizationGuid = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7") 
            //    } 
            //});
            //container.RegisterInstance<ISessionRepository>(mockSession);
            container.RegisterInstance<Database>(db);
            container.RegisterInstance<ISecurityTokenizer, EmsTokenizer>();
            container.RegisterInstance<HallData.Session.ISessionRepository, HallData.EMS.Data.Session.SessionRepository>();
            container.RegisterInstance<ISession, RepositorySession>();

            container.RegisterInstance<IGlobalizationRepository, GlobalizationRepository>();
            container.RegisterInstance<IContactMechanismRepository, ContactMechanismRepository>();
            container.RegisterInstance<IReadOnlyProductContactMechanismRepository, IProductContactMechanismRepository, ProductContactMechanismRepository>();
            container.RegisterInstance<IReadOnlyPartyContactMechanismRepoistory, IPartyContactMechanismRepository, PartyContactMechanismRepository>();
            container.RegisterInstance<IReadOnlyProductRepository, IProductRepository, ProductRepository>();
            container.RegisterInstance<IReadOnlyPartyRepository, IPartyRepository, PartyRepository>();
            container.RegisterInstance<IReadOnlyOrganizationRepository, IOrganizationRepository, OrganizationRepository>();
            container.RegisterInstance<IReadOnlyPersonRepository, IPersonRepository, PersonRepository>();
            container.RegisterInstance<IReadOnlyCustomerRepository, ICustomerRepository, CustomerRepository>();
            container.RegisterInstance<IReadOnlyCustomerOrganizationRepository, ICustomerOrganizationRepository, CustomerOrganizationRepository>();
            container.RegisterInstance<IReadOnlyCustomerPersonRepository, ICustomerPersonRepository, CustomerPersonRepository>();
            container.RegisterInstance<IReadOnlyBusinessUnitRepository, IBusinessUnitRepository, BusinessUnitRepository>();
            container.RegisterInstance<IReadOnlyEmployeeRepository, IEmployeeRepository, EmployeeRepository>();
            container.RegisterInstance<IReadOnlyUserRepository, IUserRepository, UserRepository>();
            
            container.RegisterInstance<IPersonalizationRepository, PersonalizationRepository>();
            container.RegisterInstance<IEnumerationsRepository, EnumerationsRepository>();
            
            container.RegisterInstance<ISecurityImplementation, SessionSecurityImplementation>();
            //container.RegisterInstance<ITranslationImplementation, TranslationImplementation>();
            container.RegisterInstance<ITranslationImplementation, MockTranslationImplementation>();
            container.RegisterInstance<IPartyContactImplementation, PartyContactImplementation>();
            container.RegisterInstance<IProductContactImplementation, ProductContactImplementation>();

            container.RegisterInstance<IReadOnlyProductImplementation, ReadOnlyProductImplementation>();
            container.RegisterInstance<IReadOnlyPartyImplementation, ReadOnlyPartyImplementation>();
            container.RegisterInstance<IReadOnlyOrganizationImplementation, ReadOnlyOrganizationImplementation>();
            container.RegisterInstance<IReadOnlyPersonImplementation, ReadOnlyPersonImplementation>();
            container.RegisterInstance<IReadOnlyCustomerImplementation, ReadOnlyCustomerImplementation>();
            container.RegisterInstance<IReadOnlyCustomerOrganizationImplementation, ReadOnlyCustomerOrganizationImplementation>();
            container.RegisterInstance<IReadOnlyCustomerPersonImplementation, ReadOnlyCustomerPersonImplementation>();
            container.RegisterInstance<IReadOnlyEmployeeImplementation, ReadOnlyEmployeeImplementation>();
            container.RegisterInstance<IReadOnlyBusinessUnitImplementation, ReadOnlyBusinessUnitImplementation>();
            container.RegisterInstance<IReadOnlyUserImplementation, ReadOnlyUserImplementation>();

            container.RegisterInstance<ICustomerOrganizationImplementationBase, CustomerOrganizationImplementationBase>();
            container.RegisterInstance<ICustomerPersonImplementationBase, CustomerPersonImplementationBase>();

            container.RegisterInstance<IProductImplementation, ProductImplementation>();
            container.RegisterInstance<ICustomerImplementation, CustomerImplementation>();
            container.RegisterInstance<IPartyImplementation, PartyImplementation>();
            container.RegisterInstance<IOrganizationImplementation, OrganizationImplementation>();
            container.RegisterInstance<IPersonImplementation, PersonImplementation>();
            container.RegisterInstance<ICustomerOrganizationImplementation, CustomerOrganizationImplementation>();
            container.RegisterInstance<ICustomerPersonImplementation, CustomerPersonImplementation>();
            container.RegisterInstance<IBusinessUnitImplementation, BusinessUnitImplementation>();
            container.RegisterInstance<IEmployeeImplementation, EmployeeImplementation>();
            container.RegisterInstance<IUserImplementation, UserImplementation>();
            container.RegisterInstance<IPersonalizationImplementation, PersonalizationImplementation>();
            container.RegisterInstance<IEnumerationsImplemention, EnumerationsImplemention>();



            //ValidationResultFactory.Initialize((result, errorCode) => {
            //    if (errorCode != null)
            //        return container.Resolve<GlobalizedValidationResult>(new ParameterOverride("result", result), new ParameterOverride("errorCode", errorCode));
            //    else
            //        return container.Resolve<GlobalizedValidationResult>(new ParameterOverride("result", result));
            //});
            ValidationResultFactory.Initialize((result, errorCode) => result);
        }
        public static void RegisterMock(IUnityContainer container)
        {
            Container = container;

            var created = new ApplicationViews.StatusTypeResult() { StatusTypeId = 1, Name = "Created" };
            OrganizationResult hdsOrganization = new OrganizationResult() { Code = "HDS", Ein = "EIN", Name = "Hallmark Data Systems", PartyGuid = Guid.NewGuid(), Status = created, Tier = new ApplicationViews.TierType() { TierTypeId = 3, Name = "3" }, Website = "www.halldata.com" };
        }
        public static void RegisterControllers(IUnityContainer container, RouteCollection routes)
        {
            BusinessProxyControllerFactory.RegisterDefault<ISecurityImplementation>(container, routes, "sessions", "api", 
                e => e.SignIn(default(string), default(string), default(string), default(CancellationToken)));
            BusinessProxyControllerFactory.RegisterDefault<IUserImplementation>(container, routes);
            BusinessProxyControllerFactory.RegisterDefault<IProductImplementation>(container, routes);
            
            BusinessProxyControllerFactory.RegisterDefault<ICustomerOrganizationImplementation>(container, routes, null, "api",
                e => e.Get(default(CustomerId), default(string), default(CancellationToken)), e => e.GetView(default(CustomerId), default(string), default(CancellationToken)),
                e => e.DeleteSoft(default(CustomerId), default(CancellationToken)),
                e => e.DeleteHard(default(CustomerId), default(CancellationToken)), 
                e => e.ChangeStatus(default(CustomerId), default(string), default(string), default(CancellationToken)),
                e => e.ChangeStatusForce(default(CustomerId), default(string), default(string), default(CancellationToken)));
            BusinessProxyControllerFactory.RegisterDefault<ICustomerPersonImplementation>(container, routes, null, "api",
               e => e.Get(default(CustomerId), default(string), default(CancellationToken)), e => e.GetView(default(CustomerId), default(string), default(CancellationToken)),
               e => e.DeleteSoft(default(CustomerId), default(CancellationToken)),
               e => e.DeleteHard(default(CustomerId), default(CancellationToken)), 
               e => e.ChangeStatus(default(CustomerId), default(string), default(string), default(CancellationToken)),
               e => e.ChangeStatusForce(default(CustomerId), default(string), default(string), default(CancellationToken)));
            BusinessProxyControllerFactory.RegisterDefault<ICustomerImplementation>(container, routes, null, "api",
                e => e.Get(default(CustomerId), default(string), default(CancellationToken)), e => e.GetView(default(CustomerId), default(string), default(CancellationToken)),
                e => e.DeleteSoft(default(CustomerId), default(CancellationToken)),
                e => e.DeleteHard(default(CustomerId), default(CancellationToken)), 
                e => e.ChangeStatus(default(CustomerId), default(string), default(string), default(CancellationToken)),
                e => e.ChangeStatusForce(default(CustomerId), default(string), default(string), default(CancellationToken)));
            BusinessProxyControllerFactory.RegisterDefault<IBusinessUnitImplementation>(container, routes);
            BusinessProxyControllerFactory.RegisterDefault<IEmployeeImplementation>(container, routes, null, "api", 
                e => e.Get(default(EmployeeId), default(string), default(CancellationToken)), e => e.GetView(default(EmployeeId), default(string), default(CancellationToken)), 
                e => e.DeleteSoft(default(EmployeeId), default(CancellationToken)), 
                e => e.DeleteHard(default(EmployeeId), default(CancellationToken)), 
                e => e.ChangeStatus(default(EmployeeId), default(string), default(string), default(CancellationToken)),
                e => e.ChangeStatusForce(default(EmployeeId), default(string), default(string), default(CancellationToken)));
            BusinessProxyControllerFactory.RegisterDefault<IPersonImplementation>(container, routes);
            BusinessProxyControllerFactory.RegisterDefault<IPartyImplementation>(container, routes);
            BusinessProxyControllerFactory.RegisterDefault<IOrganizationImplementation>(container, routes);
            BusinessProxyControllerFactory.RegisterDefault<IPersonalizationImplementation>(container, routes);
            BusinessProxyControllerFactory.RegisterDefault<IEnumerationsImplemention>(container, routes);
        }

        public static void Configure(IUnityContainer container, HttpConfiguration config)
        {
            config.DependencyResolver = new UnityResolver(container);
            config.Services.Replace(typeof(IHttpControllerSelector), new UnityControllerSelector(GlobalConfiguration.Configuration, container));
            config.Services.Replace(typeof(IHttpActionSelector), new DynamicActionSelector(container, container.Resolve<ITranslationImplementation>(), config));
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
                c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "HallData.ApplicationViews.XML"));
            }).EnableSwaggerUi("api/documentation/{*assetPath}");
        }
    }

    internal static class UnityExstensions
    {
        public static void RegisterInstance<TInterface, TImplementation>(this IUnityContainer container)
            where TImplementation : TInterface
        {
            container.RegisterInstance(typeof(TInterface), container.Resolve(typeof(TImplementation)));
        }
        public static void RegisterInstance<TInterfaceBase, TInterface, TImplementation>(this IUnityContainer container)
            where TInterface : TInterfaceBase
            where TImplementation: TInterface
        {
            var instance = container.Resolve<TImplementation>();
            container.RegisterInstance(typeof(TInterfaceBase), instance);
            container.RegisterInstance(typeof(TInterface), instance);
        }
    }
}
