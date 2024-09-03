using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HallData.Session;
using HallData.Security;
using HallData.Data;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data.Common;


namespace HallData.EMS.Data.Session
{
    public class SessionRepository : HallData.Repository.Repository, HallData.Session.ISessionRepository
    {
        public SessionRepository(Database db) : base(db) { }

        protected SessionState ReadSessionState(DbDataReader dr)
        {
            SessionState session = new SessionState();
            dr.MapField(session, s => s.ActivityCount);
            dr.MapField(session, s => s.CreatedDate);
            dr.MapField(session, s => s.Guid);
            dr.MapField(session, s => s.IsActive);
            dr.MapField(session, s => s.LastActivityDate);
            dr.MapField(session, s => s.User.FirstName);
            dr.MapField(session, s => s.User.LastName);
            dr.MapField(session, s => s.User.UserGuid);
            dr.MapField(session, s => s.User.UserName);
            dr.MapField(session, s => s.User.OrganizationGuid);
            return session;
        }
        protected SecurityUser ReadUser(DbDataReader dr)
        {
            SecurityUser user = new SecurityUser();
            dr.MapField(user, u => u.FirstName);
            dr.MapField(user, u => u.LastName);
            dr.MapField(user, u => u.OrganizationGuid);
            dr.MapField(user, u => u.UserGuid);
            dr.MapField(user, u => u.UserName);
            return user;
        }
        public async Task<SessionState> GetUpdateSession(Guid sessionId, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_selectupdate_sessions");
            cmd.AddParameter("sessionid", sessionId);
            List<SessionState> states = new List<SessionState>();
            await this.Execute(cmd, () => this.Database.ExecuteReaderAsync(cmd, token, dr => states.Add(ReadSessionState(dr))));
            return states.SingleOrDefault();
        }

        public async Task<SessionState> GetSession(Guid sessionId, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_select_sessions");
            cmd.AddParameter("sessionid", sessionId);
            List<SessionState> states = new List<SessionState>();
            await this.Execute(cmd, () => this.Database.ExecuteReaderAsync(cmd, token, dr => states.Add(ReadSessionState(dr))));
            return states.SingleOrDefault();
        }

        public SessionState GetUpdateSessionSync(Guid sessionId)
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_selectupdate_sessions");
            cmd.AddParameter("sessionid", sessionId);
            List<SessionState> states = new List<SessionState>();
            this.ExecuteSync(cmd, () => this.Database.ExecuteReader(cmd, dr => states.Add(ReadSessionState(dr))));
            return states.SingleOrDefault();
        }

        public SessionState GetSessionSync(Guid sessionId)
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_select_sessions");
            cmd.AddParameter("sessionid", sessionId);
            List<SessionState> states = new List<SessionState>();
            this.ExecuteSync(cmd, () => this.Database.ExecuteReader(cmd, dr => states.Add(ReadSessionState(dr))));
            return states.SingleOrDefault();
        }

        public async Task<SessionState> LoginUserWindowAuthentication(string username, string ipAddress, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_login_windowsauth");
            cmd.AddParameter("username", username);
            cmd.AddParameter("ipaddress", ipAddress);
            List<SessionState> states = new List<SessionState>();
            await this.Execute(cmd, () => this.Database.ExecuteReaderAsync(cmd, token, dr => states.Add(ReadSessionState(dr))));
            return states.SingleOrDefault();
        }

        public SessionState LoginUserWindowAuthenticationSync(string username, string ipAddress)
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_login_windowsauth");
            cmd.AddParameter("username", username);
            cmd.AddParameter("ipaddress", ipAddress);
            List<SessionState> states = new List<SessionState>();
            this.ExecuteSync(cmd, () => this.Database.ExecuteReader(cmd, dr => states.Add(ReadSessionState(dr))));
            return states.SingleOrDefault();
        }

        public async Task<bool> Logout(Guid sessionId, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_logout");
            cmd.AddParameter("sessionid", sessionId);
            return (bool)await this.Execute(cmd, () => this.Database.ExecuteScalarAsync(cmd, token));
        }

        public bool LogoutSync(Guid sessionId)
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_logout");
            cmd.AddParameter("sessionid", sessionId);
            return (bool)this.ExecuteSync(cmd, () => this.Database.ExecuteScalar(cmd));
        }


        public SessionState LoginUserSync(string username, string password, string ipAddress)
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_login");
            cmd.AddParameter("username", username);
            cmd.AddParameter("ipaddress", ipAddress);
            cmd.AddParameter("password", password);
            List<SessionState> states = new List<SessionState>();
            this.ExecuteSync(cmd, () => this.Database.ExecuteReader(cmd, dr => states.Add(ReadSessionState(dr))));
            return states.SingleOrDefault();
        }

        public async Task<SessionState> LoginUser(string username, string password, string ipAddress, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_login");
            cmd.AddParameter("username", username);
            cmd.AddParameter("ipaddress", ipAddress);
            cmd.AddParameter("password", password);
            List<SessionState> states = new List<SessionState>();
            await Execute(cmd, () => this.Database.ExecuteReaderAsync(cmd, token, dr => states.Add(ReadSessionState(dr))));
            return states.SingleOrDefault();
        }

        public async Task<SecurityUser> GetUser(string username, CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_select_securityusers");
            cmd.AddParameter("username", username);
            List<SecurityUser> users = new List<SecurityUser>();
            await Execute(cmd, () => this.Database.ExecuteReaderAsync(cmd, token, dr => users.Add(ReadUser(dr))));
            return users.SingleOrDefault();
        }

        public SecurityUser GetUserSync(string username)
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_select_securityusers");
            cmd.AddParameter("username", username);
            List<SecurityUser> users = new List<SecurityUser>();
            ExecuteSync(cmd, () => this.Database.ExecuteReader(cmd, dr => users.Add(ReadUser(dr))));
            return users.SingleOrDefault();
        }
    }
}
