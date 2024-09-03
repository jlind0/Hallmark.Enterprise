using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Repository;
using HallData.ApplicationViews;
using System.Threading;
using Newtonsoft.Json;
using HallData.Security;

namespace HallData.Session
{
    public interface ISessionRepository : IRepository
    {
        Task<SessionState> GetUpdateSession(Guid sessionId, CancellationToken token = default(CancellationToken));
        Task<SessionState> GetSession(Guid sessionId, CancellationToken token = default(CancellationToken));
        SessionState GetUpdateSessionSync(Guid sessionId);
        SessionState GetSessionSync(Guid sessionId);
        Task<SessionState> LoginUserWindowAuthentication(string username, string ipAddress, CancellationToken token = default(CancellationToken));
        SessionState LoginUserWindowAuthenticationSync(string username, string ipAddress);
        SessionState LoginUserSync(string username, string password, string ipAddress);
        Task<SessionState> LoginUser(string username, string password, string ipAddress, CancellationToken token = default(CancellationToken));
        Task<SecurityUser> GetUser(string username, CancellationToken token = default(CancellationToken));
        SecurityUser GetUserSync(string username);
        Task<bool> Logout(Guid sessionId, CancellationToken token = default(CancellationToken));
        bool LogoutSync(Guid sessionId);
    }
   
}
