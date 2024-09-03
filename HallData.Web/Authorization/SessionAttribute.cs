using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net;
using HallData.Business;
using HallData.Web.Controllers;
using HallData.Exceptions;
using HallData.Translation;
using System.ComponentModel.DataAnnotations;

namespace HallData.Web.Authorization
{
    public class SessionAttribute : AuthorizeAttribute
    {
        public bool RequireSession { get; set; }
        protected ITranslationService Translation { get; private set; }
        public SessionAttribute(ITranslationService traslation, bool requireSession = true)
        {
            this.RequireSession = requireSession;
            this.Translation = traslation;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            IEnumerable<string> values;
            bool sessionIdProvided = false;
            if(actionContext.Request.Headers.TryGetValues("session.id", out values))
            {
                Guid sessionId;
                if (Guid.TryParse(values.Single(), out sessionId))
                {
                    sessionIdProvided = true;
                    IBusinessProxyController<IBusinessImplementation> controller = actionContext.ControllerContext.Controller as IBusinessProxyController<IBusinessImplementation>;
                    if (controller != null)
                    {
                        controller.BusinessImplementation.CurrentSessionId = sessionId;
                        try
                        {
                            if (this.RequireSession && !controller.BusinessImplementation.IsCurrentSessionActiveSync())
                                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Provided session is not active");
                        }
                        catch(GlobalizedAuthenticationException ex)
                        {
                            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, this.Translation.GetErrorMessage(ex.ErrorCode), ex);
                        }
                        catch(GlobalizedAuthorizationException ex)
                        {
                            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, this.Translation.GetErrorMessage(ex.ErrorCode), ex);
                        }
                        catch(GlobalizedValidationException ex)
                        {
                            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, this.Translation.GetErrorMessage(ex.ErrorCode), ex);
                        }
                        catch(GlobalizedException ex)
                        {
                            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, this.Translation.GetErrorMessage(ex.ErrorCode), ex);
                        }
                        catch(ValidationException ex)
                        {
                            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, ex.Message, ex);
                        }

                    }
                }
            }
            if(this.RequireSession && !sessionIdProvided)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Session Id must be a guid and provided");
            }
        }
    }
}
