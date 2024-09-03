using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Session;
using System.Web;
using System.Threading;
using HallData.Exceptions;
using HallData.Security;

namespace HallData.Web.Session
{
	public class RepositorySession : ISession
	{
		protected ISessionRepository Repository { get; private set; }
		protected ISecurityTokenizer Tokenizer { get; private set; }
		protected IHttpContextWrapper HttpContext { get; private set; }
		public RepositorySession(ISessionRepository repository, ISecurityTokenizer tokenizer, IHttpContextWrapper httpContext)
		{
			this.Repository = repository;
			this.Tokenizer = tokenizer;
			this.HttpContext = httpContext;
		}
		public string IpAddress
		{
			get { return HttpContext.UserHostAddress; }
		}
		protected SessionState CurrentSessionSate
		{
			get
			{
					return HttpContext.Items["__sessionstate"] as SessionState;
			}
			set
			{
				HttpContext.Items["__sessionstate"] = value;
			}
		}

		protected SessionState ValidateSession(SessionState state)
		{
			this.CurrentSessionSate = state;
			if (state == null)
				return null;
			if(!state.IsActive)
			{
				this.CurrentSessionSate = null;
				this.CurrentSessionId = null;
				return null;
			}
			if (state.LastActivityDate.AddMinutes(20) < DateTime.UtcNow) //todo: grab from config
			{
				var logOut = Logout();
				this.CurrentSessionId = null;
				this.CurrentSessionSate = null;
				return null;
			}
			return state;
		}

		protected bool SessionUpdatedInContext
		{
			get { return HttpContext.Items["__SessionUpdatedInContext"] as bool? ?? false; }
			set { HttpContext.Items["__SessionUpdatedInContext"] = value; }
		}

		public async Task<SessionState> GetSession(CancellationToken token = default(CancellationToken))
		{
			if (this.CurrentSessionId == null)
				return null;
			if (this.CurrentSessionSate != null)
				return this.CurrentSessionSate;
			this.CurrentSessionSate = await this.Repository.GetSession(this.CurrentSessionId.Value, token);
			return this.CurrentSessionSate;
		}

		public SessionState GetSessionSync()
		{
			if (this.CurrentSessionId == null)
				return null;
			if (this.CurrentSessionSate != null)
				return this.CurrentSessionSate;
			this.CurrentSessionSate = this.Repository.GetSessionSync(this.CurrentSessionId.Value);
			return this.CurrentSessionSate;
		}

		public async Task<SessionState> GetUpdateSession(CancellationToken token = default(CancellationToken))
		{
			if (this.CurrentSessionId == null)
				return null;
			if (!this.SessionUpdatedInContext)
			{
				var session = ValidateSession(await this.Repository.GetUpdateSession(this.CurrentSessionId.Value, token));
				this.SessionUpdatedInContext = true;
				return session;
			}
			else
				return this.CurrentSessionSate;
		}

		public SessionState GetUpdateSessionSync()
		{
			if (this.CurrentSessionId == null)
				return null;
			if (!this.SessionUpdatedInContext)
			{
				var session = ValidateSession(this.Repository.GetUpdateSessionSync(this.CurrentSessionId.Value));
				this.SessionUpdatedInContext = true;
				return session;
			}
			else
				return this.CurrentSessionSate;
		}

		public async Task<SessionState> Login(CancellationToken token = default(CancellationToken))
		{
			//TODO: find out how to impersonate an anon user 
			if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
				throw new GlobalizedAuthorizationException("LOGIN_WINDOWSUSER_NOT_AUTH");
			return ProcessLoginSession(await this.Repository.LoginUserWindowAuthentication(GetWindowsIdentityName(), this.IpAddress, token));
		}

