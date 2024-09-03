using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Security;
using System.Threading;
using HallData.Exceptions;
using HallData.Utilities;

namespace HallData.Session.Mocks
{
    public class MockSessionRepository : ISessionRepository
    {
        private Dictionary<string, SecurityUser> Users { get; set; }
        private Dictionary<Guid, SessionState> Sessions { get; set; }
        public MockSessionRepository(IEnumerable<SecurityUser> users)
        {
            this.Users = users.ToDictionary(u => u.UserName);
            this.Sessions = new Dictionary<Guid, SessionState>();
        }
        public Task<SessionState> GetUpdateSession(Guid sessionId, CancellationToken token = default(CancellationToken))
        {
            SessionState state;
            SessionState rtnState = null;
            if (this.Sessions.TryGetValue(sessionId, out state))
            {
                rtnState = state.CreateRelatedInstance<SessionState>();
                if (state.IsActive)
                {
                    state.ActivityCount += 1;
                    state.LastActivityDate = DateTime.UtcNow;
                }
            }
            else
                throw new GlobalizedAuthenticationException("NOT_AUTH");
            return Task.FromResult(state);
        }

        public Task<SessionState> GetSession(Guid sessionId, CancellationToken token = default(CancellationToken))
        {
            SessionState state;
            if (!this.Sessions.TryGetValue(sessionId, out state))
                throw new GlobalizedAuthenticationException("NOT_AUTH");
            return Task.FromResult(state);
        }

        public SessionState GetUpdateSessionSync(Guid sessionId)
        {
            SessionState state;
            SessionState rtnState = null;
            if (this.Sessions.TryGetValue(sessionId, out state))
            {
                rtnState = state.CreateRelatedInstance<SessionState>();
                if (state.IsActive)
                {
                    state.ActivityCount += 1;
                    state.LastActivityDate = DateTime.UtcNow;
                }
            }
            else
                throw new GlobalizedAuthenticationException("NOT_AUTH");
            return rtnState;
        }

        public SessionState GetSessionSync(Guid sessionId)
        {
            SessionState state;
            if (!this.Sessions.TryGetValue(sessionId, out state))
                throw new GlobalizedAuthenticationException("NOT_AUTH");
            return state;
        }

        public Task<SessionState> LoginUserWindowAuthentication(string username, string ipAddress, CancellationToken token = default(CancellationToken))
        {
            SecurityUser user;
            if (this.Users.TryGetValue(username, out user))
            {
                SessionState session = new SessionState() { ActivityCount = 0, CreatedDate = DateTime.UtcNow, Guid = Guid.NewGuid(), IsActive = true, LastActivityDate = DateTime.UtcNow, User = user };
                this.Sessions.Add(session.Guid, session);
                return Task.FromResult(session);
            }
            throw new GlobalizedAuthenticationException("LOGIN_USER_NOTEXIST");
        }

        public SessionState LoginUserWindowAuthenticationSync(string username, string ipAddress)
        {
            SecurityUser user;
            if (this.Users.TryGetValue(username, out user))
            {
                SessionState session = new SessionState() { ActivityCount = 0, CreatedDate = DateTime.UtcNow, Guid = Guid.NewGuid(), IsActive = true, LastActivityDate = DateTime.UtcNow, User = user };
                this.Sessions.Add(session.Guid, session);
                return session;
            }
            throw new GlobalizedAuthenticationException("LOGIN_USER_NOTEXIST");
        }

        public SessionState LoginUserSync(string username, string password, string ipAddress)
        {
            SecurityUser user;
            if (this.Users.TryGetValue(username, out user))
            {
                SessionState session = new SessionState() { ActivityCount = 0, CreatedDate = DateTime.UtcNow, Guid = Guid.NewGuid(), IsActive = true, LastActivityDate = DateTime.UtcNow, User = user };
                this.Sessions.Add(session.Guid, session);
                return session;
            }
            throw new GlobalizedAuthenticationException("NOT_AUTH");
        }

        public Task<SessionState> LoginUser(string username, string password, string ipAddress, CancellationToken token = default(CancellationToken))
        {
            SecurityUser user;
            if (this.Users.TryGetValue(username, out user))
            {
                SessionState session = new SessionState() { ActivityCount = 0, CreatedDate = DateTime.UtcNow, Guid = Guid.NewGuid(), IsActive = true, LastActivityDate = DateTime.UtcNow, User = user };
                this.Sessions.Add(session.Guid, session);
                return Task.FromResult(session);
            }
            throw new GlobalizedAuthenticationException("NOT_AUTH");
        }

        public Task<SecurityUser> GetUser(string username, CancellationToken token = default(CancellationToken))
        {
            SecurityUser user;
            this.Users.TryGetValue(username, out user);
            return Task.FromResult(user);
        }

        public SecurityUser GetUserSync(string username)
        {
            SecurityUser user;
            this.Users.TryGetValue(username, out user);
            return user;
        }

        public Task<bool> Logout(Guid sessionId, CancellationToken token = default(CancellationToken))
        {
            SessionState session;
            if(this.Sessions.TryGetValue(sessionId, out session) && session.IsActive)
            {
                session.IsActive = false;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public bool LogoutSync(Guid sessionId)
        {
            SessionState session;
            if (this.Sessions.TryGetValue(sessionId, out session) && session.IsActive)
            {
                session.IsActive = false;
                return true;
            }
            return false;
        }
    }
}
