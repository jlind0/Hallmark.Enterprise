using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Business;
using System.Threading;
using HallData.Security;

namespace HallData.Session
{
	public interface ISession : IBusinessImplementation
	{
		Task<SessionState> GetSession(CancellationToken token = default(CancellationToken));
		SessionState GetSessionSync();
		Task<SessionState> GetUpdateSession(CancellationToken token = default(CancellationToken));
		SessionState GetUpdateSessionSync();
		Task<SessionState> Login(CancellationToken token = default(CancellationToken));
		SessionState LoginSync();
		SessionState LoginSync(string username, string password = null, string token = null);
		Task<SessionState> Login(string username, string password = null, string token = null, CancellationToken cancellationToken = default(CancellationToken));
		Task<bool> Logout();
		bool LogoutSync();
	}

	public class SessionState
	{
		public SecurityUser User { get; set; }
		public int ActivityCount { get; set; }
		public DateTime LastActivityDate { get; set; }
		public DateTime CreatedDate { get; set; }
		public Guid Guid { get; set; }
		public bool IsActive { get; set; }
	}
}
