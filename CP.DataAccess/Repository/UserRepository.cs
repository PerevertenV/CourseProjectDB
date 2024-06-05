using CP.DataAccess.Data;
using CP.DataAccess.Repository.IRepository;
using CP.Models;
using System.Security.Cryptography;
using System.Text;

namespace CP.DataAccess.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

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

        public void Update(User obj)
        {
            _db.Users.Update(obj);
        }
    }
}
