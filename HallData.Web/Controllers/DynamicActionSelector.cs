using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Reflection;
using Microsoft.Practices.Unity;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Reflection.Emit;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.Web.Http.ModelBinding;
using HallData.Business;
using HallData.Web.ModelBinding;
using HallData.Exceptions;
using HallData.Translation;
using HallData.Web.Authorization;
using System.Web.Http.Cors;
using System.Web.Cors;
using HallData.Utilities;
using System.Collections.Concurrent;
using System.Web.Http.Filters;


namespace HallData.Web.Controllers
{
	public class DynamicActionSelector : ApiControllerActionSelector
	{
		private IUnityContainer Container { get; set; }
		private ITranslationService Translator { get; set; }
		private HttpConfiguration Configuration { get; set; }
		
		public DynamicActionSelector(IUnityContainer container, ITranslationService translator, HttpConfiguration config)
		{
			this.Container = container;
			this.Translator = translator;
			this.Configuration = config;
		}
		
		private static readonly Lazy<ConcurrentDictionary<string, ILookup<string, HttpActionDescriptor>>> ServiceActionDescriptorMappingLazy = new Lazy<ConcurrentDictionary<string, ILookup<string, HttpActionDescriptor>>>(() => new ConcurrentDictionary<string, ILookup<string, HttpActionDescriptor>>(), LazyThreadSafetyMode.PublicationOnly);
		
		protected static ConcurrentDictionary<string, ILookup<string, HttpActionDescriptor>> ServiceActionDescriptorMapping
		{
			get { return ServiceActionDescriptorMappingLazy.Value; }
		}

		public override ILookup<string, HttpActionDescriptor> GetActionMapping(HttpControllerDescriptor controllerDescriptor)
		{
			ILookup<string, HttpActionDescriptor> mapping;
			if (!ServiceActionDescriptorMapping.TryGetValue(controllerDescriptor.ControllerName.ToLower(), out mapping))
			{
				if (controllerDescriptor.ControllerType.GetGenericInterface(typeof(IBusinessProxyController<>)) != null)
				{
					IBusinessProxyController<IBusinessImplementation> controller = this.Container.Resolve<IHttpController>(controllerDescriptor.ControllerName.ToLower()) as IBusinessProxyController<IBusinessImplementation>;
					if (controller != null)
					{
						var methods = controller.BusinessImplementation.GetType().GetMethodsCached(BindingFlags.Public | BindingFlags.Instance);
						var interfaces = controller.BusinessImplementation.GetType().GetInterfacesCached().SelectMany(i => i.GetMethodsCached(BindingFlags.Public | BindingFlags.Instance));
						var actionServiceMethods = (from am in methods
													join ai in interfaces on am.Name equals ai.Name
													select new { Method = am, InterfaceMethod = ai, ServiceMethods = am.GetCustomAttributesCached<ServiceMethod>(true).Union(ai.GetCustomAttributesCached<ServiceMethod>(true)), Routes = am.GetCustomAttributesCached<ServiceRoute>(true).Union(ai.GetCustomAttributesCached<ServiceRoute>(true)) }
													).Where(m => m.ServiceMethods.Count() > 0 && m.Routes.Any(r =>
														Configuration.Routes.ContainsKey(string.Format("{0}{1}", controllerDescriptor.ControllerName.ToLower(), r.Key))));
						List<HttpActionDescriptor> actionDescriptors = new List<HttpActionDescriptor>();
						foreach (var method in actionServiceMethods)
						{
							foreach (var httpMethod in method.ServiceMethods)
							{
								actionDescriptors.Add(new ServiceMethodHttpActionDescriptor(controllerDescriptor, method.Method, method.InterfaceMethod, httpMethod, this.Translator));
							}
						}
						mapping = actionDescriptors.ToLookup(m => m.ActionName);
					}
				}
				else
					mapping = base.GetActionMapping(controllerDescriptor);
				ServiceActionDescriptorMapping.TryAdd(controllerDescriptor.ControllerName.ToLower(), mapping);
			}
			return mapping;
		}

