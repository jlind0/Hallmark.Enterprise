using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Business;
using System.Threading;

namespace HallData.Security
{
	public interface ISecurityImplementation : IUserSecurityImplemention
	{
		[GetMethod(acceptSessionHeader: false)]
		[ServiceRoute("SignOut", "{sessionId}/End/")]
		[Description("Signs out the user")]
		Task<bool> SignOut([Description("The sessiond id to sign out")]Guid sessionId, CancellationToken token = default(CancellationToken));
		
		[GetMethod(acceptSessionHeader: false)]
		[ServiceRoute("IsActiveSession", "{sessionId}/IsActive/")]
		[Description("Checks to see if session is active")]
		Task<bool> IsActiveSession(Guid sessionId, CancellationToken token = default(CancellationToken));
		
		Task<bool> MarkSessionActivity(CancellationToken token = default(CancellationToken));
		
		bool MarkSessionActivitySync();
		
		Task<SecurityUser> GetSignedInUser(CancellationToken token = default(CancellationToken));
		
		SecurityUser GetSignedInUserSync();
		
		bool IsActiveSessionSync(Guid sessionId);
        [GetMethod(acceptSessionHeader: false)]
        [ServiceRoute("SignInWindowsAuth", "create/")]
        [Description("Signs in the user with windows authentication")]
        Task<Guid?> SignInWindowsAuth(CancellationToken token = default(CancellationToken));
		
		Guid? SignInSync(string userName, string password = null, string token = null);
		
		Guid? SignInWindowsAuthSync();
		
		bool SignOutSync(Guid sessionId);
	}
}
