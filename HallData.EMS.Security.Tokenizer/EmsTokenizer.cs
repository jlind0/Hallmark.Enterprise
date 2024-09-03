using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Security;

namespace HallData.EMS.Security.Tokenizer
{
	public class EmsTokenizer : ISecurityTokenizer
	{
		protected byte[] Salt { get; private set; }

		public EmsTokenizer()
		{
			this.Salt = null;
		}

		public EmsTokenizer(byte[] salt = null)
		{
			this.Salt = salt;
		}

		public virtual string TokenizeUserNameOrganizationId(string userName, Guid organizationId)
		{
			return PasswordHash.CreateHash(userName + organizationId.ToString());
		}

		public virtual bool VerifyToken(string token, string userName, Guid organizationId)
		{
			return PasswordHash.ValidatePassword(userName + organizationId.ToString(), token);
		}

		public virtual string Hash(string password)
		{
			return PasswordHash.CreateHash(password, this.Salt);
		}
	}
}