		public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
		{
			IBusinessProxyController<IBusinessImplementation> controller = controllerContext.Controller as IBusinessProxyController<IBusinessImplementation>;
			if (controller != null)
			{
				object actionName;
				if (controllerContext.RouteData.Values.TryGetValue("action", out actionName))
				{
					var actions = this.GetActionMapping(controllerContext.ControllerDescriptor);
					var corsRequestContext = controllerContext.Request.Properties.ContainsKey("MS_CorsRequestContextKey") ? controllerContext.Request.Properties["MS_CorsRequestContextKey"] as CorsRequestContext : null as CorsRequestContext;
					bool isOptions = (corsRequestContext != null && corsRequestContext.HttpMethod == "OPTIONS") || controllerContext.Request.Method == HttpMethod.Options;
					var actionsForRequest = actions[((string)actionName)].OfType<ServiceMethodHttpActionDescriptor>().Where(a => a.ServiceMethod.ToHttpMethod() == controllerContext.Request.Method || isOptions).ToArray();
					if(actionsForRequest.Length == 0)
						throw new HttpResponseException(controllerContext.Request.CreateErrorResponse(HttpStatusCode.NotFound, this.Translator.GetErrorMessage("ACTIONS_NOT_FOUND")));
					if(actionsForRequest.Length > 1 && !isOptions)
						throw new HttpResponseException(controllerContext.Request.CreateErrorResponse(HttpStatusCode.Ambiguous, this.Translator.GetErrorMessage("ACTIONS_MULTIPLE_FOUND")));
					var selectedAction = actionsForRequest.First();
				   
					if (isOptions)
						return selectedAction.CreateOptionsActionDescriptor();
					return selectedAction;
				}
			}
			return base.SelectAction(controllerContext);
		}
	}

	public class ServiceMethodHttpActionDescriptor : HttpActionDescriptor
	{
		public MethodInfo BusinessMethod { get; private set; }
		public ServiceMethod ServiceMethod { get; private set; }
		public MethodInfo InterfaceMethod { get; private set; }
		private ITranslationService Translator { get; set; }
		public bool IsOptions { get; private set; }
		public ServiceMethodHttpActionDescriptor(HttpControllerDescriptor controllerDescriptor, MethodInfo businessMethod, MethodInfo interfaceMethod, ServiceMethod serviceMethod, ITranslationService translator, bool isOptions = false)
			: base(controllerDescriptor)
		{
			this.BusinessMethod = businessMethod;
			this.ServiceMethod = serviceMethod;
			this.Translator = translator;
			this.InterfaceMethod = interfaceMethod;
			this.IsOptions = isOptions;
			if (serviceMethod != null)
			{
				this.SupportedHttpMethods.Clear();
				if (!isOptions)
					this.SupportedHttpMethods.Add(serviceMethod.ToHttpMethod());
				else
				{
					this.SupportedHttpMethods.Add(HttpMethod.Get);
					this.SupportedHttpMethods.Add(HttpMethod.Delete);
					this.SupportedHttpMethods.Add(HttpMethod.Head);
					this.SupportedHttpMethods.Add(HttpMethod.Options);
					this.SupportedHttpMethods.Add(HttpMethod.Post);
					this.SupportedHttpMethods.Add(HttpMethod.Put);
				}
			}
			this.ParamatersLazy = new Lazy<Collection<HttpParameterDescriptor>>(() => this.InitializateParameters(), LazyThreadSafetyMode.PublicationOnly);
			this.FiltersLazy = new Lazy<Collection<IFilter>>(() => this.InitializeFilters(), LazyThreadSafetyMode.PublicationOnly);
			this.ReturnTypeLazy = new Lazy<Type>(() => this.InitializeReturnType(), LazyThreadSafetyMode.ExecutionAndPublication);
		}

		public ServiceMethodHttpActionDescriptor CreateOptionsActionDescriptor()
		{
			return new ServiceMethodHttpActionDescriptor(this.ControllerDescriptor, this.BusinessMethod, this.InterfaceMethod, this.ServiceMethod, this.Translator, true);
		}

