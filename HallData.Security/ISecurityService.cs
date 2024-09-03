using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Security
{
    public interface ISecurityService
    {
        Task<string[]> GetAllRoles(Guid? userID = null);
        string[] GetRolesForSignedInUser();
    }
}
