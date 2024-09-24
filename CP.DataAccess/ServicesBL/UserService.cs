using CP.DataAccess.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.ServicesBL
{
    public class UserService: IUserService
    {
		public string PasswordHashCoder(string password)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(password);
			byte[] encrypted = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
			return Convert.ToBase64String(encrypted);
		}

		public string DecryptString(string encryptedText)
		{
			byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
			byte[] decrypted = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
			return Encoding.Unicode.GetString(decrypted);
		}
	}
}
