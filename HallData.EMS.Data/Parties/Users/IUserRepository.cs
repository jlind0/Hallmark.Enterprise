using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HallData.EMS.ApplicationViews.Results;
using HallData.Repository;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Data
{
    public interface IReadOnlyUserRepository : IReadOnlyPersonRepository<UserResult>
    {
        Task<QueryResult<UserResult>> GetByUserName(string username, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
        Task<QueryResult<JObject>> GetByUserNameView(string username, string viewname = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }
    public interface IUserRepository : IPersonRepository<UserResult, UserForAdd, UserForUpdate>, IReadOnlyUserRepository
    {
        Task<bool> ChangePassword(string username, string currentPassword, string newPassword, Guid? userId = null, CancellationToken token = default(CancellationToken));
        Task<bool> ChangePasswordAdmin(Guid targetUserId, string password, Guid userId, CancellationToken token = default(CancellationToken));
        Task<ChangeStatusResult> ChangeStatusUserRelationship(Guid targetUserId, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }
}
