using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HallData.Business;

namespace HallData.Security
{
    public interface IUserSecurityImplemention : IBusinessImplementation
    {
        [GetMethod(acceptSessionHeader: false)]
        [ServiceRoute("SignIn", "{userName}/Sessions/Create/")]
        [Description("Signs In a user")]
        Task<Guid?> SignIn([Description("User name to sign in")]string userName, [Description("Password to sign in, must be supplied if token is not")]string password = null, [Description("Token to sign in, must be suplied if password is not")]string token = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
