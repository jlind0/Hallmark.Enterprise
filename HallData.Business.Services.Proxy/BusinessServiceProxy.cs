using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Business;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Newtonsoft.Json;
using System.Web;
using System.Threading;
using HallData.Web;

namespace HallData.Business.Services.Proxy
{
    public class BusinessServiceProxy<TBusinessImplmention>
        where TBusinessImplmention: IBusinessImplementation
    {
        public string ServiceUrl { get; private set; }
        public BusinessServiceProxy(string serviceUrlBase, string serviceName = null)
        {
            if(string.IsNullOrWhiteSpace(serviceName))
            {
                var serviceAttr = typeof(TBusinessImplmention).GetCustomAttribute<ServiceAttribute>(true);
                if (serviceAttr == null)
                    throw new ArgumentException("serviceName not provided and Business Implemention does not have a ServiceAttribute");
                serviceName = serviceAttr.Name;
            }
            if (!string.IsNullOrWhiteSpace(serviceName))
                serviceUrlBase = string.Format("{0}{1}/", serviceUrlBase, serviceName);
            this.ServiceUrl = serviceUrlBase;
        }
        public async Task<T> Execute<T>(Expression<Func<TBusinessImplmention, Task<T>>> methodCall, Guid? sessionId = null)
        {
            MethodCallExpression me = methodCall.Body as MethodCallExpression;
            if (me == null)
                throw new ArgumentException("methodCall is not a MethodCallExpression");
            var message = await Execute(me, sessionId);
            if (!message.IsSuccessStatusCode)
                throw new InvalidOperationException(await message.Content.ReadAsStringAsync());
            else
                return JsonConvert.DeserializeObject<T>(await message.Content.ReadAsStringAsync());
        }
        public Task<HttpResponseMessage> ExecuteMessage(Expression<Action<TBusinessImplmention>> methodCall, Guid? sessionId = null)
        {
            MethodCallExpression me = methodCall.Body as MethodCallExpression;
            if (me == null)
                throw new ArgumentException("methodCall is not a MethodCallExpression");
            return Execute(me, sessionId);
        }
        protected async Task<HttpResponseMessage> Execute(MethodCallExpression me, Guid? sessionId)
        {
            var serviceMethod = me.Method.GetCustomAttribute<ServiceMethod>(true);
            if (serviceMethod == null)
                throw new ArgumentException("the method called is not a service method");
            if (serviceMethod.RequireSessionHeader && sessionId == null)
                throw new ArgumentException("the method requires a session id");
            var routes = me.Method.GetCustomAttributes<ServiceRoute>(true).Select(r => new Route() { RoutePath = r.RoutePath }).ToArray();
            var parameters = me.Method.GetParameters();
            var arguments = me.Arguments.Select(a => new { Value = GetValue(a) }).ToArray();
            foreach (var route in routes)
            {
                for (var i = 0; i < parameters.Length && i < arguments.Length; i++)
                {
                    var parm = parameters[i];
                    if (parm.ParameterType != typeof(CancellationToken) && parm.GetCustomAttribute<Content>(true) == null)
                    {
                        var arg = arguments[i];
                        var value = arg.Value;
                        if (value != null)
                        {
                            if (parm.GetCustomAttribute<JsonEncode>(true) != null)
                                value = JsonConvert.SerializeObject(value);
                            value = HttpUtility.UrlEncode(value.ToString());
                            if (route.RoutePath.Contains("{" + parm.Name + "}"))
                                route.RoutePath = route.RoutePath.Replace("{" + parm.Name + "}", value.ToString());
                            else
                            {
                                if (!route.RoutePath.Contains("?"))
                                    route.RoutePath += "?";
                                if (!route.RoutePath.EndsWith("?"))
                                    route.RoutePath += "&";
                                route.RoutePath += parm.Name + "=" + value.ToString();
                            }
                        }
                    }
                }
            }
            var selectedRoute = routes.FirstOrDefault(f => f.IsFilled);
            if (selectedRoute != null)
            {
                using (var client = new HttpClient())
                {

                    var url = this.ServiceUrl + selectedRoute.RoutePath;
                    object data = null;
                    CancellationToken token = default(CancellationToken);
                    for (var i = 0; i < parameters.Length && i < arguments.Length; i++)
                    {
                        var parm = parameters[i];
                        var arg = arguments[i];
                        if (arg.Value != null)
                        {
                            if (parm.ParameterType == typeof(CancellationToken))
                                token = (CancellationToken)arg.Value;
                            else if (parm.GetCustomAttribute<Content>(true) != null)
                                data = arg.Value;
                        }
                    }
                    HttpContent content = data != null ? new StringContent(JsonConvert.SerializeObject(data)) : null as HttpContent;
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(url),
                        Method = serviceMethod.ToHttpMethod()
                    };
                    if (content != null)
                    {
                        request.Content = content;
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json; charset=utf-8");
                    }
                    if (serviceMethod.AcceptSessionHeader && sessionId != null)
                        request.Headers.Add("session.id", sessionId.ToString());
                    var message = await client.SendAsync(request, token);
                    return message;
                }
            }
            throw new ArgumentException("Could not match route");
        }
        protected static object GetValue(Expression expression)
        {
            while (expression.CanReduce)
                expression = expression.Reduce();
            var argMember = Expression.Convert(expression, typeof(object));
            var getterLamda = Expression.Lambda<Func<object>>(argMember);
            var getter = getterLamda.Compile();
            return getter();
        }
        protected class Route
        {
            public string RoutePath { get; set; }
            public bool IsFilled { get { return !RoutePath.Contains("{"); } }
        }
    }
    
}
