using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Session;
using System.Threading;
using System.Security.Authentication;
using HallData.Business;
using HallData.Repository;
using HallData.Exceptions;

namespace HallData.Security
{
    public class SessionSecurityImplementation : ISecurityImplementation
    {
        protected ISession Session { get; private set; }
        public SessionSecurityImplementation(ISession session)
        {
            this.Session = session;
        }
        public async Task<Guid?> SignIn(string userName, string password = null, string token = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var session = await this.Session.Login(userName, password, token, cancellationToken);
            if (session == null)
                throw new GlobalizedAuthenticationException();
            return session.Guid;
        }


        public Task<bool> SignOut(Guid sessionId, CancellationToken token = default(CancellationToken))
        {
            this.CurrentSessionId = sessionId;
            return this.Session.Logout();
        }

        public async Task<bool> IsActiveSession(Guid sessionId, CancellationToken token = default(CancellationToken))
        {
            this.CurrentSessionId = sessionId;
            var session = await this.Session.GetSession(token);
            return session != null && session.IsActive;
        }

        public bool IsActiveSessionSync(Guid sessionId)
        {
            this.CurrentSessionId = sessionId;
            var session = this.Session.GetSessionSync();
            return session != null && session.IsActive;
        }

        public Guid? SignInSync(string userName, string password = null, string token = null)
        {
            var session = this.Session.LoginSync(userName, password, token);
            if (session == null)
                throw new GlobalizedAuthenticationException();
            return session.Guid;
        }

        public Guid? SignInWindowsAuthSync()
        {
            var session = this.Session.LoginSync();
            if (session == null)
                throw new GlobalizedAuthenticationException();
            return session.Guid;
        }

        public bool SignOutSync(Guid sessionId)
        {
            this.CurrentSessionId = sessionId;
            return this.Session.LogoutSync();
        }

        public Guid? CurrentSessionId
        {
            get
            {
                return this.Session.CurrentSessionId;
            }
            set
            {
                this.Session.CurrentSessionId = value;
            }
        }


        public async Task<SecurityUser> GetSignedInUser(CancellationToken token = default(CancellationToken))
        {
            var session = await this.Session.GetSession(token);
            if (session == null)
                return null;
            return session.User;
        }


        public SecurityUser GetSignedInUserSync()
        {
            var session = this.Session.GetSessionSync();
            if (session == null)
                return null;
            return session.User;
        }


        public async Task<bool> MarkSessionActivity(CancellationToken token = default(CancellationToken))
        {
            var session = await this.Session.GetUpdateSession(token);
            return session != null && session.IsActive;
        }

        public bool MarkSessionActivitySync()
        {
            var session = this.Session.GetUpdateSessionSync();
            return session != null && session.IsActive;
        }


        public Task<bool> IsCurrentSessionActive(CancellationToken token = default(CancellationToken))
        {
            return this.Session.IsCurrentSessionActive(token);
        }

        public bool IsCurrentSessionActiveSync()
        {
            return this.Session.IsCurrentSessionActiveSync();
        }


        public async Task<Guid?> SignInWindowsAuth(CancellationToken token = default(CancellationToken))
        {
            var session = await this.Session.Login(token);
            if (session == null)
                throw new GlobalizedAuthenticationException();
            return session.Guid;
        }
    }
}