		public override async Task<object> ExecuteAsync(HttpControllerContext controllerContext, IDictionary<string, object> arguments, CancellationToken cancellationToken)
		{
			if (this.IsOptions)
				return controllerContext.Request.CreateResponse(HttpStatusCode.OK);
			IBusinessProxyController<IBusinessImplementation> controller = controllerContext.Controller as IBusinessProxyController<IBusinessImplementation>;
			
			if (controller != null)
			{
				try
				{
					if (controller.ModelState.IsValid)
					{
						var parmInfos = this.BusinessMethod.GetParametersCached();
						object[] parameters = new object[parmInfos.Length];
						for (var parmIndex = 0; parmIndex < parameters.Length; parmIndex++)
						{
							parameters[parmIndex] = Type.Missing;
						}
						int p = 0;
						foreach (var parameter in parmInfos)
						{
							object argument;
							if (arguments.TryGetValue(parameter.Name, out argument))
								parameters[p] = argument;
							else if (parameter.ParameterType == typeof(CancellationToken))
								parameters[p] = cancellationToken;
							p++;
						}
						var methodResult = this.BusinessMethod.Invoke(controller.BusinessImplementation, parameters);
						Task actionTask = methodResult as Task;
						if (actionTask != null)
						{
							await actionTask;
							var actionTaskType = actionTask.GetType();
							if (actionTaskType.IsGenericType)
							{
								var result = actionTaskType.GetPropertyCached("Result").GetValue(actionTask);
								if (result == null)
									return controllerContext.Request.CreateErrorResponse(HttpStatusCode.NoContent, this.Translator.GetErrorMessage("REQUEST_NULL"));
								else
									return controllerContext.Request.CreateResponse(HttpStatusCode.OK, result);
							}
							else
								return controllerContext.Request.CreateResponse(HttpStatusCode.OK);
						}
						if (this.BusinessMethod.ReturnType == typeof(void))
							return controllerContext.Request.CreateResponse(HttpStatusCode.OK);
						if(methodResult == null)
							return controllerContext.Request.CreateErrorResponse(HttpStatusCode.NoContent, this.Translator.GetErrorMessage("REQUEST_NULL"));
						return controllerContext.Request.CreateResponse(HttpStatusCode.OK, methodResult);
					}
					else
						throw new ValidationException(controller.ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage);
				}
				catch(GlobalizedAuthenticationException ex)
				{
					return controllerContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, this.Translator.GetErrorMessage(ex.ErrorCode), ex);
				}
				catch(GlobalizedAuthorizationException ex)
				{
					return controllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, this.Translator.GetErrorMessage(ex.ErrorCode), ex);
				}
				catch(GlobalizedValidationException ex)
				{
					return controllerContext.Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, this.Translator.GetErrorMessage(ex.ErrorCode), ex);
				}
				catch(GlobalizedException ex)
				{
					return controllerContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, this.Translator.GetErrorMessage(ex.ErrorCode), ex);
				}
				catch(ValidationException ex)
				{
					return controllerContext.Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, ex.Message, ex);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex)
				{
					return controllerContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
				}
			}
			return controllerContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, this.Translator.GetErrorMessage("SERVER_MISCONFIGURED"));
		}

		public override string ActionName
		{
			get { return BusinessMethod.Name; }
		}

		private readonly Lazy<Collection<HttpParameterDescriptor>> ParamatersLazy;

		public override Collection<HttpParameterDescriptor> GetParameters()
		{
			return this.ParamatersLazy.Value;
		}

		private Collection<HttpParameterDescriptor> InitializateParameters()
		{
			Collection<HttpParameterDescriptor> paramaters = new Collection<HttpParameterDescriptor>();
			var parms = this.BusinessMethod.GetParametersCached();
			var interfaceParameters = this.InterfaceMethod.GetParametersCached();
			for (var i = 0; i < parms.Length && i < interfaceParameters.Length; i++)
			{
				var p = parms[i];
				if (p.ParameterType != typeof(CancellationToken))
					paramaters.Add(new ServiceMethodParameterDescriptor(this, parms[i], interfaceParameters[i]));
			}
			return paramaters;
		}

		private readonly Lazy<Collection<IFilter>> FiltersLazy;

		public override Collection<IFilter> GetFilters()
		{
			return this.FiltersLazy.Value;
		}

		private Collection<IFilter> InitializeFilters()
		{
			var filters = base.GetFilters();
			if (this.ServiceMethod.AcceptSessionHeader)
				filters.Add(new SessionAttribute(this.Translator, this.ServiceMethod.RequireSessionHeader));
			return filters;
		}

		public override Collection<T> GetCustomAttributes<T>()
		{
			var attributes = base.GetCustomAttributes<T>();
			if (attributes == null)
				attributes = new Collection<T>();

			if (typeof(T) == typeof(Attribute) || typeof(T) == typeof(EnableCorsAttribute) || typeof(T) == typeof(ICorsPolicyProvider))
				attributes.Add(new EnableCorsAttribute("*", "*", "*") as T);
			return attributes;
		}

		public override Collection<T> GetCustomAttributes<T>(bool inherit)
		{
			var attributes = base.GetCustomAttributes<T>(inherit);
			if (attributes == null)
				attributes = new Collection<T>();

			if (typeof(T) == typeof(Attribute) || typeof(T) == typeof(EnableCorsAttribute) || typeof(T) == typeof(ICorsPolicyProvider))
				attributes.Add(new EnableCorsAttribute("*", "*", "*") as T);
			return attributes;
		}

		private readonly Lazy<Type> ReturnTypeLazy;

		private Type InitializeReturnType()
		{
			if (this.BusinessMethod.ReturnType.IsGenericType && this.BusinessMethod.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
				return this.BusinessMethod.ReturnType.GetGenericArguments().Single();
			if (this.BusinessMethod.ReturnType == typeof(Task))
				return typeof(void);
			return this.BusinessMethod.ReturnType;
		}

		public override Type ReturnType
		{
			get { return ReturnTypeLazy.Value; }
		}
	}

	public class ServiceMethodParameterDescriptor : HttpParameterDescriptor
	{
		public ParameterInfo Parameter { get; private set; }
		public ParameterInfo InterfaceParameter { get; private set; }
		public ServiceMethodParameterDescriptor(HttpActionDescriptor actionDescriptor, ParameterInfo parameter, ParameterInfo interfaceParameter)
			: base(actionDescriptor)
		{
			this.Parameter = parameter;
			this.InterfaceParameter = interfaceParameter;
		}

		public override string ParameterName
		{
			get { return Parameter.Name; }
		}

		public override Type ParameterType
		{
			get { return Parameter.ParameterType; }
		}

		public override Collection<T> GetCustomAttributes<T>()
		{
			List<Attribute> attributes = new List<Attribute>();
			if ((typeof(T) == typeof(Attribute) || typeof(T) == typeof(ParameterBindingAttribute) || typeof(T) == typeof(ModelBinderAttribute)) && (Parameter.GetCustomAttributeCached<JsonEncode>(true) != null || InterfaceParameter.GetCustomAttributeCached<JsonEncode>(true) != null))
				attributes.Add(new ModelBinderAttribute(typeof(JsonParameterModelBinder)));
			if ((typeof(T) == typeof(Attribute) || typeof(T) == typeof(ParameterBindingAttribute) || typeof(T) == typeof(FromBodyAttribute)) && (Parameter.GetCustomAttributeCached<Content>(true) != null || InterfaceParameter.GetCustomAttributeCached<Content>(true) != null))
				attributes.Add(new FromBodyAttribute());
			attributes.AddRange(Parameter.GetCustomAttributes(typeof(T), true).OfType<Attribute>());
			return new Collection<T>(attributes.Cast<T>().ToList());
		}
	}
}
