using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.WebHost;
using System.Web.SessionState;
using System.Web.Routing;
using System.Web;

namespace HallData.Web.Controllers
{
    public class SessionControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public SessionControllerHandler(RouteData routeData) : base(routeData) { }
    }
    public class SessionRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SessionControllerHandler(requestContext.RouteData);
        }
    }
}
