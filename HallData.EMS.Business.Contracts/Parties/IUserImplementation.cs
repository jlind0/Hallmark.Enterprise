using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.Security;
using HallData.EMS.Models;
using HallData.Business;
using System.Threading;
using HallData.Validation;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Business
{
    public interface IReadOnlyUserImplementation : IReadOnlyPersonImplementation<Guid, UserResult>
    {
        [GetMethod]
        [ServiceRoute("GetByUserName", "UserName/{username}/")]
        [ServiceRoute("GetByUserNameTyped", "UserName/{username}/TypedView/{viewName}/")]
        [ServiceRoute("GetByUserNameTypedDefault", "UserName/{username}/TypedView/")]
        Task<QueryResult<UserResult>> GetByUserName(string username, string viewName = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetByUserNameView", "UserName/{username}/View/{viewname}/")]
        [ServiceRoute("GetByUserNameViewDefault", "UserName/{username}/View/")]
        Task<QueryResult<JObject>> GetByUserNameView(string username, string viewname = null, CancellationToken token = default(CancellationToken));
    }
    [Service("users")]
    public interface IUserImplementation : IPersonImplementation<Guid, UserResult, UserForAdd, UserForUpdate>, IReadOnlyUserImplementation, IUserSecurityImplemention
    {
        [UpdateMethod(requireSessionHeader: false)]
        [ServiceRoute("ChangePassword", "{username}/Password/")]
        Task<ChangePasswordResult> ChangePassword(string username, [Content]ChangePasswordParameters parameters, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangePasswordAdmin", "{userId}/Password/Admin/")]
        Task<ChangePasswordResult> ChangePasswordAdmin(Guid userId, [Content][GlobalizedRequired("CHANGE_PASSWORD_ADMIN_PASSWORD_REQUIRED")] string password, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusUserRelationship", "{userId}/UserRelationship/Status/{statusTypeName}/")]
        [ServiceRoute("ChangeStatusUserRelationshipTyped", "{userId}/UserRelationship/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusUserRelationshipTypedDefault", "{userId}/UserRelationship/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<UserResult>> ChangeStatusUserRelationship(Guid userId, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusUserRelationshipForce", "{userId}/UserRelationship/Status/{statusTypeName}/Force/")]
        [ServiceRoute("ChangeStatusUserRelationshipForceTyped", "{userId}/UserRelationship/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusUserRelationshipForceTypedDefault", "{userId}/UserRelationship/Status/{statusTypeName}/Force/TypedView/")]
        Task<ChangeStatusQueryResult<UserResult>> ChangeStatusUserRelationshipForce(Guid userId, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
    }
}