		protected SessionState ProcessLoginSession(SessionState session)
		{
			this.CurrentSessionSate = session;
			if (session == null)
			{
				this.CurrentSessionId = null;
				return null;
			}
			this.CurrentSessionId = session.Guid;
			return session;
		}
		public SessionState LoginSync()
		{
			 if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
				throw new GlobalizedAuthorizationException("LOGIN_WINDOWSUSER_NOT_AUTH");

			 return ProcessLoginSession(this.Repository.LoginUserWindowAuthenticationSync(GetWindowsIdentityName(), this.IpAddress));
		}
		protected string GetWindowsIdentityName()
		{
			string[] name = Thread.CurrentPrincipal.Identity.Name.Split('\\');
			if (name.Length == 1)
				return name[0];
			return name[1];
		}
		public SessionState LoginSync(string username, string password = null, string token = null)
		{
			if ((string.IsNullOrWhiteSpace(password) && string.IsNullOrWhiteSpace(token)) || (!string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(token)))
				throw new GlobalizedValidationException("LOGIN_PARAMETERS_INVALID");
			if (token != null)
			{
				var user = this.Repository.GetUserSync(username);
				if (user == null || token != this.Tokenizer.TokenizeUserNameOrganizationId(username, user.OrganizationGuid))
					throw new GlobalizedAuthenticationException("NOT_AUTH");
				var session = this.Repository.LoginUserWindowAuthenticationSync(username, this.IpAddress);
				return this.ProcessLoginSession(session);
			}
			else
			{
				var session = this.Repository.LoginUserSync(username, this.Tokenizer.Hash(password), this.IpAddress);
				return this.ProcessLoginSession(session);
			}
		}

		public async Task<SessionState> Login(string username, string password = null, string token = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (string.IsNullOrWhiteSpace(username) || (string.IsNullOrWhiteSpace(password) && string.IsNullOrWhiteSpace(token)) || (!string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(token)))
				throw new GlobalizedValidationException("LOGIN_PARAMETERS_INVALID");
			if (token != null)
			{
				var user = await this.Repository.GetUser(username, cancellationToken);
				if (user == null || !this.Tokenizer.VerifyToken(token, user.UserName, user.OrganizationGuid))
					throw new GlobalizedAuthenticationException("NOT_AUTH");
				var session = await this.Repository.LoginUserWindowAuthentication(username, this.IpAddress, cancellationToken);
				return this.ProcessLoginSession(session);
			}
			else
			{
				var session = this.Repository.LoginUserSync(username, this.Tokenizer.Hash(password), this.IpAddress);
				return this.ProcessLoginSession(session);
			}
		}

		public async Task<bool> Logout()
		{
			if (this.CurrentSessionId == null)
				throw new GlobalizedAuthenticationException("NOT_AUTH");
			var loggedOut = await this.Repository.Logout(this.CurrentSessionId.Value);
			if(loggedOut)
			{
				this.CurrentSessionId = null;
				this.CurrentSessionSate = null;
			}
			return loggedOut;
		}

		public bool LogoutSync()
		{
			if (this.CurrentSessionId == null)
				throw new GlobalizedAuthenticationException("NOT_AUTH");
			var loggedOut = this.Repository.LogoutSync(this.CurrentSessionId.Value);
			if(loggedOut)
			{
				this.CurrentSessionId = null;
				this.CurrentSessionSate = null;
			}
			return loggedOut;
		}

		public Guid? CurrentSessionId
		{
			get
			{
				return HttpContext.Items["__sessionid"] as Guid?;
			}
			set
			{
				HttpContext.Items["__sessionid"] = value;
			}
		}

		public async Task<bool> IsCurrentSessionActive(CancellationToken token = default(CancellationToken))
		{
			if (this.CurrentSessionId == null)
				return false;
			var session = ValidateSession(await this.GetUpdateSession(token));
			return session != null && session.IsActive;
		}

		public bool IsCurrentSessionActiveSync()
		{
			if (this.CurrentSessionId == null)
				return false;
			var session = ValidateSession(this.GetUpdateSessionSync());
			return session != null && session.IsActive;
		}
	}
}
