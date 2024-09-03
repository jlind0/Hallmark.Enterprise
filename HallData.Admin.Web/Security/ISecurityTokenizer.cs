using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Security
{
	interface ISecurityTokenizer
	{
		string TokenizeUserNameOrganizationId(string userName, Guid organizationId);
		bool VerifyToken(string token, string userName, Guid organizationId);
		string Hash(string password);
	}
}
